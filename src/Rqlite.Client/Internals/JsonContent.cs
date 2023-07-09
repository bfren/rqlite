// Maybe: Rqlite Client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Rqlite.Client;

/// <summary>
/// Serialises and encodes JSON content correctly for use in HttpRequestMessage.
/// </summary>
internal sealed class JsonContent : StringContent
{
	/// <summary>
	/// Serialise content and set encoding.
	/// </summary>
	/// <param name="content">Content to be serialised as JSON.</param>
	internal JsonContent(object content)
		: base(JsonSerializer.Serialize(content, RqliteClient.JsonOptions), Encoding.UTF8, "application/json")
	{ }
}
