using BrowserLibCore;

namespace Browser;

public static class GadgetReplaceExtensions
{
	public static string GetReplaceURL(this GadgetServerOptions option, string GadgetServerCustom = "") => option switch
	{
		GadgetServerOptions.Wiki => "https://kcwiki.github.io/cache/gadget_html5/",
		GadgetServerOptions.Custom => GadgetServerCustom.CombineUrl("/gadget_html5/"),
		_ => "https://electronicobserveren.github.io/cache/gadget_html5/"
	};
}
