using Shared.Domain.Entities;
using Shared.Infrastructure.GerenteNacionalDBContext;
using Trade.Domain.Entities;
using Trade.Domain.Interfaces;


namespace Trade.Infrastructure.Repository.Commands
{
    public class MutationsTrade(GerenteNacionalDbContext context) : ITradeRepository
    {
        private readonly GerenteNacionalDbContext _context = context;

        // Método para inserción en grupo
        public async Task InsertVentasApp(List<VentasAppFromTrade> transactionsTrade)
        {
            try
            {
                foreach (var transactionTrade in transactionsTrade)
                {
                    try
                    {
                        foreach (System.Reflection.PropertyInfo propertyInfo in transactionTrade.GetType().GetProperties())
                        {
                            Console.WriteLine($"Nombre de la propiedad: {propertyInfo.Name}, Tipo de dato: {propertyInfo.PropertyType}, Valor: {propertyInfo.GetValue(transactionTrade)}");
                        }
                        _context.VentasApp.Add(transactionTrade);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        using (StreamWriter sw = new("log.txt", true))
                        {
                            sw.WriteLine($"Error: {ex.Message}");
                        }
                        // Aquí puedes manejar el error como prefieras
                        Console.WriteLine($"Error al insertar la transacción: {ex.Message}");
                    }
                    break;
                }
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        // Método para actualización en grupo
        public async Task UpdateVentasApp(List<VentasAppFromTrade> transactionsTrade)
        {
            foreach (var transactionTrade in transactionsTrade)
            {
                try
                {
                    _context.VentasApp.Update(transactionTrade);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    using (StreamWriter sw = new("log.txt", true))
                    {
                        sw.WriteLine($"Error: {ex.Message}");
                    }
                    // Aquí puedes manejar el error como prefieras
                    Console.WriteLine($"Error al actualizar la transacción: {ex.Message}");
                }
            }
        }

        // Método para eliminación en grupo
        public async Task DeleteVentasApp(List<VentasAppFromTrade> transactionsTrade)
        {
            foreach (var transactionTrade in transactionsTrade)
            {
                try
                {
                    _context.VentasApp.Remove(transactionTrade);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    using (StreamWriter sw = new("log.txt", true))
                    {
                        sw.WriteLine($"Error: {ex.Message}");
                    }
                    // Aquí puedes manejar el error como prefieras
                    Console.WriteLine($"Error al eliminar la transacción: {ex.Message}");
                }
            }
        }
    }
}
