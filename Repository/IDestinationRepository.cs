using BookTour.Models;

namespace BookTour.Repository
{
    public interface IDestinationRepository
    {
        Task<IEnumerable<Destination>> GetAllDestinationsAsync();
        Task<IEnumerable<Destination>> GetFeaturedDestinationsAsync();
        Task<Destination> GetDestinationByIdAsync(int id);
        Task<IEnumerable<Destination>> GetDestinationsByCityIdAsync(int cityId);
    }
}
