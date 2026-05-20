using Renci.SshNet;
using System.Text;

namespace NRAdmanWebApplicationNet10.Services
{
    public interface ISSHService
    {
        Task<SSHConnectionResult> TestConnectionAsync(string host, int port, string username, string password);
        Task<SSHCommandResult> ExecuteCommandAsync(string host, int port, string username, string password, string command);
        Task<SSHConnectionResult> ConnectAndExecuteAsync(string host, int port, string username, string password, string command);
    }

    public class SSHConnectionResult
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; } = "";
        public string? Output { get; set; }
        public DateTime ConnectedAt { get; set; }
    }

    public class SSHCommandResult
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; } = "";
        public string? Output { get; set; }
        public string? ErrorOutput { get; set; }
        public int? ExitStatus { get; set; }
    }

    public class SSHService : ISSHService
    {
        private readonly ILogger<SSHService> _logger;

        public SSHService(ILogger<SSHService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Test SSH connection to a remote server
        /// </summary>
        public async Task<SSHConnectionResult> TestConnectionAsync(string host, int port, string username, string password)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                    {
                        return new SSHConnectionResult
                        {
                            IsSuccessful = false,
                            Message = "Host, username, dan password tidak boleh kosong."
                        };
                    }

                    using (var sshClient = new SshClient(host, port, username, password))
                    {
                        sshClient.Connect();

                        if (sshClient.IsConnected)
                        {
                            _logger.LogInformation("SSH connection successful to {Host}:{Port} with user {Username}", host, port, username);

                            sshClient.Disconnect();

                            return new SSHConnectionResult
                            {
                                IsSuccessful = true,
                                Message = $"Berhasil terhubung ke {host}:{port}",
                                ConnectedAt = DateTime.Now
                            };
                        }
                        else
                        {
                            return new SSHConnectionResult
                            {
                                IsSuccessful = false,
                                Message = "Gagal terhubung ke server SSH."
                            };
                        }
                    }
                }
                catch (Renci.SshNet.Common.SshConnectionException ex)
                {
                    _logger.LogError(ex, "SSH connection failed to {Host}:{Port}", host, port);
                    return new SSHConnectionResult
                    {
                        IsSuccessful = false,
                        Message = $"Koneksi SSH gagal: {ex.Message}"
                    };
                }
                catch (Renci.SshNet.Common.SshAuthenticationException ex)
                {
                    _logger.LogError(ex, "SSH authentication failed for user {Username} on {Host}:{Port}", username, host, port);
                    return new SSHConnectionResult
                    {
                        IsSuccessful = false,
                        Message = $"Autentikasi SSH gagal: Username atau password salah."
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error during SSH connection to {Host}:{Port}", host, port);
                    return new SSHConnectionResult
                    {
                        IsSuccessful = false,
                        Message = $"Error: {ex.Message}"
                    };
                }
            });
        }

        /// <summary>
        /// Execute a command on remote SSH server
        /// </summary>
        public async Task<SSHCommandResult> ExecuteCommandAsync(string host, int port, string username, string password, string command)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(command))
                    {
                        return new SSHCommandResult
                        {
                            IsSuccessful = false,
                            Message = "Command tidak boleh kosong."
                        };
                    }

                    using (var sshClient = new SshClient(host, port, username, password))
                    {
                        sshClient.Connect();

                        if (sshClient.IsConnected)
                        {
                            var sshCommand = sshClient.CreateCommand(command);
                            var output = sshCommand.Execute();

                            _logger.LogInformation("SSH command executed on {Host}: {Command}", host, command);

                            sshClient.Disconnect();

                            return new SSHCommandResult
                            {
                                IsSuccessful = sshCommand.ExitStatus == 0,
                                Message = sshCommand.ExitStatus == 0 ? "Perintah berhasil dijalankan." : "Perintah selesai dengan error status.",
                                Output = output,
                                ErrorOutput = sshCommand.Error,
                                ExitStatus = sshCommand.ExitStatus
                            };
                        }
                        else
                        {
                            return new SSHCommandResult
                            {
                                IsSuccessful = false,
                                Message = "Gagal terhubung ke server SSH."
                            };
                        }
                    }
                }
                catch (Renci.SshNet.Common.SshConnectionException ex)
                {
                    _logger.LogError(ex, "SSH connection failed to {Host}:{Port}", host, port);
                    return new SSHCommandResult
                    {
                        IsSuccessful = false,
                        Message = $"Koneksi SSH gagal: {ex.Message}"
                    };
                }
                catch (Renci.SshNet.Common.SshAuthenticationException ex)
                {
                    _logger.LogError(ex, "SSH authentication failed for user {Username} on {Host}:{Port}", username, host, port);
                    return new SSHCommandResult
                    {
                        IsSuccessful = false,
                        Message = "Autentikasi SSH gagal: Username atau password salah."
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error executing SSH command on {Host}:{Port}", host, port);
                    return new SSHCommandResult
                    {
                        IsSuccessful = false,
                        Message = $"Error: {ex.Message}"
                    };
                }
            });
        }

        /// <summary>
        /// Connect to SSH server and execute command, then close connection
        /// </summary>
        public async Task<SSHConnectionResult> ConnectAndExecuteAsync(string host, int port, string username, string password, string command)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    // First test connection
                    var connectionResult = await TestConnectionAsync(host, port, username, password);

                    if (!connectionResult.IsSuccessful)
                    {
                        return connectionResult;
                    }

                    // If connection successful, execute command
                    var commandResult = await ExecuteCommandAsync(host, port, username, password, command);

                    return new SSHConnectionResult
                    {
                        IsSuccessful = commandResult.IsSuccessful,
                        Message = commandResult.Message,
                        Output = commandResult.Output,
                        ConnectedAt = connectionResult.ConnectedAt
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in ConnectAndExecuteAsync");
                    return new SSHConnectionResult
                    {
                        IsSuccessful = false,
                        Message = $"Error: {ex.Message}"
                    };
                }
            });
        }
    }
}
