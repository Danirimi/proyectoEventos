using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace proyectoEventos.Modelo
{
    public static class MigradorDatos
    {
        public static void MigrarTodosLosDatos()
        {
            try
            {
                // Verificar conexión a MySQL
                if (!MySQLConexion.ProbarConexion())
                {
                    MessageBox.Show("No se pudo conectar a MySQL. Verifique la configuración.", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int usuariosMigrados = MigrarUsuarios();
                int eventosMigrados = MigrarEventos();
                int ticketsMigrados = MigrarTickets();

                string mensaje = $"Migración completada:\n" +
                                $"- Usuarios migrados: {usuariosMigrados}\n" +
                                $"- Eventos migrados: {eventosMigrados}\n" +
                                $"- Tickets migrados: {ticketsMigrados}";

                MessageBox.Show(mensaje, "Migración Exitosa", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error durante la migración: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static int MigrarUsuarios()
        {
            int totalMigrados = 0;
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            
            // Migrar usuarios normales
            string rutaUsuarios = Path.Combine(baseDir, "Modelo", "Datos", "usuarios.json");
            if (File.Exists(rutaUsuarios))
            {
                string jsonUsuarios = File.ReadAllText(rutaUsuarios);
                var usuarios = JsonConvert.DeserializeObject<List<UsuarioJson>>(jsonUsuarios);
                
                if (usuarios != null)
                {
                    IUsuario repo = new IUsuarioMySQL();
                    foreach (var u in usuarios)
                    {
                        try
                        {
                            var usuario = new Usuario(u.Nombre, u.Correo, u.Cedula, 
                                u.Edad, u.Contrasena, false);
                            repo.AgregarUsuario(usuario);
                            totalMigrados++;
                        }
                        catch (Exception ex)
                        {
                            // Continuar con el siguiente si hay error
                            Console.WriteLine($"Error al migrar usuario {u.Nombre}: {ex.Message}");
                        }
                    }
                }
            }

            // Migrar administradores
            string rutaAdmins = Path.Combine(baseDir, "Modelo", "Datos", "administradores.json");
            if (File.Exists(rutaAdmins))
            {
                string jsonAdmins = File.ReadAllText(rutaAdmins);
                var admins = JsonConvert.DeserializeObject<List<UsuarioJson>>(jsonAdmins);
                
                if (admins != null)
                {
                    IUsuario repo = new IUsuarioMySQL();
                    foreach (var a in admins)
                    {
                        try
                        {
                            var admin = new Usuario(a.Nombre, a.Correo, a.Cedula, 
                                a.Edad, a.Contrasena, true);
                            repo.AgregarUsuario(admin);
                            totalMigrados++;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error al migrar admin {a.Nombre}: {ex.Message}");
                        }
                    }
                }
            }

            return totalMigrados;
        }

        private static int MigrarEventos()
        {
            int totalMigrados = 0;
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string rutaEventos = Path.Combine(baseDir, "Modelo", "Datos", "eventos.json");

            if (File.Exists(rutaEventos))
            {
                string jsonEventos = File.ReadAllText(rutaEventos);
                var eventos = JsonConvert.DeserializeObject<List<Evento>>(jsonEventos);

                if (eventos != null)
                {
                    InterfaceEvento repo = new IEventoMySQL();
                    foreach (var e in eventos)
                    {
                        try
                        {
                            repo.agregarEvento(e);
                            totalMigrados++;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error al migrar evento {e.NombreEvento}: {ex.Message}");
                        }
                    }
                }
            }

            return totalMigrados;
        }

        private static int MigrarTickets()
        {
            int totalMigrados = 0;
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string rutaTickets = Path.Combine(baseDir, "Modelo", "Datos", "tickets.json");

            if (File.Exists(rutaTickets))
            {
                string jsonTickets = File.ReadAllText(rutaTickets);
                var tickets = JsonConvert.DeserializeObject<List<Ticket>>(jsonTickets);

                if (tickets != null)
                {
                    ITicket repo = new ITicketMySQL();
                    foreach (var t in tickets)
                    {
                        try
                        {
                            repo.GenerarTicket(t);
                            totalMigrados++;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error al migrar ticket {t.id}: {ex.Message}");
                        }
                    }
                }
            }

            return totalMigrados;
        }

        // Clase auxiliar para deserializar usuarios desde JSON
        private class UsuarioJson
        {
            public string Nombre { get; set; }
            public string Correo { get; set; }
            public string Cedula { get; set; }
            public int Edad { get; set; }
            public string Contrasena { get; set; }
            public int id { get; set; }
            public bool esadmin { get; set; }
        }
    }
}