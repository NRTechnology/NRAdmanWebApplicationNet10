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
        public string AcctSessionId { get; set; } = "";

        [Column("acctuniqueid")]
        public string AcctUniqueId { get; set; } = "";

        [Column("username")]
        public string? UserName { get; set; }

        [Column("realm")]
        public string? Realm { get; set; }

        [Column("nasipaddress")]
        public IPAddress NasIpAddress { get; set; } = IPAddress.None;

        [Column("nasportid")]
        public string? NasPortId { get; set; }

        [Column("nasporttype")]
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
        public string? AcctAuthentic { get; set; }

        [Column("connectinfo_start")]
        public string? ConnectInfoStart { get; set; }

        [Column("connectinfo_stop")]
        public string? ConnectInfoStop { get; set; }

        [Column("acctinputoctets")]
        public long? AcctInputOctets { get; set; }

        [Column("acctoutputoctets")]
        public long? AcctOutputOctets { get; set; }

        [Column("calledstationid")]
        public string? CalledStationId { get; set; }

        [Column("callingstationid")]
        public string? CallingStationId { get; set; }

        [Column("acctterminatecause")]
        public string? AcctTerminateCause { get; set; }

        [Column("servicetype")]
        public string? ServiceType { get; set; }

        [Column("framedprotocol")]
        public string? FramedProtocol { get; set; }

        [Column("framedipaddress")]
        public IPAddress? FramedIpAddress { get; set; }

        [Column("class")]
        public string? Class { get; set; }
    }
}
