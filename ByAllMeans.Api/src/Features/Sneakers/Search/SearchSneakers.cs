using ByAllMeans.Api.Features.Shared;
using ByAllMeans.Api.Features.Shared.Repositories;
using ByAllMeans.Api.Features.Sneakers.Add;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ByAllMeans.Api.Features.Sneakers.Search;


public sealed class Search : SneakersController
{
    [HttpPost]
    [Route("Search")]
    public async Task<SearchSneakers.Response> AddSneakers([FromServices] ISender sender,
        [FromBody] SearchSneakers.Query command)
    {
        return await sender.Send(command);
    }
}
public static class SearchSneakers
{
    public record Query(Guid UserId,SortBy SortBy, string SearchBy) : IRequest<Response>;

    public record Response(int Results, IEnumerable<Features.Sneakers.Sneakers> Sneakers);

    internal sealed class SearchSneakersHandler : IRequestHandler<Query, Response>
    {
        private readonly ISneakersRepository _repository;

        public SearchSneakersHandler(ISneakersRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var res = await _repository.SearchAsync(request);

            var sneakersEnumerable = res as Features.Sneakers.Sneakers[] ?? res.ToArray();
            return new Response(sneakersEnumerable.Count(),sneakersEnumerable);
        }
    }
}