using Godot;
using System;
using System.Drawing;
using System;
 
 
public partial class SimpleGraphNode : GraphNode
{
    public GDpsx_Dialogue_Graph parentGraph;

    public void ResizeNode(Vector2 newSize)
    {
        this.Size = newSize;
    }

    public void Select()
    {
        
        GD.Print("Selected");
        if(parentGraph != null)
        {
            //parentGraph.selected_nodes.Add(this);
        }
        else
        {
            GD.Print("No parent node");
        }
    }

    public void DeSelect()
    {
        GD.Print("DeSelected");
        if(parentGraph != null)
        {
            //parentGraph.selected_nodes.Remove(this);
        }
        else
        {
            GD.Print("No parent node");
        }
    }

    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("Dialogue_Delete")) DeleteNode();
    }

    public void DeleteNode()
    {
        if(!Selected)return;
        GD.Print("Deleting Nodes");
        //parentGraph.selected_nodes.Remove(this);
        QueueFree();
    }

    
}
