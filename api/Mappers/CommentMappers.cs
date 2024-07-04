using api.DTOs.Comment;
using api.DTOs.Stock;
using api.Models;

namespace api.Mappers;

public static class CommentMappers
{
    public static CommentDto ToCommentDto(this Comment commentModel)
    {
        return new CommentDto
        {
            Id = commentModel.Id,
            Title = commentModel.Title,
            Content = commentModel.Content,
            CreatedOn = commentModel.CreatedOn,
            StockId = commentModel.StockId
        };
    }

    public static Comment ToCommentFromCreate(this CreateCommentRequestDto commentRequestDto, int stockId)
    {
        return new Comment
        {
            Title = commentRequestDto.Title,
            Content = commentRequestDto.Content,
            StockId = stockId
        };
    }
    
    public static Comment ToCommentFromUpdate(this UpdateCommentRequestDto commentRequestDto)
    {
        return new Comment
        {
            Title = commentRequestDto.Title,
            Content = commentRequestDto.Content
        };
    }
}