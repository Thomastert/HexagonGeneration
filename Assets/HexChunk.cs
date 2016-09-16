using UnityEngine;
using System.Collections;

public class HexChunk : MonoBehaviour
{
    [SerializeField]
    public HexInfo[,] hexArray;
    public int xSize;
    public int ySize;
    public Vector3 hexSize;

    public int xSector;
    public int ySector;
    public WorldManager worldManager;

    private MeshFilter filter;
    private new BoxCollider collider;

    public void SetSize(int x, int y)
    {
        xSize = x;
        ySize = y;
    }

    public void OnDestroy()
    {
        Destroy(renderer.material);
    }

    public void AllocateHexArray()
    {
        hexArray = new HexInfo[xSize, ySize];
    }

    public void Begin()
    {
        GenerateChunk();
        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < ySize; z++)
            {
                if (hexArray[x, z] != null)
                {
                    hexArray[x, z].parentChunk = this;
                    hexArray[x, z].Start();
                }
                else
                {
                    Debug.Log("empty");
                }
            }
        }
        Combine();
    }

    public void GenerateChunk()
    {
        bool odd;
        for (int y = 0; y < ySize; y++)
        {
            odd = (y % 2) == 0;
            if(odd == true)
            {
                for(int x = 0; x < xSize; x++)
                {
                    GenerateHex(x, y);
                }
            }
            else
            {
                for (int x = 0; x < xSize; x++)
                {
                    GenerateHexOffset(x, y);
                }
            }
        }
    }

    public void GenerateHex(int x, int y)
    {
        HexInfo hex;
        Vector2 WorldArrayPosition;
        hexArray[x, y] = new HexInfo();
        hex = hexArray[x, y];

        WorldArrayPosition.x = x + (xSize * xSector);
        WorldArrayPosition.y = y + (ySize * ySector);

        hex.CubeGridPostion = new Vector3 (WorldArrayPosition.x - Mathf.Round((WorldArrayPosition.y / 2) + .1f), WorldArrayPosition.y, -(WorldArrayPosition.x - Mathf.Round((worldArrayPosition.y / 2) + .1f) + worldArrayPosition.y));
        hex.localPosition = new Vector3((x * (worldManager.hexExt.x * 2) + worldManager.hexExt.x), 0, (y * worldManager.hexExt.z) * 1.5f);
        hex.worldPosition = new Vector3(hex.localPosition.x + (xSector * (xSize * hexSize.x)), hex.localPosition.y, hex.localPosition.z + ((ySector * (ySize * hexSize.z)) * (.75f)));

        hex.hexExt = worldManager.hexExt;
        hex.hexCenter = worldManager.hexCenter;
    }

}
