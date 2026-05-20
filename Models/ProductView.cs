using System.ComponentModel.DataAnnotations;

namespace Coursework.Models;

/// <summary>Запись о просмотре товара, используемая системой рекомендаций.</summary>
public class ProductView
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(64)]
    public string SessionId { get; set; } = string.Empty;

    [Required]
    public int ChetkasId { get; set; }

    public Chetkas? Chetkas { get; set; }

    public DateTime ViewedAt { get; set; } = DateTime.UtcNow;
}
