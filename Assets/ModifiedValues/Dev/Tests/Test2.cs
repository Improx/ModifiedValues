using System.Collections;
using System.Collections.Generic;
using ModifiedValues;
using UnityEngine;

namespace ModifiedValues.Dev.Tests
{
	public class Test2 : MonoBehaviour
	{
		public ModifiedFloat UnInit;
		private void Awake()
		{
			var test = new NonSerializedHolder();
			//float value = test.Speed;
			//Debug.Log(value);
			float unInitValue = UnInit;
			Debug.Log(UnInit);
		}
	}

	public class NonSerializedHolder
	{
		public ModifiedFloat Speed;

	}
}