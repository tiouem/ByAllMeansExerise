using ByAllMeans.Api.Features.Sneakers.Add;
using ByAllMeans.Api.Features.Sneakers.Edit;
using ByAllMeans.Api.Features.Sneakers.Search;

namespace ByAllMeans.Api.Features.Shared.Repositories;

public interface ISneakersRepository
{
    Task<IEnumerable<Sneakers.Sneakers>> SearchAsync(SearchSneakers.Query query);
    Task<IEnumerable<Sneakers.Sneakers>> AddAsync(AddSneakers.Command command);
    Task<Sneakers.Sneakers> EditAsync(EditSneakers.Command request);
    Task<Sneakers.Sneakers?> GetByIdAsync(Guid id);
    Task<bool> DeleteAsync(Guid id);
}