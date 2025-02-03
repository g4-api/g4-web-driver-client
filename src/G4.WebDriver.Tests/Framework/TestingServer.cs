using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace G4.WebDriver.Tests.Framework
{
    [ApiController]
    [Route("api/v1/g4")]
    public class TestController(ILogger<TestController> logger) : ControllerBase
    {
        // Logger instance for logging information within this controller.
        private readonly ILogger<TestController> _logger = logger;

        // GET: api/v1/g4/ping
        [HttpGet, Route("ping")]
        public IActionResult Ping()
        {
            // Retrieve the host and port from the request.
            var host = Request.Host.Host;
            var port = Request.Host.Port;

            // Log the host and port information.
            _logger.LogInformation("Received ping request from host {Host} on port {Port}.", host, port);

            // Return a simple "pong" response.
            return Ok("pong");
        }

        // GET: api/v1/g4/wait
        [HttpGet, Route("wait")]
        public IActionResult LongOperation()
        {
            // Wait for 15 seconds to simulate a long operation.
            Thread.Sleep(timeout: TimeSpan.FromSeconds(15));

            // Log the completion of the long operation.
            return Ok("Done");
        }
    }

    /// <summary>
    /// Represents the startup configuration for the application.
    /// </summary>
    /// <param name="configuration">The configuration for the application.</param>
    public class Startup(IConfiguration configuration)
    {
        /// <summary>
        /// Gets the configuration for the application.
        /// </summary>
        public IConfiguration Configuration { get; } = configuration;

        /// <summary>
        /// Configures services for the application.
        /// </summary>
        /// <param name="services">The collection of services to configure.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure JSON options for controllers
            services.AddControllers().AddJsonOptions(i =>
            {
                i.JsonSerializerOptions.WriteIndented = true;
                i.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                i.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                i.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                i.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });

            // Enable XML serialization for controllers
            services.AddControllers().AddXmlSerializerFormatters();

            // Add Swagger generation services to the service collection.
            services.AddSwaggerGen();
        }

        /// <summary>
        /// Configures the application.
        /// </summary>
        /// <param name="app">The application builder.</param>
        public void Configure(IApplicationBuilder app)
        {
            // Enable Developer Exception Page for detailed error information during development
            app.UseDeveloperExceptionPage();

            // Enable routing
            app.UseRouting();

            // Enable authorization
            app.UseAuthorization();

            // Map controllers for handling endpoints
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI();

            // Configure Swagger UI
            app.UseSwaggerUI(options =>
            {
                // Set the Swagger JSON endpoint and API version
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");

                // Set the route prefix for the Swagger UI
                options.RoutePrefix = string.Empty;
            });

            // Serve static files from the "Pages" directory at the "/test" endpoint
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Environment.CurrentDirectory, "Pages")),
                RequestPath = "/test/static"
            });
        }
    }

    /// <summary>
    /// Represents a simple web server for hosting a web application.
    /// </summary>
    public static class WebServer
    {
        // Static variable to hold the web host instance.
        private static IWebHost s_webHost = NewWebHost(port: 9002, shutdownTimeout: TimeSpan.FromSeconds(60));

        /// <summary>
        /// Starts the web host asynchronously.
        /// </summary>
        public static void StartWebHost() => Task.Run(s_webHost.Run);

        /// <summary>
        /// Creates a new web host with default settings.
        /// </summary>
        public static void NewWebHost()
            => s_webHost = NewWebHost(port: 9002, shutdownTimeout: TimeSpan.FromSeconds(60));

        // Creates a new web host with the specified port and shutdown timeout.
        private static IWebHost NewWebHost(int port, TimeSpan shutdownTimeout)
        {
            return WebHost
                .CreateDefaultBuilder()
                .UseUrls()
                .ConfigureKestrel(i => i.Listen(IPAddress.Any, port))
                .UseStartup<Startup>()
                .UseShutdownTimeout(shutdownTimeout)
                .Build();
        }

        /// <summary>
        /// Removes the existing web host, stopping and disposing it if it exists.
        /// </summary>
        public static void RemoveWebHost()
        {
            try
            {
                // Attempt to stop and dispose the web host
                s_webHost?.StopAsync().GetAwaiter().GetResult();
                s_webHost?.Dispose();
            }
            catch (Exception e)
            {
                // Handle any exceptions that might occur during stopping or disposing
                Console.WriteLine($"{e}");
                s_webHost.Dispose();
            }
        }
    }
}
