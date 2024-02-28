using Godot;
using System;
 


    public enum EventType
    {
        Start,
        End,
        Conditional,
        Dialogue,
        DialogueOption,
        Function,
    }

    public enum EventFunctionType
    {
        None,
        GiveItem,
        ChangeHealth,
        HasItem,
        TakeItem,
        PlaySound,
        PlayAnimation,
        SpawnObject,
        TeleportPlayer,
        PlayVideo,
        DisplayMessage,
    }
