using Godot;
using System;
using System.Reflection;

namespace GDpsx_API.EventSystem
{
	public partial class GDpsx_ES_ConditionalNodeLibrary : Node
	{
		public bool PlayerHasItem(string itemName, int amount)
		{
			GDpsx_Project.addons.GDpsx.Game.Scripts.First_Person.FPS_HeroMovement player =
				GDpsx_Project.addons.GDpsx.Game.Scripts.GDpsx_Utility.GetPlayer(GetTree()) as
					GDpsx_Project.addons.GDpsx.Game.Scripts.First_Person.FPS_HeroMovement;
			return player.Inventory.HasItem(itemName);
		}
		public bool PlayerStatEquals(string statName, double value)
		{
			return false;
		}
		public bool PlayerStatGreaterThan(string statName, double value)
		{
			return false;
		}
		public bool PlayerStatLessThan(string statName, double value)
		{
			return false;
		}
		public bool ObjectSolved(string objectName)
		{
			return false;
		}


	}
}
