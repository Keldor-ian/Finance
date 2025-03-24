using FinShark.Database;
using FinShark.DTOs.Comment;
using FinShark.Interfaces;
using FinShark.Models;
using FinShark.Queries.Comments;
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

        public async Task<List<Comment>> GetAllCommentsAsync(CommentQueryObject commentQuery)
        {
            var comments = _context.Comments.Include(a => a.AppUser).AsQueryable();

            if (!string.IsNullOrWhiteSpace(commentQuery.Symbol))
            {
                comments = comments.Where(s => s.Stock.Symbol == commentQuery.Symbol.Trim());
            }

            if (commentQuery.IsDescending == true)
            {
                comments = comments.OrderByDescending(c => c.TimePosted);
            }

            return await comments.ToListAsync();
        }

        public async Task<Comment?> GetCommentByIdAsync(int commentId)
        {
            var getComment = await _context.Comments.Include(a => a.AppUser).FirstOrDefaultAsync(c => c.CommentId == commentId);

            if (getComment == null) return null;

            return getComment;
        }
    }
}
