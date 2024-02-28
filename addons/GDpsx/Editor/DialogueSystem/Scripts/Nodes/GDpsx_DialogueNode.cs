using Godot;
using System;
using System.Drawing;
 
using System.Net;
using Godot.Collections;
using Microsoft.VisualBasic;
using GDpsx_API;
using System.Collections.Generic;
using GDpsx_API.EventSystem;

public partial class GDpsx_DialogueNode : GDpsx_GraphNode
{
    [Export] public MenuButton SpeakingCharacter_Menu;
    [Export] public LineEdit SpeakingCharacter_Label;
    [Export] public TextEdit message_Text;
    
    [Export] public PackedScene responseScene;
    [Export] public Array<GDpsx_DialogueResponse> responses = new Array<GDpsx_DialogueResponse>();

    [Export] public GDpsx_DialogueNodeResource data;
    [Export] public CheckBox isResponseCheckBox;

    
    public string name;
    public string message;
    public bool isResponse = false;
    public override void _Ready()
    {
        SetIsResponse(isResponse);
        data = new GDpsx_DialogueNodeResource();
        PopulateSpeakingCharacterMenu();
        SpeakingCharacter_Menu.GetPopup().IndexPressed += SetSpeakingCharacter;
    }
    public void UpdateData()
    {
        data.speakerName = SpeakingCharacter_Label.Text;
        GD.Print(data.speakerName);
        data.nodeID = titleLine.Text;
        GD.Print(data.nodeID);
        data.message = message_Text.Text;
        GD.Print(data.message);
    }

    public void SetNodeTitle(string title = "Dialogue Box")
    {
        this.Name = title;
    }

    public override void DeleteNode(bool bypassSelected)
    {
        for(int i = responses.Count - 1; i >= 0; i--)
        {
            RemoveResponse(1);
        }
        base.DeleteNode(bypassSelected);

    }

    public void ConstructResponseLink(StringName fromNode, int fromPort, StringName toNode, int toPort)
    {
        var responseAtIndex = responses[fromPort-1];
        var responseText = responseAtIndex.responseText.Text;
        ResponseItem responseItem = new ResponseItem();
        responseItem.toNode = toNode;
        responseItem.responseText = responseText;
        responseAtIndex.data = responseItem;
        data.responses.Insert(fromPort-1, responseItem);
    }

    

    public void AddResponse()
    {
        var newResponse = responseScene.Instantiate() as GDpsx_DialogueResponse;
        SetSlotEnabledRight(0, false);
        responses.Add(newResponse);
        var index = responses.IndexOf(newResponse);
        newResponse.index = index;
        newResponse.slotIndex = index+1;
        SetSlotEnabledRight((index+1), true);
        
        AddChild(newResponse);
    }

    public void SetIsResponse(bool value)
    {
        isResponse = value;
        
        isResponseCheckBox.ButtonPressed = value;
        SetSlotEnabledLeft(0, value);
    }
    public void RemoveResponse(int index = -1)
    {
        
        if(responses.Count == 0) return;
        if(index != -1)
        {
        
        //GD.Print();
        GD.Print($"index: {index} ||| count: {responses.Count}");
            if(responses.Count == 1) 
            {
                responses[0].QueueFree();
                responses.RemoveAt(0);
            }
            else
            {
                responses[index].QueueFree();
                responses.RemoveAt(index);
            }
        }
        else
        {
        var responseAtIndex = responses[responses.Count-1];
        var slotIndex = responseAtIndex.slotIndex;
        data.responses.RemoveAt(slotIndex);
        GD.Print(responses.Count);
        responseAtIndex.QueueFree();
        responses.RemoveAt(responses.Count-1);
        GD.Print(responses.Count);
        
        
    }

        List<ConnectionDetails> connectionDetails = parentGraph.GetConnectedNodesDetails(parentGraph.graphEdit, Name);
        foreach(var connection in connectionDetails)
        {
            GD.Print(connection.From + " ||| " + connection.To + " ||| " + connection.ToSlot + " ||| " + connection.FromPort);
            parentGraph.graphEdit.DisconnectNode(connection.From, connection.FromPort, connection.To, connection.ToSlot);
            parentGraph.graphEdit.DisconnectNode(connection.To, connection.ToSlot, connection.From, connection.FromPort);
        }

        if(responses.Count == 0)
        {
            SetSlotEnabledRight(0, true);
        }
    }

    private void SetSpeakingCharacter(long index)
    {
        string characterName = SpeakingCharacter_Menu.GetPopup().GetItemText((int)index);
        SpeakingCharacter_Label.Text = characterName;
    }

    private void PopulateSpeakingCharacterMenu()
    {
        var characters = parentGraph.Brain.Characters;
        foreach(var character in characters)
        {
            SpeakingCharacter_Menu.GetPopup().AddItem(character);
        }
    }

}