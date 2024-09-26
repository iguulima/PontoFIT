using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller {
    [HttpGet]
    public IActionResult Login() {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password) {
        // Aqui você pode implementar lógica de processamento de login, se necessário

        // Para fins de demonstração, apenas redireciona para a página inicial
        return RedirectToAction("Index", "Home");
    }
}
