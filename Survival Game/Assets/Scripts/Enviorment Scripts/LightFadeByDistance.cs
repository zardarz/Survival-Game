using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFadeByDistance : MonoBehaviour
{
    GameObject globalLight;

    Transform playerTransform;

    [SerializeField] private float radiusOfInfluence;
    [SerializeField] private AnimationCurve darknessCurve;

    void Awake()
    {
        globalLight = GameObject.Find("Global Light 2D");
    }

    void Update()
    {
        playerTransform = PlayerMovement.GetPlayerTransform();
        float disFromPlayer = Vector2.Distance(playerTransform.position, transform.position) / radiusOfInfluence;

        if(darknessCurve.Evaluate(disFromPlayer) == 1) return;
        globalLight.GetComponent<Light2D>().intensity = darknessCurve.Evaluate(disFromPlayer);
    }
}