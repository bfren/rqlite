// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Rqlite.Client.Internals;

namespace Rqlite.Client;

public sealed partial class RqliteClient : IRqliteClient
{
	/// <summary>
	/// Send a request and deserialise the JSON response.
	/// </summary>
	/// <typeparam name="T">Response type.</typeparam>
	/// <param name="request">Request message</param>
	/// <returns>Deserialised JSON response.</returns>
	/// <exception cref="JsonException">If the JSON response returns a null value.</exception>
	internal async Task<T> SendAsync<T>(HttpRequestMessage request)
	{
		Logger.Request(request);

		var response = await HttpClient.SendAsync(request);
		var json = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
		Logger.ResponseJson(json);

		var result = JsonSerializer.Deserialize<T>(json, JsonContent.SerialiserOptions);
		return result ?? throw new JsonException($"'{json}' deserialised to a null value.");
	}
}
