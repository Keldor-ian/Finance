using FinShark.DTOs.Comment;
using FinShark.Interfaces;
using FinShark.Mappers;
using FinShark.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;

// Next Steps:

// Add FK relationship between Comments and Stocks (Adding a list of comments to a stock)
// Adding the Comment List property to the Stock/StockDTO

namespace FinShark.Controllers
{
    [ApiController]
    [Route("api/comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        [Route("{commentId:int}")]

        public async Task<IActionResult> GetCommentById([FromRoute] int commentId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var getCommentById = await _commentRepo.GetCommentByIdAsync(commentId);
            
            if (getCommentById == null) return BadRequest("The comment being retrieved doesn't exist!");

            return Ok(getCommentById.ToCommentDTO());
        }

        [HttpPost]
        [Route("{stockId:int}")]

        public async Task<IActionResult> CreateCommentAsync([FromRoute] int stockId, [FromBody] CreateCommentRequestDTO commentDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var getStock = await _stockRepo.StockExists(stockId);

            if (getStock == false) return BadRequest($"Stock with an id: {stockId} was not found!");

            var commentModel = commentDto.ToCommentFromCreateDTO(stockId);

            await _commentRepo.CreateCommentAsync(commentModel);

            if (commentModel == null) return BadRequest("Stock does not exist!");

            return CreatedAtAction(nameof(GetCommentById), new { commentId = commentModel.CommentId }, commentModel.ToCommentDTO());
        }

    }
}
