using System.Linq.Expressions;
using ByAllMeans.Api.Database;
using ByAllMeans.Api.Features.Sneakers.Add;
using ByAllMeans.Api.Features.Sneakers.Edit;
using ByAllMeans.Api.Features.Sneakers.Search;
using Microsoft.EntityFrameworkCore;

namespace ByAllMeans.Api.Features.Shared.Repositories;

public class SneakersRepository : ISneakersRepository
{
    private readonly AppDbContext _dbContext;

    public SneakersRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<IEnumerable<Sneakers.Sneakers>> SearchAsync(SearchSneakers.Query query)
    {
        var res = _dbContext.Sneakers.Where(x => x.UserId == query.UserId && x.Name.Contains(query.SearchBy))
            .OrderBy(GetSortBy(query.SortBy));

        return res;
    }

    private Expression<Func<Sneakers.Sneakers, object>> GetSortBy(SortBy querySortBy)
    {
        return querySortBy switch
        {
            SortBy.OldestYear => snk => snk.Year,
            SortBy.SmallestSize => snk => snk.Size,
            SortBy.LowestPrice => snk => snk.Price,
            _ => throw new ArgumentOutOfRangeException(nameof(querySortBy), querySortBy, null)
        };
    }

    public async Task<IEnumerable<Sneakers.Sneakers>> AddAsync(AddSneakers.Command command)
    {
        var toBeCreated = command.SneakersToCreates.Select(x => new Sneakers.Sneakers()
        {
            UserId = command.UserId,
            Name = x.Name,
            Brand = x.Brand,
            Price = x.Price,
            Rating = x.Rating,
            Size = x.Size,
            Year = x.Year
        }).ToArray();

        await _dbContext.Sneakers.AddRangeAsync(toBeCreated);
        await _dbContext.SaveChangesAsync();
        return toBeCreated;
    }

    public async Task<Sneakers.Sneakers> EditAsync(EditSneakers.Command request)
    {
        var entity = await _dbContext.Sneakers.SingleOrDefaultAsync(x => x.Id == request.Id);
        if (entity is null)
            throw new ArgumentException();

        entity.Brand = request.Brand;
        entity.Size = request.Size;
        entity.Name = request.Name;
        entity.Year = request.Year;
        entity.Price = request.Price;
        entity.Rating = request.Rating;

        var res = _dbContext.Sneakers.Update(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<Sneakers.Sneakers?> GetByIdAsync(Guid id)
    {
        var entity = await _dbContext.Sneakers.FindAsync(id);

        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _dbContext.Sneakers.SingleOrDefaultAsync(x => x.Id == id);
        if (entity is null)
            throw new ArgumentException();

        _dbContext.Sneakers.Remove(entity);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}