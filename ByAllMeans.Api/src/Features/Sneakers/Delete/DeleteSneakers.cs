using ByAllMeans.Api.Features.Shared;
using ByAllMeans.Api.Features.Shared.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ByAllMeans.Api.Features.Sneakers.Delete;

public sealed class Delete: SneakersController
{
    [HttpDelete]
    public async Task<DeleteSneakers.Response> DeleteSneakers([FromServices] ISender sender, [FromQuery] Guid id)
    {
        return await sender.Send(new DeleteSneakers.Command(id));
    }
}

public static class DeleteSneakers
{
    public record Command(Guid id) : IRequest<Response>;

    public record Response(bool Success);

    internal sealed class DeleteSneakersHandler : IRequestHandler<Command, Response>
    {
        private readonly ISneakersRepository _repository;

        public DeleteSneakersHandler(ISneakersRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var result = await _repository.DeleteAsync(request.id);

            return new Response(result);
        }
    }
}