using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using RestSharp;
using System.Configuration;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using WebProject.interfaces;
using WebProject.interfaces.auth;
using WebProject.interfaces.Repositories;
using WebProject.Models.auth;
using WebProject.Models.Requests;
using WebProject.Provaiders;
using WebProject.Repositories;
using WebProject.Services;

namespace WebProject.Controllers
{
    public class LoginController : Controller
    {
        private UserService _userService {  get; set; }

        public LoginController([FromServices]UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            try
            {
                string connect = "server=mysql-container;port=3306;user=root;password=root;database=itransition_task4";
                MySqlConnection mySqlConnection = new MySqlConnection(connect);
                mySqlConnection.Open();
                Console.WriteLine("Chotki");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return View();
        }

        [HttpPost]
        async public Task<IActionResult> Login(UserLoginRequest loginRequest)
        {
            LoginModel model = new LoginModel(loginRequest, ModelState);
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            CookieOptions cookieOptions = new CookieOptions()
            {
                Expires = DateTimeOffset.Now.AddDays(2),
            };

            string? token = await _userService.Login(loginRequest);

            if (token == null)
            {
                model.Errors["Email"] = "No such username or password is incorrect or you are blocked";
                model.Errors["Password"] = "Enter password";
                return View(model);
            }

            Response.Cookies.Append("loginData", token, cookieOptions);

            return RedirectToAction(controllerName: "home", actionName: "index");
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        async public Task<IActionResult> Registration(UserRegistrationRequest registrationRequest)
        {
            RegistrationModel model = new RegistrationModel(registrationRequest, ModelState);
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool isRegistreted = await _userService.Registration(registrationRequest);

            if (!isRegistreted)
            {
                model.Errors["Email"] = "Email adress is already exists";
                return View(model);
            }

            return RedirectToAction(controllerName: "login", actionName: "login");
        }
    }
}
