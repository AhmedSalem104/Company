using System.ComponentModel.DataAnnotations;

namespace Company.Web.DTO
{
    public class ForgetPasswordDTO
    {

        [Required(ErrorMessage = "يرجى إدخال البريد الإلكتروني.")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني الذي أدخلته غير صحيح.")]
        public string? Email { get; set; }
    }
}
