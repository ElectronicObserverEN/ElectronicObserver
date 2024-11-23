﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Wpf.SenkaLeaderboard;

namespace ElectronicObserver.Window.Wpf.Bonodere;

public class BonodereHttpClient
{
	private HttpClient? CurrentClient { get; set; }

	public bool IsReady => CurrentClient is not null;

	private static HttpClient MakeHttpClient() => new()
	{
		BaseAddress = new("https://bonodere.famluro.es/api/"),
		DefaultRequestHeaders =
		{
			UserAgent =
			{
#if DEBUG
				new ProductInfoHeaderValue($"ElectronicObserverEN-DEBUG", SoftwareInformation.VersionEnglish),
#else
				new ProductInfoHeaderValue($"ElectronicObserverEN", SoftwareInformation.VersionEnglish),
#endif
			},
		},
	};

	public async Task<BonodereLoginResponse?> Login(string login, SecureString password)
	{
		CurrentClient = MakeHttpClient();

		HttpResponseMessage response = await CurrentClient.PostAsJsonAsync("auth/login", new BonodereLoginRequest
		{
			Key = login,
			Password = SecureStringToString(password),
			Duration = 1500000000,
		});

		response.EnsureSuccessStatusCode();

		BonodereLoginResponse? responseParsed = await response.Content.ReadFromJsonAsync<BonodereLoginResponse>();

		if (responseParsed is null)
		{
			CurrentClient.Dispose();
			CurrentClient = null;
			return null;
		}

		CurrentClient = MakeHttpClient();
		CurrentClient.DefaultRequestHeaders.Add("x-access-token", responseParsed.Token);

		return responseParsed;
	}

	public async Task<BonodereUserDataResponse?> LoginFromToken(string token, string userId)
	{
		CurrentClient?.Dispose();
		CurrentClient = null;

		CurrentClient = MakeHttpClient();
		CurrentClient.DefaultRequestHeaders.Add("x-access-token", token);

		HttpResponseMessage response = await CurrentClient.GetAsync($"user/data/{userId}");

		if (!response.IsSuccessStatusCode)
		{
			await HandleErrorResponse(response);
			return null;
		}

		BonodereUserDataResponse? responseParsed = await response.Content.ReadFromJsonAsync<BonodereUserDataResponse>();

		if (responseParsed is null)
		{
			CurrentClient.Dispose();
			CurrentClient = null;
			return null;
		}

		return responseParsed;
	}

	public async Task Logout()
	{
		if (CurrentClient is null) return;

		HttpResponseMessage response = await CurrentClient.PostAsJsonAsync("auth/logout", new object());

		if (!response.IsSuccessStatusCode)
		{
			await HandleErrorResponse(response);
		}

		CurrentClient.Dispose();
		CurrentClient = null;
	}

	public async Task SubmitData(List<SenkaEntryModel> data)
	{
		if (CurrentClient is null) return;

		HttpResponseMessage response = await CurrentClient.PostAsJsonAsync("senka/submit", new BonodereSubmissionRequest()
		{
			Data = data,
		});

		if (!response.IsSuccessStatusCode)
		{
			await HandleErrorResponse(response);
		}
		else
		{
			Logger.Add(2, "Bonodere submission : Success");
		}
	}

	private async Task HandleErrorResponse(HttpResponseMessage response)
	{
		BonodereError? errorData = await response.Content.ReadFromJsonAsync<BonodereError>();

		if (errorData is not null && !string.IsNullOrEmpty(errorData.Message))
		{
			Logger.Add(2, errorData switch
			{
				{ Code: >0 } => $"Bonodere error : {errorData.Message} ({errorData.Code})",
				 _ => $"Bonodere error : {errorData.Message}",
			});
		}
		else
		{
			response.EnsureSuccessStatusCode();
		}
	}

	private string SecureStringToString(SecureString value)
	{
		IntPtr valuePtr = IntPtr.Zero;

		try
		{
			valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
			return Marshal.PtrToStringUni(valuePtr) ?? "";
		}
		finally
		{
			Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
		}
	}
}
