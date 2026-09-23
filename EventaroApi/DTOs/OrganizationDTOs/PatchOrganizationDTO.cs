namespace EventaroApi.DTOs.OrganizationDTOs;

public class PatchOrganizationDTO
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Email { get; set; }
}
