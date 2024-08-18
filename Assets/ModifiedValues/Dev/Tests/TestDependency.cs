using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModifiedValues.Dev.Tests
{
	public class TestDependency : MonoBehaviour
	{
		public ModifiedFloat GeneralSpeed = 10;
		public ModifiedFloat AttackSpeed;
		public ModifiedFloat MoveSpeed;

		private void Awake()
		{
			AttackSpeed = new ModifiedFloat(() => GeneralSpeed, GeneralSpeed);
			MoveSpeed = new ModifiedFloat(() => GeneralSpeed, GeneralSpeed);

			Debug.Log("<color=green>Initial values:</color>");
			Debug.Log("AttackSpeed speed:" + AttackSpeed);
			Debug.Log("MoveSpeed speed:" + MoveSpeed);
			Debug.Log("General speed:" + GeneralSpeed);

			Debug.Log("Adding dirty subscriptions.");

			//Subscribing to BecameDirty events, like a UI would
			GeneralSpeed.BecameDirty += (s, e) => Debug.Log("Just modded General speed: " + GeneralSpeed);
			AttackSpeed.BecameDirty += (s, e) => Debug.Log("Just modded Attack speed: " + AttackSpeed);
			MoveSpeed.BecameDirty += (s, e) => Debug.Log("Just modded Move speed: " + MoveSpeed);

			Debug.Log("<color=green>Modifying</color>");

			GeneralSpeed.Add(5);

			Debug.Log("<color=green>Testing that a disposed ModifiedValues ubsibscribes from dependency, by setting a new object:</color>");
			AttackSpeed = new ModifiedFloat(() => GeneralSpeed, GeneralSpeed);
			AttackSpeed.BecameDirty += (s, e) => Debug.Log("Just modded Attack speed: " + AttackSpeed);

			GeneralSpeed.Add(5);
		}
	}
}