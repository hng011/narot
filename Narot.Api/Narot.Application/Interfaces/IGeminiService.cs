using Narot.Domain.Entities;

namespace Narot.Application.Interfaces
{
    public interface IGeminiService
    {
        Task<string> GenerateReadingsAsync(List<string> questions, List<NarotCard> cards);
    }
}