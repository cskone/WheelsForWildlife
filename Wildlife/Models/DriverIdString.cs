using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wildlife.Models
{
    [Table("dbo.OptedInDrivers")]
    public class DriverIdString
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public string DriverId { get; set; }

        public DriverIdString(string id)
        {
            DriverId = id;
        }
        public DriverIdString()
        {

        }
    }    
}