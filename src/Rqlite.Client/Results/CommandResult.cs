// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Rqlite.Client.Internals;

namespace Rqlite.Client.Results;

/// <summary>
/// Command result properties.
/// </summary>
public readonly struct CommandResult
{
	/// <summary>
	/// For INSERT commands, the ID of the last item inserted.
	/// </summary>
	public readonly required int LastInsertId { get; init; }

	/// <summary>
	/// The number of rows affected by the command.
	/// </summary>
	public readonly required int RowsAffected { get; init; }

	internal static CommandResult Create(ExecuteResponseResult result) =>
		new() { LastInsertId = result.LastInsertId, RowsAffected = result.RowsAffected };
}
