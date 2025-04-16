//Bana users.json dosyasında bulunan [{"Username": "admin","Password": "1234","Role": "Admin","IsActive": true,"CreatedAt": "2023-10-01T00:00:00"}] şekilindeki json dosyasını kullanarak login işlemi yapacağım .net de çalışacak ve ekte gönderdiğim html kodu ile uyumlu çalışacak c# kodunu yaz
//Session ve cookie işlemlerini yapacak şekilde düzenle

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

public class LoginModel : PageModel
{
    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public IActionResult OnPost()
    {
        var users = JsonSerializer.Deserialize<List<User>>(
            System.IO.File.ReadAllText("wwwroot/data/users.json")
        );

        var user = users?.FirstOrDefault(u => 
            u.Username == Username && 
            u.Password == Password && 
            u.IsActive
        );

        if (user == null)
        {
            ModelState.AddModelError("", "Invalid username or password");
            return Page();
        }

        var token = Guid.NewGuid().ToString();
        HttpContext.Session.SetString("username", user.Username);
        HttpContext.Session.SetString("token", token);
        HttpContext.Session.SetString("session_id", HttpContext.Session.Id);

        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.UtcNow.AddMinutes(30),
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        };

        Response.Cookies.Append("AuthToken", token, cookieOptions);
        Response.Cookies.Append("Username", user.Username, cookieOptions);
        Response.Cookies.Append("SessionId", HttpContext.Session.Id, cookieOptions);

        return RedirectToPage("/Index");
    }
}