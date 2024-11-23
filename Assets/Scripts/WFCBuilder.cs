using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFCBuilder : MonoBehaviour
{
    [SerializeField] private int Width;
    [SerializeField] private int Height;

    private WFCNode[,] _grid;

    public List<WFCNode> Nodes = new List<WFCNode>();
    public List<Vector2Int> _toCollapse = new List<Vector2Int>();

    private Vector2Int[] offsets = new Vector2Int[]
    {
        new Vector2Int(0, 1),   // Top
        new Vector2Int(0, -1),  // Bottom
        new Vector2Int(1, 0),   // Right
        new Vector2Int(-1, 0)   // Left
    };

    private void Start()
    {
        _grid = new WFCNode[Width, Height];
        for (int x = 0; x < Width; x++)
        CollapseWorld();
    }

    private void CollapseWorld() {
        _toCollapse.Clear();
        _toCollapse.Add(new Vector2Int(Width / 2, Height / 2));
        while (_toCollapse.Count > 0) {
            int x = _toCollapse[0].x;
            int y = _toCollapse[0].y;

            List<WFCNode> potentialNodes = new List<WFCNode>(Nodes);

            for (int i = 0; i < offsets.Length; i++) {
                Vector2Int neighbor = new Vector2Int(x + offsets[i].x, y + offsets[i].y);
                if (IsInsideGrid(neighbor) && _grid[neighbor.x, neighbor.y] != null) {
                    WFCNode neighborNode = _grid[neighbor.x, neighbor.y];
                    if (neighborNode != null) {
                        switch (i) {
                            case 0:
                                WhittleNodes(potentialNodes, neighborNode.Bottom.CompatibleNodes);
                                break;
                            case 1:
                                WhittleNodes(potentialNodes, neighborNode.Top.CompatibleNodes);
                                break;
                            case 2:
                                WhittleNodes(potentialNodes, neighborNode.Left.CompatibleNodes);
                                break;
                            case 3:
                                WhittleNodes(potentialNodes, neighborNode.Right.CompatibleNodes);
                                break;
                        }
                    } else {
                        if (!_toCollapse.Contains(neighbor)) {
                            _toCollapse.Add(neighbor);
                        }
                    }
                }
            }
            if (potentialNodes.Count < 1) {
                _grid[x, y] = Nodes[0];
                Debug.LogWarning("Attempted wave collapse at " + x + ", " + y + ", but none were valid. Defaulting to first node.");
            } else {
                _grid[x, y] = potentialNodes[Random.Range(0, potentialNodes.Count)];
            }

            GameObject newNode = Instantiate(_grid[x, y].Prefab, new Vector3(x, y, 0f), Quaternion.identity);
            _toCollapse.RemoveAt(0);
        }
    }

    private void WhittleNodes(List<WFCNode> potentialNodes, List<WFCNode> validNodes) {
        for (int i = potentialNodes.Count - 1; i > -1; i--) {
            if (!validNodes.Contains(potentialNodes[i])) {
                potentialNodes.RemoveAt(i);
            }
        }
    }
    private bool IsInsideGrid(Vector2Int pos) {
        return pos.x >= 0 && pos.x < Width && pos.y >= 0 && pos.y < Height;
    }
}
