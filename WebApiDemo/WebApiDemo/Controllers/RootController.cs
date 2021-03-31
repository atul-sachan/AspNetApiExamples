using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiDemo.Models;

namespace WebApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RootController : ControllerBase
    {
        public IActionResult GetRoot()
        {
            // create links for root
            var links = new List<LinkDto>();
            links.Add(new LinkDto(Url.Link("GetRoot", new { }), "self", "GET"));
            links.Add(new LinkDto(Url.Link("GetAuthors", new { }), "authors", "GET"));
            links.Add(new LinkDto(Url.Link("CreateAuthor", new { }), "create_author", "POST"));
            return Ok(links);

        }
    }
}
