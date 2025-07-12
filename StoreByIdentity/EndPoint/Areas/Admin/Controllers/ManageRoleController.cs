using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EndPoint.Areas.Admin.Controllers
{
    public class ManageRoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageRoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        [Area("Admin")]
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }
        [Area("Admin")]
       
        public IActionResult AddRole() 
        {
            return View();
        }
        [Area("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(string name)
        {
            if (string.IsNullOrEmpty(name)) return NotFound();
            var role = new IdentityRole(name);
            var result=await _roleManager.CreateAsync(role);//ساخت role
            if (result.Succeeded)
            {
                return RedirectToAction("Index");   
                 }
            foreach (  var error in result.Errors)
            {
                ModelState.TryAddModelError(string.Empty, error.Description);
            }
            return View(role);
        }
        [Area("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var role = await _roleManager.FindByIdAsync(id);
            if(role==null) return NotFound();   
              await _roleManager.DeleteAsync(role);
            return RedirectToAction("Index");
        }
        [Area("Admin")]
         public async Task<IActionResult> EditRole(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();
      
            return View(role);
        }
        [Area("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> EditRole(string id,string name)
        {
            if (string.IsNullOrEmpty(id)|| string.IsNullOrEmpty(name)) return NotFound();
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();
            role.Name = name;
            var result=await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(string.Empty, error.Description);
            }
            return View(role);
        }


    }
}
