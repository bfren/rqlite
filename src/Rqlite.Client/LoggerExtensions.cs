// Maybe: Rqlite Client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace Rqlite.Client;

public static class LoggerExtensions
{
	private static readonly Action<ILogger, HttpMethod, Uri?, string?, Exception?> LogRequest =
		LoggerMessage.Define<HttpMethod, Uri?, string?>(LogLevel.Debug, new(), "{Method} {Uri}: {Content}");

	private static readonly Action<ILogger, string, Exception?> LogResponseJson =
		LoggerMessage.Define<string>(LogLevel.Debug, new(), "Response JSON: {Json}");

	private static readonly Action<ILogger, string?, Exception?> LogVersion =
		LoggerMessage.Define<string?>(LogLevel.Debug, new(), "Rqlite version: {Version}");

	public static void Request(this ILogger @this, HttpRequestMessage request)
	{
		var content = request.Content?.ReadAsStringAsync().Result;
		LogRequest(@this, request.Method, request.RequestUri, content, null);
	}

	public static void ResponseJson(this ILogger @this, string json) =>
		LogResponseJson(@this, json, null);

	public static void Version(this ILogger @this, string? version) =>
		LogVersion(@this, version, null);
}
