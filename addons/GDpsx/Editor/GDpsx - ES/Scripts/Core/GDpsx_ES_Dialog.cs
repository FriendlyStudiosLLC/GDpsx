using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
namespace GDpsx_API.EventSystem
{
    public partial class GDpsx_ES_Dialog : GDpsx_ES_Node
    {
        [Export] public MenuButton SpeakingCharacter_Menu;
        [Export] public LineEdit SpeakingCharacter_Label;
        [Export] public TextEdit message_Text;
        [Export] public PackedScene responseScene;
        [Export] public Array<GDpsx_DialogueResponse> responses = new Array<GDpsx_DialogueResponse>();



        public void PopulateCharacterList()
        {

        }

        public override void DeleteNode(bool bypassSelected)
        {
            base.DeleteNode(bypassSelected);
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
                GD.Print(responses.Count);
                responseAtIndex.QueueFree();
                responses.RemoveAt(responses.Count-1);
                GD.Print(responses.Count);
            }

            List<ConnectionDetails> connectionDetails = ParentGraph.GetConnectedNodesDetails(Name);
            foreach(var connection in connectionDetails)
            {
                GD.Print(connection.From + " ||| " + connection.To + " ||| " + connection.ToSlot + " ||| " + connection.FromPort);
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