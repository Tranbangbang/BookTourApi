using BookTour.Dto.Common;
using BookTour.Dto.Request;
using BookTour.Dto.Response;
using BookTour.Models;
using BookTour.Repository;

namespace BookTour.Service.Impl
{
    public class TourService : ITourService
    {
        private readonly ITourRepository _tourRepository;

        public TourService(ITourRepository tourRepository)
        {
            _tourRepository = tourRepository;
        }

        public async Task<ApiResponse<List<TourResponse>>> GetAllToursAsync()
        {
            try
            {
                var tours = await _tourRepository.GetAllToursAsync();
                var response = tours.Select(MapToTourResponse).ToList();
                return ApiResponse<List<TourResponse>>.SuccessResponse(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TourResponse>>.ErrorResponse($"Lỗi khi lấy danh sách tour: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<TourResponse>>> GetFeaturedToursAsync()
        {
            try
            {
                var tours = await _tourRepository.GetFeaturedToursAsync();
                var response = tours.Select(MapToTourResponse).ToList();
                return ApiResponse<List<TourResponse>>.SuccessResponse(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TourResponse>>.ErrorResponse($"Lỗi khi lấy danh sách tour nổi bật: {ex.Message}");
            }
        }

        public async Task<ApiResponse<TourDetailResponse>> GetTourByIdAsync(int id)
        {
            try
            {
                var tour = await _tourRepository.GetTourByIdAsync(id);
                if (tour == null)
                {
                    return ApiResponse<TourDetailResponse>.ErrorResponse("Tour không tồn tại");
                }

                var response = MapToTourDetailResponse(tour);
                return ApiResponse<TourDetailResponse>.SuccessResponse(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<TourDetailResponse>.ErrorResponse($"Lỗi khi lấy chi tiết tour: {ex.Message}");
            }
        }

    

        #region Helper Methods
        private TourResponse MapToTourResponse(Tour tour)
        {
            return new TourResponse
            {
                TourId = tour.TourId,
                TourName = tour.TourName,
                Description = tour.Description,
                Duration = tour.Duration,
                Transportation = tour.Transportation,
                AdultPrice = tour.AdultPrice,
                ChildPrice = tour.ChildPrice,
                //IsFeatured = tour.IsFeatured,
                PrimaryImageUrl = tour.TourImages
                    .FirstOrDefault(i => i.IsPrimary)?.ImageUrl ??
                    tour.TourImages.FirstOrDefault()?.ImageUrl
            };
        }

        private TourDetailResponse MapToTourDetailResponse(Tour tour)
        {
            return new TourDetailResponse
            {
                TourId = tour.TourId,
                TourName = tour.TourName,
                Description = tour.Description,
                Duration = tour.Duration,
                Transportation = tour.Transportation,
                AdultPrice = tour.AdultPrice,
                ChildPrice = tour.ChildPrice,
                IsFeatured = tour.IsFeatured,
                imageCover = tour.TourImages.FirstOrDefault()?.ImageUrl,
                Images = tour.TourImages.Select(i => new TourImageResponse
                {
                    ImageId = i.ImageId,
                    ImageUrl = i.ImageUrl,
                    IsPrimary = i.IsPrimary
                }).ToList(),
                Schedules = tour.TourSchedules.OrderBy(s => s.DayNumber).Select(s => new TourScheduleResponse
                {
                    ScheduleId = s.ScheduleId,
                    DayNumber = s.DayNumber,
                    Description = s.Description,
                    Activities = s.Activities
                }).ToList(),
                Destinations = tour.TourDestinations.OrderBy(td => td.OrderNumber).Select(td => new DestinationResponse
                {
                    DestinationId = td.Destination.DestinationId,
                    DestinationName = td.Destination.DestinationName,
                    Description = td.Destination.Description,
                    PrimaryImageUrl = td.Destination.DestinationImages
                        .FirstOrDefault(i => i.IsPrimary)?.ImageUrl ??
                        td.Destination.DestinationImages.FirstOrDefault()?.ImageUrl
                }).ToList(),
                Reviews = tour.Reviews.OrderByDescending(r => r.ReviewDate).Select(r => new ReviewResponse
                {
                    ReviewId = r.ReviewId,
                    UserName = r.User.FullName,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    ReviewDate = r.ReviewDate
                }).ToList()
            };
        }

        public async Task<ApiResponse<List<TourResponse>>> SearchToursAsync(SearchTourRequest request)
        {
            try
            {
                var tours = await _tourRepository.SearchToursAsync(
                    request.Destination,
                    request.MinPrice,
                    request.MaxPrice,
                    request.Duration);

                var response = tours.Select(MapToTourResponse).ToList();
                return ApiResponse<List<TourResponse>>.SuccessResponse(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TourResponse>>.ErrorResponse($"Lỗi khi tìm kiếm tour: {ex.Message}");
            }
        }


        public async Task<ApiResponse<BookingResponse>> BookTourAsync(BookTourRequest request)
        {
            try
            {
                var tour = await _tourRepository.GetTourByIdAsync(request.TourId);
                if (tour == null)
                {
                    return ApiResponse<BookingResponse>.ErrorResponse("Tour không tồn tại");
                }
                decimal totalAmount = (request.AdultCount * tour.AdultPrice) + (request.ChildCount * tour.ChildPrice);

                var booking = new Booking
                {
                    UserId = request.UserId,
                    TourId = request.TourId,
                    TourDate = request.TourDate,
                    AdultCount = request.AdultCount,
                    ChildCount = request.ChildCount,
                    TotalAmount = totalAmount,
                    Status = "Đang chờ",
                    PaymentStatus = "Chưa thanh toán",
                    PaymentMethod = request.PaymentMethod,
                    BookingDate = DateTime.Now
                };

                var createdBooking = await _tourRepository.CreateBookingAsync(booking);

                if (request.Passengers != null && request.Passengers.Count > 0)
                {
                    var bookingDetails = request.Passengers.Select(p => new BookingDetail
                    {
                        BookingId = createdBooking.BookingId,
                        PassengerName = p.PassengerName,
                        PassengerType = p.PassengerType,
                        Price = p.PassengerType == "Adult" ? tour.AdultPrice : tour.ChildPrice
                    }).ToList();

                    await _tourRepository.CreateBookingDetailsAsync(bookingDetails);
                }

                var response = new BookingResponse
                {
                    BookingId = createdBooking.BookingId,
                    TourName = tour.TourName,
                    TourDate = createdBooking.TourDate,
                    AdultCount = createdBooking.AdultCount,
                    ChildCount = createdBooking.ChildCount,
                    TotalAmount = createdBooking.TotalAmount,
                    Status = createdBooking.Status,
                    PaymentStatus = createdBooking.PaymentStatus,
                    BookingDate = createdBooking.BookingDate
                };

                return ApiResponse<BookingResponse>.SuccessResponse(response, "Đặt tour thành công");
            }
            catch (Exception ex)
            {
                return ApiResponse<BookingResponse>.ErrorResponse($"Lỗi khi đặt tour: {ex.Message}");
            }
        }


        public async Task<ApiResponse<List<BookingResponse>>> GetBookingHistoryAsync(int userId)
        {
            try
            {
                var bookings = await _tourRepository.GetBookingsByUserIdAsync(userId);

                var response = bookings.Select(b => new BookingResponse
                {
                    BookingId = b.BookingId,
                    TourName = b.Tour.TourName,
                    TourDate = b.TourDate,
                    AdultCount = b.AdultCount,
                    ChildCount = b.ChildCount,
                    TotalAmount = b.TotalAmount,
                    Status = b.Status,
                    PaymentStatus = b.PaymentStatus,
                    BookingDate = b.BookingDate,
                    ImageUrl = b.Tour.TourImages.FirstOrDefault(i => i.IsPrimary)?.ImageUrl ??
                        b.Tour.TourImages.FirstOrDefault()?.ImageUrl
                }).ToList();

                return ApiResponse<List<BookingResponse>>.SuccessResponse(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<BookingResponse>>.ErrorResponse($"Lỗi khi lấy lịch sử đặt tour: {ex.Message}");
            }
        }

        #endregion
    }
}