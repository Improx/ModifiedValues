using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModifiedValues;

public class TestModifyWithModifiedValue : MonoBehaviour
{
	public ModifiedFloat A = 10;
	public ModifiedFloat B = 100;
	public ModifiedBool C = false;
	public ModifiedBool D = false;

	private void Awake()
	{
		C.OrDynamic(D);
		Debug.Log(C);
		D.Set(true);
		Debug.Log(C);
	}
}
