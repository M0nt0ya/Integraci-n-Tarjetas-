using Kiosko.Domain.Entities;
using Kiosko.Domain.Repository;

namespace Kiosko.Application.Services
{
    public class KioskoServicesMethods
    {
        private readonly IKioskoRepository _kioskoRepository;

        public KioskoServicesMethods(IKioskoRepository kioskoRepository)
        {
            _kioskoRepository = kioskoRepository;
        }

        public void CreateKiosko(KioskoApp kiosko)
        {
            _kioskoRepository.CreateKiosko(kiosko);
        }

        public void UpdateKiosko(KioskoApp kiosko)
        {
            _kioskoRepository.UpdateKiosko(kiosko);
        }

        public KioskoApp GetByIdKiosko(int kioskoId)
        {
            return _kioskoRepository.GetById(kioskoId);
        }

        public List<KioskoApp> GetAllKioskos()
        {
            return _kioskoRepository.GetAllKioskos();
        }
        
    }
}
