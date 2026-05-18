using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Practos3.Models;

public class OrderItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int OrderId { get; set; }

    public Order? Order { get; set; }

    [Required]
    public int ChetkasId { get; set; }

    public Chetkas? Chetkas { get; set; }

    [Required]
    [Range(1, 1000)]
    public int Quantity { get; set; }

    [Required]
    [Precision(10, 2)]
    public decimal UnitPrice { get; set; }
}
