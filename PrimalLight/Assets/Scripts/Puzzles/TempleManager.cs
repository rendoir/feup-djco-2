using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleManager : MonoBehaviour
{

	private SlidingPillars spPuzzle1;
    public GameObject elevator;
    public GameObject secretDoor;
    public float setElevatorTime = 5f;
    
    public static TempleManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);    
    }

    void Start()
    {
    	BoardTile[,] spPuzzle1Board = {
            {BoardTile.Path(false,true),    BoardTile.Empty(),              BoardTile.Path(false,true)},
            {BoardTile.Target(true,true),   BoardTile.Path(true,false),     BoardTile.Path(false,true)},
            {BoardTile.Path(false,true),    BoardTile.Empty(),              BoardTile.Target(false,true)},
            {BoardTile.Target(true,true),   BoardTile.Path(true,false),     BoardTile.Path(false,true)},
            {BoardTile.Path(false,false),   BoardTile.Empty(),             BoardTile.Path(false,false)},
        };

        BoardPillar[] spPuzzle1Pillars= {
            BoardPillar.Key(0,0),
            BoardPillar.Key(0,2),
            BoardPillar.Key(4,0),
            BoardPillar.Regular(4,2)
        };

        BoardTile[,] spPuzzle2Board = {
            {BoardTile.Empty(),             BoardTile.Path(false,true),     BoardTile.Empty()},
            {BoardTile.Path(true,true),     BoardTile.Path(true,true),      BoardTile.Path(false,true)},
            {BoardTile.Target(false,true),  BoardTile.Path(false,true),     BoardTile.Path(false,true)},
            {BoardTile.Path(false,true),    BoardTile.Path(false,true),     BoardTile.Path(false,true)},
            {BoardTile.Path(true,false),    BoardTile.Path(true,true),      BoardTile.Path(true,false)},
            {BoardTile.Empty(),             BoardTile.Path(false,false),    BoardTile.Empty()},
        };

    	spPuzzle1 = gameObject.GetComponent<SlidingPillars>();
    	spPuzzle1.SetBoard(spPuzzle1Board);
        spPuzzle1.SetPillars(spPuzzle1Pillars);
        spPuzzle1.Generate();
    }

    public void OnRewardCollected(){
        secretDoor.GetComponent<TempleStatueDoor>().Action();
        StartCoroutine(SetElevator());
    }

    IEnumerator SetElevator(){
        yield return new WaitForSeconds(setElevatorTime);
        elevator.GetComponent<TempleElevator>().Set();
    }
}
