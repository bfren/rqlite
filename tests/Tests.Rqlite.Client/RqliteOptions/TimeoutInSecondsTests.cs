// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace Rqlite.Client.RqliteOptionsTests.TimeoutInSecondsTests;

/// <see cref="RqliteOptions.TimeoutInSeconds"/>

public class default_value
{
	[Fact]
	public void is_thirty_seconds()
	{
		// Arrange
		var options = new RqliteOptions();
		var thirtySeconds = 30;

		// Act
		var result = options.TimeoutInSeconds;

		// Assert
		Assert.Equal(thirtySeconds, result);
	}
}
