using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.GerenteNacionalDBContext;
using System;

namespace Kiosko.Domain.Entities
{
    public class KioskoApp : ITransactionKiosko
    {
        public int Id { get; set; }
        public int IdLocal { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public string Clave { get; set; }
        public string Puerto { get; set; }
        public string TokenAcceso { get; set; }
        public DateTime TokenFechaCaduca { get; set; }
        public DateTime TokenFechaCreado { get; set; }
        public int Estado { get; set; }

        public const string ENDPOINT_CONSULTAR_VENTAS = "/api/reportes/ventas-switch";
        public const string ENDPOINT_TOKEN = "/api/login";

        public bool TieneToken()
        {
            return !string.IsNullOrEmpty(TokenAcceso) && TokenAcceso != null;
        }

        public bool TokenCaducado()
        {
            if (TokenFechaCaduca == null)
                return true;
            return DateTime.Now > TokenFechaCaduca;
        }

        public string URLServidor()
        {
            var port = string.IsNullOrEmpty(Puerto) ? "" : $":{Puerto}";
            return $"http://{Direccion}{port}";
        }

        public string CrearURL(string ruta = "/")
        {
            var urlServidorConRuta = URLServidor() + ruta;
            Console.WriteLine($"CREARURL_KIOSKO() = {urlServidorConRuta}");
            return urlServidorConRuta;
        }

        public bool TokenValido()
        {
            return TieneToken() && !TokenCaducado();
        }

        public void Save()
        {
            var optionsBuilder = new DbContextOptionsBuilder<GerenteNacionalDbContext>();
            optionsBuilder.UseSqlServer("cadena de conexión a tu base de datos");

            using (var dbContext = new GerenteNacionalDbContext(optionsBuilder.Options))
            {
                var entry = dbContext.Entry(this);
                if (entry.State == EntityState.Detached)
                {
                    dbContext.Set<KioskoApp>().Attach(this);
                    entry.State = EntityState.Modified;
                }

                dbContext.SaveChanges();
            }
        }
    }


}
