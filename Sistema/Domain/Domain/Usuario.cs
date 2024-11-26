using Domain.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Domain
{
    public class Usuario: DomainGeneric<Usuario> { 
    
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Login {  get; set; }
        public string Senha { get; set; }
        public TipoUsuario TipoUsuario { get; set; }
        public virtual List<Marcacao> UsuarioMarcacao { get; set; }


        public Usuario Entrar(Context context, string login, string senha)
        {
            Usuario usuario = context.GetDbSet<Usuario>().
                Where(linha => linha.Login.Equals(login) && linha.Senha.Equals(senha)).
                FirstOrDefault();
            return usuario;
        }

        internal override Dictionary<string, string> ValidaSalvar(Usuario usuario)
        {
            return Valida(usuario);
        }   

        internal override Dictionary<string, string> ValidaRemover(Usuario usuario)
        {
            return new Dictionary<string, string>();
        }

        private Dictionary<string, string> Valida(Usuario usuario)
        {
            Dictionary<string, string> erros =
                new Dictionary<string, string>();

            if (String.IsNullOrEmpty(Nome))
            {
                erros.Add("Nome", "Valor inválido");
            }
            if (!String.IsNullOrEmpty(Nome) && Nome.Length > 50)
            {
                erros.Add("Nome", "Este campo deve possuir no máximo 50 caracteres");
            }

            if (String.IsNullOrEmpty(Login))
            {
                erros.Add("Login", "Valor inválido");
            }
            if (!String.IsNullOrEmpty(Login) && Login.Length > 20)
            {
                erros.Add("Login", "Este campo deve possuir no máximo 20 caracteres");
            }

            if (String.IsNullOrEmpty(Senha))
            {
                erros.Add("Senha", "Valor inválido");
            }
            if (!String.IsNullOrEmpty(Senha) && Senha.Length > 10)
            {
                erros.Add("Senha", "Este campo deve possuir no máximo 10 caracteres");
            }
            if (!TipoUsuario.IsDefined(TipoUsuario))
            {
                erros.Add("TipoUsuario", "Este campo deve possuir um tipo");
            }
            

            return erros;
        }

        internal override Dictionary<string, string> ValidaAlterar(Usuario objectDomain)
        {
            return Valida(this);
        }
    }

    public enum TipoUsuario
    {
        Admin,
        Cliente
    }
}
