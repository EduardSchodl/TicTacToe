using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class LargeFieldComputationTests : MonoBehaviour, IUnitTestCollection
{

    [SerializeField]
    private bool testDisabled;

    public IUnitTest[] GetUnitTests()
    {
        return new IUnitTest[]
        {
            Run15x15Test
        };
    }

    public IEnumerable<TestAssert> Run15x15Test()
    {

        Field f = new Field("| | | | | | | | | | | | | | | |"
                          + "| | | | | | | | | | | | | | | |"
                          + "| | | | | | | | | | | | | | | |"
                          + "| | | | | | | | | | | | | | | |"
                          + "| | | | | | | | | |x| | | | | |"
                          + "| | | | | | | | |o| | | | | | |"
                          + "| | | | | | |o|o|o|o|x| | | | |"
                          + "| | | | | | |o|x| | | | | | | |"
                          + "| | | | | |o|x| |x| | | | | | |"
                          + "| | | | |x| | | | | | | | | | |"
                          + "| | | | | | | | | | | | | | | |"
                          + "| | | | | | | | | | | | | | | |"
                          + "| | | | | | | | | | | | | | | |"
                          + "| | | | | | | | | | | | | | | |"
                          + "| | | | | | | | | | | | | | | |"
            );


        Stopwatch stopwatch = new Stopwatch();
        

        GameSetupValues.INSTANCE.ServerSetPiecesNeededToWin(5);

        MinMaxFieldNode root = new MinMaxFieldNode(null, f, Player.CIRCLE);

        stopwatch.Start();
        
        root.GetBestTreeMoveDepth(4);

        stopwatch.Stop();

        DebugUtil.PrintStopwatchTime(stopwatch);

        yield break;
    }



    public bool IsTestDisabled()
    {
        return testDisabled;
    }

}
