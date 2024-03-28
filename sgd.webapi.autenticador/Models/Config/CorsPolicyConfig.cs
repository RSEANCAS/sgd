namespace sgd.webapi.autenticador.Models.Config
{
    public class CorsPolicyConfig
    {
        public string Name { get; set; }
        public string[] Origins { get; set; }
        public string[] Headers { get; set; }
        public string[] Methods { get; set; }
    }
}
