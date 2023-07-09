// Maybe: Rqlite Client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;

namespace Rqlite.Client.Exceptions;

/// <summary>Thrown when <see cref="RqliteClientFactory"/> cannot find the specified client.</summary>
public sealed class UnknownClientException : Exception
{
	/// <summary>Create blank exception.</summary>
	public UnknownClientException() { }

	/// <summary>Create exception with message.</summary>
	/// <param name="message"></param>
	public UnknownClientException(string message) : base(message) { }

	/// <summary>Create exception with message and inner exception.</summary>
	/// <param name="message"></param>
	/// <param name="inner"></param>
	public UnknownClientException(string message, Exception inner) : base(message, inner) { }
}
