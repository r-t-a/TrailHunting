using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using TrailHunting.Scripts.Enums;
using TrailHunting.Scripts.Managers;
using TrailHunting.Scripts;
using TrailHunting.Scripts.Enums.Terrain;

public class TopDownStart : Node2D
{
    #region Properties
    public int SmallGameCounter;
    public int MediumGameCounter;
    public int MedLargeGameCounter;
    public int LargeGameCounter;
    
    public TileMap GroundTileMap;
    public Timer SpawnTimer;
    public Timer GameTimer;
    public Button EndButton;
    public Label DisplayTimer;
    public PackedScene Player;
    public PackedScene Deer;
    public PackedScene Rabbit;
    public PackedScene Squirrel;
    public PackedScene Bear;
    public PackedScene Buck;
    public PackedScene Buffalo;

    private List<Area2D> spawns = new List<Area2D>();
    private MapType currentMap;
    #endregion

    #region Overrides
    public override void _Ready()
    {
        GroundTileMap = GetNodeOrNull<TileMap>("TileMap");
        SpawnTimer = GetNodeOrNull<Timer>("SpawnTimer");
        GameTimer = GetNodeOrNull<Timer>("GameTimer");
        EndButton = GetNodeOrNull<Button>("CanvasLayer/End");
        DisplayTimer = GetNodeOrNull<Label>("CanvasLayer/DisplayTimer");

        Player = (PackedScene)ResourceLoader.Load("res://Scenes/HuntingPlayer.tscn");
        Deer = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/TopDown/Deer.tscn");
        Rabbit = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/TopDown/Rabbit.tscn");
        Squirrel = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/TopDown/Squirrel.tscn");
        Buffalo = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/TopDown/Buffalo.tscn");
        Bear = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/TopDown/Bear.tscn");
        Buck = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/TopDown/Buck.tscn");

        spawns.Add(GetNodeOrNull<Area2D>("Spawns/Spawn1"));
        spawns.Add(GetNodeOrNull<Area2D>("Spawns/Spawn2"));
        spawns.Add(GetNodeOrNull<Area2D>("Spawns/Spawn3"));
        spawns.Add(GetNodeOrNull<Area2D>("Spawns/Spawn4"));
        spawns.Add(GetNodeOrNull<Area2D>("Spawns/Spawn5"));
        spawns.Add(GetNodeOrNull<Area2D>("Spawns/Spawn6"));
        spawns.Add(GetNodeOrNull<Area2D>("Spawns/Spawn7"));
        spawns.Add(GetNodeOrNull<Area2D>("Spawns/Spawn8"));
        spawns.Add(GetNodeOrNull<Area2D>("Spawns/Spawn9"));
        spawns.Add(GetNodeOrNull<Area2D>("Spawns/Spawn10"));
        spawns.Add(GetNodeOrNull<Area2D>("Spawns/Spawn11"));
        spawns.Add(GetNodeOrNull<Area2D>("Spawns/Spawn12"));

        if (GameManager.PlayerManager.IsEndless)
        {
            EndButton.Visible = true;
            DisplayTimer.Visible = false;
            GameTimer.Stop();
        }
        else
        {
            EndButton.Visible = false;
            DisplayTimer.Visible = true;
            GameTimer.Start();
        }

        BuildLevel();
    }

    public override void _Process(float delta)
    {
        if (DisplayTimer.Visible)
        {
            DisplayTimer.Text = Mathf.FloorToInt(GameTimer.TimeLeft).ToString();
        }
    }
    #endregion

    #region Events
    private void _on_Squirrel_SmallGameDead()
    {
        SmallGameCounter += 1;
    }

    private void _on_Rabbit_SmallGameDead()
    {
        SmallGameCounter += 1;
    }

    private void _on_Deer_MediumGameDead()
    {
        MediumGameCounter += 1;
    }

    private void _on_Bear_MedLargeGameDead()
    {
        MedLargeGameCounter += 1;
    }

    private void _on_Buffalo_LargeGameDead()
    {
        LargeGameCounter += 1;
    }

    private void _on_SpawnTimer_timeout()
    {
        var currentSpawn = GetTree().GetNodesInGroup("Animals");
        if (currentSpawn.Count >= Constants.MaxAnimalSpawn)
            return;

        var boundaryTiles = GroundTileMap.GetUsedCellsById(1);
        var spawnTile = (Vector2)boundaryTiles[new Random().Next(boundaryTiles.Count - 1)];
        var randAnimal = new Random().Next(6);

        if (spawnTile.x == 0)
        {
            SpawnAnimal(randAnimal, spawnTile, SpawnQuadrant.Left);
        }
        if (spawnTile.y == 0)
        {
            SpawnAnimal(randAnimal, spawnTile, SpawnQuadrant.Top);
        }
        if (spawnTile.x == 47)
        {
            SpawnAnimal(randAnimal, spawnTile, SpawnQuadrant.Right);
        }
        if (spawnTile.y == 36)
        {
            SpawnAnimal(randAnimal, spawnTile, SpawnQuadrant.Bottom);
        }
    }

    private void _on_GameTimer_timeout()
    {
        SpawnTimer.Stop();
        GameManager.BuildTopDownResultsDialog(SmallGameCounter, MediumGameCounter, MedLargeGameCounter, LargeGameCounter);
        GameManager.ResultsDialog.Show();
    }

    private void _on_AcceptDialog_modal_closed()
    {
        GameManager.ResultsDialog.Hide();
        GameManager.End();
    }

    private void _on_AcceptDialog_confirmed()
    {
        GameManager.ResultsDialog.Hide();
        GameManager.End();
    }

    private void _on_End_button_up()
    {
        SpawnTimer.Stop();
        GameManager.BuildTopDownResultsDialog(SmallGameCounter, MediumGameCounter, MedLargeGameCounter, LargeGameCounter);
        GameManager.ResultsDialog.Show();
    }
    #endregion

    #region Methods
    private void BuildLevel()
    {
        SpawnPlayer();
        BuildMap();
    }

    private void SpawnPlayer()
    {
        var spawnPoint = spawns.FirstOrDefault(x => x.Name == GameManager.GetSpawnPoint());
        spawnPoint = spawnPoint ?? GetNodeOrNull<Area2D>("Spawns/Spawn1"); // If something goes wrong default to Spawn1
        var player = (KinematicBody2D)Player.Instance();
        player.Position = spawnPoint.Position;
        AddChild(player);
        spawns.Remove(spawnPoint);
    }

    private void SpawnTerrain(StaticBody2D terrainObject)
    {
        var spawnPoint = spawns.FirstOrDefault(x => x.Name == GameManager.GetSpawnPoint());
        spawnPoint = spawnPoint ?? spawns.First();
        if (terrainObject != null)
        {
            terrainObject.Position = spawnPoint.Position;
            AddChild(terrainObject);
            spawns.Remove(spawnPoint);
        }
    }

    private void SpawnAnimal(int animal, Vector2 location, SpawnQuadrant spawn)
    {
        var global = GroundTileMap.MapToWorld(location);
        switch ((Animals)animal)
        {
            case Animals.Squirrel:
                var squirrel = (KinematicBody2D)Squirrel.Instance();
                squirrel.GlobalPosition = global;
                squirrel.Call("SetSpawn", (int)spawn);
                AddChild(squirrel);
                break;
            case Animals.Rabbit:
                var rabbit = (KinematicBody2D)Rabbit.Instance();
                rabbit.GlobalPosition = global;
                rabbit.Call("SetSpawn", (int)spawn);
                AddChild(rabbit);
                break;
            case Animals.Doe:
                var deer = (KinematicBody2D)Deer.Instance();
                deer.GlobalPosition = global;
                deer.Call("SetSpawn", (int)spawn);
                AddChild(deer);
                break;
            case Animals.Buck:
                var buck = (KinematicBody2D)Buck.Instance();
                buck.GlobalPosition = global;
                buck.Call("SetSpawn", (int)spawn);
                AddChild(buck);
                break;
            case Animals.Bear:
                var bear = currentMap != MapType.Mountains || currentMap != MapType.Woods
                    ? (KinematicBody2D)Rabbit.Instance()
                    : (KinematicBody2D)Bear.Instance();
                bear.GlobalPosition = global;
                bear.Call("SetSpawn", (int)spawn);
                AddChild(bear);
                break;
            case Animals.Buffalo:
                var buffalo = currentMap != MapType.Plains 
                    ? (KinematicBody2D)Buffalo.Instance()
                    : (KinematicBody2D)Squirrel.Instance();
                buffalo.GlobalPosition = global;
                buffalo.Call("SetSpawn", (int)spawn);
                AddChild(buffalo);
                break;
            default:
                var defaultMoreSquirrel = (KinematicBody2D)Squirrel.Instance();
                defaultMoreSquirrel.GlobalPosition = global;
                defaultMoreSquirrel.Call("SetSpawn", (int)spawn);
                AddChild(defaultMoreSquirrel);
                break;
        }
    }

    private void BuildMap()
    {
        (MapType map, List<int> terrainObjects) = GameManager.BuildTopDownLevel();
        currentMap = map;
        switch (map)
        {
            default:
            case MapType.Woods:
                BuildWoodsLevel(terrainObjects);
                break;
            case MapType.Desert:
                BuildDesertLevel(terrainObjects);
                break;
            case MapType.Plains:
                BuildPlainsLevel(terrainObjects);
                break;
            case MapType.Mountains:
                BuildMountainLevel(terrainObjects);
                break;
        }
    }

    private void BuildWoodsLevel(List<int> levelObjects)
    {
        var tree = (PackedScene)ResourceLoader.Load("res://Scenes/Foliage/Tree.tscn");
        var bush = (PackedScene)ResourceLoader.Load("res://Scenes/Foliage/Bush.tscn");
        var redFlower = (PackedScene)ResourceLoader.Load("res://Scenes/Foliage/RedFlower.tscn");
        var purpleFlower = (PackedScene)ResourceLoader.Load("res://Scenes/Foliage/PurpleFlower.tscn");

        foreach (var levelObject in levelObjects)
        {
            var objectType = (WoodTerrain)levelObject;

            StaticBody2D spawnObject;
            switch (objectType)
            {
                default:
                case WoodTerrain.Tree:
                    spawnObject = (StaticBody2D)tree.Instance();
                    break;
                case WoodTerrain.Bush:
                    spawnObject = (StaticBody2D)bush.Instance();
                    break;
                case WoodTerrain.RedFlower:
                    spawnObject = (StaticBody2D)redFlower.Instance();
                    break;
                case WoodTerrain.PurpleFlower:
                    spawnObject = (StaticBody2D)purpleFlower.Instance();
                    break;
            }
            SpawnTerrain(spawnObject);
        }
    }

    private void BuildDesertLevel(List<int> levelObjects)
    {
        var cactus = (PackedScene)ResourceLoader.Load("res://Scenes/Foliage/Cactus.tscn");
        var bush = (PackedScene)ResourceLoader.Load("res://Scenes/Foliage/Bush.tscn");

        foreach (var levelObject in levelObjects)
        {
            var objectType = (DesertTerrain)levelObject;

            StaticBody2D spawnObject;
            switch (objectType)
            {
                default:
                case DesertTerrain.Cactus:
                    spawnObject = (StaticBody2D)cactus.Instance();
                    break;
                case DesertTerrain.Bush:
                    spawnObject = (StaticBody2D)bush.Instance();
                    break;
            }
            SpawnTerrain(spawnObject);
        }
    }

    private void BuildPlainsLevel(List<int> levelObjects)
    {
        var bush = (PackedScene)ResourceLoader.Load("res://Scenes/Foliage/Bush.tscn");
        var redFlower = (PackedScene)ResourceLoader.Load("res://Scenes/Foliage/RedFlower.tscn");
        var purpleFlower = (PackedScene)ResourceLoader.Load("res://Scenes/Foliage/PurpleFlower.tscn");

        foreach (var levelObject in levelObjects)
        {
            var objectType = (PlainsTerrain)levelObject;

            StaticBody2D spawnObject;
            switch (objectType)
            {
                default:
                case PlainsTerrain.Bush:
                    spawnObject = (StaticBody2D)bush.Instance();
                    break;
                case PlainsTerrain.RedFlower:
                    spawnObject = (StaticBody2D)redFlower.Instance();
                    break;
                case PlainsTerrain.PurpleFlower:
                    spawnObject = (StaticBody2D)purpleFlower.Instance();
                    break;
            }
            SpawnTerrain(spawnObject);
        }
    }

    private void BuildMountainLevel(List<int> levelObjects)
    {
        var pine = (PackedScene)ResourceLoader.Load("res://Scenes/Foliage/Pine.tscn");
        var bush = (PackedScene)ResourceLoader.Load("res://Scenes/Foliage/Bush.tscn");
        var redFlower = (PackedScene)ResourceLoader.Load("res://Scenes/Foliage/RedFlower.tscn");
        var rock = (PackedScene)ResourceLoader.Load("res://Scenes/Foliage/Rock.tscn");

        foreach (var levelObject in levelObjects)
        {
            var objectType = (MoutainTerrain)levelObject;

            StaticBody2D spawnObject;
            switch (objectType)
            {
                default:
                case MoutainTerrain.Tree:
                    spawnObject = (StaticBody2D)pine.Instance();
                    break;
                case MoutainTerrain.Bush:
                    spawnObject = (StaticBody2D)bush.Instance();
                    break;
                case MoutainTerrain.RedFlower:
                    spawnObject = (StaticBody2D)redFlower.Instance();
                    break;
                case MoutainTerrain.Rock:
                    spawnObject = (StaticBody2D)rock.Instance();
                    break;
            }
            SpawnTerrain(spawnObject);
        }
    }

    private void Cleanup()
    {
        var currentSpawn = GetTree().GetNodesInGroup("Animals");
        foreach (var spawn in currentSpawn)
        {
            if (spawn is KinematicBody2D node)
            {
                node.QueueFree();
            }
        }
    }
    #endregion
}