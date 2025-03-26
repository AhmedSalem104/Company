using System.ComponentModel.DataAnnotations;

namespace Company.Web.DTO
{
    public class ResetPasswordDTO
    {

        [Required(ErrorMessage = "يرجى إدخال كلمة المرور.")]
        [MinLength(6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل.")]
        [MaxLength(20, ErrorMessage = "كلمة المرور يجب أن تكون 20 حرفًا كحد أقصى.")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "يرجى  تأكيد كلمة المرور.")]
        [Compare("NewPassword", ErrorMessage = "كلمة المرور وتأكيدها غير متطابقين.")]
        public string? ConfirmePassword { get; set; }

    }
}
