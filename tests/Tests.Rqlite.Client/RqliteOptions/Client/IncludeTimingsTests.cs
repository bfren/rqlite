// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace Rqlite.Client.RqliteOptionsTests.ClientTests.IncludeTimingsTests;

/// <see cref="RqliteOptions.Client.IncludeTimings"/>

public class default_value
{
	[Fact]
	public void is_null()
	{
		// Arrange
		var client = new RqliteOptions.Client();

		// Act
		var result = client.IncludeTimings;

		// Assert
		Assert.Null(result);
	}
}
