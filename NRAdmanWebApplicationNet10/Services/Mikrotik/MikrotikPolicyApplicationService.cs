using Microsoft.EntityFrameworkCore;
using NRAdmanWebApplicationNet10.Models;
using System.Text.Json;

namespace NRAdmanWebApplicationNet10.Services.Mikrotik
{
    /// <summary>
    /// DTO untuk Mikrotik Queue Command
    /// </summary>
    public class MikrotikQueueCommand
    {
        public string? QueueName { get; set; }
        public string? TargetAddress { get; set; }
        public string? MaxLimitDown { get; set; }
        public string? MaxLimitUp { get; set; }
        public string? BurstLimitDown { get; set; }
        public string? BurstLimitUp { get; set; }
        public int? BurstThresholdDown { get; set; }
        public int? BurstThresholdUp { get; set; }
        public int? BurstTime { get; set; }
        public int? Priority { get; set; }
        public bool? IsDisabled { get; set; }
        public string? Parent { get; set; } // untuk PCQ (Per Connection Queue)

        /// <summary>
        /// Generate Mikrotik API command dari policy
        /// </summary>
        public Dictionary<string, string> ToMikrotikAttributes()
        {
            var attrs = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(QueueName))
                attrs["name"] = QueueName;

            if (!string.IsNullOrEmpty(TargetAddress))
                attrs["target"] = TargetAddress;

            if (!string.IsNullOrEmpty(MaxLimitDown) && !string.IsNullOrEmpty(MaxLimitUp))
                attrs["max-limit"] = $"{MaxLimitDown}/{MaxLimitUp}";

            if (!string.IsNullOrEmpty(BurstLimitDown) && !string.IsNullOrEmpty(BurstLimitUp))
                attrs["burst-limit"] = $"{BurstLimitDown}/{BurstLimitUp}";

            if (BurstThresholdDown.HasValue && BurstThresholdUp.HasValue)
                attrs["burst-threshold"] = $"{BurstThresholdDown}/{BurstThresholdUp}";

            if (BurstTime.HasValue)
                attrs["burst-time"] = $"{BurstTime}s";

            if (Priority.HasValue)
                attrs["priority"] = Priority.ToString();

            if (IsDisabled.HasValue)
                attrs["disabled"] = IsDisabled.Value ? "true" : "false";

            if (!string.IsNullOrEmpty(Parent))
                attrs["parent"] = Parent;

            return attrs;
        }
    }

    /// <summary>
    /// Service untuk mapping policy aplikasi ke format Mikrotik queue command
    /// </summary>
    public class MikrotikPolicyApplicationService
    {
        private readonly ILogger<MikrotikPolicyApplicationService> _logger;

        public MikrotikPolicyApplicationService(ILogger<MikrotikPolicyApplicationService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Convert MikrotikRadiusPolicy ke MikrotikQueueCommand
        /// </summary>
        public MikrotikQueueCommand ConvertPolicyToQueueCommand(
            MikrotikRadiusPolicy policy,
            string targetAddress,
            string? customQueueName = null)
        {
            try
            {
                var queueName = customQueueName ?? $"Q-{policy.PolicyName}-{Guid.NewGuid().ToString().Substring(0, 6)}";

                var command = new MikrotikQueueCommand
                {
                    QueueName = queueName,
                    TargetAddress = targetAddress,
                    MaxLimitDown = policy.DownloadLimit > 0 ? $"{policy.DownloadLimit}M" : "Unlimited",
                    MaxLimitUp = policy.UploadLimit > 0 ? $"{policy.UploadLimit}M" : "Unlimited",
                    BurstLimitDown = policy.BurstLimitDown > 0 ? $"{policy.BurstLimitDown}M" : "-",
                    BurstLimitUp = policy.BurstLimitUp > 0 ? $"{policy.BurstLimitUp}M" : "-",
                    BurstThresholdDown = policy.BurstThresholdDown,
                    BurstThresholdUp = policy.BurstThresholdUp,
                    BurstTime = policy.BurstTime,
                    Priority = policy.Priority,
                    IsDisabled = !policy.IsActive
                };

                _logger.LogInformation($"Policy '{policy.PolicyName}' dikonversi ke queue command untuk {targetAddress}");
                return command;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error mengkonversi policy '{policy.PolicyName}'");
                throw;
            }
        }

        /// <summary>
        /// Validasi apakah policy dapat diterapkan ke router
        /// </summary>
        public (bool IsValid, List<string> Errors) ValidatePolicyApplication(
            MikrotikRadiusPolicy policy,
            string targetAddress)
        {
            var errors = new List<string>();

            // Validasi policy
            if (policy == null)
                errors.Add("Policy tidak ditemukan");

            if (!policy.IsActive)
                errors.Add("Policy tidak aktif");

            // Validasi target address
            if (string.IsNullOrEmpty(targetAddress))
                errors.Add("Target address tidak boleh kosong");

            // Validasi bandwidth settings
            if (policy.DownloadLimit > 0)
                errors.Add("Download limit harus lebih besar dari 0");

            if (policy.UploadLimit > 0)
                errors.Add("Upload limit harus lebih besar dari 0");

            // Validasi burst settings jika ada
            if (policy.BurstLimitDown > 0)
                errors.Add("Burst limit down harus lebih besar dari 0");

            if (policy.BurstLimitUp > 0)
                errors.Add("Burst limit up harus lebih besar dari 0");

            // Validasi priority
            if (policy.Priority < 1 || policy.Priority > 16)
                errors.Add("Priority harus antara 1-16");

            return (errors.Count == 0, errors);
        }

        /// <summary>
        /// Generate deployment script untuk multiple routers
        /// </summary>
        public string GenerateDeploymentScript(
            MikrotikRadiusPolicy policy,
            List<(NetworkRouter Router, string TargetAddress)> deployments)
        {
            try
            {
                var script = "# Deployment Script untuk Policy: " + policy.PolicyName + "\n";
                script += "# Generated: " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + "\n\n";

                foreach (var (router, targetAddress) in deployments)
                {
                    script += $"# Router: {router.Name} ({router.IpAddress})\n";
                    var command = ConvertPolicyToQueueCommand(policy, targetAddress);
                    var attrs = command.ToMikrotikAttributes();

                    script += "/queue/simple/add ";
                    script += string.Join(" ", attrs.Select(kvp => $"{kvp.Key}=\"{kvp.Value}\""));
                    script += "\n\n";
                }

                return script;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating deployment script");
                throw;
            }
        }

        /// <summary>
        /// Calculate impact dari policy change pada existing queues
        /// </summary>
        public Dictionary<string, object> CalculatePolicyImpact(
            MikrotikRadiusPolicy oldPolicy,
            MikrotikRadiusPolicy newPolicy)
        {
            var impact = new Dictionary<string, object>
            {
                { "policy-name", newPolicy.PolicyName },
                { "changes", new List<string>() }
            };

            var changes = (List<string>)impact["changes"];

            if (oldPolicy.DownloadLimit != newPolicy.DownloadLimit)
                changes.Add($"Download limit: {oldPolicy.DownloadLimit}Mbps → {newPolicy.DownloadLimit}Mbps");

            if (oldPolicy.UploadLimit != newPolicy.UploadLimit)
                changes.Add($"Upload limit: {oldPolicy.UploadLimit}Mbps → {newPolicy.UploadLimit}Mbps");

            if (oldPolicy.Priority != newPolicy.Priority)
                changes.Add($"Priority: {oldPolicy.Priority} → {newPolicy.Priority}");

            if (oldPolicy.BurstLimitDown != newPolicy.BurstLimitDown || oldPolicy.BurstLimitUp != newPolicy.BurstLimitUp)
                changes.Add($"Burst limits changed");

            if (oldPolicy.IsActive != newPolicy.IsActive)
                changes.Add($"Status: {(newPolicy.IsActive ? "Activated" : "Deactivated")}");

            impact["change-count"] = changes.Count;
            return impact;
        }

        /// <summary>
        /// Generate rollback script
        /// </summary>
        public string GenerateRollbackScript(List<MikrotikQueueConfig> queueConfigs)
        {
            try
            {
                var script = "# Rollback Script\n";
                script += "# Generated: " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + "\n\n";

                foreach (var config in queueConfigs)
                {
                    if (!string.IsNullOrEmpty(config.MikrotikQueueId) && !string.IsNullOrEmpty(config.Router?.IpAddress))
                    {
                        script += $"# Remove queue '{config.QueueName}' from {config.Router.IpAddress}\n";
                        script += $"/queue/simple/remove [find id={config.MikrotikQueueId}]\n\n";
                    }
                }

                return script;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating rollback script");
                throw;
            }
        }
    }
}
