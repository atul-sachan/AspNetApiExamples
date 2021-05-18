using AutoMapper;
using BusinessApplication.TourManagement.Api.Dtos;
using BusinessApplication.TourManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessApplication.TourManagement.Api.Controllers
{
    [Route("api/tours/{tourId}/shows")]
    [ApiController]
    public class ShowsController: ControllerBase
    {
        private readonly ITourManagementRepository repository;
        private readonly IMapper mapper;

        public ShowsController(ITourManagementRepository repository, IMapper mapper)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IActionResult> GetShows(Guid tourId)
        {
            //var tourFromRepo = await this.repository.GetTour(tourId, true);
            if(!(await this.repository.TourExists(tourId)))
            {
                return NotFound();
            }
            var showsFromRepo = await this.repository.GetShows(tourId);
            var shows = this.mapper.Map<IEnumerable<Show>>(showsFromRepo);
            return Ok(shows);

        }
    }
}
