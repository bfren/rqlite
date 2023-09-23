// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Rqlite.Internal.Request;

/// <summary>
/// Serialises and encodes JSON content correctly for use in HttpRequestMessage.
/// </summary>
public sealed class JsonContent : StringContent
{
	/// <summary>
	/// Shared options for JSON serialisation.
	/// </summary>
	public static JsonSerializerOptions SerialiserOptions { get; } =
		new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

	/// <summary>
	/// Serialise content and set encoding.
	/// </summary>
	/// <param name="content">Content to be serialised as JSON.</param>
	public JsonContent(object? content) : base(
		content: JsonSerializer.Serialize(content, SerialiserOptions),
		encoding: Encoding.UTF8,
		mediaType: "application/json"
	)
	{ }
}
