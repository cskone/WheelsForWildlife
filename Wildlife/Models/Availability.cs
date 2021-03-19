using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Wildlife.Models
{
    [Table("dbo.Availability")]
    public class Availability
    {
        [Key]
        [Required]
        [Display(Name = "Slot Id")]
        public int SlotId { get; set; }
        [Required]
        [Display(Name = "Driver Email")]
        public string DriverId { get; set; }

        [Display(Name = "Day Of The Week")]
        public DayOfWeek Dayoftheweek { get; set; }

        [Display(Name = "Start Time")]
        public double StartTime { get; set; }

        [Display(Name = "End Time")]
        public double EndTime { get; set; }

        public Availability(string driverid, DayOfWeek dayoftheweek, double starttime, double endtime)
        {
            DriverId = driverid;
            Dayoftheweek = dayoftheweek;
            StartTime = starttime;
            EndTime = endtime;
        }

        public Availability()
        {

        }

        public void Update(double starttime, double endtime)
        {
            StartTime = starttime;
            EndTime = endtime;
        }
    }



}