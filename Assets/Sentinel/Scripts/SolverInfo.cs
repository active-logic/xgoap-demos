using UnityEngine;

namespace Activ.GOAP{
public class SolverInfo : MonoBehaviour{

    public SolverStats stats
    => GetComponent<SolverOwner>().stats;

}}
