using Newtonsoft.Json;

namespace DNATestingSystem.MVCWebApp.FE.TienDM.Converters
{
    public class DateOnlyConverter : JsonConverter<DateOnly>
    {
        public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString("yyyy-MM-dd"));
        }

        public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string? dateString = reader.Value?.ToString();
            if (string.IsNullOrEmpty(dateString))
                return default;

            return DateOnly.Parse(dateString);
        }
    }

    public class TimeOnlyConverter : JsonConverter<TimeOnly>
    {
        public override void WriteJson(JsonWriter writer, TimeOnly value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString("HH:mm:ss"));
        }

        public override TimeOnly ReadJson(JsonReader reader, Type objectType, TimeOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string? timeString = reader.Value?.ToString();
            if (string.IsNullOrEmpty(timeString))
                return default;

            return TimeOnly.Parse(timeString);
        }
    }
}
