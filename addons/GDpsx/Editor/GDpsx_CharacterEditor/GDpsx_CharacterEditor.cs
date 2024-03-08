using Godot;
using System;

[Tool]
public partial class GDpsx_CharacterEditor : Control
{
    [Export] public PackedScene charButton = (PackedScene)GD.Load("res://addons/GDpsx/Editor/GDpsx_CharacterEditor/GDpsx_CharacterButton.tscn");
    [Export] public GDpsx_Editor editor;
    [Export] public Button saveCharButton;
    [Export] public Button clearCharButton;
    [Export] public AnimationPlayer charSpinner;
    [Export] public Node3D charSceneRoot;
    [Export] public Button AddCharButton;
    [Export] public Button characterScene;
    [Export] public MenuButton characterTypeMenuButton;
    [Export] public FileDialog characterSceneDialog;
    [Export] public LineEdit characterNameBox;
    [Export] public SpinBox MaxHealth;
    [Export] public SpinBox DamagePerAttack;
    [Export] public SpinBox AttackRate;
    [Export] public VBoxContainer characterList;
    private int _charTypeSelectedIndex;

    public PackedScene character3DScene;
    [Export] public string gameBrainPath;

    public override void _Ready()
    {
        characterTypeMenuButton.GetPopup().IndexPressed += charTypeSelectedIndex;
    }

    public void OpenCharacterDialog()
    {
        characterSceneDialog.Visible = true;
    }
    public void GetCharacterSceneData(string path)
    {
        if(Engine.IsEditorHint())
        {
            if(charSceneRoot.GetChildCount() != 0)
            {
                foreach(var child in charSceneRoot.GetChildren())
                {
                    child.QueueFree();
                }
            }
            character3DScene = (PackedScene)GD.Load(path);
            var charScene = character3DScene.Instantiate();
            charSpinner.Play("RotateCharacter");
            charSceneRoot.AddChild(charScene);
        }
    }
     private void CreateNodeTypes() //Populates Menu Button with necessary options
        {
            characterTypeMenuButton.GetPopup().AddItem("None");
            characterTypeMenuButton.GetPopup().AddItem("NPC");
            characterTypeMenuButton.GetPopup().AddItem("Ped");
            characterTypeMenuButton.GetPopup().AddItem("Enemy");
            characterTypeMenuButton.GetPopup().AddItem("Prop");
        }
    public void charTypeSelectedIndex(long index)
    {
        _charTypeSelectedIndex = (int)index;
        characterTypeMenuButton.Text = characterTypeMenuButton.GetPopup().GetItemText(_charTypeSelectedIndex);
    }

    public void ClearCharData()
    {
        characterNameBox.Text = "";
        MaxHealth.Value = 0;
        DamagePerAttack.Value = 0;
        AttackRate.Value = 0;
        characterTypeMenuButton.Text = "Select Type";
        if(charSceneRoot.GetChildCount() != 0)
            {
                foreach(var child in charSceneRoot.GetChildren())
                {
                    child.QueueFree();
                }
            }
        
    }
    public void AddChar()
    {
        ClearCharData();
    }
    public void SaveChar()
    {
        var _tempChar = new GDpsx_Character();
        _tempChar.characterName = characterNameBox.Text;
        _tempChar.MaxHealth = (int)MaxHealth.Value;
        _tempChar.DamagePerAttack = (int)DamagePerAttack.Value;
        _tempChar.AttackRate = (int)AttackRate.Value;
        _tempChar.CharacterScene = character3DScene;
        
        
        foreach(var character in editor.gameBrain.Characters)
        {
            var charIndex = editor.gameBrain.Characters.IndexOf(character);
            if(character.characterName == _tempChar.characterName)
            {
                editor.gameBrain.Characters.RemoveAt(charIndex);
                editor.gameBrain.Characters.Insert(charIndex, _tempChar);
                UpdateBrain();
                GD.Print(editor.gameBrainPath);
                SaveBrain(editor.gameBrainPath);
                return;
            }
        }
        
        editor.gameBrain.Characters.Add(_tempChar);
        
        ClearCharData();
        UpdateBrain();
        SaveBrain(gameBrainPath);
    }

    public void DeleteChar()
    {
        foreach(var _char in editor.gameBrain.Characters)
        {
            if(_char.characterName == characterNameBox.Text)
            {
                editor.gameBrain.Characters.Remove(_char);
            }
        }
        ClearCharData();
        UpdateBrain();
        SaveBrain(editor.gameBrainPath);
    }



    public CharacterType charTypeFromIndex(long Index)
    {
        switch(Index)
        {
            case 0:
                return CharacterType.None;
            case 1:
                return CharacterType.NPC;
            case 2:
                return CharacterType.Ped;
            case 3:
                return CharacterType.Enemy;
            case 4:
                return CharacterType.Prop;
        }
        return CharacterType.None;
    }
    
    public void NewBrain()
    {

    }

    public void LoadBrain(string path)
    {
    }

    public void UpdateBrain()
    {
        if(editor.gameBrain == null)
        {
            return;
        }
        foreach(var child in characterList.GetChildren())
        {
            child.QueueFree();
        }
        
        foreach(var character in editor.gameBrain.Characters)
        {
            if(character == null) return;
            GD.Print($"Character name: {character.characterName}");
            var button = charButton.Instantiate() as GDpsx_CharacterButton;
            button.characterEditor = this;
            button.character = character;
            button.Text = character.characterName;
            button.Pressed += () => LoadCharacter(button.character);
            characterList.AddChild(button);
        }
    }

    public long CharTypeToIndex(CharacterType type)
    {
        switch(type)
        {
            case CharacterType.None:
                return 0;
            case CharacterType.NPC:
                return 1;
            case CharacterType.Ped:
                return 2;
            case CharacterType.Enemy:
                return 3;
            case CharacterType.Prop:
                return 4;
        }
        return -1;
    }

    public void LoadCharacter(GDpsx_Character charData)
    {

        characterNameBox.Text = charData.characterName;
        MaxHealth.Value = charData.MaxHealth;
        DamagePerAttack.Value = charData.DamagePerAttack;
        AttackRate.Value = charData.AttackRate;
        character3DScene = charData.CharacterScene;
    }

    public void ResetMenuButton()
    {
        if(Engine.IsEditorHint())
        {
            while(characterTypeMenuButton.ItemCount != 0)
            {
                for(int i = 0; i<characterTypeMenuButton.ItemCount; i++)
                {
                    characterTypeMenuButton.GetPopup().RemoveItem(i);
                }
            }
            if(characterTypeMenuButton.ItemCount == 0)
            {
                CreateNodeTypes();
            }
        }

    }
    
    public void SaveBrain(string path)
    {
        
            ResourceSaver.Save(editor.gameBrain, editor.gameBrainPath, ResourceSaver.SaverFlags.None);
        

    }


}
