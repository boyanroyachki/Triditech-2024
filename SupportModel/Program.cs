using System.Text;
using System.Text.Json;

string apiKey = "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";
string apiUrl = "https://api.openai.com/v1/chat/completions";

Console.WriteLine("Enter your question or search term:");
string query = Console.ReadLine();

using HttpClient client = new HttpClient();
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

var request = new
{
    model = "gpt-3.5-turbo",
    messages = new[]
    {
        new { role = "system", content = "You are a helpful assistant that provides search results." },
        new { role = "user", content = query }
    }
};

var jsonRequest = JsonSerializer.Serialize(request);
var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

HttpResponseMessage response = await client.PostAsync(apiUrl, content);

if (response.IsSuccessStatusCode)
{
    string responseContent = await response.Content.ReadAsStringAsync();
    Console.WriteLine("Search Result:");
    Console.WriteLine(responseContent);
}
else
{
    Console.WriteLine($"Error performing search. Status code: {response.StatusCode}");
}