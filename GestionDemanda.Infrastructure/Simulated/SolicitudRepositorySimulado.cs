using GestionDemanda.Application.Interfaces;
using GestionDemanda.Domain.Entities;
using GestionDemanda.Shared.DTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDemanda.Infrastructure.Simulated
{
    public class SolicitudRepositorySimulado : ISolicitudRepository
    {
        private static readonly ConcurrentDictionary<string, Solicitud> _db = new();

        public Task GuardarAsync(Solicitud solicitud)
        {
            solicitud.Id = Guid.NewGuid().ToString(); // Genera un ID ficticio
            _db[solicitud.Id] = solicitud;  // Lo guarda en memoria
            return Task.CompletedTask;
        }

        private static readonly List<Solicitud> _solicitudes = new();

        public Task<IEnumerable<Solicitud>> ObtenerTodasAsync()
        {
            return Task.FromResult<IEnumerable<Solicitud>>(_solicitudes);
        }

        public Task<(IEnumerable<Solicitud> resultados, int total)> ConsultarAsync(SolicitudFiltroDto filtro)
        {
            throw new NotImplementedException();
        }
    }
}
