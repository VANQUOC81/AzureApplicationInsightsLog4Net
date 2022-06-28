using log4net;
using log4net.Config;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Log4NetAppender;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Log4Net
{
    internal class Program
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        static void Main(string[] args)
        {
            // load log4net configuration
            XmlConfigurator.Configure();

            // get all existing appenders from xml configuration
            var appenders = Log.Logger.Repository.GetAppenders();

            // supply instrumentation key to the aiAppender
            ApplicationInsightsAppender appInsightAppender = (ApplicationInsightsAppender)appenders.First(x => x.Name == "aiAppender");
            //appInsightAppender.InstrumentationKey = "InstrumentationKey=xxxxGOTOxxxAZUREPORTALxxxx;IngestionEndpoint=https://westeurope-2.in.applicationinsights.azure.com/;LiveEndpoint=https://westeurope.livediagnostics.monitor.azure.com/";
            appInsightAppender.InstrumentationKey = "xxxxGOTOxxxAZUREPORTALxxxx";
            appInsightAppender.ActivateOptions();

            // configure telemetry
            var configuration = TelemetryConfiguration.CreateDefault();
            //configuration.ConnectionString = "InstrumentationKey=xxxxGOTOxxxAZUREPORTALxxxx;IngestionEndpoint=https://westeurope-2.in.applicationinsights.azure.com/;LiveEndpoint=https://westeurope.livediagnostics.monitor.azure.com/";
            var telemetryClient = new TelemetryClient(configuration);

            // send trace information
            telemetryClient.TrackTrace("hello I am here with ApplicationInsights.config + log4net", SeverityLevel.Information);

            // log information
            Log.Info("Log4Net: hello I am here");

            // Initiate flush and give it some time to finish.
            telemetryClient.Flush();
            Task.Delay(7000).Wait();

            Console.WriteLine("Task Delay done, all messages flushed!");
            Console.ReadKey();
        }

    }
}
