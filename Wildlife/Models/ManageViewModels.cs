using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Device.Location;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Owin.Security;

namespace Wildlife.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
        public ApplicationUser User {get; set;}
    }

    public class DriveIndexViewModel
    {
        public List<Drive> drives { get; set; }
        public ApplicationUser User { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class UserInfoViewModel
    {
        public ApplicationUser User { get; set; }
    }

    public class EditAdminUserInfoViewModel
    {
        public bool IsSuperUser { get; set; }

        [Display(Name = "Old User Role")]
        public string OldUserRole { get; set; }

        [Required]
        [Display(Name = "User Role")]
        public string NewUserRole { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Old Email")]
        public string OldEmail { get; set; }

        [Required]
        [StringLength(255)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string NewEmail { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Old UserName")]
        public string OldUserName { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        [Display(Name = "UserName")]
        public string NewUserName { get; set; }

    }
    public class EditUserInfoViewModel
    {
        [Display(Name = "Old First Name")]
        public string OldFirstName { get; set; }
        [Display(Name = "First Name")]
        public string NewFirstName { get; set; }

        [Display(Name = "Old Last Name")]
        public string OldLastName { get; set; }
        [Display(Name = "Last Name")]
        public string NewLastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Old Email")]
        public string OldEmail { get; set; }

        [Required]
        [StringLength(255)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string NewEmail { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Old UserName")]
        public string OldUserName { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        [Display(Name = "UserName")]
        public string NewUserName { get; set; }

        [Phone]
        [Display(Name = "Old Phone Number")]
        public string OldPhoneNumber { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string NewPhoneNumber { get; set; }

        [Display(Name = "Old Vehicle Model")]
        public string OldVehicleModel { get; set; }

        [Required]
        [Display(Name = "Vehicle Model")]
        public string NewVehicleModel { get; set; }


        [Display(Name = "Old Vehicle Make")]
        public string OldVehicleMake { get; set; }

        [Required]
        [Display(Name = "Vehicle Make")]
        public string NewVehicleMake { get; set; }

        [Required]
        [Display(Name = "Address Line 1")]
        public string NewAddressLine1 { get; set; }
        [Display(Name = "Address Line 2")]
        public string NewAddressLine2 { get; set; }
        [Required]
        [Display(Name = "City")]
        public string NewCity { get; set; }
        [Required]
        [Display(Name = "State")]
        public string NewStateProvince { get; set; }
        [Required]
        [Display(Name = "Zip Code")]
        public string NewPostalCode { get; set; }

        [Display(Name = "Calendar Info")]
        public List<Availability> Availabilities { get; set; }
    }
    public class DriveInfoViewModel
    {
        [Required]
        public int DriveId { get; set; }
        [Required]
        [Display(Name = "Drive Title")]
        public string DriveName { get; set; }

        [Required]
        [Display(Name = "Address Line 1")]
        public string StartAddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string StartAddressLine2 { get; set; }

        [Required]
        [Display(Name = "City")]
        public string StartCity { get; set; }


        [Required]
        [Display(Name = "Country")]
        public string StartCountryRegion { get; set; }


        [Required]
        [Display(Name = "Postal Code")]
        public string StartPostalCode { get; set; }


        [Required]
        [Display(Name = "State")]
        public string StartStateProvince { get; set; }

        [Required]
        [Display(Name = "Address Line 1")]
        public string EndAddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string EndAddressLine2 { get; set; }

        [Required]
        [Display(Name = "City")]
        public string EndCity { get; set; }


        [Required]
        [Display(Name = "Country")]
        public string EndCountryRegion { get; set; }


        [Required]
        [Display(Name = "Postal Code")]
        public string EndPostalCode { get; set; }


        [Required]
        [Display(Name = "State")]
        public string EndStateProvince { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Drive Details")]
        public string ExtraDetails { get; set; }

        [Display(Name = "Driver Email")]
        public string DriverId { get; set; }

        [Display(Name = "Distance")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public double DriveDistance { get; set; }

        [Display(Name = "Duration")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal DriveDuration { get; set; }

        [Display(Name = "Distance From You")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public double UserDistance { get; set; }

        [Display(Name = "Travel Time to Drive Pickup")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal UserDuration { get; set; }

        public string ImgSrc { get; set; }

        public bool DriveDone { get; set; }
        public System.Web.Mvc.SelectList OptedInDrivers { get; set; }

        public string SelectedDriver { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}