using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameAssert : TestAssert
{
    private const string FAIL_MESSAGE_BASE = "Field should have been ";
    private const string UNENDED_MESSAGE = "unfinished";
    private const string DRAW_MESSAGE = "a draw";
    private const string WIN_MESSAGE = "a win for {0}";

    private Field checkedField;
    private bool expectedGameEnded;
    private Player? expectedWinningPlayer;

    private static string ConstructFailMessageAppend(bool gameEnded, Player? winningPlayer)
    {
        return $" - values: (gameEnded: {gameEnded}, winningPlayer: {winningPlayer})";
    }

    private string GetInnerStateString()
    {
        if (expectedGameEnded && !expectedWinningPlayer.HasValue)
        {
            return DRAW_MESSAGE;
        }

        if (!expectedGameEnded && !expectedWinningPlayer.HasValue)
        {
            return UNENDED_MESSAGE;
        }

        return string.Format(WIN_MESSAGE, expectedWinningPlayer);
    }

    public EndGameAssert(Field checkedField, bool expectedGameEnded, Player? expectedWinningPlayer)
    {
        this.checkedField = checkedField;
        this.expectedGameEnded = expectedGameEnded;
        this.expectedWinningPlayer = expectedWinningPlayer;
    }

    public override (bool assertSucceded, string failMessage) TestAssertMethod()
    {
        (bool checkGameEnded, Player? checkWinningPlayer) = EndChecker.CheckGameEnded(checkedField);
        bool expectedValuesMatch = expectedGameEnded == checkGameEnded && checkWinningPlayer.Equals(expectedWinningPlayer);

        return (expectedValuesMatch, FAIL_MESSAGE_BASE + GetInnerStateString() + "." + ConstructFailMessageAppend(checkGameEnded, checkWinningPlayer));
    }
}
