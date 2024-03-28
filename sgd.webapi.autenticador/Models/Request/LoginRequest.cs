namespace sgd.webapi.autenticador.Models.Request
{
    public class LoginRequest
    {
        public string? AplicacionCodigo { get; set; }
        public string? UsuarioId { get; set; }
        public string? Contraseña { get; set; }
    }
}
