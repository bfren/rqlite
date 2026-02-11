// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Rqlite.Internal.Response;

namespace Rqlite.Client;

/// <summary>
/// Command result properties.
/// </summary>
public readonly struct RqliteCommandResult
{
	/// <summary>
	/// For INSERT commands, the ID of the last item inserted.
	/// </summary>
	public readonly required long LastInsertId { get; init; }

	/// <summary>
	/// The number of rows affected by the command.
	/// </summary>
	public readonly required long RowsAffected { get; init; }

	internal static RqliteCommandResult Create(ExecuteResponseResult result) =>
		new() { LastInsertId = result.LastInsertId, RowsAffected = result.RowsAffected };
}
