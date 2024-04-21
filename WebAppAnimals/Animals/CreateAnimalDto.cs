using System.ComponentModel.DataAnnotations;

namespace WebAppAnimals.Animals;

public class CreateAnimalDto
{
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    [Required]
    public string Category { get; set; }
    [Required]
    public string Area { get; set; }
}