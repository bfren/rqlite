// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Rqlite.Internal.Request;

namespace Rqlite.Client.RqliteClientTests;

public abstract class RqliteClientTests
{
	protected static (RqliteClient, Vars) Setup(bool? withTimings = null, HttpStatusCode? status = null, HttpContent? content = null)
	{
		var handler = Substitute.ForPartsOf<HttpMessageHandlerMock>(status);
		handler.SendAsync(default!).ReturnsForAnyArgs(x =>
		{
			var status = handler.Status;
			var value = handler.Value;
			return Task.FromResult(new HttpResponseMessage(status) { Content = content ?? new JsonContent(value, RqliteClientFactory.DefaultJsonOptions) });
		});

		var httpClient = new HttpClient(handler) { BaseAddress = new("http://localhost:4001") };

		var includeTimings = withTimings ?? Rnd.Flip;

		var logger = Substitute.ForPartsOf<LoggerMock>();

		return (new(httpClient, RqliteClientFactory.DefaultJsonOptions, includeTimings, logger), new(httpClient, handler, includeTimings, logger));
	}

	protected static string Json<T>(T obj) =>
		JsonSerializer.Serialize(obj, RqliteClientFactory.DefaultJsonOptions);

	public sealed record class Vars(
		HttpClient HttpClient,
		HttpMessageHandlerMock HttpMessageHandler,
		bool IncludeTimings,
		LoggerMock Logger
	);

	public sealed record class Response(
		Guid Foo,
		int Bar
	);

	public abstract class HttpMessageHandlerMock : HttpMessageHandler
	{
		public HttpStatusCode Status { get; private init; }

		public Response Value { get; private init; }

		public HttpMessageHandlerMock(HttpStatusCode? status = null) =>
			(Status, Value) = (status ?? HttpStatusCode.OK, new(Rnd.Guid, Rnd.Int));

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
			SendAsync(request);

		public abstract Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
	}

	public abstract class LoggerMock : ILogger<RqliteClient>
	{
		public abstract IDisposable? BeginScope<TState>(TState state) where TState : notnull;

		public bool IsEnabled(LogLevel logLevel) => true;

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) =>
			Log(logLevel, formatter(state, exception));

		public abstract void Log(LogLevel logLevel, string message);
	}
}
