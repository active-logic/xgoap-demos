using UnityEngine;
using NUnit.Framework;
using Activ.GOAP;

public class TestSentinel : TestBase{

    GroundModel ground;

    [SetUp] public void Setup(){
        ground = new GroundModel();
        ground.Clear();
    }

    [Test] public void TestOneStepShoot(){
        var x =
            new SentinelModel((Transform)null,
                              new SentinelModel.Target(3, 0),
                              ground);
        o( x.target != null );
        var p = new Solver<SentinelModel>();
        var g = Goal();
        var s = p.Next(x, in g);
        var path = s.Path();
        o( path.Length == 2 );
        foreach(var n in path) print(n.ToString());
    }

    [Test] public void TestMoveAndShoot(){
        var x =
            new SentinelModel((Transform)null,
                              new SentinelModel.Target(4, 0),
                              ground);
        o( x.target != null );
        var p = new Solver<SentinelModel>();
        var g = Goal();
        var s = p.Next(x, in g);
        var path = s.Path();
        o( path.Length, 3 );
        foreach(var n in path) print(n.ToString());
    }

    [Test] public void TestMove4x4AndShoot(){
        var x =
            new SentinelModel((Transform)null,
                              new SentinelModel.Target(4, 4),
                              ground);
        o( x.target != null );
        var p = new Solver<SentinelModel>();
        p.maxNodes = 2500;
        var g = Goal();
        var s = p.Next(x, in g);
        var path = s.Path();
        o( path.Length, 6 );
        print($"Iter: {p.I}, max fringe: {p.fxMaxNodes}");
        print(p.state.ToString());
        foreach(var n in path) print(n.ToString());
    }

    [Test] public void Test_H_Move4x4AndShoot(){
        var x =
            new SentinelModel((Transform)null,
                              new SentinelModel.Target(4, 4),
                              ground);
        o( x.target != null );
        var p = new Solver<SentinelModel>();
        p.maxNodes = 2500;
        var g = HGoal();
        var s = p.Next(x, in g);
        var path = s.Path();
        o( path.Length, 6 );
        print($"Iter: {p.I}, max fringe: {p.fxMaxNodes}");
        print(p.state.ToString());
        foreach(var n in path) print(n.ToString());
    }

    [Test] public void TestMove5x3AndShoot(){
        var x =
            new SentinelModel((Transform)null,
                              new SentinelModel.Target(5, 3),
                              ground);
        o( x.target != null );
        var p = new Solver<SentinelModel>();
        p.maxIter  = 10000;
        p.maxNodes = 100000;
        var g = Goal();
        var s = p.Next(x, in g);
        print($"Iter: {p.I}, max fringe: {p.fxMaxNodes}");
        var path = s.Path();
        o( path.Length, 6 );
        print(p.state.ToString());
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
