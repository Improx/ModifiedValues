using System.Collections;
using System.Collections.Generic;
using ModifiedValues;
using UnityEngine;

namespace ModifiedValues.Dev.Tests
{
	public class TestList : MonoBehaviour
	{
		public List<ModifiedFloat> Stats = new();

		private void Awake()
		{
			Stats.Add(new ModifiedFloat(5));
		}
	}
}