namespace GDpsx_Project.addons.GDpsx.Game.Scripts
{
	internal interface IWeapon
	{
		void PrimaryUse();
		void SecondaryUse();

		void Reload();

		void Attack();
	}
}