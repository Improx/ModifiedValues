using System.Collections;
using System.Collections.Generic;
using ModifiedValues;
using UnityEngine;

public class TestModifiedValueT : MonoBehaviour
{
	public ModifiedValue<Vector3> VecTest = Vector3.one;
	public ModifiedValue<TestEnum> EnumTest;

}

public enum TestEnum { A, B, C }