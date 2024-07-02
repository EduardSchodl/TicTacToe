using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using UnityEngine;

public class FieldBestMoveJob
{
    private BackgroundWorker workerThread;

    private Field field;
    private Player previousPlayer;
    
    private Vector2Int workResult;

    private LinkedList<FieldJobStartEvent> onJobStarts = new LinkedList<FieldJobStartEvent>();
    private LinkedList<FieldJobEndEvent> onJobEnds = new LinkedList<FieldJobEndEvent>();

    public void AddOnJobStartHook(FieldJobStartEvent onJobStart)
    {
        onJobStarts.AddLast(onJobStart);
    }

    public void AddOnJobEndHook(FieldJobEndEvent onJobEnd)
    {
        onJobEnds.AddLast(onJobEnd);
    }

    public void RunOnJobStarts()
    {
        foreach(FieldJobStartEvent onJobStart in onJobStarts)
        {
            onJobStart();
        }
    }

    public void RunOnJobEnds(Vector2Int bestMoveResult)
    {
        foreach(FieldJobEndEvent onJobEnd in onJobEnds)
        {
            onJobEnd(bestMoveResult);
        }
    }

    public FieldBestMoveJob(Field field, Player previousPlayer)
    {
        this.field = field;
        this.previousPlayer = previousPlayer;

        workerThread = new BackgroundWorker();

        workerThread.DoWork += PerformJob;
        workerThread.RunWorkerCompleted += JobCompleted;
    }

    public void StartJob()
    {
        RunOnJobStarts();

        workerThread.RunWorkerAsync();
    }

    public void JobCompleted(object obj, AsyncCompletedEventArgs args)
    {
        RunOnJobEnds(workResult);
    }

    public void PerformJob(object obj, DoWorkEventArgs args)
    {
        MinMaxFieldNode rootNode = null;

        // Field is copied in the root node and all additional nodes
        // therefore we only need it during creation.
        lock (field)
        {
            rootNode = MinMaxFieldNode.CreateRootNode(field, previousPlayer);
        }

        MinMaxFieldNode.AI_EVALUATION_STOPWATCH.Restart();

        Vector2Int bestMove = rootNode.GetBestTreeMove();

        Debug.Log("Evaluation time: ");
        DebugUtil.PrintStopwatchTime(MinMaxFieldNode.AI_EVALUATION_STOPWATCH);

        lock(this)
        {
            workResult = bestMove;
        }
    }

}

public delegate void FieldJobStartEvent();
public delegate void FieldJobEndEvent(Vector2Int bestMove);