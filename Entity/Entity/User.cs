using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Entity
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual List<Bookmark> Bookmarks { get; set; }
        public virtual List<Category> Categories { get; set; }
    }
}
