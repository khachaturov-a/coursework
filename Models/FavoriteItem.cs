using System.ComponentModel.DataAnnotations;

namespace Practos3.Models;

public class FavoriteItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(64)]
    public string SessionId { get; set; } = string.Empty;

    [Required]
    public int ChetkasId { get; set; }

    public Chetkas? Chetkas { get; set; }

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}
