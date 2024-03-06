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

    public override void _Ready()
    {
       StartEventGraph();
    }

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
            GotoNode(nextNode.NodeName);
        } 
    }

    public void GotoNode(StringName nodeName)
    {
        foreach(var node in data.nodes)
        {
            if(node.NodeName == nodeName)
            {
                currentNode = node;
                PerformNode();
                return;
            }
        }
    }

    public void GotoNextNode()
    {
        var nextNode = currentNode.GotoNodes[0].AsStringName();
        GotoNode(nextNode);
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
                //TODO: Still need to implement population of dialog data in the dialog_box class
                break;
            case NodeType.Start:
                GotoNextNode();
                break;
                    
        }
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
        functionLibary.currentParameters = functionNode.parameterList;
        GD.Print(functionLibary.currentParameters[0].ToString());
        functionLibary.Call(functionNode.methodName, ConvertToArray(functionNode.parameterList));
        
        GotoNextNode();

    }

    public static Variant[] ConvertToArray(Godot.Collections.Array<Variant> godotArray)
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


    
    
}
