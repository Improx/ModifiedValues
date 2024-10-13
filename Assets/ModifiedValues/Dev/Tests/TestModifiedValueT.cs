using System.Collections;
using System.Collections.Generic;
using ModifiedValues;
using UnityEngine;

public class TestModifiedValueT : MonoBehaviour
{
	public ModifiedValue<Vector3> VecTest = Vector3.one;
	public ModifiedValue<TestEnum> EnumTest = TestEnum.A;
	public ModifiedFloat FloatTest = 0;

	private void Awake()
	{
		FloatTest.Set(1);
	}

}

public enum TestEnum { A, B, C }