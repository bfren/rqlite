// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace Rqlite.Client.RqliteOptionsTests.ClientTests.BaseAddressTests;

/// <see cref="RqliteOptions.Client.BaseAddress"/>

public class default_value
{
	[Fact]
	public void is_rqlite_default()
	{
		// Arrange
		var client = new RqliteOptions.Client();
		var rqliteDefaultAddress = "http://localhost:4001";

		// Act
		var result = client.BaseAddress;

		// Assert
		Assert.Equal(rqliteDefaultAddress, result);
	}
}
