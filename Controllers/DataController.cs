using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Smart_farm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        Model.DBContext db;

        public DataController(IConfiguration config)
        {
            db = new Model.DBContext(config);
        }

        [HttpGet]
        public async Task<Model.RecordData> Get()
        {
            return db.recordDatas.OrderBy(r => r.datetime).Last();
        }

        [HttpGet("DiWiFiPush")]
        public async Task<Model.D1wifi> Push(double humidity, int raw, int relayOn)
        {
            Model.D1wifi d1wifiData = new Model.D1wifi()
            {
                Raw = raw,
                Relayon = relayOn == 1,
                Datetime = DateTime.Now,
                Humidity = humidity,
            };
            Model.SensorContext sensorContext = new Model.SensorContext();
            sensorContext.D1wifis.Add(d1wifiData);


            var realTimeData = sensorContext.RealTimes.FirstOrDefault(i => i.Name == "D1Wifi");
            if (realTimeData != null)
            {
                realTimeData.Datetime = d1wifiData.Datetime;
                realTimeData.Humidity = d1wifiData.Humidity;
                realTimeData.Raw = d1wifiData.Raw;
                realTimeData.Relayon = d1wifiData.Relayon;
            }
            else
            {
                sensorContext.RealTimes.Add(new Model.RealTime
                {
                    Relayon = d1wifiData.Relayon,
                    Raw = d1wifiData.Raw,
                    Datetime = d1wifiData.Datetime,
                    Humidity = d1wifiData.Humidity,
                    Name = "D1Wifi"
                });
            }

            sensorContext.SaveChangesAsync();

            return d1wifiData;
        }

        [HttpGet("Query")]
        public async Task<Model.RecordData[]> Query(DateTime from, DateTime to)
        {
            return db.recordDatas.OrderBy(r => r.datetime).Where(r => r.datetime >= from && r.datetime <= to).ToArray();
        }
    }
}
