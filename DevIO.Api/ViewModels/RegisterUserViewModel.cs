using System.ComponentModel.DataAnnotations;

namespace DevIO.Api.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength =2)]        
        public string UserName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [Required]
        [StringLength(8, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password",ErrorMessage ="As senhas informadas não são identicas")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginUserViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string UserName { get; set; }

        [Required]
        [StringLength(8, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength =6)]
        public string Password { get; set; }

    }
}
