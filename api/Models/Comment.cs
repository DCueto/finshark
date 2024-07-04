using System.ComponentModel.DataAnnotations;

namespace api.Models;

public class Comment
{
    public int Id { get; set; }

    [StringLength(280)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(280)]
    public string Content { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; } = DateTime.Now;
    
    public int? StockId { get; set; }
    public Stock? Stock { get; set; }
}