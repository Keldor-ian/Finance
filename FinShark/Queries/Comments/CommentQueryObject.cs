using FinShark.Models;
namespace FinShark.Queries.Comments
{
    public class CommentQueryObject
    {
        public string? Symbol { get; set; }
        public bool IsDescending { get; set; } = true;

    }
}
