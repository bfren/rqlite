// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Wrap;

namespace Rqlite.Internal;

/// <summary>
/// Extension methods for <see cref="ILogger"/> to use <see cref="LoggerMessage"/> pattern.
/// </summary>
public static partial class LoggerExtensions
{
	// HT https://stackoverflow.com/a/462586/8199362
	[GeneratedRegex(@"\\U([0-9A-F]{4})", RegexOptions.IgnoreCase, "en-GB")]
	private static partial Regex UnicodeEscapedCharacters();

	private static readonly Action<ILogger, ErrValue, Exception?> LogErr =
		LoggerMessage.Define<ErrValue>(LogLevel.Error, new(), "Error: {ErrValue}");

	private static readonly Action<ILogger, HttpMethod, Uri?, string?, Exception?> LogRequest =
		LoggerMessage.Define<HttpMethod, Uri?, string?>(LogLevel.Debug, new(), "{Method} {Uri}: {Content}");

	private static readonly Action<ILogger, string, Exception?> LogResponseJson =
		LoggerMessage.Define<string>(LogLevel.Debug, new(), "Response JSON: {Json}");

	private static readonly Action<ILogger, string, Exception?> LogVersion =
		LoggerMessage.Define<string>(LogLevel.Information, new(), "Rqlite version: {Version}");

	/// <summary>
	/// Unescape UTF8 characters to show correctly in logs.
	/// </summary>
	/// <param name="bytes">UTF8 string as byte array.</param>
	/// <returns>Unescaped string (or empty string if <paramref name="bytes"/> is null or empty).</returns>
	public static string UnescapeUTF8(byte[]? bytes)
	{
		if (bytes is null || bytes.Length == 0)
		{
			return string.Empty;
		}

		var regex = UnicodeEscapedCharacters();
		return regex.Replace(
			input: Encoding.UTF8.GetString(bytes),
			evaluator: match => parse(match).ToString()
		);

		static char parse(Match m) =>
			(char)int.Parse(m.Groups[1].Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
	}

	public static void Err(this ILogger @this, ErrValue value) =>
		LogErr(@this, value, null);

	/// <summary>
	/// Log a <see cref="HttpRequestMessage"/>.
	/// </summary>
	/// <param name="this"></param>
	/// <param name="request">HttpRequestMessage instance.</param>
	public static void Request(this ILogger @this, HttpRequestMessage request)
	{
		var content = UnescapeUTF8(request.Content?.ReadAsByteArrayAsync().Result);
		LogRequest(@this, request.Method, request.RequestUri, content, null);
	}

	/// <summary>
	/// Log raw response JSON.
	/// </summary>
	/// <param name="this"></param>
	/// <param name="json">Response JSON string.</param>
	public static void ResponseJson(this ILogger @this, string json) =>
		LogResponseJson(@this, json, null);

	/// <summary>
	/// Log the Rqlite version.
	/// </summary>
	/// <param name="this"></param>
	/// <param name="version">Version string.</param>
	public static void Version(this ILogger @this, string version) =>
		LogVersion(@this, version, null);
}
