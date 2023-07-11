namespace ModifiedValues
{
	public static class Settings
	{
		public static ShowLatestValue ShowLatestValue = ShowLatestValue.Always;
	}

	public enum ShowLatestValue { Never, OnlyRuntime, Always }
}