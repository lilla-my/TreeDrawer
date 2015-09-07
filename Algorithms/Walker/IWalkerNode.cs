using System.Collections.Generic;

namespace Algorithm.Walker
{
    internal interface IWalkerNode
    {
        INodeStructure InnerNode { get; }
        float X { get; set; }
        int Y { get; }
        float Mod { get; set; }
        WalkerNode Parent { get; }
        List<WalkerNode> Children { get; }
        int Order { get; }
        WalkerNode AddChild(INodeStructure childInnerNode);
        WalkerNode GetLeftSibling ();
    }
}