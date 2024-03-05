using Godot;
using System;

public partial class GDpsx_GlobalEventSystem : Node
{
    [Export] public GDpsx_ES_R_Data data;
    [Export] public GDpsx_dialog_box dialog_Box;

    public void SetEventPath(string path)
    {
        var dataTest = ResourceLoader.Load("res://Test_data.tres", "res://addons/GDpsx/Editor/GDpsx - ES/Scripts/Core/Resources/GDpsx_ES_R_Data.cs", ResourceLoader.CacheMode.Ignore);
        var _data = dataTest as GDpsx_ES_R_Data;
        data = _data;
    }


    
    
}
