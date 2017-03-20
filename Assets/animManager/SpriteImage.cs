using System;
using System.Collections.Generic;
using UnityEngine;
//单独创建一张图片
public class SpriteImage : MonoBehaviour 
{
    //宽度
    public float width;
    //高度
    public float height;
    public void OnInit(string imageName)
    {
        imageName = imageName + ".png";
        SpriteFrame frame = SpriteFrameCache.getInstance().getSpriteFrame(imageName);
        width = frame.width;
        height = frame.height;

        Mesh mesh = new Mesh();
        mesh.vertices = frame.vertices;
        mesh.triangles = frame.triangles;
        mesh.uv = frame.uv;
        gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
        gameObject.GetComponent<MeshRenderer>().sharedMaterial = SpriteFrameCache.getInstance().getSpriteTexture(frame.textureName);
    }
}

