//.net de sessin bilgisini silen c# komutunu yaz

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class LogoutModel : PageModel
{
    public IActionResult OnPost()
    {
        HttpContext.Session.Clear();
        
        return RedirectToPage("/Login");
    }
}