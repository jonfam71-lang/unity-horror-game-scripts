using UnityEngine;

public class Test : MonoBehaviour
{

    public GameObject targat;

    void Start()
    {
        
    }


    void Update()
    {
        
        transform.Rotate(0, 45f * Time.deltaTime, 0);

    }
}
