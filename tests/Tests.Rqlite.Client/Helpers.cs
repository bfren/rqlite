// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Text;
using Rqlite.Internal.Request;
using Rqlite.Internal.Response;

namespace Rqlite.Client;

internal static class Helpers
{
	#region ExecuteAsync

	public static Func<HttpRequestMessage, Task<Result<List<ExecuteResponseResult>>>> GetExecuteSubstitute() =>
		Substitute.For<Func<HttpRequestMessage, Task<Result<List<ExecuteResponseResult>>>>>();

	public static Func<HttpRequestMessage, Task<Result<List<ExecuteResponseResult>>>> GetExecuteSubstitute(int lastResultId, int rowsAffected)
	{
		var sub = GetExecuteSubstitute();
		sub.Invoke(default!).ReturnsForAnyArgs(GetExecuteResponseResult(lastResultId, rowsAffected));
		return sub;
	}

	public static Result<List<ExecuteResponseResult>> GetExecuteResponseResult(int lastResultId, int rowsAffected) =>
		new List<ExecuteResponseResult> { new() { LastInsertId = lastResultId, RowsAffected = rowsAffected } };

	#endregion

	#region GetQueryAsync

	public static Func<HttpRequestMessage, Task<Result<List<QueryResponseResult<T>>>>> GetQuerySubstitute<T>() =>
		Substitute.For<Func<HttpRequestMessage, Task<Result<List<QueryResponseResult<T>>>>>>();

	public static Func<HttpRequestMessage, Task<Result<List<QueryResponseResult<T>>>>> GetQuerySubstitute<T>(T[] returnValue)
	{
		var sub = GetQuerySubstitute<T>();
		sub.Invoke(default!).ReturnsForAnyArgs(GetQueryResponseResult(returnValue));
		return sub;
	}

	public static Result<List<QueryResponseResult<T>>> GetQueryResponseResult<T>(T[] value) =>
		new List<QueryResponseResult<T>> { new() { Rows = [.. value] } };

	#endregion

	#region GetScalarAsync

	public static Func<HttpRequestMessage, Task<Result<List<ScalarResponseResult<T>>>>> GetScalarSubstitute<T>() =>
		Substitute.For<Func<HttpRequestMessage, Task<Result<List<ScalarResponseResult<T>>>>>>();

	public static Func<HttpRequestMessage, Task<Result<List<ScalarResponseResult<T>>>>> GetScalarSubstitute<T>(T returnValue)
		where T : struct
	{
		var sub = GetScalarSubstitute<T>();
		sub.Invoke(default!).ReturnsForAnyArgs(GetScalarResponseResult(returnValue));
		return sub;
	}

	public static Result<List<ScalarResponseResult<T>>> GetScalarResponseResult<T>(T value)
		where T : struct =>
		new List<ScalarResponseResult<T>> { { new() { Values = [new List<T>([value])] } } };

	#endregion

	#region GetStatusAsync

	public static HttpContent GetStatusHttpContent(string? version = null)
	{
		var content = new RqliteStatus(
			Build: new(version ?? Rnd.Str),
			Http: new(Rnd.Str, Rnd.Str, new(Rnd.Str)),
			Node: new(Rnd.Str),
			Store: new(Rnd.Int)
		);

		return new JsonContent(content);
	}

	#endregion

	#region HttpContent

	public static string ReadContent(HttpRequestMessage request)
	{
		if (request.Content is null)
		{
			return string.Empty;
		}

		var ms = new MemoryStream();
		request.Content.ReadAsStream().CopyTo(ms);
		return Encoding.UTF8.GetString(ms.ToArray());
	}

	#endregion
}
