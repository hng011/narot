namespace Narot.Domain.Entities
{
    public class NarotDeck
    {
        public string Name { get; set; } = null!;
        public List<NarotCard>? Cards { get; set; }
    }
}
