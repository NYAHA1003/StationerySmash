using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private enum Dir
    {
        Right,
        Left,
        Up,
        Down,
    }

    private string val;
    private int maxLine = 10;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < maxLine; i++)
        {
            for(int j = 0; j < maxLine; j++)
            {
                if(j > i && j < maxLine / 2)
                {
                    val += "*";
                }
                else
                {
                    val += "  ";
                }
            }
            val += '\n';
        }
        //for(int i = 0; i < maxLine; i++)
        //{
        //    for (int j = i; j > 0; j--)
        //    {
        //        val += "  ";
        //    }
        //    for (int j = maxLine - i; j > 0; j--)
        //    {
        //        val += "*";
        //    }
        //    for (int j = maxLine - i; j > 0; j--)
        //    {
        //        val += "  ";
        //    }
        //    for (int j = i; j >= 0; j--)
        //    {
        //        val += "*";
        //    }
        //    val += '\n';
        //}
        //for (int i = 0; i < maxLine; i++)
        //{
        //    for (int j = maxLine - i; j > 0; j--)
        //    {
        //        val += "*";
        //    }
        //    for (int j = i; j > 0; j--)
        //    {
        //        val += "  ";
        //    }
        //    for (int j = i; j > 0; j--)
        //    {
        //        val += "*";
        //    }
        //    for (int j = maxLine - i; j >= 0; j--)
        //    {
        //        val += "  ";
        //    }
        //    val += '\n';
        //}

        Debug.Log(val);
    }

}
