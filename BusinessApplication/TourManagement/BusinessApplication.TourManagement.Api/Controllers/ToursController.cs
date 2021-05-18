using AutoMapper;
using BusinessApplication.TourManagement.Api.Dtos;
using BusinessApplication.TourManagement.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessApplication.TourManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToursController : ControllerBase
    {
        private readonly ITourManagementRepository repository;
        private readonly IMapper mapper;

        public ToursController(ITourManagementRepository repository, IMapper mapper)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IActionResult> GetTours()
        {
            var toursFromRepo = await this.repository.GetTours();
            var tours = this.mapper.Map<IEnumerable<Tour>>(toursFromRepo);
            return Ok(tours);
        }

        [HttpGet("{tourId}", Name = "GetTour")]
        public async Task<IActionResult> GetTour(Guid tourId)
        {
            var tourFromRepo = await this.repository.GetTour(tourId);

            if (tourFromRepo == null)
            {
                return BadRequest();
            }

            var tour = this.mapper.Map<Tour>(tourFromRepo);

            return Ok(tour);
        }
    }
}
