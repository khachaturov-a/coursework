using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Models;

public class Chetkas
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Precision(10, 2)]
    public decimal Price { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    [Required]
    [StringLength(50)]
    public string Material { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Required]
    public int CategoryId { get; set; }

    public Category? Category { get; set; }

    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public ICollection<FavoriteItem> FavoriteItems { get; set; } = new List<FavoriteItem>();
    public ICollection<ProductView> ProductViews { get; set; } = new List<ProductView>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

