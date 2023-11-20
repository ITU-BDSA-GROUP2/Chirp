namespace EFCore;

public interface IFollowerListRepository
{
    public Task Follow(AuthorDto authorFollows, AuthorDto authorFollowed);
    public Task UnFollow(AuthorDto authorFollows, AuthorDto authorFollowed);
}