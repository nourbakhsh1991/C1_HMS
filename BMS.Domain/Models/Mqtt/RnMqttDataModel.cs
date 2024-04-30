using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.Models.Mqtt
{
    public class RnMqttDataModel
    {
        public string Id { get; set; }
        public List<int> Data {  get; set; }
        public int Type {  get; set; }
        public int StartAddress {  get; set; }
        public int Count { get; set; }
        public string ModbusName {  get; set; }
        public override string ToString()
        {
            return ModbusName + " (" + Id + ") " + $"Total count is {Count} starting from {StartAddress}";
        }
    }
}
