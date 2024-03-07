using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class GDpsx_dialog_box : Control
{
    [Export] public GDpsx_GlobalEventSystem eventSystem;
    [Export] public Label characterName;
    [Export] public Label message;
    [Export] public VBoxContainer buttonContainer;
    [Export] public Button NoReplyButton;
    



    public void DisplayDialogBox()
    {
        var player = GDpsx_API.GDpsx_Utility.GetPlayer(GetTree()) as GDpsx_HeroMovementBase;
        player.SetMouseMode(Input.MouseModeEnum.Confined);
        Visible = true;
        PopulateDialog();
    }

    public void HideDialogBox()
    {
        var player = GDpsx_API.GDpsx_Utility.GetPlayer(GetTree()) as GDpsx_HeroMovementBase;
        player.SetMouseMode(Input.MouseModeEnum.Captured);
        Visible = false;
    }

    public void PopulateDialog()
    {   ClearDialogCheck();
        var currentNode = eventSystem.currentNode as GDpsx_ES_R_Dialogue;
        
        characterName.Text = currentNode.character;
        message.Text = currentNode.message;

        Array<String> options = ReplyOptions(currentNode);
        if(options == null)
        {
            NoReplyButton.Visible = true;
            return;
        }
        else
        {
            NoReplyButton.Visible = false;
            foreach (var option in options)
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
        if(message.Text != null || buttonContainer.GetChildCount() != 0)
        {
            characterName.Text = null;
            message.Text = null;
            foreach(var child in buttonContainer.GetChildren())
            {
                child.QueueFree();
            }
        }
    }

    public Array<String> ReplyOptions(GDpsx_ES_R_Dialogue node)
    {
        Array<String> temp = new Array<String>();
        foreach (var _node in node.GotoNodes)
        {
            var optionNode = eventSystem.GetNodeByNodeName(_node.AsStringName()) as GDpsx_ES_R_Option;
            if(optionNode == null) return null;
            if(optionNode.hasRequirement)
            {
                var result = (bool)eventSystem.conditionLibary.Call(optionNode.methodName, eventSystem.ConvertToArray(optionNode.parameterList));
                if(result)
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
