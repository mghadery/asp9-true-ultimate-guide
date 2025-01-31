using Section12.IService;

namespace Section12.Service;

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
