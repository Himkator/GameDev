using UnityEngine;

public class Heart : Hazard
{
    protected override void OnHitPlayer(Health p) { p.Heal(1); }
}
