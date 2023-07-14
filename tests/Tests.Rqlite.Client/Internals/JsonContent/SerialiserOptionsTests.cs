// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace Rqlite.Client.Internals.JsonContentTests;

public class SerialiserOptionsTests
{
	public class default_value
	{
		[Fact]
		public void PropertyNameCaseInsensitive_is_true()
		{
			// Arrange

			// Act

			// Assert
			Assert.True(JsonContent.SerialiserOptions.PropertyNameCaseInsensitive);
		}
	}
}
