namespace CodeHub.Web.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;

    using CodeHub.Web.Infrastructure.Filters;

    public class RegisterViewModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        [UserNameAllowedSymbolsAttribute(ErrorMessage = "Only latin letters, digits and '_' are allowed in the username")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [RegularExpression(@"([\w\.\-_]+)?\w+@[\w-_]+(\.\w+){1,}")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Upload an avatar")]
        public string Avatar { get; set; }
    }
}