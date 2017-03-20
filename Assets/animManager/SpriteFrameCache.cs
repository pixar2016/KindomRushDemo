using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class SpriteFrameCache
{
    private static SpriteFrameCache instance = null;
    //帧名-帧信息
    private Dictionary<string, SpriteFrame> _spriteFrames;
    //帧引用图片名-帧引用图片
	private Dictionary<string, Material> _spriteTexture;
    private SpriteFrameCache()
    {
        _spriteFrames = new Dictionary<string, SpriteFrame>();
        _spriteTexture = new Dictionary<string, Material>();
    }

    public static SpriteFrameCache getInstance()
    {
        if (instance == null)
        {
            instance = new SpriteFrameCache();
        }
        return instance;
    }
    //根据图片名加载其中包含的帧动画信息
    public void addSpriteFrameFromFile(string fileName)
    {
        string infos = FileUtils.LoadFile(Application.dataPath, fileName);
        JsonData AnimJson = JsonMapper.ToObject(infos);
        IDictionary dict = AnimJson["frames"] as IDictionary;
        foreach (string key in dict.Keys)
        {
            SpriteFrame frame = new SpriteFrame();
            frame.CreateMesh(AnimJson["frames"][key], AnimJson["meta"]);
            _spriteFrames.Add(key, frame);
        }

        string pictName = (string)AnimJson["meta"]["image"];
        if (!_spriteTexture.ContainsKey(pictName))
        {
            int index = pictName.LastIndexOf('.');
            string temp = pictName.Substring(0, index);
			Material pict = (Material)Resources.Load("Material/" + temp);//(Texture)Resources.Load("Material/test1"); 
            _spriteTexture.Add(pictName, pict);
        }
    }

    //取得预加载的材质
	public Material getSpriteTexture(string textureName)
    {
        if (_spriteTexture.ContainsKey(textureName))
        {
            return _spriteTexture[textureName];
        }
        return null;
    }

    public void addSpriteFrame(SpriteFrame frame, string frameName)
    {
        _spriteFrames.Add(frameName, frame);
    }
    //根据帧名字得到该帧动画信息
    public SpriteFrame getSpriteFrame(string framePictName)
    {
        if (_spriteFrames.ContainsKey(framePictName))
        {
            return _spriteFrames[framePictName];
        }
        return null;
    }
}

