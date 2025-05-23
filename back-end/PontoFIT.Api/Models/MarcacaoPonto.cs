namespace PontoFIT.Api.Models { 
    public class MarcacaoPonto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime? HrEntrada { get; set; }
        public DateTime? HrSaida { get; set; }
        public Usuario Usuario { get; set; }
    }
}