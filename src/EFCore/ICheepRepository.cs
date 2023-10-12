public interface ICheepRepository {
    
Task CreateCheep(CheepDto cheep);
Task<IEnumerable<CheepDto>> GetCheepFromPage(int page);
Task<IEnumerable<CheepDto>> GetCheepFromAuthor(string authorname, int? page);
}