using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebProject.Models;
using WebProject.Services;

namespace WebProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private UserService _userService { get; set; }

        public HomeController
        (
            ILogger<HomeController> logger,
            [FromServices] UserService userService
        )
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cookiesData = HttpContext.Request.Cookies["loginData"];

            if(cookiesData == null)
            {
                return RedirectToAction(controllerName: "login", actionName: "login");
            }

            var user = await _userService.GetTokenUser(HttpContext.Request.Cookies["loginData"]);

            if (user == null || user.isBlocked) { return RedirectToAction(controllerName: "login", actionName: "login"); }

            return View(await _userService.GetAllUsers());
        }

        [HttpPost]
        async public Task<IActionResult> UnBlockUser(string[] email)
        {
            var allUser = await _userService.GetAllUsers();

            if(allUser != null)
            {
                var blockedUser = allUser.FindAll(x=>x.isBlocked == true && email.Contains(x.Email));

                foreach(var user in blockedUser)
                {
                    user.isBlocked = false;
                    await _userService.EditUser(user);
                }
            }

            return RedirectToAction(controllerName: "home", actionName: "index");
        }

        [HttpPost]
        async public Task<IActionResult> BlockUser(string[] email)
        {
            var allUser = await _userService.GetAllUsers();

            if (allUser != null)
            {
                var unLockedUsers = allUser.FindAll(x => x.isBlocked == false && email.Contains(x.Email));

                foreach (var user in unLockedUsers)
                {
                    user.isBlocked = true;
                    await _userService.EditUser(user);
                }
            }

            return RedirectToAction(controllerName: "home", actionName: "index");
        }

        [HttpPost]
        async public Task<IActionResult> DeleteUser(string[] email)
        {
            foreach (var em in email)
            {
                await _userService.DeletUser(em);
            }

            return RedirectToAction(controllerName : "home",actionName : "index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
