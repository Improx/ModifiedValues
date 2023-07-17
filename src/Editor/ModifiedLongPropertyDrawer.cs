using System.Reflection;
using ModifiedValues;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ModifiedLong))]
public class ModifiedLongPropertyDrawer : PropertyDrawer
{
	private ModifiedLong _modValue;
	private UnityEngine.Object _targetObject;
	private const float _extraTotalHeight = 30;

	// Draw the property inside the given rect
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		//This initialization part should only run once, or until the property
		//is initialized properly
		if (_modValue is null)
		{
			UnityEngine.Object targetObject = property.serializedObject.targetObject;
			ModifiedLong modValue = (ModifiedLong) GetPropertyInstance(property, targetObject);
			if (!modValue.Init)
			{
				//This modValue property was declared but not assigned to, so
				//Unity sneakily created a bad instance of it, bypassing all constructors
				position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
				GUI.contentColor = new Color(1f, 0.77f, 0.77f);
				if (Settings.ShouldShowLatestValue)
				{
					position.height -= _extraTotalHeight;
				}
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

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return base.GetPropertyHeight(property, label) + (Settings.ShouldShowLatestValue ? _extraTotalHeight : 0);
	}
}