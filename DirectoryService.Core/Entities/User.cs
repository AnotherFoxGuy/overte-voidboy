using DirectoryService.Shared;

namespace DirectoryService.Core.Entities;

public class User : BaseEntity
{
    public IdentityProvider IdentityProvider;
    public AccountState State;
    public string? Username;
    public string? Email;
    public string? AuthHash;
    public long AuthVersion;
    public bool Activated;
    public UserRole Role;
    public string? CreatorIp;
    public string? Language;
    public string? ConnectionGroup;
    public string? FriendsGroup;
    public bool? Self { get; set; }
    public bool? Connection { get; set; }
    public bool? Friend { get; set; }
    public string? Settings { get; set; }

    public bool Enabled => Activated && State == AccountState.Normal;
}