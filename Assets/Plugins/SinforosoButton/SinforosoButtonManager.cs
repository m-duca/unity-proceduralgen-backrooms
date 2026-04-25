using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Object), true)] // "Object" inclui MonoBehaviour e ScriptableObject
public class SinforosoButtonManager : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Object _obj_target = target;

        MethodInfo[] _methods = _obj_target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (MethodInfo _method in _methods)
        {
            var _attributes = _method.GetCustomAttributes(typeof(SinforosoButton), false);

            if (_attributes.Length > 0)
            {
                if (GUILayout.Button(_method.Name))
                {
                    _method.Invoke(_obj_target, null);
                }
            }
        }
    }
}
