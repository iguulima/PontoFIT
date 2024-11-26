using Domain.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Domain
{
	public class Marcacao: DomainGeneric<Marcacao>
	{
		public int Id { get; set; }
        public DateTime HoraMarcacao { get; set; }
		public virtual Usuario Usuario { get; set; }
		public int? UsuarioId { get; set; }
        public string Tipo { get; set; }


        private Dictionary<string, string> Valida(Marcacao alimento)
		{
            Dictionary<string, string> erros =
                new Dictionary<string, string>();

            if (Usuario == null && UsuarioId == null)
            {
                erros.Add("Categoria", "Valor inválido");
            }

            return erros;
        }

        internal override Dictionary<string, string> ValidaSalvar(Marcacao alimento)
        {
            return Valida(alimento);
        }

        internal override Dictionary<string, string> ValidaAlterar(Marcacao alimento)
        {
            return Valida(alimento);
        }

        internal override Dictionary<string, string> ValidaRemover(Marcacao objectDomain)
        {
            return new Dictionary<string, string>();
        }
    }
}
