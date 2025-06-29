using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class DayAndNightCycle : MonoBehaviour
{
    public float time;

    public float lengthOfDay;

    [SerializeField] private Gradient dayAndNightGradient;

    private Light2D light;

    void Start()
    {
        light = gameObject.GetComponent<Light2D>();
    }

    void Update()
    {
        time += Time.deltaTime;

        light.color = dayAndNightGradient.Evaluate((time % lengthOfDay) / lengthOfDay);
    }
}