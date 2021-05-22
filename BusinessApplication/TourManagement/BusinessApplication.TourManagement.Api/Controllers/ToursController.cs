using AutoMapper;
using BusinessApplication.TourManagement.Api.ActionConstraints;
using BusinessApplication.TourManagement.Api.Dtos;
using BusinessApplication.TourManagement.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly IUserInfoService userInfoService;

        public ToursController(ITourManagementRepository repository, IUserInfoService userInfoService, IMapper mapper)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.userInfoService = userInfoService ?? throw new ArgumentNullException(nameof(userInfoService));
        }

        [HttpGet]
        public async Task<IActionResult> GetTours()
        {
            IEnumerable<Entities.Tour> toursFromRepo = new List<Entities.Tour>();

            if (userInfoService.Role == "Administrator")
            {
                toursFromRepo = await repository.GetTours();
            }
            else
            {
                if (!Guid.TryParse(userInfoService.UserId, out Guid userIdAsGuid))
                {
                    return Forbid();
                }

                toursFromRepo = await repository.GetToursForManager(userIdAsGuid);
            }

            var tours = this.mapper.Map<IEnumerable<Tour>>(toursFromRepo);
            return Ok(tours);
        }


        // [HttpGet("{tourId}", Name = "GetTour")]
        //public async Task<IActionResult> GetTour(Guid tourId)
        //{
        //    var tourFromRepo = await repository.GetTour(tourId);

        //    if (tourFromRepo == null)
        //    {
        //        return BadRequest();
        //    }

        //    var tour = this.mapper.Map<Tour>(tourFromRepo);

        //    return Ok(tour);
        // }        

        [HttpGet("{tourId}")]
        //public async Task<IActionResult> GetDefaultTour(Guid tourId, [FromHeader(Name ="Accept")] string acceptHeaderValue )
        public async Task<IActionResult> GetDefaultTour(Guid tourId)
        {
            if (Request.Headers.TryGetValue("Accept",
                out StringValues values))
            {
                Debug.WriteLine($"Accept header(s): {string.Join(",", values)}");
            }

            return await GetSpecificTour<Tour>(tourId);
        }

        [HttpGet("{tourId}", Name = "GetTour")]
        //[Authorize(Policy = "UserMustBeTourManager")]
        [RequestHeaderMatchesMediaType("Accept",
            new[] { "application/vnd.marvin.tour+json" })]
        public async Task<IActionResult> GetTour(Guid tourId)
        {
            return await GetSpecificTour<Tour>(tourId);
        }

        [HttpGet("{tourId}")]
        //[Authorize(Policy = "UserMustBeTourManager")]
        //[Authorize(Policy = "UserMustBeAdministrator")]
        [RequestHeaderMatchesMediaType("Accept",
            new[] { "application/vnd.marvin.tourwithestimatedprofits+json" })]
        public async Task<IActionResult> GetTourWithEstimatedProfits(Guid tourId)
        {
            return await GetSpecificTour<TourWithEstimatedProfits>(tourId);
        }

        [HttpGet("{tourId}")]
        //[Authorize(Policy = "UserMustBeTourManager")]
        [RequestHeaderMatchesMediaType("Accept",
            new[] { "application/vnd.marvin.tourwithshows+json" })]
        public async Task<IActionResult> GetTourWithShows(Guid tourId)
        {
            return await GetSpecificTour<TourWithShows>(tourId, true);
        }

        [HttpGet("{tourId}")]
        //[Authorize(Policy = "UserMustBeTourManager")]
        //[Authorize(Policy = "UserMustBeAdministrator")]
        [RequestHeaderMatchesMediaType("Accept",
            new[] { "application/vnd.marvin.tourwithestimatedprofitsandshows+json" })]
        public async Task<IActionResult> GetTourWithEstimatedProfitsAndShows(Guid tourId)
        {
            return await GetSpecificTour<TourWithEstimatedProfitsAndShows>(tourId, true);
        }



        [HttpPost]
        [RequestHeaderMatchesMediaType("Content-Type",
            new[] { "application/json",
                    "application/vnd.marvin.tourforcreation+json" })]
        public async Task<IActionResult> AddTour([FromBody] TourForCreation tour)
        {
            return await AddSpecificTour(tour);
        }

        [HttpPost]
        //[Authorize(Policy = "UserMustBeAdministrator")]
        [RequestHeaderMatchesMediaType("Content-Type",
            new[] { "application/vnd.marvin.tourwithmanagerforcreation+json" })]
        public async Task<IActionResult> AddTourWithManager(
            [FromBody] TourWithManagerForCreation tour)
        {
            return await AddSpecificTour(tour);
        }

        [HttpPost]
        [RequestHeaderMatchesMediaType("Content-Type",
            new[] { "application/vnd.marvin.tourwithshowsforcreation+json" })]
        public async Task<IActionResult> AddTourWithShows(
            [FromBody] TourWithShowsForCreation tour)
        {
            return await AddSpecificTour(tour);
        }

        [HttpPost]
        //[Authorize(Policy = "UserMustBeAdministrator")]
        [RequestHeaderMatchesMediaType("Content-Type",
            new[] { "application/vnd.marvin.tourwithmanagerandshowsforcreation+json" })]
        public async Task<IActionResult> AddTourWithManagerAndShows(
            [FromBody] TourWithManagerAndShowsForCreation tour)
        {
            return await AddSpecificTour(tour);
        }

        private async Task<IActionResult> GetSpecificTour<T>(Guid tourId,
                bool includeShows = false) where T : class
        {
            var tourFromRepo = await repository.GetTour(tourId, includeShows);

            if (tourFromRepo == null)
            {
                return BadRequest();
            }

            return Ok(this.mapper.Map<T>(tourFromRepo));
        }

        public async Task<IActionResult> AddSpecificTour<T>(T tour) where T : class
        {
            if (tour == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var tourEntity = this.mapper.Map<Entities.Tour>(tour);

            if (tourEntity.ManagerId == Guid.Empty)
            {
                if (!Guid.TryParse(userInfoService.UserId, out Guid userIdAsGuid))
                {
                    return Forbid();
                }

                tourEntity.ManagerId = userIdAsGuid;
            }

            await repository.AddTour(tourEntity);

            if (!await repository.SaveAsync())
            {
                throw new Exception("Adding a tour failed on save.");
            }

            var tourToReturn = this.mapper.Map<Tour>(tourEntity);

            return CreatedAtRoute("GetTour",
                new { tourId = tourToReturn.TourId },
                tourToReturn);
        }

        [HttpPatch("{tourId}")]
        public async Task<IActionResult> PartiallyUpdateTour(Guid tourId,
          [FromBody] JsonPatchDocument<TourForUpdate> jsonPatchDocument)
        {
            if (jsonPatchDocument == null)
            {
                return BadRequest();
            }

            var tourFromRepo = await repository.GetTour(tourId);

            if (tourFromRepo == null)
            {
                return BadRequest();
            }

            var tourToPatch = this.mapper.Map<TourForUpdate>(tourFromRepo);

            jsonPatchDocument.ApplyTo(tourToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            if (!TryValidateModel(tourToPatch))
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            this.mapper.Map(tourToPatch, tourFromRepo);

            await repository.UpdateTour(tourFromRepo);

            if (!await repository.SaveAsync())
            {
                throw new Exception("Updating a tour failed on save.");
            }

            return NoContent();
        }
    }

}

