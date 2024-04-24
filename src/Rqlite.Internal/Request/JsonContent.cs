// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Rqlite.Internal.Request;

/// <summary>
/// Serialises and encodes JSON content correctly for use in HttpRequestMessage.
/// </summary>
/// <remarks>
/// Serialise content and set encoding.
/// </remarks>
/// <param name="content">Content to be serialised as JSON.</param>
public sealed class JsonContent(object? content) : StringContent(
	content: JsonSerializer.Serialize(content, SerialiserOptions),
	encoding: Encoding.UTF8,
	mediaType: "application/json"
	)
{
	/// <summary>
	/// Shared options for JSON serialisation.
	/// </summary>
	public static JsonSerializerOptions SerialiserOptions { get; } =
		new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
}
