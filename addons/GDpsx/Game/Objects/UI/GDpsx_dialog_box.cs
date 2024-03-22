using Godot;
using Godot.Collections;
using System;
using System.Linq;
using GDpsx_Project.addons.GDpsx.Game.Scripts;

public partial class GDpsx_dialog_box : Control
{
	[Export] public GDpsx_Project.addons.GDpsx.Game.Scripts.EventSystem.Core.GDpsx_GlobalEventSystem eventSystem;
	[Export] public Label characterName;
	[Export] public Label message;
	[Export] public VBoxContainer buttonContainer;
	[Export] public Button NoReplyButton;


	public void DisplayDialogBox()
	{
		GDpsx_HeroMovementBase player = GDpsx_Utility.GetPlayer(GetTree()) as GDpsx_HeroMovementBase;
		player.SetMouseMode(Input.MouseModeEnum.Confined);
		Visible = true;
		PopulateDialog();
	}

	public void HideDialogBox()
	{
		GDpsx_HeroMovementBase player = GDpsx_Utility.GetPlayer(GetTree()) as GDpsx_HeroMovementBase;
		player.SetMouseMode(Input.MouseModeEnum.Captured);
		Visible = false;
	}

	public void PopulateDialog()
	{
		ClearDialogCheck();
		GDpsx_ES_R_Dialogue currentNode = eventSystem.currentNode as GDpsx_ES_R_Dialogue;

		characterName.Text = currentNode.character;
		message.Text = currentNode.message;

		Array<string> options = ReplyOptions(currentNode);
		if (options == null)
		{
			NoReplyButton.Visible = true;
			return;
		}
		else
		{
			NoReplyButton.Visible = false;
			foreach (string option in options)
			{
				Button optionButton = new Button();
				optionButton.Text = option;
				optionButton.Pressed += () => eventSystem.ProcessResponse(option);
				buttonContainer.AddChild(optionButton);
			}
		}
	}

	public void GotoNextDialog()
	{
		eventSystem.GotoNextNode();
	}


	private void ClearDialogCheck()
	{
		if (message.Text != null || buttonContainer.GetChildCount() != 0)
		{
			characterName.Text = null;
			message.Text = null;
			foreach (Node child in buttonContainer.GetChildren())
			{
				child.QueueFree();
			}
		}
	}

	public Array<string> ReplyOptions(GDpsx_ES_R_Dialogue node)
	{
		Array<string> temp = new Array<string>();
		foreach (Variant _node in node.GotoNodes)
		{
			GDpsx_ES_R_Option optionNode = eventSystem.GetNodeByNodeName(_node.AsStringName()) as GDpsx_ES_R_Option;
			if (optionNode == null) return null;
			if (optionNode.hasRequirement)
			{
				bool result = (bool)eventSystem.conditionLibary.Call(optionNode.methodName, eventSystem.ConvertToArray(optionNode.parameterList));
				if (result)
				{
					GD.Print(optionNode.OptionText);
					temp.Add(optionNode.OptionText);
				}
			}
			else
			{
				GD.Print(optionNode.OptionText);
				temp.Add(optionNode.OptionText);
			}
		}
		return temp;
	}
}
