using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BrianGoapData 
{
	public List<BrianGoapDataTuple> Data = new List<BrianGoapDataTuple>();

	[System.Serializable]
	public class BrianGoapDataTuple
	{
		public BrianGoapDataTuple(string key, bool value)
		{
			Key = key;
			Value = value;
		}

		public string Key;
		public bool Value;
	}

	public bool this[string key]
	{
		get
		{
			foreach(var tuple in Data)
			{
				if(tuple.Key == key)
				{
					return tuple.Value;
				}
			}
			throw new KeyNotFoundException();
		}
		set
		{
			foreach (var tuple in Data)
			{
				if (tuple.Key == key)
				{
					tuple.Value = value;
					return;
				}
			}
			Data.Add(new BrianGoapDataTuple(key, value));
		}
	}

	public bool ContainsKey(string key)
	{
		foreach (var tuple in Data)
		{
			if (tuple.Key == key)
			{
				return true;
			}
		}
		return false;
	}
}

