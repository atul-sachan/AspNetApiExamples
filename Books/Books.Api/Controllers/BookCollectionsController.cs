using AutoMapper;
using Books.Api.Filters;
using Books.Api.ModelBinder;
using Books.Api.Models;
using Books.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [BooksResultFilter]
    public class BookCollectionsController : ControllerBase
    {
        private readonly IBookRepository _booksRepository;
        private readonly IMapper _mapper;

        public BookCollectionsController(IBookRepository booksRepository, IMapper mapper)
        {
            _booksRepository = booksRepository ?? throw new ArgumentNullException(nameof(booksRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // api/bookcollections/(id1,id2,...)
        [HttpGet("({bookIds})", Name = "GetBookCollection")]
        public async Task<IActionResult> GetBookCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> guids)
        {
            var bookEntities = await _booksRepository.GetBooksAsync(guids);
            if(guids.Count() != bookEntities.Count())
            {
                return NotFound();
            }
            return Ok(bookEntities);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(IEnumerable<BookForCreation> bookForCreations)
        {
            var bookEntities = _mapper.Map<IEnumerable<Entities.Book>>(bookForCreations);
            foreach (var bookEntity in bookEntities)
            {
                _booksRepository.AddBook(bookEntity);
            }
            await _booksRepository.SaveChangesAsync();
            var booksToReturn = await _booksRepository.GetBooksAsync(bookEntities.Select(b => b.Id).ToList());

            var bookIds = string.Join(",", booksToReturn.Select(b => b.Id));
            return CreatedAtRoute("GetBookCollection", new { bookIds }, booksToReturn);

        }
    }
}
