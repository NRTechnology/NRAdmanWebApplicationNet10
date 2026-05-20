namespace NRAdmanWebApplicationNet10.ViewModels
{
    public class SSHConnectionRequest
    {
        public string Server { get; set; } = "";
        public int Port { get; set; } = 22;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class SSHCommandRequest
    {
        public string Server { get; set; } = "";
        public int Port { get; set; } = 22;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string Command { get; set; } = "";
    }
}
