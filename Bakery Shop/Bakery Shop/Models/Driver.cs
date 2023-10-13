using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bakery_Shop.Models
{
    public class Driver
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DriverId { get; set; }
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string UserId { get; set; }
        public bool IsAvailable { get; set; }
        public int TotalDeliveries { get; set; }
        public string Phone { get; set; }
        public Driver()
        {
            this.IsAvailable = true;
            this.TotalDeliveries = 0;
        }
    }
}