using GDpsx_Project.addons.GDpsx.Game.Scripts.EventSystem.Core;
using Godot;
using Godot.Collections;

namespace GDpsx_Project.addons.GDpsx.Game.Scripts
{

	public partial class GDpsx_Utility : Node
	{


		public static GDpsx_HeroMovementBase GetPlayer(SceneTree tree)
		{
			Array<Node> playerGroup = tree.GetNodesInGroup("Player");
			GDpsx_HeroMovementBase player = playerGroup[0] as GDpsx_HeroMovementBase;
			return player;
		}

		public static EventSystem.Core.GDpsx_GlobalEventSystem GetEventSystem(SceneTree tree)
		{
			Array<Node> ES_Group = tree.GetNodesInGroup("EventSystem");
			if (ES_Group.Count != 0)
			{
				GDpsx_GlobalEventSystem ES = ES_Group[0] as EventSystem.Core.GDpsx_GlobalEventSystem;
				return ES;
			}
			else
			{
				return null;
			}

		}

	}


}
