using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility
{
	/// <summary>
	/// https://gist.github.com/dalexsoto/9fd3c5bdbe9f61a717d47c5843384d11
	/// </summary>
	public static class HttpClientProgressExtensions
	{
		public static async Task DownloadDataAsync(this HttpClient client, string requestUrl, Stream destination, IProgress<float> progress)
			=> await client.DownloadDataAsync(requestUrl, destination, progress, default);

		public static async Task DownloadDataAsync(this HttpClient client, string requestUrl, Stream destination, IProgress<float> progress, CancellationToken cancellationToken)
		{
			using var response = await client.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

			var contentLength = response.Content.Headers.ContentLength;
			using var download = await response.Content.ReadAsStreamAsync(cancellationToken);

			if (progress is null || !contentLength.HasValue)
			{
				await download.CopyToAsync(destination, cancellationToken);
				return;
			}

			var progressWrapper = new Progress<long>(totalBytes => progress.Report(GetProgressPercentage(totalBytes, contentLength.Value)));
			await download.CopyToAsync(destination, 81920, progressWrapper, cancellationToken);

			static float GetProgressPercentage(float totalBytes, float currentBytes) => (totalBytes / currentBytes) * 100f;
		}

		static async Task CopyToAsync(this Stream source, Stream destination, int bufferSize, IProgress<long> progress, CancellationToken cancellationToken)
		{
			if (bufferSize < 0)
				throw new ArgumentOutOfRangeException(nameof(bufferSize));
			if (source is null)
				throw new ArgumentNullException(nameof(source));
			if (!source.CanRead)
				throw new InvalidOperationException($"'{nameof(source)}' is not readable.");
			if (destination == null)
				throw new ArgumentNullException(nameof(destination));
			if (!destination.CanWrite)
				throw new InvalidOperationException($"'{nameof(destination)}' is not writable.");

			var buffer = new byte[bufferSize];
			long totalBytesRead = 0;
			int bytesRead;
			while ((bytesRead = await source.ReadAsync(buffer, cancellationToken).ConfigureAwait(false)) != 0)
			{
				await destination.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken).ConfigureAwait(false);
				totalBytesRead += bytesRead;
				progress?.Report(totalBytesRead);
			}
		}
	}
}
