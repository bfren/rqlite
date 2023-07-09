// Maybe: Rqlite Client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;

namespace Rqlite.Client.Exceptions;

/// <summary>Thrown when <see cref="RqliteClientFactory"/> cannot determine the default named instance.</summary>
public sealed class UndefinedDefaultClientException : Exception
{
	/// <summary>Create blank exception.</summary>
	public UndefinedDefaultClientException() { }

	/// <summary>Create exception with message.</summary>
	/// <param name="message"></param>
	public UndefinedDefaultClientException(string message) : base(message) { }

	/// <summary>Create exception with message and inner exception.</summary>
	/// <param name="message"></param>
	/// <param name="inner"></param>
	public UndefinedDefaultClientException(string message, Exception inner) : base(message, inner) { }
}
