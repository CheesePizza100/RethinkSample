using System.Text;

namespace RethinkSample.Extensions;

public static class IFormFileExtensions
{
    public static string ReadAsString(this IFormFile file)
    {
        StringBuilder sb = new();
        using (StreamReader reader = new(file.OpenReadStream()))
        while (reader.Peek() >= 0)
        {
            sb.AppendLine(reader.ReadLine());
        }

        return sb.ToString();
    }
}