using GestionDemanda.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDemanda.Application.Interfaces
{
    public interface IPdfGenerator
    {
        Stream GenerarPdf(Solicitud solicitud);
    }
}
