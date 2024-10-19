using System;
using System.Collections;
using System.Collections.Generic;
using ModifiedValues;
using UnityEngine;

public class TestDirty : MonoBehaviour
{
	ModifiedBool TestBool = false;
	ModifiedFloat TestFloat = 0;

	private void Awake()
	{
		TestBool.BecameDirty += BecameDirtyHandler;
		TestFloat.BecameDirty += BecameDirtyHandler;
		TestBool.Set(true);
		TestFloat.Set(1);
	}

	private void BecameDirtyHandler(object sender, EventArgs e)
	{
		Debug.Log(TestBool);
		Debug.Log(TestFloat);
	}
}
