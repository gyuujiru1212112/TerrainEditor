using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    public Transform target;

    private TextMeshProUGUI _selectedText;
    private TextMeshProUGUI _heightText;
    private TerrainEditor _terrainEditor;

    // Start is called before the first frame update
    void Start()
    {
        // get the terrain editor
        _terrainEditor = target.GetComponent<TerrainEditor>();

        // get all the children text and assign to default strings
        var textChildren = GetComponentInChildren<TextMeshProUGUI>();
        _selectedText = transform.Find("SelectedText").GetComponent<TextMeshProUGUI>();
        _heightText = transform.Find("HeightText").GetComponent<TextMeshProUGUI>();

        _selectedText.text = "Selected Cell: (-1, -1)";
        _heightText.text = $"Height: 0 (Max: {_terrainEditor.maxHeight})";
    }

    // Update is called once per frame
    void Update()
    {
        // get the data and change the text
        int selectedX = _terrainEditor.GetSelectedX();
        int selectedZ = _terrainEditor.GetSelectedZ();
        float selectedH = _terrainEditor.GetSelectedHeight();
        _selectedText.text = $"Selected Cell: ({selectedX}, {selectedZ})";
        _heightText.text = $"Height: {selectedH} (Max: {_terrainEditor.maxHeight})";
    }
}
