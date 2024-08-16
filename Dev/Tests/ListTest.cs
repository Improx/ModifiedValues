using System.Collections;
using System.Collections.Generic;
using ModifiedValues;
using UnityEngine;

public class ListTest : MonoBehaviour
{
	public List<ModifiedFloat> Stats = new();

	private void Awake()
	{
		Stats.Add(new ModifiedFloat(5));
	}
}
