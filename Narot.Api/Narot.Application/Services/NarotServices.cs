using Narot.Application.Interfaces;
using Narot.Domain.Entities;

namespace Narot.Application.Services
{
    public class NarotServices : INarotService
    {
        private readonly INarotRepository _narotRepository;
        private readonly IGeminiService _geminiService;
 
        public NarotServices(INarotRepository narotRepository, IGeminiService geminiService)
        {
            _narotRepository = narotRepository;
            _geminiService = geminiService;
        }

        public async Task<NarotDeck> GetDeckAsync(string deckName)
        {
            return await _narotRepository.GetDeckAsync(deckName);
        }

        public async Task<List<string>> GetDeckNamesAsync()
        {
            return await _narotRepository.GetDeckNamesAsync();
        }

        public async Task<NarotReading> GetNarotReadingAsync(List<string> questions, string deckName)
        {
            var deck = await _narotRepository.GetDeckAsync(deckName);

            if (deck == null) {
                throw new Exception("Deck not found");
            }

            if (questions.Count > deck.Cards.Count)
            {
                throw new InvalidOperationException("Cannot draw more cards than are in the deck.");
            }

            var rd = new Random();
            var drawnCards = new List<NarotCard>();
            var drawnIndexes = new HashSet<int>();

            foreach (var q in questions) 
            {            
                int cardIndex;
                do
                {
                    cardIndex = rd.Next(deck.Cards.Count);
                } while (drawnIndexes.Contains(cardIndex));

                drawnIndexes.Add(cardIndex);
                drawnCards.Add(deck.Cards[cardIndex]);
            }

            var readings = await _geminiService.GenerateReadingsAsync(questions, drawnCards);

            return new NarotReading
            {
                Questions = questions,
                DrawnCards = drawnCards,
                Readings = readings,
            };
        
        }
    }
}
