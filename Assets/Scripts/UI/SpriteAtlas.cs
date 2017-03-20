using UnityEngine;
using System.Collections.Generic;
    
public class SpriteAtlas : MonoBehaviour
{
    public Sprite[] sprites;
    private Dictionary<string, Sprite> dict;

    public Sprite GetSprite(string name)
    {
        Sprite ret = null;
        if (!string.IsNullOrEmpty(name))
        {
            if (dict == null)
            {
                dict = new Dictionary<string, Sprite>();
                for (int i = 0, max = sprites.Length; i < max; ++i)
                {
                    if (sprites[i])
                    {
                        dict.Add(sprites[i].name, sprites[i]);
                    }
                }
            }
            if (!dict.TryGetValue(name, out ret))
            {
                Debug.Log(string.Format("load error {0} ", name));
            }
        }
        else
        {
            Debug.Log("name IsNullOrEmpty");
        }
        return ret;
    }
}

