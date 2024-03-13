using ByAllMeans.Api.Features.Shared;
using ByAllMeans.Api.Features.Shared.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ByAllMeans.Api.Features.Sneakers.Get;

public sealed class Get : SneakersController
{
    [HttpGet]
    public async Task<IResult> GetSneakers([FromServices] ISender sender,
        [FromQuery] Guid id)
    {
        
        var res =  await sender.Send(new GetSneakersById.Query(id));

        return  res.Sneakers is null ? Results.NotFound() : Results.Ok(res);
        
    }
}

public static class GetSneakersById
{
    public record Query(Guid Id) : IRequest<Response>;

    public record Response(Sneakers? Sneakers);

    internal sealed class GetSneakersByIdHandler : IRequestHandler<Query, Response>
    {
        private readonly ISneakersRepository _repository;

        public GetSneakersByIdHandler(ISneakersRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var res = await _repository.GetByIdAsync(request.Id);

            return new Response(res);
        }
    }
}