using System;
using System.Text;
using Microsoft.CodeAnalysis;

namespace ElectronicObserver.SourceGenerators
{
	[Generator]
	public class BuildInfoGenerator : IIncrementalGenerator
	{
		public void Initialize(IncrementalGeneratorInitializationContext context)
		{
			context.RegisterPostInitializationOutput(GenerateBuildInfo);
		}

		private void GenerateBuildInfo(IncrementalGeneratorPostInitializationContext context)
		{
			StringBuilder sourceBuilder = new(
				$$"""
					using System;

					namespace ElectronicObserver.Generated
					{
						public static class BuildInfo
						{
							public static long TimeStamp => {{DateTime.UtcNow.Ticks}};
						}
					}
				""");

			context.AddSource("ElectronicObserver.Generated", sourceBuilder.ToString());
		}
	}
}
