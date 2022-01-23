using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReadLater5.Models
{
    public class BookmarkModel
    {
        public int ID { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string URL { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        public string UserId { get; set; }
        public List<CategoryModel> Categories { get; set; }
        public string NewCategoryName { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreateDate { get; }
    }
}
