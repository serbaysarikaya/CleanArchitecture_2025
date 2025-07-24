using CleanArchitecture_2025.Application.Services;
using CleanArchitecture_2025.Domain.Dtos;
using CleanArchitecture_2025.Domain.Users;
using CleanArchitecture_2025.Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text;
using System.Text.Json;
using TS.Result;
using System.Net.Http.Headers;

namespace CleanArchitecture_2025.Infrastructure.Services
{
    public sealed class KeycloakService(IOptions<KeycloakConfiguration> options) : IJwtProvider
    {
        public async Task<string> GetAccessToken(CancellationToken cancellationToken = default)
        {
            HttpClient client = new();
            string endpoint = $"{options.Value.HostName}/realms/{options.Value.Realm}/protocol/openid-connect/token";
            List<KeyValuePair<string, string>> data = new()
            {
                new("grant_type", "client_credentials"),
                new("client_id", options.Value.ClientId),
                new("client_secret", options.Value.ClientSecret)
            };

            var result = await PostUrlEncodedFormAsync<GetAccessTokenResponseDto>(endpoint, data, false, cancellationToken);
            if (!result.IsSuccessful || result.Data == null)
                throw new Exception("Keycloak access token alınamadı: " + result.ErrorMessages);

            return result.Data.AccessToken;
        }

        public async Task<Result<T>> GetAsync<T>(string endpoint, bool reqToken = false, CancellationToken cancellationToken = default)
        {
            HttpClient httpClient = new();
            if (reqToken)
            {
                string token = await GetAccessToken();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var message = await httpClient.GetAsync(endpoint, cancellationToken);
            var response = await message.Content.ReadAsStringAsync();

            if (!message.IsSuccessStatusCode)
            {
                if (message.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errorResultForBadRequest = JsonSerializer.Deserialize<BadRequestErrorResponseDto>(response);
                    return Result<T>.Failure(errorResultForBadRequest?.ErrorDescription ?? "BadRequest");
                }
                var errorResultForOther = JsonSerializer.Deserialize<ErrorResponseDto>(response);
                return Result<T>.Failure(errorResultForOther?.ErrorMessage ?? "Error");
            }

            if (message.StatusCode == HttpStatusCode.Created || message.StatusCode == HttpStatusCode.NoContent)
            {
                return Result<T>.Succeed(default!);
            }

            var obj = JsonSerializer.Deserialize<T>(response);
            return obj is not null ? Result<T>.Succeed(obj) : Result<T>.Failure("Response deserialize edilemedi");
        }

        public async Task<Result<T>> PutAsync<T>(string endpoint, object data, bool reqToken = false, CancellationToken cancellationToken = default)
        {
            string stringData = JsonSerializer.Serialize(data);
            var content = new StringContent(stringData, Encoding.UTF8, "application/json");
            HttpClient httpClient = new();
            if (reqToken)
            {
                string token = await GetAccessToken();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var message = await httpClient.PutAsync(endpoint, content, cancellationToken);
            var response = await message.Content.ReadAsStringAsync();

            if (!message.IsSuccessStatusCode)
            {
                if (message.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errorResultForBadRequest = JsonSerializer.Deserialize<BadRequestErrorResponseDto>(response);
                    return Result<T>.Failure(errorResultForBadRequest?.ErrorDescription ?? "BadRequest");
                }
                var errorResultForOther = JsonSerializer.Deserialize<ErrorResponseDto>(response);
                return Result<T>.Failure(errorResultForOther?.ErrorMessage ?? "Error");
            }

            if (message.StatusCode == HttpStatusCode.Created || message.StatusCode == HttpStatusCode.NoContent)
            {
                return Result<T>.Succeed(default!);
            }

            var obj = JsonSerializer.Deserialize<T>(response);
            return obj is not null ? Result<T>.Succeed(obj) : Result<T>.Failure("Response deserialize edilemedi");
        }

        public async Task<Result<T>> DeleteAsync<T>(string endpoint, bool reqToken = false, CancellationToken cancellationToken = default)
        {
            HttpClient httpClient = new();
            if (reqToken)
            {
                string token = await GetAccessToken();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var message = await httpClient.DeleteAsync(endpoint, cancellationToken);
            var response = await message.Content.ReadAsStringAsync();

            if (!message.IsSuccessStatusCode)
            {
                if (message.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errorResultForBadRequest = JsonSerializer.Deserialize<BadRequestErrorResponseDto>(response);
                    return Result<T>.Failure(errorResultForBadRequest?.ErrorDescription ?? "BadRequest");
                }
                var errorResultForOther = JsonSerializer.Deserialize<ErrorResponseDto>(response);
                return Result<T>.Failure(errorResultForOther?.ErrorMessage ?? "Error");
            }

            if (message.StatusCode == HttpStatusCode.Created || message.StatusCode == HttpStatusCode.NoContent)
            {
                return Result<T>.Succeed(default!);
            }

            var obj = JsonSerializer.Deserialize<T>(response);
            return obj is not null ? Result<T>.Succeed(obj) : Result<T>.Failure("Response deserialize edilemedi");
        }

        public async Task<Result<T>> DeleteAsync<T>(string endpoint, object data, bool reqToken = false, CancellationToken cancellationToken = default)
        {
            HttpClient httpClient = new();
            if (reqToken)
            {
                string token = await GetAccessToken();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var request = new HttpRequestMessage(HttpMethod.Delete, endpoint)
            {
                Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json")
            };

            var message = await httpClient.SendAsync(request, cancellationToken);
            var response = await message.Content.ReadAsStringAsync();

            if (!message.IsSuccessStatusCode)
            {
                if (message.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errorResultForBadRequest = JsonSerializer.Deserialize<BadRequestErrorResponseDto>(response);
                    return Result<T>.Failure(errorResultForBadRequest?.ErrorDescription ?? "BadRequest");
                }
                var errorResultForOther = JsonSerializer.Deserialize<ErrorResponseDto>(response);
                return Result<T>.Failure(errorResultForOther?.ErrorMessage ?? "Error");
            }

            if (message.StatusCode == HttpStatusCode.Created || message.StatusCode == HttpStatusCode.NoContent)
            {
                return Result<T>.Succeed(default!);
            }

            var obj = JsonSerializer.Deserialize<T>(response);
            return obj is not null ? Result<T>.Succeed(obj) : Result<T>.Failure("Response deserialize edilemedi");
        }

        public async Task<Result<T>> PostAsync<T>(string endpoint, object data, bool reqToken = false, CancellationToken cancellationToken = default)
        {
            string stringData = JsonSerializer.Serialize(data);
            var content = new StringContent(stringData, Encoding.UTF8, "application/json");
            HttpClient httpClient = new();
            if (reqToken)
            {
                string token = await GetAccessToken();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var message = await httpClient.PostAsync(endpoint, content, cancellationToken);
            var response = await message.Content.ReadAsStringAsync();

            if (!message.IsSuccessStatusCode)
            {
                if (message.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errorResultForBadRequest = JsonSerializer.Deserialize<BadRequestErrorResponseDto>(response);
                    return Result<T>.Failure(errorResultForBadRequest?.ErrorDescription ?? "BadRequest");
                }
                var errorResultForOther = JsonSerializer.Deserialize<ErrorResponseDto>(response);
                return Result<T>.Failure(errorResultForOther?.ErrorMessage ?? "Error");
            }

            if (message.StatusCode == HttpStatusCode.Created || message.StatusCode == HttpStatusCode.NoContent)
            {
                return Result<T>.Succeed(default!);
            }

            var obj = JsonSerializer.Deserialize<T>(response);
            return obj is not null ? Result<T>.Succeed(obj) : Result<T>.Failure("Response deserialize edilemedi");
        }

        public async Task<Result<T>> PostUrlEncodedFormAsync<T>(string endpoint, List<KeyValuePair<string, string>> data, bool reqToken = false, CancellationToken cancellationToken = default)
        {
            HttpClient httpClient = new();
            if (reqToken)
            {
                string token = await GetAccessToken();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var message = await httpClient.PostAsync(endpoint, new FormUrlEncodedContent(data), cancellationToken);
            var response = await message.Content.ReadAsStringAsync();

            if (!message.IsSuccessStatusCode)
            {
                if (message.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errorResultForBadRequest = JsonSerializer.Deserialize<BadRequestErrorResponseDto>(response);
                    return Result<T>.Failure(errorResultForBadRequest?.ErrorDescription ?? "BadRequest");
                }
                else if (message.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var errorResultForBadRequest = JsonSerializer.Deserialize<BadRequestErrorResponseDto>(response);
                    return Result<T>.Failure(errorResultForBadRequest?.ErrorDescription ?? "Unauthorized");
                }
                var errorResultForOther = JsonSerializer.Deserialize<ErrorResponseDto>(response);
                return Result<T>.Failure(errorResultForOther?.ErrorMessage ?? "Error");
            }

            if (message.StatusCode == HttpStatusCode.Created || message.StatusCode == HttpStatusCode.NoContent)
            {
                return Result<T>.Succeed(default!);
            }

            var obj = JsonSerializer.Deserialize<T>(response);
            return obj is not null ? Result<T>.Succeed(obj) : Result<T>.Failure("Response deserialize edilemedi");
        }

        public async Task<string> CreateTokenAsync(AppUser user, string password, CancellationToken cancellationToken = default)
        {
            string endpoint = $"{options.Value.HostName}/realms/{options.Value.Realm}/protocol/openid-connect/token";
            List<KeyValuePair<string, string>> data = new()
    {
        new("grant_type", "password"),
        new("client_id", options.Value.ClientId),
        new("client_secret", options.Value.ClientSecret),
        new("username", user.UserName!),
        new("password", password)
    };

            var response = await PostUrlEncodedFormAsync<GetAccessTokenResponseDto>(endpoint, data, false, cancellationToken);
            if (!response.IsSuccessful || response.Data == null)
            {
                string errors = response.ErrorMessages switch
                {
                    IEnumerable<string> list => string.Join(" | ", list),
                    _ => response.ErrorMessages?.ToString() ?? "Bilinmeyen hata"
                };
            }

            return response.Data!.AccessToken;
        }
    }
}