using UnityEngine;
using NUnit.Framework;

public class TestVector2i : TestBase{

    [Test] public void TestNeighbor(){
        Vector2i a = (0, 0), b = (0, 1);
        o(a.IsNeighbor(b));
        o(b.IsNeighbor(a));
    }

    [Test] public void TestNeighbor1(){
        Vector2i a = (0, 0), b = (1, 1);
        o( a.IsNeighbor(b) );
        o( b.IsNeighbor(a) );
    }

    [Test] public void TestNeighbor2(){
        Vector2i a = (2, 3), b = (4, 3);
        o( !a.IsNeighbor(b) );
        o( !b.IsNeighbor(a) );
    }
}
