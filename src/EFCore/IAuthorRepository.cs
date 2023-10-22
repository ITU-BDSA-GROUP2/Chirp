namespace EFCore;

public interface IAuthorRepository
{
    public Task CreateNewAuthor(AuthorDto newAuthor);
    public Task GetAuthorByName(string authorName);
    public Task GetAuthorByEmail(string authorEmail);
}