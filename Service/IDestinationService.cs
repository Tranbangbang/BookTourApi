using BookTour.Dto.Common;
using BookTour.Dto.Response;

namespace BookTour.Service
{
    public interface IDestinationService
    {
        Task<ApiResponse<List<DestinationListResponse>>> GetAllDestinationsAsync();
        Task<ApiResponse<List<DestinationListResponse>>> GetFeaturedDestinationsAsync();
        Task<ApiResponse<DestinationDetailResponse>> GetDestinationByIdAsync(int id);
        Task<ApiResponse<List<DestinationListResponse>>> GetDestinationsByCityIdAsync(int cityId);
    }
}