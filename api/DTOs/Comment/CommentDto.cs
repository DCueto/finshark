using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Comment;

public class CommentDto
{
    public int Id { get; set; }

    [Required]
    [MinLength(5, ErrorMessage = "Title must be 5 characters")]
    [MaxLength(280, ErrorMessage = "Title can't be over 280 characters")]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [MinLength(5, ErrorMessage = "Content must be 5 characters")]
    [MaxLength(280, ErrorMessage = "Content can't be over 280 characters")]
    public string Content { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; } = DateTime.Now;

    public string CreatedBy { get; set; } = string.Empty;
    
    public int? StockId { get; set; }
}