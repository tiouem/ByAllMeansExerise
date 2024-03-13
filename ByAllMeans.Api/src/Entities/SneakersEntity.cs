namespace ByAllMeans.Api.Entities;

public class SneakersEntity : BaseEntity
{
    public string Name { get; set; }
    public string Brand { get; set; }
    public double Price { get; set; }
    public int Size { get; set; }
    public int Year { get; set; }
}