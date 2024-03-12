using DirectoryService.Shared;

namespace DirectoryService.Core.Entities;

public class SessionToken : BaseEntity
{
    public Guid RefreshToken { get; set; }
    public string UserId { get; set; }
    public TokenScope Scope { get; set; }
    public DateTime Expires { get; set; }
}