using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.Hosting;

namespace prdManageLmda
{
    public sealed class Program

    {

        public static void Main(string[] args)

        {

            var host = CreateHostBuilder(args).Build();

            host.Run();

        }
        /// <summary>

        /// Creating the Host builder for ASP.Net Core API.

        /// </summary>

        /// <param name="args">Parameters to the builder.</param> 

        /// <returns>Returns the HostBuilder object with startup file.</returns>

        public static IHostBuilder CreateHostBuilder(string[] args) =>

            Host.CreateDefaultBuilder(args)

                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

    
}
}
