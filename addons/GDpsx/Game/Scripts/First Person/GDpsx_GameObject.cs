using Godot;
using System;
using GDpsx_API;
using Godot.Collections;
using System.Threading.Tasks;



public partial class GDpsx_GameObject : Node3D, IInteractable
{
    
    [Export] private string _objectID;

    [Export] public bool HasLookAtMessage = true;

    [Export] public string LookAtMessage = "Change Me";
    [Export] public bool InfiniteUses = false;
    [Export] public int Uses = -1; //If -1, use infinite times
    private float cachedUses;
    
    [Export] public Array<GDpsx_InteractionEventBase> EventChain =  new Array<GDpsx_InteractionEventBase>();
    
    private bool FailedEvent = false;

    public override void _Ready()
    {
       cachedUses = Uses;
    }

    private async void performEventExecution_Async(GDpsx_InteractionEventBase eventBase)
    {
        if(FailedEvent) 
        {
            if(Uses < cachedUses) Uses++;
            return;
        }
        if(eventBase.WaitTime == 0)
        {
            eventBase.Call("Enter", GetTree(), (GDpsx_GameObject)this);
            return;
        }
        await Task.Delay((int)eventBase.WaitTime * 1000);
        
        eventBase.Call("Enter", GetTree(), (GDpsx_GameObject)this);
    }

    public void EnterInteract()
    {
        if(FailedEvent == true) 
        {
            SetFailedEvent(false);
        }
        if(!CanUse()) return;
        foreach(var eventBase in EventChain)
        {
            if(eventBase.HasMethod("Enter"))
            {
                performEventExecution_Async(eventBase);
            }
        }
    }

    public Node GetNodeFromPath(NodePath nodePath)
    {
        return GetNode(nodePath);
    }

    public void SetFailedEvent(bool value)
    {
        FailedEvent = value;
    }

    public bool CanUse()
    {
        if(InfiniteUses)
        {
            return true;
        }
        if(Uses > 0)
        {
            Uses--;
            return true;
        }
        return false;

    }

    public void ExitInteract()
    {
        throw new NotImplementedException();
    }

    public string GetLookatMessage()
    {
        if(HasLookAtMessage)
        {
            return LookAtMessage;
        }
        return "";
        
    }

    public Node3D PerformLookAt()
    {
        throw new NotImplementedException();
    }

    string IInteractable.LookAtMessage()
    {
        throw new NotImplementedException();
    }
}