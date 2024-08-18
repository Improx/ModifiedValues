using System.Collections;
using System.Collections.Generic;
using ModifiedValues;
using UnityEngine;

namespace ModifiedValues.Dev.Tests
{
	public class TestList : MonoBehaviour
	{
		public ModifiedFloat Haha = 5;
		public ModifiedFloat Haha2;
		public List<ModifiedFloat> Stats = new();

		private void Awake()
		{
			Haha2 = Haha;

			Stats.Add(new ModifiedFloat(5));
			Stats.Add(Haha);
		}
	}
}