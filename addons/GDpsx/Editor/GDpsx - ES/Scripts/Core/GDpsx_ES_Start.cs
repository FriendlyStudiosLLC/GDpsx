using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
namespace GDpsx_API.EventSystem
{
    public partial class GDpsx_ES_Start : GDpsx_ES_Node
    {
        public GDpsx_ES_R_StartNode resource;

        public void ConstructResource()
        {
            resource = new GDpsx_ES_R_StartNode();
        }
    }
}
