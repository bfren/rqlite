// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace Rqlite.Client.RqliteClientTests.ConstructorTests;

public class when_includeTimings_is_true : RqliteClientTests
{
	[Fact]
	public void ExecuteUri_QueryVars_includes_timings()
	{
		// Arrange
		var (client, v) = Setup(true);

		// Act

		// Assert
		Assert.Contains("timings", client.ExecuteUri().QueryVars.AllKeys);
	}

	[Fact]
	public void QueryUri_QueryVars_includes_timings()
	{
		// Arrange
		var (client, v) = Setup(true);

		// Act

		// Assert
		Assert.Contains("timings", client.QueryUri().QueryVars.AllKeys);
	}
}


public class when_includeTimings_is_false : RqliteClientTests
{
	[Fact]
	public void ExecuteUri_QueryVars_does_not_include_timings()
	{
		// Arrange
		var (client, v) = Setup(false);

		// Act

		// Assert
		Assert.DoesNotContain("timings", client.ExecuteUri().QueryVars.AllKeys);
	}

	[Fact]
	public void QueryUri_QueryVars_does_not_include_timings()
	{
		// Arrange
		var (client, v) = Setup(false);

		// Act

		// Assert
		Assert.DoesNotContain("timings", client.QueryUri().QueryVars.AllKeys);
	}
}
