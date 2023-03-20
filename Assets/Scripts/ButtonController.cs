using System.Collections;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject target;

    public void OnClick() {
        // Disable mouse events on target object
        TerrainEditor terrainEditor = target.GetComponent<TerrainEditor>();
        if (terrainEditor != null) {
            terrainEditor.DisableMouseEvents();
        }

        // Wait for a few seconds, and then re-enable mouse events on target object
        StartCoroutine(EnableMouseEventsAfterDelay(terrainEditor, 2.0f));
    }

    IEnumerator EnableMouseEventsAfterDelay(TerrainEditor terrainEditor, float delay) {
        yield return new WaitForSeconds(delay);
        if (terrainEditor != null) {
            terrainEditor.EnableMouseEvents();
        }
    }
}
