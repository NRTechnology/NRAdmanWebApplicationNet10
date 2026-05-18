using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NRAdmanWebApplicationNet10.Services;
using NRAdmanWebApplicationNet10.Models;
using NRAdmanWebApplicationNet10.ViewModels;

namespace NRAdmanWebApplicationNet10.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = "Administrator")]
    public class RouterQueueController(ApplicationDbContext applicationDbContext,
        IWebHostEnvironment environment, ILogger<RouterQueueController> logger,
        IConfiguration configuration) : Controller
    {
        public IActionResult Index()
        {
            return View("RouterQueueList");
        }

        [HttpGet]
        public IActionResult GetJsonResult()
        {
            try
            {
                var data = applicationDbContext.MikrotikSimpleQueues
                    .Join(applicationDbContext.Nas,
                        queue => queue.NasId,
                        nas => nas.Id,
                        (queue, nas) => new
                        {
                            id = queue.Id,
                            nasId = queue.NasId,
                            nasName = nas.NasName,
                            queueName = queue.QueueName,
                            targetAddress = queue.TargetAddress,
                            parent = queue.Parent,
                            maxLimit = queue.MaxLimit,
                            burstLimit = queue.BurstLimit,
                            burstThreshold = queue.BurstThreshold,
                            burstTime = queue.BurstTime,
                            priority = queue.Priority,
                            packetMark = queue.PacketMark,
                            comment = queue.Comment,
                            disabled = queue.Disabled,
                            createdAt = queue.CreatedAt,
                            createdBy = queue.CreatedBy
                        })
                    .OrderByDescending(q => q.createdAt)
                    .ToList();

                return Json(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Gagal mengambil data queue");
                return Json(new List<object>());
            }
        }

        [HttpGet]
        public IActionResult CreateModal()
        {
            var nasOptions = applicationDbContext.Nas
                .Select(n => new { id = n.Id, nasName = n.NasName })
                .ToList();

            ViewBag.NasOptions = nasOptions;
            var viewModel = new MikrotikSimpleQueueViewModel();
            return PartialView("../Shared/_Modals/_ModalCreateRouterQueue", viewModel);
        }

        [HttpGet]
        public IActionResult EditModal(int id)
        {
            var queue = applicationDbContext.MikrotikSimpleQueues.FirstOrDefault(q => q.Id == id);
            if (queue == null)
            {
                return NotFound();
            }

            var nasOptions = applicationDbContext.Nas
                .Select(n => new { id = n.Id, nasName = n.NasName })
                .ToList();

            ViewBag.NasOptions = nasOptions;

            var viewModel = new MikrotikSimpleQueueViewModel
            {
                Id = queue.Id,
                NasId = queue.NasId,
                NasName = applicationDbContext.Nas.FirstOrDefault(n => n.Id == queue.NasId)?.NasName,
                QueueName = queue.QueueName,
                TargetAddress = queue.TargetAddress,
                Parent = queue.Parent,
                MaxLimit = queue.MaxLimit,
                BurstLimit = queue.BurstLimit,
                BurstThreshold = queue.BurstThreshold,
                BurstTime = queue.BurstTime,
                Priority = queue.Priority,
                PacketMark = queue.PacketMark,
                Comment = queue.Comment,
                Disabled = queue.Disabled
            };

            return PartialView("../Shared/_Modals/_ModalEditRouterQueue", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MikrotikSimpleQueueViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                var errorList = errors.Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Validasi gagal.", errors = errorList });
            }

            // Check if queue name already exists for this NAS
            if (applicationDbContext.MikrotikSimpleQueues.Any(q => 
                q.NasId == model.NasId && q.QueueName == model.QueueName))
            {
                return Json(new { success = false, message = "Queue Name sudah ada untuk NAS ini." });
            }

            try
            {
                var entity = new MikrotikSimpleQueue
                {
                    NasId = model.NasId,
                    QueueName = model.QueueName,
                    TargetAddress = model.TargetAddress,
                    Parent = model.Parent,
                    MaxLimit = model.MaxLimit,
                    BurstLimit = model.BurstLimit,
                    BurstThreshold = model.BurstThreshold,
                    BurstTime = model.BurstTime,
                    Priority = model.Priority,
                    PacketMark = model.PacketMark,
                    Comment = model.Comment,
                    Disabled = model.Disabled,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = User.Identity?.Name
                };

                applicationDbContext.MikrotikSimpleQueues.Add(entity);
                applicationDbContext.SaveChanges();

                logger.LogInformation("Queue {QueueName} berhasil dibuat untuk NAS ID {NasId} oleh {AdminUser}", 
                    model.QueueName, model.NasId, User.Identity?.Name);
                return Json(new { success = true, message = "Queue berhasil ditambahkan." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Gagal menyimpan Queue {QueueName}", model.QueueName);
                return Json(new { success = false, message = "Gagal menyimpan data Queue. Silakan coba lagi." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, MikrotikSimpleQueueViewModel model)
        {
            if (id != model.Id)
            {
                return Json(new { success = false, message = "ID tidak sesuai." });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                var errorList = errors.Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Validasi gagal.", errors = errorList });
            }

            // Check if queue name already exists for this NAS (exclude current record)
            if (applicationDbContext.MikrotikSimpleQueues.Any(q => 
                q.NasId == model.NasId && q.QueueName == model.QueueName && q.Id != id))
            {
                return Json(new { success = false, message = "Queue Name sudah ada untuk NAS ini." });
            }

            try
            {
                var entity = applicationDbContext.MikrotikSimpleQueues.FirstOrDefault(q => q.Id == id);
                if (entity == null)
                {
                    return Json(new { success = false, message = "Data Queue tidak ditemukan." });
                }

                entity.NasId = model.NasId;
                entity.QueueName = model.QueueName;
                entity.TargetAddress = model.TargetAddress;
                entity.Parent = model.Parent;
                entity.MaxLimit = model.MaxLimit;
                entity.BurstLimit = model.BurstLimit;
                entity.BurstThreshold = model.BurstThreshold;
                entity.BurstTime = model.BurstTime;
                entity.Priority = model.Priority;
                entity.PacketMark = model.PacketMark;
                entity.Comment = model.Comment;
                entity.Disabled = model.Disabled;
                entity.UpdatedAt = DateTime.UtcNow;
                entity.UpdatedBy = User.Identity?.Name;

                applicationDbContext.MikrotikSimpleQueues.Update(entity);
                applicationDbContext.SaveChanges();

                logger.LogInformation("Queue {QueueName} berhasil diperbarui oleh {AdminUser}", 
                    model.QueueName, User.Identity?.Name);
                return Json(new { success = true, message = "Queue berhasil diperbarui." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Gagal memperbarui Queue dengan ID {QueueId}", id);
                return Json(new { success = false, message = "Gagal memperbarui data Queue. Silakan coba lagi." });
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                var entity = applicationDbContext.MikrotikSimpleQueues.FirstOrDefault(q => q.Id == id);
                if (entity == null)
                {
                    return Json(new { success = false, message = "Data Queue tidak ditemukan." });
                }

                applicationDbContext.MikrotikSimpleQueues.Remove(entity);
                applicationDbContext.SaveChanges();

                logger.LogInformation("Queue {QueueName} berhasil dihapus oleh {AdminUser}", 
                    entity.QueueName, User.Identity?.Name);
                return Json(new { success = true, message = "Queue berhasil dihapus." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Gagal menghapus Queue dengan ID {QueueId}", id);
                return Json(new { success = false, message = "Gagal menghapus data Queue. Silakan coba lagi." });
            }
        }
    }
}
