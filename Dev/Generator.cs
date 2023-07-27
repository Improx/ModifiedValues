using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ModifiedValues.Dev
{
	public static class Generator
	{
		[MenuItem("Tools/ModifiedValues/Generate classes and drawers")]
		public static void Generate()
		{
			GenerateClasses();
			GenerateDrawers();
			Debug.Log("Generated ModifiedValues classes and drawers.");
			AssetDatabase.Refresh();
		}

		/// <summary>
		/// Generates almost all Modified<TYPE> classes.
		/// Continuous numbers based on float and discrte numbers based on uint
		/// bool and Enum not generated because they don't have enough similarities
		/// </summary>
		private static void GenerateClasses()
		{
			GenerateContinuousNumberClasses();
			GenerateDiscreteNumberClasses();
		}

		private static void GenerateContinuousNumberClasses()
		{
			List<string> types = new List<string>
			{
				"Decimal",
				"Double"
			};

			string sourceFile = "Assets/ModifiedValues/Runtime/ModifiedFloat.cs";
			foreach (string type in types)
			{
				string destinationFile = $"Assets/ModifiedValues/Runtime//Modified{type}.cs";
				try
				{
					File.Copy(sourceFile, destinationFile, true);
				}
				catch (IOException e)
				{
					Debug.Log(e.Message);
				}
				string text = File.ReadAllText(destinationFile);
				text = text.Replace("Float", type);
				text = text.Replace("float", type.ToLower());
				File.WriteAllText(destinationFile, text);
			}
		}

		private static void GenerateDiscreteNumberClasses()
		{
			List<string> types = new List<string>
			{
				"Int",
				"Long",
				"Ulong"
			};

			string sourceFile = "Assets/ModifiedValues/Runtime/ModifiedUint.cs";
			foreach (string type in types)
			{
				string destinationFile = $"Assets/ModifiedValues/Runtime//Modified{type}.cs";
				try
				{
					File.Copy(sourceFile, destinationFile, true);
				}
				catch (IOException e)
				{
					Debug.Log(e.Message);
				}
				string text = File.ReadAllText(destinationFile);
				text = text.Replace("Uint", type);
				text = text.Replace("uint", type.ToLower());
				File.WriteAllText(destinationFile, text);
			}
		}

		/// <summary>
		/// Generates all Modified<TYPE>PropertyDrawer classes
		/// based on ModifiedFloatPropertyDrawer
		/// </summary>
		private static void GenerateDrawers()
		{
			List<string> types = new List<string>
			{
				"Bool",
				"Decimal",
				"Double",
				"Int",
				"Long",
				"Uint",
				"Ulong"
			};
			//Enum not included because ModifiedEnum<T> is a generic
			//type, and Unity can't make generic drawers

			string sourceFile = "Assets/ModifiedValues/Editor/ModifiedFloatPropertyDrawer.cs";
			foreach (string type in types)
			{
				string destinationFile = $"Assets/ModifiedValues/Editor/Modified{type}PropertyDrawer.cs";
				try
				{
					File.Copy(sourceFile, destinationFile, true);
				}
				catch (IOException e)
				{
					Debug.Log(e.Message);
				}
				string text = File.ReadAllText(destinationFile);
				text = text.Replace("Float", type);
				File.WriteAllText(destinationFile, text);
			}

		}

	}
}