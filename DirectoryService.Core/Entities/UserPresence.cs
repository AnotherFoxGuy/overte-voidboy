namespace DirectoryService.Core.Entities;

public class UserPresence : BaseEntity
{
    public bool? Connected { get; set; }
    public string? DomainId { get; set; }
    public string? PlaceId { get; set; }
    public string? NetworkAddress { get; set; }
    public string? PublicKey { get; set; }
    public string? Path { get; set; }
    public DateTime LastHeartbeat { get; set; }
    public string? NodeId { get; set; }
    public string? Availability { get; set; }
}