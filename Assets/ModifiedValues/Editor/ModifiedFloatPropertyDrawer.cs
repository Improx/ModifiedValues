using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ModifiedValues.Editor
{
	[CustomPropertyDrawer(typeof(ModifiedFloat))]
	public class ModifiedFloatPropertyDrawer : PropertyDrawer
	{
		private ModifiedFloat _modValue;
		private UnityEngine.Object _targetObject;
		private const float _extraTotalHeight = 36;

		// Draw the property inside the given rect
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			//This initialization part should only run once, or until the property
			//is initialized properly
			if (_modValue is null)
			{
				UnityEngine.Object targetObject = property.serializedObject.targetObject;
				ModifiedFloat modValue = (ModifiedFloat)GetPropertyInstance(property, targetObject);
				if (!modValue.Init)
				{
					//This modValue property was declared but not assigned to, so
					//Unity sneakily created a bad instance of it, bypassing all constructors
					position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
					GUI.contentColor = new Color(1f, 0.77f, 0.77f);
					EditorGUI.LabelField(position, "Uninitialized.");
					GUI.contentColor = Color.white;
					return;
				}
				_targetObject = targetObject;
				_modValue = modValue;

				//Without this, the inspector would only update if moving mouse inside it:
				_modValue.BecameDirty += (s, e) => EditorUtility.SetDirty(_targetObject);
			}

			EditorGUI.BeginProperty(position, label, property);
			Rect fieldRect = position;
			if (Settings.ShouldShowLatestValue)
			{
				fieldRect.height -= _extraTotalHeight;
			}
			if (property.FindPropertyRelative("_usingSavedBaseValue").boolValue)
			{
				//Using saved base value
				EditorGUI.PropertyField(fieldRect, property.FindPropertyRelative("_savedBaseValue"), label);
			}
			else
			{
				//Using base value getter
				fieldRect = EditorGUI.PrefixLabel(fieldRect, GUIUtility.GetControlID(FocusType.Passive), label);
				GUI.contentColor = new Color(0.7f, 0.77f, 1f);
				if (GUI.Button(fieldRect, new GUIContent("Base value getter: " + _modValue.BaseValue, "Click to use saved base value instead.")))
				{
					_modValue.UseSavedBaseValue();
				}
				GUI.contentColor = Color.white;
			}

			//Show current value
			if (Settings.ShouldShowLatestValue)
			{
				GUI.contentColor = new Color(0.68f, 0.68f, 0.68f);
				EditorGUI.LabelField(position, "     Current value: " + _modValue.Value.ToString());
				GUI.contentColor = Color.white;
			}

			EditorGUI.EndProperty();
		}

		public System.Object GetPropertyInstance(SerializedProperty property, UnityEngine.Object targetObject)
		{
			if (property == null || targetObject == null)
				return null;

			string path = property.propertyPath.Replace(".Array.data[", "[");
			object obj = targetObject;
			string[] elements = path.Split('.');

			foreach (var element in elements)
			{
				if (element.Contains("["))
				{
					string elementName = element.Substring(0, element.IndexOf("["));
					int index = int.Parse(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
					obj = GetValue(obj, elementName, index);
				}
				else
				{
					obj = GetValue(obj, element);
				}
			}

			return obj;
		}

		private static object GetValue(object source, string name)
		{
			if (source == null)
				return null;

			var type = source.GetType();
			var field = type.GetField(name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

			if (field == null)
			{
				var property = type.GetProperty(name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);
				if (property == null)
					return null;

				return property.GetValue(source, null);
			}

			return field.GetValue(source);
		}

		private static object GetValue(object source, string name, int index)
		{
			var enumerable = GetValue(source, name) as System.Collections.IEnumerable;
			if (enumerable == null)
				return null;

			var enm = enumerable.GetEnumerator();

			for (int i = 0; i <= index; i++)
			{
				if (!enm.MoveNext())
					return null;
			}

			return enm.Current;
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			bool initialized = _modValue is not null && _modValue.Init;
			return base.GetPropertyHeight(property, label) + (Settings.ShouldShowLatestValue && initialized ? _extraTotalHeight : 0);
		}
	}
}