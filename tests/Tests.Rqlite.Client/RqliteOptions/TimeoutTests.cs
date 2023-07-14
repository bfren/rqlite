// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace Rqlite.Client.RqliteOptionsTests;

public class TimeoutTests
{
	public class default_value
	{
		[Fact]
		public void is_thirty_seconds()
		{
			// Arrange
			var thirtySeconds = 30;
			var options = new RqliteOptions();

			// Act
			var result = options.TimeoutInSeconds;

			// Assert
			Assert.Equal(thirtySeconds, result);
		}
	}
}
