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
/// <param name="opt">JsonSerializerOptions.</param>
public sealed class JsonContent(object? content, JsonSerializerOptions opt) : StringContent(
	content: JsonSerializer.Serialize(content, opt),
	encoding: Encoding.UTF8,
	mediaType: "application/json"
	)
{ }
