using AutoMapper;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiDemo.Models;
using WebApiDemo.Services;

namespace WebApiDemo.Controllers
{
    [Route("api/authors/{authorId}/courses")]
    [ApiController]
    // [ResponseCache(CacheProfileName = "240SecondsCacheProfile")]
    [HttpCacheExpiration(CacheLocation = CacheLocation.Public)]
    [HttpCacheValidation(MustRevalidate = true)]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseLibraryRepository courseLibraryRepository;
        private readonly IMapper mapper;

        public CoursesController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            this.courseLibraryRepository = courseLibraryRepository ?? throw new ArgumentNullException(nameof(courseLibraryRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public ActionResult<IEnumerable<CourseDto>> GetCoursesFromAuthor(Guid authorId)
        {
            if (!this.courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courses = this.courseLibraryRepository.GetCourses(authorId);
            return Ok(this.mapper.Map<IEnumerable<CourseDto>>(courses));
        }

        [HttpGet("{courseId}", Name = "GetCourseForAuthor")]
        // [ResponseCache(Duration = 120)]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 1000)]
        [HttpCacheValidation(MustRevalidate = false)]
        public ActionResult<CourseDto> GetCourseForAuthor(Guid authorId, Guid courseId)
        {
            if (!this.courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courseForAuthorFromRepo = this.courseLibraryRepository.GetCourse(authorId, courseId);
            if (courseForAuthorFromRepo == null)
            {
                return NotFound();
            }

            return Ok(this.mapper.Map<CourseDto>(courseForAuthorFromRepo));
        }

        [HttpPost(Name = "CreateCourseForAuthor")]
        public ActionResult<CourseDto> CreateCourseForAuthor(Guid authorId, CourseForCreationDto course)
        {
            if (!this.courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courseEntity = this.mapper.Map<Entities.Course>(course);
            this.courseLibraryRepository.AddCourse(authorId, courseEntity);
            this.courseLibraryRepository.Save();

            var courseToReturn = this.mapper.Map<CourseDto>(courseEntity);
            return CreatedAtRoute("GetCourseForAuthor", new { authorId = authorId, courseId = courseToReturn.Id }, courseToReturn);
        }

        [HttpPut("{courseId}")]
        public IActionResult UpdateCourseForAuthor(Guid authorId, Guid courseId, CourseForUpdateDto course)
        {
            if (!this.courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courseForAuthorFromRepo = this.courseLibraryRepository.GetCourse(authorId, courseId);
            if (courseForAuthorFromRepo == null)
            {
                var courseToAdd = this.mapper.Map<Entities.Course>(course);
                courseToAdd.Id = courseId;

                this.courseLibraryRepository.AddCourse(authorId, courseToAdd);
                this.courseLibraryRepository.Save();

                var courseToReturn = this.mapper.Map<CourseDto>(courseToAdd);
                return CreatedAtRoute("GetCourseForAuthor", new { authorId, courseId = courseToReturn.Id }, courseToReturn);
            }

            // map the entity to a CourseForUpdateDto
            // apply the updated field values to that dto
            // map the CourseForUpdateDto back to an entity
            this.mapper.Map(course, courseForAuthorFromRepo);
            this.courseLibraryRepository.UpdateCourse(courseForAuthorFromRepo);
            this.courseLibraryRepository.Save();
            
            return NoContent();
        }

        [HttpPatch("{courseId}")]
        public ActionResult PartiallyUpdateCourseForAuthor(Guid authorId, Guid courseId, JsonPatchDocument<CourseForUpdateDto> patchDocument)
        {
            if (!this.courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courseForAuthorFromRepo = this.courseLibraryRepository.GetCourse(authorId, courseId);
            if (courseForAuthorFromRepo == null)
            {
                var courseDto = new CourseForUpdateDto();
                patchDocument.ApplyTo(courseDto, ModelState);

                if (!TryValidateModel(courseDto))
                {
                    return ValidationProblem(ModelState);
                }

                var courseToAdd = this.mapper.Map<Entities.Course>(courseDto);
                courseToAdd.Id = courseId;

                this.courseLibraryRepository.AddCourse(authorId, courseToAdd);
                this.courseLibraryRepository.Save();

                var courseToReturn = this.mapper.Map<CourseDto>(courseToAdd);

                return CreatedAtRoute("GetCourseForAuthor", new { authorId, courseId = courseToReturn.Id }, courseToReturn);
            }

            var courseToPatch = this.mapper.Map<CourseForUpdateDto>(courseForAuthorFromRepo);
            // add validation
            patchDocument.ApplyTo(courseToPatch, ModelState);
            if (!TryValidateModel(courseToPatch))
            {
                return ValidationProblem(ModelState);
            }

            this.mapper.Map(courseToPatch, courseForAuthorFromRepo);
            this.courseLibraryRepository.UpdateCourse(courseForAuthorFromRepo);
            this.courseLibraryRepository.Save();

            return NoContent();
        }

        [HttpDelete("{courseId}")]
        public ActionResult DeleteCourseForAuthor(Guid authorId, Guid courseId)
        {
            if (!this.courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courseForAuthorFromRepo = this.courseLibraryRepository.GetCourse(authorId, courseId);
            if (courseForAuthorFromRepo == null)
            {
                return NotFound();
            }

            this.courseLibraryRepository.DeleteCourse(courseForAuthorFromRepo);
            this.courseLibraryRepository.Save();

            return NoContent();
        }

        public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}
