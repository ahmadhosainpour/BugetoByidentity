using Azure.Core;
using Buget_store.Application.Interface.Context;
using Buget_store.Application.Service.User.Queries.GetUsers;
using Bugeto_store.Domain.Entities.User;
using Bugeto_store.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Data;



namespace EndPoint.Areas.Admin.Controllers
{
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



        [Area("Admin")]
        public async Task<IActionResult> UserManager()
        {

            var user = await _context.Users.ToListAsync();
            ViewBag.context = user;
          
                return View(user);
 
               
          
        }


        [Area("Admin")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(List<User> Users)
        {

            var selectedUsers = Users.Where(u => u.IsSelected).ToList();//دریافت لیست کاربر انتخاب شده


          
                return View(selectedUsers);
     


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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrEdit(int selectedUserId, string action)
        {
            //switch (action)
            //{
            //    case "حذف":
            var user = _context.Users.Find(selectedUserId);
            //    if (user != null)
            //    {
            //        _context.Users.Remove(user);
            //        _context.SaveChanges();
            //    }
            //    return RedirectToAction("UserManager");
            //case "ویرایش":





            //    var users = _context.Users.Find(selectedUserId);
            //    TempData["id"] = selectedUserId;

            //    ViewBag.user = users;
            //    return View(users);

            ////("Edit" , new { id = selectedUserId });



            //default:
            return View();
            // }
        }
        [Area("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string fullname, string email, string password)
        {

            var users = _context.Users.Find(TempData["id"]);
            if (ViewBag.id != users.ID)


                users.Email = email;
            users.FullName = fullname;
            await _context.SaveChangesAsync();



            return RedirectToAction("UserManager");


        }


        [Area("Admin")]
        public IActionResult Edit()
        {




            return View();


        }
        [Area("Admin")]
        public async Task<IActionResult> ModifyUser()
        {


            return View();
        }
        [Area("Admin")]
        public IActionResult DeleteOrEdit()
        {
            User user = new User
            {
                FullName = ViewBag.user.FullName,
                Email = ViewBag.user.Email
            };

            return View(user);
        }


    }


}

