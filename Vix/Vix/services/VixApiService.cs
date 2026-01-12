using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq; // Necesario para JObject

namespace Vix.Services
{
    public class VixApiService
    {
        private readonly HttpClient _httpClient;

        // QUERY DE GRAPHQL (Copiada del curl y escapada para C#)
        // Usamos @"" para strings multilínea
        private const string DEPORTES_QUERY = @"
        query PageData($urlPath: ID!, $uiModulesPagination: PaginationParams, $contentPagination: PaginationParams, $minCount: Int!, $hours: Int!) {
            uiPage(urlPath: $urlPath) {
                pageName
                urlPath
                uiModules(pagination: $uiModulesPagination) {
                    totalCount
                    edges {
                        node {
                            moduleType
                            ...on UiHeroCarousel {
                                title
                                contents(pagination: $contentPagination) {
                                    edges {
                                        node {
                                            textTitle
                                        }
                                    }
                                }
                            }
                            ...on UiMixedContentCarousel {
                                treatment
                            }
                        }
                    }
                }
            }
        }";

        public VixApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetAnonTokenAsync()
        {
            // ... (Tu código actual de GetAnonTokenAsync va aquí, déjalo tal cual) ...
            // Solo asegúrate de que el código que corregimos antes esté aquí.

            var requestBody = new { installationId = "javier-test" };
            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "https://identity-api.qa.vix.tv/v1/auth/key/anon-user");
            request.Content = content;
            request.Headers.Add("User-Agent", "insomnia/9.3.1");
            request.Headers.Add("x-vix-api-key", "K5EL0BSwTDaKAYwGhMUv2GtNtJPGt9HyG4TofaNsX7QXAzFm");
            request.Headers.Add("x-vix-platform", "samsungtv");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode(); // Lanza error si falla
            return await response.Content.ReadAsStringAsync();
        }

        // --- NUEVO MÉTODO PARA GRAPHQL ---
        // En VixApiService.cs

        // Cambia la firma del método para aceptar ambos tokens
        public async Task<string> GetDeportesPageAsync(string accessToken, string userToken)
        {
            // ... (la parte de construir el body GraphQL déjala igual) ...
            var graphQLRequest = new
            {
                query = DEPORTES_QUERY,
                operationName = "PageData",
                variables = new
                {
                    urlPath = "/deportes",
                    uiModulesPagination = new { first = 5 },
                    contentPagination = new { first = 5 },
                    minCount = 3,
                    hours = 2
                }
            };

            var json = JsonConvert.SerializeObject(graphQLRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "https://client-api.qa.vix.tv/gql/v2");
            request.Content = content;

            // Headers fijos
            request.Headers.Add("User-Agent", "insomnia/12.1.0");
            request.Headers.Add("x-vix-app-version", "5.0.0");
            request.Headers.Add("x-vix-device-type", "smarttv");
            request.Headers.Add("x-vix-platform", "samsungtv");

            // --- AQUÍ ESTÁ EL CAMBIO IMPORTANTE ---

            // 1. El 'accessToken' va en el Authorization Bearer
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // 2. El 'userToken' va en el header propio de Vix
            request.Headers.Add("x-vix-user-token", userToken);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"GraphQL Error ({response.StatusCode}): {errorBody}");
            }

            return await response.Content.ReadAsStringAsync();
        }

    }
}
