using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Linq;

/**
 * Keeps track of the state of the game and whether the players and enemies are still alive.
 */
public class GameState : MonoBehaviour {

    /**
     * The characters that all should stay alive.
     */
    public Character[] players;

    /**
     * The character that all should be killed.
     */
    public Character[] enemies;

    /**
     * The player has won this level.
     */
    public UnityEvent onWin = new UnityEvent();

    /**
     * The player has lost the level.
     */
    public UnityEvent onGameOver = new UnityEvent();

	// Use this for initialization
	void Start () {
        // add calls to CheckGameState() when an event happens to the players/enemies
        foreach (var c in players.Concat(enemies))
        {
            c.onDeath.AddListener(CheckGameState);
        }
	}

    /**
     * This checks whether the game is over.
     */
    private void CheckGameState()
    {
        // check whether all players are alive
        var allPlayersAlive = players.All(player => player.isAlive);

        if (allPlayersAlive)
        {
            onWin.Invoke();
        }
        else
        {
            var allEnemiesDead = enemies.All(enemy => !enemy.isAlive);
            if (allEnemiesDead)
            {
                onGameOver.Invoke();
            }
        }
    }
}
