namespace RackDAT_API.Contracts
{
    public class UsuarioResponse
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string apellido_pat { get; set; }
        public string apellido_mat { get; set; }
        public string correo { get; set; }
        public string clave { get; set; }
        public int tipoUsuario { get; set; }
        public int carrera { get; set; }

    }

}
