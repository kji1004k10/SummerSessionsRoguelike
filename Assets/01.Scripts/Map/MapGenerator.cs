using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public static readonly int MapSize = 30, StageCount = 50, BossCount = 3;
    public static readonly Vector2Int[] Directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    public static MapGenerator Instance;

    [SerializeField] Tilemap _targetTilemap;
    [SerializeField] StageShape[] _shapes;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Debug.LogWarning("�� �����Ⱑ 2�� �̻� �ֽ��ϴ�!");
            Destroy(gameObject);
        }
    }

    public void Generate()
    {
        _targetTilemap.ClearAllTiles();

        
    }

    public void TryGenerateTransition()
    {

    }
}

public struct StageShape
{
    public Tilemap ShapeMap;
    public Transform KeyPositions;
    public bool IsTopOpened, IsBottomOpened, IsLeftOpened, IsRightOpened;
    public StageType Type;
}

public enum StageType
{
    Monster, Shop, Spawn, HealArea, Boss
} 