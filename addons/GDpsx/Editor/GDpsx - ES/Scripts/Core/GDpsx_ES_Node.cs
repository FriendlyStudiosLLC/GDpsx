using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

namespace GDpsx_API.EventSystem
{
	[Tool]
	public partial class GDpsx_ES_Node : GraphNode
	{
		[Export] public LineEdit title;
		public GDpsx_ES_Graph ParentGraph;
		[Export] public int index;
		public Vector2 NodePosition;
		[Export] public NodeType nodeType;
		public string nodeName;
		public List<ConnectionDetails> connectionDetails = new List<ConnectionDetails>();

		/*
		Deletes the node and clears it from the parent graph.
		This is virtual since child classes may require additional steps when deleting a node.
		*/
		public override void _Process(double delta)
		{
			Title = $"{nodeType}_{title.Text}";
			Name = Title;
			if (Input.IsActionJustPressed("ui_graph_delete")) DeleteNode(false);
		}
		public virtual void DeleteNode(bool bypassSelected)
		{
			GDpsx_ES_Graph parentGraph = GetParent() as GDpsx_ES_Graph;
			if (!Selected && !bypassSelected) return;
			if (Selected || !bypassSelected || !Selected && bypassSelected)
			{

				List<ConnectionDetails> connectionDetails = parentGraph.GetConnectedNodesDetails(Name);

				foreach (ConnectionDetails connection in connectionDetails)
				{
					parentGraph.DisconnectNode(connection.From, connection.FromPort, connection.To, connection.ToSlot);
					parentGraph.DisconnectNode(connection.To, connection.ToSlot, connection.From, connection.FromPort);
				}
				if (Selected) parentGraph.SelectedNodes.Remove(this);
				parentGraph.Nodes.Remove(this);
				QueueFree();
			}

			//parentGraph.RefreshNodeIndexes();
		}
		public void ResizeNode(Vector2 newSize) //
		{
			Size = newSize;
		}
		public void Select()
		{
			if (ParentGraph != null)
			{
				ParentGraph.SelectedNodes.Add(this);
			}
		}
		public void DeSelect()
		{
			if (ParentGraph != null)
			{
				ParentGraph.SelectedNodes.Remove(this);
			}
		}

	}
}
