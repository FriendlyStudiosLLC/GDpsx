using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

namespace GDpsx_API.EventSystem
{
	[Tool]
	public partial class GDpsx_ES_Start : GDpsx_ES_Node
	{
		public GDpsx_ES_R_StartNode resource;
		public GraphType graphType = GraphType.Event;

		public void ConstructResource()
		{
			resource = new GDpsx_ES_R_StartNode(graphType);
		}
	}
}
