# Mikrotik RADIUS Management & Synchronization System
## Algoritma Implementasi Lengkap

### 1. PENDAHULUAN

Sistem ini didesain untuk mengelola dan mensinkronisasi RADIUS policies serta accounting data pada Mikrotik Router menggunakan Mikrotik API yang ter-enkripsi SSL (port 8729).

**Tujuan Utama:**
- Terpusat-kan manajemen bandwidth/QoS rules di aplikasi
- Sinkronisasi otomatis status queues ke database
- Tracking deployment history dan rollback capability
- Collect accounting data untuk reporting

---

### 2. ARSITEKTUR SISTEM

```
┌─────────────────────────────────────────────────────────────┐
│                    ASP.NET Core Application                  │
├─────────────────────────────────────────────────────────────┤
│                                                               │
│  ┌──────────────────────────────────────────────────────┐   │
│  │           MikrotikRadiusDeploymentController         │   │
│  │  - Deploy(), ExecuteDeploy(), Rollback(),            │   │
│  │  - SyncStatus(), PullAccounting()                     │   │
│  └──────────────────────────────────────────────────────┘   │
│                          ↓                                    │
│  ┌──────────────────────────────────────────────────────┐   │
│  │            MikrotikSyncService (Orchestrator)        │   │
│  │  - DeployPolicyToRouterAsync()                       │   │
│  │  - SyncQueueStatusAsync()                            │   │
│  │  - PullAccountingDataAsync()                         │   │
│  │  - RollbackDeploymentAsync()                         │   │
│  └──────────────────────────────────────────────────────┘   │
│           ↙                              ↘                    │
│  ┌──────────────────────┐    ┌──────────────────────────┐   │
│  │ MikrotikApiService   │    │ MikrotikPolicyApplication   │   │
│  │ (SSL API Client)     │    │ Service (Script Generator)   │   │
│  │ - TestConnection()   │    │ - ConvertPolicyToCommand()   │   │
│  │ - CreateQueue()      │    │ - GenerateDeployScript()     │   │
│  │ - GetQueues()        │    │ - GenerateRollbackScript()   │   │
│  │ - GetQueueStats()    │    │ - ValidatePolicyApplication()│   │
│  └──────────────────────┘    └──────────────────────────┘   │
│           ↓                              ↓                    │
│  ┌─────────────────────────────────────────────────────┐    │
│  │          ApplicationDbContext (EF Core)             │    │
│  │  - MikrotikRadiusPolicies                           │    │
│  │  - MikrotikQueueConfigs (Deployment Tracking)      │    │
│  │  - MikrotikRadiusAccounting (Usage Data)            │    │
│  └─────────────────────────────────────────────────────┘    │
│           ↓                                                   │
│  ┌─────────────────────────────────────────────────────┐    │
│  │         PostgreSQL Database (Npgsql)                │    │
│  └─────────────────────────────────────────────────────┘    │
│                                                               │
└─────────────────────────────────────────────────────────────┘
							↕ (SSL/TLS Port 8729)
┌─────────────────────────────────────────────────────────────┐
│               Mikrotik Router (API Server)                   │
├─────────────────────────────────────────────────────────────┤
│                                                               │
│  /queue/simple (Queue Management)                            │
│  /ip/firewall/filter (Rules)                                 │
│  /interface (Stats & Monitoring)                             │
│                                                               │
└─────────────────────────────────────────────────────────────┘
```

---

### 3. ENTITIES & DATABASE SCHEMA

#### 3.1 MikrotikRadiusPolicy
```sql
CREATE TABLE mikrotik_radius_policies (
	id SERIAL PRIMARY KEY,
	policy_name VARCHAR(255) NOT NULL UNIQUE,
	description VARCHAR(500),
	download_limit DECIMAL(10,2),  -- Mbps
	upload_limit DECIMAL(10,2),    -- Mbps
	burst_limit_down DECIMAL(10,2),-- Mbps
	burst_limit_up DECIMAL(10,2),  -- Mbps
	burst_threshold_down INT,       -- %
	burst_threshold_up INT,         -- %
	burst_time INT,                 -- seconds
	priority INT DEFAULT 8,         -- 1-16
	is_active BOOLEAN DEFAULT true,
	created_date TIMESTAMP,
	modified_date TIMESTAMP,
	created_by VARCHAR(255),
	modified_by VARCHAR(255)
);
```

#### 3.2 MikrotikQueueConfig (Deployment Tracking)
```sql
CREATE TABLE mikrotik_queue_config (
	id SERIAL PRIMARY KEY,
	router_id UUID REFERENCES routers(id),
	policy_id INT REFERENCES mikrotik_radius_policies(id),
	mikrotik_queue_id VARCHAR(255),     -- ID dari Mikrotik
	queue_name VARCHAR(255),             -- RADIUS-{PolicyId}-{RouterName}
	target_address VARCHAR(50),          -- IP target
	deployment_status VARCHAR(50),       -- Pending|Deployed|Failed|RolledBack
	sync_status VARCHAR(50),             -- Synced|OutOfSync|Error
	last_error TEXT,
	deployed_date TIMESTAMP,
	last_sync_date TIMESTAMP,
	config_metadata JSONB,               -- Backup config
	created_by VARCHAR(255),
	modified_by VARCHAR(255)
);
```

#### 3.3 MikrotikRadiusAccounting
```sql
CREATE TABLE mikrotik_radius_accounting (
	id SERIAL PRIMARY KEY,
	username VARCHAR(255),
	nas_ip_address VARCHAR(50),
	acct_input_octets BIGINT,
	acct_output_octets BIGINT,
	acct_input_packets BIGINT,
	acct_output_packets BIGINT,
	acct_session_time BIGINT,         -- seconds
	acct_status_type VARCHAR(50),     -- Interim-Update|Stop
	acct_session_id VARCHAR(255),
	acct_terminate_cause VARCHAR(100),
	created_date TIMESTAMP
);
```

---

### 4. FLOW DEPLOYMENT

#### Step 1: User Membuat Policy
```csharp
// User creates MikrotikRadiusPolicy
var policy = new MikrotikRadiusPolicy
{
	PolicyName = "PREMIUM_100M",
	DownloadLimit = 100,      // Mbps
	UploadLimit = 50,         // Mbps
	BurstLimitDown = 200,     // Mbps
	BurstLimitUp = 100,       // Mbps
	Priority = 5,
	IsActive = true
};
_db.MikrotikRadiusPolicies.Add(policy);
await _db.SaveChangesAsync();
```

#### Step 2: User Deploy ke Router(s)
```csharp
// UI form input:
// - Select Policy
// - Select Routers (multi-select)
// - Input target addresses per router

var deploymentResult = await _syncService.DeployPolicyToMultipleRoutersAsync(
	policyId: 5,
	routerIds: new[] { routerId1, routerId2 },
	targetAddresses: new[] { "192.168.100.50", "192.168.100.51" }
);

// Result:
// {
//   Success: true,
//   DeployedCount: 2,
//   FailedCount: 0,
//   Errors: []
// }
```

#### Step 3: System Processing
```
1. For each (router, target):
   a) Validate connection settings
   b) Test SSL connection to Mikrotik API
   c) Convert policy to Mikrotik command
	  - Download 100 Mbps = 100,000,000 bps
	  - Create simple-queue with targeting
   d) Apply command via API
   e) Get queue ID dari Mikrotik response
   f) Save MikrotikQueueConfig tracking record
	  - deployment_status = "Deployed"
	  - deployed_date = now
	  - config_metadata = backup config JSON
```

#### Step 4: Mikrotik Queue Creation
```
Mikrotik API call:
/queue/simple/add name=RADIUS-5-Router1 \
  target=192.168.100.50 \
  max-limit=100M/50M \
  burst-limit=200M/100M \
  burst-threshold=50/50 \
  burst-time=10 \
  priority=5

Response:
{
  ".id": "*1",
  "name": "RADIUS-5-Router1",
  "target": "192.168.100.50",
  "max-limit": "100M/50M",
  ...
}
```

---

### 5. FLOW SYNCHRONIZATION

#### 5.1 SyncQueueStatusAsync (Periodic - every 5 minutes)

```csharp
var syncResult = await _syncService.SyncQueueStatusAsync();
```

**Processing:**
```
1. For each active Router:
   a) Connect via SSL (port 8729)
   b) /queue/simple/print → get all queues
   c) Find queues matching pattern "RADIUS-*"
   d) For each matched queue:
	  - Check if entry exists in MikrotikQueueConfig
	  - If exists: update sync_status = "Synced", last_sync_date = now
	  - If not exists: mark as Out-Of-Sync (manual intervention needed)
	  - If deployment was expected but queue missing: mark as Failed
   e) Close connection

2. Return { QueuesSynced: X, Errors: [] }
```

#### 5.2 PullAccountingDataAsync (Periodic - every 1 hour)

```csharp
var acctResult = await _syncService.PullAccountingDataAsync(routerId);
```

**Processing:**
```
1. For router in list:
   a) Connect via SSL API
   b) For each deployed queue:
	  - Call /queue/simple/get with queue ID
	  - Extract: bytes-in, bytes-out, packets-in, packets-out
	  - Call /queue/simple/get-stats untuk session details
   c) Create MikrotikRadiusAccounting record:
	  {
		username: "Queue-{QueueName}",
		nas_ip_address: router.IpAddress,
		acct_input_octets: bytes_in,
		acct_output_octets: bytes_out,
		acct_input_packets: packets_in,
		acct_output_packets: packets_out,
		acct_session_time: uptime_seconds,
		acct_status_type: "Interim-Update",
		created_date: now
	  }
   d) Save to database
   e) Close connection

2. Return { QueuesSynced: X, RecordsCaptured: Y }
```

---

### 6. FLOW ROLLBACK

```csharp
var rollbackResult = await _syncService.RollbackDeploymentAsync(
	configIds: new[] { configId1, configId2 }
);
```

**Processing:**
```
1. For each configId:
   a) Load MikrotikQueueConfig record
   b) Load related Router
   c) Connect via SSL API
   d) Execute Mikrotik command:
	  /queue/simple/remove [find id={queue_id}]
   e) Update MikrotikQueueConfig:
	  - deployment_status = "RolledBack"
	  - sync_status = "Synced"
	  - last_sync_date = now
   f) Log rollback action (audit trail)
   g) Close connection

2. Return { RolledBackCount: X, FailedCount: Y }
```

---

### 7. SSL/TLS CONNECTION

#### 7.1 Connection Settings
```csharp
var connSettings = new MikrotikConnectionSettings
{
	RouterName = "ISP-Router-1",
	ApiHost = "192.168.1.1",
	ApiPort = 8729,                  // Mikrotik SSL API default
	ApiUsername = "admin",
	ApiPassword = "encrypted_pwd",
	UseSSL = true,
	IgnoreCertificate = true,        // For self-signed certs (dev/test)
	ConnectionTimeout = 10000        // 10 seconds
};
```

#### 7.2 Certificate Handling
```csharp
// Development/Testing:
ServicePointManager.ServerCertificateValidationCallback = 
	(sender, cert, chain, errors) => true;  // Accept all

// Production:
// ServicePointManager.ServerCertificateValidationCallback = 
//     (sender, cert, chain, errors) => 
//     {
//         // Implement certificate pinning
//         // Validate against known cert thumbprint
//         return ValidateCertificateThumbprint(cert);
//     };
```

---

### 8. ERROR HANDLING

#### 8.1 Connection Failures
```csharp
try
{
	var connTest = await _apiService.TestConnectionAsync(connSettings);
	if (!connTest.Success)
	{
		// Log error
		// Mark config as Failed
		// Queue for retry
		result.Errors.Add($"Router {router.Name}: {connTest.ErrorMessage}");
		result.FailedOperations++;
		continue;  // Try next router
	}
}
catch (Exception ex)
{
	_logger.LogError(ex, "Connection error to {RouterName}", router.Name);
	result.Errors.Add($"Router {router.Name}: {ex.Message}");
	result.FailedOperations++;
}
```

#### 8.2 Retry Strategy
```
Attempt 1: Immediate
Attempt 2: Wait 5 seconds
Attempt 3: Wait 15 seconds
Max Retries: 3

If all fail:
- Mark deployment as Failed
- Send alert notification
- Allow manual retry
```

#### 8.3 Partial Failures
```
Deploy to 5 routers:
- Router A: Success
- Router B: Failed (connection timeout)
- Router C: Success
- Router D: Success
- Router E: Failed (API error)

Result:
{
  Success: true,                    // Partial success
  DeployedCount: 3,
  FailedCount: 2,
  Errors: [
	"Router B: Connection timeout",
	"Router E: API error (500)"
  ]
}

User can:
- View detailed errors
- Retry failed routers
- Rollback successful ones if needed
```

---

### 9. SECURITY CONSIDERATIONS

#### 9.1 API Credentials Storage
```csharp
// DO NOT store plaintext in database
// Use ASP.NET Core Configuration encryption

// In appsettings.json (encrypted):
{
  "Mikrotik": {
	"Routers": [
	  {
		"Name": "ISP-1",
		"ApiHost": "192.168.1.1",
		"ApiUsername": "admin",
		"ApiPassword": "ENCRYPTED_VALUE",  // Encrypt at rest
		"ApiPort": 8729
	  }
	]
  }
}

// Load from configuration:
var settings = _configuration.GetSection("Mikrotik:Routers");
```

#### 9.2 Audit Trail
```csharp
// Log all deployments/rollbacks
_logger.LogInformation(
	"Policy {PolicyId} deployed to router {RouterId} by user {UserId} at {Timestamp}",
	policyId, routerId, User.Identity.Name, DateTime.UtcNow
);

// Store in audit table
var auditRecord = new AuditLog
{
	Action = "DEPLOY_POLICY",
	PolicyId = policyId,
	RouterId = routerId,
	UserId = User.Identity.Name,
	Timestamp = DateTime.UtcNow,
	Details = JsonConvert.SerializeObject(deploymentResult)
};
_db.AuditLogs.Add(auditRecord);
```

#### 9.3 Authorization
```csharp
// Only admins can deploy policies
[Authorize(Roles = "Administrator")]
public async Task<IActionResult> ExecuteDeploy(int policyId, ...)
{
	// ...
}

// Track who deployed what
config.CreatedBy = User.Identity.Name;
config.ModifiedBy = User.Identity.Name;
```

---

### 10. MONITORING & MAINTENANCE

#### 10.1 Periodic Tasks

| Task | Frequency | Purpose |
|------|-----------|---------|
| SyncQueueStatusAsync | Every 5 min | Verify deployed queues exist |
| PullAccountingDataAsync | Every 1 hour | Collect usage statistics |
| VerifyDeploymentIntegrity | Daily | Check local DB vs Mikrotik |
| CleanupExpiredRecords | Weekly | Archive old accounting records |
| CheckFailedDeployments | Daily | Alert on failures |

#### 10.2 Monitoring Dashboard (UI)

```
┌─ Deployment Status ─────────────────┐
│ Total Policies: 25                   │
│ Deployed: 23                         │
│ Failed: 2                            │
│ Pending: 0                           │
└──────────────────────────────────────┘

┌─ Queue Status ──────────────────────┐
│ Total Queues: 47                     │
│ Synced: 45                           │
│ Out-Of-Sync: 2                       │
│ Errors: 0                            │
│ Last Sync: 2 minutes ago             │
└──────────────────────────────────────┘

┌─ Router Health ─────────────────────┐
│ ISP-1: Connected (47 queues)         │
│ ISP-2: Connected (25 queues)         │
│ ISP-3: Disconnected ⚠️               │
│ ISP-4: Connected (30 queues)         │
└──────────────────────────────────────┘
```

---

### 11. TROUBLESHOOTING

#### Issue: Queue deployed but not visible in Mikrotik
```
Solution:
1. Check MikrotikQueueConfig.deployment_status
2. Verify target address is correct
3. Test manual connection to router
4. Check Mikrotik API firewall rules
5. Review Mikrotik logs
```

#### Issue: Sync showing Out-Of-Sync
```
Solution:
1. Manually delete queue on Mikrotik
2. Click "Retry Deploy" in UI
3. Or "Rollback" and redeploy
```

#### Issue: SSL Certificate errors
```
Solution (Dev):
- Set IgnoreCertificate = true in MikrotikConnectionSettings

Solution (Prod):
- Install valid certificate on Mikrotik
- Implement certificate pinning in application
```

---

### 12. CONTOH API COMMANDS

#### Create Simple Queue
```
/queue/simple/add name=RADIUS-5-Router1 target=192.168.100.50 \
  max-limit=100M/50M burst-limit=200M/100M \
  burst-threshold=50/50 burst-time=10 priority=5
```

#### Get All Queues
```
/queue/simple/print
```

#### Get Queue Details
```
/queue/simple/get [/queue/simple/find name=RADIUS-5-Router1]
```

#### Update Queue
```
/queue/simple/set [/queue/simple/find name=RADIUS-5-Router1] \
  max-limit=150M/75M
```

#### Remove Queue
```
/queue/simple/remove [/queue/simple/find name=RADIUS-5-Router1]
```

#### Get Interface Stats
```
/interface/get [/interface/find name=ether1] stats
```

---

**Dokumen ini menjelaskan complete algoritma & implementasi untuk Mikrotik RADIUS Management System menggunakan SSL-encrypted API connections.**
