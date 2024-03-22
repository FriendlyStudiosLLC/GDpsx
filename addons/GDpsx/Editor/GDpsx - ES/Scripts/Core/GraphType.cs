namespace GDpsx_API.EventSystem
{
	public enum GraphType
	{
		None,
		Event, //Can run parallel or drive quest progression
		Dialog, //Can run parallel or drive quest progression
		Quest //Only one quest can be done at a time.
	}
}