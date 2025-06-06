using System.ComponentModel.DataAnnotations;

namespace practice1.Models;

public class Carousel
{
    [Key]
    public int CarouselId { get; set; }

    [Required]
    [StringLength(500)]
    public string ImageUrl { get; set; } = "/images/carousel1.png";

    [StringLength(200)]
    public string? Title { get; set; } = "Welcome to Our Store";

    [StringLength(500)]
    public string? Description { get; set; } = "Discover our amazing products.";

    public int Order { get; set; } = 1;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.Now;
}
