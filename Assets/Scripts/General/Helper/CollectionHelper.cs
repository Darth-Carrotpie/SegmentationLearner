using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class CollectionHelper
{
    public static void RemoveByValue<TKey, TValue>(Dictionary<TKey, TValue> _dic, TValue someValue)
    {
        List<TKey> itemsToRemove = (from pair in _dic where pair.Value.Equals(someValue) select pair.Key).ToList();

        foreach (TKey item in itemsToRemove)
        {
            //Debug.Log(item.ToString());
            _dic.Remove(item);
        }
    }

    public static string PrintDict<TKey, TValue>(Dictionary<TKey, TValue> _dic){
        string output = "[";
        foreach (TKey item in _dic.Keys)
        {
            output+=item;
            output+=":";
            output+=_dic[item];
            output+=";";
            //Debug.Log(item.ToString());
        }
        output+="]";
        return output;
    }
    public static string PrintGoList<T>(List<T> _list){
        string output = "GO-Names=[";
        foreach (T item in _list)
        {
            MonoBehaviour mono = item as MonoBehaviour;
            GameObject go = item as GameObject;
            if(mono)
                if (mono.gameObject){
                    output+=mono.gameObject.name;
                    output+=";";
                }
            if(go)
                if (go){
                    output+=go.name;
                    output+=";";
                }
        }
        output+="]";
        return output;
    }
}
