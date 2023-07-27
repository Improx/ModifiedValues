using UnityEngine;

namespace ModifiedValues.Editor
{
	public static class Settings
	{
		public static ShowLatestValue ShowLatestValue = ShowLatestValue.Always;

		public static bool ShouldShowLatestValue => ShowLatestValue == ShowLatestValue.Always || (Application.isPlaying && Settings.ShowLatestValue == ShowLatestValue.OnlyRuntime);
	}

	public enum ShowLatestValue { Never, OnlyRuntime, Always }
}