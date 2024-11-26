using Domain.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.EF
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }


        public DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class
        {
            return Set<TEntity>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Usuario>(t => {
                t.ToTable("Usuarios");
                t.Property(t => t.Id)
                    .HasColumnType("int")
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                t.Property(t => t.Nome)
                    .HasColumnType("varchar(50)")
                    .IsRequired();

                t.Property(t => t.Login)
                    .HasColumnType("varchar(20)")
                    .IsRequired();

                t.Property(t => t.Senha)
                    .HasColumnType("varchar(10)")
                    .IsRequired();

                t.Property(t => t.TipoUsuario)
                    .HasColumnType("varchar(10)")
                    .IsRequired();

                t.HasMany(t => t.UsuarioMarcacao)
                    .WithOne(m => m.Usuario)
                    .HasForeignKey(m => m.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade); // Exclusão em cascata
            });

            // Configuração da tabela Marcacao
            modelBuilder.Entity<Marcacao>(t => {
                t.ToTable("Marcacao");
                t.Property(t => t.Id)
                    .HasColumnType("int")
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                t.Property(t => t.HoraMarcacao)
                    .HasColumnType("datetime")
                    .IsRequired();

                t.HasOne(t => t.Usuario)
                    .WithMany(u => u.UsuarioMarcacao)
                    .HasForeignKey(t => t.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade); // Exclusão em cascata
            });
        }
    }
}
