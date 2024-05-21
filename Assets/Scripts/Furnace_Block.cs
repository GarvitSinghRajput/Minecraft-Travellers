using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace_Block : Block
{
	/// <summary>
	/// Overridding the base method to animate Furnace UVs.
	/// </summary>
	/// <param name="pos"></param>
    public override void AddVoxelDataToChunk(Vector3 pos)
    {
		for (int p = 0; p < 6; p++)
		{
			if (!CheckVoxel(pos + block_data.faceChecks[p]))
			{
				vertices.Add(pos + block_data.voxelVerticesPos[block_data.voxelTris[p, 0]]);
				vertices.Add(pos + block_data.voxelVerticesPos[block_data.voxelTris[p, 1]]);
				vertices.Add(pos + block_data.voxelVerticesPos[block_data.voxelTris[p, 2]]);
				vertices.Add(pos + block_data.voxelVerticesPos[block_data.voxelTris[p, 3]]);
				if (furnace && block_data.blockType.GetTextureID(p) == 12)
				{
					AddTexture(14);
				}
				else
					AddTexture(block_data.blockType.GetTextureID(p));

				triangles.Add(vertexIndex);
				triangles.Add(vertexIndex + 1);
				triangles.Add(vertexIndex + 2);
				triangles.Add(vertexIndex + 2);
				triangles.Add(vertexIndex + 1);
				triangles.Add(vertexIndex + 3);
				vertexIndex += 4;

			}
		}
	}

    public override void AnimateFrunace(float timer)
    {
        base.AnimateFrunace(timer);
    }
}
