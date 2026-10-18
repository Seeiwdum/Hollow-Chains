using HollowChains.Core.Events;
using HollowChains.Gameplay.Interaction;
using HollowChains.Gameplay.Triggers;
using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HollowChains.Editor.Greybox
{
    /// <summary>
    /// Editor helpers for building ProBuilder greybox scenes (D-022/D-023): grey blocks, yellow interactables,
    /// invisible trigger volumes, capsule NPCs, per-scene light + Volume. Every block stays a ProBuilder mesh so
    /// level design can keep editing it with the ProBuilder tools. Used by greybox build commands; not in builds.
    /// </summary>
    public static class GreyboxKit
    {
        public const string MaterialFolder = "Assets/Placeholders/Materials";
        public const string VolumeFolder = "Assets/_Project/Settings/Volumes";

        public static Material Grey => Mat("M_Greybox", new Color(0.5f, 0.5f, 0.5f));
        public static Material Dark => Mat("M_Greybox_Dark", new Color(0.24f, 0.28f, 0.31f));   // alley / city backdrop
        public static Material Wood => Mat("M_Greybox_Wood", new Color(0.42f, 0.36f, 0.3f));
        public static Material Foliage => Mat("M_Greybox_Foliage", new Color(0.19f, 0.23f, 0.18f));
        public static Material Water => Mat("M_Greybox_Water", new Color(0.44f, 0.66f, 0.72f));
        public static Material Interact => Mat("M_Greybox_Interact", new Color(0.88f, 0.69f, 0.25f));
        public static Material Villager => Mat("M_Capsule_Villager", new Color(0.55f, 0.6f, 0.66f));
        public static Material Elena => Mat("M_Capsule_Elena", new Color(0.95f, 0.89f, 0.77f));
        public static Material Hollow => Mat("M_Capsule_Hollow", new Color(0.04f, 0.04f, 0.06f));

        public static Material Mat(string name, Color color)
        {
            string path = $"{MaterialFolder}/{name}.mat";
            var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat != null) return mat;
            EnsureFolder(MaterialFolder);
            mat = new Material(Shader.Find("Universal Render Pipeline/Lit")) { color = color };
            AssetDatabase.CreateAsset(mat, path);
            return mat;
        }

        public static GameObject Group(string name, Transform parent = null)
        {
            var go = new GameObject(name);
            if (parent != null) go.transform.SetParent(parent, false);
            return go;
        }

        public static GameObject Block(string name, Transform parent, Vector3 center, Vector3 size, float yaw = 0f, Material mat = null)
            => Finish(ShapeGenerator.GenerateCube(PivotLocation.Center, size), name, parent, center, yaw, mat ?? Grey);

        public static GameObject Prism(string name, Transform parent, Vector3 center, Vector3 size, float yaw = 0f, Material mat = null)
            => Finish(ShapeGenerator.GeneratePrism(PivotLocation.Center, size), name, parent, center, yaw, mat ?? Wood);

        public static GameObject Cylinder(string name, Transform parent, Vector3 baseCenter, float radius, float height, Material mat = null, int sides = 12)
        {
            var go = Finish(ShapeGenerator.GenerateCylinder(PivotLocation.Center, sides, radius, height, 0), name, parent,
                baseCenter + Vector3.up * (height * 0.5f), 0f, mat ?? Grey);
            return go;
        }

        /// <summary>Simple tree: trunk cylinder + blocky canopy.</summary>
        public static GameObject Tree(string name, Transform parent, Vector3 basePos, float height, bool dead = false)
        {
            var tree = Group(name, parent);
            tree.transform.position = basePos;
            Cylinder("Trunk", tree.transform, basePos, dead ? 0.25f : 0.35f, height, Wood, 8);
            if (!dead) Block("Canopy", tree.transform, basePos + Vector3.up * (height * 0.85f), new Vector3(3.2f, 2.6f, 3.2f), Random.Range(0f, 90f), Foliage);
            return tree;
        }

        /// <summary>Capsule stand-in for characters (D-023): height in metres, optional facing.</summary>
        public static GameObject Capsule(string name, Transform parent, Vector3 feet, float height, float yaw, Material mat)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            go.name = name;
            go.transform.SetParent(parent, false);
            go.transform.position = feet + Vector3.up * (height * 0.5f);
            go.transform.localScale = new Vector3(height * 0.36f, height * 0.5f, height * 0.36f);
            go.transform.rotation = Quaternion.Euler(0f, yaw, 0f);
            go.GetComponent<MeshRenderer>().sharedMaterial = mat;
            return go;
        }

        public static T Trigger<T>(string name, Transform parent, Vector3 center, Vector3 size) where T : PlayerTriggerZone
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.position = center;
            var box = go.AddComponent<BoxCollider>();
            box.isTrigger = true;
            box.size = size;
            return go.AddComponent<T>();
        }

        public static T Interactable<T>(string name, Transform parent, Vector3 center, Vector3 size, string prompt) where T : InteractableBase
        {
            var go = Block(name, parent, center, size, 0f, Interact);
            GameObjectUtility.SetStaticEditorFlags(go, 0); // interactables move (bucket) — never static
            var component = go.AddComponent<T>();
            SetString(component, "_prompt", prompt);
            return component;
        }

        public static void SetString(Object target, string property, string value)
        {
            var so = new SerializedObject(target);
            so.FindProperty(property).stringValue = value;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        public static void SetRef(Object target, string property, Object value)
        {
            var so = new SerializedObject(target);
            so.FindProperty(property).objectReferenceValue = value;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        public static T Channel<T>(string name) where T : EventChannelBaseSO =>
            AssetDatabase.LoadAssetAtPath<T>($"Assets/_Project/Data/Events/{name}.asset");

        public static GameObject PlayerRig(Vector3 feet, float yaw)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Project/Prefabs/PlayerRig.prefab");
            var rig = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            rig.transform.SetPositionAndRotation(feet, Quaternion.Euler(0f, yaw, 0f));
            return rig;
        }

        /// <summary>Directional light + global Volume + fog following the Art Bible palette for the scene.</summary>
        public static void Atmosphere(string profileName, Color lightColor, float lightIntensity, Vector3 lightEuler,
            float saturation, Color colorFilter, float vignette, Color fogColor, float fogDensity)
        {
            var lightGo = new GameObject("Sun");
            var light = lightGo.AddComponent<Light>();
            light.type = LightType.Directional;
            light.color = lightColor;
            light.intensity = lightIntensity;
            light.shadows = LightShadows.Soft;
            lightGo.transform.rotation = Quaternion.Euler(lightEuler);

            EnsureFolder(VolumeFolder);
            string path = $"{VolumeFolder}/{profileName}.asset";
            var profile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(path);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<VolumeProfile>();
                AssetDatabase.CreateAsset(profile, path);
                var color = profile.Add<ColorAdjustments>(true);
                color.saturation.Override(saturation);
                color.colorFilter.Override(colorFilter);
                var vig = profile.Add<Vignette>(true);
                vig.intensity.Override(vignette);
                var tone = profile.Add<Tonemapping>(true);
                tone.mode.Override(TonemappingMode.ACES);
                foreach (var component in profile.components) AssetDatabase.AddObjectToAsset(component, profile);
                EditorUtility.SetDirty(profile);
            }

            var volumeGo = new GameObject("Global Volume");
            var volume = volumeGo.AddComponent<Volume>();
            volume.isGlobal = true;
            volume.sharedProfile = profile;

            RenderSettings.fog = fogDensity > 0f;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogDensity;
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = Color.Lerp(fogColor, Color.grey, 0.5f);
        }

        public static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;
            var parent = System.IO.Path.GetDirectoryName(path).Replace('\\', '/');
            EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, System.IO.Path.GetFileName(path));
        }

        private static GameObject Finish(ProBuilderMesh mesh, string name, Transform parent, Vector3 center, float yaw, Material mat)
        {
            mesh.SetMaterial(mesh.faces, mat);
            mesh.ToMesh();
            mesh.Refresh();
            var go = mesh.gameObject;
            go.name = name;
            if (parent != null) go.transform.SetParent(parent, false);
            go.transform.SetPositionAndRotation(center, Quaternion.Euler(0f, yaw, 0f));
            var collider = go.AddComponent<MeshCollider>();
            collider.sharedMesh = go.GetComponent<MeshFilter>().sharedMesh;
            GameObjectUtility.SetStaticEditorFlags(go, StaticEditorFlags.BatchingStatic | StaticEditorFlags.OccluderStatic | StaticEditorFlags.OccludeeStatic);
            return go;
        }
    }
}
