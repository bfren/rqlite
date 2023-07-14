// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace Rqlite.Client.RqliteOptionsTests;

public class IncludeTimingsTests
{
	public class default_value
	{
		public void is_true()
		{
			// Arrange
			var options = new RqliteOptions();

			// Act
			var result = options.IncludeTimings;

			// Assert
			Assert.True(result);
		}
	}
}