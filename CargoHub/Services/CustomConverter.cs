using System.Text.Json;
using System.Text.Json.Serialization;
namespace CargoHub.Services
{
    public class CustomDateTimeConverter : JsonConverter<DateTime>
    {
        private static readonly string[] DateTimeFormats =
    {
        "yyyy-MM-dd H:mm:ss",
        "yyyy-MM-ddTH:mm:ssZ",
        "yyyy-MM-dd",
        "yyyy-MM-ddTH:mm:ss"
    };
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateTimeString = reader.GetString();

            foreach (var format in DateTimeFormats)
            {
                if (DateTime.TryParseExact(dateTimeString, format, null, System.Globalization.DateTimeStyles.AssumeUniversal, out var dateTime))
                {
                    return dateTime;
                }
            }
            throw new JsonException();
        }



        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}