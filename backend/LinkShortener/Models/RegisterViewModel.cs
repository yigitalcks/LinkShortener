using System.ComponentModel.DataAnnotations;

namespace LinkShortener.ViewModels
{
    public class RegisterViewModel
    {
        [Required] 
        [EmailAddress] 
        public string Email { get; set; }

        [Required] 
        [MinLength(6)] 
        public string Password { get; set; }
    }
}