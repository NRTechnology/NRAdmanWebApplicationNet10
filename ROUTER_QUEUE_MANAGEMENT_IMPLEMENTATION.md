# Router Queue Management Implementation

## Overview
Implemented a complete Router Queue Management system for Mikrotik Simple Queue in the Administrator panel. The implementation follows the same architecture pattern as SettingsNasController with comprehensive CRUD operations, robust error handling, and detailed logging.

## Files Created/Modified

### 1. Controller: RouterQueueController.cs
**Path:** `Areas/Administrator/Controllers/RouterQueueController.cs`

**Features:**
- ✅ `Index()` - Main page view
- ✅ `GetJsonResult()` - Returns JSON data for DataTable with NAS information joined
- ✅ `CreateModal()` - Returns partial view for create form with NAS dropdown
- ✅ `EditModal(id)` - Returns partial view for edit form with populated data
- ✅ `Create(model)` - POST handler with validation, duplicate check, and detailed error handling
- ✅ `Update(id, model)` - POST handler with ID validation, duplicate check, and error handling
- ✅ `Delete(id)` - POST handler for deletion with logging

**Key Features:**
- Authorization: `[Authorize(Roles = "Administrator")]`
- Duplicate validation for Queue Name per NAS
- Comprehensive logging for all operations
- Structured error responses with detailed error arrays
- NAS lookup with join operation for data display

### 2. View: RouterQueueList.cshtml
**Path:** `Areas/Administrator/Views/RouterQueue/RouterQueueList.cshtml`

**Features:**
- DataTable with 9 columns:
  - ID, NAS Name, Queue Name, Target Address, Max Limit, Priority, Status, Created Date, Actions
- Bandwidth formatting (bps → Kbps → Mbps → Gbps)
- Status badge (Active/Disabled)
- Responsive design with Bootstrap 5
- Add, Edit, Delete button handlers
- AJAX-based modal loading
- DataTable pagination with custom styling

**DataTable Configuration:**
- Server-side AJAX data source
- Responsive layout with proper column widths
- Custom button rendering for actions
- Automatic DataTable reload after CRUD operations

### 3. Modal: _ModalCreateRouterQueue.cshtml
**Path:** `Areas/Administrator/Views/Shared/_Modals/_ModalCreateRouterQueue.cshtml`

**Form Fields:**
- NAS Selection (dropdown from database)
- Queue Name (required, 3-255 chars)
- Target Address (required, IP/Subnet format validation)
- Parent Queue (optional)
- Max Limit (optional, bandwidth in bps)
- Burst Limit (optional)
- Burst Threshold (optional)
- Burst Time (optional, in seconds)
- Priority (required, 0-16 with labels)
- Packet Mark (optional)
- Comment (optional, up to 500 chars)
- Disable Queue (checkbox)

**Features:**
- Bootstrap Offcanvas modal
- AJAX form submission
- Multi-line error display in Toast
- Field-level error display (with is-invalid classes)
- Form reset after successful submission
- Anti-forgery token support
- Helper functions: `clearFieldErrors()`, `displayFieldErrors()`

### 4. Modal: _ModalEditRouterQueue.cshtml
**Path:** `Areas/Administrator/Views/Shared/_Modals/_ModalEditRouterQueue.cshtml`

**Features:**
- Same form fields as Create modal
- Pre-populated form values
- Hidden ID field for routing
- Improved UX with:
  - Status badge display
  - Field validation with error messages
  - Selected state for dropdown and priority select
  - Form error summary area

**Client-Side Functions:**
- `clearFieldErrors()` - Resets all error states
- `displayFieldErrors(errors)` - Maps server errors to form fields

## Validation & Error Handling

### Server-Side (ViewModel: MikrotikSimpleQueueViewModel)
```
✓ NasId - Required
✓ QueueName - Required, 3-255 chars, unique per NAS
✓ TargetAddress - Required, IP/Subnet format validation
✓ Parent - Optional, max 255 chars
✓ MaxLimit - Optional, 0 to long.MaxValue
✓ BurstLimit - Optional, 0 to long.MaxValue
✓ BurstThreshold - Optional, 0 to long.MaxValue
✓ BurstTime - Optional, 0 to int.MaxValue
✓ Priority - Required, 0-16
✓ PacketMark - Optional, max 255 chars
✓ Comment - Optional, max 500 chars
✓ Disabled - Optional boolean
```

### Error Response Format
```json
{
  "success": false,
  "message": "Validasi gagal.",
  "errors": [
    "Queue Name tidak boleh kosong.",
    "Target Address maksimal 50 karakter."
  ]
}
```

## Database Integration

### MikrotikSimpleQueue Model
- Table: `mikrotik_simple_queues`
- Foreign Key: NasId → Nas.Id
- Tracked fields: CreatedAt, UpdatedAt, CreatedBy, UpdatedBy
- Disabled field for soft disabling without deletion

### Related Models
- **Nas** - Foreign key relationship for NAS selection
- **MikrotikSimpleQueueViewModel** - Data transfer object with validation

## User Experience Features

### Toast Notifications
- ✅ Success: "Queue berhasil ditambahkan/diperbarui/dihapus"
- ✅ Error: Multi-line error messages from server
- ✅ Exception: "Terjadi kesalahan. Silakan coba lagi."

### Bandwidth Display Helper
```javascript
formatBandwidth(bytes) // Converts bps to Kbps/Mbps/Gbps
// Examples: 1000000 → "1.00 Mbps", 1000 → "1.00 Kbps"
```

### Status Badge
- 🟢 Active (green badge)
- 🔴 Disabled (red badge)

## Logging
All operations logged with:
- Operation type (Create/Update/Delete)
- Queue details (QueueName, NasId)
- Admin user (User.Identity.Name)
- Timestamps
- Errors with full exception details

## Security
- ✅ `[Authorize(Roles = "Administrator")]` on all actions
- ✅ `[ValidateAntiForgeryToken]` on POST operations
- ✅ ID validation (id must match model.Id)
- ✅ Duplicate prevention (unique Queue Name per NAS)
- ✅ Input validation using Data Annotations

## Testing Checklist
- [ ] View RouterQueueList page loads correctly
- [ ] Add New Queue button opens create modal
- [ ] Create modal has all fields with proper validation
- [ ] Submitting invalid form shows error Toast
- [ ] Successfully creating queue reloads DataTable
- [ ] Edit button opens edit modal with populated data
- [ ] Updating queue shows success Toast
- [ ] Delete button with confirmation works
- [ ] Toast displays multi-line error messages correctly
- [ ] NAS dropdown populates correctly
- [ ] Priority selector shows all 17 options (0-16)
- [ ] Bandwidth formatting displays correctly
- [ ] Status badge shows Active/Disabled correctly

## Integration Points
- Uses `ToastHelper` for notifications (must be included in layout)
- Uses DataTable for data display (must have DataTable scripts loaded)
- Requires `_Scripts` section with jQuery and Bootstrap
- Requires `ApplicationDbContext` with `MikrotikSimpleQueues` DbSet
- Requires MikrotikSimpleQueueViewModel with validation attributes
