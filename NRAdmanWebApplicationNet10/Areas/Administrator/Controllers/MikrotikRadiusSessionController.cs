using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NRAdmanWebApplicationNet10.Services;
using NRAdmanWebApplicationNet10.ViewModels;

namespace NRAdmanWebApplicationNet10.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = "Administrator")]
    public class MikrotikRadiusSessionController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<MikrotikRadiusSessionController> _logger;

        public MikrotikRadiusSessionController(ApplicationDbContext db, ILogger<MikrotikRadiusSessionController> logger)
        {
            _db = db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("MikrotikRadiusAccountingList");
        }

        [HttpGet]
        public IActionResult GetJsonResult()
        {
            try
            {
                var data = _db.MikrotikRadiusAccounting
                    .OrderByDescending(s => s.AcctStartTime)
                    .Select(s => new
                    {
                        id = s.Id,
                        username = s.Username,
                        nasIpAddress = s.NasIpAddress,
                        sessionId = s.AcctSessionId,
                        startTime = s.AcctStartTime.HasValue ? s.AcctStartTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "-",
                        stopTime = s.AcctStopTime.HasValue ? s.AcctStopTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "-",
                        duration = s.AcctSessionTime.HasValue ? FormatDuration(s.AcctSessionTime.Value) : "-",
                        inputData = FormatBytes(s.AcctInputOctets ?? 0),
                        outputData = FormatBytes(s.AcctOutputOctets ?? 0),
                        totalData = FormatBytes((s.AcctInputOctets ?? 0) + (s.AcctOutputOctets ?? 0)),
                        framedIp = s.FramedIpAddress ?? "-",
                        statusType = s.AcctStatusType ?? "-",
                        terminateCause = s.AcctTerminateCause ?? "-"
                    }).ToList();

                return Json(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Radius accounting data");
                return Json(new { error = "Error loading data" });
            }
        }

        [HttpGet]
        public IActionResult Detail(long id)
        {
            var acct = _db.MikrotikRadiusAccounting.FirstOrDefault(s => s.Id == id);
            if (acct == null)
                return NotFound();

            var model = new MikrotikRadiusAccountingViewModel
            {
                Id = acct.Id,
                Username = acct.Username,
                NasIpAddress = acct.NasIpAddress,
                NasPort = acct.NasPort,
                AcctSessionId = acct.AcctSessionId,
                AcctStartTime = acct.AcctStartTime,
                AcctStopTime = acct.AcctStopTime,
                AcctSessionTime = acct.AcctSessionTime,
                AcctInputOctets = acct.AcctInputOctets,
                AcctOutputOctets = acct.AcctOutputOctets,
                AcctInputPackets = acct.AcctInputPackets,
                AcctOutputPackets = acct.AcctOutputPackets,
                FramedIpAddress = acct.FramedIpAddress,
                CalledStationId = acct.CalledStationId,
                CallingStationId = acct.CallingStationId,
                AcctTerminateCause = acct.AcctTerminateCause,
                AcctStatusType = acct.AcctStatusType,
                CreatedDate = acct.CreatedDate
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteOldSessions(int days = 30)
        {
            try
            {
                var cutoffDate = DateTime.UtcNow.AddDays(-days);
                var sessionsToDelete = _db.MikrotikRadiusAccounting
                    .Where(s => s.CreatedDate < cutoffDate && s.AcctStatusType == "Stop")
                    .ToList();

                _db.MikrotikRadiusAccounting.RemoveRange(sessionsToDelete);
                _db.SaveChanges();

                return Json(new { success = true, message = $"{sessionsToDelete.Count} accounting record(s) dihapus." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting old accounting records");
                return Json(new { success = false, message = "Gagal menghapus record." });
            }
        }

        private static string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        private static string FormatDuration(long seconds)
        {
            long hours = seconds / 3600;
            long minutes = (seconds % 3600) / 60;
            long secs = seconds % 60;

            if (hours > 0)
                return $"{hours}h {minutes}m {secs}s";
            else if (minutes > 0)
                return $"{minutes}m {secs}s";
            else
                return $"{secs}s";
        }
    }
}
