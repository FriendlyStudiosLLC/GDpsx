using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
namespace GDpsx_API.EventSystem
{
    [Tool]
    public partial class GDpsx_ES_Dialog : GDpsx_ES_Node
    {
        [Export] public MenuButton SpeakingCharacter_Menu;
        [Export] public LineEdit SpeakingCharacter_Label;
        [Export] public TextEdit message_Text;
        [Export] public PackedScene responseScene;
        [Export] public Array<GDpsx_ES_DialogueResponse> responses = new Array<GDpsx_ES_DialogueResponse>();

        public Dictionary dialogData = new Dictionary();

        public override void _Ready()
        {
            nodeType = NodeType.Dialog;
        }

        public void ConstructDataFromDictionary(Dictionary _dialogData, string name)
        {
            Name = name;
            Dictionary data_dictionary = _dialogData[name].AsGodotDictionary();
            foreach(var data_key in data_dictionary.Keys)
            {
                SpeakingCharacter_Label.Text = data_dictionary["Character"].AsStringName();
                message_Text.Text = data_dictionary["Message"].ToString();
            }
            //dialogData = _dialogData;
            //SpeakingCharacter_Label.Text = _dialogData["Character"].AsString();
            //message_Text.Text = _dialogData["Message"].AsString();
        }

        public void ConstructDialogDictionary()
        {
            dialogData = new Dictionary();
            var dialogData_Template = new Dictionary();
            dialogData[Name] = dialogData_Template;
            dialogData_Template["Character"] = SpeakingCharacter_Label.Text;
            dialogData_Template["Message"] = message_Text.Text;
            dialogData_Template["Responses"] = new Godot.Collections.Array<string>();
            if(responses.Count != 0)
            {
                foreach(var response in responses)
                {
                    ((Godot.Collections.Array)dialogData_Template["Responses"]).Add(response.responseText.Text);
                }
            }
            dialogData[Name] = dialogData_Template;
        }

        public override void DeleteNode(bool bypassSelected)
        {
            base.DeleteNode(bypassSelected);
        }

        public void AddResponse(string name = "null")
        {
            var newResponse = responseScene.Instantiate() as GDpsx_ES_DialogueResponse;
            SetSlotEnabledRight(0, false);
            newResponse.parentNode = this;
            responses.Add(newResponse);
            var index = responses.IndexOf(newResponse);
            newResponse.index = index;
            newResponse.slotIndex = index+1;
            SetSlotEnabledRight((index+1), true);
            
            AddChild(newResponse);
        }

        public void RemoveResponse(int index = -1)
        {
            
            if(responses.Count == 0) return;
            if(index != -1)
            {
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
                responseAtIndex.QueueFree();
                responses.RemoveAt(responses.Count-1);
            }

            List<ConnectionDetails> connectionDetails = ParentGraph.GetConnectedNodesDetails(Name);
            foreach(var connection in connectionDetails)
            {
                ParentGraph.DisconnectNode(connection.From, connection.FromPort, connection.To, connection.ToSlot);
                ParentGraph.DisconnectNode(connection.To, connection.ToSlot, connection.From, connection.FromPort);
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
            var characters = ParentGraph.GameBrain.Characters;
            foreach(var character in characters)
            {
                SpeakingCharacter_Menu.GetPopup().AddItem(character);
            }
        }
    }
}