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
    #region Exports
    [Export]
    protected NodePath ResultsPopupNodePath { get; private set; }
    [Export]
    protected NodePath TileMapPath { get; private set; }
    [Export]
    protected NodePath SpawnTimerPath { get; private set; }
    [Export]
    protected NodePath SpawnNodePath { get; private set; }
    [Export]
    protected NodePath AmmoValueNodePath { get; private set; }
    [Export]
    protected NodePath ScoreValueNodePath { get; private set; }
    [Export]
    protected PackedScene HuntingPlayer { get; private set; }
    [Export]
    protected PackedScene Deer { get; private set; }
    [Export]
    protected PackedScene Rabbit { get; private set; }
    [Export]
    protected PackedScene Squirrel { get; private set; }
    [Export]
    protected PackedScene Bear { get; private set; }
    [Export]
    protected PackedScene Buck { get; private set; }
    [Export]
    protected PackedScene Buffalo { get; private set; }
    #endregion

    #region Properties
    public int SmallGameCounter;
    public int MediumGameCounter;
    public int MedLargeGameCounter;
    public int LargeGameCounter;

    private HuntingPlayer Player;
    private Results resultsPopup;
    private TileMap groundTileMap;
    private Timer spawnTimer;
    private Label ammoValue;
    private Label scoreValue;
    private int hitCount = 0;

    private List<Area2D> spawns = new List<Area2D>();
    private MapType currentMap;
    #endregion

    #region Overrides
    public override void _Ready()
    {
        resultsPopup = GetNodeOrNull<Results>(ResultsPopupNodePath);
        groundTileMap = GetNodeOrNull<TileMap>(TileMapPath);
        spawnTimer = GetNodeOrNull<Timer>(SpawnTimerPath);
        ammoValue = GetNodeOrNull<Label>(AmmoValueNodePath);
        scoreValue = GetNodeOrNull<Label>(ScoreValueNodePath);

        spawns.Add(GetNodeOrNull<Area2D>($"{SpawnNodePath}/Spawn1"));
        spawns.Add(GetNodeOrNull<Area2D>($"{SpawnNodePath}/Spawn2"));
        spawns.Add(GetNodeOrNull<Area2D>($"{SpawnNodePath}/Spawn3"));
        spawns.Add(GetNodeOrNull<Area2D>($"{SpawnNodePath}/Spawn4"));
        spawns.Add(GetNodeOrNull<Area2D>($"{SpawnNodePath}/Spawn5"));
        spawns.Add(GetNodeOrNull<Area2D>($"{SpawnNodePath}/Spawn6"));
        spawns.Add(GetNodeOrNull<Area2D>($"{SpawnNodePath}/Spawn7"));
        spawns.Add(GetNodeOrNull<Area2D>($"{SpawnNodePath}/Spawn8"));
        spawns.Add(GetNodeOrNull<Area2D>($"{SpawnNodePath}/Spawn9"));
        spawns.Add(GetNodeOrNull<Area2D>($"{SpawnNodePath}/Spawn10"));

        BuildLevel();
    }

    public override void _Process(float delta)
    {
        scoreValue.Text = GameManager.CurrentScore.ToString();
    }
    #endregion

    #region Events
    private void onShotBullet()
    {
        ammoValue.Text = Player.AmmoCount.ToString();
        if (Player.AmmoCount == 0) spawnTimer.Stop();
    }

    private void onBulletHit()
    {
        hitCount += 1;
        GameManager.UpdateScore(100);
    }

    private void _on_Squirrel_SmallGameDead()
    {
        GameManager.UpdatePlayerStats(Animals.Squirrel);
        SmallGameCounter += 1;
    }

    private void _on_Rabbit_SmallGameDead()
    {
        GameManager.UpdatePlayerStats(Animals.Rabbit);
        SmallGameCounter += 1;
    }

    private void _on_Deer_MediumGameDead()
    {
        GameManager.UpdatePlayerStats(Animals.Doe);
        MediumGameCounter += 1;
    }

    private void _on_Buck_MediumGameDead()
    {
        GameManager.UpdatePlayerStats(Animals.Buck);
        MediumGameCounter += 1;
    }

    private void _on_Bear_MedLargeGameDead()
    {
        GameManager.UpdatePlayerStats(Animals.Bear);
        MedLargeGameCounter += 1;
    }

    private void _on_Buffalo_LargeGameDead()
    {
        GameManager.UpdatePlayerStats(Animals.Buffalo);
        LargeGameCounter += 1;
    }

    private void _on_SpawnTimer_timeout()
    {
        var currentSpawn = GetTree().GetNodesInGroup(Constants.Animals);
        if (currentSpawn.Count >= GameManager.MaxAnimalSpawn)
            return;

        var boundaryTiles = groundTileMap.GetUsedCellsById(1);
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
    #endregion

    #region Methods
    private void BuildLevel()
    {
        SpawnPlayer();
        BuildMap();
        SetupUI();
    }

    private void SetupUI()
    {
        resultsPopup.Hide();
        ammoValue.Text = Player.AmmoCount.ToString();
        scoreValue.Text = GameManager.CurrentScore.ToString();
    }

    private void SpawnPlayer()
    {
        var spawnPoint = spawns.FirstOrDefault(x => x.Name == GameManager.GetSpawnPoint());
        spawnPoint = spawnPoint ?? GetNodeOrNull<Area2D>($"{SpawnNodePath}/Spawn1"); // Default to Spawn 1 if null
        Player = HuntingPlayer.Instance() as HuntingPlayer;
        Player.Position = spawnPoint.Position;
        AddChild(Player);
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
                var spawnSquirrel = (KinematicBody2D)Squirrel.Instance();
                spawnSquirrel.GlobalPosition = global;
                spawnSquirrel.Call(Constants.SetSpawn, (int)spawn);
                AddChild(spawnSquirrel);
                break;
            case Animals.Rabbit:
                var spawnRabbit = (KinematicBody2D)Rabbit.Instance();
                spawnRabbit.GlobalPosition = global;
                spawnRabbit.Call(Constants.SetSpawn, (int)spawn);
                AddChild(spawnRabbit);
                break;
            case Animals.Doe:
                var spawnDoe = (KinematicBody2D)Deer.Instance();
                spawnDoe.GlobalPosition = global;
                spawnDoe.Call(Constants.SetSpawn, (int)spawn);
                AddChild(spawnDoe);
                break;
            case Animals.Buck:
                var spawnBuck = (KinematicBody2D)Buck.Instance();
                spawnBuck.GlobalPosition = global;
                spawnBuck.Call(Constants.SetSpawn, (int)spawn);
                AddChild(spawnBuck);
                break;
            case Animals.Bear:
                var spawnBearOrRabbit = currentMap != MapType.Mountains || currentMap != MapType.Woods
                    ? (KinematicBody2D)Rabbit.Instance()
                    : (KinematicBody2D)Bear.Instance();
                spawnBearOrRabbit.GlobalPosition = global;
                spawnBearOrRabbit.Call(Constants.SetSpawn, (int)spawn);
                AddChild(spawnBearOrRabbit);
                break;
            case Animals.Buffalo:
                var spawnBuffaloOrSquirrel = currentMap != MapType.Plains 
                    ? (KinematicBody2D)Buffalo.Instance()
                    : (KinematicBody2D)Squirrel.Instance();
                spawnBuffaloOrSquirrel.GlobalPosition = global;
                spawnBuffaloOrSquirrel.Call(Constants.SetSpawn, (int)spawn);
                AddChild(spawnBuffaloOrSquirrel);
                break;
            default:
                var defaultMoreSquirrel = (KinematicBody2D)Squirrel.Instance();
                defaultMoreSquirrel.GlobalPosition = global;
                defaultMoreSquirrel.Call(Constants.SetSpawn, (int)spawn);
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
        var tree = (PackedScene)ResourceLoader.Load(Constants.Tree);
        var bush = (PackedScene)ResourceLoader.Load(Constants.Bush);
        var redFlower = (PackedScene)ResourceLoader.Load(Constants.RedFlower);
        var purpleFlower = (PackedScene)ResourceLoader.Load(Constants.PurpleFlower);

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
        var cactus = (PackedScene)ResourceLoader.Load(Constants.Cactus);
        var bush = (PackedScene)ResourceLoader.Load(Constants.Bush);

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
        var bush = (PackedScene)ResourceLoader.Load(Constants.Bush);
        var redFlower = (PackedScene)ResourceLoader.Load(Constants.RedFlower);
        var purpleFlower = (PackedScene)ResourceLoader.Load(Constants.PurpleFlower);

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
        var pine = (PackedScene)ResourceLoader.Load(Constants.Pine);
        var bush = (PackedScene)ResourceLoader.Load(Constants.Bush);
        var redFlower = (PackedScene)ResourceLoader.Load(Constants.RedFlower);
        var rock = (PackedScene)ResourceLoader.Load(Constants.Rock);

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
    #endregion
}