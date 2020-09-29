using AutoMapper;
using LibraryApi.Domain;
using LibraryApi.Models.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Profiles
{
    public class BooksProfile:Profile
    {
        public BooksProfile()
        {
            CreateMap<Book, GetBooksResponseItem>();
            CreateMap<Book, GetBookDetailsResponse>();
        }
    }
}
