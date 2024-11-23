using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WFCNode", menuName = "WFC/Node")]
[System.Serializable]
public class WFCNode : ScriptableObject
{
    public string Name;
    public GameObject Prefab;
    public WFCNodeConnection Top;
    public WFCNodeConnection Bottom;
    public WFCNodeConnection Left;
    public WFCNodeConnection Right;

}

[System.Serializable]
public class WFCNodeConnection
{
    public List<WFCNode> CompatibleNodes = new List<WFCNode>();
}
