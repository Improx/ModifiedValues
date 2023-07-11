using System.Reflection;
using ModifiedValues;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ModifiedDecimal))]
public class ModifiedDecimalPropertyDrawer : PropertyDrawer
{
	private ModifiedDecimal _modValue;
	private UnityEngine.Object _targetObject;

	// Draw the property inside the given rect
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		//This initialization part should only run once, or until the property
		//is initialized properly
		if (_modValue is null)
		{
			UnityEngine.Object targetObject = property.serializedObject.targetObject;
			ModifiedDecimal modValue = (ModifiedDecimal) GetPropertyInstance(property, targetObject);
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

		if (property.FindPropertyRelative("_usingSavedBaseValue").boolValue)
		{
			EditorGUI.PropertyField(position, property.FindPropertyRelative("_savedBaseValue"), label);
		}
		else
		{
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			GUI.contentColor = new Color(0.7f, 0.77f, 1f);
			position.height *= 1.7f;

			if (GUI.Button(position, "Base value getter: " + _modValue.BaseValue + "\n Click to use saved base value."))
			{
				_modValue.UseSavedBaseValue();
			}
			GUILayout.Space(10);
			GUI.contentColor = Color.white;
		}

		if (Settings.ShowLatestValue == ShowLatestValue.Always || (Application.isPlaying && Settings.ShowLatestValue == ShowLatestValue.OnlyRuntime))
		{
			GUI.contentColor = new Color(0.68f, 0.68f, 0.68f);
			GUILayout.Label("     Current value: " + _modValue.Value.ToString());
			GUILayout.Space(5);
			GUI.contentColor = Color.white;
		}

		EditorGUI.EndProperty();
	}

	public System.Object GetPropertyInstance(SerializedProperty property, UnityEngine.Object targetObject)
	{

		string path = property.propertyPath;

		System.Object obj = targetObject;
		var type = obj.GetType();

		var fieldNames = path.Split('.');
		for (int i = 0; i < fieldNames.Length; i++)
		{
			var info = type.GetField(fieldNames[i], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			if (info == null)
				break;

			// Recurse down to the next nested object.
			obj = info.GetValue(obj);
			type = info.FieldType;
		}

		return obj;
	}
}