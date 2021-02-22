using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wildlife.Models
{
    public class Drive
    {
        [Key]
        [Required]
        [Display(Name = "Drive Id")]
        public int DriveId { get; set; }
        [Required]
        [Display(Name = "Drive Title")]
        public string DriveName { get; set; }
        [Required]
        [Display(Name = "Drive Starting Location")]
        public int StartLocation { get; set; }
        [Required]
        [Display(Name = "Drive Ending Location")]
        public int EndLocation { get; set; }
        [Display(Name = "Drive Details")]
        public string ExtraDetails { get; set; }
        [Display(Name = "Driver Email")]
        public string DriverId { get; set; }

        public Drive(string driveName, int startLocation, int endLocation, string driverId)
        {
            DriveName = driveName;
            StartLocation = startLocation;
            EndLocation = endLocation;
            DriverId = driverId;
        }
        public Drive()
        {

        }
    
    }

}