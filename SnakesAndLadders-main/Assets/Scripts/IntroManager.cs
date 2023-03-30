using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public int _no_of_snakes;
    public int _no_of_ladders;
    private int _height;
    private int _width;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void width(string text)
    {
        if (int.TryParse(text, out int result))
        {
            _width = result;
            if (_width > 0)
            {
                GameValues.width = _width;
            }
        }
    }
    public void height(string text)
    {
        if (int.TryParse(text, out int result))
        {
            _height = result;
            if (_height > 0)
            {
                GameValues.height = _height;
            }
        }
    }

    public void ladders(string text)
    {
        if (int.TryParse(text, out int result))
        {
            _no_of_ladders = result;
            GameValues.no_of_ladders = _no_of_ladders;
        }
    }

    public void snakes(string text)
    {
        if (int.TryParse(text, out int result))
        {
            _no_of_snakes = result;
            GameValues.no_of_snakes = _no_of_snakes;
        }
    }

    public void submitData()
    {
        SceneManager.LoadScene(sceneName: "Gameplay");
    }
}

public class GameValues
{
    public static int width = 5;
    public static int height = 5;
    public static int no_of_ladders = 2;
    public static int no_of_snakes = 2;

    // You can add other static variables here.

}
