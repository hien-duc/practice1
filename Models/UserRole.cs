using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace practice1.Models;

public class UserRole
{
    [Key]
    [Column(Order = 1)]
    public int UserId { get; set; }
    public User User { get; set; }

    [Key]
    [Column(Order = 2)]
    public int RoleId { get; set; }
    public Role Role { get; set; }
}