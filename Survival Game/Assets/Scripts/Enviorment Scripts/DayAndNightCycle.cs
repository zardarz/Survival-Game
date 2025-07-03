using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class DayAndNightCycle : MonoBehaviour
{
    public float time;

    public float lengthOfDay;

    [SerializeField] private Gradient dayAndNightGradient;

    private Light2D globalLight;

    public static bool isNightTime;

    void Start()
    {
        globalLight = gameObject.GetComponent<Light2D>();
    }

    void Update()
    {
        time += Time.deltaTime;

        float zeroToOneTime = (time % lengthOfDay) / lengthOfDay;

        isNightTime = zeroToOneTime > 0.3f && zeroToOneTime < 0.7f;

        globalLight.color = dayAndNightGradient.Evaluate(zeroToOneTime);
    }
}