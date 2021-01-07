using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TodoClient.Helpers
{
    public class StringConverter: JsonConverter<string>
    {
        //Class này có tác dụng làm cho ứng dụng Blazor tương thích với các API trả về 
        //thuộc tính integer id hoặc string id trong phản hồi JSON.
        //Nếu không có trình chuyển đổi chuỗi, một API trả về id int sẽ dẫn đến lỗi
        //Dạng như: The JSON value could not be converted to System.String. Path: $.id.
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // deserialize numbers as strings.
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32().ToString();
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                return reader.GetString();
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}
