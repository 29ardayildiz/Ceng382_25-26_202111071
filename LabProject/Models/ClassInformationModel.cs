using System.ComponentModel.DataAnnotations;

public class ClassInformationModel
{
    public int Id { get; set; }
//requierd olan yerlede kullanıcıya error mesdajı nasıl yazdırılır
    [Required(ErrorMessage = "Class name is required.")]
    public string ClassName { get; set; }

    [Required(ErrorMessage = "Student count is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Student count must be greater than 0.")]
    public int StudentCount { get; set; }

    public string Description { get; set; }


    public static List<ClassInformationModel> Classes { get; } = new();
    private static int _currentId = 0;

    public static int GetNextId() => ++_currentId;
}