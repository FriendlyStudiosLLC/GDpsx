using Godot;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

[Tool]
public partial class GDpsx_ItemEditor : Control
{
    [Export] public PackedScene itemButton = (PackedScene)GD.Load("res://addons/GDpsx/Editor/GDpsx_ItemsEditor/Objects/GDpsx_ItemButton.tscn");
    [Export] public GDpsx_Editor editor;
    [Export] public Button saveItemButton;
    [Export] public Button clearItemButton;
    [Export] public Button AddItemButton;
    [Export] public Button itemIconButton;
    [Export] public Button loadItemScene;
    [Export] public Button loadItemEquippedScene;
    [Export] public TextureRect itemIcon;
    [Export] public MenuButton itemTypeMenuButton;
    [Export] public FileDialog itemIconDialog;
    [Export] public FileDialog itemSceneDialog;
    [Export] public FileDialog itemEquippedSceneDialog;
    [Export] public TextEdit descriptionBox;
    [Export] public LineEdit itemNameBox;
    [Export] public SpinBox maxStackSizeBox;
    [Export] public VBoxContainer itemList;
    private int _itemTypeSelectedIndex;

    public Texture2D itemTexture;
    public PackedScene itemScene;
    public PackedScene itemEquippedScene;
    [Export] public string gameBrainPath;

    public override void _Ready()
    {
        itemTypeMenuButton.GetPopup().IndexPressed += itemTypeSelectedIndex;
    }

    public void OpenIconDialog()
    {
        itemIconDialog.Visible = true;
    }
    public void GetItemIconData(string path)
    {
        itemTexture = (Texture2D)GD.Load(path);
        itemIcon.Texture = itemTexture;
    }
    public void GetItemSceneData(string path)
    {
        itemScene = (PackedScene)GD.Load(path);
    }
    public void GetItemEquippedSceneData(string path)
    {
        
        itemEquippedScene = (PackedScene)GD.Load(path);
    }
    public void OpenEquippedSceneDialog()
    {
        
        itemEquippedSceneDialog.Visible = true;
    }
    public void OpenSceneDialog()
    {
        itemSceneDialog.Visible = true;
    }
     private void CreateNodeTypes() //Populates Menu Button with necessary options
        {
            itemTypeMenuButton.GetPopup().AddItem("None");
            itemTypeMenuButton.GetPopup().AddItem("Weapon");
            itemTypeMenuButton.GetPopup().AddItem("Tool");
            itemTypeMenuButton.GetPopup().AddItem("Consumable");
            itemTypeMenuButton.GetPopup().AddItem("Key");
            itemTypeMenuButton.GetPopup().AddItem("Quest");
            itemTypeMenuButton.GetPopup().AddItem("Armor");
        }
    public void itemTypeSelectedIndex(long index)
    {
        _itemTypeSelectedIndex = (int)index;
        
        itemTypeMenuButton.Text = itemTypeMenuButton.GetPopup().GetItemText(_itemTypeSelectedIndex);
    }

    public void ClearItemData()
    {
        descriptionBox.Text = "";
        itemNameBox.Text = "";
        itemIcon.Texture = null;
        itemTypeMenuButton.Text = "Select Type";
        
    }
    public void AddItem()
    {
        ClearItemData();
    }
    public void SaveItem()
    {
        var _tempItem = new GDpsx_Item();
        _tempItem.itemName = itemNameBox.Text;
        _tempItem.itemDescription = descriptionBox.Text;
        _tempItem.maxStackSize = (int)maxStackSizeBox.Value;
        _tempItem.itemType = itemTypeFromIndex(_itemTypeSelectedIndex);
        _tempItem.itemIcon = itemTexture;
        _tempItem.pickupScene = itemScene;
        _tempItem.equippedScene = itemEquippedScene;
        
        
        foreach(var item in editor.gameBrain.Items)
        {
            var itemIndex = editor.gameBrain.Items.IndexOf(item);
            if(item.itemName == _tempItem.itemName)
            {
                editor.gameBrain.Items.RemoveAt(itemIndex);
                editor.gameBrain.Items.Insert(itemIndex, _tempItem);
                UpdateBrain();
                GD.Print(editor.gameBrainPath);
                SaveBrain(editor.gameBrainPath);
                return;
            }
        }
        
        editor.gameBrain.Items.Add(_tempItem);
        
        ClearItemData();
        UpdateBrain();
        SaveBrain(gameBrainPath);
    }

    public void DeleteItem()
    {
        foreach(var item in editor.gameBrain.Items)
        {
            if(item.itemName == itemNameBox.Text)
            {
                editor.gameBrain.Items.Remove(item);
            }
        }
        ClearItemData();
        UpdateBrain();
        SaveBrain(editor.gameBrainPath);
    }



    public ItemType itemTypeFromIndex(long Index)
    {
        switch(Index)
        {
            case 0:
                return ItemType.None;
            case 1:
                return ItemType.Weapon;
            case 2:
                return ItemType.Tool;
            case 3:
                return ItemType.Consumable;
            case 4:
                return ItemType.Key;
            case 5:
                return ItemType.Quest;
            case 6:
                return ItemType.Armor;
        }
        return ItemType.None;
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
        foreach(var child in itemList.GetChildren())
        {
            child.QueueFree();
        }
        
        foreach(var item in editor.gameBrain.Items)
        {
            if(item == null) return;
            GD.Print($"Item name: {item.itemName}");
            var button = itemButton.Instantiate() as GDpsx_ItemButton;
            button.itemEditor = this;
            button.itemData = item;
            button.Text = item.itemName;
            button.Pressed += () => LoadItem(button.itemData);
            itemList.AddChild(button);
        }
    }

    public long ItemTypeToIndex(ItemType type)
    {
        switch(type)
        {
            case ItemType.None:
                return 0;
            case ItemType.Weapon:
                return 1;
            case ItemType.Tool:
                return 2;
            case ItemType.Consumable:
                return 3;
            case ItemType.Key:
                return 4;
            case ItemType.Quest:
                return 5;
            case ItemType.Armor:
                return 6;
        }
        return -1;
    }

    public void LoadItem(GDpsx_Item itemData)
    {

        itemNameBox.Text = itemData.itemName;
        descriptionBox.Text = itemData.itemDescription;
        maxStackSizeBox.Value = itemData.maxStackSize;
        itemTypeMenuButton.GetPopup().SetFocusedItem((int)ItemTypeToIndex(itemData.itemType));
        itemTexture = itemData.itemIcon;
        itemIcon.Texture = itemTexture;
        itemScene = itemData.pickupScene;
        itemEquippedScene = itemData.equippedScene;
    }

    public void ResetMenuButton()
    {
        if(Engine.IsEditorHint())
        {
            while(itemTypeMenuButton.ItemCount != 0)
            {
                for(int i = 0; i<itemTypeMenuButton.ItemCount; i++)
                {
                    itemTypeMenuButton.GetPopup().RemoveItem(i);
                }
            }
            if(itemTypeMenuButton.ItemCount == 0)
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
