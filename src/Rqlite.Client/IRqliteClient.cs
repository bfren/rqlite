// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Threading.Tasks;
using Rqlite.Client.Response;

namespace Rqlite.Client;

/// <summary>
/// Makes requests to Rqlite instance using HttpClient.
/// </summary>
public interface IRqliteClient : IDisposable
{
	/// <summary>
	/// Use /status endpoint to check Rqlite is running and return the version of the connected instance.
	/// </summary>
	/// <returns>Rqlite version string.</returns>
	Task<string> GetVersionAsync();

	/// <summary>
	/// Use /status endpoint to get the status of the connected Rqlite instance.
	/// </summary>
	/// <returns>RqliteStatus values.</returns>
	Task<RqliteStatus> GetStatusAsync();

	/// <summary>
	/// Execute commands and return results. (Does not use a transaction -
	/// i.e. if one fails, the others will be executed.)
	/// </summary>
	/// <param name="commands">Rqlite commands.</param>
	/// <returns>Command results.</returns>
	Task<RqliteExecuteResponse> ExecuteAsync(params string[] commands);

	/// <summary>
	/// Execute commands and return results, optionally using a single transaction
	/// (i.e. if one fails, none will be executed).
	/// </summary>
	/// <param name="asSingleTransaction">If true, commands will be executed together as a single transaction.</param>
	/// <param name="commands">Rqlite commands.</param>
	/// <returns>Command results.</returns>
	Task<RqliteExecuteResponse> ExecuteAsync(bool asSingleTransaction, params string[] commands);

	/// <summary>
	/// Execute parameterised command and return results.
	/// </summary>
	/// <param name="command">Rqlite parameterised command.</param>
	/// <param name="param">Command parameters - property names must match parameter names.</param>
	/// <returns>Command results.</returns>
	Task<RqliteExecuteResponse> ExecuteAsync(string command, object param);

	/// <summary>
	/// Execute parameterised commands and return results. (Does not use a transaction -
	/// i.e. if one fails, the others will be executed.)
	/// </summary>
	/// <param name="commands">Rqlite parameterised commands - property names must match parameter names.</param>
	/// <returns>Command results.</returns>
	Task<RqliteExecuteResponse> ExecuteAsync(params (string command, object param)[] commands);

	/// <summary>
	/// Execute parameterised commands and return results, optionally using a single transaction
	/// (i.e. if one fails, none will be executed).
	/// </summary>
	/// <param name="asSingleTransaction">If true, commands will be executed together as a single transaction.</param>
	/// <param name="commands">Rqlite parameterised commands - property names must match parameter names.</param>
	/// <returns>Command results.</returns>
	Task<RqliteExecuteResponse> ExecuteAsync(bool asSingleTransaction, params (string command, object param)[] commands);

	/// <summary>
	/// Execute queries and return results.
	/// </summary>
	/// <param name="queries">Rqlite queries.</param>
	/// <returns>Query results.</returns>
	Task<RqliteQueryResponse> QueryAsync(params string[] queries);

	/// <summary>
	/// Execute parameterised query and return results.
	/// </summary>
	/// <param name="query">Rqlite parameterised query.</param>
	/// <param name="param">Query parameters - property names must match parameter names.</param>
	/// <returns>Query results.</returns>
	Task<RqliteQueryResponse> QueryAsync(string query, object param);

	/// <summary>
	/// Execute parameterised queries and return results.
	/// </summary>
	/// <param name="queries">Rqlite parameterised queries and parameters - property names must match parameter names.</param>
	/// <returns>Query results.</returns>
	Task<RqliteQueryResponse> QueryAsync(params (string query, object param)[] queries);

	/// <summary>
	/// Execute queries and return strongly-typed results. Warning: each query MUST have the same return value or columns,
	/// or the deserialisation to <typeparamref name="T"/> will fail.
	/// </summary>
	/// <typeparam name="T">Return model type.</typeparam>
	/// <param name="queries">Rqlite queries.</param>
	/// <returns>Query results.</returns>
	Task<RqliteQueryResponse<T>> QueryAsync<T>(params string[] queries);

	/// <summary>
	/// Execute parameterised query and return results.
	/// </summary>
	/// <typeparam name="T">Return model type.</typeparam>
	/// <param name="query">Rqlite parameterised query.</param>
	/// <param name="param">Query parameters - property names must match parameter names.</param>
	/// <returns>Query results.</returns>
	Task<RqliteQueryResponse<T>> QueryAsync<T>(string query, object param);

	/// <summary>
	/// Execute parameterised queries and return results. Warning: each query MUST have the same return value or columns,
	/// or the deserialisation to <typeparamref name="T"/> will fail.
	/// </summary>
	/// <typeparam name="T">Return model type.</typeparam>
	/// <param name="queries">Rqlite parameterised queries and parameters - property names must match parameter names.</param>
	/// <returns>Query results.</returns>
	Task<RqliteQueryResponse<T>> QueryAsync<T>(params (string query, object param)[] queries);

	/// <summary>
	/// Execute a query and return a strongly-typed value.
	/// </summary>
	/// <typeparam name="T">Return value type.</typeparam>
	/// <param name="query">Rqlite query.</param>
	/// <returns>Query value.</returns>
	Task<RqliteScalarResponse<T>> ScalarAsync<T>(string query);

	/// <summary>
	/// Execute parameterised query and return a strongly-typed value.
	/// </summary>
	/// <typeparam name="T">Return value type.</typeparam>
	/// <param name="query">Rqlite parameterised query.</param>
	/// <param name="param">Query parameters - property names must match parameter names.</param>
	/// <returns>Query value.</returns>
	Task<RqliteScalarResponse<T>> ScalarAsync<T>(string query, object param);
}
