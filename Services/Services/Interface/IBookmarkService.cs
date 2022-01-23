using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IBookmarkService
    {
        Bookmark CreateBookmark(Bookmark bookmark);
        Task<List<Bookmark>> GetBookmarksForCategory(int categoryId);
        Task<Bookmark> GetBookmarkById(int id);
        Task UpdateBookmark(string url, string description, Bookmark bookmark);
        Task DeleteBookmark(Bookmark entity);
        Task<List<Bookmark>> GetAllBookmarks();
        Task<List<Bookmark>> GetBookmarkByUserId(string userId);
    }
}
