using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class IndexModel : PageModel
{
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

    public void OnGet()
    {
        if (!ClassInformationModel.Classes.Any())
        {
            for (int i = 1; i <= 100; i++)
            {
                ClassInformationModel.Classes.Add(new ClassInformationModel
                {
                    Id = ClassInformationModel.GetNextId(),
                    ClassName = $"Class {i:000}",
                    StudentCount = i + 10,
                    Description = $"This is description for Class {i:000}"
                });
            }
        }

        var query = ClassInformationModel.Classes.AsQueryable();

        if (!string.IsNullOrEmpty(FilterClassName))
        {
            query = query.Where(c => c.ClassName.Contains(FilterClassName, StringComparison.OrdinalIgnoreCase));
        }

        TotalPages = (int)Math.Ceiling(query.Count() / (double)PageSize);
        query = query.Skip((PageNumber - 1) * PageSize).Take(PageSize);

        FilteredClasses = query.Select(c => new ClassInformationTable
        {
            Id = c.Id,
            ClassName = c.ClassName,
            StudentCount = c.StudentCount,
            Description = c.Description
        }).ToList();

        if (EditId > 0)
        {
            var existingClass = ClassInformationModel.Classes.FirstOrDefault(c => c.Id == EditId);
            if (existingClass != null)
            {
                NewClass = existingClass;
            }
        }
    }

    public IActionResult OnPostAdd()
    {
        if (!ModelState.IsValid) return Page();

        NewClass.Id = ClassInformationModel.GetNextId();
        ClassInformationModel.Classes.Add(NewClass);
        return RedirectToPage();
    }

    public IActionResult OnPostEdit(int id)
    {
        var existingClass = ClassInformationModel.Classes.FirstOrDefault(c => c.Id == id);
        if (existingClass != null && ModelState.IsValid)
        {
            existingClass.ClassName = NewClass.ClassName;
            existingClass.StudentCount = NewClass.StudentCount;
            existingClass.Description = NewClass.Description;
        }
        return RedirectToPage();
    }

    public IActionResult OnPostDelete(int id)
    {
        var classToDelete = ClassInformationModel.Classes.FirstOrDefault(c => c.Id == id);
        if (classToDelete != null)
        {
            ClassInformationModel.Classes.Remove(classToDelete);
        }
        return RedirectToPage();
    }
}