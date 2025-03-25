using System.ComponentModel.DataAnnotations;

namespace Company.Web.DTO
{
    public class SignUpDTO
    {
        [Required(ErrorMessage = "يرجى إدخال اسم المستخدم.")]
        [MinLength(3, ErrorMessage = "اسم المستخدم يجب أن يكون 3 أحرف على الأقل.")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "يرجى إدخال الاسم الأول.")]
        [MinLength(3, ErrorMessage = "الاسم الأول يجب أن يكون 3 أحرف على الأقل.")]
        public string? FName { get; set; }

        [Required(ErrorMessage = "يرجى إدخال الاسم الأخير.")]
        [MinLength(3, ErrorMessage = "الاسم الأخير يجب أن يكون 3 أحرف على الأقل.")]
        public string? LName { get; set; }

        [Required(ErrorMessage = "يرجى إدخال البريد الإلكتروني.")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني الذي أدخلته غير صحيح.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "يرجى إدخال كلمة المرور.")]
        [MinLength(6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل.")]
        [MaxLength(20, ErrorMessage = "كلمة المرور يجب أن تكون 20 حرفًا كحد أقصى.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "يرجى  تأكيد كلمة المرور.")]
        [Compare("Password", ErrorMessage = "كلمة المرور وتأكيدها غير متطابقين.")]
        public string? ConfirmePassword { get; set; }

        [Required(ErrorMessage = "يرجى الموافقة على الشروط والأحكام.")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "يجب الموافقة على الشروط والأحكام.")]
        public bool IsAgree { get; set; }
    }
}
