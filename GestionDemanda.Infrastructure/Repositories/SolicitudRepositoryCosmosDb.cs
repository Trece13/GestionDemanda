using GestionDemanda.Application.Interfaces;
using GestionDemanda.Domain.Entities;
using GestionDemanda.Shared.DTOs;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionDemanda.Infrastructure.Repositories
{
    public class SolicitudRepositoryCosmosDb : ISolicitudRepository
    {
        private readonly Container _container;

        public SolicitudRepositoryCosmosDb(IConfiguration configuration)
        {
            var connectionString = configuration["CosmosDb:ConnectionString"];
            var databaseName = configuration["CosmosDb:DatabaseName"];
            var containerName = configuration["CosmosDb:ContainerName"];

            var client = new CosmosClient(connectionString);
            _container = client.GetContainer(databaseName, containerName);
        }

        public async Task GuardarAsync(Solicitud solicitud)
        {
            if (string.IsNullOrEmpty(solicitud.Id))
            {
                solicitud.Id = Guid.NewGuid().ToString();
            }

            await _container.CreateItemAsync(solicitud, new PartitionKey(solicitud.Id));
        }

        public async Task<IEnumerable<Solicitud>> ObtenerTodasAsync()
        {
            var query = _container.GetItemQueryIterator<Solicitud>("SELECT * FROM c");
            var results = new List<Solicitud>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public async Task<(IEnumerable<Solicitud> resultados, int total)> ConsultarAsync(SolicitudFiltroDto filtro)
        {
            var queryBuilder = new StringBuilder("SELECT * FROM c WHERE 1=1");

            if (!string.IsNullOrWhiteSpace(filtro.Pais))
                queryBuilder.Append(" AND c.Pais = @pais");
            if (!string.IsNullOrWhiteSpace(filtro.TipoSolicitud))
                queryBuilder.Append(" AND c.TipoSolicitud = @tipo");
            if (!string.IsNullOrWhiteSpace(filtro.GerenciaN1))
                queryBuilder.Append(" AND c.GerenciaN1 = @gerenciaN1");
            if (!string.IsNullOrWhiteSpace(filtro.GerenciaN2))
                queryBuilder.Append(" AND c.GerenciaN2 = @gerenciaN2");
            if (!string.IsNullOrWhiteSpace(filtro.ResponsableNombre))
                queryBuilder.Append(" AND c.ResponsableNombre = @responsableNombre");
            if (!string.IsNullOrWhiteSpace(filtro.ResponsableCorreo))
                queryBuilder.Append(" AND c.ResponsableCorreo = @responsableCorreo");
            if (filtro.FechaDesde.HasValue)
                queryBuilder.Append(" AND c.FechaEntrega >= @fechaDesde");
            if (filtro.FechaHasta.HasValue)
                queryBuilder.Append(" AND c.FechaEntrega <= @fechaHasta");

            var queryDef = new QueryDefinition(queryBuilder.ToString());

            if (!string.IsNullOrWhiteSpace(filtro.Pais))
                queryDef.WithParameter("@pais", filtro.Pais);
            if (!string.IsNullOrWhiteSpace(filtro.TipoSolicitud))
                queryDef.WithParameter("@tipo", filtro.TipoSolicitud);
            if (!string.IsNullOrWhiteSpace(filtro.GerenciaN1))
                queryDef.WithParameter("@gerenciaN1", filtro.GerenciaN1);
            if (!string.IsNullOrWhiteSpace(filtro.GerenciaN2))
                queryDef.WithParameter("@gerenciaN2", filtro.GerenciaN2);
            if (!string.IsNullOrWhiteSpace(filtro.ResponsableNombre))
                queryDef.WithParameter("@responsableNombre", filtro.ResponsableNombre);
            if (!string.IsNullOrWhiteSpace(filtro.ResponsableCorreo))
                queryDef.WithParameter("@responsableCorreo", filtro.ResponsableCorreo);
            if (filtro.FechaDesde.HasValue)
                queryDef.WithParameter("@fechaDesde", filtro.FechaDesde.Value);
            if (filtro.FechaHasta.HasValue)
                queryDef.WithParameter("@fechaHasta", filtro.FechaHasta.Value);

            var iterator = _container.GetItemQueryIterator<Solicitud>(queryDef);
            int skip = (filtro.Pagina - 1) * filtro.ElementosPorPagina;

            var resultados = new List<Solicitud>();
            int total = 0;
            int index = 0;

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                foreach (var item in response)
                {
                    if (index >= skip && resultados.Count < filtro.ElementosPorPagina)
                        resultados.Add(item);

                    index++;
                }

                total += response.Count;

                if (resultados.Count >= filtro.ElementosPorPagina)
                    break;
            }

            return (resultados, total);
        }
    }
}
