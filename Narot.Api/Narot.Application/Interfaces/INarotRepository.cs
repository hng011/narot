using Narot.Domain.Entities;

namespace Narot.Application.Interfaces
{
    public interface INarotRepository
    {
        Task<List<string>> GetDeckNamesAsync();
        Task<NarotDeck> GetDeckAsync(string deckName);
    }
}
