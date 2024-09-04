using System.Text.Json.Serialization;

namespace Api.Dtos.User
{
    public class CreateUserRequestDto
    {
        [JsonIgnore]
        public int ID { get; set; }
        public string ADI { get; set; }
        public string SOYADI { get; set; }
        public string KULLANICI_ADI { get; set; }
        public string SIFRE { get; set; }
    }
}
