// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Text.Json.Serialization;

namespace Rqlite.Internal.Response;

/// <summary>
/// Result properties.
/// </summary>
public sealed record class ExecuteResponseResult : ResponseResult
{
	/// <summary>
	/// For INSERT commands, the ID of the last item inserted.
	/// </summary>
	[JsonPropertyName("last_insert_id")]
	public long LastInsertId { get; init; }

	/// <summary>
	/// The number of rows affected by the command.
	/// </summary>
	[JsonPropertyName("rows_affected")]
	public long RowsAffected { get; init; }
}
