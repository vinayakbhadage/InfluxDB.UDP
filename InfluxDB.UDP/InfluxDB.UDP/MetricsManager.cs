using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace InfluxDB.UDP
{
    public static class MetricsManager
    {
        public static string HostName
        {
            get;
            set;
        }
        public static string MachineName
        {
            get;
            set;
        }
        public static int UdpPort
        {
            get;
            set;
        }

        //Need to call Configure to Initialize the Host
        public static void Configure(string hostName, string machineName = "", string udpPort = "4444")
        {
            HostName = hostName;
            MachineName = machineName;
            UdpPort = Convert.ToInt32(udpPort);
        }

        public static MetricsTimer StartTimer(string seriesName, Dictionary<string, object> dataPoints = null)
        {
            if (dataPoints == null)
            {
                dataPoints = new Dictionary<string, object>();
            }

            return new MetricsTimer(seriesName, dataPoints);
        }

        internal static void WriteUdp(string seriesName, Dictionary<string, object> dataPoints)
        {
            if (!dataPoints.ContainsKey("MachineName"))
                dataPoints.Add("MachineName", MachineName);

            var series = new TimeSeries.Builder(seriesName)
               .Columns(dataPoints.Keys.ToArray())
               .Values(dataPoints.Values.ToArray())
               .Build();

            TimeSeries[] arrayTimeSeries = { series };

            var content = JsonConvert.SerializeObject(arrayTimeSeries);
            byte[] sendBytes = Encoding.UTF8.GetBytes(content);

            if (string.IsNullOrEmpty(HostName))
                HostName = "192.168.*.***";   //Default Host Name
            new UdpClient().Send(sendBytes, sendBytes.Length, HostName, UdpPort);
        }

        public static void Time(Action action, string seriesName, Dictionary<string, object> dataPoints = null)
        {
            using (var metrics = StartTimer(seriesName, dataPoints))
            {
                metrics.AddDataPoints("MachineName", MachineName);
                action();
            }
        }

        public static T Time<T>(Func<T> func, string seriesName, Dictionary<string, object> dataPoints = null)
        {
            using (var metrics = StartTimer(seriesName, dataPoints))
            {
                metrics.AddDataPoints("MachineName", MachineName);
                return func();
            }
        }

        public static void Timer(string seriesName, long elapsedMilliseconds, Dictionary<string, object> dataPoints = null)
        {
            if (dataPoints == null)
            {
                dataPoints = new Dictionary<string, object>();
            }

            dataPoints.Add("TimeTaken", elapsedMilliseconds);
            dataPoints.Add("MachineName", MachineName);

            MetricsTimer.SendStats(seriesName, dataPoints);
        }
    }
}
