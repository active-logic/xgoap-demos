using UnityEditor;
using static UnityEditor.EditorGUILayout;

namespace Activ.GOAP{
[CustomEditor(typeof(SolverInfo))]
public class SolverEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        SolverInfo self = (SolverInfo)target;
        var stats = self.stats;
        LabelField($"Status: {stats?.status}");
        LabelField($"Iterations: {stats?.iteration}");
        LabelField($"Fringe: {stats?.peak}");
    }
}}
