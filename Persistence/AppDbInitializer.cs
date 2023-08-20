using TechBlogApp.Domain.Models;
using TechBlogApp.Domain.Static;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;


namespace TechBlogApp.Persistence
{
    public class AppDbInitializer
    {

        //        ////RunWays
        //        //if (!context.Articles.Any())
        //        //{
        //        //    context.Articles.AddRange(new List<Article>()
        //        //    {
        //        //        new Article()
        //        //        {
        //        //              //Id
        //        //              ArticleId = Guid.NewGuid(),
                             
        //        //        },
        //        //        //new Article()
        //        //        //{
        //        //        //      //Id
        //        //        //      ArticleId = Guid.NewGuid(),

        //        //        //},


        //        //    });
        //        //    context.SaveChanges();
        //        //}


        //    }

        //}



        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles Section
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.Creator))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Creator));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //User section
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var adminUser = await userManager.FindByEmailAsync("admin@gmail.com");
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        Fullname = "Application Admin",
                        UserName = "app-admin",
                        Email = "admin@gmail.com",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "Astrit123@.");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);



                }
                //Creator section
                var creatorUser = await userManager.FindByEmailAsync("creator@gmail.com");
                if (creatorUser == null)
                {
                    var newCreatorUser = new AppUser()
                    {
                        Fullname = "Application Creator",
                        UserName = "app-creator",
                        Email = "creator@gmail.com",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newCreatorUser, "Astrit123@.");
                    await userManager.AddToRoleAsync(newCreatorUser, UserRoles.Creator);



                }


                var appUser = await userManager.FindByEmailAsync("user@gmail.com");
                if (appUser == null)
                {
                    var newAppUser = new AppUser()
                    {
                        Fullname = "Application User",
                        UserName = "app-user",
                        Email = "user@gmail.com",
                        EmailConfirmed = true,
                        
                    };
                    await userManager.CreateAsync(newAppUser, "Astrit123@.");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    
    
    
    
    
    }
}
    

          

