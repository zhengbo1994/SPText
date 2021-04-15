using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SPCoreText.Models
{
    public class Movie
    {
        public int ID { get; set; }
        public string Title { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreateDatetime { get; set; }
        public decimal Price { get; set; }
    }
}
