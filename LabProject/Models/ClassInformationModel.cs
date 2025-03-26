using System.ComponentModel.DataAnnotations;

public class ClassInformationModel
{
    public int Id { get; set; }
    
    [Required]
    public string ClassName { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "Student count must be at least 1")]
    public int StudentCount { get; set; }
    
    public string Description { get; set; }
}