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
		var content = new JsonContent(new { v0, v1 });
		var expected = $"{{\"v0\":\"{v0}\",\"v1\":{v1}}}";

		// Act
		var result = content.ReadAsStringAsync().Result;

		// Assert
		Assert.Equal(expected, result);
	}

	[Fact]
	public void uses_utf8_charset()
	{
		// Arrange
		var content = new JsonContent(Rnd.Str);

		// Act
		var result = content.Headers.ContentType?.CharSet;

		// Assert
		Assert.Equal("utf-8", result);
	}

	[Fact]
	public void uses_json_media_type()
	{
		// Arrange
		var content = new JsonContent(Rnd.Str);

		// Act
		var result = content.Headers.ContentType?.MediaType;

		// Assert
		Assert.Equal("application/json", result);
	}
}
