# Dokumentasi: Edit & Delete NAS dengan Modal Window

## Overview
Implementasi lengkap untuk Edit dan Delete NAS menggunakan modal window (offcanvas) dengan AJAX pada ASP.NET Core 10 Razor Pages.

## File yang Dimodifikasi/Dibuat

### 1. SettingsNasController.cs - Controller Actions

#### EditModal (GET)
- **Endpoint**: `/Administrator/SettingsNas/EditModal?id={id}`
- **Tujuan**: Mengembalikan partial view berisi form modal untuk edit
- **Return**: `PartialView("../Shared/_Modals/_ModalEditNas", viewModel)`

#### UpdateNas (POST)
- **Endpoint**: `/Administrator/SettingsNas/UpdateNas/{id}`
- **Tujuan**: Memproses update data NAS via AJAX
- **Validasi**:
  - Model validation
  - Check NAS Name uniqueness (exclude current record)
- **Response**: JSON dengan `{ success: bool, message: string }`
- **Return pada sukses**: `Json(new { success = true, message = "NAS berhasil diperbarui." })`

#### DeleteNas (POST)
- **Endpoint**: `/Administrator/SettingsNas/DeleteNas/{id}`
- **Tujuan**: Menghapus NAS via AJAX
- **Confirmation**: Client-side confirmation dialog (JavaScript)
- **Response**: JSON dengan `{ success: bool, message: string }`

### 2. _ModalEditNas.cshtml - Modal Partial View
- **Lokasi**: `Areas/Administrator/Views/Shared/_Modals/_ModalEditNas.cshtml`
- **Tipe**: Offcanvas (side panel)
- **ID Modal**: `offcanvasEditNas`
- **Form ID**: `form-edit-nas-modal`
- **Fitur**:
  - Form fields untuk semua property NAS
  - CSRF token verification
  - Error message container
  - AJAX form submission dengan jQuery
  - Auto-reload DataTable setelah sukses update

### 3. SettingsNasList.cshtml - List View
- **Perubahan pada DataTable columns**:
  - Render tombol Edit dan Delete sebagai `<button>` bukan link
  - Edit button: trigger AJAX load modal + offcanvas show
  - Delete button: trigger confirmation dialog + AJAX delete

- **JavaScript Handlers**:
  - `.btn-edit-nas` click: Load modal via `EditModal` action
  - `.btn-delete-nas` click: Confirm + AJAX call ke `DeleteNas` action
  - Auto-reload DataTable setelah operasi sukses

- **Modal Container**: Div `#editModalContainer` untuk menampung modal yang di-load via AJAX

## Alur Kerja

### Edit Flow
1. User click tombol "Edit" di DataTable
2. JavaScript trigger AJAX GET ke `EditModal` action
3. Controller return partial view (modal form)
4. Modal di-inject ke `#editModalContainer` dan ditampilkan
5. User fill form dan click "Update"
6. Form submit via AJAX ke `UpdateNas` action
7. Controller validate & update database
8. Return JSON success
9. JavaScript close modal + reload DataTable

### Delete Flow
1. User click tombol "Delete" di DataTable
2. Browser show confirmation dialog
3. If confirmed, trigger AJAX POST ke `DeleteNas` action
4. Controller delete record dan return JSON success
5. JavaScript reload DataTable

## Keamanan

### CSRF Protection
- Menggunakan `@Html.AntiForgeryToken()` di form
- Token di-submit via form data
- Controller menggunakan `[ValidateAntiForgeryToken]` attribute

### Authorization
- Controller menggunakan `[Authorize(Roles = "Administrator")]`
- Hanya user dengan role "Administrator" yang dapat akses

### Input Validation
- Model validation via `ModelState.IsValid`
- Server-side check untuk NAS Name uniqueness
- Validation errors di-return sebagai JSON

## Dependencies
- jQuery (untuk AJAX & event handling)
- Bootstrap 5 (untuk Offcanvas modal)
- DataTables (untuk tabel list)

## Notes
- Modal menggunakan Bootstrap Offcanvas (side panel) bukan Modal tradisional
- All AJAX calls include CSRF token
- DataTable reference disimpan globally untuk reload dari modal
- Error handling dengan try-catch dan JSON error response
