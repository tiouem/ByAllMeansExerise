using ByAllMeans.Api.Features.Shared;

namespace ByAllMeans.Api.Features.Sneakers;

public class Sneakers
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public double Price { get; set; }
    public int Size { get; set; }
    public int Year { get; set; }
    public Rating Rating { get; set; }
}