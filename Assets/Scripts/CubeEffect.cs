using UnityEngine;

public class CubeEffect : MonoBehaviour
{
    public Material highlightMaterial; // material used for highlighting
    public Material selectMaterial; // material used for selecting

    public int X, Z;

    private bool _isSelected;

    private Material _defaultMaterial; // default material of the cube

    // Start is called before the first frame update
    void Start()
    {
        // Get the default material from the MeshRenderer
        _isSelected = false;
        _defaultMaterial = GetComponent<MeshRenderer>().material;
    }

    public void SetSelected(bool isSelected) {        
        if (isSelected) {
            GetComponent<MeshRenderer>().material = selectMaterial;
        }
        else {
            GetComponent<MeshRenderer>().material = _defaultMaterial;
        }

        _isSelected = isSelected;
    }

    private void OnMouseEnter()
    {
        // When get hovered over, change to the highlight material
        GetComponent<MeshRenderer>().material = highlightMaterial;
    }

    private void OnMouseExit()
    {
        if (!_isSelected) {
        // not selected, change to the default material
            GetComponent<MeshRenderer>().material = _defaultMaterial;
        }
        else {
            // selected, change to the select material
            GetComponent<MeshRenderer>().material = selectMaterial;
        }
    }
}
