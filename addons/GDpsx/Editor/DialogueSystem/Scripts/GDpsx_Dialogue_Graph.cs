using Godot;
using Godot.Collections;
using System;
using System.IO;
using Microsoft.VisualBasic;
using FileAccess = Godot.FileAccess;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using GDpsx_API;
using System.Threading.Tasks;
using GDpsx_API.EventSystem;

[Tool]
public partial class GDpsx_Dialogue_Graph : Control
{
    [Export] public PackedScene SimpleGraphNode;
    [Export] public GraphEdit graphEdit;
    [Export] public MenuButton AddNodeButton;
    [Export] public GDpsx_GameBrain Brain;
    [Export] public FileDialog save_fileDialog;
    [Export] public FileDialog load_fileDialog;
    public Array<GDpsx_GraphNode> Nodes = new Array<GDpsx_GraphNode>();
    public Array<GDpsx_GraphNode> selected_nodes = new Array<GDpsx_GraphNode>();
    public Dictionary dialogue = new Dictionary();
    private string filePath = "";


    

    public void SetFilePath(string path)
    {
        filePath = path;
    }

    public override void _Ready()
    {
        CreateNodeTypes();
        AddNodeButton.GetPopup().IndexPressed += AddNodeOfType;
    }
    
    private void CreateNodeTypes()
    {
        AddNodeButton.GetPopup().AddItem("Dialogue Node");
    }

    public void AddNodeOfType(long index)
    {
        string graphNodeType = AddNodeButton.GetPopup().GetItemText((int)index);

        switch (graphNodeType)
        {
            case "Simple Dialogue Node":
                AddNode();
                break;
        }

    }

    public void OpenSaveFileDialogue()
    {
        save_fileDialog.Visible = true;
    }

    public void OpenLoadFileDialogue()
    {
        load_fileDialog.Visible = true;
    }

public void AddNode()
    {
        var sgn = SimpleGraphNode.Instantiate() as GDpsx_DialogueNode;
        
        sgn.parentGraph = this;
        Nodes.Add(sgn);
        sgn.index = Nodes.IndexOf(sgn);
        sgn.Name = $"{sgn.nodeType} | {sgn.index}";
        
        graphEdit.AddChild(sgn);
    }

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if(Input.IsKeyPressed(Key.Key1)) AddNode();
    }

    public void ConnectionRequest(StringName fromNode, int fromPort, StringName toNode, int toPort)
    {
        graphEdit.ConnectNode(fromNode, fromPort, toNode, toPort);
        //graphEdit.ConnectNode(toNode, toPort, fromNode, fromPort);
        //GD.Print($"{fromNode} | {fromPort} | {toNode} | {toPort}");
        // if(fromPort != 0 && GetNodeByName(fromNode).nodeType == NodeType.Dialogue)
        // {
        //     GDpsx_DialogueNode dialogueNode = GetNodeByName(fromNode) as GDpsx_DialogueNode;
        //     if(dialogueNode != null) dialogueNode.ConstructResponseLink(fromNode, fromPort, toNode, toPort);
            
        //     GDpsx_DialogueNode toDialogueNode = GetNodeByName(toNode) as GDpsx_DialogueNode;
        //     if(toDialogueNode != null)
        //     {
        //         toDialogueNode.data.fromSlot = fromNode;
        //         toDialogueNode.data.fromSlotIndex = fromPort;
        //     }
        //     //GD.Print(dialogueNode.data.responses[fromPort-1].toNode);
        // }
        // if(fromPort == 0 && GetNodeByName(fromNode).nodeType == NodeType.Dialogue)
        // {
        //     GDpsx_DialogueNode fromDialogueNode = GetNodeByName(fromNode) as GDpsx_DialogueNode;
        //     GDpsx_DialogueNode toDialogueNode = GetNodeByName(toNode) as GDpsx_DialogueNode;
        //     if(fromDialogueNode != null)
        //     {
        //         fromDialogueNode.data.toSlot = toNode;
        //         fromDialogueNode.data.toSlotIndex = toPort;
        //     }
        //     if(toDialogueNode != null)
        //     {
        //         toDialogueNode.data.fromSlot = fromNode;
        //         toDialogueNode.data.fromSlotIndex = fromPort;

        //     }
        //     //GD.Print(dialogueNode.data.responses[fromPort-1].toNode);
        // }
    }

    public void DisconnectRequest(StringName fromNode, int fromPort, StringName toNode, int toPort)
    {
        graphEdit.DisconnectNode(fromNode, fromPort, toNode, toPort);
        graphEdit.DisconnectNode(toNode, fromPort, fromNode, toPort);
    }


    public GDpsx_GraphNode GetNodeByName(StringName nodeName)
    {
        foreach(var node in Nodes)
        {
            
            if(node.Name == nodeName)
            {
                return node as GDpsx_GraphNode;
            }
            
        }
        return null;
    }

    public GDpsx_DialogueNode CheckConnection(GDpsx_DialogueNode node)
    {
        foreach(Dictionary connection in graphEdit.GetConnectionList())
        {
            if((StringName)connection["from"] == node.Name)
            {
                //return GetNodeByName((StringName)connection["to"]);
            }

        }
        return null;
    }
    public void ExportDialogue(string path)
    {
        dialogue = new Dictionary();
        foreach(var node in Nodes)
        {
            var dialog_template = new Dictionary(); 

            //Name the node
            dialogue[node.Title] = dialog_template;
            dialog_template["id"] = node.Title;

            //offset
            dialog_template["offset x"] = node.PositionOffset.X;
            dialog_template["offset y"] = node.PositionOffset.Y;
            
            //go to
            dialog_template["go to"] = new Godot.Collections.Array<StringName>();
            dialog_template["come from"] = new Godot.Collections.Array<StringName>();
            dialog_template["go to index"] = new Godot.Collections.Array<int>();
            dialog_template["come from index"] = new Godot.Collections.Array<int>();
            foreach(Dictionary connection in graphEdit.GetConnectionList())
            {
                if(connection["from_node"].AsStringName() == node.Name)
                {
                    ((Godot.Collections.Array)dialog_template["go to"]).Add(connection["to_node"].ToString());
                    ((Godot.Collections.Array)dialog_template["come from"]).Add(connection["from_node"].ToString());
                    ((Godot.Collections.Array)dialog_template["go to index"]).Add(connection["to_port"]);
                    ((Godot.Collections.Array)dialog_template["come from index"]).Add(connection["from_port"]);
                }
                #region Comments
                // Connection keywords
                // ["from_node"] = node.name # from current node
                // ["from_port"] = option_count # from current option port
                // ["to_node"] = data[node_index]["go to"][option_count - 1] # connect to node
                // ["to_port"] = 0 # connect to text port
                #endregion
            }

            switch (node.nodeType)
            {
                case NodeType.Dialogue:
                    
                    var dialogueNode = node as GDpsx_DialogueNode;
                    dialog_template["responses"] = new Godot.Collections.Array<String>();

                    
                    dialog_template["character"] = dialogueNode.SpeakingCharacter_Label.Text;
                    dialog_template["text"] = dialogueNode.message_Text.Text;
                    dialog_template["isResponse"] = dialogueNode.isResponse;
                     
                    foreach(var response in dialogueNode.responses)
                    {
                        ((Godot.Collections.Array)dialog_template["responses"]).Add(response.responseText.Text);
                    }
                    break;
            }

            dialogue[node.Title] = dialog_template;
            
        Save(path);
        }
    }


    public void Save(string path)
    {
        var file = FileAccess.Open(path,FileAccess.ModeFlags.Write);
        file.StoreLine(Json.Stringify(dialogue));
        file.Close();
    }

    public async void Load(string path)
    {
        EmptyGraph();
        await Task.Delay(100);
        var file = FileAccess.Open(path,FileAccess.ModeFlags.Read);
        var fileData = file.GetAsText(true);
        file.Close();
        var json_object = new Json();
        var parsedData = json_object.Parse(fileData);
        var data = json_object.Data.AsGodotDictionary();

        //Convert keys to dictionary
        foreach(var key in data.Keys)
        {
            var nestedDictionary = data[key].AsGodotDictionary();
            
            var sgn = SimpleGraphNode.Instantiate() as GDpsx_DialogueNode;

            var goto_Array = nestedDictionary["go to"].AsGodotArray<StringName>();
            var gotoIndex_Array = nestedDictionary["go to index"].AsInt32Array();
            var comefrom_Array = nestedDictionary["come from"].AsGodotArray<StringName>();
            var comefromIndex_Array = nestedDictionary["come from index"].AsInt32Array();

            var response_Array = nestedDictionary["responses"].AsGodotArray<string>();
            sgn.Name = nestedDictionary["id"].AsStringName();
            sgn.isResponse = nestedDictionary["isResponse"].AsBool();
            sgn.parentGraph = this;
            graphEdit.AddChild(sgn);
            
            Nodes.Add(sgn);
            sgn.index = Nodes.IndexOf(sgn);
            sgn.Name = $"{sgn.nodeType} | {sgn.index}";
            sgn.SpeakingCharacter_Label.Text = nestedDictionary["character"].ToString();
            sgn.message_Text.Text = nestedDictionary["text"].AsString();
            Godot.Vector2 positionOffset = new Godot.Vector2();
            positionOffset.X = (float)nestedDictionary["offset x"].AsDouble();
            positionOffset.Y = (float)nestedDictionary["offset y"].AsDouble();
            sgn.PositionOffset = positionOffset;
            var responseIndex = new int();
            foreach(var response in response_Array)
            {
                var newResponse = sgn.responseScene.Instantiate() as GDpsx_DialogueResponse;
                var index = response_Array.IndexOf(response);
                responseIndex = index+1;
                newResponse.responseText.Text = response;
                sgn.responses.Add(newResponse);
                newResponse.index = index;
                newResponse.slotIndex = index+1;
                sgn.SetSlotEnabledRight((index+1), true);
                
                sgn.SetSlotEnabledRight(0, false);
                sgn.AddChild(newResponse);
            }
            
            foreach(var goto_Key in goto_Array)
            {
                var index = goto_Array.IndexOf(goto_Key);
                var goto_count = 0;
                graphEdit.ConnectNode(sgn.Name, comefromIndex_Array[index], goto_Key, gotoIndex_Array[index]);
                goto_count += 1;
            }
            
        }
    }

    public void EmptyGraph()
    {
        for(int i = Nodes.Count - 1; i >= 0; i--)
        {
            Nodes[i].DeleteNode(true);
        }
    }

    public List<ConnectionDetails> GetConnectedNodesDetails(GraphEdit graphEdit, StringName nodeName)
    {
        var connectionDetailsList = new List<ConnectionDetails>();

        foreach (Dictionary connection in graphEdit.GetConnectionList())
        {
            if (connection["from_node"].AsStringName() == nodeName)
            {
                connectionDetailsList.Add(new ConnectionDetails
                {
                    From = (StringName)connection["from_node"],
                    To = (StringName)connection["to_node"],
                    FromPort = (int)connection["from_port"],
                    ToSlot = (int)connection["to_port"]
                });
            }
            else if (connection["to_node"].AsStringName() == nodeName)
            {
                connectionDetailsList.Add(new ConnectionDetails
                {
                    From = (StringName)connection["to_node"],
                    To = (StringName)connection["from_node"],
                    FromPort = (int)connection["to_port"],
                    ToSlot = (int)connection["from_port"]
                });
            }
        }

        return connectionDetailsList;
    }

   
    
}
