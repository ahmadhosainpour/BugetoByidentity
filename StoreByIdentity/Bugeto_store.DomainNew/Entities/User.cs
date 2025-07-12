using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace EndPoint.Areas.Admin.ViewModel
{
    public class User
    {
        public int ID { get; set; }
        [DisplayName("نام کاربر")]
        [Required  (ErrorMessage ="لطفا نام کاربری را وارد نمایید ")]
        [Remote("IsUserNamelInUse","Accounts", areaName: "Admin", HttpMethod = "Post", ErrorMessage = "یوزر وارد شده قبلا در سیستم ثبت شده است")]
        public string FullName { get; set; }
        [DisplayName("آدرس ایمیل")]
        [Required(ErrorMessage = "لطفا ایمیل را وارد نماید")]
        [EmailAddress(ErrorMessage = "آدرس ایمیل اشتباه است")]
        [Remote("IsEmailInUse", "Accounts", areaName: "Admin", HttpMethod = "Post", ErrorMessage = "ایمیل وارد شده قبلا در سیستم ثبت شده است")]
        public string Email { get; set; }
        [Required(ErrorMessage = "لطفا پسورد را وارد نمایید")]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }
        [Required(ErrorMessage = "لطفا پسورد را وارد نمایید")]
        [System.ComponentModel.DataAnnotations.Compare(nameof(PassWord))]
        [DataType(DataType.Password)]
        public string ConfirmPassWord { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public bool isdisabled { get; set; }
        public bool IsSelected { get; set; }
    }
}
