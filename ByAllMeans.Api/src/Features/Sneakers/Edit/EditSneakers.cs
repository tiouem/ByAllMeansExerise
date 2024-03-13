using ByAllMeans.Api.Features.Shared;
using ByAllMeans.Api.Features.Shared.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ByAllMeans.Api.Features.Sneakers.Edit;

public sealed class Edit : SneakersController
{
    [HttpPut]
    public async Task<EditSneakers.Response> EditSneakers([FromServices] ISender sender,
        [FromBody] EditSneakers.Command command)
    {
        return await sender.Send(command);
    }
}

public static class EditSneakers
{
    public record Command(Guid Id, string Name, string Brand, double Price, int Size, int Year, Rating Rating)
        : IRequest<Response>;

    public record Response(Sneakers Sneakers);

    internal sealed class EditSneakersHandler : IRequestHandler<Command, Response>
    {
        private readonly ISneakersRepository _repository;

        public EditSneakersHandler(ISneakersRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var res = await _repository.EditAsync(request);
            return new Response(res);
        }
    }
}