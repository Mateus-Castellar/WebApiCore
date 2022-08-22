namespace AppCore.API.Extensions
{
    public class AppSettings
    {
        public string Secret { get; set; }//chave de criptografia do token
        public int ExpiracaoHoras { get; set; }
        public string Emissor { get; set; }
        public string ValidoEm { get; set; }//em quais Urls o token será valido
    }
}
