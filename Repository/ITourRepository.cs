using BookTour.Models;

namespace BookTour.Repository
{
    public interface ITourRepository
    {
        Task<IEnumerable<Tour>> GetAllToursAsync();
        Task<IEnumerable<Tour>> GetFeaturedToursAsync();
        Task<Tour> GetTourByIdAsync(int id);
        Task<IEnumerable<Tour>> SearchToursAsync(string destination, decimal? minPrice, decimal? maxPrice, int? duration);
        Task<Booking> CreateBookingAsync(Booking booking);
        Task<IEnumerable<BookingDetail>> CreateBookingDetailsAsync(IEnumerable<BookingDetail> bookingDetails);
    }
}
