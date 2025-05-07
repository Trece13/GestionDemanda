using GestionDemanda.Application.Interfaces;
using GestionDemanda.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDemanda.Infrastructure.Simulated
{
    public class EmailServiceSimulado : IEmailService
    {
        public Task EnviarConfirmacionAsync(Solicitud solicitud, string pdfUrl)
        {
            Console.WriteLine($"📨 Simulando envío de correo a {solicitud.ResponsableCorreo} con link al PDF: {pdfUrl}");
            return Task.CompletedTask;
        }
    }
}
