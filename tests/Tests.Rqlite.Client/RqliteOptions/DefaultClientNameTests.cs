// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace Rqlite.Client.RqliteOptionsTests.DefaultClientNameTests;

/// <see cref="RqliteOptions.DefaultClientName"/>

public class default_value
{
	public void is_null()
	{
		// Arrange
		var options = new RqliteOptions();

		// Act
		var result = options.DefaultClientName;

		// Assert
		Assert.Null(result);
	}
}
