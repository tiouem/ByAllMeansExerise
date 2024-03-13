using ByAllMeans.Api.Features.Shared.Repositories;
using ByAllMeans.Api.Features.Sneakers.Get;
using Moq;

namespace ByAllMeans.Test.Sneakers.Unit;

public class GetSneakersTests
{
    [Test]
    public async Task GetSneakers_CallsRepositoryOnce()
    {
        var request = new GetSneakersById.Query(Guid.Empty);
        var repoMock = new Mock<ISneakersRepository>();
        var handler = new GetSneakersById.GetSneakersByIdHandler(repoMock.Object);

        var res = await handler.Handle(request,It.IsAny<CancellationToken>());

        
        repoMock.Verify(x=>x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }
}