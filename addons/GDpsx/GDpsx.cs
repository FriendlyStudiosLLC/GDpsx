#if TOOLS
using Godot;
using System;

[Tool]
public partial class GDpsx : EditorPlugin
{
	private Control editor;
	public override void _EnterTree()
	{
		// var editorInterface = EditorInterface.Singleton;
		// var editorScene = GD.Load<PackedScene>("res://addons/GDpsx/Editor/DialogueSystem/Objects/DialogueGraph.tscn");
		// editor = (Control)editorScene.Instantiate();

		

		// editorInterface.GetEditorMainScreen().AddChild(editor);
		// var editorSettings = editorInterface.GetEditorSettings();
        // var baseColor = (Color)editorSettings.GetSetting("interface/theme/base_color");
        // editor.Set("base_color", baseColor.V < 0.5 ? Colors.White : Colors.Black); // Adjust property name if necessary
        // _MakeVisible(false);

		//AddCustomType("GDpsx | DialogueBox", "Panel",
		//	GD.Load<Script>("res://addons/GDpsx/Editor/DialogueSystem/Scripts/GDpsx_Dialogue_Graph.cs"),
		//	GD.Load<Texture2D>("res://addons/GDpsx/Editor/Icons/Dialogue.svg")
		//);


		// Initialization of the plugin goes here.
		AddCustomType("GDpsx | State Machine", "Node",
			GD.Load<Script>("res://addons/GDpsx/Game/Scripts/GDpsx_StateMachine.cs"),
			GD.Load<Texture2D>("res://addons/GDpsx/Editor/Icons/StateMachine.png")
		);

		AddCustomType("GDpsx | State", "Node",
			GD.Load<Script>("res://addons/GDpsx/Game/Scripts/GDpsx_State.cs"),
			GD.Load<Texture2D>("res://addons/GDpsx/Editor/Icons/State.png")
		);

		AddCustomType("GDpsx | Inventory Component", "Node",
			GD.Load<Script>("res://addons/GDpsx/Game/Scripts/Inventory/GDpsx_Inventory.cs"),
			GD.Load<Texture2D>("res://addons/GDpsx/Editor/Icons/InventoryComponent.png")
		);
	}

	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.
		if(editor != null)
		{
			editor.QueueFree();
		}

		//RemoveCustomType("GDpsx | DialogueBox");
	}

    public override bool _HasMainScreen()
    {
        return true;
    }

	private void MakeVisible(bool visible)
    {
        if (editor != null)
        {
            editor.Visible = visible;
        }
    }

    public override string _GetPluginName()
    {
        return "GDpsx";
    }

    public override Texture2D _GetPluginIcon()
    {
        return GD.Load<Texture2D>("res://addons/GDpsx/Editor/Icons/Dialogue.svg");
    }

    public override void _SaveExternalData()
    {
        if (editor != null)
        {
            // Assuming `files` is a property of your editor and `SaveAll` is a method you've defined
            // You'll need to properly reference and invoke your method based on your editor's implementation
            // This line is a placeholder and needs adjustment
            //((Node)editor.Get("files")).Call("save_all");
        }
    }
}
#endif
