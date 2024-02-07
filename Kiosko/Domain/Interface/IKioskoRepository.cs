using Kiosko.Domain.Entities;
using System.Collections.Generic;

namespace Kiosko.Domain.Repository
{
    public interface IKioskoRepository
    {
        void CreateKiosko(KioskoApp kioskoApp);
        void UpdateKiosko(KioskoApp kioskoApp);
        KioskoApp GetById(int kioskoApp);
        List<KioskoApp> GetAllKioskos();
    }
}
