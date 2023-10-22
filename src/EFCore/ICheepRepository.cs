namespace EFCore;

public interface ICheepRepository
{
    public Tast CreateCheep(CheepDto newCheep);
    public Task<IEnumerable<CheepDto>> GetCheeps(int offset);
    public Task<IEnumerable<CheepDto>> GetCheepFromAuthor(string user, int offset);
}