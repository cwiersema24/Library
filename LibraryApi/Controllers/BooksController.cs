using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryApi.Domain;
using LibraryApi.Models.Books;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class BooksController:ControllerBase
    {
        private LibraryDataContext _context;
        private IMapper _mapper;
        private MapperConfiguration _mapperConfig;

        public BooksController(LibraryDataContext context, IMapper mapper, MapperConfiguration mapperConfig)
        {
            _context = context;
            _mapper = mapper;
            _mapperConfig = mapperConfig;
        }

        [HttpGet("books")]
        [Produces("application/json")]
        public async Task<ActionResult<GetBooksResponse>> GetBooks()
        {
            var response = new GetBooksResponse();
            var books = await BooksInInventory()
                .ProjectTo<GetBooksResponseItem>(_mapperConfig)
                .ToListAsync();
            response.Data = books;
            return Ok(response);
        }
        /// <summary>
        /// Gives you a book for a specific Id
        /// </summary>
        /// <param name="bookId">The id of the book</param>
        /// <returns>Either the book details or a 404</returns>
        [HttpGet("books/{bookId:int}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetBookDetailsResponse>> GetBookById([FromRoute] int bookId)
        {
            var book = await BooksInInventory().Where(b => b.Id == bookId)
                .ProjectTo<GetBookDetailsResponse>(_mapperConfig)
                .SingleOrDefaultAsync();
            if (book == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(book);
            }
        }

        private IQueryable<Book> BooksInInventory()
        {
            return _context.Books.Where(b => b.IsInInventory == true);
        }
    }


}
