using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class MatchInfo : MonoBehaviour
{
    private int draws = 0;
    private Dictionary<Player, int> winCounterByPlayer = new Dictionary<Player, int>();

    private Player? lastWinningPlayer;

    public Player? LastWinningPlayer => lastWinningPlayer;

    public void SetLastWinningPlayer(Player? lastWinningPlayer)
    {
        this.lastWinningPlayer = lastWinningPlayer;
    }
    
    public bool MatchHasEnded()
    {
        return GetTotalMatchesPlayed() >= GameSetupValues.INSTANCE.TotalRoundsPlayed;
    }

    public Player? GetMatchWinningPlayer()
    {


        List<Player> bestPlayers = winCounterByPlayer.Keys.ToList().OrderByDescending((player) => GetWinsByPlayer(player)).ToList();
        
        Player best = bestPlayers[0];
        Player secondBest = bestPlayers[1];

        // Two best players == draw
        if(GetWinsByPlayer(best) == GetWinsByPlayer(secondBest))
        {
            return null;
        }

        return best;

    }

    public int GetTotalMatchesPlayed()
    {
        return draws + GetMyWins() + GetOtherWins();
    }

    public int GetWinsByPlayer(Player p)
    {
        return winCounterByPlayer[p];
    }

    public int GetMyWins()
    {
        return GetWinsByPlayer(MyPlayer);
    }

    public int GetOtherWins()
    {
        int totalWins = 0;
        foreach(Player player in winCounterByPlayer.Keys)
        {
            if(PlayerIsMyPlayer(player))
            {
                continue;
            }

            int playerWins = GetWinsByPlayer(player);
            totalWins += playerWins;
        }

        return totalWins;
    }

    public void InitWinDictionary()
    {
        foreach(Player player in EnumUtil.IterateValues<Player>())
        {
            winCounterByPlayer.Add(player, 0);
        }
    }

    public void IncreaseWinCountFor(Player? player)
    {
        if(!player.HasValue)
        {
            ++draws;
            return;
        }

        int currentValue = winCounterByPlayer[player.Value];
        winCounterByPlayer[player.Value] = currentValue + 1;
    }

    /// <summary>
    /// The player the client is playing as.
    /// </summary>
    public Player MyPlayer
    {
        get;
        set;
    } = Player.CIRCLE;

    public bool PlayerIsMyPlayer(Player otherPlayer)
    {
        return MyPlayer == otherPlayer;
    }

    public static MatchInfo INSTANCE
    {
        get;
        private set;
    }

    private void OnDestroy()
    {
        INSTANCE = null;
    }

    private void Awake()
    {
        InitWinDictionary();
        InitSingleton();
    }

    private void InitSingleton()
    {
        if (INSTANCE != null)
        {
            Debug.LogError($"Tried to create a second instance of {nameof(MatchInfo)}.");
            return;
        }

        INSTANCE = this;
    }

}
