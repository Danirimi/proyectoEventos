public class Usuario
{
    // ❌ ELIMINAR ESTO:
    // private static int contadorIds = 0;

    public int id { get; set; }  // ✅ Cambiar de 'private set' a 'set'
    public string Nombre { get; set; }
    public string Correo { get; set; }
    public string Cedula { get; set; }
    public int Edad { get; set; }
    public string Contrasena { get; set; }
    public bool esadmin { get; set; }

    // Constructor para crear usuario NUEVO (sin id)
    public Usuario(string Nombre, string Correo, string Cedula, int Edad, string Contrasena, bool Esadmin)
    {
        this.id = 0;  // ✅ 0 indica que es nuevo, la BD asignará id
        this.Nombre = Nombre;
        this.Correo = Correo;
        this.Cedula = Cedula;
        this.Edad = Edad;
        this.Contrasena = Contrasena;
        this.esadmin = Esadmin;
    }

    // Constructor COMPLETO (con id desde BD)
    public Usuario(int id, string Nombre, string Correo, string Cedula, int Edad, string Contrasena, bool Esadmin)
    {
        this.id = id;  // ✅ Usar id real de la BD
        this.Nombre = Nombre;
        this.Correo = Correo;
        this.Cedula = Cedula;
        this.Edad = Edad;
        this.Contrasena = Contrasena;
        this.esadmin = Esadmin;
    }

    // Constructor vacío
    public Usuario() { }
}