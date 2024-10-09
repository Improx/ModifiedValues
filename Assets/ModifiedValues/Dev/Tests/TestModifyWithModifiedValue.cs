using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModifiedValues;

public class TestModifyWithModifiedValue : MonoBehaviour
{
	public ModifiedFloat A = 10;
	public ModifiedFloat B = 100;

	private void Awake()
	{
		A.MulDynamic(B);
		Debug.Log(A);
		B.Set(4);
		Debug.Log(A);
	}
}
