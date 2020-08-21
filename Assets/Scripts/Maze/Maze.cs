using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class Maze
{

    private TextAsset mazeDescription;
    private MazePhysicalSpecification specs;
    
    private GameObject startTrigger;
    private GameObject[] goalTriggers;

    GameObject theMaze;
    private PhysicMaterial mazeMaterial;

    public Maze(TextAsset mazeDescription, MazePhysicalSpecification specs, PhysicMaterial mazeMaterial)
    {
        this.mazeDescription = mazeDescription;
        this.specs = specs;
        this.mazeMaterial = mazeMaterial;
    }
    
    public Vector3 StartingPosition()
    {
        return startTrigger.transform.position;
    }

    public GameObject Build()
    {
        string text = mazeDescription.text;
        string[] lines = text.Split(
            new[] { "\r\n", "\r", "\n" },
            StringSplitOptions.None
        );

        GameObject maze = new GameObject("Maze");
        GameObject posts = new GameObject("Posts");
        GameObject walls = new GameObject("Walls");
        GameObject triggers = new GameObject("Triggers");
        triggers.transform.parent = maze.transform;
        posts.transform.parent = maze.transform;
        walls.transform.parent = maze.transform;
        List<GameObject> goals = new List<GameObject>();

        float yPos = 0.0f;
        for (int i = 0; i < lines.Length; i++)
        {
            float xPos = 0.0f;
            string line = lines[i];
            for (int j = 0; j < line.Length; j += 2)
            {
                char c = line[j];
                Vector3 pos = new Vector3(xPos, 0.0f, yPos);
                if (c == 'o')
                {
                    GameObject post = InstantiatePost();
                    post.transform.Translate(pos);
                    post.transform.parent = posts.transform;
                }
                else if (c == '-')
                {
                    GameObject wall = InstantiateWall();
                    wall.transform.Translate(pos);
                    wall.transform.parent = walls.transform;
                }
                else if (c == '|')
                {
                    GameObject wall = InstantiateWall();
                    wall.transform.Translate(pos);
                    wall.transform.Rotate(new Vector3(0, 90, 0));
                    wall.transform.parent = walls.transform;
                }
                else if (c == 'G')
                {
                    GameObject trigger = InstantiateAreaTrigger();
                    trigger.name = "Goal Trigger";
                    trigger.transform.position = pos;
                    trigger.transform.parent = triggers.transform;
                    goals.Add(trigger);
                }
                else if (c == 'S')
                {
                    GameObject trigger = InstantiateAreaTrigger();
                    trigger.transform.position = pos;
                    trigger.name = "Start Trigger";
                    trigger.transform.parent = triggers.transform;
                    this.startTrigger = trigger;
                }
                xPos += specs.CellSize/2;
            }
            yPos -= specs.CellSize/2;
        }

        this.goalTriggers = goals.ToArray();
        this.theMaze = maze;
        GameObject floor = InstantiateWallPrimitive();
        floor.name = "Floor";
        int xl = (lines[0].Length - 1)/4;
        int yl = (lines.Length - 1)/2;

        float xS = xl * specs.CellSize;
        float yS = yl * specs.CellSize;

        floor.transform.localScale = new Vector3(xl * specs.CellSize + specs.WallThickness, specs.WallHeight, yl * specs.CellSize + specs.WallThickness);
        floor.transform.Translate(0, -specs.WallHeight, 0);
        floor.transform.parent = theMaze.transform;
        posts.transform.Translate(-xS/2, 0.0f, yS/2);
        walls.transform.Translate(-xS/2, 0.0f, yS/2);
        triggers.transform.Translate(-xS/2, 0.0f, yS/2);
        return maze;
    }

    public void Unbuild()
    {
        GameObject.Destroy(theMaze);
    }

    private GameObject InstantiateWallPrimitive()
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        BoxCollider collider = wall.AddComponent<BoxCollider>();
        collider.material = mazeMaterial;
        Rigidbody rb = wall.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        return wall;
    }

    private GameObject InstantiatePost()
    {
        GameObject post = InstantiateWallPrimitive();
        post.transform.localScale = new Vector3(specs.WallThickness, specs.WallHeight, specs.WallThickness);
        post.name = "Post";
        return post;
    }

    private GameObject InstantiateWall()
    {
        GameObject wall = InstantiateWallPrimitive();
        wall.transform.localScale = new Vector3(specs.WallLength, specs.WallHeight, specs.WallThickness);
        wall.name = "Wall";
        return wall;
    }

    private GameObject InstantiateAreaTrigger()
    {
        GameObject trigger = new GameObject();
        BoxCollider col = trigger.AddComponent<BoxCollider>();
        col.isTrigger = true;
        trigger.transform.localScale = new Vector3(specs.CellSize, specs.WallHeight * 2, specs.CellSize);
        return trigger;
    }
}
