using UnityEngine;
using NUnit.Framework;
using Activ.GOAP;

public class TestPerf : TestBase{

    GroundModel ground;

    [SetUp] public void Setup() => ground = new GroundModel();

    [Test] public void Test_x_10(){
        var z = 0;
        for(int i=0;i<10;i++){
        // -4, 2  Puts the sentinel top-right
        // -3, -2 Is on left side, faster
        var x =
            new SentinelModel((Transform)null,  // -4, 2
                              new SentinelModel.Target(-4, 2),
                              ground);
        o( x.target != null );
        var p = new Solver<SentinelModel>();
        p.maxIter = 2000;
        p.maxNodes = 512;
        var g = HGoal();
        var s = p.Next(x, in g);
        //print(p.state.ToString());
        z += p.I;
        //rint($"Iter: {p.I}, max fringe: {p.fxMaxNodes}");
        //var path = s.Path();
        o( s.Path().Length, 33 );
        //foreach(var n in path) print(n.ToString());
        }
        print($"Iter x: {z/10}"); // 1166 x 790
        // speed increase is 25%; saved iterations 32%
    }

    Goal<SentinelModel> Goal()
    => new Goal<SentinelModel>( m => m.target == null );

    Goal<SentinelModel> HGoal()
    => new Goal<SentinelModel>(
        m => m.target == null,
        m => m.target?.Dist(m.x, m.y) ?? 0
    );

}
