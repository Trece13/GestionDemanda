using GestionDemanda.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace GestionDemanda.Infrastructure.Services
{
    public class SharePointService : ISharePointService
    {
        private readonly string _tenantId;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _siteUrl;
        private readonly string _folderPath;
        private readonly HttpClient _httpClient;

        public SharePointService(IConfiguration configuration)
        {
            _tenantId = configuration["SharePoint:TenantId"];
            _clientId = configuration["SharePoint:ClientId"];
            _clientSecret = configuration["SharePoint:ClientSecret"];
            _siteUrl = configuration["SharePoint:SiteUrl"];
            _folderPath = configuration["SharePoint:FolderPath"];

            _httpClient = new HttpClient();
        }

        public async Task<string> SubirArchivoAsync(string nombreArchivo, Stream contenido)
        {
            var token = await ObtenerTokenAsync();
            var siteId = await ObtenerSiteIdAsync(token);

            var uploadUrl = $"https://graph.microsoft.com/v1.0/sites/{siteId}/drive/root:/{_folderPath}/{nombreArchivo}:/content";

            var request = new HttpRequestMessage(HttpMethod.Put, uploadUrl)
            {
                Content = new StreamContent(contenido)
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error subiendo archivo a SharePoint: {error}");
            }

            var json = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(json);
            var webUrl = document.RootElement.GetProperty("webUrl").GetString();

            return webUrl;
        }

        private async Task<string> ObtenerTokenAsync()
        {
            var url = $"https://login.microsoftonline.com/{_tenantId}/oauth2/v2.0/token";

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", _clientId },
                { "client_secret", _clientSecret },
                { "scope", "https://graph.microsoft.com/.default" }
            });

            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error obteniendo token: {error}");
            }

            var json = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(json);
            return document.RootElement.GetProperty("access_token").GetString();
        }

        private async Task<string> ObtenerSiteIdAsync(string token)
        {
            var domain = new Uri(_siteUrl).Host;
            var path = _siteUrl.Substring(_siteUrl.IndexOf("/sites/") + 7);

            var url = $"https://graph.microsoft.com/v1.0/sites/{domain}:/sites/{path}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error obteniendo Site ID: {error}");
            }

            var json = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(json);
            return document.RootElement.GetProperty("id").GetString();
        }
    }
}
