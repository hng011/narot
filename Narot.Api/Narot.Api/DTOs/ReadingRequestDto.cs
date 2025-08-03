namespace Narot.Api.DTOs
{
    public record ReadingRequestDto 
    (
        List<string> Questions,
        string DeckName
    );
}
