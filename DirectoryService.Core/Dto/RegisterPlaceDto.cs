using System.Net;

namespace DirectoryService.Core.Dto;

public class RegisterPlaceDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string DomainId { get; set; }
    public string? Path { get; set; }
    public IPAddress? CreatorIp { get; set; }
}