using Godot;
using System;

[Tool]
public partial class GDpsx_ItemButton : Button
{
    public GDpsx_Item itemData;
    [Export] public GDpsx_ItemEditor itemEditor;
}
