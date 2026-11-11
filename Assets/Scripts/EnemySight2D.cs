using UnityEngine;

public static class EnemySight2D
{
    public static bool CanSee(Vector2 eye, Vector2 facing, Vector2 target,
                              float range, float fov, LayerMask obstacleMask)
    {
        Vector2 toTarget = target - eye;
        float dist = toTarget.magnitude;

        if (dist > range) return false;
        if (dist < 0.01f) return true;
        if (Vector2.Angle(facing, toTarget) > fov * 0.5f)
            return false;

        return Physics2D.Raycast(eye, toTarget / dist, dist, obstacleMask).collider == null;
    }
}