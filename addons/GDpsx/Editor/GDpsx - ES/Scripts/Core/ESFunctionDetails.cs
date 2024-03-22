using Godot;

namespace GDpsx_API.EventSystem
{
	public class ESFunctionDetails
	{
		private Variant methodName;
		private Variant parameter1Type;
		private Variant parameter1;
		private Variant parameter2Type;
		private Variant parameter2;

		public ESFunctionDetails(Variant MethodName, Variant Parameter1, Variant Parameter1Type, Variant Parameter2, Variant Parameter2Type)
		{
			methodName = MethodName;
			parameter1Type = Parameter1Type;
			parameter1 = Parameter1;
			parameter2Type = Parameter2Type;
			parameter2 = Parameter2;
		}

	}
}