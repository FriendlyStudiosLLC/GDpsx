using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
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

         public GDpsx_ES_Node GetNodeByName(StringName nodeName)
        {
            foreach(var node in Nodes)
            {
                
                if(node.Name == nodeName)
                {
                    return node as GDpsx_ES_Node;
                }
                
            }
            return null;
        }

        public new void ConnectionRequest(StringName fromNode, int fromPort, StringName toNode, int toPort)
        {
            ConnectNode(fromNode, fromPort, toNode, toPort);
            ConnectNode(toNode, fromPort, fromNode, toPort);
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
                var connectionList = new Array<Dictionary>();
                var connections = new Dictionary();
                connections["go to"] = new StringName();
                connections["come from"] = new StringName();
                connections["go to index"] = new int();
                connections["come from index"] = new int();
                foreach(var _connectionDetails in GetConnectedNodesDetails(node.Name))
                {
                    
                        connections["go to"] = _connectionDetails.To;
                        connections["come from"] = _connectionDetails.From;
                        connections["go to index"] = _connectionDetails.ToSlot;
                        connections["come from index"] = _connectionDetails.FromPort;
                        //((Godot.Collections.Array)connections["go to"]).Add(connection["to_node"].ToString());
                        //(Godot.Collections.Array)connections["come from"]).Add(connection["from_node"].ToString());
                        //((Godot.Collections.Array)connections["go to index"]).Add(connection["to_port"]);
                        //((Godot.Collections.Array)connections["come from index"]).Add(connection["from_port"]);
                    
                        connectionList.Add(connections);
                }
                data_template["Connections"] = connectionList;
                
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
