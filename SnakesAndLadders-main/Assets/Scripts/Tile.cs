using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour {
    private Color _baseColor = Color.cyan, _offsetColor = Color.gray;
    [SerializeField] private SpriteRenderer _renderer;
    //[SerializeField] public Text _text;
    //[SerializeField] private GameObject _highlight;
    
    public void Init(bool isOffset) {
        _renderer.color = isOffset ? _offsetColor : _baseColor;

        
    }

    //void OnMouseEnter() {
    //    _highlight.SetActive(true);
    //}

    //void OnMouseExit()
    //{
    //    _highlight.SetActive(false);
    //}
}