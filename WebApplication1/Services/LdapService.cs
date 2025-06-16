namespace WebApplication1.Services;
using Microsoft.Extensions.Configuration;
using Novell.Directory.Ldap;
using WebApplication1.Models;

public class LdapService
{
    private readonly string _ldapHost;
    private readonly int _ldapPort;
    private readonly string _ldapBaseDn;
    private readonly string _ldapUsuariosBaseDn;
    private readonly string _ldapAdminUser;
    private readonly string _ldapAdminPassword;

    public LdapService(IConfiguration configuration)
    {
        var ldapSettings = configuration.GetSection("LdapSettings");

        _ldapHost = ldapSettings["LdapHost"];
        _ldapPort = int.Parse(ldapSettings["LdapPort"]);
        _ldapBaseDn = ldapSettings["LdapBaseDn"];
        _ldapUsuariosBaseDn = ldapSettings["LdapGrupoUsuarios"];
        _ldapAdminUser = ldapSettings["LdapAdminUser"];
        _ldapAdminPassword = ldapSettings["LdapAdminPassword"];
    }

    public bool AutenticarUsuario(string username, string password)
    {
        try
        {
            // Crear una conexión LDAP
            LdapConnection conn = new LdapConnection();
            conn.Connect(_ldapHost, _ldapPort);

            string dn = $"uid={username},ou=usuarios,dc=asociadosgerenciales,dc=com";

            // Bind (autenticación)
            conn.Bind(dn, password);

            // Si la autenticación es exitosa, el Bind no lanza una excepción
            conn.Disconnect();
            return true;
        }
        catch (LdapException ex)
        {
            // Capturar excepciones de autenticación (credenciales inválidas, etc.)
            Console.WriteLine("Error de autenticación: " + ex.Message);
            Console.WriteLine(ex.StackTrace);
            return false;
        }
    }

    public List<Empleado> BuscarUsuarios(string? nombre = null, string? correo = null)
    {
        try
        {
            List<Empleado> empleados = new List<Empleado>();

            // Crear una conexión LDAP
            LdapConnection conn = new LdapConnection();
            conn.Connect(_ldapHost, _ldapPort);

            // Bind (autenticación como administrador)
            conn.Bind(_ldapAdminUser, _ldapAdminPassword);

            // Crear una búsqueda
            LdapSearchConstraints constraints = new LdapSearchConstraints();
            constraints.TimeLimit = 30000; // Tiempo límite de 30 segundos

            // Definir filtro de búsqueda dinámico
            string filtro;
            if (!string.IsNullOrEmpty(correo))
            {
                // Búsqueda exacta por correo
                filtro = $"(mail={correo})";
            }
            else if (!string.IsNullOrEmpty(nombre))
            {
                // Búsqueda parcial por nombre o apellido (contiene el valor)
                filtro = $"(|(givenName=*{nombre}*)(sn=*{nombre}*))";
            }
            else
            {
                // Si no hay parámetros, buscar todos los usuarios
                filtro = "(objectClass=inetOrgPerson)";
            }

            string[] atributos = new string[] { "givenName", "sn", "cn", "mail", "uid" };
            var results = conn.Search(_ldapBaseDn, LdapConnection.ScopeSub, filtro, null, false);

    
            // Recopilar resultados
            while (results.HasMore())
            {

                LdapEntry entry = results.Next();
                try
                {
                    Empleado empleado = new Empleado
                    {
                        Nombre = entry.GetAttribute("givenName")?.StringValue ?? "N/A",
                        Apellido = entry.GetAttribute("sn")?.StringValue ?? "N/A",
                        Usuario = entry.GetAttribute("uid")?.StringValue ?? "N/A",
                        Email = entry.GetAttribute("mail")?.StringValue ?? "N/A"
                    };
                    empleados.Add(empleado);
                }
                catch (KeyNotFoundException ex)
                {
                }
              
               
            }

           
            conn.Disconnect();
            return empleados;
        }
        catch (LdapException ex)
        {
            Console.WriteLine($"Error en la búsqueda LDAP: {ex.Message}");
            Console.WriteLine($"Código de error: {ex.ResultCode}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
            return new List<Empleado>();
        }
    }
}