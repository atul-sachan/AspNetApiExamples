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
    public class BandsController : ControllerBase
    {
        private readonly ITourManagementRepository repository;
        private readonly IMapper mapper;

        public BandsController(ITourManagementRepository repository, IMapper mapper)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IActionResult> GetBands()
        {
            var bandsFromRepo = await this.repository.GetBands();
            var bands = this.mapper.Map<IEnumerable<Band>>(bandsFromRepo);
            return Ok(bands);
        }
    }
}
