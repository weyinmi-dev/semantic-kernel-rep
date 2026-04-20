using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

var modelId = config["GoogleAI:Gemini:ModelId"];
var apiKey = config["GoogleAI:ApiKey"];

if (string.IsNullOrWhiteSpace(modelId))
{
    throw new InvalidOperationException("Missing configuration value: GoogleAI:Gemini:ModelId");
}

if (string.IsNullOrWhiteSpace(apiKey))
{
    throw new InvalidOperationException("Missing configuration value: GoogleAI:ApiKey");
}

var builder = Kernel.CreateBuilder();
builder.AddGoogleAIGeminiChatCompletion(modelId, apiKey);

Kernel kernel = builder.Build();

//Get reference to chat completion service
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

while (true)
{
    //Get input from user
    Console.Write("\nEnter your prompt: ");
    var prompt = Console.ReadLine();

    //Exit if prompt is null or empty
    if (string.IsNullOrEmpty(prompt))
        break;

    //Get response from chat completion service
    var response = await chatCompletionService.GetChatMessageContentAsync(prompt);
    Console.WriteLine(response);
}
