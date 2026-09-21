using System.ComponentModel.DataAnnotations;

namespace EventaroApi.DTOs.UserDTOs
{
    public class CreateUserDTO
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        [MinLength(8)]
        public required string Password { get; set; }
        [Required]
        [MaxLength(255)]
        public required string FirstName { get; set; }
        [Required]
        [MaxLength(255)]
        public required string LastName { get; set; }
        public int? OrganizationId { get; set; }
    }
}
