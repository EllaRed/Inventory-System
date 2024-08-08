using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is used to populate a grid with the given number of rows and columns.
/// <para>Emmanuella Dasilva-Domingos</para>
/// <para>Last Updated: August 7, 2024</para>
/// </summary>


public class PopulateGrid : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab; // The grid cells to instantiate and fill up the grid
    [SerializeField] private int rows; // The number of rows 
    [SerializeField] private int columns; // The number of columns 
    [SerializeField] private float horizontalSpacing; // The horizontal spacing between the cells
    [SerializeField] private float verticalSpacing; // The vertical spacing between the cells
    [SerializeField] private bool fixedGridSize; //if you want to expand this for scrollable inventories
    [SerializeField] private bool fixedCellSize = false;
    private GridLayoutGroup gridLayoutGroup; // The grid layout group component
    [SerializeField] private RectTransform gridPanel;
    [SerializeField] private bool fixedRows;
    [SerializeField] private bool fixedColumns;

    private GameObject[] cells;

    public GameObject[] Cells { get => cells; set => cells = value; }

    [SerializeField] private InventoryManager inventoryManager;

    private void Start()
    {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        gridPanel = GetComponent<RectTransform>();

        StartCoroutine(Populate());
    }

    /// <summary>
    /// This method populates the grid with the given number of rows and columns.
    /// Rows can still overflow, need to adjust the math for that, can't use content size fitter
    /// because cells would exist but not be seen.
    /// </summary>
    private IEnumerator Populate()
    {
        if (fixedColumns)
        {
            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayoutGroup.constraintCount = columns;
        }
        if (fixedRows)
        {
            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            gridLayoutGroup.constraintCount = rows;
        }


        if (!fixedCellSize)
        {

            gridLayoutGroup.spacing = new Vector2(horizontalSpacing, verticalSpacing);
            // Get the space available for the grid
            float panelWidth = gridPanel.rect.width - (gridLayoutGroup.padding.left + gridLayoutGroup.padding.right);
            float panelHeight = gridPanel.rect.height - (gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom);

            // Calculate the size of each cell
            float cellWidth = (panelWidth - (gridLayoutGroup.spacing.x * (columns - 1))) / columns;
            float cellHeight = (panelHeight - (gridLayoutGroup.spacing.y * (rows - 1))) / rows;
            float cellSize = Mathf.Min(cellHeight, cellWidth);

            gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize); //square cells
            float exactHorizontalSpacing = (panelWidth - (cellSize * columns)) / (columns - 1);
            gridLayoutGroup.spacing = new Vector2(exactHorizontalSpacing-((gridLayoutGroup.padding.left+1)/4), verticalSpacing);
        }

        if (cellPrefab != null)
        {

            Cells = new GameObject[rows * columns];

            for (int i = 0; i < rows * columns; i++)
            {
                GameObject cell = Instantiate(cellPrefab, transform);
                cell.name = "Cell " + i;
                Cells[i] = cell;

                SlotUI slotUI = cell.AddComponent<SlotUI>();
                slotUI.slot = cell.AddComponent<ItemSlot>();
            }

            inventoryManager.itemSlotList.AddRange(Cells);
            yield return null;
            inventoryManager.InitializeSlots();
        }
    }
}
