// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Rqlite.Client.RqliteClientFactoryTests;

public abstract class RqliteClientFactoryTests
{
	protected static (RqliteClientFactory, Vars) Setup(RqliteOptions? options = null)
	{
		var httpClientFactorySub = Substitute.For<IHttpClientFactory>();
		var loggerSub = Substitute.For<ILogger<RqliteClient>>();
		var optionsInst = options ?? new();
		var optionsSub = Substitute.For<IOptions<RqliteOptions>>();
		_ = optionsSub.Value.Returns(optionsInst);
		var rqliteClientFactory = new RqliteClientFactory(httpClientFactorySub, loggerSub, optionsSub);

		return (rqliteClientFactory, new(httpClientFactorySub, loggerSub, optionsInst));
	}

	public sealed record class Vars(
		IHttpClientFactory HttpClientFactory,
		ILogger<RqliteClient> Logger,
		RqliteOptions Options
	);
}
