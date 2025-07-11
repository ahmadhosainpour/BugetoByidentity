﻿using Microsoft.AspNetCore.Mvc;
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
        [Required]
        public string FullName { get; set; }
        [DisplayName("آدرس ایمیل")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PassWord { get; set; }
        [Required]
        [Compare(nameof(PassWord))]
        public string ConfirmPassWord { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public bool isdisabled { get; set; }
        public bool IsSelected { get; set; } 
        public ICollection<UserInRole> userInRoles { get; set; }

    }

}
