using FinShark.DTOs.Comment;
using FinShark.Models;

namespace FinShark.Mappers
{
    public static class CommentMapper
    {
        public static CommentDTO ToCommentDTO(this Comment commentModel)
        {
            return new CommentDTO
            {
                CommentId = commentModel.CommentId,
                Title = commentModel.Title,
                Content = commentModel.Content,
                TimePosted = commentModel.TimePosted,
                StockId = commentModel.StockId,
            };
        }

        // Why stockId as a parameter?
        public static Comment ToCommentFromCreateDTO(this CreateCommentRequestDTO commentDto, int stockId)
        {
            return new Comment
            {
                Title = commentDto.Title,
                Content = commentDto.Content,
                StockId = stockId
            };
        }
    }
}
