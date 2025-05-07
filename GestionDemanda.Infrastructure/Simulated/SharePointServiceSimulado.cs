using GestionDemanda.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDemanda.Infrastructure.Simulated
{
    public class SharePointServiceSimulado : ISharePointService
    {
        public Task<string> SubirArchivoAsync(string nombreArchivo, Stream contenido)
        {
            // Simula un URL en SharePoint
            return Task.FromResult($"https://fake.sharepoint.com/sites/gestion/{nombreArchivo}.pdf");
        }
    }
}
