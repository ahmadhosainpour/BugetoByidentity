using Azure.Core;
using Buget_store.Application.Interface.Context;
using Buget_store.Application.Service.User.Queries.GetUsers;
using Bugeto_store.Domain.Entities.User;
using Bugeto_store.Persistence.Context;
using EndPoint.Areas.Admin.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Protocol.Plugins;
using System.Data;
using System.Text;



namespace EndPoint.Areas.Admin.Controllers
{
  //  [Authorize]
    public class UsersController : Controller
    {

        public readonly IGetUserService _getUserService;


        private readonly DataBaseContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UsersController(IGetUserService getUserService, DataBaseContext context, IPasswordHasher<User> passwordHasher)
        {
            _getUserService = getUserService;
            _context = context;
            _passwordHasher = passwordHasher;
        }



        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }


        [AllowAnonymous]
        [Area("Admin")]
        public async Task<IActionResult> UserManager()
        {

            var user = await _context.Users.ToListAsync();


            return View(user);



        }
        [Area("Admin")]
        [HttpPost]
        public async Task<IActionResult> ModifyUser(string fullName,string email,string role, string password, string confirmpassword)
        {
            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(string.Empty, "لطفا اطلاعات را درست وارد نمایید");
                return View();
            }

            var users = new User
            {


                FullName = fullName,
                Email = email,
                Role = role,


            };
            users.PassWord = _passwordHasher.HashPassword(users, password);
            users.ConfirmPassWord = _passwordHasher.HashPassword(users, password);
            _context.Users.Add(users);  //add to DbSet<User>
            await _context.SaveChangesAsync(); //save to database

            return View("ModifyUser");
        }



        [Area("Admin")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(List<User> Users)
        {

            var selectedUsers = Users.Where(u => u.IsSelected).ToList();//دریافت لیست کاربر انتخاب شده



            return View(selectedUsers);



        }


        [Area("Admin")]
        [HttpPost]
        public async Task<IActionResult> DisableUser(List<User> Users)
        {
            var user = await _context.Users.ToListAsync();
            var selectedUsers = Users.Where(u => u.IsSelected).ToList();//دریافت لیست کاربر انتخاب شده
            foreach (var item in selectedUsers)
            {
                var result = _context.Users.Where(p => p.ID == item.ID);
                foreach (var item1 in result)
                {
                    item1.isdisabled = true;
                }
                await _context.SaveChangesAsync();
            }
            return View("UserManager");




        }


        [Area("Admin")]
        [HttpPost]
        public async Task<IActionResult> EnableleUser(List<User> Users)
        {
            var user = await _context.Users.ToListAsync();
            var selectedUsers = Users.Where(u => u.IsSelected).ToList();//دریافت لیست کاربر انتخاب شده


            foreach (var item in selectedUsers)
            {
                var result = _context.Users.Where(p => p.ID == item.ID);
                foreach (var item1 in result)
                {
                    item1.isdisabled = false;
                }
                await _context.SaveChangesAsync();
            }
            return View("UserManager");




        }


        [Area("Admin")]
        public IActionResult ChangePassword()
        {
            {



                return View();
            }
        }



        [Area("Admin")]
        [HttpPost]

        public async Task<IActionResult> UserRoleManage(string name)
        {
            var Roles = _context.Roles.FirstOrDefault(n => n.Name == name);


            return RedirectToAction("UserManager");
        }





        [Area("Admin")]
        [HttpPost]
       
        public async Task<IActionResult> DeleteUsers(List<User> Users)
        {

            var user = await _context.Users.ToListAsync();
            var selectedUsers = Users.Where(u => u.IsSelected).ToList();//دریافت لیست کاربر انتخاب شده
            try
            {
                foreach (var item in selectedUsers)
                {
                    var result = _context.Users.Where(p => p.ID == item.ID).ToList();
                    Users.Remove(item);



                }

                _context.Users.RemoveRange(selectedUsers);//حذف از دیتابیس
                await _context.SaveChangesAsync();
                return View("UserManager");

            }
            catch (Exception ex)
            {
                string message = " <i class=\"fas fa-exclamation-triangle m-5\" style=\"font-size:200px\"></i>" +
                    "<span style='color:red;'>کاربر مورد نظر به دلیل ارتباط با بخشهای دیگر قابل حذف کردن نمی باشد لطفا ابتدا در قسمتهای دیگر کاربر  را حذف نموده سپس اقدام به حذف نمایید</span>";

                return Content(message, "text/html", Encoding.UTF8);
            }
                
            }
        [Area("Admin")]
        public async Task<IActionResult> ModifyUser()
        {


            return View();
        }




    }


    }


