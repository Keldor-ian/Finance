using FinShark.Database;
using FinShark.DTOs.Comment;
using FinShark.Interfaces;
using FinShark.Models;
using Microsoft.EntityFrameworkCore;

namespace FinShark.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Comment?> CreateCommentAsync(Comment commentModel)
        {
            var addComment = await _context.Comments.AddAsync(commentModel);

            if (addComment == null) return null;
            
            await _context.SaveChangesAsync();

            return commentModel;
        }

        public async Task<Comment?> GetCommentByIdAsync(int commentId)
        {
            var getComment = await _context.Comments.FirstOrDefaultAsync(c => c.CommentId == commentId);

            if (getComment == null) return null;

            return getComment;
        }
    }
}
