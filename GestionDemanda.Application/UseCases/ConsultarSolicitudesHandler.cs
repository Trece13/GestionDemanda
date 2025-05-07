using GestionDemanda.Application.Interfaces;
using GestionDemanda.Domain.Entities;
using GestionDemanda.Shared.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionDemanda.Application.UseCases
{
    public class ConsultarSolicitudesHandler
    {
        private readonly ISolicitudRepository _repository;

        public ConsultarSolicitudesHandler(ISolicitudRepository repository)
        {
            _repository = repository;
        }

        // Consulta con filtros y paginación
        public async Task<(IEnumerable<SolicitudDto> Resultados, int Total)> HandleAsync(SolicitudFiltroDto filtro)
        {
            var (solicitudes, total) = await _repository.ConsultarAsync(filtro);

            var resultados = solicitudes.Select(s => new SolicitudDto
            {
                Id = s.Id,
                Titulo = s.Titulo,
                Descripcion = s.Descripcion,
                FechaCreacion = s.FechaCreacion
                // Agrega más campos si es necesario
            });

            return (resultados, total);
        }

        // Consulta sin filtros (opcional)
        public async Task<IEnumerable<SolicitudDto>> ObtenerTodasAsync()
        {
            var solicitudes = await _repository.ObtenerTodasAsync();
            return solicitudes.Select(s => new SolicitudDto
            {
                Id = s.Id,
                Titulo = s.Titulo,
                Descripcion = s.Descripcion,
                FechaCreacion = s.FechaCreacion
            });
        }
    }
}
