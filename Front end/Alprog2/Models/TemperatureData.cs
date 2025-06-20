using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Alprog2.Models
{
    public class TemperatureModel
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("suhu")]
        public double suhu { get; set; }

        [BsonElement("Waktu")]
        public DateTime Waktu { get; set; }

        [BsonElement("resistance")]
        public double resistance { get; set; }

        [BsonElement("numerik")]
        public double numerik { get; set; }

        public string ValueText => $"{suhu} °C";

        public string TimeText => Waktu.ToString("g");
    }
    class TemperatureData
    {
    }
}
