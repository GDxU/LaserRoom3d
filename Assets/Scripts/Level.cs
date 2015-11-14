using UnityEngine;
using System.Collections;

public static class Gameplay
{
    public static double startTime;
    //当前重力方向
    public static Vector3 playerGravityDir;
    //保存上一次非临时重力场
    public static Vector3 saveGravityDir;
    //当前是否进入临时重力场
    public static bool isTemperaryGravity;
    //临时重力场进入时间
    public static float TemperaryGravityEntryTime;
}
