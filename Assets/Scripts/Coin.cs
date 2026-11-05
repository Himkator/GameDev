using UnityEngine;

public class Coin : Hazard
{
    protected override void OnHitPlayer(Health p) { p.Heal(0); }
}
