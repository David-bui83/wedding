using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using wedding.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace wedding.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("registor")]
        public IActionResult Register(User user)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email","Email is already taken");
                    return View("Index");
                }
                else
                {
                    PasswordHasher<User> hasher = new PasswordHasher<User>();
                    user.Password = hasher.HashPassword(user,user.Password);
                    dbContext.Add(user);
                    dbContext.SaveChanges();

                    var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == user.Email);

                    HttpContext.Session.GetInt32("id");
                    HttpContext.Session.SetInt32("id",userInDb.UserId);
                    return RedirectToAction("Dashboard");
                }
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUser userSubmission)
        {
            if(ModelState.IsValid)
            {
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.LoginEmail);
                if(userInDb == null)
                {
                    ModelState.AddModelError("LoginEmail","Invalid Email/Password");
                    return View("Index");
                }
                
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LoginPassword);
                if(result==0)
                {
                    ModelState.AddModelError("LoginEmail","Invalid Email/Password");
                    return View("Index");
                }
                else
                {
                    HttpContext.Session.GetInt32("id");
                    HttpContext.Session.SetInt32("id",userInDb.UserId);
                    return RedirectToAction("Dashboard");
                }
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            int? uid = HttpContext.Session.GetInt32("id");

            if(uid == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.uid = (int)uid;

           

            // List<RSVP> rsvp = dbContext.RSVPs.Include(w => w.Wedding).ToList();
            // foreach(var r in rsvp)
            // {
            //     if(r.Wedding.WeddingDate < DateTime.Now)
            //     {
            //         dbContext.Remove(r);
            //         dbContext.SaveChanges();
            //     }
            // }

            // var wedList = dbContext.Weddings.Include(w => w.creator).Include(w => w.UserRsvp).ThenInclude(g => g.User).OrderBy(w => w.WeddingDate).ToList();

            //  foreach(var act in wedList)
            // {
            //     DateTime actDate = Convert.ToDateTime(act.WeddingDate);

            //     if (actDate < DateTime.Now)
            //     {
            //         Delete(act.WeddingId);
            //     }
            // }

            var wedList = dbContext.Weddings.Include(c => c.creator).Include(u => u.UserRsvp).ThenInclude(r => r.User).Where(w => w.WeddingDate >= DateTime.Now).ToList();

            // foreach(var wed in wedList)
            // {
            //     if(wed.WeddingDate < DateTime.Now)
            //     {
            //         dbContext.Remove(wed);
            //         dbContext.SaveChanges();
            //     }
            // }

            ViewBag.weds = wedList;
            return View("Dashboard",wedList);
        }

        [HttpGet("display/{wid}")]
        public IActionResult Display(int wid)
        {
            int? uid = HttpContext.Session.GetInt32("id");
            if(uid == null)
            {
                return RedirectToAction("Index");
            }

            Wedding wed = dbContext.Weddings
                        .Include(w => w.UserRsvp)
                        .ThenInclude(r => r.User)
                        .FirstOrDefault(i => i.WeddingId == wid);
            ViewBag.wed = wed;

            return View("Display",wed);
        }

        [HttpGet("new")]
        public IActionResult NewWedding()
        {
            int? id = HttpContext.Session.GetInt32("id");
            if(id == null)
            {
                return RedirectToAction("Index");
            }
            return View("NewWedding");
        }

        [HttpPost("new/create")]
        public IActionResult CreateWedding(Wedding wedding)
        {
            int? id = HttpContext.Session.GetInt32("id");
            if(id == null)
            {
                return RedirectToAction("Index");
            }

            if(ModelState.IsValid)
            {
                wedding.UserId = (int)id;
                dbContext.Add(wedding);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View("NewWedding");
        }

        [HttpGet("join/{w_id}")]
        public IActionResult Join(int w_id)
        {
            int uid = (int)HttpContext.Session.GetInt32("id");
           RSVP rsvp = new RSVP();
            rsvp.UserId = uid;
            rsvp.WeddingId = w_id;
            dbContext.Add(rsvp);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("leave/{w_id}")]
        public IActionResult Leave(int w_id)
        {
            int uid = (int)HttpContext.Session.GetInt32("id");
            RSVP rsvp = dbContext.RSVPs.FirstOrDefault(r => r.WeddingId == w_id && r.UserId == uid);
            dbContext.Remove(rsvp);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("delete/{w_id}")]
        public IActionResult Delete(int w_id)
        {
            Wedding wed = dbContext.Weddings.FirstOrDefault(w => w.WeddingId == w_id);
            dbContext.Remove(wed);
            dbContext.SaveChanges();
            return Redirect("Dashboard");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
