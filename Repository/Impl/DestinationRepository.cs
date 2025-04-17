using BookTour.Models;
using Microsoft.EntityFrameworkCore;

namespace BookTour.Repository.Impl
{
    public class DestinationRepository : IDestinationRepository
    {
        private readonly BookTourContext _context;

        public DestinationRepository(BookTourContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Destination>> GetAllDestinationsAsync()
        {
            return await _context.Destinations
                .Include(d => d.City)
                .Include(d => d.DestinationImages)
                .Where(d => d.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Destination>> GetFeaturedDestinationsAsync()
        {
            return await _context.Destinations
                .Include(d => d.City)
                .Include(d => d.DestinationImages)
                .Where(d => d.IsActive && d.IsFeatured)
                .ToListAsync();
        }

        public async Task<Destination> GetDestinationByIdAsync(int id)
        {
            return await _context.Destinations
                .Include(d => d.City)
                .Include(d => d.DestinationImages)
                .Include(d => d.DestinationDetails)
                .Include(d => d.TourDestinations)
                    .ThenInclude(td => td.Tour)
                        .ThenInclude(t => t.TourImages)
                .FirstOrDefaultAsync(d => d.DestinationId == id);
        }

        public async Task<IEnumerable<Destination>> GetDestinationsByCityIdAsync(int cityId)
        {
            return await _context.Destinations
                .Include(d => d.City)
                .Include(d => d.DestinationImages)
                .Where(d => d.IsActive && d.CityId == cityId)
                .ToListAsync();
        }
    }
}