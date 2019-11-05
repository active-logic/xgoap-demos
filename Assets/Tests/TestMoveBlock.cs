using UnityEngine;
using NUnit.Framework;
using Activ.GOAP;

public class TestPerf : TestBase{

    GroundModel ground;

    [SetUp] public void Setup() => ground = new GroundModel();

    [Test] public void Test_x_100(){
        var z = 0;
        for(int i=0;i<100;i++){
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
            p.tolerance = 1.75f;
            var g = HGoal();
            var s = p.Next(x, in g);
            z += p.iteration;
            o( s.Path().Length, 33 );
        }
        print($"Iter x: {z/100}"); // 1166 x 790
    }

    Goal<SentinelModel> Goal() => ( m => m.target == null, null );

    Goal<SentinelModel> HGoal()
    => ( m => m.target == null,
         m => m.target?.Dist(m.x, m.y) ?? 0 );

}
