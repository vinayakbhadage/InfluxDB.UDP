## InfluxDB UDP Client ##


InfluxDB is a time series, metrics, and analytics database. It’s written in Go and has no external dependencies.


The advantage of InfluxDB UDP Client is that it sends stats to database when you want to send it. 
Also, if there's an issue in infuxDB it **will not** affect your application. So, even if your InfluxDB server is not running your application will still not break.

Also If you want to add a column in database you simply have to send the the column name and its value. And the client will add a column itself. 


### How to Use it ###

1.First you have to install the InfluxDB with UDP using [this](https://github.com/vinayakbhadage/docker-influxdb-collectd) or from http://influxdb.com/docs/v0.8/introduction/getting_started.html

Provided that your InfluxDB is installed on the Host. After this your client is ready to send the stats.

Initialize Client:

	var hostName = "";//Host Name
	var machineName = "";//Name of the machine of which stats you are storing.Just for reference
	var udpPort = "";//Port on which InfluxDB is listening

	MetricsManager.Configure(hostName,machineName,udpPort);



2.Now send stats to server.You can use any 1 method out of these 3 for that.

	MetricsTimer StartTimer(string seriesName, Dictionary<string, object> dataPoints = null)
	//Or
	void Time(Action action, string seriesName, Dictionary<string, object> dataPoints = null)
	//Or
	T Time<T>(Func<T> func, string seriesName, Dictionary<string, object> dataPoints = null)

You can send the stats through dataPoints.

Sample:

1)

	var seriesName = "Sample.TestSeries";
	var dataPoints = new Dictionary<string, object>();
    	dataPoints.Add("ApplicationId", 12454556546);

    using (var metrics = MetricsManager.StartTimer(seriesName))
    {
		//Call Your Method Here

        metrics.AddDataPoints("OrganizationId", 111);
        metrics.AddDataPoints("UserId", 6543);
    }

2)

    private void SampleMethod()
    {
        Thread.Sleep(3000);
    }

    var seriesName = "Sample.TestSeries";

	var dataPoints = new Dictionary<string, object>();
    dataPoints.Add("ApplicationId", 12454556546);    
	
	MetricsManager.Time(SampleMethod, seriesName, dataPoints);



3)

    private int SampleMethod(int sleepTime)
    {
        Thread.Sleep(sleepTime);
        return sleepTime;
    }

    var seriesName = "Sample.TestSeries";

	var dataPoints = new Dictionary<string, object>();
    dataPoints.Add("ApplicationId", 12454556546);   

    var returnValue = MetricsManager.Time(() => SampleMethod(3000), seriesName, dataPoints);





**Contributers**:

[Vinayak Bhadage](https://in.linkedin.com/pub/vinayak-bhadage/9/624/47a)

[Abhijeet Mahajan](https://in.linkedin.com/pub/abhijeet-mahajan/88/575/188)


**References:**

https://github.com/vinayakbhadage/InfluxDB.Net

https://github.com/vinayakbhadage/statsd-csharp-client
