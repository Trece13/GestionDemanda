using GestionDemanda.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDemanda.Application.Interfaces
{
    public interface IRegistrarSolicitudHandler
    {
        Task<string> HandleAsync(SolicitudDto dto);
    }
}
