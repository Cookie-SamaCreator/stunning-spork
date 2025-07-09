#if UNITY_EDITOR
using UnityEditor;
using RPG.Definitions;

[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var weaponTypeProp = serializedObject.FindProperty("weaponType");
        var elementTypeProp = serializedObject.FindProperty("elementType");

        EditorGUILayout.PropertyField(weaponTypeProp);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("physicalDamage"));
        EditorGUILayout.PropertyField(elementTypeProp);

        // elemental damage only available if elemental type isn't None
        if ((int)elementTypeProp.enumValueIndex != 0) // 0 = None
            EditorGUILayout.PropertyField(serializedObject.FindProperty("elementalDamage"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("attackRate"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ignoreOwner"));

        if ((WeaponType)weaponTypeProp.enumValueIndex == WeaponType.Magic)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("manaCost"));

        if ((WeaponType)weaponTypeProp.enumValueIndex == WeaponType.Ranged)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("weaponRange"));

        if ((WeaponType)weaponTypeProp.enumValueIndex == WeaponType.Ranged)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("projectilePrefab"));

        if ((WeaponType)weaponTypeProp.enumValueIndex == WeaponType.Melee ||
            (WeaponType)weaponTypeProp.enumValueIndex == WeaponType.Magic)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("damageZonePrefab"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("statModifiers"), true);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif