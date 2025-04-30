using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using LabProject.Data;     
using LabProject.Models;

public class IndexModel : PageModel
{
    private readonly SchoolDbContext _context;

    public IndexModel(SchoolDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public ClassInformationModel NewClass { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public int EditId { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? FilterClassName { get; set; }

    [BindProperty(SupportsGet = true)]
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
    public int TotalPages { get; set; }

    public List<ClassInformationTable> FilteredClasses { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        if (!IsUserAuthenticated())
        {
            return RedirectToPage("/Login");
        }

        var query = _context.Classes
        .Where(c => c.IsActive == true) 
        .AsQueryable();

        if (!string.IsNullOrEmpty(FilterClassName))
        {
            query = query.Where(c => c.Name.Contains(FilterClassName));
        }

        TotalPages = (int)Math.Ceiling(await query.CountAsync() / (double)PageSize);

        var pagedData = await query
            .Skip((PageNumber - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        FilteredClasses = pagedData.Select(c => new ClassInformationTable
        {
            Id = c.Id,
            ClassName = c.Name,
            StudentCount = c.PersonCount,
            Description = c.Description
        }).ToList();

        if (EditId > 0)
        {
            var existingClass = await _context.Classes.FindAsync(EditId);
            if (existingClass != null)
            {
                NewClass = new ClassInformationModel
                {
                    Id = existingClass.Id,
                    ClassName = existingClass.Name,
                    StudentCount = existingClass.PersonCount,
                    Description = existingClass.Description
                };
            }
        }

        return Page();
    }

    private bool IsUserAuthenticated()
    {
        var sessionToken = HttpContext.Session.GetString("token");
        var cookieToken = Request.Cookies["AuthToken"];
        return sessionToken == cookieToken && !string.IsNullOrEmpty(sessionToken);
    }

    public async Task<IActionResult> OnPostAddAsync()
    {
        if (!ModelState.IsValid) return Page();

        var newClass = new Class
        {
            Name = NewClass.ClassName,
            PersonCount = NewClass.StudentCount,
            Description = NewClass.Description
        };

        _context.Classes.Add(newClass);
        await _context.SaveChangesAsync();

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEditAsync(int id)
    {
        var existingClass = await _context.Classes.FindAsync(id);
        if (existingClass != null && ModelState.IsValid)
        {
            existingClass.Name = NewClass.ClassName;
            existingClass.PersonCount = NewClass.StudentCount;
            existingClass.Description = NewClass.Description;

            await _context.SaveChangesAsync();
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var classToDelete = await _context.Classes.FindAsync(id);
        if (classToDelete != null)
        {
            classToDelete.IsActive = false;
            //_context.Classes.Remove(classToDelete);
            await _context.SaveChangesAsync();
        }
        return RedirectToPage();
    }

//utils.cs ile dışa aktarmayı yap
//export butonu çalışmıyor. json boş. [] bu şekilde geliyor.
    public async Task<IActionResult> OnPostExportAsync(bool filtered = false, string? selectedColumns = null, string? filterClassName = null)
    {
        var query = _context.Classes.AsQueryable();

        if (filtered && !string.IsNullOrEmpty(filterClassName))
        {
            query = query.Where(c => c.Name.Contains(filterClassName));
        }

        query = query.Skip((PageNumber - 1) * PageSize).Take(PageSize);

        var data = await query.ToListAsync();

        var selected = selectedColumns?.Split(',')
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Trim())
            .ToList();

        var exportData = data.Select(item =>
        {
            var dict = new Dictionary<string, object>();
            if (selected?.Contains("ClassName") == true) dict["ClassName"] = item.Name;
            if (selected?.Contains("StudentCount") == true) dict["StudentCount"] = item.PersonCount;
            if (selected?.Contains("Description") == true) dict["Description"] = item.Description;
            return dict;
        }).Where(d => d.Count > 0).ToList();

        /*var exportData = data.Select(item =>
        {
            var dict = new Dictionary<string, object>();
            if (selected?.Contains("ClassName") == true) dict["ClassName"] = item.ClassName;
            if (selected?.Contains("StudentCount") == true) dict["StudentCount"] = item.StudentCount;
            if (selected?.Contains("Description") == true) dict["Description"] = item.Description;
            return dict;
        }).Where(d => d.Count > 0).ToList();   bu şekilde oluşturulann JSON formatında veriyi nasıl export ederim
        */

        var json = Utils.Instance.ExportToJson(exportData);
        return File(Encoding.UTF8.GetBytes(json), "application/json", "export.json");
    }
}
