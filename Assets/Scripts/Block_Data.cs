 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Data : MonoBehaviour
{
    public bool debug;
    public Transform[] vertices;
    public Triangle3[] triangles;
    [SerializeField]
    private Vector3 size = Vector3.one, position;
    public Material material;
    public BlockType blockType;
    
    [HideInInspector]
    public Vector3[] voxelVerticesPos;
    [HideInInspector]
    public int TextureAtlasSizeInBlocks = 4;
    public static int ChunkWidth = 1;
    public static int ChunkHeight = 1;

    private void Awake()
    {
        voxelVerticesPos = new Vector3[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            voxelVerticesPos[i] = vertices[i].localPosition;
        }
    }

    public float NormalizedBlockTextureSize
    {

        get { return 1f / (float)TextureAtlasSizeInBlocks; }

    }

    public readonly Vector3[] faceChecks = new Vector3[6] {

        new Vector3(0.0f, 0.0f, -1.0f),
        new Vector3(0.0f, 0.0f, 1.0f),
        new Vector3(0.0f, 1.0f, 0.0f),
        new Vector3(0.0f, -1.0f, 0.0f),
        new Vector3(-1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f)

    };

    public readonly int[,] voxelTris = new int[6, 4] {

        // Back, Front, Top, Bottom, Left, Right

		// 0 1 2 2 1 3
		{0, 3, 1, 2}, // Back Face
		{5, 6, 4, 7}, // Front Face
		{3, 7, 2, 6}, // Top Face
		{1, 5, 0, 4}, // Bottom Face
		{4, 7, 0, 3}, // Left Face
		{1, 2, 5, 6} // Right Face

	};

    public readonly Vector2[] voxelUvs = new Vector2[4] {

        new Vector2 (0.0f, 0.0f),
        new Vector2 (0.0f, 1.0f),
        new Vector2 (1.0f, 0.0f),
        new Vector2 (1.0f, 1.0f)

    };



    //GUI Region Starts here
    #region GUI Functions
    private void OnDrawGizmos()
    {
        Vector3 target = new Vector3(transform.position.x + position.x, transform.position.y + position.y, transform.position.z + position.z);
        Gizmos.DrawWireCube(target, size);

        if (debug)
        {
            for (int i = 0; i < 8; i++)
            {
                drawTriangleGismoz(triangles[i].vertices[0].position, Color.cyan, triangles[i].vertices[1].position, Color.magenta, triangles[i].vertices[2].position, Color.yellow);
            }

            for (int i = 8; i < 12; i++)
            {
                drawTriangleGismoz(triangles[i].vertices[0].position, Color.cyan, triangles[i].vertices[1].position, Color.cyan, triangles[i].vertices[2].position, Color.yellow, true);
            }
        }
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, 1f);
    //}

    private void drawTriangleGismoz(Vector3 first, Color first_color, Vector3 second, Color second_color, Vector3 third, Color third_color, bool outlined = false)
    {
                if (outlined)
        {
            Gizmos.color = first_color;
            Gizmos.DrawLine(new Vector3(first.x, first.y, first.z), new Vector3(second.x, second.y, second.z));

            Gizmos.color = second_color;
            Gizmos.DrawLine(new Vector3(second.x, second.y, second.z), new Vector3(third.x, third.y, third.z));

            Gizmos.color = third_color;
            Gizmos.DrawLine(new Vector3(third.x, third.y, third.z), new Vector3(first.x, first.y, first.z));
        }
        else
        {
            Gizmos.color = first_color;
            Gizmos.DrawLine(new Vector3(first.x, first.y, first.z), new Vector3(second.x, second.y, second.z));

            Gizmos.color = second_color;
            Gizmos.DrawLine(new Vector3(second.x, second.y, second.z), new Vector3(third.x, third.y, third.z));

            Gizmos.color = third_color;
            Gizmos.DrawLine(new Vector3(third.x, third.y, third.z), new Vector3(first.x, first.y, first.z));
        }
    }
    #endregion
}

[System.Serializable]
public class Triangle3
{
    public Transform[] vertices = new Transform[3];
}

[System.Serializable]
public class BlockType
{

    public string blockName;

    [Header("Texture Values")]
    public int backFaceTexture;
    public int frontFaceTexture;
    public int topFaceTexture;
    public int bottomFaceTexture;
    public int leftFaceTexture;
    public int rightFaceTexture;

    // Back, Front, Top, Bottom, Left, Right

    public int GetTextureID(int faceIndex)
    {
        switch (faceIndex)
        {
            case 0:
                return backFaceTexture;
            case 1:
                return frontFaceTexture;
            case 2:
                return topFaceTexture;
            case 3:
                return bottomFaceTexture;
            case 4:
                return leftFaceTexture;
            case 5:
                return rightFaceTexture;
            default:
                Debug.Log("Error in GetTextureID; invalid face index");
                return 0;
        }
    }
}