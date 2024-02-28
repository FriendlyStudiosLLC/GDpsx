using Godot;
using System;
using System.Drawing;
 
using GDpsx_API;
using System.Collections.Generic;
using GDpsx_API.EventSystem;

public partial class GDpsx_GraphNode : GraphNode
{
    
    public GDpsx_Dialogue_Graph parentGraph;
    [Export] public LineEdit titleLine;
    [Export] public NodeType nodeType;
    public string nodeName = "";
    public Vector2 position;
    public int index;


    public void ResizeNode(Vector2 newSize)
    {
        this.Size = newSize;
    }

    public void Select()
    {
        if(parentGraph != null)
        {
            parentGraph.selected_nodes.Add(this);
        }
    }

    public void DeSelect()
    {
        if(parentGraph != null)
        {
            parentGraph.selected_nodes.Remove(this);
        }
    }

    public override void _Process(double delta)
    {
        Title = $"{nodeType} | {index}";
        if(Input.IsActionJustPressed("Dialogue_Delete")) DeleteNode(false);
    }

    public virtual void DeleteNode(bool bypassSelected)
    {
        
        if(!Selected && !bypassSelected) return;
        if(Selected || !bypassSelected || !Selected && bypassSelected) 
        {
            
            List<ConnectionDetails> connectionDetails = parentGraph.GetConnectedNodesDetails(parentGraph.graphEdit, Name);

            foreach(var connection in connectionDetails)
            {
               parentGraph.graphEdit.DisconnectNode(connection.From, connection.FromPort, connection.To, connection.ToSlot);
                parentGraph.graphEdit.DisconnectNode(connection.To, connection.ToSlot, connection.From, connection.FromPort);
            }
            if(Selected)parentGraph.selected_nodes.Remove(this);
            parentGraph.Nodes.Remove(this);
            QueueFree();
        }
        
        //parentGraph.RefreshNodeIndexes();
    }

    
}