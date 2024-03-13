using ByAllMeans.Api.Features.Shared;
using ByAllMeans.Api.Features.Shared.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ByAllMeans.Api.Features.Sneakers.Add;

public sealed class Add: SneakersController
{
    [HttpPost]
    public async Task<AddSneakers.Response> AddSneakers([FromServices] ISender sender,
        [FromBody] AddSneakers.Command command)
    {
        return await sender.Send(command);
    }
}

public static class AddSneakers
{
    public record Command(Guid UserId, IEnumerable<SneakersToCreate> SneakersToCreates) : IRequest<Response>;

    public record Response(IEnumerable<Guid> createdSneakersId);

    public record SneakersToCreate(string Name, string Brand, double Price, int Size, int Year, Rating Rating);

    internal sealed class AddSneakerHandler : IRequestHandler<Command, Response>
    {
        private readonly ISneakersRepository _repository;

        public AddSneakerHandler(ISneakersRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response> Handle(Command command, CancellationToken cancellationToken)
        {
            var addedSneakers = await _repository.AddAsync(command);

            var res = addedSneakers.Select(x => x.Id);
            return new Response(res);
        }
    }
}