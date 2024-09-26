using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicObserverTypes.Data;

/// <summary>
/// IDを持つデータのリストを保持します。
/// </summary>
/// <typeparam name="TData"></typeparam>
public class IDDictionary<TData> : IReadOnlyDictionary<int, TData> where TData : class, IIdentifiable
{

	private readonly IDictionary<int, TData> dict;

	public IDDictionary()
		: this(new List<TData>())
	{
	}

	public IDDictionary(IEnumerable<TData> source)
	{
		dict = source.ToDictionary(x => x.ID);
	}


	public void Add(TData data)
	{
		dict.Add(data.ID, data);
	}

	public void Remove(TData data)
	{
		dict.Remove(data.ID);
	}

	public void Remove(int id)
	{
		dict.Remove(id);
	}

	public int RemoveAll(Predicate<TData> predicate)
	{
		var removekeys = dict.Values.Where(elem => predicate(elem)).Select(elem => elem.ID).ToArray();

		foreach (var key in removekeys)
		{
			dict.Remove(key);
		}

		return removekeys.Count();
	}

	public void Clear()
	{
		dict.Clear();
	}


	public bool ContainsKey(int key)
	{
		return dict.ContainsKey(key);
	}

	public IEnumerable<int> Keys => dict.Keys;

	public bool TryGetValue(int key, out TData value)
	{
		return dict.TryGetValue(key, out value);
	}

	public IEnumerable<TData> Values => dict.Values;

#pragma warning disable CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
	public TData? this[int key] => dict.ContainsKey(key) ? dict[key] : null;
#pragma warning restore CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).

	public int Count => dict.Count;

	public IEnumerator<KeyValuePair<int, TData>> GetEnumerator()
	{
		return dict.GetEnumerator();
	}

	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return dict.GetEnumerator();
	}
}
