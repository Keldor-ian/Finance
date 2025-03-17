namespace FinShark.DTOs.Comment
{
    public class CommentDTO
    {
        public int CommentId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime TimePosted { get; set; } = DateTime.Now;

        // DTOs do not need a navigation!
        public int StockId { get; set; }
    }
}
