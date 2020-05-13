
using UnityEngine;
using UnityEngine.Networking;

public class LifeTimer : MonoBehaviour {

    public float life = 2f;

    private float _timer = 0f;

    void Start()
    {

    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > life) {
            Destroy(this.gameObject);
        }
    }
}
