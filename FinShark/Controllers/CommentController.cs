using FinShark.ClaimExtensions;
using FinShark.DTOs.Comment;
using FinShark.Interfaces;
using FinShark.Mappers;
using FinShark.Models;
using FinShark.Queries.Comments;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;


namespace FinShark.Controllers
{
    [ApiController]
    [Route("api/comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUser> _userManager;
        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCommentsAsync([FromQuery] CommentQueryObject commentQuery)
        {
            var getAllComments = await _commentRepo.GetAllCommentsAsync(commentQuery);

            var comments = getAllComments.Select(s => s.ToCommentDTO());

            return Ok(comments);
        }

        [HttpGet]
        [Route("{commentId:int}")]
        public async Task<IActionResult> GetCommentById([FromRoute] int commentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var getCommentById = await _commentRepo.GetCommentByIdAsync(commentId);
            
            if (getCommentById == null)
            {
                return BadRequest("The comment being retrieved doesn't exist!");
            }

            return Ok(getCommentById.ToCommentDTO());
        }

        [HttpPost]
        [Route("{stockId:int}")]
        public async Task<IActionResult> CreateCommentAsync([FromRoute] int stockId, [FromBody] CreateCommentRequestDTO commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _stockRepo.StockExists(stockId))
            {
                return BadRequest("Stock does not exist!");
            }

            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            var commentModel = commentDto.ToCommentFromCreateDTO(stockId);
            commentModel.AppUserId = appUser.Id;

            await _commentRepo.CreateCommentAsync(commentModel);

            if (commentModel == null)
            {
                return BadRequest("Stock does not exist!");
            }

            return CreatedAtAction(nameof(GetCommentById), new { commentId = commentModel.CommentId }, commentModel.ToCommentDTO());
        }
    }
}
