namespace EFCore;

public interface IAuthorRepository
{
    public Task CreateNewAuthor(string name, string email);
    public Task<AuthorDto> GetAuthorByName(string authorName);
    public Task<AuthorDto> GetAuthorByEmail(string authorEmail);
}