using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * 12f;
        Destroy(gameObject, 3f);  
    }  

}
