using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bakery_Shop.Models
{
    public class CustomOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomOrderID { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
        public int numberOfServings { get; set; }
        public string Flavour { get; set; }
        public string Icing { get; set; }
        public string Filling { get; set; }
        public string CakeType { get; set; }
        public double TotalAmount { get; set; }
        public string SketchImage { get; set; }
        public string OrderDate { get; set; }
        public bool IsPaymentCompleted { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsReady { get; set; }
        public int? AssignedDriver { get; set; }
        public string OrderType { get; set; }
        public bool isPreparing { get; set; }
        public bool isArriving { get; set; }
        public bool isReceived { get; set; }
        public bool isDelivered { get; set; }
        public DateTime? Deliverydate { get; set; }
        public string TrackingNumber { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        public CustomOrder()
        {
            this.OrderType = "Custom";
            this.Status = "Pending..";
            this.OrderDate = DateTime.Now.ToShortDateString();
            this.IsPaymentCompleted = false;
            this.IsAccepted = false;
            this.IsReady = false;
            this.isPreparing = false;
            this.isArriving = false;
            this.isReceived = true;
            this.isDelivered = false;
        }
    }
}