using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptoHelper
{
    public static string md5 (string str)
    {
        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding ();
        byte[] bytes = encoding.GetBytes (str);      
        var sha = new System.Security.Cryptography.MD5CryptoServiceProvider();
        return System.BitConverter.ToString (sha.ComputeHash (bytes));
    }
}
