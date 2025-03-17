using FinShark.DTOs.Comment;
using FinShark.Models;

namespace FinShark.Interfaces
{
    public interface ICommentRepository
    {
        public Task<Comment?> GetCommentByIdAsync(int commentId);
        public Task<Comment?> CreateCommentAsync(Comment commentModel);
    }
}
