using UnityEngine;
using System.Collections;

public class MapIndexing  {
	//the center of the map is (0,0)
	public static Vector2 getPositionFromIndex(int x, int y, float block_size, int total_block_count){
		float origin_x = -(block_size * total_block_count) / 2 + block_size / 2;
		float origin_y = origin_x;

		float position_x = origin_x + x * block_size;
		float position_y = origin_y + y * block_size;

		return(new Vector2 (position_x, position_y));
	}

	//the center of the map is (0,0)
	public static Vector2 getIndexFromPosition(float position_x, float position_y, float block_size, int total_block_count){
		float origin_x = -(block_size * total_block_count) / 2 + block_size / 2;
		float origin_y = origin_x;
		int x = Mathf.RoundToInt((position_x - origin_x) / block_size);
		int y = Mathf.RoundToInt((position_y - origin_y) / block_size);

		return (new Vector2 (x, y));
	}

}
