using UnityEngine;

public class ArmsModel : MonoBehaviour
{
    public float amplitude = 0.05f; // высота покачивания
    public float frequency = 6f; // скорость покачивания

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        // получаем скорость игрока
        float speed = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).magnitude;

        if (speed > 0.1f)
        {
            float bob = Mathf.Sin(Time.time * frequency) * amplitude;
            transform.localPosition = startPos + new Vector3(0, bob, 0);
        }
        else
        {
            // возвращаем руки в исходное положение
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                startPos,
                Time.deltaTime * 5f
            );
        }
    }

}