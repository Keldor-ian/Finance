namespace FinShark.Models
{
    public class Comment
    {
        public  int CommentId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime TimePosted { get; set; } = DateTime.Now;

        // Navigation
        public Stock Stock { get; set; } = null!;
        public int StockId { get; set; }
    }
}
