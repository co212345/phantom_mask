using System.Text.Json;
using System.Text.Json.Serialization;

namespace phantom_mask.Data
{
    public class JsonDateTimeConverter : JsonConverter<DateTime>
    {
        private static readonly string[] formats = {
        "yyyy-MM-dd HH:mm:ss",
    };

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string str = reader.GetString()!;
            foreach (var fmt in formats)
            {
                if (DateTime.TryParseExact(str, fmt, null, System.Globalization.DateTimeStyles.None, out var dt))
                    return dt;
            }
            throw new JsonException($"Invalid date format: {str}");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss"));
    }

}
