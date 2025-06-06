// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Rqlite.Client;

/// <summary>
/// <see cref="ServiceCollection"/> extension methods.
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Register Rqlite classes and options.
	/// </summary>
	/// <param name="this">IServiceCollection instance.</param>
	/// <returns>Configurred IServiceCollection instance.</returns>
	public static IServiceCollection AddRqlite(this IServiceCollection @this)
	{
		// Register Rqlite classes
		_ = @this.AddSingleton<IRqliteClientFactory, RqliteClientFactory>();
		_ = @this.AddOptions<RqliteOptions>().BindConfiguration("Rqlite");

		// Add configured HttpClients
		using var provider = @this.BuildServiceProvider();
		var rqliteOptions = provider.GetRequiredService<IOptions<RqliteOptions>>().Value;
		foreach (var (name, connection) in rqliteOptions.Clients)
		{
			_ = @this.AddHttpClient(name, opt =>
			{
				opt.BaseAddress = new(connection.BaseAddress ?? rqliteOptions.BaseAddress);
				opt.Timeout = TimeSpan.FromSeconds(connection.TimeoutInSeconds ?? rqliteOptions.TimeoutInSeconds);

				if (!string.IsNullOrEmpty(connection.AuthString))
				{
					var encodedAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes(connection.AuthString));
					opt.DefaultRequestHeaders.Add("Authorization", encodedAuth);
				}
			});
		}

		return @this;
	}
}
