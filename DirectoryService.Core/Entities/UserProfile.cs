namespace DirectoryService.Core.Entities;

public class UserProfile : BaseEntity
{
    public string UserId { get; set; }
    public string? HeroImageUrl { get; set; }
    public string? ThumbnailImageUrl { get; set; }
    public string? TinyImageUrl { get; set; }
}