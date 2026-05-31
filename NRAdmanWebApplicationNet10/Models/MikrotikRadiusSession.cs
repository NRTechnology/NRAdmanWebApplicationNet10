using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    /// <summary>
    /// Model untuk menyimpan accounting records dari Mikrotik RADIUS server
    /// Data ini berasal dari FreeRADIUS accounting table
    /// </summary>
    [Table("mikrotik_radius_accounting")]
    public class MikrotikRadiusAccounting
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("username")]
        [Required]
        [MaxLength(64)]
        [Display(Name = "Username")]
        public string Username { get; set; } = "";

        [Column("nas_ip_address")]
        [MaxLength(15)]
        [Display(Name = "NAS IP Address")]
        public string? NasIpAddress { get; set; }

        [Column("nas_port")]
        [Display(Name = "NAS Port")]
        public int? NasPort { get; set; }

        [Column("acct_session_id")]
        [MaxLength(64)]
        [Display(Name = "Session ID")]
        public string? AcctSessionId { get; set; }

        [Column("acct_start_time")]
        [Display(Name = "Start Time")]
        public DateTime? AcctStartTime { get; set; }

        [Column("acct_stop_time")]
        [Display(Name = "Stop Time")]
        public DateTime? AcctStopTime { get; set; }

        [Column("acct_session_time")]
        [Display(Name = "Session Duration (seconds)")]
        public long? AcctSessionTime { get; set; }

        [Column("acct_input_octets")]
        [Display(Name = "Input Data (bytes)")]
        public long? AcctInputOctets { get; set; }

        [Column("acct_output_octets")]
        [Display(Name = "Output Data (bytes)")]
        public long? AcctOutputOctets { get; set; }

        [Column("acct_input_packets")]
        [Display(Name = "Input Packets")]
        public long? AcctInputPackets { get; set; }

        [Column("acct_output_packets")]
        [Display(Name = "Output Packets")]
        public long? AcctOutputPackets { get; set; }

        [Column("acct_terminate_cause")]
        [MaxLength(32)]
        [Display(Name = "Terminate Cause")]
        public string? AcctTerminateCause { get; set; }

        [Column("framed_ip_address")]
        [MaxLength(15)]
        [Display(Name = "Framed IP Address")]
        public string? FramedIpAddress { get; set; }

        [Column("called_station_id")]
        [MaxLength(50)]
        [Display(Name = "Called Station ID")]
        public string? CalledStationId { get; set; }

        [Column("calling_station_id")]
        [MaxLength(50)]
        [Display(Name = "Calling Station ID")]
        public string? CallingStationId { get; set; }

        [Column("acct_status_type")]
        [MaxLength(20)]
        [Display(Name = "Status Type")]
        public string? AcctStatusType { get; set; }

        [Column("created_date")]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }

    public enum EnumAcctTerminateCause
    {
        [Display(Name = "User Request")]
        UserRequest = 1,
        [Display(Name = "Lost Carrier")]
        LostCarrier = 2,
        [Display(Name = "Lost Service")]
        LostService = 3,
        [Display(Name = "Idle Timeout")]
        IdleTimeout = 4,
        [Display(Name = "Session Timeout")]
        SessionTimeout = 5,
        [Display(Name = "Admin Reset")]
        AdminReset = 6,
        [Display(Name = "Admin Reboot")]
        AdminReboot = 7,
        [Display(Name = "Port Error")]
        PortError = 8,
        [Display(Name = "NAS Error")]
        NasError = 9,
        [Display(Name = "NAS Request")]
        NasRequest = 10
    }
}

