using Microsoft.EntityFrameworkCore;
using Shared.Domain.Criteria;
using Shared.Domain.Entities;
using Shared.Infrastructure.GerenteNacionalDBContext;
using Shared.Infrastructure.Repository;
using Trade.Domain.Entities;

namespace Trade.Infrastructure.Repository.Querys
{

    public class VentasAppRepository(GerenteNacionalDbContext context)
    {
        private readonly GerenteNacionalDbContext _context = context;

        public async Task<List<VentasAppFromTrade>> QueryData()
        {
            try
            {
                // Realiza la consulta a la base de datos
                var ventasApps = await _context.VentasApp.Take(3).ToListAsync();

                // Procesa los resultados si es necesario

                return ventasApps;
            }
            catch (Exception)
            {
                //Console.WriteLine(ex.Message);
                // Manejar la excepción según tus necesidades
                return []; // Otra opción sería lanzar la excepción nuevamente si es necesario
            }
        }

        public async Task<List<VentasApp>> SearchTransactionsByDate(DateTime date)
        {
            try
            {
                // Obtener el context de la base de datos
                var ventasAppAsQuery = _context.VentasApp.AsQueryable();

                var filterByDate = new Filter
                {
                    Field = "Fecha",
                    ValueField = date,
                    OperatorField = "Equal"
                };

                // var filterByEmail = new Filter
                // {
                //     Field = "correo",
                //     ValueField = "ilinkzzx@gmail.com",
                //     OperatorField = "Equal"
                // };

                var Filters = new Filter[] { filterByDate };

                var limitsTransactions = 1;
                
                var Criteria = new Criteria<VentasApp>(Filters, limitsTransactions);

                var CriteriaConverter = new EntityFrameworkCriteriaConverter<VentasApp>(Criteria);

                // Obtiene la consulta
                var query = _context.VentasApp.AsQueryable();
                // Aplica los criterios a la consulta
                var filteredQuery = CriteriaConverter.ApplyCriteria(query);
                // Ejecuta la consulta
                var transactionsVentasApps = await filteredQuery.ToListAsync();

                return transactionsVentasApps;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fallo exitosamente: " + ex.Message);
                // Manejar la excepción según tus necesidades
                return []; // Otra opción sería lanzar la excepción nuevamente si es necesario
            }
        }


    }
}
