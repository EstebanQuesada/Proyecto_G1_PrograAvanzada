using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo1.Models;
using Dapper;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using ProyectoGrupo1.Services;

namespace ProyectoGrupo1.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbService _dbService;

        public HomeController(DbService dbService)
        {
            _dbService = dbService;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UsuarioID") == null)
                return RedirectToAction("Login", "Usuario");

            return View(); 
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Usuario");
        }

        private string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return string.Empty;

            using var sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
