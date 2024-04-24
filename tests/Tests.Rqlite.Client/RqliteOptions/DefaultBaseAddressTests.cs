// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace Rqlite.Client.RqliteOptionsTests.DefaultBaseAddressTests;

/// <see cref="RqliteOptions.BaseAddress"/>

public class default_value
{
	public void is_localhost_4001()
	{
		// Arrange
		var options = new RqliteOptions();
		var url = "http://localhost:4001";

		// Act
		var result = options.BaseAddress;

		// Assert
		Assert.Equal(url, result);
	}
}
