using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugeto_store.Domain.Entities.User
{
    public class Login
    {
        [DisplayName("نام کاربر")]
        [Required]
        public string FullName { get; set; }
        [Required(ErrorMessage = "لطفا پسورد را وارد نمایید")]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }
        [DisplayName(" مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
        
    }
}
