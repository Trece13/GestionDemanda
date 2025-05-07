using GestionDemanda.Application.Interfaces;
using GestionDemanda.Domain.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDemanda.Infrastructure.Simulated
{
    public class PdfGeneratorSimulado : IPdfGenerator
    {
        public Stream GenerarPdf(Solicitud solicitud)
        {
            var contenido = $"PDF generado para: {solicitud.Titulo} - {solicitud.ResponsableNombre}";
            return new MemoryStream(Encoding.UTF8.GetBytes(contenido));
        }
    }
}
