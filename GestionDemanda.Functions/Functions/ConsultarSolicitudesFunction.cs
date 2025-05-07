using GestionDemanda.Application.UseCases;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GestionDemanda.Shared.DTOs;

namespace GestionDemanda.Functions.Functions
{
    public class ConsultarSolicitudesFunction
    {
        private readonly ConsultarSolicitudesHandler _handler;

        public ConsultarSolicitudesFunction(ConsultarSolicitudesHandler handler)
        {
            _handler = handler;
        }


        [Function("ConsultarSolicitudes")]
        public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "solicitud")] HttpRequestData req)
        {
            var queryParams = System.Web.HttpUtility.ParseQueryString(req.Url.Query);

            var filtro = new SolicitudFiltroDto
            {
                Pais = queryParams["pais"],
                TipoSolicitud = queryParams["tipoSolicitud"],
                GerenciaN1 = queryParams["gerenciaN1"],
                GerenciaN2 = queryParams["gerenciaN2"],
                ResponsableNombre = queryParams["responsableNombre"],
                ResponsableCorreo = queryParams["responsableCorreo"],
                FechaDesde = DateTime.TryParse(queryParams["fechaDesde"], out var desde) ? desde : null,
                FechaHasta = DateTime.TryParse(queryParams["fechaHasta"], out var hasta) ? hasta : null,
                Pagina = int.TryParse(queryParams["pagina"], out var pagina) ? pagina : 1,
                ElementosPorPagina = int.TryParse(queryParams["elementosPorPagina"], out var cantidad) ? cantidad : 10
            };

            var (resultados, total) = await _handler.HandleAsync(filtro);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new { total, resultados });
            return response;
        }


    }
}
