using System.ComponentModel;

namespace BrowserLibCore;
public enum GadgetServerOptions
{
	[Description("https://kcwiki.github.io/cache")]
	Wiki,
	[Description("https://electronicobserveren.github.io/cache")]
	EO,
	Custom
}
