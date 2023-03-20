using UnityEngine;
using System.IO;

public class TerrainEditor : MonoBehaviour
{
    public int gridSize = 100; // 100 x 100 cell
    public float cellSize = 1f; // size for each cell 1unit x 1unit
    public GameObject defaultPrefab;

    public float raiseAndLowerAmount = 1f;
    public float maxHeight = 60f;
    private float minHeight = 0f;

    // 100 x 100 array to store all the cell objects
    private GameObject[,] cells;
    // store the height for each cell
    private float[,] cellHeights;

    // x coord for selected cell
    private int selectedX;
    // z coord for selected cell
    private int selectedZ;

    private bool _enableMouseEvent;

    // Start is called before the first frame update
    void Start()
    {
        selectedX = -1;
        selectedZ = -1;
        _enableMouseEvent = true;

        // instantiate the 2d cell array
        cells = new GameObject[gridSize, gridSize];
        cellHeights = new float[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                // instantiate cell from default prefab
                var cell = Instantiate(defaultPrefab, transform);

                cell.transform.position = new Vector3(x * cellSize, 0f, z * cellSize);
                cell.transform.localScale = new Vector3(cellSize, 1f, cellSize);

                //todo change to getter/setter
                cell.GetComponent<CubeEffect>().X = x;
                cell.GetComponent<CubeEffect>().Z = z;
                

                cells[x, z] = cell;
                
                cellHeights[x,z] = 0f;
            }
        }
    }

    void Update()
    {
        if (_enableMouseEvent && Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                GameObject cube = hit.collider.gameObject;
                if (cube.GetComponent<CubeEffect>() is CubeEffect cubeEffect) {
                    int x = cubeEffect.X;
                    int z = cubeEffect.Z;

                    if (selectedX >= 0 && selectedX < gridSize && selectedZ >=0 && selectedZ < gridSize) {
                        var oldCell = cells[selectedX, selectedZ];
                        oldCell.GetComponent<CubeEffect>().SetSelected(false);
                    }
                    
                    cube.GetComponent<CubeEffect>().SetSelected(true);

                    selectedX = x;
                    selectedZ = z;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RaiseHeight();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LowerHeight();
        }

        // // Check for the Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Quit the application
            Application.Quit();
        }
    }

    public int GetSelectedX() { return selectedX; }
    public int GetSelectedZ() { return selectedZ; }

    public float GetSelectedHeight()
    {
        if (selectedX >= 0 && selectedX < gridSize && selectedZ >= 0 && selectedZ < gridSize) { 
            return cellHeights[selectedX, selectedZ];
        }

        return 0;
    }

    public void RaiseHeight()
    {
        if (selectedX >= 0 && selectedX < gridSize &&
            selectedZ >= 0 && selectedZ < gridSize)
        {
            var raisedHeight = cellHeights[selectedX,selectedZ] + raiseAndLowerAmount;
            if (raisedHeight >= minHeight && raisedHeight <= maxHeight) {   
                SetHeight(selectedX, selectedZ, raisedHeight);
            }
        }
    }

    public void LowerHeight()
    {
        if (selectedX >= 0 && selectedX < gridSize &&
            selectedZ >= 0 && selectedZ < gridSize) {
            var loweredHeight = cellHeights[selectedX,selectedZ] - raiseAndLowerAmount;
            if (loweredHeight >= minHeight && loweredHeight <= maxHeight) {   
                SetHeight(selectedX, selectedZ, loweredHeight);
            }
        }
    }

    public void SaveData()
    {
        string filePath = "terrain_data.txt";
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // col title
            string title = "Terrain Type".PadRight(15) + "Height".PadRight(15);
            writer.WriteLine(title);
            for (int x = 0; x < gridSize; ++x) {
                for (int z = 0; z < gridSize; ++z) {
                    string line = $"({x}, {z})".PadRight(15) + cellHeights[x,z].ToString().PadRight(15);
                    writer.WriteLine(line);
                }
            }
        }
    }

    internal void DisableMouseEvents()
    {
        _enableMouseEvent = false;
    }

    internal void EnableMouseEvents()
    {
        _enableMouseEvent = true;
    }

    private void SetHeight(int x, int z, float height)
    {
        float baseHeight = 0.5f;
        var cell = cells[x, z];
        var mesh = cell.GetComponent<MeshFilter>().mesh;
        var vertices = mesh.vertices;
        var normals = mesh.normals;

        for (int i = 0; i < vertices.Length; i++)
        {
            if (vertices[i].y >= 0f)
            {
                vertices[i].y = (height + baseHeight);
            }
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        cellHeights[x,z] = height;
    }
}
