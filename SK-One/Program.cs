using System.Reflection.Metadata;
using HandlebarsDotNet;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;

var modelId = "";
var endPoint = "";
var apiKey = "";

var builder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(modelId, endPoint, apiKey);
var kernel = builder.Build();

var result = await kernel.InvokePromptAsync("Give me a list of 10 breakfast foods with eggs and cheese");
Console.WriteLine(result);

string city = "Rome";
var prompt = "I'm visiting {{$city}}. What are some activities I should do today?";

var activitiesFunction = kernel.CreateFunctionFromPrompt(prompt);
var arguments = new KernelArguments { ["city"] = city };

// InvokeAsync on the KernelFunction object
var resultII = await activitiesFunction.InvokeAsync(kernel, arguments);
Console.WriteLine(resultII);

// InvokeAsync on the kernel object
resultII = await kernel.InvokeAsync(activitiesFunction, arguments);
Console.WriteLine(resultII);

const string HandlebarsTemplate = """
    <message role="system">You are an AI assistant designed to help with image recognition tasks.</message>
    <message role="user">
        <text>{{request}}</text>
        <image>{{imageData}}</image>
    </message>
    """;

var templateFactory = new HandlebarsPromptTemplateFactory();
var promptTemplateConfig = new PromptTemplateConfig()
{
    Template = HandlebarsTemplate,
    TemplateFormat = "handlebars",
    Name = "Vision_Chat_Prompt",
};

// Create a function from the Handlebars template configuration
var function = kernel.CreateFunctionFromPrompt(promptTemplateConfig, templateFactory);

var argumentsII = new KernelArguments(new Dictionary<string, object?>
{
    {"request", "Describe this image"},
    {"imageData", "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAoAAAAKCAYAAACNMs+9AAAAAXNSR0IArs4c6QAAACVJREFUKFNj/KTO/J+BCMA4iBUyQX1A0I10VAizCj1oMdyISyEAFoQbHwTcuS8AAAAASUVORK5CYII="}
});

var response = await kernel.InvokeAsync(function, argumentsII);