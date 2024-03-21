// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace Rqlite.Internal.Request.UriBuilderTests.AddQueryVarTests;

/// <see cref="UriBuilder.AddQueryVar(string, string)"/>

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
		var single = Assert.Single(builder.QueryVars.AllKeys);
		Assert.Equal(key, single);
		Assert.Equal(value, builder.QueryVars[key]);
	}
}
