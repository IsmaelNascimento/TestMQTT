using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FantomLib
{
    /// <summary>
    /// オブジェクトを等間隔に並べる（主にUI用）
    /// 2017/11/22 Fantom (Unity 5.6.3p1)
    /// </summary>
    [CustomEditor(typeof(ObjectArrangeTool))]
    public class ObjectArrangeToolEditor : Editor
    {
        int fromIndex;
        int toIndex;

        public override void OnInspectorGUI()
        {
            //元のInspector部分を表示
            base.OnInspectorGUI();

            if (!Application.isPlaying)
            {
                var tool = target as ObjectArrangeTool;
                if (tool.executing)
                    return;

                GUILayout.Space(15);

                EditorGUILayout.LabelField("Copy Index");
                EditorGUI.indentLevel++;
                fromIndex = EditorGUILayout.IntField("From", fromIndex);
                toIndex = EditorGUILayout.IntField("To", toIndex);
                EditorGUI.indentLevel--;
                if (GUILayout.Button("Copy Elements"))
                {
                    tool.CopyElements(fromIndex, toIndex);
                }

                GUILayout.Space(15);

                if (GUILayout.Button("Arrage"))
                {
                    tool.Arrange();
                }
            }
        }
    }
}
