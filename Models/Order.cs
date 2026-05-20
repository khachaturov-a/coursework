using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Models;

/// <summary>Заказ покупателя, оформленный из корзины.</summary>
public class Order
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(64)]
    public string SessionId { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [Precision(10, 2)]
    public decimal TotalAmount { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = "Обрабатывается";

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
