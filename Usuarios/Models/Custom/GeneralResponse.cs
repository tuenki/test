namespace Usuarios.Models.Custom
{
    public class GeneralResponse
    {
        public bool StatusCode { get; set; }
        public string Msg { get; set; }

        public object? Data { get; set; }
    }
}
