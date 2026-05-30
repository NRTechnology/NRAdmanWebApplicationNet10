namespace NRAdmanWebApplicationNet10.ViewModels
{
    public class SshConnectionRequest
    {
        public string IpAddress { get; set; } = "";
        public int Port { get; set; } = 22;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class SshCommandRequest
    {
        public string IpAddress { get; set; } = "";
        public int Port { get; set; } = 22;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string Command { get; set; } = "";
    }
}
