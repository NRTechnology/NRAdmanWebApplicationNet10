namespace NRAdmanWebApplicationNet10.ViewModels
{
    public class SSHConnectionViewModel
    {
        public int Id { get; set; }
        public string NasName { get; set; } = "";
        public string Server { get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public int Port { get; set; } = 22;
    }
}
