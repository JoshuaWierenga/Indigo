using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Indigo.Server
{
    /// <summary>
    /// Main class for server, handles creation and running of a webhost
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Creates and runs a new webhost
        /// </summary>
        /// <param name="args">Arguments to be passed to webhost</param>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        /// <summary>
        /// Creates a new webhost setup for kestrel
        /// </summary>
        /// <param name="args">Arguments to be passed to webhost</param>
        /// <returns></returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();
    }
}