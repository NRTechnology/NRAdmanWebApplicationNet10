# Dokumentasi: Create, Edit & Delete NAS dengan AJAX Modal

## Overview
Implementasi lengkap untuk Create, Edit, dan Delete NAS menggunakan modal window (offcanvas) dengan AJAX pada ASP.NET Core 10.

---

## 📋 File yang Dimodifikasi

### 1. SettingsNasController.cs - Controller Actions

#### **CreateNas** (POST)
```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult CreateNas(NRAdmanWebApplicationNet10.ViewModels.Nas model)
```
- **Endpoint**: `POST /Administrator/SettingsNas/CreateNas`
- **Tujuan**: Membuat NAS baru via AJAX
- **Validasi**:
  - Model validation
  - Check NAS Name uniqueness
- **Response**: JSON `{ success: bool, message: string }`
- **Successs**: `Json(new { success = true, message = "NAS berhasil ditambahkan." })`

#### **EditModal** (GET)
```csharp
[HttpGet]
public IActionResult EditModal(int id)
```
- **Endpoint**: `GET /Administrator/SettingsNas/EditModal?id={id}`
- **Tujuan**: Mengembalikan partial view (offcanvas modal) untuk edit form
- **Return**: `PartialView("../Shared/_Modals/_ModalEditNas", viewModel)`

#### **UpdateNas** (POST)
```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult UpdateNas(int id, NRAdmanWebApplicationNet10.ViewModels.Nas model)
```
- **Endpoint**: `POST /Administrator/SettingsNas/UpdateNas/{id}`
- **Tujuan**: Update NAS data via AJAX
- **Validasi**: Model + uniqueness check (exclude current record)
- **Response**: JSON `{ success: bool, message: string }`

#### **DeleteNas** (POST)
```csharp
[HttpPost]
public IActionResult DeleteNas(int id)
```
- **Endpoint**: `POST /Administrator/SettingsNas/DeleteNas/{id}`
- **Tujuan**: Hapus NAS via AJAX
- **Response**: JSON `{ success: bool, message: string }`

---

## 📁 Modal Views

### **_ModalCreateNas.cshtml**
- **Lokasi**: `Areas/Administrator/Views/Shared/_Modals/_ModalCreateNas.cshtml`
- **Tipe**: Offcanvas (side panel)
- **ID Modal**: `offcanvasCreateNas`
- **Form ID**: `form-create-nas`
- **Fitur**:
  - Form fields untuk semua property NAS
  - CSRF token verification
  - Error message container (`#form-create-error-summary`)
  - AJAX form submission dengan jQuery
  - Auto-reload DataTable setelah sukses
  - Auto-reset form setelah sukses

### **_ModalEditNas.cshtml**
- **Lokasi**: `Areas/Administrator/Views/Shared/_Modals/_ModalEditNas.cshtml`
- **Tipe**: Offcanvas (side panel)
- **ID Modal**: `offcanvasEditNas`
- **Form ID**: `form-edit-nas-modal`
- **Fitur**:
  - Pre-filled form dengan data NAS yang akan diedit
  - CSRF token verification
  - Error message container (`#form-error-summary`)
  - AJAX form submission
  - Auto-reload DataTable & close modal setelah sukses

---

## 🎯 Alur Kerja

### **Create NAS Flow**
1. User click tombol "Add New Record" di halaman list
2. Offcanvas modal terbuka
3. User fill form dan click "Create"
4. Form submit via AJAX POST ke `CreateNas` action
5. Controller validate & save ke database
6. Return JSON success
7. JavaScript close modal + reload DataTable + reset form
8. Success notification ditampilkan

### **Edit NAS Flow**
1. User click tombol "Edit" di DataTable
2. JavaScript trigger AJAX GET ke `EditModal` action
3. Controller return partial view (modal form)
4. Modal di-inject ke `#editModalContainer` dan ditampilkan
5. Form sudah pre-filled dengan data NAS
6. User modify fields dan click "Update"
7. Form submit via AJAX POST ke `UpdateNas` action
8. Controller validate & update database
9. Return JSON success
10. JavaScript close modal + reload DataTable
11. Success notification ditampilkan

### **Delete NAS Flow**
1. User click tombol "Delete" di DataTable
2. Browser show confirmation dialog
3. If confirmed, trigger AJAX POST ke `DeleteNas` action
4. Controller delete record dan return JSON success
5. JavaScript reload DataTable + show notification

---

## 🔒 Keamanan

### **CSRF Protection**
```html
@Html.AntiForgeryToken()
```
- Form include CSRF token
- Token di-submit via form data
- Controller validate dengan `[ValidateAntiForgeryToken]`

### **Authorization**
```csharp
[Area("Administrator")]
[Authorize(Roles = "Administrator")]
public class SettingsNasController : Controller
```
- Hanya user dengan role "Administrator" yang dapat akses

### **Input Validation**
- Model validation via `ModelState.IsValid`
- Server-side uniqueness check untuk NAS Name
- Error handling dengan try-catch
- Validation errors di-return sebagai JSON

---

## 🔄 DataTable Integration

### **Global Reference**
```javascript
var dataTable;  // Global variable
// Stored after DataTable initialization
window.dataTable = dataTable;
```

### **Auto-Reload**
```javascript
if (window.dataTable && window.dataTable.ajax) {
    window.dataTable.ajax.reload();
}
```

---

## 📦 Dependencies

- **jQuery** - AJAX & event handling
- **Bootstrap 5** - Offcanvas modal
- **DataTables** - Table with AJAX data source

---

## ✨ Fitur Tambahan

### **Error Handling**
- Model validation errors ditampilkan di modal
- Server errors di-return sebagai JSON message
- Client-side error display di `#form-error-summary`

### **User Experience**
- Offcanvas modal (side panel) yang smooth
- Auto-close setelah operasi sukses
- Form auto-reset setelah create
- Confirmation dialog untuk delete
- DataTable auto-refresh tanpa page reload
- Success notification dengan alert

### **Consistency**
- Semua form menggunakan AJAX (Create, Edit)
- Konsisten naming convention untuk element IDs
- Konsisten error handling & response format
- Konsisten validation logic (model + server-side)

---

## 🧪 Testing Checklist

- [ ] Create NAS via AJAX form
- [ ] Verify NAS Name uniqueness check on create
- [ ] Edit NAS via modal
- [ ] Verify NAS Name uniqueness check on edit (exclude current record)
- [ ] Delete NAS with confirmation
- [ ] DataTable auto-refresh after create/edit/delete
- [ ] Modal close after success
- [ ] Form reset after create
- [ ] Error messages display correctly
- [ ] CSRF token validation works

---

## 📝 Notes

- Semua form menggunakan `form.serialize()` untuk CSRF token handling
- Modal menggunakan Bootstrap Offcanvas bukan Modal tradisional
- DataTable reference disimpan globally untuk access dari modal
- Response format selalu JSON untuk AJAX requests
- Error handling lengkap dengan logging via `logger.LogError()`
