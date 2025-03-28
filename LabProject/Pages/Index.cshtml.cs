using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class IndexModel : PageModel
{
    [BindProperty]
    public ClassInformationModel NewClass { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public int EditId { get; set; }

    public void OnGet()
    {
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
        if (!ModelState.IsValid)
        {
            return Page();
        }

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