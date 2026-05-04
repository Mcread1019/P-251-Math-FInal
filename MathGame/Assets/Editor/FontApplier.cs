#if UNITY_EDITOR
using TMPro;
using UnityEditor;
using UnityEngine;

// Logic Game → Font Setup
public class FontApplierWindow : EditorWindow
{
    TMP_FontAsset _font;
    Vector2 _scroll;

    [MenuItem("Logic Game/Font Setup")]
    public static void ShowWindow() => GetWindow<FontApplierWindow>("Logic Font Setup");

    void OnGUI()
    {
        _scroll = EditorGUILayout.BeginScrollView(_scroll);
        GUILayout.Space(6);

        // ── Step 1 ───────────────────────────────────────────────────────────
        EditorGUILayout.HelpBox(
            "STEP 1 — Get a math-capable font\n\n" +
            "Download 'DejaVu Sans' — it has full coverage of all logic symbols:\n" +
            "  https://dejavu-fonts.github.io/\n\n" +
            "Click 'Download' on that page, unzip, and drag\n" +
            "  DejaVuSans.ttf  into  Assets/Fonts/  in your Unity Project window.\n\n" +
            "(Noto Sans Regular does NOT include math operators — you need\n" +
            " Noto Sans Math or DejaVu Sans specifically.)",
            MessageType.Info);

        GUILayout.Space(4);

        // ── Step 2 ───────────────────────────────────────────────────────────
        EditorGUILayout.HelpBox(
            "STEP 2 — Create a Dynamic TMP Font Asset\n\n" +
            "  a) Select DejaVuSans.ttf in the Project window.\n" +
            "  b) Go to  Window → TextMeshPro → Font Asset Creator.\n" +
            "  c) Set  Atlas Population Mode  →  Dynamic\n" +
            "     (Dynamic adds characters to the atlas at runtime, so you\n" +
            "      never have to list every Unicode codepoint manually.)\n" +
            "  d) Click  Generate Font Atlas,  then  Save.\n" +
            "     Save it as  Assets/Fonts/DejaVuSans SDF.asset",
            MessageType.Info);

        GUILayout.Space(4);

        // ── Step 3 ───────────────────────────────────────────────────────────
        EditorGUILayout.HelpBox(
            "STEP 3 — Apply the font\n\n" +
            "Drag the saved .asset into the field below, then click Apply.\n" +
            "Re-run  Logic Game → Setup Scene  afterwards so new components\n" +
            "also pick up the font.",
            MessageType.Info);

        GUILayout.Space(8);
        _font = (TMP_FontAsset)EditorGUILayout.ObjectField(
            "Font Asset (.asset)", _font, typeof(TMP_FontAsset), false);

        GUILayout.Space(2);

        // Warn if the assigned font is Static (common mistake)
        if (_font != null && _font.atlasPopulationMode == AtlasPopulationMode.Static)
        {
            EditorGUILayout.HelpBox(
                "This font asset is in STATIC mode — symbols may still appear as □.\n" +
                "Regenerate it with Atlas Population Mode set to Dynamic.",
                MessageType.Warning);
        }

        GUILayout.Space(4);
        GUI.enabled = _font != null;
        if (GUILayout.Button("Apply Font to All TMP Components in Scene", GUILayout.Height(36)))
            ApplyFont();
        GUI.enabled = true;

        GUILayout.Space(8);
        EditorGUILayout.HelpBox(
            "Alternative: add the font asset to the global fallback list at\n" +
            "Window → TextMeshPro → Settings → Fallback Font Assets.\n" +
            "That lets the default font handle ASCII while DejaVu handles symbols.",
            MessageType.None);

        EditorGUILayout.EndScrollView();
    }

    void ApplyFont()
    {
        var components = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
        int count = 0;
        foreach (var tmp in components)
        {
            if (tmp.hideFlags != HideFlags.None) continue;
            Undo.RecordObject(tmp, "Apply Logic Font");
            tmp.font = _font;
            EditorUtility.SetDirty(tmp);
            count++;
        }
        Debug.Log($"[Logic Game] Applied '{_font.name}' to {count} TMP components.");
        EditorUtility.DisplayDialog("Done",
            $"Font applied to {count} TextMeshProUGUI components.\n\n" +
            "Re-run Logic Game → Setup Scene to apply it to freshly-built panels too.",
            "OK");
    }
}
#endif

