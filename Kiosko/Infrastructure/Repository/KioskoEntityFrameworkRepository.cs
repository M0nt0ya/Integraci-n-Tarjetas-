using System.Collections.Generic;
using System.Linq;
using Kiosko.Domain.Entities;
using Kiosko.Domain.Repository;
using Shared.Infrastructure.GerenteNacionalDBContext;

namespace Kiosko.Infrastructure.Repository
{
    public class KioskoEntityFrameworkRepository : IKioskoRepository
    {
        private readonly GerenteNacionalDbContext _dbContext;

        public KioskoEntityFrameworkRepository(GerenteNacionalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateKiosko(KioskoApp kiosko)
        {
            _dbContext.Kioskos.Add(kiosko);
            _dbContext.SaveChanges();
        }

        public void UpdateKiosko(KioskoApp kiosko)
        {
            _dbContext.Kioskos.Attach(kiosko);
            _dbContext.Entry(kiosko).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public KioskoApp GetById(int kioskoId)
        {
            return _dbContext.Kioskos.Find(kioskoId);
        }

        public List<KioskoApp> GetAllKioskos()
        {
            return _dbContext.Kioskos.ToList();
        }
    }
}
