namespace EFCore;

public interface IFollowerListRepository
{
    public Task Follow(string authorFollows, string authorFollowed);
    public Task UnFollow(string authorFollows, string authorFollowed);
    public Task<IEnumerable<FollowDto>> GetFollowers(string authorName);
}