using Buget_store.Application.Service.User.Queries.GetUsers;
using Bugeto_store.Domain.Entities.User;
using Bugeto_store.Persistence.Context;
using EndPoint.Areas.Admin.ViewModel;
using EndPoint.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace EndPoint.Areas.Admin.Controllers
{
   //برای دیدن این کنترلر باید حتما وارد سایت شوند
    public class AccountsController : Controller
    {
        private readonly UserManager<IdentityUser> _UserManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMessageSender _messageSender;

        public AccountsController(UserManager<IdentityUser> UserManager, SignInManager<IdentityUser> signInManager,IMessageSender messageSender)
        {
            _UserManager = UserManager;
            _signInManager = signInManager;
            _messageSender = messageSender;
        }



        [Area("Admin")]

        public IActionResult Index()
        {
            return View();
        }
        [Area("Admin")]
       
        public async Task<IActionResult> IsUserNamelInUse(string FullName)//بررسی وجود یوزر 
        {
            var userName = await _UserManager.FindByNameAsync(FullName);
            if (userName == null) return Json(true);
           
            return Json("یوزر وارد شده قبلا در سیستم ثبت شده است");
        }
        [Area("Admin")]
    
        public async Task<IActionResult> IsEmailInUse(string Email)//بررسی وجود یوزر 
        {
            var userName = await _UserManager.FindByEmailAsync(Email);
            if (userName == null) return Json(true);
            return Json("ایمیل وارد شده قبلا در سیستم ثبت شده است");
        }
        [Area("Admin")]
        public async Task<IActionResult> ConfirmEmail(string userName,string token)
        {
            if (string.IsNullOrEmpty(userName)||string.IsNullOrEmpty(token))
            {
              return  NotFound();  
            }
            var user=await _UserManager.FindByNameAsync(userName);
            if (user == null) return NotFound();
            var result=await _UserManager.ConfirmEmailAsync(user, token);
            return Content(result.Succeeded ? "Email Confirmed" : "Email Not Confirmed");         
        }


        [Area("Admin")]
        [HttpPost]
        public async Task<IActionResult> ModifyUser(User Model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser()
                {
                    UserName = Model.FullName,
                    Email = Model.Email
                };
                var result = await _UserManager.CreateAsync(user, Model.PassWord);
                if (result.Succeeded)
                {
                    var emailcomfirmationtoken=await _UserManager.GenerateEmailConfirmationTokenAsync(user);
                    var emailMessage = Url.Action("ConfirmEmail", "Accounts",
                        new {username=user.UserName ,token=emailcomfirmationtoken},
                        Request.Scheme);
                    _messageSender.sendEmailAsync(Model.Email, "email Confirmation", emailMessage);
                    return RedirectToAction("Register", "Accounts");
                }
                else


                { //بررسی انجام عملیات ایجا یوزر
                    foreach (var Error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, Error.Description);//نمایش  خطا به کاربر 

                    }
                }
            }


            return View(Model);



        }

        [Area("Admin")]
        [HttpGet]
        public async Task<IActionResult> Register()
        {

            /*     var user = await _context.Users.ToListAsync();*/


            return View(/*user*/);



        }
        [Area("Admin")]
        [HttpGet]
        public async Task<IActionResult> Login(string ReturnUrl)//دریافت لینک وارد شده
        {

            if (_signInManager.IsSignedIn(User))//بررسی loginبودن کاربر
            {
                return RedirectToAction("index", "ManageUser");
            }

            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }



        [Area("Admin")]
       [HttpPost]
        public async Task<IActionResult> Login(Login model, string ReturnUrl = null)
        {

           
            if (_signInManager.IsSignedIn(User))//بررسی loginبودن کاربر
            {
                return RedirectToAction("index", "ManageUser");
            }
            ViewData["returnUrl"] = ReturnUrl;//پرکردنviewdata  در صورت  وارد کردن رمز اشتباه
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.FullName, model.PassWord, model.RememberMe, true);
                if (result.Succeeded)
                {
                    if ((!string.IsNullOrEmpty(ReturnUrl) )&& Url.IsLocalUrl(ReturnUrl))//بررسی آدرس وب سایت برای جلوگیری از هک سایت 
                    {

                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "ManageUser");
                    }
                        
                }

                if (result.IsLockedOut)
                {
                    ViewData["ErrorMessage"] = "به دلیل وارد کردن تعداد زیاد اطلاعات اشتباه کاربری شما برای مدت پنج دقیقه  غیر فعال شده است";
                    return View(model);
                }

                ModelState.AddModelError("", "رمز عبور یا نام کاربری اشتباه است");//نمایش  خطا به کاربر 


                //      ModelState.AddModelError("", "رمز عبور یا نام کاربری اشتباه است");

            }
            return View(model);
        }
      


        [Area("Admin")]
        [HttpPost]  //فقط به postباید پاسخگو باشد
        [ValidateAntiForgeryToken]
        public IActionResult LogOut()
        {

            _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Accounts");
        }

        //[Area("Admin")]
        //[HttpPost]
        //public async Task<IActionResult> ModifyUser(string fullName, string email, string role, string password, string confirmpassword)
        //{
        //    if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(password))
        //    {
        //        ModelState.AddModelError(string.Empty, "لطفا اطلاعات را درست وارد نمایید");
        //        return View();
        //    }

        //    var users = new User
        //    {


        //        FullName = fullName,
        //        Email = email,
        //        Role = role,


        //    };
        //   // users.PassWord = password.HashPassword(users, password);
        //    //users.ConfirmPassWord = _passwordHasher.HashPassword(users, password);
        //    //_context.Users.Add(users);  //add to DbSet<User>
        //    //await _context.SaveChangesAsync(); //save to database

        //    return View("ModifyUser");
        //}
    }
}



//[Area("Admin")]
//[HttpPost]
//public async Task<IActionResult> ChangePassword(List<User> Users)
//{

//    var selectedUsers = Users.Where(u => u.IsSelected).ToList();//دریافت لیست کاربر انتخاب شده



//    return View(selectedUsers);



//}


//[Area("Admin")]
//[HttpPost]
//public async Task<IActionResult> DisableUser(List<User> Users)
//{
//   // var user = await _context.Users.ToListAsync();
//    var selectedUsers = Users.Where(u => u.IsSelected).ToList();//دریافت لیست کاربر انتخاب شده
//    foreach (var item in selectedUsers)
//    {
//        var result = _context.Users.Where(p => p.ID == item.ID);
//        foreach (var item1 in result)
//        {
//            item1.isdisabled = true;
//        }
//        await _context.SaveChangesAsync();
//    }
//    return View("UserManager");




////}


//[Area("Admin")]
//[HttpPost]
//public async Task<IActionResult> EnableleUser(List<User> Users)
//{
//    //var user = await _context.Users.ToListAsync();
//    var selectedUsers = Users.Where(u => u.IsSelected).ToList();//دریافت لیست کاربر انتخاب شده


//    foreach (var item in selectedUsers)
//    {
//        var result = _context.Users.Where(p => p.ID == item.ID);
//        foreach (var item1 in result)
//        {
//            item1.isdisabled = false;
//        }
//        await _context.SaveChangesAsync();
//    }
//    return View("UserManager");




//}


//[Area("Admin")]
//public IActionResult ChangePassword()
//{
//    {



//        return View();
//    }
//}



//[Area("Admin")]
//[HttpPost]

//public async Task<IActionResult> UserRoleManage(string name)
//{
//    var Roles = _context.Roles.FirstOrDefault(n => n.Name == name);


//    return RedirectToAction("UserManager");
//}





//[Area("Admin")]
//[HttpPost]

//        public async Task<IActionResult> DeleteUsers(List<User> Users)
//        {

//            //var user = await _context.Users.ToListAsync();
//            var selectedUsers = Users.Where(u => u.IsSelected).ToList();//دریافت لیست کاربر انتخاب شده
//            try
//            {
//                foreach (var item in selectedUsers)
//                {
//                    var result = _context.Users.Where(p => p.ID == item.ID).ToList();
//                    Users.Remove(item);



//                }

//                _context.Users.RemoveRange(selectedUsers);//حذف از دیتابیس
//                await _context.SaveChangesAsync();
//                return View("UserManager");

//            }
//            catch (Exception ex)
//            {
//                string message = " <i class=\"fas fa-exclamation-triangle m-5\" style=\"font-size:200px\"></i>" +
//                    "<span style='color:red;'>کاربر مورد نظر به دلیل ارتباط با بخشهای دیگر قابل حذف کردن نمی باشد لطفا ابتدا در قسمتهای دیگر کاربر  را حذف نموده سپس اقدام به حذف نمایید</span>";

//                return Content(message, "text/html", Encoding.UTF8);
//            }

//        }
//        [Area("Admin")]
//        public async Task<IActionResult> ModifyUser()
//        {


//            return View();
//        }




//    }

//}
