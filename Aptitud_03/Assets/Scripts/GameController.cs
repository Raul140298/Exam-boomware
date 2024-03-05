using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Button button_2x2;
    [SerializeField] private Button button_2x4;
    [SerializeField] private Button button_4x2;
    [SerializeField] private GameObject pfb_2x2;
    [SerializeField] private GameObject pfb_2x4;
    [SerializeField] private GameObject pfb_4x2;
    [SerializeField] private TMP_Text warning;

    private Cell[,] grid;
    private List<Cell> cells_2x2_Possible;
    private List<Cell> cells_2x4_Possible;
    private List<Cell> cells_4x2_Possible;
    private int[] x_DIr = new[] { 1, 1, -1, -1 };
    private int[] y_DIr = new[] { 1, -1, 1, -1 };
    private int curr_Dir_Id = -1;
    
    //***************************************** CORE ******************************************
    void Start()
    {
        button_2x2.onClick.AddListener(OnButton2x2Clicked);
        button_2x4.onClick.AddListener(OnButton2x4Clicked);
        button_4x2.onClick.AddListener(OnButton4x2Clicked);

        InitializeCells();
    }

    private void OnDestroy()
    {
        button_2x2.onClick.RemoveAllListeners();
        button_2x4.onClick.RemoveAllListeners();
        button_4x2.onClick.RemoveAllListeners();
    }
    
    //***************************************** BUTTONS ******************************************

    private void OnButton2x2Clicked()
    {
        OnButtonClick(cells_2x2_Possible, 2, 2, pfb_2x2);
    }

    private void OnButton2x4Clicked()
    {
        OnButtonClick(cells_2x4_Possible, 2, 4, pfb_2x4);
    }

    private void OnButton4x2Clicked()
    {
        OnButtonClick(cells_4x2_Possible, 4, 2, pfb_4x2);
    }
    
    private void OnButtonClick(List<Cell> cellsPossible, int width, int height, GameObject prefab)
    {
        if (!PossibleToPlace(cellsPossible, width, height, prefab, true)) //Means there isn't possible to place a piece
        {
            warning.text = "Cannot insert more " + width + "x" + height + " pieces.";
            
            if (!PossibleToPlace(cells_2x2_Possible, 2, 2, prefab, false)) //Could be possible to place a 2x2
            {
                warning.text = "";
                SceneManager.LoadScene(0);
            }
        }
    }
    
    //***************************************** FUNCTIONS ******************************************

    private void InitializeCells()
    {
        grid = new Cell[8,8];
        cells_2x2_Possible = new List<Cell>();
        cells_2x4_Possible = new List<Cell>();
        cells_4x2_Possible = new List<Cell>();
        
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                grid[i,j] = new Cell(i, j);
                
                cells_2x2_Possible.Add(grid[i,j]);
                cells_2x4_Possible.Add(grid[i,j]);
                cells_4x2_Possible.Add(grid[i,j]);
            }
        }
    }

    private bool PossibleToPlace(List<Cell> cellsPossible, int width, int height, GameObject prefab, bool shouldPlace)
    {
        while (cellsPossible.Count > 0)
        {
            Cell cell = cellsPossible[Random.Range(0, cellsPossible.Count)];
            
            if (!cell.isEmpty)
            {
                //This is useful even if you only want to check for a 2x2 space because it cleans the list
                cellsPossible.Remove(cell);
                continue;
            }

            int x = cell.x;
            int y = cell.y;

            if (CanPlacePiece(x, y, width, height))
            {
                if (shouldPlace)
                {
                    PlacePiece(cellsPossible, x, y, width, height);
                
                    //There is an offset because the 2x2 pivot is in the bottom left position
                    float xOffset = (x_DIr[curr_Dir_Id] < 0) ? 1 : 0;
                    float yOffset = (y_DIr[curr_Dir_Id] < 0) ? 1 : 0;

                    Vector2 position = new Vector2(
                        x + x_DIr[curr_Dir_Id] * width / 2f + xOffset,
                        y + y_DIr[curr_Dir_Id] * height / 2f + yOffset
                    );
                
                    GameObject newGo = Instantiate(prefab, position, Quaternion.identity);
                    TweakGoColor(newGo, 0.2f);
                }
                
                return true;
            }

            cellsPossible.Remove(grid[x, y]); // Isn't possible to place a piece, but is Empty
        }

        return false;
    }

    private bool CanPlacePiece(int startX, int startY, int width, int height)
    {
        // Iterate 4 times becasue there are 4 ways to put the piece on the start position, this way
        // we save 4 possible searches on the Random.Range
        for (int k = 0; k < 4; k++)
        {
            int endX = startX + width * x_DIr[k];
            int endY = startY + height * y_DIr[k];

            bool canPlacePiece = true;

            for (int i = startX; i != endX; i += x_DIr[k])
            {
                if (i < 0 || i >= grid.GetLength(0))
                {
                    canPlacePiece = false;
                    break;
                }

                for (int j = startY; j != endY; j += y_DIr[k])
                {
                    if (j < 0 || j >= grid.GetLength(1) || !grid[i, j].isEmpty)
                    {
                        canPlacePiece = false;
                        break;
                    }
                }

                if (canPlacePiece == false) break;
            }

            if (canPlacePiece)
            {
                curr_Dir_Id = k;
                return true;
            }
        }

        return false;
    }

    private void PlacePiece(List<Cell> cellsPossible, int startX, int startY, int width, int height)
    {
        //Use curr_Dir_Id to iterate the same as CanPlacePiece did when return true
        
        int endX = startX + width * x_DIr[curr_Dir_Id];
        int endY = startY + height * y_DIr[curr_Dir_Id];
        
        for (int i = startX; i != endX; i += x_DIr[curr_Dir_Id])
        {
            for (int j = startY; j != endY; j += y_DIr[curr_Dir_Id])
            {
                grid[i, j].isEmpty = false;
                cellsPossible.Remove(grid[i, j]);
            }
        }
    }

    private void TweakGoColor(GameObject go, float variation)
    {
        Color baseColor = go.GetComponent<Renderer>().material.color;
                    
        float colorVariation = variation;
        Color newColor = new Color(
            baseColor.r + Random.Range(-colorVariation, colorVariation),
            baseColor.g + Random.Range(-colorVariation, colorVariation),
            baseColor.b + Random.Range(-colorVariation, colorVariation)
        );

        go.GetComponent<Renderer>().material.color = newColor;
    }
}
