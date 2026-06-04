using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Platform : MonoBehaviour
{
    public GameObject nWall, eWall, wWall, sWall;
    private float WallHeight;
    private float wallhight;
    private float wallscaleX;
    private float wallscaleZ;

    // Start is called before the first frame update
    void Start()
    {
        WallHeight = 300f;
        wallhight = WallHeight * 0.5f;
        wallscaleX = transform.localScale.x;
        wallscaleZ = transform.localScale.z;

        FindWall(ref nWall, "Nwall");
        FindWall(ref eWall, "Ewall");
        FindWall(ref wWall, "Wwall");
        FindWall(ref sWall, "Swall");
        
        WallPosition();
        WallScaleSetting();
        WallSetting(ref nWall, wallscaleX, WallHeight, 1f);
        WallSetting(ref eWall, 1f, WallHeight, wallscaleZ);
        WallSetting(ref wWall, 1f, WallHeight, wallscaleZ);
        WallSetting(ref sWall, wallscaleX, WallHeight, 1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FindWall(ref GameObject wall, string wallname)
    {
        // 假设脚本挂载在父对象上
        Transform wallTransform = transform.Find(wallname);
        if (wallTransform != null)
        {
            wall = wallTransform.gameObject;
        }
        else
        {
            Debug.Log("无法访问电子墙");
        }
    }


    private void WallPosition()
    {
        Vector3 platformposition = transform.position;
        nWall.transform.position = platformposition + new Vector3(0, wallhight, wallscaleZ * 0.5f);
        sWall.transform.position = platformposition + new Vector3(0, wallhight, -wallscaleZ * 0.5f);
        eWall.transform.position = platformposition + new Vector3(wallscaleX * 0.5f, wallhight, 0);
        wWall.transform.position = platformposition + new Vector3(-wallscaleX * 0.5f, wallhight, 0);
        
    }

    private void WallScaleSetting()
    {
        nWall.transform.localScale = new Vector3(1f / wallscaleX, 1, 1f / wallscaleZ);
        eWall.transform.localScale = new Vector3(1f / wallscaleX, 1, 1f / wallscaleZ);
        wWall.transform.localScale = new Vector3(1f / wallscaleX, 1, 1f / wallscaleZ);
        sWall.transform.localScale = new Vector3(1f / wallscaleX, 1, 1f / wallscaleZ);
    }

    private void WallSetting(ref GameObject wall,float length,float height,float thickness) 
    { 
        BoxCollider boxcollider = wall.GetComponent<BoxCollider>();
        if (boxcollider != null)
        {
            // 设置碰撞箱的局部尺寸：X轴长度，Y轴高度，Z轴厚度
            boxcollider.size = new Vector3(length, height, thickness);
        }

    }

}