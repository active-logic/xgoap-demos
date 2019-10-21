using UnityEngine;
using NUnit.Framework;
using Activ.GOAP;

public class TestMoveBlock : TestBase{

    GroundModel ground;

    [SetUp] public void Setup() => ground = new GroundModel();

    [Test] public void MoveBlock(){
        var x =
            new SentinelModel((Transform)null,
                              new SentinelModel.Target(3, 0),
                              ground);
        var P = new Vector2(2, -2);
        var B = new Vector2(2, -3);
        x.position = P;
        x.direction = new Vector2i(0, -1);
        o( ground.IsObstructed(B));
        x.Pull();
        o( !ground.IsObstructed(B) );
    }

    [Test] public void Test(){
        var x =
            new SentinelModel((Transform)null,
                              new SentinelModel.Target(-3, -2),
                              ground);
        o( x.target != null );
        var p = new Solver<SentinelModel>();
        p.maxIter = 512;
        p.maxNodes = 512;
        var g = HGoal();
        var s = p.Next(x, in g);
        print(p.state.ToString());
        print($"Iter: {p.I}, max fringe: {p.fxMaxNodes}");
        var path = s.Path();
        o( path.Length, 25 );
        foreach(var n in path) print(n.ToString());
    }

    Goal<SentinelModel> Goal()
    => new Goal<SentinelModel>( m => m.target == null );

    Goal<SentinelModel> HGoal()
    => new Goal<SentinelModel>(
        m => m.target == null,
        m => m.target?.Dist(m.x, m.y) ?? 0
    );

}
