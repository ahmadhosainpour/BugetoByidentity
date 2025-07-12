using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Bugeto_store.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata;
using static Bugeto_store.Domain.Entities.AddUserToRoleViewModel;
using EndPoint.Areas.Admin.ViewModel;
using Bugeto_store.Persistence.Context;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using EndPoint.Repositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace EndPoint.Areas.Admin.Controllers
{
    public class ManageUserController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _useManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly DataBaseContext _context;

        public ManageUserController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> useManager, DataBaseContext context, SignInManager<IdentityUser> signInManager)
        {
            _roleManager = roleManager;
            _useManager = useManager;
            _context = context;
            _signInManager = signInManager;
        }
        [Area("Admin")]
       
        public IActionResult Index()//نمایش لیست کاربران
        {
            var model = _useManager.Users.Select(u => new indexviewModel()
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email
            }).ToList();

            return View(model);
        }
        [Area("Admin")]
        public async Task<IActionResult> AddUserToRole(string id)
        {
            var user = await _useManager.FindByIdAsync(id);
            if (string.IsNullOrEmpty(id)) return NotFound();
            if (user == null) NotFound();
            var roles = _roleManager.Roles.AsNoTracking().Select(r => r.Name).ToList();
            var userroles = await _useManager.GetRolesAsync(user);
            var validroles = roles.Where(r => !userroles.Contains(r))
                .Select(r => new UserRolesViewModel(r)).ToList();

            var model = new AddUserToRoleViewModel(id, validroles);


            return View(model);
        }
        [Area("Admin")]
        [HttpPost]
        
        public async Task<IActionResult> AddUserToRole(AddUserToRoleViewModel model)
        {
            if (model == null) return NotFound();

            var user = await _useManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();
            var requestRoles = model.UserRoles.Where(r => r.IsSelected).
                Select(u => u.RoleName).ToList();

            var result = await _useManager.AddToRolesAsync(user, requestRoles);


            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [Area("admin")]
       
        public async Task<IActionResult> RemoveUserfromRole(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var user = await _useManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var allRoles = _roleManager.Roles.ToList();
            var userRoles = await _useManager.GetRolesAsync(user);

            var model = new AddUserToRoleViewModel
            {
                UserId = user.Id,
                UserRoles = allRoles
        .Where(role => userRoles.Contains(role.Name)) // only roles user currently has
        .Select(role => new UserRolesViewModel(role.Name)
        {
            RoleName = role.Name,
            IsSelected = false // allow admin to select which to remove
        }).ToList()
            };



            ViewBag.User = user.UserName;
            return View(model);
        }


        [Area("Admin")]
        [HttpPost]
     
        public async Task<IActionResult> RemoveUserRole(AddUserToRoleViewModel model)
        {
            if (model == null) return NotFound();

            var user = await _useManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();

            var currentRoles = await _useManager.GetRolesAsync(user);

            var rolesToRemove = model.UserRoles
                .Where(r => r.IsSelected && currentRoles.Contains(r.RoleName))
                .Select(r => r.RoleName)
                .ToList();

            var result = await _useManager.RemoveFromRolesAsync(user, rolesToRemove);

            if (result.Succeeded)
            {
                TempData["Message"] = "نقش‌ها با موفقیت حذف شدند";
                return RedirectToAction("RemoveUserfromRole", new { id = model.UserId });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return RedirectToAction("RemoveUserfromRole");
        }

        [Area("admin")]
        [AllowAnonymous]
        
        public async Task<IActionResult> AddUserToClaim(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
          
            var user = await _useManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var allClaims = ClaimStore.AllClaims;
            var userClaims = await _useManager.GetClaimsAsync(user);
            var validClaims = allClaims.Where(c => userClaims.All(x => x.Type != c.Type)).Select(c => new ClaimViwModel(c.Type)).ToList();
            var model = new AddOrRemoveClaimViewModel(id, validClaims);
          
            return View(model);
        }

        [Area("admin")]
        [HttpPost]
        public async Task<IActionResult> AddUserToClaim(AddOrRemoveClaimViewModel model)
        {


        
            if (model == null) return NotFound();

            var user = await _useManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();
            var requestclaim = model.UserClaims.Where(r => r.IsSelected).
                Select(u => new Claim(u.ClaimType, "True")).ToList();

            var result = await _useManager.AddClaimsAsync(user,requestclaim);


            
            if (result.Succeeded)

            {
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(string.Empty, error.Description);
            }

            return View(model);
        }


        [Area("admin")]
      [Authorize(Policy = "EmploeeListPolicy")]
        public async Task<IActionResult> RemoveUserToClaim(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var user = await _useManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var allclaim = ClaimStore.AllClaims.ToList();
            var userClaims = await _useManager.GetClaimsAsync(user);

            var validClaims = userClaims.Select(c => new ClaimViwModel(c.Type)).ToList();
            var model = new AddOrRemoveClaimViewModel(id, validClaims);
            ViewBag.user = user;
            return View(model);
        }

        [Area("admin")]
        [HttpPost]
       
        public async Task<IActionResult> RemoveUserToClaim(AddOrRemoveClaimViewModel model)
        {
            if (model == null) return NotFound();

            var user = await _useManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();
            var requestclaim = model.UserClaims.Where(r => r.IsSelected).
                Select(u => new Claim(u.ClaimType, "True")).ToList();
            

            var result = await _useManager.RemoveClaimsAsync(user,requestclaim);

            if (result.Succeeded)
            {
                TempData["Message"] = "‌ با موفقیت حذف شدند";
               
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return RedirectToAction("RemoveUserToClaim");
        }








        [Area("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();
            await _roleManager.DeleteAsync(role);
            return RedirectToAction("Index");
        }
        [Area("Admin")]
        public async Task<IActionResult> EditUser(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var user = await _useManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }
        [Area("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> EditUser(string id, string username)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(username)) return NotFound();
            var user = await _useManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            user.UserName = username;
            var result = await _useManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(string.Empty, error.Description);
            }
            return View(user);
        }


    }
}
