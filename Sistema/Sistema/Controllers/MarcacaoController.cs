using Domain.Domain;
using Domain.Domain.viewsmodel;
using Domain.EF;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sistema.Controllers {
    public class MarcacaoController : Controller {
        private readonly Context _context;

        public MarcacaoController(Context context) {
            _context = context;
        }

        public ActionResult Index() {
            List<Marcacao> marcacoes = new Marcacao().BuscarTodos(_context);

            List<Usuario> usuarios = new Usuario().BuscarTodos(_context);

            List<MarcacaoViewModel> marcacoesViewModel = marcacoes.Select(m => new MarcacaoViewModel {
                Id = m.Id,
                NomeUsuario = usuarios.FirstOrDefault(u => u.Id == m.UsuarioId)?.Nome ?? "Usuário Desconhecido"
            }).ToList();

            return View(marcacoes);
        }


        public ActionResult Details(int id) {
            Marcacao marcacao = new Marcacao().BuscarPorId(_context, id);
            return View(marcacao);
        }

        public ActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Marcacao marcacao) {
            try {
                Dictionary<string, string> erros = marcacao.Salvar(_context);
                if (erros.Count == 0) {
                    return RedirectToAction(nameof(Index));
                }
                else {
                    InjetaErros(erros);
                    return View(marcacao);
                }
            }
            catch {
                return View();
            }
        }

        public ActionResult Edit(int id) {
            Marcacao marcacao = new Marcacao().BuscarPorId(_context, id);
            return View(marcacao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Marcacao marcacaoAntiga) {
            try {
                Marcacao marcacaoAlterada = new Marcacao().BuscarPorId(_context, id);

                marcacaoAlterada.HoraMarcacao = marcacaoAntiga.HoraMarcacao;
                marcacaoAlterada.UsuarioId = marcacaoAntiga.UsuarioId;

                Dictionary<string, string> erros = marcacaoAlterada.Alterar(_context);

                if (erros.Count == 0) {
                    return RedirectToAction(nameof(Index));
                }
                else {
                    InjetaErros(erros);
                    return View(marcacaoAlterada);
                }
            }
            catch {
                return View();
            }
        }

        public ActionResult Delete(int id) {
            Marcacao marcacao = new Marcacao().BuscarPorId(_context, id);
            return View(marcacao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            try {
                Marcacao marcacao = new Marcacao().BuscarPorId(_context, id);

                Dictionary<string, string> erros = marcacao.Remover(_context);

                if (erros.Count == 0) {
                    return RedirectToAction(nameof(Index));
                }
                else {
                    InjetaErros(erros);
                    return View(marcacao);
                }
            }
            catch {
                return View();
            }
        }

        private void InjetaErros(Dictionary<string, string> erros) {
            foreach (var erro in erros) {
                ModelState.AddModelError(erro.Key, erro.Value);
            }
        }
        public IActionResult ExportarParaExcel() {
            var marcacoes = new Marcacao().BuscarTodos(_context);

            using (var package = new ExcelPackage()) {
                var worksheet = package.Workbook.Worksheets.Add("Marcações");
                worksheet.Cells[1, 1].Value = "Hora da Marcação";
                worksheet.Cells[1, 2].Value = "Codigo";
                worksheet.Cells[1, 3].Value = "Nome";
                worksheet.Cells[1, 4].Value = "Cargo";
                worksheet.Cells[1, 5].Value = "Tipo";
                worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column].Style.Font.Bold = true;



                int row = 2;
                foreach (var marcacao in marcacoes) {
                    worksheet.Cells[row, 1].Value = marcacao.HoraMarcacao.ToString("dd/MM/yyyy HH:mm");
                    worksheet.Cells[row, 2].Value = marcacao.Usuario.Id;
                    worksheet.Cells[row, 3].Value = @marcacao.Usuario.Nome;
                    worksheet.Cells[row, 4].Value = @marcacao.Usuario.TipoUsuario;
                    worksheet.Cells[row, 5].Value = marcacao.Tipo;
                    row++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);

                stream.Position = 0; 
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Lista das Marcações.xlsx");
            }

        }
    }
}
