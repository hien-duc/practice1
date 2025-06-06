using System.ComponentModel.DataAnnotations;

namespace practice1.Models;

public class Role
{
    public Role(string name)
    {
        Name = name;
    }

    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "The Name field is required.")]
    public string Name { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}