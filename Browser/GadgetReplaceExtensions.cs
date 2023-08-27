using BrowserLibCore;

namespace Browser;
internal static class GadgetReplaceExtensions
{
	public static string GetReplaceURL(this GadgetServerOptions option, string GadgetServerCustom = "") => option switch
	{
		GadgetServerOptions.Wiki => "https://kcwiki.github.io/cache/gadget_html5/",
		GadgetServerOptions.Custom => GadgetServerCustom + "/gadget_html5/",
		_ => "https://electronicobserveren.github.io/cache/gadget_html5/"
	};
}
