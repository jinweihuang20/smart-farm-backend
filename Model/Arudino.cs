using System.IO.Ports;
namespace Smart_farm.Model
{
    public class Arudino
    {
        public Model.SensorContext dbContext;
        public Arudino(string COM, int BaudRate = 9600)
        {
            this.COM = COM;
            this.BaudRate = BaudRate;
        }
        public SerialPort serial = new SerialPort();
        public string COM = "COM3";
        public int BaudRate = 9600;

        public bool Open(string COM, int BaudRate = 9600)
        {
            try
            {

                serial.PortName = COM;
                serial.BaudRate = BaudRate;
                serial.Parity = Parity.None;
                serial.StopBits = StopBits.One;
                serial.DataBits = 8;
                serial.Open();
                return serial.IsOpen;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void FetchWork()
        {
            if (Open(COM, BaudRate))
            {
                serial.DataReceived += Serial_DataReceived;
            }
        }
        DateTime lastRecordTime = DateTime.MinValue;
        private void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var str = serial.ReadLine();
            str = str.Replace("\r", "");
            string[] splited = str.Split(",");
            if (splited.Length < 6)
            {
                return;
            }
            Console.WriteLine(str);
            double humidity = Convert.ToDouble(splited[1]);
            int sensorRaw = Convert.ToInt32(splited[3]);
            bool relayOn = bool.Parse((splited[5] == "1").ToString());
            Mega2560 data = new Mega2560
            {
                Datetime = DateTime.Now,
                Humidity = humidity,
                Raw = sensorRaw,
                Relayon = relayOn,
            };

            var realTimeData = dbContext.RealTimes.FirstOrDefault(f => f.Name == "Mega2560");
            if (realTimeData != null)
            {
                realTimeData.Datetime = data.Datetime;
                realTimeData.Humidity = data.Humidity;
                realTimeData.Raw = data.Raw;
                realTimeData.Relayon = data.Relayon;
            }
            else
                dbContext.RealTimes.Add(new RealTime
                {
                    Datetime = data.Datetime,
                    Humidity = data.Humidity,
                    Relayon = data.Relayon,
                    Raw = data.Raw,
                    Name = "Mega2560"
                });

            if ((DateTime.Now - lastRecordTime).TotalSeconds >= 60)
            {
                int rowCount = dbContext.Mega2560s.Count();
                if (rowCount >= 10000)
                {
                    dbContext.Mega2560s.Remove(dbContext.Mega2560s.First());
                }
                lastRecordTime = data.Datetime;
                dbContext.Mega2560s.Add(data);
            }
            dbContext.SaveChanges();
        }
    }
}
