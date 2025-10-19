using System.Collections;
using System.Collections.Generic;
using ModifiedValues;
using UnityEngine;

public class TestChangeModifyingAmount : MonoBehaviour
{
	private void Awake()
	{
		ModifiedFloat Speed = 10;
		ModifiedFloat Amount = 2;
		Modifier<float> mod = Speed.ModifyFromLatest((latestValue) => Mathf.Pow(latestValue, Amount));
		Speed.AddDependency(Amount);

		Debug.Log(Speed);
		Amount.BaseValue = 3;
		Debug.Log(Speed);
	}
}
