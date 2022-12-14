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
    [Export]
    public int SmallGameCounter;
    [Export]
    public int MediumGameCounter;
    [Export]
    public int MedLargeGameCounter;
    [Export]
    public int LargeGameCounter;

    public PackedScene Player;
    public PackedScene Deer;
    public PackedScene Rabbit;
    public PackedScene Squirrel;
    public PackedScene Bear;
    public PackedScene Buck;
    public PackedScene Buffalo;

    private TileMap groundTileMap;
    private List<Area2D> spawns = new List<Area2D>();

    public override void _Ready()
    {
        groundTileMap = GetNode<TileMap>("TileMap");

        Player = (PackedScene)ResourceLoader.Load("res://Scenes/HuntingPlayer.tscn");
        Deer = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/Deer.tscn");
        Rabbit = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/Rabbit.tscn");
        Squirrel = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/Squirrel.tscn");
        Buffalo = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/Rabbit.tscn");
        Bear = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/Bear.tscn");
        Buck = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/Buck.tscn");

        spawns.Add(GetNode<Area2D>("Spawns/Spawn1"));
        spawns.Add(GetNode<Area2D>("Spawns/Spawn2"));
        spawns.Add(GetNode<Area2D>("Spawns/Spawn3"));
        spawns.Add(GetNode<Area2D>("Spawns/Spawn4"));
        spawns.Add(GetNode<Area2D>("Spawns/Spawn5"));
        spawns.Add(GetNode<Area2D>("Spawns/Spawn6"));
        spawns.Add(GetNode<Area2D>("Spawns/Spawn7"));
        spawns.Add(GetNode<Area2D>("Spawns/Spawn8"));
        spawns.Add(GetNode<Area2D>("Spawns/Spawn9"));
        spawns.Add(GetNode<Area2D>("Spawns/Spawn10"));
        spawns.Add(GetNode<Area2D>("Spawns/Spawn11"));
        spawns.Add(GetNode<Area2D>("Spawns/Spawn12"));

        BuildLevel();
    }

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

    private void _on_HuntingPlayer_ShotBullet()
    {
    }

    private void _on_SpawnTimer_timeout()
    {
        var currentSpawn = GetTree().GetNodesInGroup("Animals");
        if (currentSpawn.Count >= Constants.MaxAnimalSpawn)
            return;

        var boundaryTiles = groundTileMap.GetUsedCellsById(1);
        boundaryTiles.Shuffle();
        var spawnTile = (Vector2)boundaryTiles[new Random().Next(0, boundaryTiles.Count - 1)];
        var randAnimal = new Random().Next(0, 6);

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
    #endregion

    private void BuildLevel()
    {
        SpawnPlayer();
        BuildMap();
    }

    private void SpawnPlayer()
    {
        var spawnPoint = spawns.FirstOrDefault(x => x.Name == GameManager.GetSpawnPoint());
        spawnPoint = spawnPoint ?? GetNode<Area2D>("Spawns/Spawn1"); // If something goes wrong default to Spawn1
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
        var global = groundTileMap.MapToWorld(location);
        switch ((Animals)animal)
        {
            case Animals.Squirrel:
                var squirrel = (KinematicBody2D)Squirrel.Instance();
                squirrel.GlobalPosition = global;
                squirrel.Call("SetSpawn", spawn);
                AddChild(squirrel);
                break;
            case Animals.Rabbit:
                var rabbit = (KinematicBody2D)Rabbit.Instance();
                rabbit.GlobalPosition = global;
                rabbit.Call("SetSpawn", spawn);
                AddChild(rabbit);
                break;
            case Animals.Doe:
                var deer = (KinematicBody2D)Deer.Instance();
                deer.GlobalPosition = global;
                deer.Call("SetSpawn", spawn);
                AddChild(deer);
                break;
            case Animals.Buck:
                var buck = (KinematicBody2D)Buck.Instance();
                buck.GlobalPosition = global;
                buck.Call("SetSpawn", spawn);
                AddChild(buck);
                break;
            case Animals.Bear:
                var bear = (KinematicBody2D)Bear.Instance();
                bear.GlobalPosition = global;
                bear.Call("SetSpawn", spawn);
                AddChild(bear);
                break;
            case Animals.Buffalo:
                var buffalo = (KinematicBody2D)Buffalo.Instance();
                buffalo.GlobalPosition = global;
                buffalo.Call("SetSpawn", spawn);
                AddChild(buffalo);
                break;
            default:
                var defaultMoreSquirrel = (KinematicBody2D)Squirrel.Instance();
                defaultMoreSquirrel.GlobalPosition = global;
                defaultMoreSquirrel.Call("SetSpawn", spawn);
                AddChild(defaultMoreSquirrel);
                break;
        }
    }

    private void BuildMap() 
    {
        (MapType map, List<int> terrainObjects) = GameManager.BuildTopDownLevel();
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

    private void _on_EndButton_button_up()
    {
        GameManager.End();
    }
}
