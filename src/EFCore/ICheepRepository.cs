namespace EFCore;

public interface ICheepRepository
{
    public Task<IEnumerable<CheepDto>> GetCheeps(int offset);
}