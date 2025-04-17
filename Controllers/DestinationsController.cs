using BookTour.Dto.Common;
using BookTour.Dto.Response;
using BookTour.Service;
using Microsoft.AspNetCore.Mvc;

namespace BookTour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationsController : ControllerBase
    {
        private readonly IDestinationService _destinationService;

        public DestinationsController(IDestinationService destinationService)
        {
            _destinationService = destinationService;
        }

        // GET: api/Destinations
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<DestinationListResponse>>>> GetDestinations()
        {
            var response = await _destinationService.GetAllDestinationsAsync();
            if (response.Success)
            {
                return Ok(response);
            }
            return StatusCode(500, response);
        }

        // GET: api/Destinations/Featured
        [HttpGet("Featured")]
        public async Task<ActionResult<ApiResponse<List<DestinationListResponse>>>> GetFeaturedDestinations()
        {
            var response = await _destinationService.GetFeaturedDestinationsAsync();
            if (response.Success)
            {
                return Ok(response);
            }
            return StatusCode(500, response);
        }

        // GET: api/Destinations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<DestinationDetailResponse>>> GetDestination(int id)
        {
            var response = await _destinationService.GetDestinationByIdAsync(id);
            if (response.Success)
            {
                return Ok(response);
            }
            if (response.Message == "Điểm đến không tồn tại")
            {
                return NotFound(response);
            }
            return StatusCode(500, response);
        }

        // GET: api/Destinations/City/5
        [HttpGet("City/{cityId}")]
        public async Task<ActionResult<ApiResponse<List<DestinationListResponse>>>> GetDestinationsByCity(int cityId)
        {
            var response = await _destinationService.GetDestinationsByCityIdAsync(cityId);
            if (response.Success)
            {
                return Ok(response);
            }
            return StatusCode(500, response);
        }
    }
}