using Domain.Domain;
using Domain.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistema.Controllers {
    public class UsuarioController : Controller {
        private Context _context;

        public UsuarioController(Context context) {
            _context = context;
        }

        // Acesso restrito a Admins
        [Authorize(Roles = "Admin")]
        public ActionResult Index() {
            Usuario usuario = new Usuario();
            List<Usuario> usuarios = usuario.BuscarTodos(_context);
            return View(usuarios);
        }

        // Acesso restrito a Admins
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id) {
            Usuario usuario = new Usuario().BuscarPorId(_context, id);
            return View(usuario);
        }

        // Acesso restrito a Admins
        [Authorize(Roles = "Admin")]
        public ActionResult Create() {
            ViewBag.TiposUsuarioList = getTiposUsuarioList();
            return View();
        }

        // Criação de Usuário
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario usuario) {
            try {
                Dictionary<string, string> erros = usuario.Salvar(_context);

                if (erros.Count == 0) {
                    return RedirectToAction(nameof(Index));
                }
                else {
                    foreach (var erro in erros) {
                        ModelState.AddModelError(erro.Key, erro.Value);
                    }
                    return View();
                }
            }
            catch {
                return View();
            }
        }

        // Acesso restrito a Admins
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id) {
            ViewBag.TiposUsuarioList = getTiposUsuarioList();
            Usuario usuario = new Usuario().BuscarPorId(_context, id);
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Usuario usuarioAntigo) {
            try {
                Usuario usuarioAlterado = new Usuario().BuscarPorId(_context, usuarioAntigo.Id);

                usuarioAlterado.Nome = usuarioAntigo.Nome;
                usuarioAlterado.Login = usuarioAntigo.Login;

                Dictionary<string, string> erros = usuarioAlterado.Alterar(_context);
                if (erros.Count == 0)
                    return RedirectToAction(nameof(Index));
                else {
                    foreach (var erro in erros) {
                        ModelState.AddModelError(erro.Key, erro.Value);
                    }
                    return View();
                }
            }
            catch {
                return View();
            }
        }

        // Acesso restrito a Admins
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id) {
            Usuario usuario = new Usuario().BuscarPorId(_context, id);
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id) {
            try {
                Usuario usuario = new Usuario().BuscarPorId(_context, id);
                usuario.Remover(_context);
                return RedirectToAction(nameof(Index));
            }
            catch {
                return View();
            }
        }

        [Authorize(Roles = "Cliente")]
        public ActionResult BaterPonto() {
            return View();
        }

        private List<SelectListItem> getTiposUsuarioList() {
            List<TipoUsuario> tiposUsuario = Enum.GetValues(typeof(TipoUsuario)).Cast<TipoUsuario>().ToList();

            var TiposUsuarioList = tiposUsuario
                .Select(c => new SelectListItem() { Text = c.ToString(), Value = c.ToString() })
                .ToList();

            return TiposUsuarioList;
        }
    }
}
