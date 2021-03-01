using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Device.Location;
using System.Linq;
using System.Web;
namespace Wildlife.Models
{
    [Table("dbo.Drives")]
    public class Drive
    {
        [Key]
        [Required]
        [Display(Name = "Drive Id")]
        public int DriveId { get; set; }
        [Required]
        [Display(Name = "Drive Name")]
        public string DriveName { get; set; }
        //[Required] - this restructures the entire database and breaks everything whoever did this in microsoft is mean[Display(Name = "Drive Starting Location")]
        public CivicAddress StartLocation { get; set; }
        //[Required] - this restructures the entire database and breaks everything whoever did this in microsoft is mean
        [Display(Name = "Drive Ending Location")]
        public CivicAddress EndLocation { get; set; }
        [Display(Name = "Drive Details")]
        public string ExtraDetails { get; set; }
        [Display(Name = "Driver Email")]
        public string DriverId { get; set; }

        public Drive(string driveName, CivicAddress startLocation, CivicAddress endLocation, string driverId)
        {
            DriveName = driveName;
            StartLocation = startLocation;
            EndLocation = endLocation;  
            DriverId = driverId;
        }

        public Drive(string driveName, string extraDetails, CivicAddress startLocation, CivicAddress endLocation, string driverId)
        {
            DriveName = driveName;
            ExtraDetails = extraDetails;
            StartLocation = startLocation;
            EndLocation = endLocation;
            DriverId = driverId;
        }

        public Drive()
        {

        }
    }

}