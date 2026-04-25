using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SinforosoPackage
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class SinforosoButtonManager_Mono : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawButtons(target);
        }

        private void DrawButtons(object targetObj)
        {
            MethodInfo[] methodInfos = targetObj.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (MethodInfo methodInfo in methodInfos)
            {
                object[] attributes = methodInfo.GetCustomAttributes(typeof(SinforosoButton), false);

                if (attributes.Length > 0)
                {
                    if (GUILayout.Button(methodInfo.Name))
                    {
                        methodInfo.Invoke(targetObj, null);
                    }
                }
            }
        }
    }
}
