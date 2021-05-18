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
    public class ManagersController : ControllerBase
    {
        private readonly ITourManagementRepository repository;
        private readonly IMapper mapper;

        public ManagersController(ITourManagementRepository repository, IMapper mapper)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IActionResult> GetManagers()
        {
            var managersFromRepo = await this.repository.GetManagers();
            var managers = this.mapper.Map<IEnumerable<Manager>>(managersFromRepo);
            return Ok(managers);
        }
    }
}
