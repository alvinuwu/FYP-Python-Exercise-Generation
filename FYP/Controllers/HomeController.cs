using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FYP.Models;
using Microsoft.EntityFrameworkCore;

namespace FYP.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _dbContext;
        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Lecturer")]
        public IActionResult ViewUsers()
        {
            DbSet<AppUser> users = _dbContext.AppUser;
            List<AppUser> usersList = users.ToList();
            return View(usersList);
        }
        [Authorize(Roles = "Admin,Lecturer")]
        public IActionResult CreateUser()
        {
            return View();
        }
        [Authorize(Roles = "Lecturer,Admin")]
        [HttpPost]
        public IActionResult CreateUser(NewUser appUser)
        {
            DbSet<AppUser> users = _dbContext.AppUser;
            List<AppUser> userList = users.ToList();

            if (userList.Where(c => c.Id.Equals(appUser.Id)).Count() > 0)
            {
                TempData["Msg"] = "Failed to update database! User already exists.";
                TempData["MsgType"] = "danger";

                return RedirectToAction("ViewUsers");
            }
            else
            {

                AppUser user = new AppUser();
                user.Id = appUser.Id;
                user.Name = appUser.Name;
                user.Password = System.Text.Encoding.ASCII.GetBytes(appUser.Password);
                user.Role = appUser.Role;
                user.Class = appUser.Class;
                user.Email = appUser.Email;

                _dbContext.AppUser.Add(user);
                if (_dbContext.SaveChanges() == 1)
                {
                    int num_records = _dbContext.Database.ExecuteSqlInterpolated(($"UPDATE AppUser SET Password = HASHBYTES('SHA1', {user.Password}) WHERE Id = {appUser.Id}"));
                }
                ;

                TempData["Msg"] = "New User added!";
                TempData["MsgType"] = "success";

                return RedirectToAction("ViewUsers");
            }
        }
    }
}