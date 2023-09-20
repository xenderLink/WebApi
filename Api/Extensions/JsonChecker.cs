using System.Text.Json;

namespace Store.Extensions;

public static class JsonChecker
{
    public static bool IsJson(this string source)
    {
        try
        {
            JsonDocument.Parse(source);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}