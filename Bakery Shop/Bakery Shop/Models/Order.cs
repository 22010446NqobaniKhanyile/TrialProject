using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bakery_Shop.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OrderNo { get; set; }
        public string ZipCode { get; set; }
        public string CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string OrderDate { get; set; }
        public string Address { get; set; }
        public double Total { get; set; }
        public string Status { get; set; }
        public string Cancelationfile { get; set; }
        public bool isPayed { get; set; }
        public bool isPreparing { get; set; }
        public bool isArriving { get; set; }
        public bool isReceived { get; set; }
        public bool isDelivered { get; set; }
        public bool IsDeliveryConfirmed { get; set; }
        public DateTime? Deliverydate { get; set; }
        public int? AssignedDriverId { get; set; }
        public string TrackingNumber { get; set; }
        public bool isCancelled { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
        public Order()
        {
            this.OrderDate = DateTime.Now.ToShortDateString();
            this.isPayed = false;
            this.isArriving = false;
            this.isReceived = true;
            this.isPreparing = false;
            this.isDelivered = false;
            this.Status = "Received";
            this.IsDeliveryConfirmed = false;
        }
    }
}


