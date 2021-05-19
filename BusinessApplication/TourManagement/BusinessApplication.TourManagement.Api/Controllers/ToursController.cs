using AutoMapper;
using BusinessApplication.TourManagement.Api.ActionConstraints;
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
        //public async Task<IActionResult> GetDefaultTour(Guid tourId, [FromHeader(Name ="Accept")] string acceptHeaderValue )
        public async Task<IActionResult> GetDefaultTour(Guid tourId)
        {
            return await GetSpecificTour<Tour>(tourId);
        }

        [HttpGet("{tourId}")]
        [RequestHeaderMatchesMediaType("Accept", new[] { "application/vnd.marvin.tour+json" })]
        public async Task<IActionResult> GetTour(Guid tourId)
        {
            return await GetSpecificTour<Tour>(tourId);
        }

        [HttpGet("{tourId}")]
        [RequestHeaderMatchesMediaType("Accept", new[] { "application/vnd.marvin.tourwithestimatedprofits+json" })]
        public async Task<IActionResult> GetTourWithEstimatedProfit(Guid tourId)
        {
            return await GetSpecificTour<TourWithEstimatedProfit>(tourId);
        }

        [HttpPost]
        [RequestHeaderMatchesMediaType("Content-Type", new[] { "application/json", "application/vnd.marvin.tourforcreation+json" })]
        public async Task<IActionResult> AddTour([FromBody] TourForCreation tour)
        {
            return await AddSpecificTour(tour);
        }

        [HttpPost]
        [RequestHeaderMatchesMediaType("Content-Type", new[] { "application/vnd.marvin.tourwithmanagerforcreation+json" })]
        public async Task<IActionResult> AddTourWithManager(
            [FromBody] TourWithManagerForCreation tour)
        {
            return await AddSpecificTour(tour);
        }

        private async Task<IActionResult> GetSpecificTour<T>(Guid tourId) where T : class
        {
            var tourFromRepo = await this.repository.GetTour(tourId);
            if (tourFromRepo == null)
            {
                return BadRequest();
            }
            var tour = this.mapper.Map<T>(tourFromRepo);
            return Ok(tour);
        }

        public async Task<IActionResult> AddSpecificTour<T>(T tour) where T : class
        {
            if(tour == null)
            {
                return BadRequest();
            }
            var tourEntity = this.mapper.Map<Entities.Tour>(tour);

            if (tourEntity.ManagerId == Guid.Empty)
            {
                tourEntity.ManagerId = new Guid("fec0a4d6-5830-4eb8-8024-272bd5d6d2bb");
            }

            await this.repository.AddTour(tourEntity);

            if (!await this.repository.SaveAsync())
            {
                throw new Exception("Adding a tour failed on save.");
            }

            var tourToReturn = this.mapper.Map<Tour>(tourEntity);

            return CreatedAtRoute("GetTour",
                new { tourId = tourToReturn.TourId },
                tourToReturn);
        }

    }
}
