using System.Text;
using System.Text.Json;

namespace Kiosko.Aplication.Util
{
    public class Helpers
    {

        public static bool IsJSON(string str)
        {
            try
            {
                var json = JsonDocument.Parse(str);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        public static IDictionary<string, object> DecodeJWT(string jwt)
        {
            if (!string.IsNullOrEmpty(jwt))
            {
                var parts = jwt.Split('.');
                var header = DecodeFragment(parts[0]);
                var claims = DecodeFragment(parts[1]);
                var signature = Convert.FromBase64String(parts[2]);

                return new Dictionary<string, object>
                {
                    { "header", header },
                    { "claims", claims },
                    { "signature", signature }
                };
            }

            return null;
        }

        protected static IDictionary<string, object> DecodeFragment(string value)
        {
            return JsonSerializer.Deserialize<Dictionary<string, object>>(
                Encoding.UTF8.GetString(Convert.FromBase64String(value))
            );
        }

    }
}
