namespace DirectoryService.Core.Dto;

public class UserHeartbeatDto
{
    public bool? Connected { get; set; }
    public string? Path { get; set; }
    public string? DomainId { get; set; }
    public string? PlaceId { get; set; }
    public string? NetworkAddress { get; set; }
    public string? NodeId { get; set; }
    public string? Availability { get; set; }
}