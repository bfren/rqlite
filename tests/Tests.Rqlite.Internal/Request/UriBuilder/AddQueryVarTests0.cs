// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace Rqlite.Internal.Request.UriBuilderTests.AddQueryVarTests;

/// <see cref="UriBuilder.AddQueryVar(string)"/>

public class with_key
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
		var single = Assert.Single(builder.QueryVars.AllKeys);
		Assert.Equal(key, single);
		Assert.Null(builder.QueryVars[key]);
	}
}
