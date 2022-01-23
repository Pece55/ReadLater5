using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadLater5.Models
{
    public class ClientBookmarkModel
    {
        public int ID { get; set; }

        public string URL { get; set; }

        public string ShortDescription { get; set; }

        public int CategoryId { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
