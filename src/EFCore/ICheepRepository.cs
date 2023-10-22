﻿namespace EFCore;

public interface ICheepRepository
{
    public Task CreateCheep(CheepDto newCheep);
    public Task<IEnumerable<CheepDto>> GetCheeps(int offset);
    public Task<IEnumerable<CheepDto>> GetCheepsFromAuthor(string user, int offset);
}