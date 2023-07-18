// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Rqlite.Client.Internals;

/// <summary>
/// Extension methods for <see cref="ILogger"/> to use <see cref="LoggerMessage"/> pattern.
/// </summary>
internal static partial class LoggerExtensions
{
#if NET7_0_OR_GREATER
	// HT https://stackoverflow.com/a/462586/8199362
	[GeneratedRegex(@"\\U([0-9A-F]{4})", RegexOptions.IgnoreCase, "en-GB")]
	private static partial Regex UnicodeEscapedCharacters();
#endif

	private static readonly Action<ILogger, HttpMethod, Uri?, string?, Exception?> LogRequest =
		LoggerMessage.Define<HttpMethod, Uri?, string?>(LogLevel.Debug, new(), "{Method} {Uri}: {Content}");

	private static readonly Action<ILogger, string, Exception?> LogResponseJson =
		LoggerMessage.Define<string>(LogLevel.Debug, new(), "Response JSON: {Json}");

	private static readonly Action<ILogger, string?, Exception?> LogVersion =
		LoggerMessage.Define<string?>(LogLevel.Information, new(), "Rqlite version: {Version}");

	/// <summary>
	/// Unescape UTF8 characters to show correctly in logs.
	/// </summary>
	/// <param name="bytes">UTF8 string as byte array.</param>
	/// <returns>Unescaped string (or empty string if <paramref name="bytes"/> is null or empty).</returns>
	internal static string UnescapeUTF8(byte[]? bytes)
	{
		if (bytes is null || bytes.Length == 0)
		{
			return string.Empty;
		}

#if NET7_0_OR_GREATER
		var regex = UnicodeEscapedCharacters();
#else
		var regex = new Regex(@"\\U([0-9A-F]{4})", RegexOptions.IgnoreCase);
#endif
		return regex.Replace(
			input: Encoding.UTF8.GetString(bytes),
			evaluator: match => parse(match).ToString()
		);

		static char parse(Match m) =>
			(char)int.Parse(m.Groups[1].Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
	}

	/// <summary>
	/// Log a <see cref="HttpRequestMessage"/>.
	/// </summary>
	/// <param name="this"></param>
	/// <param name="request">HttpRequestMessage instance.</param>
	internal static void Request(this ILogger @this, HttpRequestMessage request)
	{
		var content = UnescapeUTF8(request.Content?.ReadAsByteArrayAsync().Result);
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
	/// <param name="version">Version string.</param>
	internal static void Version(this ILogger @this, string? version) =>
		LogVersion(@this, version, null);
}
