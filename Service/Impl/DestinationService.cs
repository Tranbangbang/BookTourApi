using BookTour.Dto.Common;
using BookTour.Dto.Response;
using BookTour.Models;
using BookTour.Repository;

namespace BookTour.Service.Impl
{
    public class DestinationService : IDestinationService
    {
        private readonly IDestinationRepository _destinationRepository;

        public DestinationService(IDestinationRepository destinationRepository)
        {
            _destinationRepository = destinationRepository;
        }

        public async Task<ApiResponse<List<DestinationListResponse>>> GetAllDestinationsAsync()
        {
            try
            {
                var destinations = await _destinationRepository.GetAllDestinationsAsync();
                var response = destinations.Select(MapToDestinationListResponse).ToList();
                return ApiResponse<List<DestinationListResponse>>.SuccessResponse(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<DestinationListResponse>>.ErrorResponse($"Lỗi khi lấy danh sách điểm đến: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<DestinationListResponse>>> GetFeaturedDestinationsAsync()
        {
            try
            {
                var destinations = await _destinationRepository.GetFeaturedDestinationsAsync();
                var response = destinations.Select(MapToDestinationListResponse).ToList();
                return ApiResponse<List<DestinationListResponse>>.SuccessResponse(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<DestinationListResponse>>.ErrorResponse($"Lỗi khi lấy danh sách điểm đến nổi bật: {ex.Message}");
            }
        }

        public async Task<ApiResponse<DestinationDetailResponse>> GetDestinationByIdAsync(int id)
        {
            try
            {
                var destination = await _destinationRepository.GetDestinationByIdAsync(id);
                if (destination == null)
                {
                    return ApiResponse<DestinationDetailResponse>.ErrorResponse("Điểm đến không tồn tại");
                }

                var response = MapToDestinationDetailResponse(destination);
                return ApiResponse<DestinationDetailResponse>.SuccessResponse(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<DestinationDetailResponse>.ErrorResponse($"Lỗi khi lấy chi tiết điểm đến: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<DestinationListResponse>>> GetDestinationsByCityIdAsync(int cityId)
        {
            try
            {
                var destinations = await _destinationRepository.GetDestinationsByCityIdAsync(cityId);
                var response = destinations.Select(MapToDestinationListResponse).ToList();
                return ApiResponse<List<DestinationListResponse>>.SuccessResponse(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<DestinationListResponse>>.ErrorResponse($"Lỗi khi lấy danh sách điểm đến theo thành phố: {ex.Message}");
            }
        }

        #region Helper Methods
        private DestinationListResponse MapToDestinationListResponse(Destination destination)
        {
            return new DestinationListResponse
            {
                DestinationId = destination.DestinationId,
                DestinationName = destination.DestinationName,
                Description = destination.Description,
                CityId = destination.CityId,
                CityName = destination.City.CityName,
                IsFeatured = destination.IsFeatured,
                PrimaryImageUrl = destination.DestinationImages
                    .FirstOrDefault(i => i.IsPrimary)?.ImageUrl ??
                    destination.DestinationImages.FirstOrDefault()?.ImageUrl
            };
        }

        private DestinationDetailResponse MapToDestinationDetailResponse(Destination destination)
        {
            return new DestinationDetailResponse
            {
                DestinationId = destination.DestinationId,
                DestinationName = destination.DestinationName,
                Description = destination.Description,
                CityId = destination.CityId,
                CityName = destination.City.CityName,
                IsFeatured = destination.IsFeatured,
                Images = destination.DestinationImages.Select(i => new DestinationImageResponse
                {
                    ImageId = i.ImageId,
                    ImageUrl = i.ImageUrl,
                    IsPrimary = i.IsPrimary
                }).ToList(),
                imageCover = destination.DestinationImages.FirstOrDefault()?.ImageUrl,
                Details = destination.DestinationDetails.Select(d => new DestinationDetailItemResponse
                {
                    DetailId = d.DetailId,
                    FeatureType = d.FeatureType,
                    FeatureValue = d.FeatureValue
                }).ToList(),
                RelatedTours = destination.TourDestinations.Select(td => new TourResponse
                {
                    TourId = td.Tour.TourId,
                    TourName = td.Tour.TourName,
                    Description = td.Tour.Description,
                    Duration = td.Tour.Duration,
                    Transportation = td.Tour.Transportation,
                    AdultPrice = td.Tour.AdultPrice,
                    ChildPrice = td.Tour.ChildPrice,
                    //IsFeatured = td.Tour.IsFeatured,
                    PrimaryImageUrl = td.Tour.TourImages
                        .FirstOrDefault(i => i.IsPrimary)?.ImageUrl ??
                        td.Tour.TourImages.FirstOrDefault()?.ImageUrl
                }).ToList()
            };
        }
        #endregion
    }
}