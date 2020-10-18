using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int X = 0;
    public int Z = 0;

    public World World;

    private void Start()
    {
        var tile = World.GetTileAt(X, Z);
        this.transform.position = tile.transform.position + new Vector3(0, 0.55f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Move(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Move(-1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Move(0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Move(1, 0);
        }
    }

    public void Move(int horizontal, int vertical)
    {
        var tileObject = World.GetTileAt(X + horizontal, Z + vertical);
        if (tileObject is null)
        {
            return;
        }

        var targetPosition = tileObject.transform.position + new Vector3(0, 0.55f);
        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, 1);

        var tile = tileObject.GetComponent<WorldTile>();
        this.X = tile.X;
        this.Z = tile.Z;
    }
}
