using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace NRAdmanWebApplicationNet10.Services.Mikrotik
{
    /// <summary>
    /// DTOs untuk Mikrotik API responses
    /// </summary>
    public class MikrotikQueueDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Target { get; set; }
        public string? MaxLimit { get; set; }
        public string? BurstLimit { get; set; }
        public string? BurstThreshold { get; set; }
        public string? BurstTime { get; set; }
        public int? Priority { get; set; }
        public string? Parent { get; set; }
        public bool? Disabled { get; set; }
        public Dictionary<string, string>? Attributes { get; set; }
    }

    public class MikrotikConnectionSettings
    {
        public string? RouterName { get; set; }
        public string? ApiHost { get; set; }
        public int ApiPort { get; set; } = 8729; // Default API SSL port
        public string? ApiUsername { get; set; }
        public string? ApiPassword { get; set; }
        public bool UseSSL { get; set; } = true;
        public bool IgnoreCertificate { get; set; } = true; // For self-signed certificates
    }

    public class MikrotikApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public Exception? Exception { get; set; }
    }

    /// <summary>
    /// Service untuk komunikasi dengan Mikrotik API menggunakan SSL/TLS
    /// Menangani koneksi, autentikasi, dan operasi queue pada Mikrotik
    /// Implementasi: REST API over HTTPS (port 8729)
    /// </summary>
    public class MikrotikApiService
    {
        private readonly ILogger<MikrotikApiService> _logger;
        private readonly HttpClient _httpClient;
        private const int MaxRetries = 3;
        private const int InitialDelayMs = 5000; // 5 seconds

        public MikrotikApiService(ILogger<MikrotikApiService> logger)
        {
            _logger = logger;
            _httpClient = CreateHttpClient();
        }

        /// <summary>
        /// Create HttpClient dengan SSL/TLS configuration
        /// </summary>
        private HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler();

            // Configure certificate validation
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                // In development: accept self-signed certificates
                if (errors == System.Net.Security.SslPolicyErrors.None)
                    return true;

                // Check for self-signed cert (development mode)
                if (errors == System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors)
                {
                    _logger.LogWarning("Self-signed certificate detected. Accepting for development.");
                    return true;
                }

                // For production, you would implement certificate pinning here
                _logger.LogError($"Certificate validation failed: {errors}");
                return false;
            };

            return new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        /// <summary>
        /// Execute HTTP request dengan retry logic untuk return values
        /// </summary>
        private async Task<T?> ExecuteWithRetryAsync<T>(
            Func<Task<HttpResponseMessage>> requestFunc,
            string operationName,
            Func<HttpResponseMessage, Task<T>>? parseResponseFunc = null)
        {
            Exception? lastException = null;

            for (int attempt = 0; attempt < MaxRetries; attempt++)
            {
                try
                {
                    var response = await requestFunc();

                    if (response.IsSuccessStatusCode)
                    {
                        if (parseResponseFunc != null)
                        {
                            return await parseResponseFunc(response);
                        }
                        return default;
                    }

                    _logger.LogWarning($"[{operationName}] Attempt {attempt + 1}/{MaxRetries} - Status: {response.StatusCode}");

                    // Exponential backoff: 5s, 15s, 30s
                    if (attempt < MaxRetries - 1)
                    {
                        int delayMs = InitialDelayMs * (int)Math.Pow(3, attempt);
                        await Task.Delay(delayMs);
                    }
                }
                catch (HttpRequestException ex)
                {
                    lastException = ex;
                    _logger.LogWarning($"[{operationName}] Attempt {attempt + 1}/{MaxRetries} - {ex.Message}");

                    if (attempt < MaxRetries - 1)
                    {
                        int delayMs = InitialDelayMs * (int)Math.Pow(3, attempt);
                        await Task.Delay(delayMs);
                    }
                }
                catch (TaskCanceledException ex)
                {
                    lastException = ex;
                    _logger.LogWarning($"[{operationName}] Timeout on attempt {attempt + 1}/{MaxRetries}");

                    if (attempt < MaxRetries - 1)
                    {
                        int delayMs = InitialDelayMs * (int)Math.Pow(3, attempt);
                        await Task.Delay(delayMs);
                    }
                }
            }

            throw lastException ?? new Exception($"Operation {operationName} failed after {MaxRetries} retries");
        }

        /// <summary>
        /// Execute HTTP request dengan retry logic tanpa return value
        /// </summary>
        private async Task ExecuteWithRetryAsync(
            Func<Task<HttpResponseMessage>> requestFunc,
            string operationName,
            Func<HttpResponseMessage, Task>? processResponseFunc = null)
        {
            Exception? lastException = null;

            for (int attempt = 0; attempt < MaxRetries; attempt++)
            {
                try
                {
                    var response = await requestFunc();

                    if (response.IsSuccessStatusCode)
                    {
                        if (processResponseFunc != null)
                        {
                            await processResponseFunc(response);
                        }
                        return;
                    }

                    _logger.LogWarning($"[{operationName}] Attempt {attempt + 1}/{MaxRetries} - Status: {response.StatusCode}");

                    // Exponential backoff: 5s, 15s, 30s
                    if (attempt < MaxRetries - 1)
                    {
                        int delayMs = InitialDelayMs * (int)Math.Pow(3, attempt);
                        await Task.Delay(delayMs);
                    }
                }
                catch (HttpRequestException ex)
                {
                    lastException = ex;
                    _logger.LogWarning($"[{operationName}] Attempt {attempt + 1}/{MaxRetries} - {ex.Message}");

                    if (attempt < MaxRetries - 1)
                    {
                        int delayMs = InitialDelayMs * (int)Math.Pow(3, attempt);
                        await Task.Delay(delayMs);
                    }
                }
                catch (TaskCanceledException ex)
                {
                    lastException = ex;
                    _logger.LogWarning($"[{operationName}] Timeout on attempt {attempt + 1}/{MaxRetries}");

                    if (attempt < MaxRetries - 1)
                    {
                        int delayMs = InitialDelayMs * (int)Math.Pow(3, attempt);
                        await Task.Delay(delayMs);
                    }
                }
            }

            throw lastException ?? new Exception($"Operation {operationName} failed after {MaxRetries} retries");
        }

        /// <summary>
        /// Test koneksi ke Mikrotik API via HTTPS
        /// </summary>
        public async Task<MikrotikApiResponse<bool>> TestConnectionAsync(MikrotikConnectionSettings settings)
        {
            try
            {
                if (string.IsNullOrEmpty(settings.ApiHost) || string.IsNullOrEmpty(settings.ApiUsername))
                {
                    return new MikrotikApiResponse<bool>
                    {
                        Success = false,
                        ErrorMessage = "Host atau Username tidak boleh kosong"
                    };
                }

                string url = $"https://{settings.ApiHost}:{settings.ApiPort}/rest/system/identity";
                var request = new HttpRequestMessage(HttpMethod.Get, url);

                // Add basic authentication header
                string credentials = Convert.ToBase64String(
                    Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                bool success = false;
                await ExecuteWithRetryAsync(
                    () => _httpClient.SendAsync(request),
                    $"TestConnection to {settings.ApiHost}",
                    async response =>
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        _logger.LogInformation($"Connection test to {settings.ApiHost} successful");
                        success = true;
                    }
                );

                return new MikrotikApiResponse<bool> { Success = true, Data = success };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error test connection ke {settings.ApiHost}");
                return new MikrotikApiResponse<bool>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    Exception = ex
                };
            }
        }

        /// <summary>
        /// Membuat Simple Queue di Mikrotik via REST API
        /// </summary>
        public async Task<MikrotikApiResponse<MikrotikQueueDto>> CreateSimpleQueueAsync(
            MikrotikConnectionSettings settings,
            string queueName,
            string targetIp,
            string maxLimitDown,
            string maxLimitUp,
            string? burstLimitDown = null,
            string? burstLimitUp = null,
            int priority = 8,
            bool disabled = false)
        {
            try
            {
                if (string.IsNullOrEmpty(queueName) || string.IsNullOrEmpty(targetIp))
                {
                    return new MikrotikApiResponse<MikrotikQueueDto>
                    {
                        Success = false,
                        ErrorMessage = "Queue name dan target IP harus diisi"
                    };
                }

                // Validasi format IP
                if (!IsValidIpAddress(targetIp))
                {
                    return new MikrotikApiResponse<MikrotikQueueDto>
                    {
                        Success = false,
                        ErrorMessage = "Format IP address tidak valid"
                    };
                }

                // Prepare JSON payload
                var payload = new Dictionary<string, object>
                {
                    { "name", queueName },
                    { "target", targetIp },
                    { "max-limit", $"{maxLimitDown}/{maxLimitUp}" },
                    { "priority", priority },
                    { "disabled", disabled ? "true" : "false" }
                };

                if (!string.IsNullOrEmpty(burstLimitDown))
                {
                    payload["burst-limit"] = $"{burstLimitDown}/{burstLimitUp}";
                }

                string url = $"https://{settings.ApiHost}:{settings.ApiPort}/rest/queue/simple/add";
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(HttpMethod.Post, url) { Content = content };

                // Add basic authentication
                string credentials = Convert.ToBase64String(
                    Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                var result = await ExecuteWithRetryAsync(
                    () => _httpClient.SendAsync(request),
                    $"CreateSimpleQueue {queueName}",
                    async response =>
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        if (string.IsNullOrEmpty(responseContent))
                        {
                            responseContent = "{}";
                        }

                        var jsonDoc = JsonDocument.Parse(responseContent);
                        var queueId = jsonDoc.RootElement.TryGetProperty(".id", out var idElement)
                            ? idElement.GetString()
                            : Guid.NewGuid().ToString().Substring(0, 8);

                        var queue = new MikrotikQueueDto
                        {
                            Id = queueId,
                            Name = queueName,
                            Target = targetIp,
                            MaxLimit = $"{maxLimitDown}/{maxLimitUp}",
                            BurstLimit = !string.IsNullOrEmpty(burstLimitDown) ? $"{burstLimitDown}/{burstLimitUp}" : null,
                            Priority = priority,
                            Disabled = disabled,
                            Attributes = new Dictionary<string, string>
                            {
                                { "target", targetIp },
                                { "max-limit", $"{maxLimitDown}/{maxLimitUp}" },
                                { "priority", priority.ToString() }
                            }
                        };

                        _logger.LogInformation($"Queue '{queueName}' untuk {targetIp} dibuat di {settings.ApiHost} dengan ID {queueId}");
                        return queue;
                    }
                );

                return new MikrotikApiResponse<MikrotikQueueDto> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error membuat queue '{queueName}'");
                return new MikrotikApiResponse<MikrotikQueueDto>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    Exception = ex
                };
            }
        }

        /// <summary>
        /// Update Simple Queue di Mikrotik via REST API
        /// </summary>
        public async Task<MikrotikApiResponse<MikrotikQueueDto>> UpdateSimpleQueueAsync(
            MikrotikConnectionSettings settings,
            string queueId,
            string? maxLimitDown = null,
            string? maxLimitUp = null,
            string? burstLimitDown = null,
            string? burstLimitUp = null,
            int? priority = null,
            bool? disabled = null)
        {
            try
            {
                if (string.IsNullOrEmpty(queueId))
                {
                    return new MikrotikApiResponse<MikrotikQueueDto>
                    {
                        Success = false,
                        ErrorMessage = "Queue ID harus diisi"
                    };
                }

                // Prepare JSON payload (only include non-null values)
                var payload = new Dictionary<string, object>();

                if (!string.IsNullOrEmpty(maxLimitDown) && !string.IsNullOrEmpty(maxLimitUp))
                {
                    payload["max-limit"] = $"{maxLimitDown}/{maxLimitUp}";
                }

                if (!string.IsNullOrEmpty(burstLimitDown) && !string.IsNullOrEmpty(burstLimitUp))
                {
                    payload["burst-limit"] = $"{burstLimitDown}/{burstLimitUp}";
                }

                if (priority.HasValue)
                {
                    payload["priority"] = priority.Value;
                }

                if (disabled.HasValue)
                {
                    payload["disabled"] = disabled.Value ? "true" : "false";
                }

                // URL encode the queue ID
                string encodedQueueId = Uri.EscapeDataString(queueId);
                string url = $"https://{settings.ApiHost}:{settings.ApiPort}/rest/queue/simple/{encodedQueueId}";
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(HttpMethod.Patch, url) { Content = content };

                // Add basic authentication
                string credentials = Convert.ToBase64String(
                    Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                var result = await ExecuteWithRetryAsync(
                    () => _httpClient.SendAsync(request),
                    $"UpdateSimpleQueue {queueId}",
                    async response =>
                    {
                        var queue = new MikrotikQueueDto
                        {
                            Id = queueId,
                            MaxLimit = !string.IsNullOrEmpty(maxLimitDown) ? $"{maxLimitDown}/{maxLimitUp}" : null,
                            BurstLimit = !string.IsNullOrEmpty(burstLimitDown) ? $"{burstLimitDown}/{burstLimitUp}" : null,
                            Priority = priority,
                            Disabled = disabled
                        };

                        _logger.LogInformation($"Queue '{queueId}' diupdate di {settings.ApiHost}");
                        return queue;
                    }
                );

                return new MikrotikApiResponse<MikrotikQueueDto> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error update queue '{queueId}'");
                return new MikrotikApiResponse<MikrotikQueueDto>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    Exception = ex
                };
            }
        }

        /// <summary>
        /// Hapus Simple Queue dari Mikrotik via REST API
        /// </summary>
        public async Task<MikrotikApiResponse<bool>> DeleteSimpleQueueAsync(
            MikrotikConnectionSettings settings,
            string queueId)
        {
            try
            {
                if (string.IsNullOrEmpty(queueId))
                {
                    return new MikrotikApiResponse<bool>
                    {
                        Success = false,
                        ErrorMessage = "Queue ID harus diisi"
                    };
                }

                // URL encode the queue ID
                string encodedQueueId = Uri.EscapeDataString(queueId);
                string url = $"https://{settings.ApiHost}:{settings.ApiPort}/rest/queue/simple/{encodedQueueId}";
                var request = new HttpRequestMessage(HttpMethod.Delete, url);

                // Add basic authentication
                string credentials = Convert.ToBase64String(
                    Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                await ExecuteWithRetryAsync(
                    () => _httpClient.SendAsync(request),
                    $"DeleteSimpleQueue {queueId}",
                    async response =>
                    {
                        _logger.LogInformation($"Queue '{queueId}' dihapus dari {settings.ApiHost}");
                    }
                );

                return new MikrotikApiResponse<bool> { Success = true, Data = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error menghapus queue '{queueId}'");
                return new MikrotikApiResponse<bool>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    Exception = ex
                };
            }
        }

        /// <summary>
        /// Dapatkan daftar Simple Queue dari Mikrotik via REST API
        /// </summary>
        public async Task<MikrotikApiResponse<List<MikrotikQueueDto>>> GetSimpleQueuesAsync(
            MikrotikConnectionSettings settings)
        {
            try
            {
                string url = $"https://{settings.ApiHost}:{settings.ApiPort}/rest/queue/simple";
                var request = new HttpRequestMessage(HttpMethod.Get, url);

                // Add basic authentication
                string credentials = Convert.ToBase64String(
                    Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                var result = await ExecuteWithRetryAsync(
                    () => _httpClient.SendAsync(request),
                    $"GetSimpleQueues from {settings.ApiHost}",
                    async response =>
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        if (string.IsNullOrEmpty(responseContent))
                        {
                            responseContent = "[]";
                        }

                        var queues = new List<MikrotikQueueDto>();

                        try
                        {
                            using (JsonDocument jsonDoc = JsonDocument.Parse(responseContent))
                            {
                                if (jsonDoc.RootElement.ValueKind == JsonValueKind.Array)
                                {
                                    foreach (var element in jsonDoc.RootElement.EnumerateArray())
                                    {
                                        var queue = new MikrotikQueueDto();

                                        if (element.TryGetProperty(".id", out var id))
                                            queue.Id = id.GetString();

                                        if (element.TryGetProperty("name", out var name))
                                            queue.Name = name.GetString();

                                        if (element.TryGetProperty("target", out var target))
                                            queue.Target = target.GetString();

                                        if (element.TryGetProperty("max-limit", out var maxLimit))
                                            queue.MaxLimit = maxLimit.GetString();

                                        if (element.TryGetProperty("burst-limit", out var burstLimit))
                                            queue.BurstLimit = burstLimit.GetString();

                                        if (element.TryGetProperty("priority", out var priority))
                                        {
                                            if (priority.TryGetInt32(out int priorityVal))
                                                queue.Priority = priorityVal;
                                        }

                                        if (element.TryGetProperty("disabled", out var disabled))
                                            queue.Disabled = disabled.GetBoolean();

                                        queues.Add(queue);
                                    }
                                }
                            }
                        }
                        catch (JsonException ex)
                        {
                            _logger.LogWarning($"Failed to parse queue response: {ex.Message}");
                        }

                        _logger.LogInformation($"Diperoleh {queues.Count} queue dari {settings.ApiHost}");
                        return queues;
                    }
                );

                return new MikrotikApiResponse<List<MikrotikQueueDto>> { Success = true, Data = result ?? new List<MikrotikQueueDto>() };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error mengambil queue list dari {settings.ApiHost}");
                return new MikrotikApiResponse<List<MikrotikQueueDto>>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    Exception = ex
                };
            }
        }

        /// <summary>
        /// Dapatkan statistik queue dari Mikrotik via REST API
        /// </summary>
        public async Task<MikrotikApiResponse<Dictionary<string, object>>> GetQueueStatsAsync(
            MikrotikConnectionSettings settings,
            string queueId)
        {
            try
            {
                // URL encode the queue ID
                string encodedQueueId = Uri.EscapeDataString(queueId);
                string url = $"https://{settings.ApiHost}:{settings.ApiPort}/rest/queue/simple/{encodedQueueId}/stats";
                var request = new HttpRequestMessage(HttpMethod.Get, url);

                // Add basic authentication
                string credentials = Convert.ToBase64String(
                    Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                var result = await ExecuteWithRetryAsync(
                    () => _httpClient.SendAsync(request),
                    $"GetQueueStats {queueId}",
                    async response =>
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        if (string.IsNullOrEmpty(responseContent))
                        {
                            responseContent = "{}";
                        }

                        var stats = new Dictionary<string, object>();

                        try
                        {
                            using (JsonDocument jsonDoc = JsonDocument.Parse(responseContent))
                            {
                                foreach (var property in jsonDoc.RootElement.EnumerateObject())
                                {
                                    object value = property.Value.ValueKind switch
                                    {
                                        JsonValueKind.Number => property.Value.GetDouble(),
                                        JsonValueKind.String => property.Value.GetString() ?? "",
                                        JsonValueKind.True => true,
                                        JsonValueKind.False => false,
                                        _ => property.Value.GetRawText()
                                    };
                                    stats[property.Name] = value;
                                }
                            }
                        }
                        catch (JsonException ex)
                        {
                            _logger.LogWarning($"Failed to parse stats response: {ex.Message}");
                        }

                        _logger.LogInformation($"Diperoleh statistik queue '{queueId}' dari {settings.ApiHost}");
                        return stats;
                    }
                );

                return new MikrotikApiResponse<Dictionary<string, object>> { Success = true, Data = result ?? new Dictionary<string, object>() };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error mengambil statistik queue dari {settings.ApiHost}");
                return new MikrotikApiResponse<Dictionary<string, object>>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    Exception = ex
                };
            }
        }

        /// <summary>
        /// Validasi format IP address
        /// </summary>
        private bool IsValidIpAddress(string ipAddress)
        {
            try
            {
                var ipPattern = @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(?:/\d+)?$";
                return Regex.IsMatch(ipAddress, ipPattern);
            }
            catch
            {
                return false;
            }
        }
    }
}
