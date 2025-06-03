using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugeto_store.Domain.Entities.User
{
    public class User
    {
      
        
        public int ID { get; set  ; } 
        [DisplayName("نام کاربر")]
        public string FullName { get; set; }
        [DisplayName("آدرس ایمیل")]
        public string Email { get; set; }
        public string PassWord { get; set; }
        public string ConfirmPassWord { get; set; }
        public string Role { get; set; }
       public bool isdisabled { get; set; }
        public bool IsSelected { get; set; } 
        public ICollection<UserInRole> userInRoles { get; set; }

    }

}
