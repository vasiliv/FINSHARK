using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "Title must be minimum 2 characters")]
        [MaxLength(20, ErrorMessage = "Title must be max 20  characters")]
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int? StockId { get; set; }
    }
}
