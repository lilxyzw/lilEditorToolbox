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
        public bool resetPosition = true;
        public bool resetRotation = true;
        public bool resetScale = false;

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
            EditorGUI.indentLevel++;
            // 翻訳すると分かりづらいのでキーは作ってない
            resetPosition = L10n.ToggleLeft("Position", resetPosition);
            resetRotation = L10n.ToggleLeft("Rotation", resetRotation);
            resetScale = L10n.ToggleLeft("Scale", resetScale);
            EditorGUI.indentLevel--;

            EditorGUI.BeginDisabledGroup(!target);

            // Reset all transforms
            EditorGUILayout.Space();
            EditorGUI.BeginDisabledGroup(!isPrefab);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            L10n.LabelField("Reset to prefab", EditorStyles.boldLabel);
            if(L10n.Button("All transforms"))
                ResetAllTransformToPrefab(target, BoolToTarget(resetRotation, resetPosition, resetScale));
            if(L10n.Button("Humanoid transforms"))
                ResetHumanoidTransformToPrefab(target, BoolToTarget(resetRotation, resetPosition, resetScale));
            EditorGUILayout.EndVertical();
            EditorGUI.EndDisabledGroup();

            // Reset humanoid transforms
            EditorGUILayout.Space();
            EditorGUI.BeginDisabledGroup(!isHuman);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            L10n.LabelField("Reset to animator", EditorStyles.boldLabel);
            if(L10n.Button("Humanoid transforms"))
                ResetHumanoidTransformToAvatar(target, BoolToTarget(resetRotation, resetPosition, resetScale));
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
                CopyTransforms(target.GetComponent<Transform>(), prefab.GetComponent<Transform>(), BoolToTarget(resetRotation, resetPosition, resetScale));
            EditorGUILayout.EndVertical();
            EditorGUI.EndDisabledGroup();

            EditorGUI.EndDisabledGroup();
        }

        private static void ResetAllTransformToPrefab(GameObject target, TransformTarget transformTarget)
        {
            if(!target || !PrefabUtility.IsPartOfAnyPrefab(target)) return;

            var transforms = target.GetComponentsInChildren<Transform>(true);
            foreach(var transform in transforms) ResetTransform(transform, transformTarget);
        }

        private static void ResetHumanoidTransformToPrefab(GameObject target, TransformTarget transformTarget)
        {
            var animator = target.GetComponent<Animator>();
            if(!animator || !animator.isHuman || !PrefabUtility.IsPartOfAnyPrefab(target)) return;

            foreach(var humanBodyBone in humanBodyBones)
                ResetTransform(animator.GetBoneTransform(humanBodyBone), transformTarget);
        }

        private static void ResetHumanoidTransformToAvatar(GameObject target, TransformTarget transformTarget)
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
                SetTransform(transform, skeletonBone.rotation, skeletonBone.position, skeletonBone.scale, transformTarget);
            }
        }

        private static void CopyTransforms(Transform target, Transform prefab, TransformTarget transformTarget)
        {
            if(!target || !prefab) return;
            for(int i = 0; i < prefab.childCount; i++)
            {
                var childPrefab = prefab.GetChild(i);
                var child = target.Find(childPrefab.name);
                if(!child) continue;
                SetTransform(child, childPrefab.localRotation, childPrefab.localPosition, childPrefab.localScale, transformTarget);
                CopyTransforms(child, childPrefab, transformTarget);
            }
        }

        private static void ResetTransform(Transform transform, TransformTarget transformTarget)
        {
            if(!transform) return;
            using var so = new SerializedObject(transform);
            using var m_LocalRotation = so.FindProperty("m_LocalRotation");
            using var m_LocalPosition = so.FindProperty("m_LocalPosition");
            using var m_LocalScale = so.FindProperty("m_LocalScale");
            if(transformTarget.HasFlag(TransformTarget.Rotation)) PrefabUtility.RevertPropertyOverride(m_LocalRotation, InteractionMode.UserAction);
            if(transformTarget.HasFlag(TransformTarget.Position)) PrefabUtility.RevertPropertyOverride(m_LocalPosition, InteractionMode.UserAction);
            if(transformTarget.HasFlag(TransformTarget.Scale   )) PrefabUtility.RevertPropertyOverride(m_LocalScale   , InteractionMode.UserAction);
        }

        private static void SetTransform(Transform transform, Quaternion rotation, Vector3 position, Vector3 scale, TransformTarget transformTarget)
        {
            using var so = new SerializedObject(transform);
            using var m_LocalRotation = so.FindProperty("m_LocalRotation");
            using var m_LocalPosition = so.FindProperty("m_LocalPosition");
            using var m_LocalScale = so.FindProperty("m_LocalScale");
            if(transformTarget.HasFlag(TransformTarget.Rotation)) m_LocalRotation.quaternionValue = rotation;
            if(transformTarget.HasFlag(TransformTarget.Position)) m_LocalPosition.vector3Value = position;
            if(transformTarget.HasFlag(TransformTarget.Scale   )) m_LocalScale.vector3Value = scale;
            so.ApplyModifiedProperties();
        }

        private static TransformTarget BoolToTarget(bool rotation, bool position, bool scale)
        {
            var transformTarget = TransformTarget.None;
            if(rotation) transformTarget |= TransformTarget.Rotation;
            if(position) transformTarget |= TransformTarget.Position;
            if(scale   ) transformTarget |= TransformTarget.Scale;
            return transformTarget;
        }

        [Flags]
        private enum TransformTarget
        {
            None = 0,
            Rotation = 1 << 0,
            Position = 1 << 1,
            Scale = 1 << 2
        }
    }
}
