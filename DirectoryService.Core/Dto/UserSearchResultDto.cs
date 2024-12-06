using System.Text.Json.Serialization;

namespace DirectoryService.Core.Dto;

public class UserSearchResultDto
{
    public string? Username { get; set; }
    
    //V2 - Standardise naming convention
    [JsonPropertyName("accountid")]
    public string? AccountId { get; set; }
    public UserImagesDto? Images { get; set; }
    public LocationReferenceDto? Location { get; set; }
}

public class VerboseUserSearchResultDto : UserSearchResultDto
{
    public object? ProfileDetail { get; set; }
}