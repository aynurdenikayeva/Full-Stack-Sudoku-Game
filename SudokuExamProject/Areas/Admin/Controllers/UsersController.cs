using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SudokuExamProject.DataAccessLayer;
using SudokuExamProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SudokuExamProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UsersController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<User> users = _context.Users.ToList();
            return View(users);
        }
        [HttpGet]
        public  IActionResult Add()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(User user)
        {

            if (ModelState.IsValid)
            {
                if (user.formFile != null)
                {
                    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                    if (!Directory.Exists(uploadFolder))
                        Directory.CreateDirectory(uploadFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + user.formFile.FileName;

                    string filePath = Path.Combine(uploadFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await user.formFile.CopyToAsync(stream);
                    }

                    user.Image = "/uploads/" + uniqueFileName;
                }
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
            return View();
        }
        [HttpGet]
        public IActionResult Read(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            User user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            User user = _context.Users.FirstOrDefault(x=>x.Id==id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpPost]
        public IActionResult Update(int? id, User user)
        {
            if (id == null)
            {
                return BadRequest();    
            }
            _context.Users.Update(user);

            _context.SaveChanges();
            return RedirectToAction("Index");


        }
        [HttpGet]
        public IActionResult Delete(int id) 
        {
            if (id == null) 
            {
                return BadRequest();
            }

            User user = _context.Users.FirstOrDefault(y=>y.Id==id);
            _context.SaveChanges();

            if (user == null)
            { 
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirm(int id) 
        {

            if (id == null) 
            { 
                return BadRequest();
            }
            User user = _context.Users.Find(id);
            _context.Remove(user);
            _context.SaveChanges();
            if (user == null) 
            { 
                return NotFound();
            }
            return RedirectToAction("Index");

        }
        [HttpPost]
        public IActionResult IsActiveStatus(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            User user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            if (user.AccountStatus == "Active")
            {
                user.AccountStatus = "Blocked";
            }
            else if (user.AccountStatus == "Blocked")
            {
                user.AccountStatus = "InActive";
            }
            else
            {
                user.AccountStatus = "Active";

            }


            _context.SaveChanges();
            return RedirectToAction("Index");
        }






    }
}
