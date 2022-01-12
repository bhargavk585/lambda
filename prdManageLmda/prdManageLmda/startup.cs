using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace prdManageLmda
{
    //class startup
    //{
    //}
    public sealed class Startup

    {
        public Startup()

        {

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">ServiceCollection object.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }



        /// <summary>

        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        /// </summary>

        /// <param name="applicationBuilder">ASP.Net Application Builder object.</param>

        public void Configure(IApplicationBuilder applicationBuilder)

        {

            try

            {

                applicationBuilder

                    .UseHsts()

                    .UseRouting()

                    .UseEndpoints(endpoints => { endpoints.MapControllers(); });

            }

            catch (Exception exception)

            {

                throw exception;

            }

        }

    }
}
