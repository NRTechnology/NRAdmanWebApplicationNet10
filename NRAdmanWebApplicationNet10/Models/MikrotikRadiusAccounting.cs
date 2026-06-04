using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    /// <summary>
    /// Model untuk RADIUS Accounting Session di Mikrotik
    /// Menyimpan data usage/session dari user yang ter-authenticate
    /// </summary>
    public class MikrotikRadiusAccounting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [MaxLength(255)]
        [Display(Name = "Username")]
        public string? Username { get; set; }
        
        [MaxLength(50)]
        [Display(Name = "NAS IP Address")]
        public string? NasIpAddress { get; set; }

        /// <summary>
        /// Total input octets (bytes yang diterima)
        /// </summary>
        [Display(Name = "Input Octets")]
        public long? AcctInputOctets { get; set; }

        /// <summary>
        /// Total output octets (bytes yang dikirim)
        /// </summary>
        [Display(Name = "Output Octets")]
        public long? AcctOutputOctets { get; set; }

        /// <summary>
        /// Input packets
        /// </summary>
        [Display(Name = "Input Packets")]
        public long? AcctInputPackets { get; set; }

        /// <summary>
        /// Output packets
        /// </summary>
        [Display(Name = "Output Packets")]
        public long? AcctOutputPackets { get; set; }

        /// <summary>
        /// Session duration dalam detik
        /// </summary>
        [Display(Name = "Session Time (seconds)")]
        public long? AcctSessionTime { get; set; }

        /// <summary>
        /// Status type: Start, Interim-Update, Stop
        /// </summary>
        [MaxLength(50)]
        [Display(Name = "Status Type")]
        public string? AcctStatusType { get; set; }

        /// <summary>
        /// Session ID dari RADIUS
        /// </summary>
        [MaxLength(255)]
        [Display(Name = "Session ID")]
        public string? AcctSessionId { get; set; }

        /// <summary>
        /// Terminate cause (jika applicable)
        /// </summary>
        [MaxLength(100)]
        [Display(Name = "Terminate Cause")]
        public string? AcctTerminateCause { get; set; }
        
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
