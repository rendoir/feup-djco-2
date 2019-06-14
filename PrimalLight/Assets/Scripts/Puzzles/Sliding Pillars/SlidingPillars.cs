using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingPillars : MonoBehaviour
{
	public GameObject pad;
	public GameObject padTarget;
	public GameObject path;
	public GameObject keyPillar;
	public GameObject regularPillar;
	
	public int posOffset;
	public Vector3 position;

	public GameObject target;
	
	private BoardTile[,] board;
	private BoardPillar[] pillars;
	
	private bool solved = false;

	public void SetBoard(BoardTile[,] board){
		this.board = board;
	}

	public void SetPillars(BoardPillar[] pillars){
		this.pillars = pillars;
	}

	private void GenerateBoard(){
		if(board == null)
			return;

		float x = position.x, y = position.y, z = position.z;
		for(int i = 0; i < board.GetLength(0); i++){

			for(int j = 0; j < board.GetLength(1); j++){
				BoardTile tile = board[i,j];

				z = position.z+j*posOffset;

				if(tile.type == BoardTileType.Target)
					Instantiate(padTarget, new Vector3(x,y,z), Quaternion.identity);
				else if(tile.type == BoardTileType.Path)
					Instantiate(pad, new Vector3(x,y,z), Quaternion.identity);
				else
					continue;

				// Check right path
				if(i < board.GetLength(0)-1){
					if(tile.rightPath)
						Instantiate(path, new Vector3(x+3,y,z),Quaternion.identity);
				}

				// Check upper path
				if(j < board.GetLength(1)-1){
					if(tile.upPath)
						Instantiate(path, new Vector3(x,y,z+3), Quaternion.Euler(0,90,0));
				}
			}
			x += posOffset;
		}
	}

	private void GeneratePillars(){
		if(pillars == null)
			return;

		int i = 0;
		foreach(BoardPillar pillar in pillars){
			Vector3 pos = position + new Vector3(pillar.pos.x*posOffset, 0, pillar.pos.y*posOffset);
			
			GameObject pillarObj = null;
			if(pillar.type == BoardPillarType.Regular)
				pillarObj = (GameObject) Instantiate(regularPillar, pos, Quaternion.identity);
			else if(pillar.type == BoardPillarType.Key)
				pillarObj = (GameObject)Instantiate(keyPillar, pos, Quaternion.identity);
			
			Pillar pillarScript = pillarObj.GetComponent<Pillar>();
			pillarScript.Init(this, i, pillar.pos);
			i++;
		}
	}

	public void Generate(){
		GenerateBoard();
		GeneratePillars();
	}

	private bool canMove(string dir, Vector2 src, Vector2 dest){
		// Check OOB
		if(	(int) dest.x >= board.GetLength(0) || 
			(int) dest.y >= board.GetLength(1) || 
			(int) dest.x < 0 || (int) dest.y < 0)
			return false;

		// Check board layout
		BoardTile srcTile = board[(int) src.x, (int) src.y];
		BoardTile destTile = board[(int) dest.x, (int) dest.y];
		if(destTile.type == BoardTileType.Empty)
			return false;

		//Check if connected
		if(dir == "up"){
			if(!srcTile.upPath)
				return false;
		}
		else if(dir == "down"){
			if(!destTile.upPath)
				return false;
		}
		else if(dir == "left"){
			if(!destTile.rightPath)
				return false;
		}
		else if(dir == "right"){
			if(!srcTile.rightPath){
				return false;;
			}
		}

		//Check other pillars
		foreach(BoardPillar pillar in pillars){
			if(pillar.pos == dest)
				return false;
		}

		return true;
	}

	private bool CheckSolved(){
		foreach(BoardPillar pillar in pillars){
			if(pillar.type == BoardPillarType.Key){
				if(board[(int) pillar.pos.x, (int) pillar.pos.y].type != BoardTileType.Target)
					return true;
			}
		}

		solved = true;
		OnSolved();

		return true;
	}

	private void OnSolved(){
		target.GetComponent<TempleDualStatueDoor>().Action();
	}

	public bool Move(int pillarIdx, string dir, out Vector3 endPos, out bool target){
		if(solved){
			endPos = Vector3.zero;
			target = false;
			return false;
		}

		bool canMove = false;

		Vector2 offset = new Vector2(0,0);
		if(dir == "up")
			offset = Vector2.up;
		else if(dir == "down")	
			offset = Vector2.down;	
		else if(dir == "left")
			offset = Vector2.left;
		else if(dir == "right")
			offset = Vector2.right;
		
		Vector2 src = pillars[pillarIdx].pos;
		Vector2 dest = src + offset;
		while(this.canMove(dir,src,dest)){
			src = dest;
			dest += offset;
			canMove = true;
		}

		// Update position
		pillars[pillarIdx].pos = src;

		// Check if puzzle is solved
		CheckSolved();

		//Check if dest pos is target
		if(board[(int) src.x, (int) src.y].type == BoardTileType.Target)
			target = true;
		else
			target = false;

		//	Get "real" scene pos
		endPos = position + new Vector3(src.x*posOffset, 0, src.y*posOffset);
		return canMove;
	}
}
