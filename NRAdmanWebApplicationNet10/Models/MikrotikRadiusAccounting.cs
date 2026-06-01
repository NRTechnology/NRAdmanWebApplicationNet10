using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    /// <summary>
    /// Model untuk RADIUS Accounting Session di Mikrotik
    /// Menyimpan data usage/session dari user yang ter-authenticate
    /// </summary>
    [Table("mikrotik_radius_accounting")]
    public class MikrotikRadiusAccounting
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("username")]
        [MaxLength(255)]
        [Display(Name = "Username")]
        public string? Username { get; set; }

        [Column("nas_ip_address")]
        [MaxLength(50)]
        [Display(Name = "NAS IP Address")]
        public string? NasIpAddress { get; set; }

        /// <summary>
        /// Total input octets (bytes yang diterima)
        /// </summary>
        [Column("acct_input_octets")]
        [Display(Name = "Input Octets")]
        public long? AcctInputOctets { get; set; }

        /// <summary>
        /// Total output octets (bytes yang dikirim)
        /// </summary>
        [Column("acct_output_octets")]
        [Display(Name = "Output Octets")]
        public long? AcctOutputOctets { get; set; }

        /// <summary>
        /// Input packets
        /// </summary>
        [Column("acct_input_packets")]
        [Display(Name = "Input Packets")]
        public long? AcctInputPackets { get; set; }

        /// <summary>
        /// Output packets
        /// </summary>
        [Column("acct_output_packets")]
        [Display(Name = "Output Packets")]
        public long? AcctOutputPackets { get; set; }

        /// <summary>
        /// Session duration dalam detik
        /// </summary>
        [Column("acct_session_time")]
        [Display(Name = "Session Time (seconds)")]
        public long? AcctSessionTime { get; set; }

        /// <summary>
        /// Status type: Start, Interim-Update, Stop
        /// </summary>
        [Column("acct_status_type")]
        [MaxLength(50)]
        [Display(Name = "Status Type")]
        public string? AcctStatusType { get; set; }

        /// <summary>
        /// Session ID dari RADIUS
        /// </summary>
        [Column("acct_session_id")]
        [MaxLength(255)]
        [Display(Name = "Session ID")]
        public string? AcctSessionId { get; set; }

        /// <summary>
        /// Terminate cause (jika applicable)
        /// </summary>
        [Column("acct_terminate_cause")]
        [MaxLength(100)]
        [Display(Name = "Terminate Cause")]
        public string? AcctTerminateCause { get; set; }

        [Column("created_date")]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
