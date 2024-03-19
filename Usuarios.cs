namespace CapaDatos
{
    public class Usuarios
    {
        public byte[] fotoBytes;
        public object Id;

        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Rol { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public byte[] Foto { get; set; }
        public int Estado { get; set; }
        public string NombreUsuario { get; set; }
        public string Password { get; set; }
    }
}