using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Comment>> GetAllAsync()
    {
        return await _context.Comments.Include(c => c.AppUser).ToListAsync();
    }

    
    public async Task<Comment?> GetByIdAsync(int id)
    {
        return await _context.Comments.Include(c => c.AppUser).FirstOrDefaultAsync( c => c.Id == id);
    }

    public async Task<Comment> CreateAsync(Comment commentModel)
    {
        await _context.Comments.AddAsync(commentModel);
        await _context.SaveChangesAsync();

        return commentModel;
    }

    public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
    {
        var existingComment = await _context.Comments.FirstOrDefaultAsync( c => c.Id == id);
        if (existingComment == null)
            return null;

        existingComment.Title = commentModel.Title;
        existingComment.Content = commentModel.Content;

        await _context.SaveChangesAsync();

        return existingComment;
    }

    public async Task<Comment?> DeleteAsync(int id)
    {
        var existingComment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
        if (existingComment == null)
            return null;
        
        _context.Comments.Remove(existingComment);
        await _context.SaveChangesAsync();

        return existingComment;
    }

    public async Task<bool> CommentExists(int id)
    {
        return await _context.Comments.AnyAsync(c => c.Id == id);
    }
}