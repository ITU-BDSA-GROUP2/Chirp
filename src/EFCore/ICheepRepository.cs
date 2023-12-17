namespace EFCore;

// <summary>
//   This class represents an interface to a repository for 'Cheep'
//   This interface contains method-signatures for creating and deleting cheeps 
//   and querying the database for cheeps based on different search-criteria. 
// </summary>

public interface ICheepRepository
{
    public Task CreateCheep(string text, string author, DateTime Timestamp);
    public Task<IEnumerable<CheepDto>> GetCheeps(int offset);
    public Task<IEnumerable<CheepDto>> GetCheepsFromAuthor(string user, int offset);
    public Task<int> GetAllCheeps();
    public Task<int> GetAllCheepsFromAuthor(string user);
    public Task<IEnumerable<CheepDto>> GetAllCheepsFromFollowed(string user, int offset);
    public Task<int> GetAllCheepsFromFollowedCount(string user);

}