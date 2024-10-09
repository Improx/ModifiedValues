using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModifiedValues;

public class TestModifyWithModifiedValue : MonoBehaviour
{
	public ModifiedFloat HP_Bad = 10;
	public ModifiedFloat MaxHP_Bad = 100;
	public ModifiedFloat HP = 10;
	public ModifiedFloat MaxHP = 100;

	private void Awake()
	{
		//This is not expected to work:
		HP_Bad.MaxCap(MaxHP_Bad);
		HP_Bad.AddDependency(MaxHP_Bad);
		Debug.Log(HP_Bad);
		MaxHP_Bad.Set(8);
		Debug.Log(HP_Bad);

		//But this is expected to work:
		HP.MaxCapDynamic(MaxHP);
		Debug.Log(HP);
		MaxHP.Set(8);
		Debug.Log(HP);
	}
}
