using Godot;

namespace GDpsx_Project.addons.GDpsx.Game.Scripts
{
	public partial class GDpsx_UI_Template : Control
	{
		[Export] public TextureRect Crosshair;
		[Export] public Texture2D Regular_Crosshair_Texture;
		[Export] public Texture2D Interact_Crosshair_Texture;
		[Export] public Label InteractionLabel;
		[Export] public ProgressBar HealthBar;


		public void UpdateHealthBar(float value)
		{
			HealthBar.Value = value;
		}

		public void SetCrosshair(bool interacting)
		{
			if (interacting)
			{
				Crosshair.Texture = Interact_Crosshair_Texture;
			}
			else
			{
				Crosshair.Texture = Regular_Crosshair_Texture;
			}
		}


		public void SetLabelText(string message)
		{
			InteractionLabel.Text = message;
		}
	}
}
