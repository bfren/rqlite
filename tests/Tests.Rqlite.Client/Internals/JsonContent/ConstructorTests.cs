// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace Rqlite.Client.Internals.JsonContentTests;

public class ConstructorTests
{
	[Fact]
	public void seralises_content_as_json()
	{
		// Arrange
		var v0 = Rnd.Str;
		var v1 = Rnd.Int;
		var expected = $"{{\"v0\":\"{v0}\",\"v1\":{v1}}}";

		// Act
		var result = new JsonContent(new { v0, v1 });

		// Assert
		Assert.Equal(expected, result.ReadAsStringAsync().Result);
	}

	[Fact]
	public void uses_utf8_charset()
	{
		// Arrange

		// Act
		var result = new JsonContent(Rnd.Str);

		// Assert
		Assert.Equal("utf-8", result.Headers.ContentType?.CharSet);
	}

	[Fact]
	public void uses_json_media_type()
	{
		// Arrange

		// Act
		var result = new JsonContent(Rnd.Str);

		// Assert
		Assert.Equal("application/json", result.Headers.ContentType?.MediaType);
	}
}
