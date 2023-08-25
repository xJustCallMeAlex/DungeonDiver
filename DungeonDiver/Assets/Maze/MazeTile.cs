using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeTile : MonoBehaviour
{
    [SerializeField] GameObject[] walls;
    [SerializeField] SpriteRenderer floor;

    public void RemoveWall(int wallToRemove)
    {
        if (wallToRemove >= 0 && wallToRemove < walls.Length)
        {
            walls[wallToRemove].gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Invalid wall index: " + wallToRemove);
        }
    }

    public void SetState(NodeState state)
    {
        switch (state)
        {
            case NodeState.Available:
                floor.material.color = Color.white; break;
            case NodeState.Current:
                floor.material.color = Color.yellow; break;
            case NodeState.Completed:
                floor.material.color = Color.blue; break;
        }
    }
}
