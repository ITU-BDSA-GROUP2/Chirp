namespace EFCore;

public interface ILikeRepository
{
    public Task Like(int cheepId, string user);
    public Task Dislike(int cheepId, string user);
    public Task<bool> IsLiked(int id, string userName);
}