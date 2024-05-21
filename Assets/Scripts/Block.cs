using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
	public MeshRenderer meshRenderer;
	public MeshFilter meshFilter;
	public Block_Data block_data;
	public bool isFurnace = false;

	internal int vertexIndex = 0;
	internal List<Vector3> vertices = new List<Vector3>();
	internal List<int> triangles = new List<int>();
	internal List<Vector2> uvs = new List<Vector2>();
	internal bool furnace;
	internal byte[,,] voxelMap = new byte[Block_Data.ChunkWidth, Block_Data.ChunkHeight, Block_Data.ChunkWidth];

	List<int> faceToRemove = new();
	/// <summary>
	/// Initialises the block.
	/// </summary>
	public void Init()
	{
		PopulateVoxelMap();
		CreateMeshData();
		CreateMesh();
		furnace = isFurnace;
	}

	void PopulateVoxelMap()
	{
		for (int y = 0; y < Block_Data.ChunkHeight; y++)
		{
			for (int x = 0; x < Block_Data.ChunkWidth; x++)
			{
				for (int z = 0; z < Block_Data.ChunkWidth; z++)
				{

					if (y < 1)
						voxelMap[x, y, z] = 0;
					else if (y == Block_Data.ChunkHeight - 1)
						voxelMap[x, y, z] = 2;
					else
						voxelMap[x, y, z] = 1;

				}
			}
		}
	}

	void CreateMeshData()
	{
		for (int y = 0; y < Block_Data.ChunkHeight; y++)
		{
			for (int x = 0; x < Block_Data.ChunkWidth; x++)
			{
				for (int z = 0; z < Block_Data.ChunkWidth; z++)
				{
					AddVoxelDataToChunk(new Vector3(x, y, z));
				}
			}
		}
	}

	internal bool CheckVoxel(Vector3 pos)
	{
		int x = Mathf.FloorToInt(pos.x);
		int y = Mathf.FloorToInt(pos.y);
		int z = Mathf.FloorToInt(pos.z);

		if (x < 0 || x > Block_Data.ChunkWidth - 1 || y < 0 || y > Block_Data.ChunkHeight - 1 || z < 0 || z > Block_Data.ChunkWidth - 1)
			return false;
		else
			return true;
	}

	/// <summary>
	/// Adds Voxel data to the block.
	/// </summary>
	/// <param name="pos"></param>
	public virtual void AddVoxelDataToChunk(Vector3 pos)
	{
		FindBlocksInRange();
		for (int p = 0; p < 6; p++)
		{
			if (faceToRemove.Contains(p))
				continue;
			if (!CheckVoxel(pos + block_data.faceChecks[p]))
			{
				vertices.Add(pos + block_data.voxelVerticesPos[block_data.voxelTris[p, 0]]);
				vertices.Add(pos + block_data.voxelVerticesPos[block_data.voxelTris[p, 1]]);
				vertices.Add(pos + block_data.voxelVerticesPos[block_data.voxelTris[p, 2]]);
				vertices.Add(pos + block_data.voxelVerticesPos[block_data.voxelTris[p, 3]]);

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

	/// <summary>
	/// Checks if any block(X) is adjacent to this block and stores the value of the 
	/// face which will not render due to block(X). 
	/// </summary>
	private void FindBlocksInRange()
    {
		GameObject[] arr = World.blocks;
		foreach (GameObject go in arr)
		{
			if (go == this)
				continue;
			Vector3 delta = transform.localPosition - go.transform.localPosition;
			delta.x = Mathf.Round(delta.x * 100f) / 100f;
			delta.y = Mathf.Round(delta.y * 100f) / 100f;
			delta.z = Mathf.Round(delta.z * 100f) / 100f;
			//x,y,z mein se ek 1 ya -1 hoga toh uss side ka face check karna hai
			if ((delta.x == 1 || delta.x == 0.99) && delta.y == 0 && delta.z == 0)
				faceToRemove.Add(1);
			if ((delta.x == -1 || delta.x == -0.99) && delta.y == 0 && delta.z == 0)
				faceToRemove.Add(0);
			if (delta.x == 0 && (delta.y == 1 || delta.y == 0.99) && delta.z == 0)
				faceToRemove.Add(3);
			if (delta.x == 0 && (delta.y == -1 || delta.y == -0.99) && delta.z == 0)
				faceToRemove.Add(2);
			if (delta.x == 0 && delta.y == 0 && (delta.z == 1 || delta.z == 0.99))
				faceToRemove.Add(4);
			if (delta.x == 0 && delta.y == 0 && (delta.z == -1 || delta.z == -0.99))
				faceToRemove.Add(5);
			//Debug.LogError(gameObject.name + " , " + go.name + " , " + delta.x + " , " + delta.y + " , " + delta.z);
		}
	}

    void CreateMesh()
	{
		Mesh mesh = new Mesh();
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.uv = uvs.ToArray();

		mesh.RecalculateNormals();

		meshFilter.mesh.Clear();
		meshFilter.mesh = mesh;
		meshFilter.mesh.name = gameObject.name;
	}

	internal void AddTexture(int textureID)
	{
		float y = textureID / block_data.TextureAtlasSizeInBlocks;
		float x = textureID - (y * block_data.TextureAtlasSizeInBlocks);

		x *= block_data.NormalizedBlockTextureSize;
		y *= block_data.NormalizedBlockTextureSize;

		y = 1f - y - block_data.NormalizedBlockTextureSize;

		uvs.Add(new Vector2(x, y));
		uvs.Add(new Vector2(x, y + block_data.NormalizedBlockTextureSize));
		uvs.Add(new Vector2(x + block_data.NormalizedBlockTextureSize, y));
		uvs.Add(new Vector2(x + block_data.NormalizedBlockTextureSize, y + block_data.NormalizedBlockTextureSize));
	}


	//Furnace Animation Functions
	public virtual void AnimateFrunace(float timer) 
	{
		if(isFurnace)
        {
			StartCoroutine(IAnimate(timer));
		}
	}

	IEnumerator IAnimate(float timer)
    {
		CreateMeshData();
		CreateMesh();
		furnace = !furnace;
		yield return new WaitForSeconds(timer);
		CreateMeshData();
		CreateMesh();
		furnace = !furnace;
	}    
}