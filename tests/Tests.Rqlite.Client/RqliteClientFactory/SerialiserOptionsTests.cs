// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace Rqlite.Client.RqliteClientFactoryTests.SerialiserOptionsTests;

/// <see cref="JsonContent.SerialiserOptions"/>

public class default_value : RqliteClientFactoryTests
{
	[Fact]
	public void PropertyNameCaseInsensitive_is_true()
	{
		// Arrange
		var (factory, _) = Setup();

		// Act
		var result = factory.JsonOptions;

		// Assert
		Assert.True(result.PropertyNameCaseInsensitive);
	}
}
