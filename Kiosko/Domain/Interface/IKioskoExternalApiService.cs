using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kiosko.Domain.Entities;

public interface IKioskoExternalApiService
{
    Task<List<KioskoApp>> GetVentas(DateTime fechaInicio, DateTime fechaFin);
}


