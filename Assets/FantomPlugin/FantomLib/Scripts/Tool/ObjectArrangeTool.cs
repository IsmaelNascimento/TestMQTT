using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FantomLib
{
    /// <summary>
    /// オブジェクトを等間隔に並べる（主にUI用）
    /// 2017/11/22 Fantom (Unity 5.6.3p1)
    /// objects[0] を基準に step ごとに配置する。
    /// </summary>
    [ExecuteInEditMode]
    public class ObjectArrangeTool : MonoBehaviour
    {
#if UNITY_EDITOR
        [Serializable]
        public enum Axis {
            X, Y, Z
        }

        public Axis axis = Axis.Y;      //並べる軸
        public float step = -100;       //並べる間隔
        public GameObject[] objects;    //並べるオブジェクト

        //実行中フラグ
        public bool executing {
            get; private set;
        }


        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        //private void Update()
        //{

        //}

        //オブジェクトを等間隔に並べる
        public void Arrange()
        {
            if (objects.Length < 2)
                return;

            executing = true;

            float start;
            if (axis == Axis.Y)
                start = objects[0].transform.localPosition.y;
            else if (axis == Axis.X)
                start = objects[0].transform.localPosition.x;
            else
                start = objects[0].transform.localPosition.z;

            for (int i = 1; i < objects.Length; i++)
            {
                Vector3 pos = objects[i].transform.localPosition;
                if (axis == Axis.Y)
                    pos.y = start + step * i;
                else if (axis == Axis.X)
                    pos.x = start + step * i;
                else
                    pos.z = start + step * i;

                objects[i].transform.localPosition = pos;
            }

            executing = false;
        }


        public void CopyElements(int from, int to)
        {
            if (from == to || 
                from < 0 || objects.Length <= from ||
                to < 0 || objects.Length <= to)
                return;

            executing = true;

            GameObject[] temp = (GameObject[])objects.Clone();
            for (int i = from, j = to; i < temp.Length && j < objects.Length; i++, j++)
            {
                objects[j] = temp[i];
            }

            executing = false;
        }

#endif
    }
}
