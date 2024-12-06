namespace DirectoryService.Core.Dto;

public class UpdateAccountDto
{
    public string AccountId { get; set; }
    public string? Email { get; set; }
    public string? PublicKey { get; set; }
    public UserImagesDto? Images { get; set; }
}