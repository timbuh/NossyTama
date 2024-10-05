using UnityEngine;

public class SpriteFaceCam : MonoBehaviour
{

    public GameObject sprite;

    void Update()
    {
        sprite.transform.rotation = Camera.main.transform.rotation;
    }
}
