using BookTour.Dto.Common;
using BookTour.Dto.Request;
using BookTour.Dto.Response;

namespace BookTour.Service
{
    public interface ITourService
    {
        Task<ApiResponse<List<TourResponse>>> GetAllToursAsync();
        Task<ApiResponse<List<TourResponse>>> GetFeaturedToursAsync();
        Task<ApiResponse<TourDetailResponse>> GetTourByIdAsync(int id);
        Task<ApiResponse<List<TourResponse>>> SearchToursAsync(SearchTourRequest request);
        Task<ApiResponse<BookingResponse>> BookTourAsync(BookTourRequest request);
    }
}