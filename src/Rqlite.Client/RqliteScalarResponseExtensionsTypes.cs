// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Text.Json;
using System.Threading.Tasks;
using Rqlite.Client.Response;

namespace Rqlite.Client;

public static partial class RqliteScalarResponseExtensions
{
	/// <summary>
	/// Flatten <paramref name="response"/> and return the first <see cref="byte"/> or <see cref="byte.MinValue"/>.
	/// </summary>
	/// <remarks>Warning: any errors will be ignored and an empty list returned instead!</remarks>
	/// <param name="response">RqliteGetScalarResponse object.</param>
	/// <returns>First <see cref="byte"/> or <see cref="byte.MinValue"/>.</returns>
	public static byte Flatten(this RqliteScalarResponse<byte> response) =>
		Flatten(response, (JsonElement e, out byte v) => e.TryGetByte(out v), byte.MinValue);

	/// <inheritdoc cref="Flatten(RqliteGetScalarResponse{byte})"/>
	public static async Task<byte> Flatten(this Task<RqliteScalarResponse<byte>> response) =>
		Flatten(await response);

	/// <summary>
	/// Flatten <paramref name="response"/> and return the first <see cref="byte[]?"/> or <see cref="Array.Empty<byte>()"/>.
	/// </summary>
	/// <remarks>Warning: any errors will be ignored and an empty list returned instead!</remarks>
	/// <param name="response">RqliteGetScalarResponse object.</param>
	/// <returns>First <see cref="byte[]?"/> or <see cref="Array.Empty<byte>()"/>.</returns>
	public static byte[]? Flatten(this RqliteScalarResponse<byte[]?> response) =>
		Flatten(response, (JsonElement e, out byte[]? v) => e.TryGetBytesFromBase64(out v), Array.Empty<byte>());

	/// <inheritdoc cref="Flatten(RqliteGetScalarResponse{byte[]?})"/>
	public static async Task<byte[]?> Flatten(this Task<RqliteScalarResponse<byte[]?>> response) =>
		Flatten(await response);

	/// <summary>
	/// Flatten <paramref name="response"/> and return the first <see cref="DateTime"/> or <see cref="DateTime.MinValue"/>.
	/// </summary>
	/// <remarks>Warning: any errors will be ignored and an empty list returned instead!</remarks>
	/// <param name="response">RqliteGetScalarResponse object.</param>
	/// <returns>First <see cref="DateTime"/> or <see cref="DateTime.MinValue"/>.</returns>
	public static DateTime Flatten(this RqliteScalarResponse<DateTime> response) =>
		Flatten(response, (JsonElement e, out DateTime v) => e.TryGetDateTime(out v), DateTime.MinValue);

	/// <inheritdoc cref="Flatten(RqliteGetScalarResponse{DateTime})"/>
	public static async Task<DateTime> Flatten(this Task<RqliteScalarResponse<DateTime>> response) =>
		Flatten(await response);

	/// <summary>
	/// Flatten <paramref name="response"/> and return the first <see cref="DateTimeOffset"/> or <see cref="DateTimeOffset.MinValue"/>.
	/// </summary>
	/// <remarks>Warning: any errors will be ignored and an empty list returned instead!</remarks>
	/// <param name="response">RqliteGetScalarResponse object.</param>
	/// <returns>First <see cref="DateTimeOffset"/> or <see cref="DateTimeOffset.MinValue"/>.</returns>
	public static DateTimeOffset Flatten(this RqliteScalarResponse<DateTimeOffset> response) =>
		Flatten(response, (JsonElement e, out DateTimeOffset v) => e.TryGetDateTimeOffset(out v), DateTimeOffset.MinValue);

	/// <inheritdoc cref="Flatten(RqliteGetScalarResponse{DateTimeOffset})"/>
	public static async Task<DateTimeOffset> Flatten(this Task<RqliteScalarResponse<DateTimeOffset>> response) =>
		Flatten(await response);

	/// <summary>
	/// Flatten <paramref name="response"/> and return the first <see cref="decimal"/> or <see cref="0"/>.
	/// </summary>
	/// <remarks>Warning: any errors will be ignored and an empty list returned instead!</remarks>
	/// <param name="response">RqliteGetScalarResponse object.</param>
	/// <returns>First <see cref="decimal"/> or <see cref="0"/>.</returns>
	public static decimal Flatten(this RqliteScalarResponse<decimal> response) =>
		Flatten(response, (JsonElement e, out decimal v) => e.TryGetDecimal(out v), 0);

	/// <inheritdoc cref="Flatten(RqliteGetScalarResponse{decimal})"/>
	public static async Task<decimal> Flatten(this Task<RqliteScalarResponse<decimal>> response) =>
		Flatten(await response);

	/// <summary>
	/// Flatten <paramref name="response"/> and return the first <see cref="double"/> or <see cref="0D"/>.
	/// </summary>
	/// <remarks>Warning: any errors will be ignored and an empty list returned instead!</remarks>
	/// <param name="response">RqliteGetScalarResponse object.</param>
	/// <returns>First <see cref="double"/> or <see cref="0D"/>.</returns>
	public static double Flatten(this RqliteScalarResponse<double> response) =>
		Flatten(response, (JsonElement e, out double v) => e.TryGetDouble(out v), 0D);

	/// <inheritdoc cref="Flatten(RqliteGetScalarResponse{double})"/>
	public static async Task<double> Flatten(this Task<RqliteScalarResponse<double>> response) =>
		Flatten(await response);

	/// <summary>
	/// Flatten <paramref name="response"/> and return the first <see cref="Guid"/> or <see cref="Guid.Empty"/>.
	/// </summary>
	/// <remarks>Warning: any errors will be ignored and an empty list returned instead!</remarks>
	/// <param name="response">RqliteGetScalarResponse object.</param>
	/// <returns>First <see cref="Guid"/> or <see cref="Guid.Empty"/>.</returns>
	public static Guid Flatten(this RqliteScalarResponse<Guid> response) =>
		Flatten(response, (JsonElement e, out Guid v) => e.TryGetGuid(out v), Guid.Empty);

	/// <inheritdoc cref="Flatten(RqliteGetScalarResponse{Guid})"/>
	public static async Task<Guid> Flatten(this Task<RqliteScalarResponse<Guid>> response) =>
		Flatten(await response);

	/// <summary>
	/// Flatten <paramref name="response"/> and return the first <see cref="short"/> or <see cref="(short)0"/>.
	/// </summary>
	/// <remarks>Warning: any errors will be ignored and an empty list returned instead!</remarks>
	/// <param name="response">RqliteGetScalarResponse object.</param>
	/// <returns>First <see cref="short"/> or <see cref="(short)0"/>.</returns>
	public static short Flatten(this RqliteScalarResponse<short> response) =>
		Flatten(response, (JsonElement e, out short v) => e.TryGetInt16(out v), (short)0);

	/// <inheritdoc cref="Flatten(RqliteGetScalarResponse{short})"/>
	public static async Task<short> Flatten(this Task<RqliteScalarResponse<short>> response) =>
		Flatten(await response);

	/// <summary>
	/// Flatten <paramref name="response"/> and return the first <see cref="int"/> or <see cref="0"/>.
	/// </summary>
	/// <remarks>Warning: any errors will be ignored and an empty list returned instead!</remarks>
	/// <param name="response">RqliteGetScalarResponse object.</param>
	/// <returns>First <see cref="int"/> or <see cref="0"/>.</returns>
	public static int Flatten(this RqliteScalarResponse<int> response) =>
		Flatten(response, (JsonElement e, out int v) => e.TryGetInt32(out v), 0);

	/// <inheritdoc cref="Flatten(RqliteGetScalarResponse{int})"/>
	public static async Task<int> Flatten(this Task<RqliteScalarResponse<int>> response) =>
		Flatten(await response);

	/// <summary>
	/// Flatten <paramref name="response"/> and return the first <see cref="long"/> or <see cref="0L"/>.
	/// </summary>
	/// <remarks>Warning: any errors will be ignored and an empty list returned instead!</remarks>
	/// <param name="response">RqliteGetScalarResponse object.</param>
	/// <returns>First <see cref="long"/> or <see cref="0L"/>.</returns>
	public static long Flatten(this RqliteScalarResponse<long> response) =>
		Flatten(response, (JsonElement e, out long v) => e.TryGetInt64(out v), 0L);

	/// <inheritdoc cref="Flatten(RqliteGetScalarResponse{long})"/>
	public static async Task<long> Flatten(this Task<RqliteScalarResponse<long>> response) =>
		Flatten(await response);

	/// <summary>
	/// Flatten <paramref name="response"/> and return the first <see cref="sbyte"/> or <see cref="(sbyte)0"/>.
	/// </summary>
	/// <remarks>Warning: any errors will be ignored and an empty list returned instead!</remarks>
	/// <param name="response">RqliteGetScalarResponse object.</param>
	/// <returns>First <see cref="sbyte"/> or <see cref="(sbyte)0"/>.</returns>
	public static sbyte Flatten(this RqliteScalarResponse<sbyte> response) =>
		Flatten(response, (JsonElement e, out sbyte v) => e.TryGetSByte(out v), (sbyte)0);

	/// <inheritdoc cref="Flatten(RqliteGetScalarResponse{sbyte})"/>
	public static async Task<sbyte> Flatten(this Task<RqliteScalarResponse<sbyte>> response) =>
		Flatten(await response);

	/// <summary>
	/// Flatten <paramref name="response"/> and return the first <see cref="float"/> or <see cref="0F"/>.
	/// </summary>
	/// <remarks>Warning: any errors will be ignored and an empty list returned instead!</remarks>
	/// <param name="response">RqliteGetScalarResponse object.</param>
	/// <returns>First <see cref="float"/> or <see cref="0F"/>.</returns>
	public static float Flatten(this RqliteScalarResponse<float> response) =>
		Flatten(response, (JsonElement e, out float v) => e.TryGetSingle(out v), 0F);

	/// <inheritdoc cref="Flatten(RqliteGetScalarResponse{float})"/>
	public static async Task<float> Flatten(this Task<RqliteScalarResponse<float>> response) =>
		Flatten(await response);

	/// <summary>
	/// Flatten <paramref name="response"/> and return the first <see cref="ushort"/> or <see cref="(ushort)0"/>.
	/// </summary>
	/// <remarks>Warning: any errors will be ignored and an empty list returned instead!</remarks>
	/// <param name="response">RqliteGetScalarResponse object.</param>
	/// <returns>First <see cref="ushort"/> or <see cref="(ushort)0"/>.</returns>
	public static ushort Flatten(this RqliteScalarResponse<ushort> response) =>
		Flatten(response, (JsonElement e, out ushort v) => e.TryGetUInt16(out v), (ushort)0);

	/// <inheritdoc cref="Flatten(RqliteGetScalarResponse{ushort})"/>
	public static async Task<ushort> Flatten(this Task<RqliteScalarResponse<ushort>> response) =>
		Flatten(await response);

	/// <summary>
	/// Flatten <paramref name="response"/> and return the first <see cref="uint"/> or <see cref="0U"/>.
	/// </summary>
	/// <remarks>Warning: any errors will be ignored and an empty list returned instead!</remarks>
	/// <param name="response">RqliteGetScalarResponse object.</param>
	/// <returns>First <see cref="uint"/> or <see cref="0U"/>.</returns>
	public static uint Flatten(this RqliteScalarResponse<uint> response) =>
		Flatten(response, (JsonElement e, out uint v) => e.TryGetUInt32(out v), 0U);

	/// <inheritdoc cref="Flatten(RqliteGetScalarResponse{uint})"/>
	public static async Task<uint> Flatten(this Task<RqliteScalarResponse<uint>> response) =>
		Flatten(await response);

	/// <summary>
	/// Flatten <paramref name="response"/> and return the first <see cref="ulong"/> or <see cref="0UL"/>.
	/// </summary>
	/// <remarks>Warning: any errors will be ignored and an empty list returned instead!</remarks>
	/// <param name="response">RqliteGetScalarResponse object.</param>
	/// <returns>First <see cref="ulong"/> or <see cref="0UL"/>.</returns>
	public static ulong Flatten(this RqliteScalarResponse<ulong> response) =>
		Flatten(response, (JsonElement e, out ulong v) => e.TryGetUInt64(out v), 0UL);

	/// <inheritdoc cref="Flatten(RqliteGetScalarResponse{ulong})"/>
	public static async Task<ulong> Flatten(this Task<RqliteScalarResponse<ulong>> response) =>
		Flatten(await response);

}