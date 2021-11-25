using Nameless.DataMono;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : Singleton<PathManager>
{
    public Dictionary<PawnAvatar, bool> pawnPath = new Dictionary<PawnAvatar, bool>();
    // Start is called before the first frame update
    public void InitPath()
    {
        this.pawnPath = new Dictionary<PawnAvatar, bool>();
    }
    public void AddPath(PawnAvatar pawnAvatar)
    {
        if (!this.pawnPath.ContainsKey(pawnAvatar))
        {
            this.pawnPath.Add(pawnAvatar, true);
        }
    }
    public void ShowPath(PawnAvatar pawnAvatar)
    {
        if (!this.pawnPath.ContainsKey(pawnAvatar))
        {
            this.pawnPath.Add(pawnAvatar, true);
        }
        foreach(var child in this.pawnPath)
        {
            child.Key.ShowPath(false);
        }
        pawnAvatar.ShowPath(true);
    }
    public void ResetPathColor()
    {
        foreach (var child in this.pawnPath)
        {
            child.Key.ShowPath(false);
        }
    }
    //void AddNewPath(PawnAvatar pawnAvatar)
    //{
    //    if (!this.pawnPath.ContainsKey(pawnAvatar))
    //    {
    //        this.pawnPath.Add(pawnAvatar, true);
    //    }
    //}
}
