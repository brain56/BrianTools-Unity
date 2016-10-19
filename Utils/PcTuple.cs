using UnityEngine;
using System.Collections;

public struct PcTuple <T,U>{

	public T x;
	public U y;

	public PcTuple(T x, U y)
	{
		this.x = x;
		this.y = y;
	}

}
