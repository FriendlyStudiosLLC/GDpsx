using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
namespace GDpsx_API.EventSystem
{
    public partial class GDpsx_ES_End : GDpsx_ES_Node
    {
        public GDpsx_ES_R_EndNode resource;
    
        [Export] GDpsx_ES_R_Data gotoGraph;


        public void ConstructResource()
        {
            var resourceData = new GDpsx_ES_R_EndNode();
            if(gotoGraph != null) 
            {
                resourceData.GotoGraph = gotoGraph;
            }
            resource = resourceData;
        }
    }
}