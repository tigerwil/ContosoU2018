using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ContosoU2018.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ContosoU2018
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    //var context = services.GetRequiredService<SchoolContext>();
                    //Creating a more complex data model - comment out line below
                    //This DbInitializer is for the School Data
                    //DbInitializer.Initialize(context);

                    //mwilliams:  Identity Framework - initial seed
                    //Note:  using the Secrets Manager tool for initial pwd
                    //       will need a reference to the Microsoft.Extensions.Configuration
                    //This DbInitializer is for user and roles within the Identity Framework
                    var config = host.Services.GetRequiredService<IConfiguration>();

                    var testUserPw = config["SeedUserPW"];//this is the password

                    //Call our Initializer
                    IdentityDbInitializer.Initialize(services, testUserPw).Wait();
                    


                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            host.Run();
            //old code
            //BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
