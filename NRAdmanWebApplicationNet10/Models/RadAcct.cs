using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace NRAdmanWebApplicationNet10.Models
{
    [Table("radacct")]
    public class RadAcct
    {
        [Key]
        [Column("radacctid")]
        public long RadAcctId { get; set; }

        [Column("acctsessionid")]
        [StringLength(64)]
        public string AcctSessionId { get; set; } = "";

        [Column("acctuniqueid")]
        [StringLength(32)]
        public string AcctUniqueId { get; set; } = "";

        [Column("username")]
        [StringLength(64)]
        public string? UserName { get; set; }

        [Column("realm")]
        [StringLength(64)]
        public string? Realm { get; set; }

        [Column("nasipaddress")]
        public IPAddress NasIpAddress { get; set; } = IPAddress.None;

        [Column("nasportid")]
        [StringLength(15)]
        public string? NasPortId { get; set; }

        [Column("nasporttype")]
        [StringLength(32)]
        public string? NasPortType { get; set; }

        [Column("acctstarttime")]
        public DateTimeOffset? AcctStartTime { get; set; }

        [Column("acctupdatetime")]
        public DateTimeOffset? AcctUpdateTime { get; set; }

        [Column("acctstoptime")]
        public DateTimeOffset? AcctStopTime { get; set; }

        [Column("acctinterval")]
        public long? AcctInterval { get; set; }

        [Column("acctsessiontime")]
        public long? AcctSessionTime { get; set; }

        [Column("acctauthentic")]
        [StringLength(32)]
        public string? AcctAuthentic { get; set; }

        [Column("connectinfo_start")]
        [StringLength(50)]
        public string? ConnectInfoStart { get; set; }

        [Column("connectinfo_stop")]
        [StringLength(50)]
        public string? ConnectInfoStop { get; set; }

        [Column("acctinputoctets")]
        public long? AcctInputOctets { get; set; }

        [Column("acctoutputoctets")]
        public long? AcctOutputOctets { get; set; }

        [Column("calledstationid")]
        [StringLength(50)]
        public string? CalledStationId { get; set; }

        [Column("callingstationid")]
        [StringLength(50)]
        public string? CallingStationId { get; set; }

        [Column("acctterminatecause")]
        [StringLength(32)]
        public string? AcctTerminateCause { get; set; }

        [Column("servicetype")]
        [StringLength(32)]
        public string? ServiceType { get; set; }

        [Column("framedprotocol")]
        [StringLength(32)]
        public string? FramedProtocol { get; set; }

        [Column("framedipaddress")]
        public IPAddress? FramedIpAddress { get; set; }

        [Column("class")]
        [StringLength(64)]
        public string? Class { get; set; }
    }
}
