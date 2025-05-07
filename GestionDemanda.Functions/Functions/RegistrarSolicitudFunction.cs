using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using GestionDemanda.Application.Interfaces;
using GestionDemanda.Shared.DTOs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace GestionDemanda.Functions.Functions.RegistrarSolicitud
{
    public class RegistrarSolicitudFunction
    {
        private readonly IRegistrarSolicitudHandler _handler;
        private readonly ILogger _logger;

        public RegistrarSolicitudFunction(
            IRegistrarSolicitudHandler handler,
            ILoggerFactory loggerFactory)
        {
            _handler = handler;
            _logger = loggerFactory.CreateLogger<RegistrarSolicitudFunction>();
        }

        [Function("RegistrarSolicitud")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "solicitud")] HttpRequestData req)
        {
            _logger.LogInformation("Solicitud recibida para registrar.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            SolicitudDto dto;
            try
            {
                dto = JsonSerializer.Deserialize<SolicitudDto>(requestBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (dto == null)
                    throw new JsonException("SolicitudDto es null");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error al deserializar la solicitud.");
                var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequest.WriteStringAsync("Solicitud inválida. Verifica el formato del JSON.");
                return badRequest;
            }

            var workItemId = await _handler.HandleAsync(dto);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new
            {
                mensaje = "¡Solicitud registrada exitosamente!",
                workItemId
            });

            return response;
        }
    }
}
