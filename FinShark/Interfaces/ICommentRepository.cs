using FinShark.DTOs.Comment;
using FinShark.Models;
using FinShark.Queries.Comments;

namespace FinShark.Interfaces
{
    public interface ICommentRepository
    {
        public Task<Comment?> GetCommentByIdAsync(int commentId);
        public Task<List<Comment>> GetAllCommentsAsync(CommentQueryObject commentQuery);
        public Task<Comment?> CreateCommentAsync(Comment commentModel);
    }
}
