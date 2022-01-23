using Data;
using Entity;
using Microsoft.EntityFrameworkCore;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class BookmarkService : IBookmarkService
    {
        private ReadLaterDataContext _ReadLaterDataContext;

        public BookmarkService(ReadLaterDataContext readLaterDataContext)
        {
            _ReadLaterDataContext = readLaterDataContext;
        }

        public Bookmark CreateBookmark(Bookmark bookmark)
        {
            if (string.IsNullOrEmpty(bookmark.URL))
                throw new Exception("URL required");
            else if (string.IsNullOrEmpty(bookmark.ShortDescription))
                throw new Exception("Short description required");

             _ReadLaterDataContext.Add(bookmark);
            _ReadLaterDataContext.SaveChanges();
            return bookmark;
        }

        public Task DeleteBookmark(Bookmark entity)
        {
            _ReadLaterDataContext.Bookmark.Remove(entity);
            return _ReadLaterDataContext.SaveChangesAsync();
    
        }

        public Task<List<Bookmark>> GetAllBookmarks()
        {
            return _ReadLaterDataContext.Bookmark.ToListAsync();
        }

        public Task<Bookmark> GetBookmarkById(int id)
        {
            return _ReadLaterDataContext.Bookmark.FirstOrDefaultAsync(x => x.ID == id);
        }

        public Task<List<Bookmark>> GetBookmarkByUserId(string userId)
        {
            return _ReadLaterDataContext.Bookmark.Include(x => x.Category).Where(x => x.UserId == userId).ToListAsync();
        }

        public Task<List<Bookmark>> GetBookmarksForCategory(int categoryId)
        {
            return _ReadLaterDataContext.Bookmark.Where(x => x.CategoryId == categoryId).Include(x => x.Category).ToListAsync();
        }

        public Task UpdateBookmark(string url, string description, Bookmark bookmark)
        {
            bookmark.ShortDescription = description;
            bookmark.URL = url;
            _ReadLaterDataContext.Update(bookmark);
            return _ReadLaterDataContext.SaveChangesAsync();
        }
    }
}
