using System.ComponentModel.DataAnnotations;

namespace Smart_farm.Model
{
    public class RecordData
    {
        [Key]
        public DateTime datetime { get; set; }
        public double humidity { get; set; }
        public int sensorRawData { get; set; }
        public bool isRelayOn { get; set; } = false;
    }
}
