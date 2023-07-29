// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Rqlite.Client.Response;

namespace Rqlite.Client.RqliteQueryResponseExtensionsTests.FlattenTests;

public class when_error_count_is_greater_than_zero
{
	[Fact]
	public async Task returns_empty_list()
	{
		// Arrange
		var response = new RqliteQueryResponse<int>(Rnd.Str);

		// Act
		var r0 = response.Flatten();
		var r1 = await Task.FromResult(response).Flatten();

		// Assert
		Assert.Empty(r0);
		Assert.Empty(r1);
	}
}

public class when_rows_is_null
{
	[Fact]
	public async Task returns_empty_list()
	{
		// Arrange
		var response = new RqliteQueryResponse<int>();

		// Act
		var r0 = response.Flatten();
		var r1 = await Task.FromResult(response).Flatten();

		// Assert
		Assert.Empty(r0);
		Assert.Empty(r1);
	}
}

public class when_rows_is_not_null
{
	[Fact]
	public async Task selects_many_rows()
	{
		// Arrange
		var (v0, v1, v2, v3, v4) = (Rnd.Int, Rnd.Int, Rnd.Int, Rnd.Int, Rnd.Int);
		var response = new RqliteQueryResponse<int>
		{
			Results = new()
			{
				new() { Rows = new() { v0, v1} },
				new() { Rows = new() { v2, v3, v4 } }
			}
		};

		// Act
		var r0 = response.Flatten();
		var r1 = await Task.FromResult(response).Flatten();

		// Assert
		Assert.Collection(r0,
			x => Assert.Equal(v0, x),
			x => Assert.Equal(v1, x),
			x => Assert.Equal(v2, x),
			x => Assert.Equal(v3, x),
			x => Assert.Equal(v4, x)
		);

		Assert.Collection(r1,
			x => Assert.Equal(v0, x),
			x => Assert.Equal(v1, x),
			x => Assert.Equal(v2, x),
			x => Assert.Equal(v3, x),
			x => Assert.Equal(v4, x)
		);
	}
}
