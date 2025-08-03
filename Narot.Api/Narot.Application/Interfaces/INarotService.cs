using Narot.Domain.Entities;

namespace Narot.Application.Interfaces
{
    public interface INarotService
    {
        Task<List<string>> GetDeckNamesAsync();
        Task<NarotDeck> GetDeckAsync(string deckName);
        Task<NarotReading> GetNarotReadingAsync(
                List<string> questions,
                string deckName
            );
    }
}
