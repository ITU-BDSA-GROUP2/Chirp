namespace EFCore;

public interface IAuthorRepository
{
    public void CreateNewAuthor(AuthorDto newAuthor);
    public Task<AuthorDto> GetAuthorByName(string authorName);
    public Task<AuthorDto> GetAuthorByEmail(string authorEmail);
}