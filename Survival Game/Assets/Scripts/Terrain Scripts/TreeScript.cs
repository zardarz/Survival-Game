using UnityEngine;

public class TreeScript : MonoBehaviour
{
    public float treeStrength;

    public void TryDestoryTree(float toolStrength) {
        if(treeStrength <= toolStrength) {
            Destroy(gameObject);
        }

    }
}