using UnityEngine;
using System.Collections;

public class WorldManager : MonoBehaviour
{

    HexInfo hexinfo;
    public Mesh flatHexagonSharedMesh;
    public float hexRadiusSize;
    [HideInInspector]
    public Vector3 hexExt;
    [HideInInspector]
    public Vector3 hexSize;
    [HideInInspector]
    public Vector3 hexCenter;
    [HideInInspector]
    public GameObject chunkHolder;
    public Vector2[] uv;
    public Texture2D terrainTexture;

    int xSectors;
    int zSectors;

    public HexInfo[,] hexChunks;

    public Vector2 mapSize;
    public int chunkSize;

    public void Awake()
    {
        GetHexProperties();
        GenerateMap();
    }

    private void GetHexProperties()
    {
        GameObject inst = new GameObject("Bounds Set Up: Flat");
        inst.AddComponent<MeshFilter>();
        inst.AddComponent<MeshRenderer>();
        inst.AddComponent<MeshCollider>();
        inst.transform.position = Vector3.zero;
        inst.transform.rotation = Quaternion.identity;

        Vector3[] vertices;
        int[] triangles;
        Vector2[] uv;

        float floorLevel = 0;
        vertices = new Vector3[]
        {
            /*0*/new Vector3((hexRadiusSize * Mathf.Cos((float)(2*Mathf.PI*(3+0.5)/6))), floorLevel, (hexRadiusSize * Mathf.Sin((float)(2*Mathf.PI*(3+0.5)/6)))),
            /*1*/new Vector3((hexRadiusSize * Mathf.Cos((float)(2*Mathf.PI*(2+0.5)/6))), floorLevel, (hexRadiusSize * Mathf.Sin((float)(2*Mathf.PI*(2+0.5)/6)))),
            /*2*/new Vector3((hexRadiusSize * Mathf.Cos((float)(2*Mathf.PI*(1+0.5)/6))), floorLevel, (hexRadiusSize * Mathf.Sin((float)(2*Mathf.PI*(1+0.5)/6)))),
            /*3*/new Vector3((hexRadiusSize * Mathf.Cos((float)(2*Mathf.PI*(0+0.5)/6))), floorLevel, (hexRadiusSize * Mathf.Sin((float)(2*Mathf.PI*(0+0.5)/6)))),
            /*4*/new Vector3((hexRadiusSize * Mathf.Cos((float)(2*Mathf.PI*(5+0.5)/6))), floorLevel, (hexRadiusSize * Mathf.Sin((float)(2*Mathf.PI*(5+0.5)/6)))),
            /*5*/new Vector3((hexRadiusSize * Mathf.Cos((float)(2*Mathf.PI*(4+0.5)/6))), floorLevel, (hexRadiusSize * Mathf.Sin((float)(2*Mathf.PI*(4+0.5)/6))))
        };

        triangles = new int[]
        {
            1,5,0,
            1,4,5,
            1,2,4,
            2,3,4
        };
        uv = new Vector2[]
        {
         new Vector2(0,0.25f),
         new Vector2(0,0.75f),
         new Vector2(0.5f,1),
         new Vector2(1,0.75f),
         new Vector2(1,0.25f),
         new Vector2(0.5f,0),
        };
        flatHexagonSharedMesh = new Mesh();
        flatHexagonSharedMesh.vertices = vertices;
        flatHexagonSharedMesh.triangles = triangles;
        flatHexagonSharedMesh.uv = uv;
        inst.GetComponent<MeshFilter>().mesh = flatHexagonSharedMesh;
        inst.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        inst.GetComponent<MeshCollider>().sharedMesh = flatHexagonSharedMesh;

        hexExt = new Vector3(inst.GetComponent<MeshCollider>().bounds.extents.x, inst.GetComponent<MeshCollider>().bounds.extents.y, inst.GetComponent<MeshCollider>().bounds.extents.z);
        hexSize = new Vector3(inst.GetComponent<MeshCollider>().bounds.size.x, inst.GetComponent<MeshCollider>().bounds.size.y, inst.GetComponent<MeshCollider>().bounds.size.z);
        hexCenter = new Vector3(inst.GetComponent<MeshCollider>().bounds.center.x, inst.GetComponent<MeshCollider>().bounds.center.y, inst.GetComponent<MeshCollider>().bounds.center.z);
        Destroy(inst);
    }

    private void GenerateMap()
    {
        xSectors = Mathf.CeilToInt(mapSize.x / chunkSize);
        xSectors = Mathf.CeilToInt(mapSize.y / chunkSize);
        hexChunks = new HexInfo[xSectors, zSectors];
        for (int x = 0; x < xSectors; x++)
        {
            for(int z = 0; z<zSectors; z++)
            {
                hexChunks[x, z] = NewChunk(x, z);
                hexChunks[x, z].gameObject.transform.position = new Vector3(x * (chunkSize * hexSize.x), 0f, (z * (chunkSize * hexSize.z) * (.75f)));
                hexChunks[x, z].hexSize = hexSize;
                hexChunks[x, z].SetSize(chunkSize, chunkSize);
                hexChunks[x, z].XSector = x;
                hexChunks[x, z].ySector = y;
                hexChunks[x, z].worldManager = this;
            }
        }
        foreach (HexChunk chunk in hexChunks)
        {
            chunk.Begin();
        }
    }

    public HexChunk NewChunk(int x, int y)
    {
        if (x == 0 && y == 0)
        {
            chunkHolder = new GameObject("ChunkHolder");
        }
        GameObject chunkObj = new GameObject("Chunk[" + x + "," + y + "]");
        chunkObj.AddComponent<HexChunk>();
        chunkObj.GetComponent<HexChunk>().AllocateHexArray();
        chunkObj.AddComponent<MeshRenderer>().material.mainTexture = terrainTexture;
        chunkObj.AddComponent<MeshFilter>();
        chunkObj.transform.parent = chunkHolder.transform;

        return chunkObj.GetComponent<HexChunk>();
    }

    


}
