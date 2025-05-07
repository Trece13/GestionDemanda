using GestionDemanda.Application.Interfaces;
using GestionDemanda.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace GestionDemanda.Infrastructure.Services
{
    public class AzureDevOpsService : IAzureDevOpsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _organization;
        private readonly string _project;
        private readonly string _personalAccessToken;

        public AzureDevOpsService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _organization = configuration["DevOpsOrganization"];
            _project = configuration["DevOpsProject"];
            _personalAccessToken = configuration["DevOpsToken"];

            if (string.IsNullOrEmpty(_organization) || string.IsNullOrEmpty(_project) || string.IsNullOrEmpty(_personalAccessToken))
            {
                throw new ArgumentException("Faltan configuraciones de Azure DevOps en local.settings.json.");
            }

            var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($":{_personalAccessToken}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
        }

        public async Task<string> CrearWorkItemAsync(Solicitud solicitud, string urlPdf)
        {
            var url = $"https://dev.azure.com/{_organization}/{_project}/_apis/wit/workitems/$User%20Story?api-version=7.0";

            //var url = $"https://dev.azure.com/{_organization}/{_project}/_apis/wit/workitems/$Task?api-version=7.0";

            var content = new[]
{
                new {
                    op = "add",
                    path = "/fields/System.Title",
                    value = solicitud.Titulo
                },
                new {
                    op = "add",
                    path = "/fields/System.Description",
                    value = $"<p>{solicitud.DescripcionFuncional}</p><p><strong>PDF:</strong> <a href='{urlPdf}'>{urlPdf}</a></p>"
                },
                new {
                    op = "add",
                    path = "/fields/System.AssignedTo",
                    value = "jucubillos4@poligran.edu.co" // <-- Así te asignas tú mismo
                },
                new {
                    op = "add",
                    path = "/fields/System.IterationPath",
                    value = "GestionDemanda\\Sprint 1" // <-- Así queda dentro de la Iteración activa
                }
    
            };
            

            var jsonContent = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json-patch+json");
            var response = await _httpClient.PostAsync(url, jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error creando Work Item: {error}");
            }

            var result = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(result);
            var workItemUrl = doc.RootElement.GetProperty("url").GetString();
            return workItemUrl;
        }
    }
}
