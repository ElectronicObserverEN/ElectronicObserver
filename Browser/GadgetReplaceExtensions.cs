using BrowserLibCore;

namespace Browser;

public static class GadgetReplaceExtensions
{
	public static string GetReplaceUrl(this GadgetServerOptions option, string gadgetServerCustom = "") => option switch
	{
		GadgetServerOptions.Wiki => "https://kcwiki.github.io/cache/gadget_html5/",
		GadgetServerOptions.Custom => Url.Combine(gadgetServerCustom, "/gadget_html5/"),
		_ => "https://electronicobserveren.github.io/cache/gadget_html5/",
	};
}
