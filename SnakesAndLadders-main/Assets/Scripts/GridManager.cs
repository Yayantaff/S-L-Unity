using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour {
    private int _width, _height;

    [SerializeField] private Tile _tilePrefab;

    [SerializeField] private Transform _cam;

    [SerializeField] public GameManager gameMan;

    
    private Dictionary<Vector2, Tile> _tiles;

    public Dictionary<int, Vector3> positions_static;

    private float anchorX, anchorY;

    private float tileWidth, tileHeight;

    [SerializeField] private Canvas can;

    void Start() {
        _width = GameValues.width;
        _height = GameValues.height;
        GenerateGrid();
    }

    void GenerateGrid() {
        _tiles = new Dictionary<Vector2, Tile>();
        positions_static = new Dictionary<int, Vector3>();

        tileWidth = _tilePrefab.GetComponent<Renderer>().bounds.size.x;
        tileHeight = _tilePrefab.GetComponent<Renderer>().bounds.size.y;

        anchorX = - (_width * tileWidth / 2) + tileWidth / 2;
        anchorY = - (_height * tileHeight / 2) + tileHeight / 2;

        for (int y = 0; y < _height; y++) {
            for (int x = 0; x < _width; x++) {
                float t = Mathf.Pow(-1, y);
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(t * (anchorX + (x * tileWidth)), anchorY + y * tileHeight), Quaternion.identity);
                int r1 = x + y * _width + 1;
                var isOffset = ((y* _width+x) % 2 == 0);// && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);


                _tiles[new Vector2(x, y)] = spawnedTile;

                positions_static[(y * _width) + x] = new Vector3(t * (anchorX + x * tileWidth), anchorY + y * tileHeight);
                
                Text textPrefabText;
                textPrefabText = spawnedTile.GetComponent<Text>();
                textPrefabText.transform.SetParent(can.transform, true);
                textPrefabText.text = $"{r1}";
                
             }
        }
        gameMan.SetUpPositions();
        // _cam.transform.position = new Vector3((float)_width/2 -0.5f, (float)_height / 2 - 0.5f,-10);
    }

    public Tile GetTileAtPosition(Vector2 pos) {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }
}