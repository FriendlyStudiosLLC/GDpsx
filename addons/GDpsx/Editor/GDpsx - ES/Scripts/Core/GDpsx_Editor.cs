using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

[Tool]
public partial class GDpsx_Editor : Control
{
    [Export] Window needBrain;

    [Export] public Button NewBrain;
    [Export] public Button LoadBrain;
    [Export] public FileDialog newBrainDialog;
    [Export] public FileDialog loadBrainDialog;
    [Export] public Label label;
    public GDpsx_GameBrain gameBrain;
    [Export] GDpsx_ItemEditor itemEditor;
    [Export] GDpsx_CharacterEditor charEditor;
    [Export] GDpsx_EventGraphSystem eventSystem;
    public Dictionary EditorData = new Dictionary();
    public string gameBrainPath;



    public void OpenNewBrain()
    {
        newBrainDialog.Visible = true;
    }
    public void OpenNeedBrain()
    {
        needBrain.Visible = true;
    }
    public void TabChanged(int tab)
    {
        if(gameBrain == null)
        {
            LoadEditorData();
        }
        itemEditor.UpdateBrain();
        charEditor.UpdateBrain();
        itemEditor.ClearItemData();
        charEditor.ClearCharData();
    }
    public void OpenLoadBrain()
    {
        loadBrainDialog.Visible = true;
    }

    public void MakeNewBrain(string path)
    {
        if(needBrain.Visible == true) needBrain.Visible = false;
        gameBrain = new GDpsx_GameBrain();
        ResourceSaver.Save(gameBrain, path+".res", ResourceSaver.SaverFlags.None);
        PerformLoadBrain(path+".res");
    }

    public void PerformLoadBrain(string path)
    {
        if(Engine.IsEditorHint())
        {
            var _gameBrain = ResourceLoader.Load(path, null, ResourceLoader.CacheMode.Ignore);
            gameBrainPath = path;
            gameBrain = _gameBrain as GDpsx_GameBrain;
            eventSystem.graph.gameBrain = gameBrain;
            label.Text = "Brain Path: "+ gameBrainPath;
            CreateEditorData();
        }
    }

    public void CreateEditorData()
    {
        if(Engine.IsEditorHint())
        {
        EditorData["GameBrain"] = gameBrainPath.ToString();
        var file = Godot.FileAccess.Open("res://addons/GDpsx/GDpsx_Editor.cfg",Godot.FileAccess.ModeFlags.WriteRead);
            file.StoreLine(Json.Stringify(EditorData));
            file.Close();
            }
    }
    public void LoadEditorData()
    {
        var file = Godot.FileAccess.Open("res://addons/GDpsx/GDpsx_Editor.cfg", Godot.FileAccess.ModeFlags.Read);
            var fileData = file.GetAsText(true);
            file.Close();
            if(file == null) return;
            var json_object = new Json();
            var parsedData = json_object.Parse(fileData);
            var data = json_object.Data.AsGodotDictionary();
            if(data["GameBrain"].ToString() != null)
            {
                PerformLoadBrain(data["GameBrain"].ToString());
            }
            else
            {
                OpenNeedBrain();
            }
    }
}
