namespace EFCore;

// <summary>
//   This class represents an interface to a repository for 'Like'
//   This interface contains method-signatures for liking and disliking cheeps
// </summary>

public interface ILikeRepository
{
    public Task Like(int cheepId, string user);
    public Task Dislike(int cheepId, string user);
    public Task<bool> IsLiked(int id, string userName);
}