using System;
using System.Configuration;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using InfluxDB.UDP;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestInfluxDB.UDP
{
    [TestClass]
    public class InfluxDBTests
    {
        [TestMethod]
        public void PublishStatsToInfluxDbUsingStartTimer()
        {
            var hostName = ConfigurationManager.AppSettings["InfluxDbServerName"];
            MetricsManager.Configure(hostName);

            var seriesName = "Sample.TestSeries";

            var dataPoints = new Dictionary<string, object>();
            dataPoints.Add("ApplicationId", 1111);

            using (var metrics = MetricsManager.StartTimer(seriesName))
            {
                metrics.AddDataPoints("OrganizationId", 111);
                metrics.AddDataPoints("UserId", 11);
            }
        }
        [TestMethod]
        public void PublishStatsToInfluxDbUsingTimeWithNoReturn()
        {
            var hostName = ConfigurationManager.AppSettings["InfluxDbServerName"];
            MetricsManager.Configure(hostName);

            var seriesName = "Sample.TestSeries";

            MetricsManager.Time(SampleMethod, seriesName);
        }

        [TestMethod]
        public void PublishStatsToInfluxDbUsingTimeWithReturn()
        {
            var hostName = ConfigurationManager.AppSettings["InfluxDbServerName"];
            MetricsManager.Configure(hostName);

            var seriesName = "Sample.TestSeries";

            var returnValue = MetricsManager.Time(() => SampleMethod(3000), seriesName);
        }

        #region private sample methods

        private void SampleMethod()
        {
            Thread.Sleep(3000);
        }

        private int SampleMethod(int sleepTime)
        {
            Thread.Sleep(sleepTime);
            return sleepTime;
        }
        
        #endregion
    }
}
