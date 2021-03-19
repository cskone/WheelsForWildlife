using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Device.Location;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Wildlife.Models
{

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public int UserID { get; set; } // this is totally empty atm
        
        // redundant can store in base user
        // public string PhoneNumber { get; set; } // Need to figure out how to inherit from Identity table

        // Make and model of car to allow individuals at pickup/dropoff locations to identify the driver
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }


        public int DriveCount { get; set; }     // Total number of drives
        public double DriveHours { get; set; }  // Total hours spent on drives based on function of Drive.DriveStart and Drive.DriveEnd
        public int AnimalCount { get; set; }    // Number of animals transported
        // https://docs.microsoft.com/en-us/dotnet/api/system.device.location?view=netframework-4.8
        public CivicAddress DriverLocation { get; set; }
//        public int DriverLication { get; set; }

        public virtual ICollection<Drive> Drives { get; set; }
        public virtual ICollection<Availability> Availabilities { get; set; }


        // not sure this is how we should store this
        [DataType(DataType.DateTime)]
        public DateTime[][] AvailabilityRange { get; set; } // First index is a continuous number of start-stop DateTimes. Second index will be size 2 containing [startDateTime, stopDateTime]
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here


            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        //public DbSet<Driver> Drivers { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}