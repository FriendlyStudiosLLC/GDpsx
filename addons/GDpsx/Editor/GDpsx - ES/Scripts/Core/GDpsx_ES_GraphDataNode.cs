using Godot;
using Godot.Collections;
using System;
using System.IO;
using System.Collections.Generic;

namespace GDpsx_API.EventSystem
{
	[Tool]
	public partial class GDpsx_ES_GraphDataNode : GDpsx_ES_Node
	{
		[Export] public GDpsx_ES_R_Data graph;
		[Export] public FileDialog fileBrowser;
		[Export] public Label graphName;

		public void ShowFileBrowser()
		{
			fileBrowser.Visible = true;
		}

		public void load(string path)
		{
			Resource dataTest = ResourceLoader.Load(path,
				"res://addons/GDpsx/Editor/GDpsx - ES/Scripts/Core/Resources/GDpsx_ES_R_Data.cs",
				ResourceLoader.CacheMode.Ignore);
			GDpsx_ES_R_Data data = dataTest as GDpsx_ES_R_Data;
			FileInfo fileInfo = new FileInfo(path);
			int directoryLength = fileInfo.DirectoryName.Length;
			string fileName = fileInfo.FullName.Remove(0, directoryLength + 1);
			if (data != null)
			{
				graph = data;
				graphName.Text = fileName;
			}
		}
	}
}
