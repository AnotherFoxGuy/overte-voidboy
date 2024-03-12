using DirectoryService.Shared;

namespace DirectoryService.Core.Entities;

public class Session
{
    public Guid Token { get; set; }
    public string UserId { get; set; }
    public TokenScope Scope { get; set; }
    public UserRole Role { get; set; }
    public bool AsAdmin { get; set; }
}