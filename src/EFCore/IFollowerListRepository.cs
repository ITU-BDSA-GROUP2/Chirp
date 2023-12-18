namespace EFCore;

// <summary>
//   This class represents an interface to a repository for 'Followerlist'
//   This interface contains method-signatures for following and unfollowing authors
// </summary>

public interface IFollowerListRepository
{
    public Task Follow(string authorFollows, string authorFollowed);
    public Task UnFollow(string authorFollows, string authorFollowed);
    public Task<IEnumerable<FollowDto>> GetFollowers(string authorName);
}