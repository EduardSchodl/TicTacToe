using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticMinMaxNode : MinMaxNode
{
    private int? nodeValue;

    public StaticMinMaxNode() : base()
    {

    }

    public StaticMinMaxNode(StaticMinMaxNode parent, int depth, int? nodeValue) : base(parent, depth)
    {
        this.nodeValue = nodeValue;
    }

    protected override int RunTerminalNodeEvaluation()
    {
        if (!nodeValue.HasValue)
        {
            Debug.LogError($"Tried to fetch a value from a valueless static node.");
            return 0;
        }

        return nodeValue.Value;
    }

    public StaticMinMaxNode CreateChild(int? nodeValue)
    {
        StaticMinMaxNode child = new StaticMinMaxNode(this, Depth + 1, nodeValue);
        ChildNodes.AddLast(child);
        return child;
    }

    public StaticMinMaxNode CreateChild()
    {
        return CreateChild(null);
    }
}
