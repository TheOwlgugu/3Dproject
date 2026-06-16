using UnityEngine;

public class VisibleCounter : MonoBehaviour
{
    public Camera cam;
    public GameObject[] allCharacters;

    [ContextMenu("PrintVisibleCount")]
    
    public void PrintCount()
    {
        int count = CountVisible();
    }

    public int CountVisible()
    {
        if (cam == null) cam = Camera.main;
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        int cnt = 0;
        foreach (var ch in allCharacters)
        {
            Renderer rend = ch.GetComponent<Renderer>();
            if (rend != null && GeometryUtility.TestPlanesAABB(planes, rend.bounds))
                cnt++;
        }
        return cnt;
    }
}