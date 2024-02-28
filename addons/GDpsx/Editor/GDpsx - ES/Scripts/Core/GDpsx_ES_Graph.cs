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
            throw new NotImplementedException();
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
            ConsolidateData();
            await Task.Delay(200);
            var file = FileAccess.Open(path,FileAccess.ModeFlags.Write);
            file.StoreLine(Json.Stringify(data));
            file.Close();
        }

        private void ConsolidateData()
        {
            data = new Dictionary();
            foreach(var node in Nodes)
            {
                var data_template = new Dictionary();
                data[node.Title] = data_template;
                var node_position = new Godot.Collections.Array<float>();
                ((Godot.Collections.Array)data_template["Position"]).Add(node.PositionOffset.X); 
                ((Godot.Collections.Array)data_template["Position"]).Add(node.PositionOffset.Y);
                data_template["ToNode"] = new Godot.Collections.Array<StringName>();
                data_template["FromNode"] = new Godot.Collections.Array<StringName>();
                data_template["ToPort"] = new Godot.Collections.Array<int>();
                data_template["FromPort"] = new Godot.Collections.Array<int>();
                foreach(Dictionary connection in GetConnectionList())
                {
                    if(connection["from_node"].AsStringName() == node.Name)
                    {
                        ((Godot.Collections.Array)data_template["ToNode"]).Add(connection["to_node"].ToString());
                        ((Godot.Collections.Array)data_template["FromNode"]).Add(connection["from_node"].ToString());
                        ((Godot.Collections.Array)data_template["ToPort"]).Add(connection["to_port"]);
                        ((Godot.Collections.Array)data_template["FromPort"]).Add(connection["from_port"]);
                    }
                }
                switch(node.Type)
                {
                    case NodeType.Dialog:
                        break;
                        
                    case NodeType.Conditional:
                        break;

                    case NodeType.Function:
                        break;
                }
                
            }
        }


        public async void Load(string path)
        {
            EmptyGraph();
            await Task.Delay(100);
        }
    }
}
