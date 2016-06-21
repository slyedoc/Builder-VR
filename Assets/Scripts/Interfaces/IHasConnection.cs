using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


interface IHasConnection
{
    void ReplaceConnection(Connector child, ConnectionJoint target);
}

