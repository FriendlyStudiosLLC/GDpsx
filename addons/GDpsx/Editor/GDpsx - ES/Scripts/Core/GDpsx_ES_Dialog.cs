using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using GDpsx_Project.addons.GDpsx.Game.Scripts;

namespace GDpsx_API.EventSystem
{
	[Tool]
	public partial class GDpsx_ES_Dialog : GDpsx_ES_Node
	{
		[Export] public MenuButton SpeakingCharacter_Menu;
		[Export] public LineEdit SpeakingCharacter_Label;
		[Export] public TextEdit message_Text;
		[Export] public PackedScene responseScene;

		public GDpsx_ES_R_Dialogue resource;

		public Dictionary dialogData = new Dictionary();

		public override void _Ready()
		{
			nodeType = NodeType.Dialog;
		}

		public void ConstructDataFromDictionary(Dictionary _dialogData, string name)
		{
			Name = name;
			Dictionary data_dictionary = _dialogData[name].AsGodotDictionary();
			foreach (Variant data_key in data_dictionary.Keys)
			{
				SpeakingCharacter_Label.Text = data_dictionary["Character"].AsStringName();
				message_Text.Text = data_dictionary["Message"].ToString();
			}
			//dialogData = _dialogData;
			//SpeakingCharacter_Label.Text = _dialogData["Character"].AsString();
			//message_Text.Text = _dialogData["Message"].AsString();
		}

		public void ConstructDialogResource()
		{
			GDpsx_ES_R_Dialogue ResourceData = new GDpsx_ES_R_Dialogue(SpeakingCharacter_Label.Text.ToString(), message_Text.Text.ToString());
			GD.Print(message_Text.Text);
			resource = ResourceData;
		}


		public void ConstructDialogDictionary()
		{
			dialogData = new Dictionary();
			Dictionary dialogData_Template = new Dictionary();
			dialogData[Name] = dialogData_Template;
			dialogData_Template["Character"] = SpeakingCharacter_Label.Text;
			dialogData_Template["Message"] = message_Text.Text;
			dialogData_Template["Responses"] = new Array<string>();

			dialogData[Name] = dialogData_Template;
		}

		public override void DeleteNode(bool bypassSelected)
		{
			base.DeleteNode(bypassSelected);
		}


		private void SetSpeakingCharacter(long index)
		{
			string characterName = SpeakingCharacter_Menu.GetPopup().GetItemText((int)index);
			SpeakingCharacter_Label.Text = characterName;
		}

		private void PopulateSpeakingCharacterMenu()
		{
			while (SpeakingCharacter_Menu.GetPopup().ItemCount != 0)
			{
				for (int i = 0; i < SpeakingCharacter_Menu.ItemCount; i++)
				{
					SpeakingCharacter_Menu.GetPopup().RemoveItem(i);
				}
			}
			Array<GDpsx_Character> characters = ParentGraph.gameBrain.Characters;
			foreach (GDpsx_Character character in characters)
			{
				SpeakingCharacter_Menu.GetPopup().AddItem(character.characterName);
			}
		}
	}
}
