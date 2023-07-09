// Maybe: Rqlite Client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;

namespace Rqlite.Client.Internals;

/// <summary>Thrown when <see cref="RqliteClientFactory"/> cannot determine the default named instance.</summary>
internal sealed class UndefinedDefaultClientException : Exception
{
	/// <summary>Create blank exception.</summary>
	internal UndefinedDefaultClientException() { }

	/// <summary>Create exception with message.</summary>
	/// <param name="message"></param>
	internal UndefinedDefaultClientException(string message) : base(message) { }

	/// <summary>Create exception with message and inner exception.</summary>
	/// <param name="message"></param>
	/// <param name="inner"></param>
	internal UndefinedDefaultClientException(string message, Exception inner) : base(message, inner) { }
}
