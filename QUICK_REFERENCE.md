# ⚡ Quick Reference: NAS AJAX Operations

## 🎯 Main Endpoints

### Create NAS
```
POST /Administrator/SettingsNas/CreateNas
Input: NAS model fields
Output: { success: true/false, message: string }
```

### Edit NAS (Load Modal)
```
GET /Administrator/SettingsNas/EditModal?id=1
Output: HTML (Offcanvas Modal Partial View)
```

### Update NAS
```
POST /Administrator/SettingsNas/UpdateNas/1
Input: NAS model fields
Output: { success: true/false, message: string }
```

### Delete NAS
```
POST /Administrator/SettingsNas/DeleteNas/1
Output: { success: true/false, message: string }
```

---

## 🔌 JavaScript Integration

### Global DataTable Reference
```javascript
var dataTable;  // Accessible from modal scripts
```

### Reload DataTable from Modal
```javascript
if (window.dataTable && window.dataTable.ajax) {
    window.dataTable.ajax.reload();
}
```

### Close Offcanvas from Modal
```javascript
var offcanvas = bootstrap.Offcanvas.getInstance(document.getElementById('offcanvasEditNas'));
if (offcanvas) offcanvas.hide();
```

### AJAX Form Submit
```javascript
$.ajax({
    url: actionUrl,
    type: 'POST',
    data: form.serialize(),  // Include CSRF token
    success: function(response) {
        if (response.success) {
            // Handle success
            window.dataTable.ajax.reload();
            offcanvas.hide();
        } else {
            // Show error: response.message
        }
    }
});
```

---

## 🎨 Modal IDs & Form IDs

### Create NAS
- Modal ID: `#offcanvasCreateNas`
- Form ID: `#form-create-nas`
- Error Summary: `#form-create-error-summary`

### Edit NAS
- Modal ID: `#offcanvasEditNas`
- Form ID: `#form-edit-nas-modal`
- Error Summary: `#form-error-summary`
- Modal Container: `#editModalContainer`

---

## ✅ Validation

### Server-Side (Controller)
- `ModelState.IsValid` - Model validation
- `applicationDbContext.Nas.Any(n => n.NasName == model.NasName)` - Uniqueness check
- `n.Id != id` - Exclude current record on update

### Client-Side (Optional Future Enhancement)
- Could add jQuery validation plugin
- Currently rely on browser HTML5 validation + server validation

---

## 🔐 CSRF Token

### Include in Form
```html
@Html.AntiForgeryToken()
```

### Serialize with Form Data
```javascript
var formData = form.serialize();  // Automatically includes token
```

### Controller Validation
```csharp
[ValidateAntiForgeryToken]
public IActionResult UpdateNas(...)
```

---

## 📊 Response Format

### Success Response
```json
{
    "success": true,
    "message": "NAS berhasil ditambahkan."
}
```

### Error Response
```json
{
    "success": false,
    "message": "NAS Name sudah ada.",
    "errors": ["error1", "error2"]  // Optional
}
```

---

## 🧩 DOM Elements

### Create Modal Elements
```html
<input id="createNasName" name="NasName" />
<input id="createShortName" name="ShortName" />
<input id="createType" name="Type" />
<input id="createPorts" name="Ports" />
<input id="createSecret" name="Secret" />
<input id="createServer" name="Server" />
<input id="createCommunity" name="Community" />
<textarea id="createDescription" name="Description"></textarea>
<select id="createRouterType" name="RouterType">...</select>
<input id="createUsername" name="Username" />
<input id="createPassword" name="Password" />
```

### Edit Modal Elements
```html
<input id="nasName" name="NasName" />
<input id="shortName" name="ShortName" />
<input id="type" name="Type" />
<input id="ports" name="Ports" />
<input id="secret" name="Secret" />
<input id="server" name="Server" />
<input id="community" name="Community" />
<textarea id="description" name="Description"></textarea>
<select id="routerType" name="RouterType">...</select>
<input id="username" name="Username" />
<input id="password" name="Password" />
<input id="modalNasId" name="Id" />
```

---

## 🔄 DataTable Column Render

```javascript
{
    data: 'id',
    title: 'Actions',
    orderable: false,
    render: function(data, type, row) {
        return `<button class="btn btn-sm btn-primary btn-edit-nas" 
                        data-id="${data}" 
                        data-bs-toggle="offcanvas" 
                        data-bs-target="#offcanvasEditNas">
                    Edit
                </button>
                <button class="btn btn-sm btn-danger btn-delete-nas" 
                        data-id="${data}">
                    Delete
                </button>`;
    }
}
```

---

## 🎯 Event Flow

### Create NAS
```
[Add New Button Click]
    ↓
[Offcanvas Modal Show]
    ↓
[User Fill Form]
    ↓
[Click Create Button]
    ↓
[Form Submit AJAX]
    ↓
[CreateNas Action Validate]
    ↓
[Save to Database]
    ↓
[Return JSON Success]
    ↓
[Close Modal + Reload Table + Reset Form]
```

### Edit NAS
```
[Edit Button Click]
    ↓
[AJAX Load Modal]
    ↓
[Offcanvas Modal Show]
    ↓
[Form Pre-filled]
    ↓
[User Modify Fields]
    ↓
[Click Update Button]
    ↓
[Form Submit AJAX]
    ↓
[UpdateNas Action Validate]
    ↓
[Update Database]
    ↓
[Return JSON Success]
    ↓
[Close Modal + Reload Table]
```

---

## 🐛 Debugging Tips

### Check CSRF Token
```javascript
var token = $('[name="__RequestVerificationToken"]').val();
console.log('CSRF Token:', token);
```

### Check DataTable Instance
```javascript
console.log('DataTable:', window.dataTable);
console.log('DataTable AJAX:', window.dataTable.ajax);
```

### Check Modal Instance
```javascript
var modal = bootstrap.Offcanvas.getInstance(document.getElementById('offcanvasEditNas'));
console.log('Modal:', modal);
```

### Check AJAX Response
```javascript
success: function(response) {
    console.log('Response:', response);
    console.log('Success:', response.success);
    console.log('Message:', response.message);
}
```

---

## 📚 Related Files

- Controller: `NRAdmanWebApplicationNet10/Areas/Administrator/Controllers/SettingsNasController.cs`
- Create Modal: `NRAdmanWebApplicationNet10/Areas/Administrator/Views/Shared/_Modals/_ModalCreateNas.cshtml`
- Edit Modal: `NRAdmanWebApplicationNet10/Areas/Administrator/Views/Shared/_Modals/_ModalEditNas.cshtml`
- List View: `NRAdmanWebApplicationNet10/Areas/Administrator/Views/SettingsNas/SettingsNasList.cshtml`

---

## 🚀 Performance Notes

- ✅ No full page reloads (AJAX)
- ✅ DataTable only reloads visible data
- ✅ Modal reuses same instances
- ✅ CSRF token cached in DOM

---

**Version: 1.0**
**Last Updated: 2025**
**Status: Production Ready ✅**
