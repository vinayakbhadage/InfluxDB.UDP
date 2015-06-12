using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfluxDB.UDP
{
    public class MetricsTimer : IDisposable
    {
        private string seriesName;
        private Dictionary<string, object> dataPoints;
        private readonly Stopwatch _stopWatch;
        private bool _disposed;


        public MetricsTimer(string seriesName, Dictionary<string, object> dataPoints)
        {
            if (dataPoints == null)
            {
                dataPoints = new Dictionary<string, object>();
            }

            this.seriesName = seriesName;
            this.dataPoints = dataPoints;
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
        }

        public void AddDataPoints(string key, object value)
        {
            if (!dataPoints.ContainsKey(key))
            {
                dataPoints.Add(key, value ?? "null");
            }
        }


        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _stopWatch.Stop();

                dataPoints.Add("TimeTaken", _stopWatch.ElapsedMilliseconds);

                SendStats(seriesName, dataPoints);
            }
        }

        public static void SendStats(string seriesName, Dictionary<string, object> dataPoints)
        {
            try
            {
                MetricsManager.WriteUdp(seriesName, dataPoints);
            }
            catch
            {
                return;
            }
        }
    }
}
