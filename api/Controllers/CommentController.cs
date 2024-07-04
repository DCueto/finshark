using api.DTOs.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;

        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepository.GetAllAsync();
            var commentsDto = comments.Select(s => s.ToCommentDto());
            
            return Ok(commentsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
                return NotFound();

            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentRequestDto commentRequestDto)
        {
            var doStockExists = await _stockRepository.StockExists(stockId);
            if (!doStockExists)
                return BadRequest($"The stockId {stockId} received doesn't exists in db. Send an existing stock");

            var commentModel = commentRequestDto.ToCommentFromCreate(stockId);
            await _commentRepository.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto commentRequestDto)
        {
            var comment = await _commentRepository.UpdateAsync(id, commentRequestDto.ToCommentFromUpdate());
            if (comment == null)
                return NotFound($"Comment not found with id {id}");
            
            // Console.WriteLine($"CommentDto before mapped to Comment: Title: {commentRequestDto.Title}, Content: {commentRequestDto.Content}");
            //
            // Console.WriteLine($"Comment after mapped from UpdateDto: Id: {comment.Id}, " +
            //                   $"Title: {comment.Title}, Content: {comment.Content}, StockId: {comment.StockId}, " +
            //                   $"CreatedOn: {comment.CreatedOn}");
            
            return Ok(comment.ToCommentDto());
        }
        
    }
}
