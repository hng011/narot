namespace Narot.Domain.Entities
{
    public class NarotReading
    {
        public List<string>? Questions { get; set; }
        public List<NarotCard>? DrawnCards { get; set; }
        public string? Readings { get; set; }        
    }
}
