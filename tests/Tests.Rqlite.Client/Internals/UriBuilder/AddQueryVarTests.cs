// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace Rqlite.Client.Internals.UriBuilderTests;

public class AddQueryVarTests
{
	public class with_key_only
	{
		[Fact]
		public void adds_key_to_QueryVars_with_null_value()
		{
			// Arrange
			var key = Rnd.Str;
			var builder = new UriBuilder(Rnd.Str);

			// Act
			builder.AddQueryVar(key);

			// Assert
			Assert.Collection(builder.QueryVars.AllKeys,
				x => Assert.Equal(key, x)
			);
			Assert.Null(builder.QueryVars[key]);
		}
	}

	public class with_key_and_value
	{
		[Fact]
		public void adds_key_to_QueryVars_with_value()
		{
			// Arrange
			var key = Rnd.Str;
			var value = Rnd.Str;
			var builder = new UriBuilder(Rnd.Str);

			// Act
			builder.AddQueryVar(key, value);

			// Assert
			Assert.Collection(builder.QueryVars.AllKeys,
				x => Assert.Equal(key, x)
			);
			Assert.Equal(value, builder.QueryVars[key]);
		}
	}
}
