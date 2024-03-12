namespace DirectoryService.Core.Entities;

public class ActivationToken : BaseEntity
{
    public string UserId { get; set; }
    public DateTime Expires { get; set; }
}