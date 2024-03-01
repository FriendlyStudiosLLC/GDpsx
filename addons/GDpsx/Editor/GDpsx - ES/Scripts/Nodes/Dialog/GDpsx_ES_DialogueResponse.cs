using GDpsx_API.EventSystem;
using Godot;
using System;
using System.Drawing;
 

public partial class GDpsx_ES_DialogueResponse : HBoxContainer
{
    [Export] public LineEdit responseText;
    [Export] public int index;
    [Export] public int slotIndex;
    public GDpsx_ES_Dialog parentNode;
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
        GD.Print(index);
        var parent = GetParent() as GDpsx_ES_Dialog;
        parent.RemoveResponse(index);
    }
}