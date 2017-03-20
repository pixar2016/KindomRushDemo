using UnityEngine;
using System.Collections;
using Hero;
using EventDispatcherSpace;

public class CharacterView {

    public CharacterInfo charInfo;
    public ILoadAsset charAsset;
    public GameObject charObj;
    public Animate charAnim;

    public CharacterView(CharacterInfo charInfo)
    {
        this.charInfo = charInfo;
        this.charInfo.eventDispatcher.Register("DoAction", DoAction);
    }

    public void LoadModel()
    {
        charAsset = GameLoader.Instance.LoadAssetSync("Resources/Prefabs/fly.prefab");
        charObj = charAsset.GameObjectAsset;
        if (charObj.GetComponent<Animate>() != null)
        {
            charAnim = charObj.GetComponent<Animate>();
        }
        else
        {
            charAnim = charObj.AddComponent<Animate>();
        }
        charAnim.OnInit(AnimationCache.getInstance().getAnimation(charInfo.charName));
        charAnim.startAnimation("idle");
    }

    public void DoAction(object[] data)
    {
        charAnim.startAnimation(data[0].ToString());
    }

    public void Release()
    {
        GameLoader.Instance.UnLoadGameObject(charAsset);
    }
    public void Update()
    {
        charObj.transform.position = charInfo.GetPosition();
        charObj.transform.eulerAngles = charInfo.GetRotation();
    }
}
