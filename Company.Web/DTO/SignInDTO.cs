using System.ComponentModel.DataAnnotations;

namespace Company.Web.DTO
{
    public class SignInDTO
    {
      

        [Required(ErrorMessage = "يرجى إدخال البريد الإلكتروني.")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني الذي أدخلته غير صحيح.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "يرجى إدخال كلمة المرور.")]
        [MinLength(6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل.")]
        [MaxLength(20, ErrorMessage = "كلمة المرور يجب أن تكون 20 حرفًا كحد أقصى.")]
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
