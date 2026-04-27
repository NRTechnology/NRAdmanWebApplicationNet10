using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    [Table("radacct")]
    public class RadAcct
    {
        [Key]
        [Column("radacctid")]
        public long RadAcctId { get; set; }

        [Required, MaxLength(64)]
        [Column("acctsessionid")]
        public string AcctSessionId { get; set; } = string.Empty;

        [Required, MaxLength(32)]
        [Column("acctuniqueid")]
        public string AcctUniqueId { get; set; } = string.Empty;

        [Required, MaxLength(64)]
        [Column("username")]
        public string Username { get; set; } = string.Empty;

        [MaxLength(64)]
        [Column("realm")]
        public string? Realm { get; set; } = string.Empty;

        [Required, MaxLength(15)]
        [Column("nasipaddress")]
        public string NasIpAddress { get; set; } = string.Empty;

        [MaxLength(32)]
        [Column("nasportid")]
        public string? NasPortId { get; set; }

        [MaxLength(32)]
        [Column("nasporttype")]
        public string? NasPortType { get; set; }

        [Column("acctstarttime")]
        public DateTime? AcctStartTime { get; set; }

        [Column("acctupdatetime")]
        public DateTime? AcctUpdateTime { get; set; }

        [Column("acctstoptime")]
        public DateTime? AcctStopTime { get; set; }

        [Column("acctinterval")]
        public int? AcctInterval { get; set; }

        [Column("acctsessiontime")]
        public uint? AcctSessionTime { get; set; }

        [MaxLength(32)]
        [Column("acctauthentic")]
        public string? AcctAuthentic { get; set; }

        [MaxLength(128)]
        [Column("connectinfo_start")]
        public string? ConnectInfoStart { get; set; }

        [MaxLength(128)]
        [Column("connectinfo_stop")]
        public string? ConnectInfoStop { get; set; }

        [Column("acctinputoctets")]
        public long? AcctInputOctets { get; set; }

        [Column("acctoutputoctets")]
        public long? AcctOutputOctets { get; set; }

        [Required, MaxLength(50)]
        [Column("calledstationid")]
        public string CalledStationId { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        [Column("callingstationid")]
        public string CallingStationId { get; set; } = string.Empty;

        [Required, MaxLength(32)]
        [Column("acctterminatecause")]
        public string AcctTerminateCause { get; set; } = string.Empty;

        [MaxLength(32)]
        [Column("servicetype")]
        public string? ServiceType { get; set; }

        [MaxLength(32)]
        [Column("framedprotocol")]
        public string? FramedProtocol { get; set; }

        [Required, MaxLength(15)]
        [Column("framedipaddress")]
        public string FramedIpAddress { get; set; } = string.Empty;

        [Required, MaxLength(45)]
        [Column("framedipv6address")]
        public string FramedIpv6Address { get; set; } = string.Empty;

        [Required, MaxLength(45)]
        [Column("framedipv6prefix")]
        public string FramedIpv6Prefix { get; set; } = string.Empty;

        [Required, MaxLength(44)]
        [Column("framedinterfaceid")]
        public string FramedInterfaceId { get; set; } = string.Empty;

        [Required, MaxLength(45)]
        [Column("delegatedipv6prefix")]
        public string DelegatedIpv6Prefix { get; set; } = string.Empty;

        [MaxLength(64)]
        [Column("class")]
        public string? Class { get; set; }
    }
}
