using System.IO.Ports;
namespace Smart_farm.Model
{
    public class Arudino
    {
        public Model.DBContext dbContext;
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
            if ((DateTime.Now - lastRecordTime).TotalSeconds >= 60)
            {
                double humidity = double.Parse(splited[1]);
                int sensorRaw = int.Parse(splited[3]);
                bool relayOn = bool.Parse((splited[5] == "1").ToString());
                RecordData data = new RecordData
                {
                    datetime = DateTime.Now,
                    humidity = humidity,
                    sensorRawData = sensorRaw,
                    isRelayOn = relayOn,
                };
                int rowCount = dbContext.recordDatas.Count();
                if (rowCount >= 10000)
                {
                    dbContext.recordDatas.Remove(dbContext.recordDatas.First());
                }
                lastRecordTime = data.datetime;
                dbContext.recordDatas.Add(data);
                dbContext.SaveChanges();
            }
        }
    }
}
