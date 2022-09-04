using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ubereats.Models
{
    public class User
    {
        [Column("User_ID")]
        public int Id { get; set; }

        [Display(Name = "Логин:")]
        [Required(ErrorMessage = "Введите логин")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Слишком короткий логин")]
        public string loginname { get; set; } = "";

        [Display(Name = "Пароль:")]
        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Слишком короткий пароль")]
        public string password { get; set; } = "";

        public string email { get; set; } = "";
    }
}
