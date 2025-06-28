using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Mvc;



namespace Bugeto_store.Domain.Entities.User
{
    public class User
    {
      
        
        public int ID { get; set  ; } 
        [DisplayName("نام کاربر")]
        [Required]
       //[Remote("IsUserNamelInUse","Accounts")]
        public string FullName { get; set; }
        [DisplayName("آدرس ایمیل")]
        [Required(ErrorMessage = "لطفا ایمیل را وارد نماید")]
        [EmailAddress(ErrorMessage = "آدرس ایمیل اشتباه است")]
       //[Remote("IsUserNamelInUse","Accounts")]
        public string Email { get; set; }
        [Required  (ErrorMessage ="لطفا پسورد را وارد نمایید")]
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
        //public ICollection<UserInRole> userInRoles { get; set; }

    }

}
