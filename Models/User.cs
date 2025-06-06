using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace practice1.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [StringLength(200)]
    public string Fullname { get; set; } = null!;

    public string? Description { get; set; }

    // Password is required for new users but can be empty during edits
    public string? Password { get; set; }

    [Required]
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [StringLength(20)]
    public string? Phone { get; set; }

    public string? Address { get; set; }

    public int Status { get; set; } = 1;

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public string? UserCode { get; set; }

    public bool IsLocked { get; set; } = false;

    public bool IsDeleted { get; set; } = false;

    public bool IsActive { get; set; } = true;

    public string? ActiveCode { get; set; }

    public string? Avatar { get; set; }


    // Navigation property
    public virtual ICollection<Product>? Products { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
    public User()
    {
        UserRoles = new List<UserRole>();
    }
}
