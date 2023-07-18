// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace Rqlite.Client.RqliteOptionsTests.ClientsTests;

/// <see cref="RqliteOptions.Clients"/>

public class default_value
{
	public void is_empty()
	{
		// Arrange
		var options = new RqliteOptions();

		// Act
		var result = options.Clients;

		// Assert
		Assert.Empty(result);
	}
}
