using UnityEngine;
using System;
using System.Collections;
public static class Common  
{
    public static IEnumerator WaitAndAction(Action action, float seconds) //等待的时间,单位秒  
    {
        if(seconds>0)
            yield return new WaitForSeconds(seconds);
        action();
    }
}
