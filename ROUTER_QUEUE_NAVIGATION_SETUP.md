# Router Queue Management - Navigation Setup

## Adding Menu Item to Administrator Sidebar

To add the Router Queue Management to your Administrator sidebar menu, add the following menu item to your navigation configuration (typically in a layout or navigation partial view):

### Example Menu HTML (Bootstrap 5)
```html
<a href="@Url.Action("Index", "RouterQueue", new { area = "Administrator" })" 
   class="nav-link @(ViewContext.RouteData.Values["Controller"]?.ToString() == "RouterQueue" ? "active" : "")">
    <i class="ti tabler-network icon-sm"></i>
    <span class="ms-2">Router Queue</span>
</a>
```

### Menu Properties
- **Controller:** RouterQueue
- **Action:** Index
- **Area:** Administrator
- **Icon Suggestion:** `ti tabler-network` (or `ti tabler-router`, `ti tabler-git-branch`)
- **Label:** Router Queue Management

### URL
Direct access URL: `https://yourdomain/Administrator/RouterQueue/Index`

### Required Permissions
- Role: **Administrator**
- Authorization: All actions require `[Authorize(Roles = "Administrator")]`

### Related Menu Items (Suggested Organization)
```
Management
├── Settings
│   ├── NAS Configuration (SettingsNas)
│   ├── Router Queue (RouterQueue) ← NEW
│   ├── Users (SettingsUser)
│   └── Other Settings...
├── Network
│   └── Router Queue (RouterQueue) ← ALTERNATIVE LOCATION
└── Administration
    └── ...
```

## Accessing the Page

### From Menu
Click "Router Queue" in the Administrator sidebar to navigate to the list view.

### Direct URL
- **List Page:** `/Administrator/RouterQueue/Index`
- **Add Queue Modal:** Opens via "Add New Queue" button on the list page
- **Edit Queue Modal:** Opens via "Edit" button on DataTable row

## Features Available on This Page

### View Queue List
- DataTable with sortable columns
- Pagination with customizable page size (7, 10, 25, 50, 100 entries)
- Search/Filter functionality
- Status badges (Active/Disabled)
- Bandwidth display formatting

### Create New Queue
- Click "Add New Queue" button
- Fill in required fields:
  - NAS (dropdown)
  - Queue Name
  - Target Address (IP/Subnet)
  - Priority (0-16)
- Optional fields:
  - Parent Queue
  - Max/Burst limits
  - Packet Mark
  - Comment
- Submit and DataTable automatically reloads

### Edit Queue
- Click "Edit" button on any row
- Modify queue settings
- Changes reflected immediately in DataTable

### Delete Queue
- Click "Delete" button on any row
- Confirm deletion in popup
- Deleted record removed from DataTable

## Troubleshooting

### Queue List Not Loading
**Issue:** DataTable shows no data
**Solutions:**
1. Verify Administrator user has proper roles
2. Check that MikrotikSimpleQueues data exists in database
3. Verify Nas records exist (queues require a NAS association)
4. Check browser console for AJAX errors

### Modal Not Opening
**Issue:** "Add New Queue" or "Edit" buttons don't open modals
**Solutions:**
1. Verify Bootstrap Offcanvas is loaded (bootstrap.bundle.min.js)
2. Check browser console for JavaScript errors
3. Verify modal HTML is being injected (check #createModalContainer and #editModalContainer)

### Validation Errors Not Showing
**Issue:** Form submission fails but no error message shown
**Solutions:**
1. Verify ToastHelper is loaded globally
2. Check that MikrotikSimpleQueueViewModel validation attributes are present
3. Check browser console Network tab for API response

### Dropdown Shows No NAS Options
**Issue:** NAS dropdown is empty in create/edit modal
**Solutions:**
1. Verify Nas records exist in database
2. Check that NAS controller GetJsonResult returns data
3. Check ViewBag.NasOptions is being populated correctly

## Performance Considerations

- DataTable pagination limits initial load to configurable rows per page
- AJAX modal loading keeps main page responsive
- Join query in GetJsonResult includes NAS Name for display
- Consider adding indexes on frequently searched columns

## Browser Compatibility

- Modern browsers with ES6 support required
- Bootstrap 5 compatible
- jQuery 3.x required for AJAX handlers
- DataTable latest version compatible

## Security Notes

- All endpoints require Administrator role
- Anti-forgery tokens validated on POST operations
- User identity logged for audit trail
- Input validation on all form fields
