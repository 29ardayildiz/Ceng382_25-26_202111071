//Utils.cs adında yeni bir C# sınıf dosyası oluşturun. İçinde, herhangi bir sınıfı JSON'a aktarabilen genel bir yöntem uygulayın. Yöntem herhangi bir model sınıfıyla çalışmalıdır. Bu sınıf tekil olarak uygulanmalıdır, böylece projedeki herhangi bir yerden erişilebilir.
using System.Text.Json;

public class Utils
{
    private static Utils _instance;
    private static readonly object _lock = new();

    public static Utils Instance
    {
        get
        {
            lock (_lock)
            {
                return _instance ??= new Utils();
            }
        }
    }

    private Utils() { }

    public string ExportToJson<T>(IEnumerable<T> data)
    {
        return JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
    }
}
