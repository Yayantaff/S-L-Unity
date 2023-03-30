using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool hasGameFinished, canClick;

    public static GameManager instance;

    int roll;

    [SerializeField]
    GameObject gamePiece;


    [SerializeField]
    Vector3 startPos;

    [SerializeField]
    private GridManager gridMan;

    public Board myboard;
    List<Player> players;
    int currentPlayer;

    public Vector3[] positions;

    Dictionary<int, int> joints;

    Dictionary<Player, GameObject> pieces;

    private List<int> _list_positions = new List<int>();

    [SerializeField] private Canvas can;


    public delegate void UpdateMessage(Player player);
    public event UpdateMessage message;

    int totalSquares;

    
    public void GameRestart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void GameQuit()
    {
//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;
//#endif
//        Application.Quit();
        SceneManager.LoadScene(sceneName: "Intro");
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        canClick = true;
        hasGameFinished = false;
        currentPlayer = 0;

    }

    private void InitBoard() { 

        SetUpLadders();

        myboard = new Board(joints);
        players = new List<Player>();
        pieces = new Dictionary<Player, GameObject>();

        for(int i = 0; i < 4; i++)
        {
            players.Add((Player)i);
            GameObject temp = Instantiate(gamePiece);
            pieces[(Player)i] = temp;
            temp.transform.position = new Vector3(startPos.x + Random.Range(0f, 1f), startPos.y + Random.Range(0f, 1f));
            temp.GetComponent<Piece>().SetColors((Player)i);
        }
    }

    public void SetUpPositions()
    {
        positions = new Vector3[100];
        positions[0] = startPos;
        totalSquares = GameValues.height * GameValues.width;
        print("Total Squares " + totalSquares);
        for(int i = 0; i < totalSquares; ++i)
        {
            _list_positions.Add(i);
            positions[i] = gridMan.positions_static[i];
        }

        InitBoard();
    }

    void SetUpLadders()
    {
        int count = 0;
        int ladders = GameValues.no_of_ladders;
        int snakes = GameValues.no_of_snakes;
        int coeff = 1;
        int total_wayblocks = ladders + snakes;
        joints = new Dictionary<int, int>();
        for (int i = 0; i < total_wayblocks; ++i)
        {
            int r1 = Random.Range(0, _list_positions.Count-1);
            int a1 = _list_positions[r1];
            _list_positions.RemoveAt(r1);

            
            int r2 = Random.Range(0, _list_positions.Count-1);
            int a2 = _list_positions[r2];
            
            if ((Mathf.Abs(a1 - a2) < 10) || (a1 == 0) || (a2 == 0) || (a1 == totalSquares - 1) || (a2 == totalSquares - 1))
            {
                i--;
                _list_positions.Add(a1);
                continue;
            }

            _list_positions.RemoveAt(r2);

            if (count >= ladders) coeff = -1;
            if (coeff * a1 < coeff * a2)
                joints.Add(a1, a2);
            else
                joints.Add(a2, a1);
            count++;
            
        }

        foreach (var i in joints.Keys)
        {

            if (joints[i] < i)
            {
                LineRenderer lineRenderer1 = new GameObject("Snake").AddComponent<LineRenderer>();
                lineRenderer1.startColor = Color.gray;
                lineRenderer1.endColor = Color.red;
                lineRenderer1.startWidth = 0.1f;
                lineRenderer1.endWidth = 0.1f;
                lineRenderer1.positionCount = 2;
                lineRenderer1.useWorldSpace = true;
                lineRenderer1.transform.SetParent(can.transform, true);

                lineRenderer1.sortingOrder = 9;
                lineRenderer1.material = new Material(Shader.Find("Sprites/Default"));

                //For drawing line in the world space, provide the x,y,z values
                lineRenderer1.SetPosition(0, positions[joints[i]]); //x,y and z position of the starting point of the line
                lineRenderer1.SetPosition(1, positions[i]); //x,y and z position of the end point of the line
            }
            else
            {
                LineRenderer lineRenderer2 = new GameObject("Ladder").AddComponent<LineRenderer>();
                lineRenderer2.startColor = Color.blue;
                lineRenderer2.endColor = Color.blue;
                lineRenderer2.startWidth = 0.1f;
                lineRenderer2.endWidth = 0.1f;
                lineRenderer2.positionCount = 2;
                lineRenderer2.useWorldSpace = true;
                lineRenderer2.transform.SetParent(can.transform, true);

                lineRenderer2.sortingOrder = 9;
                lineRenderer2.material = new Material(Shader.Find("Sprites/Default"));
                //lineRenderer.material.color = Color.red;

                //For drawing line in the world space, provide the x,y,z values
                lineRenderer2.SetPosition(0, positions[joints[i]]); //x,y and z position of the starting point of the line
                lineRenderer2.SetPosition(1, positions[i]); //x,y and z position of the end point of the line
            }
        }
    }

    private void Update()
    {
        if (hasGameFinished || !canClick) return;
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (!hit.collider) return;

            if(hit.collider.CompareTag("Die"))
            {
                roll = 1 + Random.Range(0, 6);
                hit.collider.gameObject.GetComponent<Die>().Roll(roll);
                canClick = false;
            }
        }
    }
    public void MovePiece()
    {
        List<int> result= myboard.UpdateBoard(players[currentPlayer], roll);

        if(result.Count == 0)
        {
            canClick = true;
            currentPlayer = (currentPlayer + 1) % players.Count;
            message(players[currentPlayer]);
            return;
        }

        pieces[players[currentPlayer]].GetComponent<Piece>().SetMovement(result);
        canClick = true;

        if(result[result.Count - 1] == totalSquares - 1)
        {
            players.RemoveAt(currentPlayer);
            currentPlayer %= players.Count;// currentPlayer;
            if (players.Count == 1) hasGameFinished = true;
            message(players[currentPlayer]);
            return;
        }

        currentPlayer = roll == 6 ? currentPlayer : (currentPlayer + 1) % players.Count;
        message(players[currentPlayer]);
    }
}
