using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace PontoFIT.Data {
    public class PontoFitContext : DbContext {
        public PontoFitContext(DbContextOptions<PontoFitContext> options) : base(options) { }

        public DbSet<Pessoa> Pessoa { get; set; }
        public DbSet<Cargos> Cargos { get; set; }
        public DbSet<Turnos> Turnos { get; set; }

    }

    public class Pessoa {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public DateTime DataDeNascimento { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public int CargoId { get; set; }  // Chave estrangeira para Cargo
        public Cargo Cargo { get; set; }
        public int HorarioId { get; set; }  // Chave estrangeira para Horário
        public Horario Horario { get; set; }
        public DateTime DataDeAdmissao { get; set; }
        public DateTime? DataDeTermino { get; set; }
        public bool Ativo { get; set; }
    }

    public class Cargos {
        public int Id { get; set; }
        public string Nome { get; set; }
    }

    public class Turnos {
        public int Id { get; set; }
        public string Nome { get; set; }
    }

}