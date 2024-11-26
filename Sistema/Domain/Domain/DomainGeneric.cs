using Domain.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Domain.Domain
{
    public abstract class DomainGeneric<T> where T : class
    {
        public Dictionary<string, string> Salvar(Context context)
        {
            Dictionary<string, string> erros = ValidaSalvar((T)(object)this);

            if (erros.Count == 0)
            {
                context.GetDbSet<T>().Add((T)(object)this);
                context.SaveChanges();
            }

            return erros;
        }

        public Dictionary<string, string> Alterar(Context context)
        {
            Dictionary<string, string> erros = ValidaAlterar((T)(object)this);

            if (erros.Count == 0)
            {
                context.Entry((T)(object)this).State = EntityState.Modified;
                context.SaveChanges();
            }

            return erros;
        }

        public Dictionary<string, string> Remover(Context context)
        {
            Dictionary<string, string> erros =
                new Dictionary<string, string>();

            erros = ValidaRemover((T)(object)this);

            if (erros.Count == 0)
            {
                context.GetDbSet<T>().Remove((T)(object)this);
                context.SaveChanges();
            }

            return erros;
        }

        public T BuscarPorId(Context context, int id)
        {
            var objectDomain = context.GetDbSet<T>().Find(id);
            return objectDomain;
        }

        public List<T> BuscarTodos(Context context)
        {
            List<T> objects = context.GetDbSet<T>().ToList();
            return objects;
        }

        internal abstract Dictionary<string, string> ValidaSalvar(T objectDomain);

        internal abstract Dictionary<string, string> ValidaAlterar(T objectDomain);

        internal abstract Dictionary<string, string> ValidaRemover(T objectDomain);
    }
}
