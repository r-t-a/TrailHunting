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
    protected NodePath TileMapPath;
    [Export]
    protected NodePath SpawnTimerPath;
    [Export]
    protected NodePath GameTimerPath;
    [Export]
    protected NodePath EndButtonPath;
    [Export]
    protected NodePath DisplayTimePath;
    [Export]
    protected NodePath SpawnNodePath;

    #endregion

    #region Properties
    public int SmallGameCounter;
    public int MediumGameCounter;
    public int MedLargeGameCounter;
    public int LargeGameCounter;
    
    private TileMap groundTileMap;
    private Timer spawnTimer;
    private Timer gameTimer;
    private Button endButton;
    private Label displayTimer;

    private PackedScene player;
    private PackedScene deer;
    private PackedScene rabbit;
    private PackedScene squirrel;
    private PackedScene bear;
    private PackedScene buck;
    private PackedScene buffalo;

    private List<Area2D> spawns = new List<Area2D>();
    private MapType currentMap;
    #endregion

    #region Overrides
    public override void _Ready()
    {
        groundTileMap = GetNodeOrNull<TileMap>(TileMapPath);
        spawnTimer = GetNodeOrNull<Timer>(SpawnTimerPath);
        gameTimer = GetNodeOrNull<Timer>(GameTimerPath);
        endButton = GetNodeOrNull<Button>(EndButtonPath);
        displayTimer = GetNodeOrNull<Label>(DisplayTimePath);

        player = (PackedScene)ResourceLoader.Load(Constants.HuntingPlayer);
        deer = (PackedScene)ResourceLoader.Load(Constants.TopDownDeer);
        rabbit = (PackedScene)ResourceLoader.Load(Constants.TopDownRabbit);
        squirrel = (PackedScene)ResourceLoader.Load(Constants.TopDownSquirrel);
        buffalo = (PackedScene)ResourceLoader.Load(Constants.TopDownBuffalo);
        bear = (PackedScene)ResourceLoader.Load(Constants.TopDownBear);
        buck = (PackedScene)ResourceLoader.Load(Constants.TopDownBuck);

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
        spawns.Add(GetNodeOrNull<Area2D>($"{SpawnNodePath}/Spawn11"));
        spawns.Add(GetNodeOrNull<Area2D>($"{SpawnNodePath}/Spawn12"));

        if (GameManager.PlayerManager.IsEndless)
        {
            endButton.Visible = true;
            displayTimer.Visible = false;
            gameTimer.Stop();
        }
        else
        {
            endButton.Visible = false;
            displayTimer.Visible = true;
            gameTimer.Start();
        }

        BuildLevel();
    }

    public override void _Process(float delta)
    {
        if (displayTimer.Visible)
        {
            displayTimer.Text = Mathf.FloorToInt(gameTimer.TimeLeft).ToString();
        }
    }
    #endregion

    #region Events
    private void _on_Squirrel_SmallGameDead()
    {
        GameManager.PlayerManager.SquirrelTotal += 1;
        SmallGameCounter += 1;
    }

    private void _on_Rabbit_SmallGameDead()
    {
        GameManager.PlayerManager.RabbitTotal += 1;
        SmallGameCounter += 1;
    }

    private void _on_Deer_MediumGameDead()
    {
        GameManager.PlayerManager.DoeTotal += 1;
        MediumGameCounter += 1;
    }

    private void _on_Buck_MediumGameDead()
    {
        GameManager.PlayerManager.BuckTotal += 1;
        MediumGameCounter += 1;
    }

    private void _on_Bear_MedLargeGameDead()
    {
        GameManager.PlayerManager.BearTotal += 1;
        MedLargeGameCounter += 1;
    }

    private void _on_Buffalo_LargeGameDead()
    {
        GameManager.PlayerManager.BuffaloTotal += 1;
        LargeGameCounter += 1;
    }

    private void _on_SpawnTimer_timeout()
    {
        var currentSpawn = GetTree().GetNodesInGroup(Constants.Animals);
        if (currentSpawn.Count >= Constants.MaxAnimalSpawn)
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

    private void _on_GameTimer_timeout()
    {
        spawnTimer.Stop();
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
        spawnTimer.Stop();
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
        spawnPoint = spawnPoint ?? GetNodeOrNull<Area2D>($"{SpawnNodePath}/Spawn1"); // If something goes wrong default to Spawn1
        var user = (KinematicBody2D)player.Instance();
        user.Position = spawnPoint.Position;
        AddChild(user);
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
                var spawnSquirrel = (KinematicBody2D)squirrel.Instance();
                spawnSquirrel.GlobalPosition = global;
                spawnSquirrel.Call(Constants.SetSpawn, (int)spawn);
                AddChild(spawnSquirrel);
                break;
            case Animals.Rabbit:
                var spawnRabbit = (KinematicBody2D)rabbit.Instance();
                spawnRabbit.GlobalPosition = global;
                spawnRabbit.Call(Constants.SetSpawn, (int)spawn);
                AddChild(spawnRabbit);
                break;
            case Animals.Doe:
                var spawnDoe = (KinematicBody2D)deer.Instance();
                spawnDoe.GlobalPosition = global;
                spawnDoe.Call(Constants.SetSpawn, (int)spawn);
                AddChild(spawnDoe);
                break;
            case Animals.Buck:
                var spawnBuck = (KinematicBody2D)buck.Instance();
                spawnBuck.GlobalPosition = global;
                spawnBuck.Call(Constants.SetSpawn, (int)spawn);
                AddChild(spawnBuck);
                break;
            case Animals.Bear:
                var spawnBearOrRabbit = currentMap != MapType.Mountains || currentMap != MapType.Woods
                    ? (KinematicBody2D)rabbit.Instance()
                    : (KinematicBody2D)bear.Instance();
                spawnBearOrRabbit.GlobalPosition = global;
                spawnBearOrRabbit.Call(Constants.SetSpawn, (int)spawn);
                AddChild(spawnBearOrRabbit);
                break;
            case Animals.Buffalo:
                var spawnBuffaloOrSquirrel = currentMap != MapType.Plains 
                    ? (KinematicBody2D)buffalo.Instance()
                    : (KinematicBody2D)squirrel.Instance();
                spawnBuffaloOrSquirrel.GlobalPosition = global;
                spawnBuffaloOrSquirrel.Call(Constants.SetSpawn, (int)spawn);
                AddChild(spawnBuffaloOrSquirrel);
                break;
            default:
                var defaultMoreSquirrel = (KinematicBody2D)squirrel.Instance();
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