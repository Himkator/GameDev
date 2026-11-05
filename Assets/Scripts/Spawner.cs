using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Hazard[] prefabs;
    public float spawnInterval = 0.5f;
    public float warningTime = 1f;
    public float lineWidth = 0.3f;

    Camera cam;

    void Start()
    {
        cam = Camera.main;
        InvokeRepeating(nameof(Spawn), 1f, spawnInterval);
    }

    void Spawn()
    {
        StartCoroutine(SpawnWithWarning());
    }

    IEnumerator SpawnWithWarning()
    {
        Hazard pick = prefabs[Random.Range(0, prefabs.Length)];
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;
        Vector3 c = cam.transform.position;

        float x = Random.Range(c.x - halfWidth, c.x + halfWidth);
        float y = c.y + halfHeight + 1f;
        if(pick.warningPrefab != null)
        {
            GameObject warning = Instantiate(pick.warningPrefab, new Vector3(x, c.y, 0f), Quaternion.identity);
            warning.transform.localScale = new Vector3(lineWidth, halfHeight * 2f, 1f);

            yield return new WaitForSeconds(warningTime);

            Destroy(warning);
        }
        Instantiate(pick, new Vector3(x, y, 0f), Quaternion.Euler(0, 0, 180));
    }
}