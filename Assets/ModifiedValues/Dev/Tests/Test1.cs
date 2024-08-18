using System.Collections;
using System.Collections.Generic;
using ModifiedValues;
using UnityEngine;

namespace ModifiedValues.Dev.Tests
{
	public class Test1 : MonoBehaviour
	{
		public ModifiedFloat Speed;
		public ModifiedFloat Lol = 5;
		public int MyInt;
		public ModifiedInt LolInt = 2;
		public ModifiedFloat Lol2 = 6;
		public ModifiedFloat Lol3 = 6;
		public ModifiedFloat Lol4 = 6;
		public ModifiedFloat Lol5 = new ModifiedFloat(() => 3);
		public ModifiedFloat Lol6 = 6;
		public ModifiedFloat Lol7 = new ModifiedFloat(() => 3.234523452345f);
		public ModifiedFloat Lol8 = 6;
		public ModifiedFloat Lol9 = 10;

		private void Awake()
		{
			Speed = new ModifiedFloat(() => Lol);
			Debug.Log(Lol);
			Speed.Mul(2.7f);

			//Lol9.Modify((v, lv) => v + lv * 0.2f);
			Lol9.ModifyFromLatest(v => v + Lol9.BaseValue * 0.2f);
			Lol9.BaseValue = 20;
		}
	}
}