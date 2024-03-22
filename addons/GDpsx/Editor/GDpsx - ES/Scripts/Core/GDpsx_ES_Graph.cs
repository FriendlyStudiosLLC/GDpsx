using Godot;
using Godot.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Vector2 = Godot.Vector2;

namespace GDpsx_API.EventSystem
{
	[Tool]
	public partial class GDpsx_ES_Graph : GraphEdit
	{

		[Export] public MenuButton AddNode_MenuButton;
		[Export] public GDpsx_EventGraphSystem GraphSystem;
		public GDpsx_Project.addons.GDpsx.Game.Scripts.GDpsxGameBrain gameBrain;
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
			foreach (GDpsx_ES_Node node in Nodes)
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
			foreach (GDpsx_ES_Node node in nodeList)
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
						GDpsx_ES_Node temp = Nodes[j];
						Nodes[j] = Nodes[j + 1];

						sortedIndex += 1;
						temp.index = sortedIndex;
						Nodes[j + 1] = temp;
					}
				}
			}
		}

		public void RefreshMenuButton()
		{
			if (Engine.IsEditorHint())
			{
				while (AddNode_MenuButton.ItemCount != 0)
				{
					for (int i = 0; i < AddNode_MenuButton.ItemCount; i++)
					{
						AddNode_MenuButton.GetPopup().RemoveItem(i);
					}
				}

				if (AddNode_MenuButton.ItemCount == 0) CreateNodeTypes();

			}
		}


		private void AddNodeOfType(long index)
		{


			Vector2 pos = GetLocalMousePosition();
			PackedScene NodeScene = new PackedScene();
			switch (index)
			{
				case 0:
					NodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Start_Node.tscn");
					break;
				case 1:
					NodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Dialog_Node.tscn");
					break;
				case 2:
					NodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Option_Node.tscn");
					break;
				case 3:
					NodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Condition_Node.tscn");
					break;
				case 4:
					NodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Function_Node.tscn");
					break;
				case 5:
					NodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_End_Node.tscn");
					break;
				case 6:
					NodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Wait_Node.tscn");
					break;
				case 7:
					NodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_GraphData_Node.tscn");
					break;
			}
			GDpsx_ES_Node Node = NodeScene.Instantiate() as GDpsx_ES_Node;
			Node.PositionOffset = pos;
			Node.ParentGraph = this;
			int nodeIndex = Nodes.Count;
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
			for (int i = Nodes.Count - 1; i >= 0; i--)
			{
				Nodes[i].DeleteNode(true);
			}
		}

		public GDpsx_ES_Node GetNodeByName(StringName nodeName)
		{
			foreach (GDpsx_ES_Node node in Nodes)
			{

				if (node.Name == nodeName)
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
			List<ConnectionDetails> connectionDetailsList = new List<ConnectionDetails>();

			foreach (Dictionary connection in GetConnectionList())
			{
				if (connection["from_node"].AsStringName() == nodeName)
				{
					connectionDetailsList.Add(new ConnectionDetails{
						From = (StringName)connection["from_node"],
						To = (StringName)connection["to_node"],
						FromPort = (int)connection["from_port"],
						ToSlot = (int)connection["to_port"]
					});
				}
				else if (connection["to_node"].AsStringName() == nodeName)
				{
					connectionDetailsList.Add(new ConnectionDetails{
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
			AddNode_MenuButton.GetPopup().AddItem("Reply Option Node");
			AddNode_MenuButton.GetPopup().AddItem("Conditional Node");
			AddNode_MenuButton.GetPopup().AddItem("Function Node");
			AddNode_MenuButton.GetPopup().AddItem("End Node");
			AddNode_MenuButton.GetPopup().AddItem("Wait Node");
			AddNode_MenuButton.GetPopup().AddItem("Graph Node");
		}

		public async void Save(string path)
		{
			await Task.Delay(200);
			FileAccess file = FileAccess.Open(path, FileAccess.ModeFlags.Write);
			file.StoreLine(Json.Stringify(data));
			file.Close();
		}

		public async void CollectNodeData(string path)
		{

			Array<GDpsx_ES_R_Node> nodeData = new Array<GDpsx_ES_R_Node>();
			ReorderNodesByLowestX();
			await Task.Delay(100);

			GDpsx_ES_Node lastSavedNode = new GDpsx_ES_Node();
			foreach (GDpsx_ES_Node node in Nodes)
			{
				lastSavedNode = node;
				string nodeName = node.Title;
				NodeType nodeType = node.nodeType;
				string nameCheck = node.nodeType.ToString() + "_";
				if (nodeName == nameCheck)
				{
					GD.PrintErr("Every node must have a name");
					return;
				}
				Vector2 nodePosition = node.PositionOffset;
				Godot.Collections.Array GotoNodes = new Godot.Collections.Array();
				Godot.Collections.Array FromPorts = new Godot.Collections.Array();
				foreach (Dictionary connection in GetConnectionList())
				{
					if (connection["from_node"].AsStringName() == node.Name)
					{
						GotoNodes.Add(connection["to_node"].AsStringName());
						FromPorts.Add(connection["from_port"].AsInt32());
					}
				}
				switch (node.nodeType)
				{
					case NodeType.Dialog:
						GDpsx_ES_Dialog dialogNode = node as GDpsx_ES_Dialog;
						dialogNode.ConstructDialogResource();
						GDpsx_ES_R_Dialogue dialogResourceData = dialogNode.resource;
						dialogResourceData.NodeName = nodeName;
						dialogResourceData.graphPosition = nodePosition;
						dialogResourceData.nodeType = nodeType;
						dialogResourceData.GotoNodes = GotoNodes;
						dialogResourceData.FromPorts = FromPorts;
						nodeData.Add(dialogResourceData);
						break;

					case NodeType.Conditional:
						GDpsx_ES_Condition conditionalNode = node as GDpsx_ES_Condition;
						conditionalNode.ConstructConditionalData();
						GDpsx_ES_R_Conditional conditionaResourceData = conditionalNode.resource;
						conditionaResourceData.NodeName = nodeName;
						conditionaResourceData.graphPosition = nodePosition;
						conditionaResourceData.nodeType = nodeType;
						conditionaResourceData.GotoNodes = GotoNodes;
						conditionaResourceData.FromPorts = FromPorts;
						nodeData.Add(conditionaResourceData);
						break;

					case NodeType.Option:
						GDpsx_ES_Option optionalNode = node as GDpsx_ES_Option;
						optionalNode.ConstructOptionData();
						GDpsx_ES_R_Option optionalResourceData = optionalNode.resource;
						optionalResourceData.NodeName = nodeName;
						optionalResourceData.graphPosition = nodePosition;
						optionalResourceData.nodeType = nodeType;
						optionalResourceData.GotoNodes = GotoNodes;
						optionalResourceData.FromPorts = FromPorts;
						nodeData.Add(optionalResourceData);
						break;

					case NodeType.Function:
						GDpsx_ES_Function functionNode = node as GDpsx_ES_Function;
						functionNode.ConstructFunctionData();
						GDpsx_ES_R_Function funcResourceData = functionNode.resource;
						funcResourceData.NodeName = nodeName;
						funcResourceData.graphPosition = nodePosition;
						funcResourceData.nodeType = nodeType;
						funcResourceData.GotoNodes = GotoNodes;
						funcResourceData.FromPorts = FromPorts;
						nodeData.Add(funcResourceData);
						break;
					case NodeType.Start:
						GDpsx_ES_Start startNode = node as GDpsx_ES_Start;
						startNode.ConstructResource();
						GDpsx_ES_R_StartNode startResourceData = startNode.resource;
						startResourceData.NodeName = nodeName;
						startResourceData.graphPosition = nodePosition;
						startResourceData.nodeType = nodeType;
						startResourceData.GotoNodes = GotoNodes;
						startResourceData.FromPorts = FromPorts;
						nodeData.Add(startResourceData);
						break;
					case NodeType.End:
						GDpsx_ES_End endNode = node as GDpsx_ES_End;
						endNode.ConstructResource();
						GDpsx_ES_R_EndNode endResourceData = endNode.resource;
						endResourceData.NodeName = nodeName;
						endResourceData.graphPosition = nodePosition;
						endResourceData.nodeType = nodeType;
						endResourceData.GotoNodes = GotoNodes;
						endResourceData.FromPorts = FromPorts;
						nodeData.Add(endResourceData);
						break;
					case NodeType.Wait:
						GDpsx_ES_Wait_Node waitNode = node as GDpsx_ES_Wait_Node;
						double value = waitNode.spinBox.Value;
						GDpsx_ES_R_WaitNode waitResourceData = new GDpsx_ES_R_WaitNode();
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
			GDpsx_ES_R_Data graphData = new GDpsx_ES_R_Data(nodeData);
			graphData.ResourceName = Name;
			if (Nodes[0].nodeType != NodeType.Start)
			{
				GD.PrintErr("Start Node Required");
				return;
			}
			ResourceSaver.Save(graphData, path + ".res", ResourceSaver.SaverFlags.None);
		}

		public async void ConsolidateData(string path)
		{
			ReorderNodesByLowestX();
			await Task.Delay(100);
			data = new Dictionary();
			foreach (GDpsx_ES_Node node in Nodes)
			{
				Dictionary data_template = new Dictionary();
				data[node.Title] = data_template;
				data_template["id"] = node.Title;

				data_template["index"] = node.index;
				data_template["Position X"] = node.PositionOffset.X;
				data_template["Position Y"] = node.PositionOffset.Y;
				data_template["go to"] = new Godot.Collections.Array();
				data_template["from port"] = new Godot.Collections.Array();
				foreach (Dictionary connection in GetConnectionList())
				{
					if (connection["from_node"].AsStringName() == node.Name)
					{
						((Godot.Collections.Array)data_template["go to"]).Add(connection["to_node"].ToString());
						((Godot.Collections.Array)data_template["from port"]).Add(connection["from_port"].AsInt32());
					}
				}

				switch (node.nodeType)
				{
					case NodeType.Dialog:
						GDpsx_ES_Dialog dialogNode = node as GDpsx_ES_Dialog;
						dialogNode.ConstructDialogDictionary();
						data_template["Data"] = dialogNode.dialogData;
						break;

					case NodeType.Conditional:
						break;

					case NodeType.Function:
						GDpsx_ES_Function functionNode = node as GDpsx_ES_Function;
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
			EmptyGraph();
			await Task.Delay(100);
			Resource dataTest = ResourceLoader.Load(path, null, ResourceLoader.CacheMode.Ignore);
			GDpsx_ES_R_Data data = dataTest as GDpsx_ES_R_Data;
			if (data == null || data.nodes == null)
			{
				GD.PrintErr("Failed to load resource or nodes are null: " + path);
				return;
			}

			GDpsx_ES_Node baseNode = new GDpsx_ES_Node();
			Array<GDpsx_ES_R_Node> _nodes = data.nodes;
			foreach (GDpsx_ES_R_Node node in data.nodes)
			{

				string name = node.NodeName;
				string[] actualName = name.Split('_');
				switch (node.nodeType)
				{
					case NodeType.Dialog:
						GDpsx_ES_R_Dialogue dialog_node = node as GDpsx_ES_R_Dialogue;
						PackedScene dialogueNodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Dialog_Node.tscn");
						GDpsx_ES_Dialog dialogueNode = dialogueNodeScene.Instantiate() as GDpsx_ES_Dialog;
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
						GDpsx_ES_R_Function function_node = node as GDpsx_ES_R_Function;
						PackedScene funcNodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Function_Node.tscn");
						GDpsx_ES_Function funcNode = funcNodeScene.Instantiate() as GDpsx_ES_Function;

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
						GDpsx_ES_R_StartNode start_node = node as GDpsx_ES_R_StartNode;
						PackedScene start_nodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Start_Node.tscn");
						GDpsx_ES_Start startNode = start_nodeScene.Instantiate() as GDpsx_ES_Start;
						startNode.title.Text = actualName[1];
						startNode.Title = actualName[1];
						startNode.Name = actualName[1];
						startNode.PositionOffset = node.graphPosition;
						AddChild(startNode);
						baseNode = startNode;
						Nodes.Add(baseNode);
						break;
					case NodeType.End:
						GDpsx_ES_R_EndNode end_node = node as GDpsx_ES_R_EndNode;
						PackedScene end_nodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_End_Node.tscn");
						GDpsx_ES_End endNode = end_nodeScene.Instantiate() as GDpsx_ES_End;
						endNode.title.Text = actualName[1];
						endNode.Title = actualName[1];
						endNode.Name = actualName[1];
						endNode.PositionOffset = node.graphPosition;
						AddChild(endNode);
						baseNode = endNode;
						Nodes.Add(baseNode);
						break;
					case NodeType.GraphData:
						GDpsx_ES_R_EndNode graph_node = node as GDpsx_ES_R_EndNode;
						PackedScene graph_nodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_GraphData_Node.tscn");
						GDpsx_ES_GraphDataNode graphNode = graph_nodeScene.Instantiate() as GDpsx_ES_GraphDataNode;
						graphNode.title.Text = actualName[1];
						graphNode.Title = actualName[1];
						graphNode.Name = actualName[1];
						graphNode.PositionOffset = node.graphPosition;
						AddChild(graphNode);
						baseNode = graphNode;
						Nodes.Add(baseNode);
						break;
					case NodeType.Conditional:
						GDpsx_ES_R_Conditional conditional_node = node as GDpsx_ES_R_Conditional;
						PackedScene conditionalNodeScene =
							(PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Condition_Node.tscn");
						GDpsx_ES_Condition conditionalNode = conditionalNodeScene.Instantiate() as GDpsx_ES_Condition;
						conditionalNode.title.Text = actualName[1];
						conditionalNode.Title = actualName[1];
						conditionalNode.Name = actualName[1];
						conditionalNode.PositionOffset = node.graphPosition;

						AddChild(conditionalNode);

						conditionalNode.ConstructDataFromResource(conditional_node);
						baseNode = conditionalNode;
						Nodes.Add(baseNode);
						break;
					case NodeType.Option:
						GDpsx_ES_R_Option option_node = node as GDpsx_ES_R_Option;
						PackedScene optionNodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Option_Node.tscn");
						GDpsx_ES_Option optionNode = optionNodeScene.Instantiate() as GDpsx_ES_Option;
						optionNode.title.Text = actualName[1];
						optionNode.Title = actualName[1];
						optionNode.Name = actualName[1];
						optionNode.PositionOffset = node.graphPosition;

						AddChild(optionNode);

						optionNode.ConstructDataFromResource(option_node);
						baseNode = optionNode;
						Nodes.Add(baseNode);
						break;
					case NodeType.Wait:
						GDpsx_ES_R_WaitNode wait_node = node as GDpsx_ES_R_WaitNode;
						PackedScene waitNodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Wait_Node.tscn");
						GDpsx_ES_Wait_Node waitNode = waitNodeScene.Instantiate() as GDpsx_ES_Wait_Node;
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
			foreach (GDpsx_ES_R_Node node in data.nodes)
			{
				foreach (Variant connection in node.GotoNodes)
				{
					int index = node.GotoNodes.IndexOf(connection);
					ConnectNode(node.NodeName, node.FromPorts[index].AsInt32(), connection.AsString(), 0);
				}
			}

		}


		public async void Load(string path)
		{
			EmptyGraph();
			await Task.Delay(100);
			FileAccess file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
			string fileData = file.GetAsText(true);
			file.Close();
			Json json_object = new Json();
			Error parsedData = json_object.Parse(fileData);
			Dictionary data = json_object.Data.AsGodotDictionary();

			// Clear previous load
			Nodes.Clear();
			tempNodeMap.Clear();

			foreach (Variant key in data.Keys)
			{
				Dictionary nestedDictionary = data[key].AsGodotDictionary();
				string id = nestedDictionary["id"].AsStringName().ToString();
				string[] id_parts = id.Split('_');
				GDpsx_ES_Node node = LoadNodeFactory(StringToType(id_parts[0]), nestedDictionary);
				// Add node to the Nodes array


				// Use the node ID as the key for tempNodeMap
				string nodeId = nestedDictionary["id"].AsStringName().ToString();
				tempNodeMap[nodeId] = node;

				if (node != null)
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
			foreach (Variant key in data.Keys)
			{
				Dictionary nestedDictionary = data[key].AsGodotDictionary();
				if (nestedDictionary.ContainsKey("go to"))
				{
					string nodeName = nestedDictionary["id"].AsStringName().ToString();
					Array<StringName> gotoArray = nestedDictionary["go to"].AsGodotArray<StringName>();
					foreach (StringName targetNodeName in gotoArray)
					{
						// Assuming you have a method to find a node by its ID within the GraphEdit
						GraphNode fromNode = FindNodeById(nodeName);
						GraphNode toNode = FindNodeById(targetNodeName.ToString());

						if (fromNode != null && toNode != null)
						{
							// Here you need to determine the appropriate slot indexes for your nodes
							// For example, assuming slot 0 for simplicity
							int fromSlot = 0;
							int toSlot = 0;

							// Connect the nodes within the GraphEdit environment
							ConnectNode(fromNode.Name, fromSlot, toNode.Name, toSlot);
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
			switch (input)
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
			string id = nestedDictionary["id"].AsStringName().ToString();
			string[] id_parts = id.Split('_');
			GDpsx_ES_Node baseNode = null;
			switch (type)
			{
				case NodeType.Dialog:
					PackedScene dialogueNodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Dialog_Node.tscn");
					GDpsx_ES_Dialog dialogueNode = dialogueNodeScene.Instantiate() as GDpsx_ES_Dialog;
					dialogueNode.title.Text = id_parts[1];
					dialogueNode.nodeType = NodeType.Dialog;
					dialogueNode.dialogData = nestedDictionary["Data"].AsGodotDictionary();

					Godot.Vector2 position = new Godot.Vector2((float)nestedDictionary["Position X"], (float)nestedDictionary["Position X"]);
					dialogueNode.PositionOffset = position;

					dialogueNode.ConstructDataFromDictionary(nestedDictionary["Data"].AsGodotDictionary(), nestedDictionary["id"].AsStringName());
					baseNode = dialogueNode;
					break;
				case NodeType.Function:
					PackedScene funcNodeScene = (PackedScene)ResourceLoader.Load("res://addons/GDpsx/Editor/GDpsx - ES/Objects/GDpsx_Function_Node.tscn");
					GDpsx_ES_Function funcNode = funcNodeScene.Instantiate() as GDpsx_ES_Function;
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
