using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace GDpsx_API.EventSystem
{
    public partial class GDpsx_ES_Graph : GraphEdit
    {
        
        [Export] public MenuButton AddNode_MenuButton;
        [Export] public GDpsx_GameBrain GameBrain;
        [Export] public Button Save_Button;
        [Export] public Button Load_Button;
        [Export] public FileDialog Save_FileDialog;
        [Export] public FileDialog Load_FileDialog;
        public Array<GDpsx_ES_Node> Nodes = new Array<GDpsx_ES_Node>();
        public Array<GDpsx_ES_Node> SelectedNodes = new Array<GDpsx_ES_Node>();

        public Dictionary data;



        public override void _Ready()
        {
            CreateNodeTypes();
            AddNode_MenuButton.GetPopup().IndexPressed += AddNodeOfType;
        }

        private void AddNodeOfType(long index)
        {
            var pos = GetLocalMousePosition();
            PackedScene NodeScene = new PackedScene();
            switch(index)
            {
                case 0:
                    NodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Dialog_Node.tscn");
                    break;
                case 1:
                    GD.PrintErr("Not Yet Implemented");
                    break;
                case 2:
                    NodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Function_Node.tscn");
                    break;
            }
            var Node = NodeScene.Instantiate() as GDpsx_ES_Node;
            Node.PositionOffset = pos;
            Node.ParentGraph = this;
            AddChild(Node);
            Nodes.Add(Node);
        }

        public void OpenSaveFileDialogue()
        {
            Save_FileDialog.Visible = true;
        }
        public void OpenLoadFileDialogue()
        {
            Load_FileDialog.Visible = true;
        }
        public void DisconnectRequest(StringName fromNode, int fromPort, StringName toNode, int toPort) //Called whenever a node is being disconnected
        {
            DisconnectNode(fromNode, fromPort, toNode, toPort);
            DisconnectNode(toNode, fromPort, fromNode, toPort);
        }
        public void EmptyGraph() // Empties the graph of all of it's nodes
        {
            for(int i = Nodes.Count - 1; i >= 0; i--)
            {
                Nodes[i].DeleteNode(true);
            }
        }

         public new void ConnectionRequest(StringName fromNode, int fromPort, StringName toNode, int toPort)
            {
                ConnectNode(fromNode, fromPort, toNode, toPort);
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

        public List<ConnectionDetails> GetConnectedNodesDetails(StringName nodeName) //Neatly packages a nodes connection data
        {
            var connectionDetailsList = new List<ConnectionDetails>();

            foreach (Dictionary connection in GetConnectionList())
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
        private void CreateNodeTypes() //Populates Menu Button with necessary options
        {
            AddNode_MenuButton.GetPopup().AddItem("Dialogue Node");
            AddNode_MenuButton.GetPopup().AddItem("Conditional Node");
            AddNode_MenuButton.GetPopup().AddItem("Function Node");
        }

        public async void Save(string path)
        {
            await Task.Delay(200);
            var file = FileAccess.Open(path,FileAccess.ModeFlags.Write);
            file.StoreLine(Json.Stringify(data));
            file.Close();
        }

        public void ConsolidateData(string path)
        {
            data = new Dictionary();
            foreach(var node in Nodes)
            {
                var data_template = new Dictionary();
                data[node.Title] = data_template;
                var node_position = new Vector2
                {
                    X = node.PositionOffset.X,
                    Y = node.PositionOffset.Y
                };
                data_template["Position"] = node_position;
                data_template["ToNode"] = new Godot.Collections.Array<StringName>();
                data_template["FromNode"] = new Godot.Collections.Array<StringName>();
                data_template["ToPort"] = new Godot.Collections.Array<int>();
                data_template["FromPort"] = new Godot.Collections.Array<int>();
                foreach(Dictionary connection in GetConnectionList())
                {
                    
                    
                        if(connection["to_node"].AsStringName() != node.Name) ((Godot.Collections.Array)data_template["ToNode"]).Add(connection["to_node"].ToString());
                        if(connection["from_node"].AsStringName() != node.Name) ((Godot.Collections.Array)data_template["FromNode"]).Add(connection["from_node"].ToString());
                        if(connection["to_node"].AsStringName() != node.Name) ((Godot.Collections.Array)data_template["ToPort"]).Add(connection["to_port"]);
                        if(connection["from_node"].AsStringName() != node.Name) ((Godot.Collections.Array)data_template["FromPort"]).Add(connection["from_port"]);
                    
                }
                switch(node.Type)
                {
                    case NodeType.Dialog:
                        var dialogNode = node as GDpsx_ES_Dialog;
                        dialogNode.ConstructDialogDictionary();
                        data_template["Data"] = dialogNode.dialogData;
                        break;
                        
                    case NodeType.Conditional:
                        break;

                    case NodeType.Function:
                        var functionNode = node as GDpsx_ES_Function;
                        functionNode.ConstructFunctionDictionary();
                        data_template["Data"] = functionNode.funcData;
                        break;
                }
                data[node.Title] = data_template;
                Save(path);
            }
        }


        public async void Load(string path)
        {
            EmptyGraph();
            await Task.Delay(100);
        }
    }
}
