using System;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

//记录该帧图片在图集中的信息
public class SpriteFrame
{
    public Vector3 []vertices;
    public int []triangles;
    public Vector2 []uv;
    public string textureName;
    public float width;
    public float height;
    public SpriteFrame()
    {
        
    }

    public void CreateMesh(JsonData frameData, JsonData meta)
    {
        Vector3 v1, v2, v3, v4;
        Vector2 s1, s2, s3, s4;
        float minX, maxX, minY, maxY;
        float zoomSize = 1;
        float imageWidth = (int)meta["size"]["w"];
        float imageHeight = (int)meta["size"]["h"];
        textureName = (string)meta["image"];
        bool rotated = (bool)frameData["rotated"];
        bool trimmed = (bool)frameData["trimmed"];
        float frameX = (int)frameData["frame"]["x"];
        float frameY = (int)frameData["frame"]["y"];
        float frameW = (int)frameData["frame"]["w"];
        float frameH = (int)frameData["frame"]["h"];
        float spriteSourceSizeX = (int)frameData["spriteSourceSize"]["x"];
        float spriteSourceSizeY = (int)frameData["spriteSourceSize"]["y"];
        float spriteSourceSizeW = (int)frameData["spriteSourceSize"]["w"];
        float spriteSourceSizeH = (int)frameData["spriteSourceSize"]["h"];
        float sourceSizeW = (int)frameData["sourceSize"]["w"];
        float sourceSizeH = (int)frameData["sourceSize"]["h"];
        this.width = sourceSizeW;
        this.height = sourceSizeH;

        if (rotated == false)
        {
            minX = -sourceSizeW / 2 + spriteSourceSizeX;
            maxX = minX + spriteSourceSizeW;
            //动画位置在中心
            maxY = sourceSizeH / 2 - spriteSourceSizeY;
            minY = maxY - spriteSourceSizeH;
            //Debug.Log("minX = "+minX);
            //Debug.Log("maxX = " + maxX);
            //Debug.Log("maxY = " + maxY);
            //Debug.Log("minY = " + minY);
            minX /= zoomSize;
            maxX /= zoomSize;
            maxY /= zoomSize;
            minY /= zoomSize;

            v1 = new Vector3(minX, minY, 0);
            v2 = new Vector3(minX, maxY, 0);
            v3 = new Vector3(maxX, maxY, 0);
            v4 = new Vector3(maxX, minY, 0);

            minX = frameX / imageWidth;
            maxX = (frameX + frameW) / imageWidth;
            minY = (imageHeight - frameY - frameH) / imageHeight;
            maxY = (imageHeight - frameY) / imageHeight;

            s1 = new Vector2(minX, minY);
            s2 = new Vector2(minX, maxY);
            s3 = new Vector2(maxX, maxY);
            s4 = new Vector2(maxX, minY);
        }
        else
        {
            minX = -sourceSizeW / 2 + spriteSourceSizeX;
            maxX = minX + spriteSourceSizeW;
            //动画位置在中心
            maxY = sourceSizeH / 2 - spriteSourceSizeY;
            minY = maxY - spriteSourceSizeH;
            minX /= zoomSize;
            maxX /= zoomSize;
            maxY /= zoomSize;
            minY /= zoomSize;

            v1 = new Vector3(minX, minY, 0);
            v2 = new Vector3(minX, maxY, 0);
            v3 = new Vector3(maxX, maxY, 0);
            v4 = new Vector3(maxX, minY, 0);

            minX = frameX / imageWidth;
            maxX = (frameX + frameH) / imageWidth;
            minY = (imageHeight - frameY - frameW) / imageHeight;
            maxY = (imageHeight - frameY) / imageHeight;

            s1 = new Vector2(minX, maxY);
            s2 = new Vector2(maxX, maxY);
            s3 = new Vector2(maxX, minY);
            s4 = new Vector2(minX, minY);
        }

        vertices = new Vector3[] { v1, v2, v3, v4 };
        triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        uv = new Vector2[] { s1, s2, s3, s4 };
    }
}
