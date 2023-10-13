using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bakery_Shop.Models
{
    public class Deco
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Package description")]
        public string Packagedescription { get; set; }
        [Display(Name = "Package name")]
        public string Package_name { get; set; }
        [Display(Name = "Package price")]
        public int Package_price { get; set; }

        [Display(Name = "Package picture")]

        public string Picname { get; set; }

        [Display(Name = "Package theme color")]

        public string Theme_color { get; set; }

        public int Statusnum { get; set; }

        public int Quantity { get; set; }

    }


    public class Decopiece
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Deco piece name")]


        public string Piecename { get; set; }
        [Display(Name = "Piece image")]


        public string Pieceimage { get; set; }
        [Display(Name = "Piece rental price")]

        public int Pieceprice { get; set; }

        public int Decoid { get; set; }


        [ForeignKey("Decoid")]

        public virtual Deco Deco { get; set; }
    }


    public class Decobooking
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "User email")]


        public string Useremail { get; set; }

        public DateTime Date { get; set; }
        [Display(Name = "Booking date")]
        public DateTime Bookingdate { get; set; }

        [Display(Name = "Number of guests")]

        public int Numberofguest { get; set; }
        [Display(Name = "Is delivery requested")]

        public bool Isdeliveryrequested { get; set; }

        public string Address { get; set; }

        public string Status { get; set; }

        public int Statusnum { get; set; }

        public int Packageid { get; set; }

        [Display(Name = "Booking fee")]

        public int Bookingfee { get; set; }
        [Display(Name = "Return date")]

        public DateTime Returndate { get; set; }

        public string Refundslip { get; set; }

        [ForeignKey("Packageid")]

        public virtual Deco Deco { get; set; }

    }


    public class Orderpiece
    {
        [Key]
        public int Id { get; set; }

        public int Bookingid { get; set; }

        public int Pieceid { get; set; }

        public int Booked { get; set; }

    }

    public class Cancelation
    {
        [Key]
        public int Id { get; set; }

        public int Bookingid { get; set; }

        public string Reason { get; set; }

        public DateTime Date { get; set; }

        public string Status { get; set; }

        public int Statusnum { get; set; }

        public string Cancelationfile { get; set; }

        [ForeignKey("Bookingid")]
        public virtual Decobooking Decobooking { get; set; }
    }


    public class Delivery
    {
        [Key]
        public int Id { get; set; }

        public string Status { get; set; }

        public int Statusnum { get; set; }

        public int OrderId { get; set; }

        public DateTime Date { get; set; }

        public string Address { get; set; }

        public string Driveremail { get; set; }
    }
}