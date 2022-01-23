using AutoMapper;
using Entity;
using ReadLater5.Models;

namespace ReadLater5.DTO.AutoMapper
{
    public class MapProfile 
    {
        internal static User RegisterModelToUser(RegisterModel m)
        {
            return new User()
            {
                FirstName = m.FirstName,
                LastName = m.LastName,
                UserName = m.Username,
                PasswordHash = m.Password
            };
        }

        internal static BookmarkModel DTOBookmarkForModel(Bookmark m)
        {
            return new BookmarkModel
            {
                ID = m.ID,
                CategoryId = (int)m.CategoryId,
                ShortDescription = m.ShortDescription,
                URL = m.URL,
                CategoryName = m.Category == null ? string.Empty : m.Category.Name
            };
        }

        internal static CategoryModel DTOCategoryToModel(Category c)
        {
            return new CategoryModel
            {
                Id = c.ID,
                Name = c.Name
            };
        }
        internal static Bookmark ModelToBookmark(BookmarkModel b)
        {
            return new Bookmark
            {
                URL = b.URL,
                CategoryId = b.CategoryId,
                ShortDescription = b.ShortDescription,
                UserId = b.UserId
            };
        }

        internal static ClientBookmarkModel BookmarkToCLModel(Bookmark x)
        {
            return new ClientBookmarkModel
            {
                ID = x.ID,
                URL = x.URL,
                ShortDescription = x.ShortDescription,
                CreateDate = x.CreateDate,
                CategoryId = x.CategoryId
            };
        }
    }
}
