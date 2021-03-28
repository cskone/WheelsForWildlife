using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Device.Location;
using Wildlife.Models;

[assembly: OwinStartupAttribute(typeof(Wildlife.Startup))]
namespace Wildlife
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesandUsers();
        }

        public void CreateRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Tutorial used for role creation https://www.c-sharpcorner.com/UploadFile/asmabegam/Asp-Net-mvc-5-security-and-creating-user-role/
            // Create Admin role and Super User
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.UserName = "Super.User@gmail.com";
                user.Email = "Super.User@gmail.com";
                user.DriverLocation = new CivicAddress();
                string userPWD = "SuperUser1!";

                var chkUser = UserManager.Create(user, userPWD);
                
                if (chkUser.Succeeded) 
                { 
                    UserManager.AddToRole(user.Id, "Admin"); 
                }

            }
            // Creates Driver Role
            if(!roleManager.RoleExists("Driver"))
            {
                var role = new IdentityRole();
                role.Name = "Driver";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Inactive"))
            {
                var role = new IdentityRole();
                role.Name = "Inactive";
                roleManager.Create(role);
            }
        }
    }
}
