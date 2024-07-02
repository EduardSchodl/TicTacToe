using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMaxTests : MonoBehaviour, IUnitTestCollection
{
    [SerializeField]
    private bool disableTest = false;
    public bool IsTestDisabled()
    {
        return disableTest;
    }

    public IUnitTest[] GetUnitTests()
    {
        return new IUnitTest[] {
            SimpleMaxNodeTests,
            SimpleMinNodeTests,
            AdvancedNodeTests,
            AlphaBetaPruningTest,
            DeepAlphaBetaPruningTest,
            DeepVideoPruningTest,
            AlphaCutOff,
            BetaCutOff
        };
    }

    public IEnumerable<TestAssert> SimpleMaxNodeTests()
    {
        StaticMinMaxNode parentNode = new StaticMinMaxNode();
        parentNode.CreateChild(4);
        StaticMinMaxNode largestChild = parentNode.CreateChild(12);
        parentNode.CreateChild(9);
        parentNode.CreateChild(-56);

        var result = parentNode.AlphaEvaluateNodeAndValue();

        yield return new CmpTestAssert(result.pickedNode, largestChild);
        yield return new CmpTestAssert(result.pickedNodeValue, 12);
    }

    public IEnumerable<TestAssert> SimpleMinNodeTests()
    {
        StaticMinMaxNode maxNode = new StaticMinMaxNode();
        StaticMinMaxNode minNode = maxNode.CreateChild();

        minNode.CreateChild(-44);
        minNode.CreateChild(64);
        minNode.CreateChild(939);
        minNode.CreateChild(-992);
        minNode.CreateChild(5423);

        var result = maxNode.AlphaEvaluateNodeAndValue();

        yield return new CmpTestAssert(result.pickedNode, minNode);
        yield return new CmpTestAssert(result.pickedNodeValue, -992);
    }

    public IEnumerable<TestAssert> AdvancedNodeTests()
    {
        StaticMinMaxNode root = new StaticMinMaxNode();
        StaticMinMaxNode c1 = root.CreateChild();
        StaticMinMaxNode c2 = root.CreateChild();
        StaticMinMaxNode c3 = root.CreateChild();

        // C1
        c1.CreateChild(0);

        // C2
        StaticMinMaxNode c2_1 = c2.CreateChild();
        c2.CreateChild(18);

        // C2_1
        c2_1.CreateChild(2);
        c2_1.CreateChild(1);

        // C3
        StaticMinMaxNode c3_1 = c3.CreateChild();

        // C3_1
        c3_1.CreateChild(-2);
        c3_1.CreateChild(-23);

        var result = root.AlphaEvaluateNodeAndValue();
        yield return new CmpTestAssert(result.pickedNode, c2);
        yield return new CmpTestAssert(result.pickedNodeValue, 2);

    }

    public IEnumerable<TestAssert> AlphaBetaPruningTest()
    {
        StaticMinMaxNode A = new StaticMinMaxNode();
        StaticMinMaxNode B = A.CreateChild();
        StaticMinMaxNode C = A.CreateChild();

        StaticMinMaxNode D = B.CreateChild();
        D.CreateChild(2);
        D.CreateChild(3);

        StaticMinMaxNode E = B.CreateChild();
        E.CreateChild(5);
        E.CreateChild(9);

        StaticMinMaxNode F = C.CreateChild();
        F.CreateChild(0);
        F.CreateChild(1);

        StaticMinMaxNode G = C.CreateChild();
        G.CreateChild(7);
        G.CreateChild(5);

        var result = A.AlphaEvaluateNodeAndValue();
        yield return new CmpTestAssert(result.pickedNode, B);
        yield return new CmpTestAssert(result.pickedNodeValue, 3);
    }
    /// <summary>
    /// References for test node graph
    /// https://www.semanticscholar.org/paper/Alpha-beta-Pruning-in-Chess-Engines-Marckel/3c5cdc5fb590fc56ac429884216f8e0ce31c8164
    /// https://d3i71xaburhd42.cloudfront.net/3c5cdc5fb590fc56ac429884216f8e0ce31c8164/4-Figure3-1.png
    /// verified correctly written
    /// </summary>
    /// <returns></returns>
    public IEnumerable<TestAssert> DeepAlphaBetaPruningTest()
    {
        
        StaticMinMaxNode root = new StaticMinMaxNode();
        StaticMinMaxNode l = root.CreateChild();

        StaticMinMaxNode l_l = l.CreateChild();
        StaticMinMaxNode l_l_l = l_l.CreateChild();
        l_l_l.CreateChild(5);
        l_l_l.CreateChild(6);

        StaticMinMaxNode l_l_r = l_l.CreateChild();
        l_l_r.CreateChild(7);
        l_l_r.CreateChild(4);
        l_l_r.CreateChild(5);

        StaticMinMaxNode l_r = l.CreateChild();
        StaticMinMaxNode l_r_m = l_r.CreateChild();
        l_r_m.CreateChild(3);

        StaticMinMaxNode m = root.CreateChild();

        StaticMinMaxNode m_l = m.CreateChild();

        StaticMinMaxNode m_l_l = m_l.CreateChild();
        m_l_l.CreateChild(6);

        StaticMinMaxNode m_l_r = m_l.CreateChild();
        m_l_r.CreateChild(6);
        m_l_r.CreateChild(9);

        StaticMinMaxNode m_r = m.CreateChild();

        StaticMinMaxNode m_r_m = m_r.CreateChild();
        m_r_m.CreateChild(7);

        StaticMinMaxNode r = root.CreateChild();
        StaticMinMaxNode r_l = r.CreateChild();
        StaticMinMaxNode r_l_m = r_l.CreateChild();
        r_l_m.CreateChild(5);
        
        StaticMinMaxNode r_r = r.CreateChild();

        StaticMinMaxNode r_r_l = r_r.CreateChild();
        r_r_l.CreateChild(9);
        r_r_l.CreateChild(8);

        StaticMinMaxNode r_r_r = r_r.CreateChild();
        r_r_r.CreateChild(6);

        var result = root.AlphaEvaluateNodeAndValue();

        yield return new CmpTestAssert(result.pickedNodeValue, 6);
        yield return new CmpTestAssert(result.pickedNode, m);

    }

    /// <summary>
    /// source: https://www.youtube.com/watch?v=J1GoI5WHBto&list=PL8cyEDZjzLFXDg5_hRdK1sCG_tOhKqOVT&index=6&ab_channel=CSCITutorials
    /// at: 35:05
    /// verified correctly written
    /// unvisited nodes have been assigned artifical values
    /// </summary>
    public IEnumerable<TestAssert> DeepVideoPruningTest()
    {
        StaticMinMaxNode root = new StaticMinMaxNode();
        StaticMinMaxNode l = root.CreateChild();

        StaticMinMaxNode l_l = l.CreateChild();

        StaticMinMaxNode l_l_l = l_l.CreateChild();
        l_l_l.CreateChild(10);
        l_l_l.CreateChild(11);
        
        StaticMinMaxNode l_l_r = l_l.CreateChild();
        l_l_r.CreateChild(9);
        l_l_r.CreateChild(1111); // Unvisited 1

        StaticMinMaxNode l_r = l.CreateChild();

        StaticMinMaxNode l_r_l = l_r.CreateChild();
        l_r_l.CreateChild(14);
        l_r_l.CreateChild(15);

        // l_r_r
        l_r.CreateChild(2222); // Unvisited 2 - with 2 children (but with artifical value and no children)

        StaticMinMaxNode r = root.CreateChild();

        StaticMinMaxNode r_l = r.CreateChild();

        StaticMinMaxNode r_l_l = r_l.CreateChild();
        r_l_l.CreateChild(5);
        r_l_l.CreateChild(3333); // Unvisited 3

        StaticMinMaxNode r_l_r = r_l.CreateChild();
        r_l_r.CreateChild(4);
        r_l_r.CreateChild(4444); // Unvisited 4

        // r_r
        r.CreateChild(5555); // Unvisited 5 - with many children (artifical value and no children)

        var result = root.AlphaEvaluateNodeAndValue();

        yield return new CmpTestAssert(result.pickedNodeValue, 10);
    }

    public IEnumerable<TestAssert> AlphaCutOff()
    {
        StaticMinMaxNode root = new StaticMinMaxNode();
        root.CreateChild(4);

        StaticMinMaxNode r = root.CreateChild();
        r.CreateChild(1);
        // alpha cutoff
        r.CreateChild(1111);
        r.CreateChild(2222);

        var result = root.AlphaEvaluateNodeAndValue();
        yield return new CmpTestAssert(result.pickedNodeValue, 4);
    }

    public IEnumerable<TestAssert> BetaCutOff()
    {
        StaticMinMaxNode root = new StaticMinMaxNode();
        StaticMinMaxNode min_root = root.CreateChild();

        min_root.CreateChild(1);

        StaticMinMaxNode min_root_r = min_root.CreateChild();
        min_root_r.CreateChild(0);
        min_root_r.CreateChild(5);
        // beta cutoff
        min_root_r.CreateChild(1111);

        var result = root.AlphaEvaluateNodeAndValue();
        yield return new CmpTestAssert(result.pickedNodeValue, 1);
    }

}
