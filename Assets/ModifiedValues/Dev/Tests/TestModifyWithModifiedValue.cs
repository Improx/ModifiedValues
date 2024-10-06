using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModifiedValues;

public class TestModifyWithModifiedValue : MonoBehaviour
{
	public ModifiedFloat HP = 10;
	public ModifiedFloat MaxHP = 100;

	private void Awake()
	{
		HP.MaxCap(MaxHP);
		HP.AddDependency(MaxHP);
		Debug.Log(HP);
		MaxHP.Set(8);
		Debug.Log(HP);
	}
}
