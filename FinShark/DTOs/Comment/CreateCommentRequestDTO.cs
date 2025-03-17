using System.ComponentModel.DataAnnotations;

namespace FinShark.DTOs.Comment
{
    public class CreateCommentRequestDTO
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must be (at minimum) 5 characters!")]
        [MaxLength(30, ErrorMessage = "Title cannot exceed 30 characters!")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(5, ErrorMessage = "Content must be (at minimum) 5 characters!")]
        [MaxLength(280, ErrorMessage = "Content cannot exceed 280 characters!")]
        public string Content { get; set; } = string.Empty;
    }
}
