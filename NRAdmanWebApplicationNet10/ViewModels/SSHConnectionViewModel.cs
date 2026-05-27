namespace NRAdmanWebApplicationNet10.ViewModels
{
    public class SshConnectionViewModel
    {
        public Guid Id { get; set; }
        public string RouterName { get; set; } = "";
        public string IpAddress{ get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public int Port { get; set; } = 22;
    }
}
