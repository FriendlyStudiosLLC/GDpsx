using Godot;
using Godot.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
        
        private System.Collections.Generic.Dictionary<string, GDpsx_ES_Node> tempNodeMap = new System.Collections.Generic.Dictionary<string, GDpsx_ES_Node>();

        public Dictionary data;



        public override void _Ready()
        {
            CreateNodeTypes();
            AddNode_MenuButton.GetPopup().IndexPressed += AddNodeOfType;
        }

        public void CheckNodeOrder()
        {
            foreach(var node in Nodes)
            {
                GD.Print(node.Name);
            }
        }

        public void ReordesrNodesByLowestX()
        {
            // Convert to a List for sorting
            List<GDpsx_ES_Node> nodeList = new List<GDpsx_ES_Node>();
            foreach (GDpsx_ES_Node node in Nodes)
            {
                nodeList.Add(node);
            }

            // Sort the List
            nodeList.Sort((a, b) => a.PositionOffset.X.CompareTo(b.PositionOffset.X));
            // Convert back to Godot.Collections.Array if necessary
            Array<GDpsx_ES_Node> sortedNodes = new Array<GDpsx_ES_Node>();
            foreach (var node in nodeList)
            {
                
                sortedNodes.Add(node);
            }
            Nodes = sortedNodes;
        }
        public void ReorderNodesByLowestX()
        {
            
            int sortedIndex = -1;
            for (int i = 0; i < Nodes.Count - 1; i++)
            {
                for (int j = 0; j < Nodes.Count - i - 1; j++)
                {
                    if (((GDpsx_ES_Node)Nodes[j]).PositionOffset.X > ((GDpsx_ES_Node)Nodes[j + 1]).PositionOffset.X)
                    {
                        var temp = Nodes[j];
                        Nodes[j] = Nodes[j + 1];
                        
                        sortedIndex += 1;
                        temp.index = sortedIndex;
                        Nodes[j + 1] = temp;
                    }
                }
            }
        }


        private void AddNodeOfType(long index)
        {
            var pos = GetLocalMousePosition();
            PackedScene NodeScene = new PackedScene();
            switch(index)
            {
                case 0:
                    NodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Start_Node.tscn");
                    break;
                case 1:
                    NodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Dialog_Node.tscn");
                    break;
                case 2:
                    NodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Condition_Node.tscn");
                    break;
                case 3:
                    NodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Function_Node.tscn");
                    break;
                case 4:
                    NodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_End_Node.tscn");
                    break;
                case 5:
                    NodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Wait_Node.tscn");
                    break;
                case 6:
                    NodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Graph_Node.tscn");
                    break;    
            }
            var Node = NodeScene.Instantiate() as GDpsx_ES_Node;
            Node.PositionOffset = pos;
            Node.ParentGraph = this;
            var nodeIndex = Nodes.Count;
            Node.index = nodeIndex;
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
            AddNode_MenuButton.GetPopup().AddItem("Start Node");
            AddNode_MenuButton.GetPopup().AddItem("Dialogue Node");
            AddNode_MenuButton.GetPopup().AddItem("Conditional Node");
            AddNode_MenuButton.GetPopup().AddItem("Function Node");
            AddNode_MenuButton.GetPopup().AddItem("End Node");
            AddNode_MenuButton.GetPopup().AddItem("Wait Node");
            AddNode_MenuButton.GetPopup().AddItem("Graph Node");
        }

        public async void Save(string path)
        {
            await Task.Delay(200);
            var file = FileAccess.Open(path,FileAccess.ModeFlags.Write);
            file.StoreLine(Json.Stringify(data));
            file.Close();
        }

        public async void CollectNodeData(string path)
        {
            
            Array<GDpsx_ES_R_Node> nodeData = new Array<GDpsx_ES_R_Node>();
            ReorderNodesByLowestX();
            await Task.Delay(100);
            
            var lastSavedNode = new GDpsx_ES_Node();
            foreach (var node in Nodes)
            {
                lastSavedNode = node;
                var nodeName = node.Title;
                var nodeType = node.nodeType;
                string nameCheck = node.nodeType.ToString()+"_";
                GD.Print(nameCheck);
                if(nodeName == nameCheck)
                {
                    GD.PrintErr("Every node must have a name");
                    return;
                }
                var nodePosition = node.PositionOffset;
                Godot.Collections.Array GotoNodes = new Godot.Collections.Array();
                Godot.Collections.Array FromPorts = new Godot.Collections.Array();
                foreach(Dictionary connection in GetConnectionList())
                {
                    if(connection["from_node"].AsStringName() == node.Name)
                    {
                        GotoNodes.Add(connection["to_node"].AsStringName());
                        FromPorts.Add(connection["from_port"].AsInt32());
                    }
                }
                switch(node.nodeType)
                    {
                        case NodeType.Dialog:
                            GD.Print(node.Name);
                            var dialogNode = node as GDpsx_ES_Dialog;
                            dialogNode.ConstructDialogResource();
                            var dialogResourceData = dialogNode.resource;
                            dialogResourceData.NodeName = nodeName;
                            dialogResourceData.graphPosition = nodePosition;
                            dialogResourceData.nodeType = nodeType;
                            dialogResourceData.GotoNodes = GotoNodes;
                            dialogResourceData.FromPorts = FromPorts;
                            nodeData.Add(dialogResourceData);
                            break;
                            
                        case NodeType.Conditional:
                            var conditionalNode = node as GDpsx_ES_Condition;
                            conditionalNode.ConstructConditionalData();
                            var conditionaResourceData = conditionalNode.resource;
                            conditionaResourceData.NodeName = nodeName;
                            conditionaResourceData.graphPosition = nodePosition;
                            conditionaResourceData.nodeType = nodeType;
                            conditionaResourceData.GotoNodes = GotoNodes;
                            conditionaResourceData.FromPorts = FromPorts;
                            nodeData.Add(conditionaResourceData);
                            break;

                        case NodeType.Function:
                            var functionNode = node as GDpsx_ES_Function;
                            functionNode.ConstructFunctionData();
                            var funcResourceData = functionNode.resource;
                            funcResourceData.NodeName = nodeName;
                            funcResourceData.graphPosition = nodePosition;
                            funcResourceData.nodeType = nodeType;
                            funcResourceData.GotoNodes = GotoNodes;
                            funcResourceData.FromPorts = FromPorts;
                            nodeData.Add(funcResourceData);
                            break;
                        case NodeType.Start:
                            var startNode = node as GDpsx_ES_Start;
                            startNode.ConstructResource();
                            var startResourceData = startNode.resource;
                            startResourceData.NodeName = nodeName;
                            startResourceData.graphPosition = nodePosition;
                            startResourceData.nodeType = nodeType;
                            startResourceData.GotoNodes = GotoNodes;
                            startResourceData.FromPorts = FromPorts;
                            nodeData.Add(startResourceData);
                            break;
                        case NodeType.End:
                            var endNode = node as GDpsx_ES_End;
                            endNode.ConstructResource();
                            var endResourceData = endNode.resource;
                            endResourceData.NodeName = nodeName;
                            endResourceData.graphPosition = nodePosition;
                            endResourceData.nodeType = nodeType;
                            endResourceData.GotoNodes = GotoNodes;
                            endResourceData.FromPorts = FromPorts;
                            nodeData.Add(endResourceData);
                            break;
                        case NodeType.Wait:
                            var waitNode = node as GDpsx_ES_Wait_Node;
                            var value = waitNode.spinBox.Value;
                            var waitResourceData = new GDpsx_ES_R_WaitNode();
                            waitResourceData.waitTimeMS = (int)value;
                            waitResourceData.NodeName = nodeName;
                            waitResourceData.graphPosition = nodePosition;
                            waitResourceData.nodeType = nodeType;
                            waitResourceData.GotoNodes = GotoNodes;
                            waitResourceData.FromPorts = FromPorts;
                            nodeData.Add(waitResourceData);
                            break;
                    }
            }
            var graphData = new GDpsx_ES_R_Data(nodeData);
            graphData.ResourceName = Name;
            if(Nodes[0].nodeType != NodeType.Start)
            {
                GD.Print("Adding Start Node");
                var startNode = new GDpsx_ES_R_StartNode();
                startNode.GotoNodes.Add(nodeData[0]);
                startNode.graphPosition.X = nodeData[0].graphPosition.X - 50;
                startNode.graphPosition.Y = nodeData[0].graphPosition.Y;
                startNode.NodeName = "Start_Start";
                startNode.nodeType = NodeType.Start;
                graphData.nodes.Insert(0, startNode);
            }
            if(Nodes.Last().nodeType != NodeType.End)
            {
                GD.Print("Adding End Node");
                var endNode = new GDpsx_ES_R_EndNode();
                endNode.NodeName = "End_PlaceHolderEnding";
                endNode.graphPosition.X = Nodes.Last().PositionOffset.X + 50;
                endNode.graphPosition.Y = Nodes.Last().PositionOffset.Y;
                endNode.FromPorts.Add(0);
                endNode.nodeType = NodeType.End;
                
                graphData.nodes.Add(endNode);
            }
            ResourceSaver.Save(graphData, path+".res", ResourceSaver.SaverFlags.None);
        }

        public async void ConsolidateData(string path)
        {
            ReorderNodesByLowestX();
            await Task.Delay(100);
            data = new Dictionary();
            foreach(var node in Nodes)
            {
                var data_template = new Dictionary();
                data[node.Title] = data_template;
                data_template["id"] = node.Title;
                
                data_template["index"] = node.index;
                data_template["Position X"] = node.PositionOffset.X;
                data_template["Position Y"] = node.PositionOffset.Y;
                data_template["go to"] = new Godot.Collections.Array();
                data_template["from port"] = new Godot.Collections.Array();
                foreach(Dictionary connection in GetConnectionList())
                {
                    if(connection["from_node"].AsStringName() == node.Name)
                    {
                        ((Godot.Collections.Array)data_template["go to"]).Add(connection["to_node"].ToString());
                        ((Godot.Collections.Array)data_template["from port"]).Add(connection["from_port"].AsInt32());
                    }
                }
                
                    switch(node.nodeType)
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
    
        public async void LoadResource(string path)
        {
            GD.Print("Loading " + path);
            EmptyGraph();
            await Task.Delay(100);
            var dataTest = ResourceLoader.Load(path, null, ResourceLoader.CacheMode.Ignore);
            var data = dataTest as GDpsx_ES_R_Data;
            GD.Print(data.GetType());
            if (data == null || data.nodes == null)
            {
                GD.PrintErr("Failed to load resource or nodes are null: " + path);
                return;
            }

            var baseNode = new GDpsx_ES_Node();
            GD.Print(data.nodes.Count);
            var _nodes = data.nodes;
            foreach(var node in data.nodes)
            {
                
                string name = node.NodeName;
                string[] actualName = name.Split('_');
                switch (node.nodeType)
                {
                    case NodeType.Dialog:
                        var dialog_node = node as GDpsx_ES_R_Dialogue;
                        var dialogueNodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Dialog_Node.tscn");
                        var dialogueNode = dialogueNodeScene.Instantiate() as GDpsx_ES_Dialog;
                        dialogueNode.title.Text = actualName[1];
                        dialogueNode.Title = actualName[1];
                        dialogueNode.Name = actualName[1];
                        dialogueNode.nodeType = NodeType.Dialog;
                        dialogueNode.SpeakingCharacter_Label.Text = dialog_node.character;
                        dialogueNode.message_Text.Text = dialog_node.message;
                        dialogueNode.PositionOffset = node.graphPosition;
                        baseNode = dialogueNode;
                        Nodes.Add(baseNode);
                        AddChild(dialogueNode);
                        break;
                    case NodeType.Function:
                        var function_node = node as GDpsx_ES_R_Function;
                        var funcNodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Function_Node.tscn");
                        var funcNode = funcNodeScene.Instantiate() as GDpsx_ES_Function;
                        
                        funcNode.title.Text = actualName[1];
                        funcNode.Title = actualName[1];
                        funcNode.Name = actualName[1];
                        funcNode.PositionOffset = node.graphPosition;
                        AddChild(funcNode);
                        
                        funcNode.ConstructDataFromResource(function_node);
                        baseNode = funcNode;
                        Nodes.Add(baseNode);
                        break;
                    case NodeType.Start:
                        var start_node = node as GDpsx_ES_R_StartNode;
                        var start_nodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Start_Node.tscn");
                        var startNode = start_nodeScene.Instantiate() as GDpsx_ES_Start;
                        startNode.title.Text = actualName[1]; 
                        startNode.Title = actualName[1];
                        startNode.Name = actualName[1];
                        startNode.PositionOffset = node.graphPosition;
                        AddChild(startNode);
                        baseNode = startNode;
                        Nodes.Add(baseNode);
                        break;
                    case NodeType.End:
                        var end_node = node as GDpsx_ES_R_EndNode;
                        var end_nodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_End_Node.tscn");
                        var endNode = end_nodeScene.Instantiate() as GDpsx_ES_End;
                        endNode.title.Text = actualName[1]; 
                        endNode.Title = actualName[1];
                        endNode.Name = actualName[1];
                        endNode.PositionOffset = node.graphPosition;
                        AddChild(endNode);
                        baseNode = endNode;
                        Nodes.Add(baseNode);
                        break;
                    case NodeType.GraphData:
                        var graph_node = node as GDpsx_ES_R_EndNode;
                        var graph_nodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_GraphData_Node.tscn");
                        var graphNode = graph_nodeScene.Instantiate() as GDpsx_ES_GraphDataNode;
                        graphNode.title.Text = actualName[1]; 
                        graphNode.Title = actualName[1];
                        graphNode.Name = actualName[1];
                        graphNode.PositionOffset = node.graphPosition;
                        AddChild(graphNode);
                        baseNode = graphNode;
                        Nodes.Add(baseNode);
                        break;
                    case NodeType.Conditional:
                        var conditional_node = node as GDpsx_ES_R_Conditional;
                        var conditionalNodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Condition_Node.tscn");
                        var conditionalNode = conditionalNodeScene.Instantiate() as GDpsx_ES_Condition;
                        conditionalNode.title.Text = actualName[1];
                        conditionalNode.Title = actualName[1];
                        conditionalNode.Name = actualName[1];
                        conditionalNode.PositionOffset = node.graphPosition;
                        
                        AddChild(conditionalNode);
                        
                        conditionalNode.ConstructDataFromResource(conditional_node);
                        baseNode = conditionalNode;
                        Nodes.Add(baseNode);
                        break;
                    case NodeType.Wait:
                        var wait_node = node as GDpsx_ES_R_WaitNode;
                        var waitNodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Wait_Node.tscn");
                        var waitNode = waitNodeScene.Instantiate() as GDpsx_ES_Wait_Node;
                        waitNode.title.Text = actualName[1];
                        waitNode.Title = actualName[1];
                        waitNode.Name = actualName[1];
                        waitNode.PositionOffset = node.graphPosition;
                        waitNode.spinBox.Value = wait_node.waitTimeMS;
                        
                        AddChild(waitNode);
                        
                        baseNode = waitNode;
                        Nodes.Add(baseNode);
                        break;


                    
                }
                
                
            }
            
            MakeConnectionsFromResource(data);
        }
        

        public async void MakeConnectionsFromResource(GDpsx_ES_R_Data data)
        {
            
            await Task.Delay(50);
            foreach(var node in data.nodes)
            {
                foreach(var connection in node.GotoNodes)
                {
                    var index = node.GotoNodes.IndexOf(connection);
                    GD.Print(node.NodeName +" " + node.GotoNodes.Count);
                    GD.Print($"{node.NodeName} {node.FromPorts[index].AsInt32()} {connection.AsString()} === {0}");
                    this.ConnectNode(node.NodeName, node.FromPorts[index].AsInt32(), connection.AsString(), 0);
                }
            }
            
        }



        public async void Load(string path)
        {
            GD.Print("Loading");
            EmptyGraph();
            await Task.Delay(100);
            var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
            var fileData = file.GetAsText(true);
            file.Close();
            var json_object = new Json();
            var parsedData = json_object.Parse(fileData);
            var data = json_object.Data.AsGodotDictionary();
            
            // Clear previous load
            Nodes.Clear();
            tempNodeMap.Clear();

            foreach (var key in data.Keys)
            {
                var nestedDictionary = data[key].AsGodotDictionary();
                var id = nestedDictionary["id"].AsStringName().ToString();
                var id_parts = id.Split('_');
                var node = LoadNodeFactory(StringToType(id_parts[0]), nestedDictionary);
                // Add node to the Nodes array
                

                // Use the node ID as the key for tempNodeMap
                var nodeId = nestedDictionary["id"].AsStringName().ToString();
                tempNodeMap[nodeId] = node;

                if(node != null)
                {
                    Nodes.Add(node);
                    AddChild(node);
                }
            }

            // After all nodes are instantiated, establish connections
            EstablishConnections(data);
            
            await Task.Delay(100);
            ReorderNodesByLowestX();
        }

        private void EstablishConnections(Dictionary data)
        {
            foreach (var key in data.Keys)
            {
                var nestedDictionary = data[key].AsGodotDictionary();
                if (nestedDictionary.ContainsKey("go to"))
                {
                    var nodeName = nestedDictionary["id"].AsStringName().ToString();
                    var gotoArray = nestedDictionary["go to"].AsGodotArray<StringName>();
                    foreach (StringName targetNodeName in gotoArray)
                    {
                        // Assuming you have a method to find a node by its ID within the GraphEdit
                        var fromNode = FindNodeById(nodeName);
                        var toNode = FindNodeById(targetNodeName.ToString());
                        
                        if (fromNode != null && toNode != null)
                        {
                            // Here you need to determine the appropriate slot indexes for your nodes
                            // For example, assuming slot 0 for simplicity
                            int fromSlot = 0;
                            int toSlot = 0;
                            
                            // Connect the nodes within the GraphEdit environment
                            this.ConnectNode(fromNode.Name, fromSlot, toNode.Name, toSlot);
                        }
                    }
                }
            }
        }

        private GraphNode FindNodeById(string nodeId)
        {
            foreach (Node child in GetChildren())
            {
                if (child is GraphNode && child.Name == nodeId)
                {
                    return child as GraphNode;
                }
            }
            return null;
        }

        public NodeType StringToType(string input)
        {
            switch(input)
            {
                case "Dialog":
                    return NodeType.Dialog;
                case "Function":
                    return NodeType.Function;
            }
            return NodeType.None;
        }
        public GDpsx_ES_Node LoadNodeFactory(NodeType type, Dictionary nestedDictionary)
        {
            var id = nestedDictionary["id"].AsStringName().ToString();
            var id_parts = id.Split('_');
            GDpsx_ES_Node baseNode = null;
            switch(type)
            {
                case NodeType.Dialog:
                    var dialogueNodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Dialog_Node.tscn");
                    var dialogueNode = dialogueNodeScene.Instantiate() as GDpsx_ES_Dialog;
                    dialogueNode.title.Text = id_parts[1];
                    dialogueNode.nodeType = NodeType.Dialog;
                    dialogueNode.dialogData = nestedDictionary["Data"].AsGodotDictionary();
                    
                    Godot.Vector2 position = new Godot.Vector2((float)nestedDictionary["Position X"], (float)nestedDictionary["Position X"]);
                    dialogueNode.PositionOffset = position;
                    
                    dialogueNode.ConstructDataFromDictionary(nestedDictionary["Data"].AsGodotDictionary(), nestedDictionary["id"].AsStringName());
                    baseNode = dialogueNode;
                    break;
                case NodeType.Function:
                    var funcNodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Function_Node.tscn");
                    var funcNode = funcNodeScene.Instantiate() as GDpsx_ES_Function;
                    funcNode.title.Text = id_parts[1];
                    funcNode.nodeType = NodeType.Function;
                    Godot.Vector2 funcposition = new Godot.Vector2((float)nestedDictionary["Position X"], (float)nestedDictionary["Position X"]);
                    funcNode.PositionOffset = funcposition;
                    
                    
                    funcNode.funcData = nestedDictionary["Data"].AsGodotDictionary();
                    funcNode.ConstructDataFromDictionary(nestedDictionary["id"].AsStringName());
                    baseNode = funcNode;
                    break;
            }
            return baseNode;
        }
    }
}
