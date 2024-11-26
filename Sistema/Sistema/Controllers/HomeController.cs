using Domain.Domain;
using Domain.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sistema.Models;
using Sistema.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;

namespace Sistema.Controllers
{
    public class HomeController : Controller
    {

        private Context _context;

        public HomeController(Context context)
		{
			_context = context;
		}

        [Authorize(Roles = "Admin, Cliente")]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(string Login, string Senha)
        {
            bool falha = false;
			if (string.IsNullOrWhiteSpace(Login))
			{
				ModelState.AddModelError("Login", "Usuário inválido");
                falha = true;
			}

			if (string.IsNullOrWhiteSpace(Senha))
			{
				ModelState.AddModelError("Senha", "Senha inválido");
				falha = true;
			}

			if (falha)
			{
				return View();
			}


            Usuario usuario = new Usuario().Entrar(_context, Login, Senha);

            if (usuario != null)
            {
                 new ServiceCookie().Login(HttpContext, usuario);
                 return RedirectToAction("Index");
            }
            else
            {
			    return View();
			}
	
        }

        public ActionResult Logout()
        {
            new ServiceCookie().Logout(HttpContext);
            return RedirectToAction("Login");
        }

        [Authorize(Roles = "Cliente")]
        [HttpPost]
        public IActionResult BaterPonto() {
            try {
                string userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int userId = int.Parse(userIdStr);

                Marcacao ponto = new Marcacao {
                    UsuarioId = userId,
                    HoraMarcacao = DateTime.Now
                };

                Dictionary<string, string> erros = ponto.Salvar(_context);

                TempData["Mensagem"] = "Ponto registrado com sucesso!";
            }
            catch (Exception ex) {
                TempData["Erro"] = "Erro ao registrar o ponto: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

    }
}
