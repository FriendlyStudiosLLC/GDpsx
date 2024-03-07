using GDpsx_API;
using GDpsx_API.EventSystem;
using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;

public partial class GDpsx_GlobalEventSystem : Node
{
    [Export] public GDpsx_ES_R_Data data;
    [Export] public GDpsx_dialog_box dialog_Box;
    [Export] public GDpsx_ES_R_Node currentNode;
    [Export] public GDpsx_ES_FunctionNodeLibrary functionLibary;
    [Export] public GDpsx_ES_ConditionalNodeLibrary conditionLibary;

    

    public void SetEventPath(string path)
    {
        var dataTest = ResourceLoader.Load("res://Test_data.tres", "res://addons/GDpsx/Editor/GDpsx - ES/Scripts/Core/Resources/GDpsx_ES_R_Data.cs", ResourceLoader.CacheMode.Ignore);
        var _data = dataTest as GDpsx_ES_R_Data;
        data = _data;
    }

    public void SetData(GDpsx_ES_R_Data _data)
    {
        data = _data;
    }

    public void StartEventGraph()
    {
        if(data == null) return;
        GD.Print($"Starting Graph");
        if(data.nodes[0].nodeType == GDpsx_API.EventSystem.NodeType.Start)
        {
            currentNode = data.nodes[0];
            var nextNode = currentNode.GotoNodes[0].AsGodotObject() as GDpsx_ES_R_Node;
            if((GodotObject)currentNode.GotoNodes[0] is GodotObject)
            {
                GotoNode(nextNode.NodeName);
            }
            else
            {
                GotoNextNode();
            }
        }
        else
            {
                GotoNextNode();
            }
    }

    public void ProcessResponse(string option)
    {
        foreach(var node in data.nodes)
        {
            if(node.nodeType == NodeType.Option)
            {
                GD.Print("LOADING RESPONSE");
                var optionnode = node as GDpsx_ES_R_Option;
                if(optionnode.OptionText == option)
                {
                    GotoNode(optionnode.NodeName);
                }
            }
        }

    }

    public void GotoNode(StringName nodeName)
    {
        foreach(var node in data.nodes)
        {
            if(node.NodeName == nodeName)
            {
                GD.Print($"Going to {node.NodeName}");
                currentNode = node;
                PerformNode();
                return;
            }
        }
    }

    public void GotoNextNode()
    {
        if(currentNode.GotoNodes[0].AsStringName() == null)
        {
            EndEventSystem();
        }
        var nextNode = currentNode.GotoNodes[0].AsStringName();
        GotoNode(nextNode);
    }

    public void EndEventSystem()
    {
        var player = GDpsx_Utility.GetPlayer(GetTree());
        if(player.dialogBox.Visible) player.dialogBox.HideDialogBox();
        data = null;
        QueueFree();
    }

    public void PerformNode()
    {
        if(currentNode != null)
        {
            GD.Print(currentNode.NodeName);
        }
        switch(currentNode.nodeType)
        {
            case NodeType.Wait:
                var w_node = currentNode as GDpsx_ES_R_WaitNode;
                ExecuteWaitNode(w_node.waitTimeMS);
                break;
            case NodeType.Conditional:
                var c_node = currentNode as GDpsx_ES_R_Conditional;
                ExecuteConditionNode();
                break;
            case NodeType.Function:
                var f_node = currentNode as GDpsx_ES_R_Conditional;
                ExecuteFunctionNode();
                break;
            case NodeType.Dialog:
                ExecuteDialogueNode();
                break;
            case NodeType.Start:
                GotoNextNode();
                break;
            case NodeType.Option:
                GotoNextNode();
                break;
            case NodeType.End:
                EndEventSystem();
                break;
                    
        }
    }

    public void ExecuteDialogueNode()
    {
        if(dialog_Box.eventSystem == null) dialog_Box.eventSystem = this;
        dialog_Box.DisplayDialogBox();
    }

    public async void ExecuteWaitNode(int waitTime)
    {
        await Task.Delay(waitTime);
        GotoNextNode();
    }

    public void ExecuteFunctionNode()
    {
        
        if(currentNode.nodeType != NodeType.Function) return;
        var functionNode = currentNode as GDpsx_ES_R_Function;
        functionLibary.Call(functionNode.methodName, ConvertToArray(functionNode.parameterList));
        
        GotoNextNode();

    }

    public Variant[] ConvertToArray(Godot.Collections.Array<Variant> godotArray)
    {
        // Initialize a new Variant array with the same size as the Godot.Collections.Array
        Variant[] variantArray = new Variant[godotArray.Count];

        // Copy each element from the Godot.Collections.Array to the Variant array
        for (int i = 0; i < godotArray.Count; i++)
        {
            variantArray[i] = godotArray[i];
        }

        return variantArray;
    }

    public void ExecuteConditionNode()
    {
        if(currentNode.nodeType != NodeType.Conditional) return;
        var conditioNode = currentNode as GDpsx_ES_R_Conditional;
        var result = (bool)conditionLibary.Call(conditioNode.methodName, ConvertToArray(conditioNode.parameterList));
        if(result)
        {
            var trueNode = currentNode.GotoNodes[0].AsStringName();
            GotoNode(trueNode);
        }
        else
        {
            var falseNode = currentNode.GotoNodes[1].AsStringName();
            GotoNode(falseNode);
        }
    }


    public GDpsx_ES_R_Node GetNodeByNodeName(StringName nodeName)
    {
        foreach(var node in data.nodes)
        {
            if(node.NodeName == nodeName)
            {
                return node;
            }
        }
        return null;
    }


    
    
}
