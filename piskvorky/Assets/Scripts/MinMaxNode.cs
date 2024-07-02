using System.Collections.Generic;
using UnityEngine;

public abstract class MinMaxNode
{
    /// <summary>
    /// The parent node of this node in the tree.
    /// </summary>
    public MinMaxNode Parent
    {
        get;
        protected set;
    }

    public LinkedList<MinMaxNode> ChildNodes
    {
        get;
        protected set;
    } = new LinkedList<MinMaxNode>();

    public bool HasParent()
    {
        return Parent != null;
    }

    public T GetParentTyped<T>() where T : MinMaxNode
    {
        return Parent as T;
    }

    public const int MAX_ALLOWED_DEPTH = 100;

    /// <summary>
    /// Root node starts at Depth 0.
    /// </summary>
    public int Depth
    {
        get;
        protected set;
    } = 0;

    public MinMaxNode(): this(null, 0)
    {

    }

    public MinMaxNode(MinMaxNode parent, int depth)
    {
        Parent = parent;
        Depth = depth;

        if (Depth > MAX_ALLOWED_DEPTH)
        {
            Debug.LogError($"Tried to create a node at depth ({Depth}) over the maximum depth value of ({MAX_ALLOWED_DEPTH})");
            return;
        }
    }

    protected abstract int RunTerminalNodeEvaluation();

    public virtual IEnumerable<MinMaxNode> GetNextChildNode()
    {
        foreach(MinMaxNode childNode in ChildNodes)
        {
            yield return childNode;
        }
    }

    public bool IsRoot()
    {
        return Depth == 0;
    }
    public bool IsMaximizing()
    {
        return Depth % 2 == 0;
    }

    public (MinMaxNode pickedNode, int pickedNodeValue, LinkedList<(MinMaxNode node, int nodeValue)> rootProcessedNodes) EvaluateNodeAndValue()
    {
        return EvaluateNodeAndValue(false, int.MinValue, int.MaxValue, int.MaxValue);
    }

    public (MinMaxNode pickedNode, int pickedNodeValue, LinkedList<(MinMaxNode node, int nodeValue)> rootProcessedNodes) AlphaEvaluateNodeAndValue()
    {
        return EvaluateNodeAndValue(true, int.MinValue, int.MaxValue, int.MaxValue);
    }

    public (MinMaxNode pickedNode, int pickedNodeValue, LinkedList<(MinMaxNode node, int nodeValue)> rootProcessedNodes) AlphaDepthEvalNodeAndValue(int maxSearchDepth)
    {
        return EvaluateNodeAndValue(true, int.MinValue, int.MaxValue, maxSearchDepth);
    }

    public (MinMaxNode pickedNode, int pickedNodeValue, LinkedList<(MinMaxNode node, int nodeValue)> rootProcessedNodes) EvaluateNodeAndValue(bool usePruning, int alpha, int beta, int maxSearchDepth)
    {
        bool maximizing = IsMaximizing();

        // Pick either the largest or the smallest
        // child depending on whether we are maximizing or minimizing.
        MinMaxNode suitableChild = null;
        int? suitableChildValue = null;
        int nodesProcessed = 0;

        LinkedList<(MinMaxNode node, int nodeValue)> rootProcessedNodes = IsRoot() ? new LinkedList<(MinMaxNode node, int nodeValue)>() : null;

        if(Depth > maxSearchDepth)
        {
            Debug.LogError($"Tried to evaluate a node at a value larger than the maximum search depth. ({Depth} > {maxSearchDepth})");
            return (null, 0, rootProcessedNodes);
        }

        // Nodes with children at max Depth value are considered
        // terminal and should not seek to get the value from their children.
        if(Depth == maxSearchDepth)
        {
            return (this, RunTerminalNodeEvaluation(), rootProcessedNodes);
        }

        foreach (MinMaxNode childNode in GetNextChildNode())
        {
            // First elements are picked automatically (state initialization)
            bool isFirstElement = nodesProcessed == 0;
            
            var result = childNode.EvaluateNodeAndValue(usePruning, alpha, beta, maxSearchDepth);

            if(rootProcessedNodes != null)
            {
                rootProcessedNodes.AddLast((result.pickedNode, result.pickedNodeValue));
            }
            
            ++nodesProcessed;

            int childNodeValue = result.pickedNodeValue;

            // Check whether the child is larger or smaller than what we have already
            // depending on whether we are maximizing or minimizing.
            bool foundBetterValue = (maximizing ? (childNodeValue > suitableChildValue) : (childNodeValue < suitableChildValue));
            bool useNode = isFirstElement || foundBetterValue;

            // Not the first element and not better than the node we already have
            if (!useNode)
            {
                continue;
            }

            // Sucessfully found better child
            suitableChild = childNode;
            suitableChildValue = childNodeValue;
            

            if(!usePruning)
            {
                continue;
            }
            
            // ## ALPHA BETA PRUNING ##
            if (maximizing)
            {
                alpha = System.Math.Max(alpha, suitableChildValue.Value);

            }
            else
            {
                beta = System.Math.Min(beta, suitableChildValue.Value);
            }


            
            if (usePruning && (alpha >= beta))
            {
                break;
            }
            

        }

        // Terminal nodes without children evaluate
        // the value themselves.
        if (suitableChild == null)
        {
            return (this, RunTerminalNodeEvaluation(), rootProcessedNodes);
        }

        return (suitableChild, suitableChildValue.Value, rootProcessedNodes);
    }


}
