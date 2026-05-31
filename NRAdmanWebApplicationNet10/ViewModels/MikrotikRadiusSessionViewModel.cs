using System.ComponentModel.DataAnnotations;

namespace NRAdmanWebApplicationNet10.ViewModels
{
    /// <summary>
    /// ViewModel untuk menampilkan Accounting records dari Mikrotik RADIUS
    /// </summary>
    public class MikrotikRadiusAccountingViewModel
    {
        [Display(Name = "ID")]
        public long Id { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; } = "";

        [Display(Name = "NAS IP")]
        public string? NasIpAddress { get; set; }

        [Display(Name = "NAS Port")]
        public int? NasPort { get; set; }

        [Display(Name = "Session ID")]
        public string? AcctSessionId { get; set; }

        [Display(Name = "Start Time")]
        public DateTime? AcctStartTime { get; set; }

        [Display(Name = "Stop Time")]
        public DateTime? AcctStopTime { get; set; }

        [Display(Name = "Duration")]
        public long? AcctSessionTime { get; set; }

        [Display(Name = "Duration (Formatted)")]
        public string DurationFormatted => FormatDuration(AcctSessionTime);

        [Display(Name = "Input Data")]
        public long? AcctInputOctets { get; set; }

        [Display(Name = "Input Data (Formatted)")]
        public string InputOctetsFormatted => FormatBytes(AcctInputOctets);

        [Display(Name = "Output Data")]
        public long? AcctOutputOctets { get; set; }

        [Display(Name = "Output Data (Formatted)")]
        public string OutputOctetsFormatted => FormatBytes(AcctOutputOctets);

        [Display(Name = "Total Data")]
        public long? TotalOctets => (AcctInputOctets ?? 0) + (AcctOutputOctets ?? 0);

        [Display(Name = "Total Data (Formatted)")]
        public string TotalOctetsFormatted => FormatBytes(TotalOctets);

        [Display(Name = "Input Packets")]
        public long? AcctInputPackets { get; set; }

        [Display(Name = "Output Packets")]
        public long? AcctOutputPackets { get; set; }

        [Display(Name = "Total Packets")]
        public long? TotalPackets => (AcctInputPackets ?? 0) + (AcctOutputPackets ?? 0);

        [Display(Name = "Terminate Cause")]
        public string? AcctTerminateCause { get; set; }

        [Display(Name = "IP Address")]
        public string? FramedIpAddress { get; set; }

        [Display(Name = "Called Station")]
        public string? CalledStationId { get; set; }

        [Display(Name = "Calling Station")]
        public string? CallingStationId { get; set; }

        [Display(Name = "Status Type")]
        public string? AcctStatusType { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        private string FormatBytes(long? bytes)
        {
            if (bytes == null || bytes == 0)
                return "0 B";

            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes.Value;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return $"{len:F2} {sizes[order]}";
        }

        private string FormatDuration(long? seconds)
        {
            if (seconds == null || seconds == 0)
                return "0s";

            long sec = seconds.Value;
            long hours = sec / 3600;
            long minutes = (sec % 3600) / 60;
            long secs = sec % 60;

            if (hours > 0)
                return $"{hours}h {minutes}m {secs}s";
            else if (minutes > 0)
                return $"{minutes}m {secs}s";
            else
                return $"{secs}s";
        }
    }
}
