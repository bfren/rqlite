// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace Rqlite.Client.Internals.UriBuilderTests.BuildTests;

/// <see cref="UriBuilder.Build()"/>

public class when_called
{
	[Fact]
	public void returns_relative_uri()
	{
		// Arrange
		var builder = new UriBuilder(Rnd.Str);

		// Act
		var result = builder.Build();

		// Assert
		var uri = Assert.IsType<Uri>(result);
		Assert.False(uri.IsAbsoluteUri);
	}
}

public class with_no_QueryVars
{
	[Fact]
	public void does_not_append_query_string()
	{
		// Arrange
		var builder = new UriBuilder(Rnd.Str);

		// Act
		var result = builder.Build();

		// Assert
		Assert.DoesNotContain("?", result.OriginalString);
	}

	[Fact]
	public void returns_original_path()
	{
		// Arrange
		var path = Rnd.Str;
		var builder = new UriBuilder(path);

		// Act
		var result = builder.Build();

		// Assert
		Assert.Equal($"/{path}", result.OriginalString);
	}
}

public class with_one_QueryVar
{
	[Fact]
	public void does_not_append_amp()
	{
		// Arrange
		var builder = new UriBuilder(Rnd.Str);
		builder.AddQueryVar(Rnd.Str, Rnd.Str);

		// Act
		var result = builder.Build();

		// Assert
		Assert.DoesNotContain("&", result.OriginalString);
	}
}

public class with_QueryVars
{
	[Fact]
	public void separates_vars_with_amp()
	{
		// Arrange
		var builder = new UriBuilder(Rnd.Str);
		var key = Rnd.Str;
		builder.AddQueryVar(Rnd.Str, Rnd.Str);
		builder.AddQueryVar(key, Rnd.Str);

		// Act
		var result = builder.Build();

		// Assert
		Assert.Contains($"&{key}", result.OriginalString);
	}

	[Fact]
	public void includes_keys_that_have_no_value()
	{
		// Arrange
		var builder = new UriBuilder(Rnd.Str);
		var key = Rnd.Str;
		builder.AddQueryVar(key);
		builder.AddQueryVar(Rnd.Str, Rnd.Str);

		// Act
		var result = builder.Build();

		// Assert
		Assert.Contains($"{key}&", result.OriginalString);
	}

	[Fact]
	public void separates_keys_and_values_with_equals()
	{
		// Arrange
		var builder = new UriBuilder(Rnd.Str);
		var key = Rnd.Str;
		var value = Rnd.Str;
		builder.AddQueryVar(key, value);

		// Act
		var result = builder.Build();

		// Assert
		Assert.Contains($"{key}={value}", result.OriginalString);
	}

	[Fact]
	public void escapes_values()
	{
		// Arrange
		var builder = new UriBuilder(Rnd.Str);
		var value = $"{Rnd.Str}/ {Rnd.Int}: ?";
		builder.AddQueryVar(Rnd.Str, value);
		var expected = Uri.EscapeDataString(value);

		// Act
		var result = builder.Build();

		// Assert
		Assert.Contains(expected, result.OriginalString);
	}

	[Fact]
	public void returns_correct_relative_uri()
	{
		// Arrange
		var path = Rnd.Str;
		var builder = new UriBuilder(path);
		var k0 = Rnd.Str;
		var k1 = Rnd.Str;
		var v1 = Rnd.Str;
		var k2 = Rnd.Str;
		var v2 = $"/:{Rnd.Str}?";
		builder.AddQueryVar(k0);
		builder.AddQueryVar(k1, v1);
		builder.AddQueryVar(k2, v2);
		var expected = $"/{path}?{k0}&{k1}={v1}&{k2}={Uri.EscapeDataString(v2)}";

		// Act
		var result = builder.Build();

		// Assert
		Assert.Equal(expected, result.OriginalString);
	}
}
