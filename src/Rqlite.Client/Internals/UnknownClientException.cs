// Maybe: Rqlite Client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;

namespace Rqlite.Client.Internals;

/// <summary>Thrown when <see cref="RqliteClientFactory"/> cannot find the specified client.</summary>
internal sealed class UnknownClientException : Exception
{
	/// <summary>Create blank exception.</summary>
	internal UnknownClientException() { }

	/// <summary>Create exception with message.</summary>
	/// <param name="message"></param>
	internal UnknownClientException(string message) : base(message) { }

	/// <summary>Create exception with message and inner exception.</summary>
	/// <param name="message"></param>
	/// <param name="inner"></param>
	internal UnknownClientException(string message, Exception inner) : base(message, inner) { }
}
