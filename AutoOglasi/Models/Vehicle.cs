using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AutoOglasi.Models
{
    public class Vehicle
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Marka")]
        public string Marka { get; set; } = null!;

        [BsonElement("Model")]
        public string Model { get; set; } = null!;

        [BsonElement("Godiste")]
        public int Godiste { get; set; }

        [BsonElement("Cena")]
        public double Cena { get; set; }

        [BsonElement("Opis")]
        public string Opis { get; set; } = null!;

        [BsonElement("Slike")]
        public List<string> Slike { get; set; } = new List<string>();

        [BsonElement("UserId")]
        public string UserId { get; set; } = null!;
    }
}