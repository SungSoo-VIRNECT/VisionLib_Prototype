using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TogglableObjects : MonoBehaviour
{
    public List<GameObject> connectedGameObjects = new List<GameObject>();
    private List<GameObject> gameObjectsDisabledByThis = new List<GameObject>();

    /// <summary>
    /// Enables all connected <see cref="GameObject"/>s that where disabled by this component
    /// -- i.e. via <see cref="SwitchOff"/> calls -- since the last time <see cref="SwitchOn"/>
    /// was called. 
    /// </summary>   
    public void SwitchOn()
    {
        foreach (var go in this.gameObjectsDisabledByThis)
        {
            go.SetActive(true);
        }
        this.gameObjectsDisabledByThis.Clear();
    }

    /// <summary>
    /// Disables all connected <see cref="GameObject"/>s that are not already disabled. 
    /// </summary>    
    public void SwitchOff()
    {
        this.gameObjectsDisabledByThis.AddRange(
            this.connectedGameObjects.Where(go => go != null && go.activeSelf).ToList());

        foreach (var go in this.gameObjectsDisabledByThis)
        {
            go.SetActive(false);
        }
    }
}
