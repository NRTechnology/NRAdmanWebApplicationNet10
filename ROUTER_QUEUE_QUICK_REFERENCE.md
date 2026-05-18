# Router Queue Management - Quick Reference

## 🚀 Quick Start

### Access the Page
```
URL: /Administrator/RouterQueue/Index
Menu Item: Router Queue (in Administrator sidebar)
```

### Create New Queue
1. Click "Add New Queue" button
2. Select NAS from dropdown
3. Enter Queue Name (3-255 chars)
4. Enter Target Address (e.g., 192.168.1.100)
5. Set Priority (0-16, default: 8)
6. (Optional) Configure bandwidth limits
7. Click "Create"

### Edit Existing Queue
1. Find queue in DataTable
2. Click "Edit" button
3. Modify fields as needed
4. Click "Update"

### Delete Queue
1. Click "Delete" button on queue row
2. Confirm in popup dialog
3. Queue removed from list

---

## 📋 Field Guide

| Field | Type | Required | Notes |
|-------|------|----------|-------|
| NAS | Dropdown | ✅ | Select from configured NAS |
| Queue Name | Text | ✅ | Must be unique per NAS |
| Target Address | Text | ✅ | IP address or subnet (192.168.1.0/24) |
| Parent Queue | Text | ❌ | Leave blank for top-level queue |
| Max Limit | Number | ❌ | In bps (1000000 = 1 Mbps) |
| Burst Limit | Number | ❌ | Peak bandwidth burst |
| Burst Threshold | Number | ❌ | Threshold to start burst |
| Burst Time | Number | ❌ | Duration of burst in seconds |
| Priority | Dropdown | ✅ | 0 (Highest) to 16 (Lowest) |
| Packet Mark | Text | ❌ | For advanced routing |
| Comment | Textarea | ❌ | Internal notes (max 500 chars) |
| Disable Queue | Checkbox | ❌ | Disable without deleting |

---

## 🎛️ Priority Reference

| Value | Label | Use Case |
|-------|-------|----------|
| 0-3 | Highest | Critical traffic (VoIP, Real-time) |
| 4-7 | High | Important traffic (Web, Email) |
| 8 | Default | Normal traffic |
| 9-12 | Low | Background traffic (Downloads) |
| 13-16 | Lowest | Minimal priority (P2P, Backups) |

---

## 💾 Example Queues

### Example 1: VoIP Queue
```
Queue Name: VoIP_Main
Target Address: 192.168.1.100
Priority: 1
Max Limit: 2000000 (2 Mbps)
Comment: VoIP traffic for main office
```

### Example 2: Guest Network
```
Queue Name: Guest_Limited
Target Address: 192.168.2.0/24
Priority: 8
Max Limit: 5000000 (5 Mbps)
Disabled: false
Comment: Guest network bandwidth limit
```

### Example 3: Download Queue
```
Queue Name: Downloads
Target Address: 192.168.1.50
Priority: 14
Max Limit: 10000000 (10 Mbps)
Parent: Internet
Comment: Limit heavy downloads
```

---

## ⚠️ Common Issues

### Queue Not Showing
- Verify NAS is properly configured
- Check if queue is disabled (status badge)
- Refresh page or clear browser cache

### Can't Create - "Queue Name Already Exists"
- Queue name must be unique for each NAS
- Try adding NAS prefix or timestamp

### Target Address Validation Failed
- Use format: 192.168.1.100 (single IP)
- Use format: 192.168.1.0/24 (subnet)
- Don't include /32 for single IPs

### Bandwidth Limits Not Working
- Ensure NAS is properly connected
- Check Mikrotik router configuration
- Verify queue is enabled (not disabled)

---

## 📊 DataTable Tips

### Search/Filter
- Type in search box to filter queues
- Searches Queue Name, NAS Name, Target Address

### Sorting
- Click column header to sort
- Click again to reverse sort order

### Pagination
- Change page size: 7, 10, 25, 50, 100 entries
- Use Previous/Next buttons to navigate

### Export Data (Manual)
1. Configure page size to show all entries
2. Inspect table or use browser dev tools
3. Copy data as needed

---

## 🔐 Permissions

### Required Role
- **Administrator**

### What You Can Do
- ✅ View all queues
- ✅ Create new queues
- ✅ Edit existing queues
- ✅ Delete queues
- ✅ Search and filter

### What You Cannot Do
- ❌ View as non-Administrator
- ❌ Access without authentication
- ❌ Bypass validation checks

---

## 🎨 Bandwidth Conversion Reference

| Input | Display |
|-------|---------|
| 1000 | 1.00 Kbps |
| 10000 | 10.00 Kbps |
| 100000 | 100.00 Kbps |
| 1000000 | 1.00 Mbps |
| 10000000 | 10.00 Mbps |
| 100000000 | 100.00 Mbps |
| 1000000000 | 1.00 Gbps |

---

## 🔔 Toast Messages

### Success Messages
- ✅ "Queue berhasil ditambahkan."
- ✅ "Queue berhasil diperbarui."
- ✅ "Queue berhasil dihapus."

### Error Messages
- ❌ "Validasi gagal." (with detailed errors)
- ❌ "Queue Name sudah ada untuk NAS ini."
- ❌ "Data Queue tidak ditemukan."
- ❌ "Gagal menyimpan data Queue. Silakan coba lagi."

---

## 📱 Mobile Responsiveness

- ✅ Works on tablets and phones
- ✅ Touch-friendly buttons
- ✅ Responsive table layout
- ✅ Mobile-optimized modals

---

## ⌨️ Keyboard Shortcuts

| Key | Action |
|-----|--------|
| Tab | Navigate form fields |
| Enter | Submit form |
| Esc | Close modal |
| Ctrl+A | Select all in table |

---

## 🐛 Troubleshooting Checklist

Before reporting issues, verify:

- [ ] You are logged in as Administrator
- [ ] NAS records exist in database
- [ ] JavaScript console shows no errors
- [ ] Network tab shows successful API calls
- [ ] Page has been refreshed
- [ ] Cookies/cache cleared if needed
- [ ] Using supported browser (Chrome, Firefox, Edge, Safari)

---

## 📞 Getting Help

### Issue Description
Always include:
1. What you were trying to do
2. Error message or unexpected behavior
3. Queue details (name, target address)
4. Browser console errors
5. Network tab errors

### Logs Location
Check application logs:
- Administrator action log
- Error log entries
- Queue creation/update timestamps

---

## 💡 Pro Tips

1. **Naming Convention**
   - Use descriptive names: `Office_VoIP`, `Guest_Limited`, `Backup_Low`
   - Add location prefixes if multi-site: `HQ_VoIP`, `Branch1_Guest`

2. **Organization**
   - Use Parent Queue for hierarchical limits
   - Group related queues with comments

3. **Testing**
   - Create test queue first to verify NAS
   - Use small bandwidth limits for testing
   - Verify on actual Mikrotik device

4. **Documentation**
   - Use Comments field for future reference
   - Document business reasons for each queue
   - Update if purposes change

5. **Monitoring**
   - Check regularly for disabled queues
   - Verify queue limits match requirements
   - Review audit logs periodically

---

## 📅 Audit Trail

View who created/updated each queue:
- **Created By** column (in administrator view)
- **Created At** timestamp (shown in table)
- Edit to see last updater and timestamp

---

## 🔗 Related Pages

- **NAS Settings** - Configure Mikrotik devices
- **User Management** - Manage administrator accounts
- **Activity Logs** - View all system activities

---

## ❓ FAQ

**Q: Can I create queue without NAS?**
A: No, NAS is required. Create NAS first in Settings.

**Q: What if queue name already exists?**
A: Queue names must be unique per NAS. Try different name or NAS.

**Q: Can I disable queue temporarily?**
A: Yes, check "Disable Queue" to disable without deleting.

**Q: What's Priority 8?**
A: Priority 8 is default/normal. Lower = higher priority.

**Q: How to set bandwidth in Mbps?**
A: Multiply by 1,000,000. E.g., 5 Mbps = 5000000 bps

**Q: Can I edit deleted queue?**
A: No, deleted queues are permanently removed. No undo.

**Q: What does Parent Queue do?**
A: Sets this queue as child of another. Useful for hierarchy.

---

## 📝 Notes for Administrators

- Always verify NAS connectivity before creating queues
- Test queue limits before deploying to production
- Document business rules in Comment field
- Regularly audit unused or disabled queues
- Backup database before major changes
- Monitor bandwidth usage to adjust limits
- Check logs for any error patterns

---

**Last Updated:** 2025
**Version:** 1.0
**Status:** Production Ready ✅
