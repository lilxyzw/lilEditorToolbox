using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    internal class TransformResetter : EditorWindow
    {
        [MenuItem(Common.MENU_HEAD + "Transform Resetter")]
        static void Init() => GetWindow(typeof(TransformResetter)).Show();

        private static readonly HumanBodyBones[] humanBodyBones = (Enum.GetValues(typeof(HumanBodyBones)) as HumanBodyBones[]).Where(h => h != HumanBodyBones.LastBone).ToArray();
        public GameObject target;
        public GameObject prefab;
        public bool isHuman = false;
        public bool isPrefab = false;

        void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            target = L10n.ObjectField("Edit target", target, typeof(GameObject), true) as GameObject;
            if(EditorGUI.EndChangeCheck())
            {
                if(target)
                {
                    var animator = target.GetComponent<Animator>();
                    isHuman = animator && animator.isHuman;
                    isPrefab = PrefabUtility.IsPartOfAnyPrefab(target);
                }
                else
                {
                    isHuman = false;
                    isPrefab = false;
                }
            }

            EditorGUI.BeginDisabledGroup(!target);

            // Reset all transforms
            EditorGUILayout.Space();
            EditorGUI.BeginDisabledGroup(!isPrefab);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            L10n.LabelField("Reset to prefab", EditorStyles.boldLabel);
            if(L10n.Button("All transforms"))
                ResetAllTransformToPrefab(target);
            if(L10n.Button("Humanoid transforms"))
                ResetHumanoidTransformToPrefab(target);
            EditorGUILayout.EndVertical();
            EditorGUI.EndDisabledGroup();

            // Reset humanoid transforms
            EditorGUILayout.Space();
            EditorGUI.BeginDisabledGroup(!isHuman);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            L10n.LabelField("Reset to animator", EditorStyles.boldLabel);
            if(L10n.Button("Humanoid transforms"))
                ResetHumanoidTransformToAvatar(target);
            EditorGUILayout.EndVertical();
            EditorGUI.EndDisabledGroup();

            // Copy all transforms
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            L10n.LabelField("Copy from other object", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            prefab = L10n.ObjectField("Copy from", prefab, typeof(GameObject), true) as GameObject;
            EditorGUI.BeginDisabledGroup(!prefab);
            if(L10n.Button("All transforms"))
                CopyTransforms(target.GetComponent<Transform>(), prefab.GetComponent<Transform>());
            EditorGUILayout.EndVertical();
            EditorGUI.EndDisabledGroup();

            EditorGUI.EndDisabledGroup();
        }

        private static void ResetAllTransformToPrefab(GameObject target)
        {
            if(!target || !PrefabUtility.IsPartOfAnyPrefab(target)) return;

            var transforms = target.GetComponentsInChildren<Transform>(true);
            foreach(var transform in transforms) ResetTransform(transform);
        }

        private static void ResetHumanoidTransformToPrefab(GameObject target)
        {
            var animator = target.GetComponent<Animator>();
            if(!animator || !animator.isHuman || !PrefabUtility.IsPartOfAnyPrefab(target)) return;

            foreach(var humanBodyBone in humanBodyBones)
                ResetTransform(animator.GetBoneTransform(humanBodyBone));
        }

        private static void ResetHumanoidTransformToAvatar(GameObject target)
        {
            var animator = target.GetComponent<Animator>();
            if(!animator || !animator.isHuman || !animator.avatar) return;

            var skeletonBones = animator.avatar.humanDescription.skeleton;
            foreach(var humanBodyBone in humanBodyBones)
            {
                var transform = animator.GetBoneTransform(humanBodyBone);
                if(!transform) continue;
                var skeletonBone = skeletonBones.FirstOrDefault(s => s.name == transform.name);
                if(string.IsNullOrEmpty(skeletonBone.name)) continue;
                SetTransform(transform, skeletonBone.rotation, skeletonBone.position);
            }
        }

        private static void CopyTransforms(Transform target, Transform prefab)
        {
            if(!target || !prefab) return;
            for(int i = 0; i < prefab.childCount; i++)
            {
                var childPrefab = prefab.GetChild(i);
                var child = target.Find(childPrefab.name);
                if(!child) continue;
                SetTransform(child, childPrefab.localRotation, childPrefab.localPosition);
                CopyTransforms(child, childPrefab);
            }
        }

        private static void ResetTransform(Transform transform)
        {
            if(!transform) return;
            using var so = new SerializedObject(transform);
            using var m_LocalRotation = so.FindProperty("m_LocalRotation");
            using var m_LocalPosition = so.FindProperty("m_LocalPosition");
            PrefabUtility.RevertPropertyOverride(m_LocalRotation, InteractionMode.UserAction);
            PrefabUtility.RevertPropertyOverride(m_LocalPosition, InteractionMode.UserAction);
        }

        private static void SetTransform(Transform transform, Quaternion rotation, Vector3 position)
        {
            using var so = new SerializedObject(transform);
            using var m_LocalRotation = so.FindProperty("m_LocalRotation");
            using var m_LocalPosition = so.FindProperty("m_LocalPosition");
            m_LocalRotation.quaternionValue = rotation;
            m_LocalPosition.vector3Value = position;
            so.ApplyModifiedProperties();
        }
    }
}
