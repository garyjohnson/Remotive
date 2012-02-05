using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaControl.Common.Music;
using System.ServiceModel;
using MediaControl.Common;
using System.ServiceModel.Description;

namespace MediaControl.Host.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Initializing Service...");
                EndpointService.Current.InitializeService();
                Console.WriteLine("Service initialized.");
                Console.WriteLine("Press enter to exit.");
                Console.ReadLine();
                EndpointService.Current.TeardownService();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.TargetSite);
                Console.WriteLine(ex.StackTrace);
                Console.ReadLine();
            }
        }
    }
}
