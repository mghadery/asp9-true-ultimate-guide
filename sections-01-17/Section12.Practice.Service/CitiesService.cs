using Section12.Practice.IService;

namespace Section12.Practice.Service;

public class CitiesService:ICitiesService
{
    public Guid Id { get; init; }
    public CitiesService()
    {
        Id = Guid.NewGuid();
    }
    public IEnumerable<string> GetCityNames()
    {
        return new[] { Id.ToString(), "Tehran", "Shiraz", "Mashad" };
    }
}
