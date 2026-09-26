namespace EventaroApi.DTOs.UserDTOs;

public class UserInfoDTO
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int? OrganizationId { get; set; } 
}
