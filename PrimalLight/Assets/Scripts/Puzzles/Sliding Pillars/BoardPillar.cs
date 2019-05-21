using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoardPillarType { Regular, Key }

public class BoardPillar
{
    public Vector2 pos;
    public BoardPillarType type;

    public BoardPillar(BoardPillarType type, Vector2 pos){
    	this.pos = pos;
    	this.type = type;
    }

    public static BoardPillar Regular(float x, float y){
		return new BoardPillar(BoardPillarType.Regular,new Vector2(x,y));
	}

	public static BoardPillar Key(float x, float y){
		return new BoardPillar(BoardPillarType.Key,new Vector2(x,y));
	}
}