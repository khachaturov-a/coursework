using System.ComponentModel.DataAnnotations;

namespace Practos3.Models;

public class CartItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(64)]
    public string SessionId { get; set; } = string.Empty;

    [Required]
    public int ChetkasId { get; set; }

    public Chetkas? Chetkas { get; set; }

    [Required]
    [Range(1, 100)]
    public int Quantity { get; set; } = 1;

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}
