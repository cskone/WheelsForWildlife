using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
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

    public class EditUserInfoViewModel
    {
        [Required]
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

        [Display(Name = "Old Driver Location (Zipcode)")]
        public int OldDriverLocation { get; set; }

        [Required]
        [Display(Name = "Driver Location (Zipcode)")]
        public int NewDriverLocation { get; set; }

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