// ***********************************************************************
// Company          : Kunpo
// Author           : KimCh
// Created          : 
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
namespace Game
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(Transform)), CanEditMultipleObjects]
    internal class TransformInspector : Editor
    {
        private SerializedProperty m_Position;
        private SerializedProperty m_Rotation;
        private SerializedProperty m_Scale;
        private static Contents s_Contents;

        private Vector3 m_EulerAngles;
        private Quaternion m_OldQuaternion = new Quaternion(1234f, 1234f, 4321f, -4567f);

        private static int s_FoldoutHash = "Foldout".GetHashCode();
        private void Inspector3D()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(s_Contents.positionContent, GUILayout.MaxWidth(22)))
            {
                foreach (Transform transform3 in this.targets)
                {
                    transform3.localPosition = Vector3.zero;
                }
            }
            EditorGUILayout.PropertyField(this.m_Position, GUIContent.none);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(s_Contents.rotationContent, GUILayout.MaxWidth(22)))
            {
                foreach (Transform transform3 in this.targets)
                {
                    transform3.localRotation = Quaternion.identity;
                }
            }
            this.RotationField();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(s_Contents.scaleContent, GUILayout.MaxWidth(22)))
            {
                foreach (Transform transform3 in this.targets)
                {
                    transform3.localScale = Vector3.one;
                }
            }
            EditorGUILayout.PropertyField(this.m_Scale, GUIContent.none);
            EditorGUILayout.EndHorizontal();
        }

        public void RotationField()
        {
            Transform transform = this.target as Transform;
            Quaternion localRotation = transform.localRotation;
            if (((this.m_OldQuaternion.x != localRotation.x) || (this.m_OldQuaternion.y != localRotation.y)) || ((this.m_OldQuaternion.z != localRotation.z) || (this.m_OldQuaternion.w != localRotation.w)))
            {
                this.m_EulerAngles = transform.localEulerAngles;
                this.m_OldQuaternion = localRotation;
            }
            bool flag = false;
            foreach (Transform transform2 in this.targets)
            {
                flag |= transform2.localEulerAngles != this.m_EulerAngles;
            }
            Rect totalPosition = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);
            GUIContent label = EditorGUI.BeginProperty(totalPosition, GUIContent.none, this.m_Rotation);
            EditorGUI.showMixedValue = flag;
            EditorGUI.BeginChangeCheck();
            int id = GUIUtility.GetControlID(s_FoldoutHash, FocusType.Passive, totalPosition);
            totalPosition = EditorGUI.PrefixLabel(totalPosition, id, label);
            totalPosition.height = EditorGUIUtility.singleLineHeight;

            this.m_EulerAngles = EditorGUI.Vector3Field(totalPosition, GUIContent.none, this.m_EulerAngles);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(this.targets, "Inspector");
                foreach (Transform transform3 in this.targets)
                {
                    transform3.localEulerAngles = this.m_EulerAngles;
                }
                this.m_Rotation.serializedObject.SetIsDifferentCacheDirty();
                this.m_OldQuaternion = localRotation;
            }
            EditorGUI.showMixedValue = false;
            EditorGUI.EndProperty();
        }
        public void OnEnable()
        {
            this.m_Position = base.serializedObject.FindProperty("m_LocalPosition");
            this.m_Rotation = base.serializedObject.FindProperty("m_LocalRotation");
            this.m_Scale = base.serializedObject.FindProperty("m_LocalScale");
        }

        public override void OnInspectorGUI()
        {
            if (s_Contents == null)
            {
                s_Contents = new Contents();
            }
            base.serializedObject.Update();
            this.Inspector3D();
            base.serializedObject.ApplyModifiedProperties();
        }

        private class Contents
        {
            public GUIContent positionContent = new GUIContent("P", "The local position of this Game Object relative to the parent.");
            public GUIContent rotationContent = new GUIContent("R", "The local rotation of this Game Object relative to the parent.");
            public GUIContent scaleContent = new GUIContent("S", "The local scaling of this Game Object relative to the parent.");
        }
    }
}
