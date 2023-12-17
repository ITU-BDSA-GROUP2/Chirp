namespace EFCore;

// <summary>
//   This class represents an interface to a repository for 'Author'.
//   This interface contains method-signatures for creating and deleting authors, 
//   querying the database for authors based on different search-criteria and
//   updating author information.
// </summary>

public interface IAuthorRepository
{
    public Task CreateNewAuthor(string name, string email);
    public Task<AuthorDto?> GetAuthorByID(int id);
    public Task<AuthorDto?> GetAuthorByName(string authorName);
    public Task<AuthorDto?> GetAuthorByEmail(string authorEmail);
    public Task<string> GetAuthorImageUrl(string authorName);
    public Task SetAuthorImageUrl(string authorName, string imageUrl);
    public Task UpdateAuthor(string oldName, string newName, string email);
    public Task DeleteAuthor(string name);
}