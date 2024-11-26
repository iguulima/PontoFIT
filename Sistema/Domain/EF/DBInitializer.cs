using Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.EF
{
    public class DBInitializer
    {
        public static void Initialize(Context context)
        {
            if (context.Database.EnsureCreated())
            {
                Usuario usuario = new Usuario
                {
                    Login = "admin",
                    Nome = "admin",
                    Senha = "123",
                    TipoUsuario = TipoUsuario.Admin
                };

                context.GetDbSet<Usuario>().Add(usuario);
                context.SaveChanges();
            }
        }
    }
}
