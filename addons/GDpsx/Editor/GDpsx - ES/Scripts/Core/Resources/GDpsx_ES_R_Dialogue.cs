using Godot;
using Godot.Collections;
using System;
[Tool]
public partial class GDpsx_ES_R_Dialogue : GDpsx_ES_R_Node
{
    [Export] public string character;
    [Export] public string message;

    public GDpsx_ES_R_Dialogue(string Character, string Message)
    {
        character = Character; // Initialize Character with a specific string
        message = Message; // Initialize Message with a specific string
    }

     public GDpsx_ES_R_Dialogue()
    {
    }
    
}
