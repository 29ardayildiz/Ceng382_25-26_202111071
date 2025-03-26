using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class IndexModel : PageModel
{
    private static List<ClassInformationModel> classList = new();
    public List<ClassInformationModel> ClassList => classList;
    
    [BindProperty]
    public ClassInformationModel NewClass { get; set; } = new();
    
    public void OnGet() {}
    
    public IActionResult OnPostAdd()
    {
        if (!ModelState.IsValid)
            return Page();
        
        NewClass.Id = classList.Count + 1;
        classList.Add(NewClass);
        return RedirectToPage();
    }
    
    public IActionResult OnPostDelete(int id)
    {
        var item = classList.FirstOrDefault(c => c.Id == id);
        if (item != null)
            classList.Remove(item);
        return RedirectToPage();
    }
}