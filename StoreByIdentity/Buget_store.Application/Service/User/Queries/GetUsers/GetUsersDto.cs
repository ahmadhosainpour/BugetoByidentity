using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Buget_store.Application.Service.User.Queries.GetUsers
{
    public class GetUsersDto
    {
        public int ID { get; set; }
        [DisplayName("نام کاربر")]
        [Required]
        public string FullName { get; set; }
        [DisplayName("آدرس ایمیل")]
        [Required(ErrorMessage = "لطفا ایمیل را وارد نماید")]
        [EmailAddress(ErrorMessage = "آدرس ایمیل اشتباه است")]
        public string Email { get; set; }
    }
}
