using Narot.Application.Interfaces;
using Narot.Domain.Entities;
using System.Text;
using System.Text.Json;

namespace Narot.Infrastructure.Services
{
    public class GeminiService : IGeminiService
    {
        private readonly string _apiKey;
        private readonly string _endpoint;

        public GeminiService(string apiKey, string endpoint)
        {
            _apiKey = apiKey;
            _endpoint = endpoint;
        }

        public async Task<string> GenerateReadingsAsync(List<string> questions, List<NarotCard> cards)
        {
            var cardNames = string.Join("\n", cards.Select((c, i) => $"{i + 1}: {c.Name}"));
            var questionsList = string.Join("\n", questions.Select((q, i) => $"{i + 1}: {q}"));
            var prompt = $"I have drawn the following tarot cards: \n{cardNames}."
                + $"\nPlease provide a reading for the following questions: \n{questionsList}"
                + $"\nFor each question, provide a thoughtful and insightful answer that relates to the drawn cards.";

            var body = new Dictionary<string, object>
            {
                ["contents"] = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        ["role"] = "model",
                        ["parts"] = new List<Dictionary<string, string>>
                        {
                            new Dictionary<string, string>
                            {
                                ["text"] = "You are a professional tarot card reader." +
                                "Your job is to clearly and practically explain the meaning of tarot cards." +
                                " Always provide interpretations that make sense, using straightforward language that anyone can understand." +
                                " Focus on clarity, insight, and how the card may apply to the user's current situation if provided." +
                                " Avoid vague or overly mystical language unless requested."
                            }
                        }
                    },
                    new Dictionary<string, object>
                    {
                        ["role"] = "user",
                        ["parts"] = new List<Dictionary<string, string>>
                        {
                            new Dictionary<string, string>
                            {
                                ["text"] = prompt
                            }
                        }
                    }

                }
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {  
                HttpRequestMessage req = new(HttpMethod.Post, _endpoint);
                req.Content = content;
                req.Headers.Add("X-goog-api-key", _apiKey);

                var client = new HttpClient();
                var resp = await client.SendAsync(req);
                string res = await resp.Content.ReadAsStringAsync();

                JsonDocument resJson = JsonDocument.Parse(res);
                string? finalRes = resJson.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return finalRes ?? string.Empty;
            }
            catch (Exception E)
            {
                return E.Message;
            }

        }
    }
}
