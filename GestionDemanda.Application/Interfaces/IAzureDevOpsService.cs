using GestionDemanda.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDemanda.Application.Interfaces
{
    public interface IAzureDevOpsService
    {
        Task<string> CrearWorkItemAsync(Solicitud solicitud, string urlPdf);
    }
}
