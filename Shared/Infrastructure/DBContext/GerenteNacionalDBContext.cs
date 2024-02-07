using Kiosko.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Entities;
using Trade.Domain.Entities;


namespace Shared.Infrastructure.GerenteNacionalDBContext
{
    public class GerenteNacionalDbContext(DbContextOptions<GerenteNacionalDbContext> options) : DbContext(options)
    {
        public DbSet<VentasAppFromTrade> VentasApp { get; set; }
        public DbSet<KioskoApp> Kioskos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //INICIO Modelo VentasApp
            //modelBuilder.Entity<VentasAppFromTrade>().HasNoKey();
            modelBuilder.Entity<KioskoApp>().HasKey(k => k.Id);



            modelBuilder.Entity<VentasAppFromTrade>()
                        .HasKey(m => new { m.Cod_Autorizacion });

            modelBuilder.Entity<VentasAppFromTrade>()
                        .Property(v => v.Valor)
                        .HasColumnType("decimal(18,2)");
            //FIN Modelo VentasApp
            // Otras configuraciones del modelo...

            base.OnModelCreating(modelBuilder);
        }
    }
}
