using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarFogMaster : MonoBehaviour {
	public Transform fogPiece; //one piece of a war fog
	public float blockSize;
	public int MapSize; // how many blocks
	public Transform[,] theFogs;
	public Transform thePlayer;
	private Vector2 Origin;

		
	public void generateWarFog(bool[,] fogs){
		theFogs = new Transform[MapSize, MapSize];
		for (int x = 0; x < fogs.GetLength(0); x ++) {
			for(int y = 0 ; y < fogs.GetLength(1) ; y ++){
				//x , y are the index 
				if(fogs[x,y]){
					//need a fog, need create at x , y
					float x_position = Origin.x + blockSize*x;
					float y_position = Origin.y + blockSize*y;
					Vector3 new_pos = new Vector3(x_position, y_position ,0);
					theFogs[x,y] = Instantiate(fogPiece,new_pos,Quaternion.identity) as Transform;
					theFogs[x,y].SetParent(transform);

				}
			}
		}
	
	}


	//reveal war fog with player center and radius
	public void UpdateFogWithPlayer(Vector2 player_pos, float radius){
		RevealFogs (ComputeSight (player_pos, radius));
	}


	private void RevealFogs(List<Vector2> OpenedFogList){
		for (int i = 0; i < OpenedFogList.Count; i ++) {
			int x = (int)OpenedFogList[i].x;
			int y = (int)OpenedFogList[i].y;
			if(theFogs[x,y] != null){
				Destroy(theFogs[x,y].gameObject);
				//also need to update fogs with the model
				Game.Current.Hero.CurrentEquippedMap.WarFogIndexs[x,y] = false;
			}
		}
	}
	
	private List<Vector2> ComputeSight(Vector2 player_pos, float radius){
		List<Vector2> Sight = new List<Vector2> ();
		for (int x = 0; x < MapSize; x ++) {
			for(int y = 0 ; y < MapSize; y++){
				Vector2 pos = new Vector2(Origin.x + blockSize*x, Origin.y + blockSize*y);
				if(Vector2.Distance(pos, player_pos) <= radius){
					Sight.Add(new Vector2(x,y)); // add this point to the visible sight
				}
			}
		}
		return Sight;
	}

	// Use this for initialization
	void Start () {
		Origin = new Vector2 (-(blockSize * MapSize) / 2 + blockSize / 2, -(blockSize * MapSize) / 2 + blockSize / 2);
	}


}
