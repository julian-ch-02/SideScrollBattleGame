namespace SideScrollGame;

public partial class GameMaster : Node
{
    public static Node2D PlayerBase  { get; set; }
    public static Node2D EnemyBase   { get; set; }
    public static Node   PlayerUnits { get; set; }
    public static Node   EnemyUnits  { get; set; }

    private static CancellationTokenSource SpawnUnitsCTS { get; } = new();

    public static void DestroyAllUnits(Team team)
    {
        SpawnUnitsCTS.Cancel();

        if (team == Team.Left)
            PlayerUnits.QueueFreeChildren();
        else
            EnemyUnits.QueueFreeChildren();
    }

    public override async void _Ready()
    {
        PlayerUnits = new Node2D { Name = "Player Units" };
        EnemyUnits  = new Node2D { Name = "Enemy Units" };
        PlayerBase  = GetNode<Node2D>("Player Base");
        EnemyBase   = GetNode<Node2D>("Enemy Base");

        AddChild(PlayerUnits);
        AddChild(EnemyUnits);

        await Task.Factory.StartNew(() => SpawnUnits(), SpawnUnitsCTS.Token);
    }

    private async Task SpawnUnits()
    {
        // player units
        for (int i = 0; i < 1; i++)
        {
            await Task.Delay(1, SpawnUnitsCTS.Token);
            Units.Create(Unit.OrangeBall, Team.Left);
        }

        // enemy units
        for (int i = 0; i < 30; i++)
        {
            await Task.Delay(100, SpawnUnitsCTS.Token);
            Units.Create(Unit.Skeleton, Team.Right);
        }
    }
}
