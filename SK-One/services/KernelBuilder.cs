

using Microsoft.SemanticKernel;

namespace SK_One.Services;

	public class KernelBuilder
	{
		public static void Main()
		{
			var modelId = "";
			var endPoint = "";
			var apiKey = "";

			var builder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(modelId, endPoint, apiKey);
			var kernel = builder.Build();
		}
	}
