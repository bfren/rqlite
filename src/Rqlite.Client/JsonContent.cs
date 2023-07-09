// Maybe: Rqlite Client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Rqlite.Client;

public sealed class JsonContent : StringContent
{
	public JsonContent(object content)
		: base(JsonSerializer.Serialize(content, RqliteClient.JsonOptions), Encoding.UTF8, "application/json")
	{ }
}
