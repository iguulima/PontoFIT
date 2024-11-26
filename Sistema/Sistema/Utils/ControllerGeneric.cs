using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Sistema.Controllers
{
    public abstract class ControllerGeneric: Controller
    {
        public void InjetaErros(Dictionary<string, string> erros)
        {
            foreach (var erro in erros)
            {
                ModelState.AddModelError(erro.Key, erro.Value);
            }
        }
    }
}
