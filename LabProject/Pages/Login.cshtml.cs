//Bana users.json dosyasında bulunan [{"Username": "admin","Password": "1234","Role": "Admin","IsActive": true,"CreatedAt": "2023-10-01T00:00:00"}] şekilindeki json dosyasını kullanarak login işlemi yapacağım .net de çalışacak ve ekte gönderdiğim html kodu ile uyumlu çalışacak c# kodunu yaz
//Session ve cookie işlemlerini yapacak şekilde düzenle

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

public class LoginModel : PageModel
{
    private const string UsersFilePath = "wwwroot/data/users.json";
    private const string SessionTokenKey = "AuthToken";
    private const int SessionTimeoutMinutes = 30;

    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = AuthenticateUser(Username, Password);

        if (user == null)
        {
            ModelState.AddModelError("", "Invalid username or password");
            return Page();
        }

        CreateUserSession(user);
        SetAuthCookies(user);

        return RedirectToPage("/Index");
    }

    private User AuthenticateUser(string username, string password)
    {
        try
        {
            var jsonData = System.IO.File.ReadAllText(UsersFilePath);
            var users = JsonSerializer.Deserialize<List<User>>(jsonData);
            return users?.FirstOrDefault(u => 
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && 
                u.Password == password && 
                u.IsActive);
        }
        catch (Exception ex)
        {
            
            return null;
        }
    }

    private void CreateUserSession(User user)
    {
        var token = Guid.NewGuid().ToString();
        
        HttpContext.Session.SetString("username", user.Username);
        HttpContext.Session.SetString("role", user.Role);
        HttpContext.Session.SetString(SessionTokenKey, token);
        HttpContext.Session.SetString("session_id", HttpContext.Session.Id);
        
 
        HttpContext.Session.SetInt32("SessionExpire", SessionTimeoutMinutes);
    }

    private void SetAuthCookies(User user)
    {
        var token = HttpContext.Session.GetString(SessionTokenKey);
        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.UtcNow.AddMinutes(SessionTimeoutMinutes),
            HttpOnly = true,
            Secure = Request.IsHttps,
            SameSite = SameSiteMode.Strict
        };

        Response.Cookies.Append(SessionTokenKey, token, cookieOptions);
        Response.Cookies.Append("Username", user.Username, cookieOptions);
        Response.Cookies.Append("UserRole", user.Role, cookieOptions);
    }
}