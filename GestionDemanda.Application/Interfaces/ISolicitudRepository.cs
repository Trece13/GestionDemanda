using GestionDemanda.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestionDemanda.Shared.DTOs;

namespace GestionDemanda.Application.Interfaces
{
    public interface ISolicitudRepository
    {
        Task GuardarAsync(Solicitud solicitud);
        Task<IEnumerable<Solicitud>> ObtenerTodasAsync();
        Task<(IEnumerable<Solicitud> resultados, int total)> ConsultarAsync(SolicitudFiltroDto filtro);

    }
}
