using System.ComponentModel.DataAnnotations;

namespace WebApplication2.DTO
{
    public class RegisterDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserName {  get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string[] Roles { get; set; }
    }
}
