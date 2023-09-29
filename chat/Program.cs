using System;
using System.Net.Http;
using dotenv.net;
using System.Text;
using Newtonsoft.Json;

DotEnv.Load();

string secretkey = Environment.GetEnvironmentVariable("SECRET_KEY") ?? string.Empty;


if (args.Length > 0)
{
    HttpClient client = new HttpClient();

    client.DefaultRequestHeaders.Add("authorization", $"Bearer  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");

    var content = new StringContent("{\"model\": \"text-davinci-001\", \"prompt\": \""+ args[0] +"\",\"temperature\": 1,\"max_tokens\": 100}", Encoding.UTF8, "application/json");

    HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/completions", content);

    string responseString = await response.Content.ReadAsStringAsync();

    try
    {
        var dyData = JsonConvert.DeserializeObject<dynamic>(responseString);


        string guess = GuessCommand(dyData!.choices[0].text);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"---> My guess at the command prompt is: {guess}");
        Console.ResetColor();

    }
    catch(Exception ex)
    { 
        Console.WriteLine($"---> Could not deserialize the JSON: {ex.Message}");
    }
}
else
{
    Console.WriteLine("Тебе нужно что-то написать потом....");
}


static string GuessCommand(string raw)
{
    Console.WriteLine("---> GPT-3 API Returned Text:");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine(raw);

    var lastIndex = raw.LastIndexOf('\n');

    string guess = raw.Substring(lastIndex + 1);

    Console.ResetColor();

    TextCopy.ClipboardService.SetText(guess);

    return guess;
}