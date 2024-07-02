using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class NewFieldIterationTests : MonoBehaviour, IUnitTestCollection
{
    [SerializeField]
    private bool isDisabled;

    public IUnitTest[] GetUnitTests()
    {
        return new IUnitTest[]
        {
            RunSpeedTest
        };
    }

    public bool IsTestDisabled()
    {
        return isDisabled;
    }

    public IEnumerable<TestAssert> RunSpeedTest()
    {

        GameSetupValues.INSTANCE.ServerSetPiecesNeededToWin(3);

        Field f = new Field("| | | |" +
                            "| |o| |" +
                            "| | | |");

        MinMaxFieldNode root = new MinMaxFieldNode(null, f, Player.CIRCLE);

        Stopwatch stopwatch = new Stopwatch();

        stopwatch.Start();
        root.AlphaDepthEvalNodeAndValue(int.MaxValue);
        stopwatch.Stop();

        DebugUtil.PrintStopwatchTime(stopwatch);
        

        yield break;
    }

}
