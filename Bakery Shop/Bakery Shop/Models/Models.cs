using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bakery_Shop.Models
{
    public class Models
    {
    }

    public class Ordercancelation
    {
       
            [Key]
            public int Id { get; set; }

            public int OrderId { get; set; }

            public string Reason { get; set; }

            public DateTime Date { get; set; }

            public string Status { get; set; }

            public int Statusnum { get; set; }

            public string Cancelationfile { get; set; }

            [ForeignKey("OrderId")]
            public virtual Order Order { get; set; }
    }
}