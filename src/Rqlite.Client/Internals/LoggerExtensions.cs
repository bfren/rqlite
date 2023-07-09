// Maybe: Rqlite Client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace Rqlite.Client.Internals;

/// <summary>
/// Extension methods for <see cref="ILogger"/> to use <see cref="LoggerMessage"/> pattern.
/// </summary>
internal static class LoggerExtensions
{
	private static readonly Action<ILogger, HttpMethod, Uri?, string?, Exception?> LogRequest =
		LoggerMessage.Define<HttpMethod, Uri?, string?>(LogLevel.Debug, new(), "{Method} {Uri}: {Content}");

	private static readonly Action<ILogger, string, Exception?> LogResponseJson =
		LoggerMessage.Define<string>(LogLevel.Debug, new(), "Response JSON: {Json}");

	private static readonly Action<ILogger, string?, Exception?> LogVersion =
		LoggerMessage.Define<string?>(LogLevel.Information, new(), "Rqlite version: {Version}");

	/// <summary>
	/// Log a <see cref="HttpRequestMessage"/>.
	/// </summary>
	/// <param name="this"></param>
	/// <param name="request">HttpRequestMessage instance.</param>
	internal static void Request(this ILogger @this, HttpRequestMessage request)
	{
		var content = request.Content?.ReadAsStringAsync().Result;
		LogRequest(@this, request.Method, request.RequestUri, content, null);
	}

	/// <summary>
	/// Log raw response JSON.
	/// </summary>
	/// <param name="this"></param>
	/// <param name="json">Response JSON string.</param>
	internal static void ResponseJson(this ILogger @this, string json) =>
		LogResponseJson(@this, json, null);

	/// <summary>
	/// Log the Rqlite version.
	/// </summary>
	/// <param name="this"></param>
	/// <param name="version">Version string</param>
	internal static void Version(this ILogger @this, string? version) =>
		LogVersion(@this, version, null);
}
