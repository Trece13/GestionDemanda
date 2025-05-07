using GestionDemanda.Application.Interfaces;
using GestionDemanda.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDemanda.Infrastructure.Simulated
{
    public class AzureDevOpsServiceSimulado : IAzureDevOpsService
    {
        public Task<string> CrearWorkItemAsync(Solicitud solicitud, string urlPdf)
        {
            // Simula la creación y devuelve un ID de WorkItem
            return Task.FromResult($"#WID{solicitud.Id[..6]}");
        }
    }
}
