using CefSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser.CefOp
{
	/// <summary>
	/// スクリーンショット撮影に必要なデータを挿入します。
	/// </summary>
	public class ResponseFilterPixiSetting : IResponseFilter
	{
		public bool InitFilter() => true;

		public FilterStatus Filter(Stream dataIn, out long dataInRead, Stream dataOut, out long dataOutWritten)
		{
			if (dataIn == null)
			{
				dataInRead = 0;
				dataOutWritten = 0;
				return FilterStatus.Done;
			}

			using (var reader = new StreamReader(dataIn))
			{
				string raw = reader.ReadToEnd();

				// note: preserveDrawingBuffer = true 設定時に動作が重くなる可能性がある
				// が、 false だとスクリーンショットがハードコピー(Graphics.CopyFromScreen 等)でしか取れなくなる
				// 描画直後に保存処理(toDataUrl)を行うと false でも撮れるらしいが、外部からの操作でそれができるかは不明
				string replaced = raw.Replace(
					@"/pixi.js""></script>",
					@"/pixi.js""></script><script>PIXI.settings.RENDER_OPTIONS.preserveDrawingBuffer=true;</script>");

				var bytes = Encoding.UTF8.GetBytes(replaced);
				dataOut.Write(bytes, 0, bytes.Length);

				dataInRead = dataIn.Length;
				dataOutWritten = Math.Min(bytes.Length, dataOut.Length);
			}

			return FilterStatus.Done;
		}

		public void Dispose() { }
	}

    public class AdFilter : IResponseFilter
    {
        public bool InitFilter() => true;

        public FilterStatus Filter(Stream dataIn, out long dataInRead, Stream dataOut, out long dataOutWritten)
        {
            if (dataIn == null)
            {
                dataInRead = 0;
                dataOutWritten = 0;
                return FilterStatus.Done;
            }

            using (var reader = new StreamReader(dataIn))
            {
                string raw = reader.ReadToEnd();

               string replaced = raw
                   .Replace(@"stats.g.doubleclick.net/dc.js", string.Empty)
                   .Replace("<script type=\"text/javascript\" src=\"/js/marketing/conf.js\"></script>", string.Empty)
                   .Replace("<script type=\"text/javascript\" src=\"/js/marketing/gtm.js\"></script>", string.Empty)
                   .Replace(@"//stat.i3.dmm.com/latest/js/dmm.tracking.min.js", string.Empty)
                   .Replace(@"<script>!function(d,s,id){var js,fjs=d.getElementsByTagName(s)[0],p=/^http:/.test(d.location)?'http':'https';if(!d.getElementById(id)){js=d.createElement(s);js.id=id;js.src=p+'://platform.twitter.com/widgets.js';fjs.parentNode.insertBefore(js,fjs);}}(document, 'script', 'twitter-wjs');</script>", string.Empty)
                   .Replace(@"<script type=""text/javascript"" src=""/js/netgame/analytics.js""></script>", string.Empty);

                var bytes = Encoding.UTF8.GetBytes(replaced);
                dataOut.Write(bytes, 0, bytes.Length);

                dataInRead = dataIn.Length;
                dataOutWritten = Math.Min(bytes.Length, dataOut.Length);
            }

            return FilterStatus.Done;
        }

        public void Dispose() { }
    }
}
