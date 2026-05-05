using System;

namespace NRAdmanWebApplicationNet10.Models
{
    public class LoginAttempt
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        // Nullable user id - unknown user if username not found
        public string? UserId { get; set; }

        // IP address of the requester
        public string? IpAddress { get; set; }

        // Time of attempt (UTC)
        public DateTime AttemptedAt { get; set; } = DateTime.UtcNow;

        // Whether the attempt succeeded
        public bool Success { get; set; }

        // Type of attempt: Password, TwoFactor, RecoveryCode, etc.
        public string? AttemptType { get; set; }

        // Optional detail or reason (e.g. InvalidPassword, LockedOut)
        public string? Details { get; set; }
    }
}
