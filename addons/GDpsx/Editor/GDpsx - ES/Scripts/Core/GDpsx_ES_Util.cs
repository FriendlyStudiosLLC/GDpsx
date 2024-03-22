using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

namespace GDpsx_API.EventSystem
{

	[Tool]
	public partial class GDpsx_ES_Util : Node
	{
		public void Save(string path, Dictionary data)
		{
			FileAccess file = FileAccess.Open(path, FileAccess.ModeFlags.Write);
			file.StoreLine(Json.Stringify(data));
			file.Close();
		}


	}
}
