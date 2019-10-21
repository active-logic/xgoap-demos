using UnityEngine;
using NUnit.Framework;

public class TestGroundModel : TestBase{

    /*
    [Test] public void TestEq1(){
        var a = new GroundModel();
        var b = new GroundModel();
        o(a, b);
    }*/

    [Test] public void TestEq2(){
        var a = new GroundModel();
        var b = new GroundModel();
        o(a != b);
    }

}
