// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace Rqlite.Client.Internals.UriBuilderTests;

public class ConstructorTests
{
	[Fact]
	public void saves_path()
	{
		// Arrange
		var path = Rnd.Str;

		// Act
		var result = new UriBuilder(path);

		// Assert
		Assert.Equal(path, result.Path);
	}

	[Fact]
	public void trims_slash_from_start()
	{
		// Arrange
		var path = Rnd.Str;

		// Act
		var result = new UriBuilder($"/{path}");

		// Assert
		Assert.Equal(path, result.Path);
	}

	public class when_includeTimings_is_true
	{
		[Fact]
		public void adds_timings_to_QueryVars()
		{
			// Arrange

			// Act
			var result = new UriBuilder(Rnd.Str, true);

			// Assert
			Assert.Collection(result.QueryVars.AllKeys,
				x => Assert.Equal("timings", x)
			);
		}
	}
}
