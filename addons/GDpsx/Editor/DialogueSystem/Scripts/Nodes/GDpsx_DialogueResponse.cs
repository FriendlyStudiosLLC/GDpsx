using Godot;
using System;
using System.Drawing;
 

public partial class GDpsx_DialogueResponse : HBoxContainer
{
    [Export] public LineEdit responseText;
    [Export] public int index;
    [Export] public int slotIndex;
    public GDpsx_DialogueNode parentNode;
    [Export] public ResponseItem data;

    public override void _Ready()
    {
        responseText.PlaceholderText = $"Response {slotIndex}";
    }



    public string GetResponseText()
    {
        if(responseText.Text == "") return null;
        return responseText.Text;
    }

    public void SetResponseText(string newText)
    {
        responseText.Text = newText;
    }

    public void RemoveSelf()
    {
        var parent = GetParent() as GDpsx_DialogueNode;
        parent.RemoveResponse(index);
    }
}