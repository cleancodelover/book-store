using System.ComponentModel.DataAnnotations;

namespace BookStore.API.ViewModels.Auth
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
