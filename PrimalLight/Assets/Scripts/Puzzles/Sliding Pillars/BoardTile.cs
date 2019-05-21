using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoardTileType { Empty, Target, Path }

public class BoardTile
{
	public bool upPath;
	public bool rightPath;
	public BoardTileType type;

	public BoardTile(BoardTileType type, bool upPath, bool rightPath){
		this.upPath = upPath;
		this.rightPath = rightPath;
		this.type = type;
	}

	public static BoardTile Empty(){
		return new BoardTile(BoardTileType.Empty,false,false);
	}

	public static BoardTile Path(bool upPath, bool rightPath){
		return new BoardTile(BoardTileType.Path,upPath,rightPath);
	}

	public static BoardTile Target(bool upPath, bool rightPath){
		return new BoardTile(BoardTileType.Target,upPath,rightPath);
	}
}
