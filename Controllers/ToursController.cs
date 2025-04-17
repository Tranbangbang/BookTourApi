using BookTour.Dto.Common;
using BookTour.Dto.Request;
using BookTour.Dto.Response;
using BookTour.Service;
using Microsoft.AspNetCore.Mvc;

namespace BookTour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToursController : ControllerBase
    {
        private readonly ITourService _tourService;

        public ToursController(ITourService tourService)
        {
            _tourService = tourService;
        }

        // GET: api/Tours
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<TourResponse>>>> GetTours()
        {
            var response = await _tourService.GetAllToursAsync();
            if (response.Success)
            {
                return Ok(response);
            }
            return StatusCode(500, response);
        }

        // GET: api/Tours/Featured
        [HttpGet("Featured")]
        public async Task<ActionResult<ApiResponse<List<TourResponse>>>> GetFeaturedTours()
        {
            var response = await _tourService.GetFeaturedToursAsync();
            if (response.Success)
            {
                return Ok(response);
            }
            return StatusCode(500, response);
        }

        // GET: api/Tours/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<TourDetailResponse>>> GetTour(int id)
        {
            var response = await _tourService.GetTourByIdAsync(id);
            if (response.Success)
            {
                return Ok(response);
            }
            if (response.Message == "Tour không tồn tại")
            {
                return NotFound(response);
            }
            return StatusCode(500, response);
        }

        // POST: api/Tours/Search
        [HttpPost("Search")]
        public async Task<ActionResult<ApiResponse<List<TourResponse>>>> SearchTours(SearchTourRequest request)
        {
            var response = await _tourService.SearchToursAsync(request);
            if (response.Success)
            {
                return Ok(response);
            }
            return StatusCode(500, response);
        }

        // POST: api/Tours/Book
        [HttpPost("Book")]
        public async Task<ActionResult<ApiResponse<BookingResponse>>> BookTour(BookTourRequest request)
        {
            var response = await _tourService.BookTourAsync(request);
            if (response.Success)
            {
                return Ok(response);
            }
            if (response.Message == "Tour không tồn tại")
            {
                return NotFound(response);
            }
            return StatusCode(500, response);
        }
    }
}