#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using LogicGame;

// Run via: Unity menu  →  Logic Game  →  Setup Scene
// Buttons register their own listeners in Awake() so the scene only needs
// to be set up ONCE — no need to re-run this after the first save.
public static class LogicGameSetup
{
    [MenuItem("Logic Game/Setup Scene")]
    public static void SetupScene()
    {
        var old = GameObject.Find("LogicGameRoot");
        if (old != null) Object.DestroyImmediate(old);

        var root = new GameObject("LogicGameRoot");

        // ── Managers ─────────────────────────────────────────────────────────
        var gmGO = new GameObject("GameManager");
        gmGO.transform.SetParent(root.transform);
        var gm = gmGO.AddComponent<GameManager>();

        var smGO = new GameObject("ScoreManager");
        smGO.transform.SetParent(root.transform);
        smGO.AddComponent<ScoreManager>();

        // ── Canvas ───────────────────────────────────────────────────────────
        var canvasGO = new GameObject("Canvas");
        canvasGO.transform.SetParent(root.transform);
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode         = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight  = 0.5f;
        canvasGO.AddComponent<GraphicRaycaster>();

        if (Object.FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            var es = new GameObject("EventSystem");
            es.transform.SetParent(root.transform);
            es.AddComponent<UnityEngine.EventSystems.EventSystem>();
            es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        // ── Panels ───────────────────────────────────────────────────────────
        var mainMenuPanel   = BuildMainMenuPanel(canvasGO);
        var difficultyPanel = BuildDifficultyPanel(canvasGO);
        var polishPanel     = BuildPolishPanel(canvasGO);
        var sellPanel       = BuildSellPanel(canvasGO);
        var resultsPanel    = BuildResultsPanel(canvasGO);

        // ── Wire GameManager ─────────────────────────────────────────────────
        var gmSO = new SerializedObject(gm);
        gmSO.FindProperty("mainMenuPanel").objectReferenceValue   = mainMenuPanel;
        gmSO.FindProperty("difficultyPanel").objectReferenceValue = difficultyPanel;
        gmSO.FindProperty("polishPanel").objectReferenceValue     = polishPanel;
        gmSO.FindProperty("sellPanel").objectReferenceValue       = sellPanel;
        gmSO.FindProperty("resultsPanel").objectReferenceValue    = resultsPanel;
        gmSO.ApplyModifiedProperties();

        // ── Save scene so Play works without re-running this script ──────────
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();
        Debug.Log("Logic Game setup complete — scene saved. Press Play.");
    }

    // ── Panel builders ────────────────────────────────────────────────────────

    static GameObject BuildMainMenuPanel(GameObject canvas)
    {
        var panel     = MakeFullPanel(canvas, "MainMenuPanel", new Color(0.08f, 0.08f, 0.15f));
        var scoreText = MakeText(panel, "TotalScoreText", "Total Coins: 0", 28, 0, -40, 500, 55);
        MakeText(panel, "TitleText", "GEM LOGIC\nA Logic Proof Game", 52, 0, 100, 900, 180);
        var playBtn   = MakeButton(panel, "PlayButton", "PLAY", 0, -160, 320, 75);

        var ui   = panel.AddComponent<MainMenuUI>();
        var uiSO = new SerializedObject(ui);
        uiSO.FindProperty("totalScoreText").objectReferenceValue = scoreText.GetComponent<TextMeshProUGUI>();
        uiSO.FindProperty("playButton").objectReferenceValue     = playBtn;
        uiSO.ApplyModifiedProperties();

        return panel;
    }

    static GameObject BuildDifficultyPanel(GameObject canvas)
    {
        var panel  = MakeFullPanel(canvas, "DifficultyPanel", new Color(0.08f, 0.1f, 0.08f));
        MakeText(panel, "Title", "Choose Difficulty", 42, 0, 210, 700, 90);
        var easy   = MakeButton(panel, "EasyButton",   "EASY\n(1 blank)",      0,  80, 340, 80);
        var medium = MakeButton(panel, "MediumButton", "MEDIUM\n(2-3 blanks)", 0, -20, 340, 80);
        var hard   = MakeButton(panel, "HardButton",   "HARD\n(4-5 blanks)",   0,-120, 340, 80);
        var back   = MakeButton(panel, "BackButton",   "Back",                  0,-230, 200, 60);

        var ui   = panel.AddComponent<DifficultyUI>();
        var uiSO = new SerializedObject(ui);
        uiSO.FindProperty("easyButton").objectReferenceValue   = easy;
        uiSO.FindProperty("mediumButton").objectReferenceValue = medium;
        uiSO.FindProperty("hardButton").objectReferenceValue   = hard;
        uiSO.FindProperty("backButton").objectReferenceValue   = back;
        uiSO.ApplyModifiedProperties();

        panel.SetActive(false);
        return panel;
    }

    static GameObject BuildPolishPanel(GameObject canvas)
    {
        var panel = MakeFullPanel(canvas, "PolishPanel", new Color(0.1f, 0.07f, 0.15f));
        MakeText(panel, "Header", "POLISH PHASE - Soundness & Shininess", 26, 0, 460, 1700, 55);

        var questionText = MakeText(panel, "QuestionText", "", 22, 0, 310, 1680, 180);
        questionText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;

        const float blankW = 155f, blankGap = 12f, blankStep = blankW + blankGap;
        float blankStart = -2f * blankStep;
        var blankBtns   = new Button[5];
        var blankLabels = new TextMeshProUGUI[5];
        for (int i = 0; i < 5; i++)
        {
            blankBtns[i]   = MakeButton(panel, $"BlankSlot_{i}", "___", blankStart + i * blankStep, 155f, blankW, 60f);
            blankLabels[i] = blankBtns[i].GetComponentInChildren<TextMeshProUGUI>();
        }

        MakeText(panel, "WordBankLabel", "- Word Bank -", 20, 0, 72f, 400, 35);

        const float wbW = 190f, wbGap = 10f, wbStep = wbW + wbGap;
        float wbStart = -1.5f * wbStep;
        var wordBtns   = new Button[8];
        var wordLabels = new TextMeshProUGUI[8];
        for (int i = 0; i < 8; i++)
        {
            float x = wbStart + (i % 4) * wbStep;
            float y = i < 4 ? 18f : -48f;
            wordBtns[i]   = MakeButton(panel, $"WordBtn_{i}", "", x, y, wbW, 55f);
            wordLabels[i] = wordBtns[i].GetComponentInChildren<TextMeshProUGUI>();
        }

        var feedbackText    = MakeText(panel, "FeedbackText",    "", 20, 0, -130f, 1680, 50);
        var explanationText = MakeText(panel, "ExplanationText", "", 18, 0, -220f, 1680, 100);
        var gemValueText    = MakeText(panel, "GemValueText",    "", 22, 0, -340f, 600,  50);
        var submitBtn       = MakeButton(panel, "SubmitButton",   "Submit",          -150f, -430f, 240, 65);
        var continueBtn     = MakeButton(panel, "ContinueButton", "Continue > Sell",  150f, -430f, 280, 65);

        // ── Gem display area (right side of panel) ────────────────────────────
        var gemController = BuildGemDisplay(panel);

        var phaseUI = panel.AddComponent<PolishPhaseUI>();
        var so      = new SerializedObject(phaseUI);
        so.FindProperty("questionText").objectReferenceValue    = questionText.GetComponent<TextMeshProUGUI>();
        so.FindProperty("feedbackText").objectReferenceValue    = feedbackText.GetComponent<TextMeshProUGUI>();
        so.FindProperty("explanationText").objectReferenceValue = explanationText.GetComponent<TextMeshProUGUI>();
        so.FindProperty("gemValueText").objectReferenceValue    = gemValueText.GetComponent<TextMeshProUGUI>();
        so.FindProperty("submitButton").objectReferenceValue    = submitBtn;
        so.FindProperty("continueButton").objectReferenceValue  = continueBtn;
        so.FindProperty("gemDisplay").objectReferenceValue      = gemController;
        AssignArray(so, "blankSlotButtons", blankBtns);
        AssignArray(so, "blankSlotLabels",  blankLabels);
        AssignArray(so, "wordBankButtons",  wordBtns);
        AssignArray(so, "wordBankLabels",   wordLabels);
        so.ApplyModifiedProperties();

        panel.SetActive(false);
        return panel;
    }

    // Creates gem image, cloth overlay, and 8 shine rays; returns the GemDisplayController.
    static GemDisplayController BuildGemDisplay(GameObject panel)
    {
        const float GemX = 700f, GemY = 250f, GemSize = 200f;

        // Container (just a RectTransform anchor)
        var container = new GameObject("GemDisplay");
        container.transform.SetParent(panel.transform, false);
        var crt = container.AddComponent<RectTransform>();
        crt.sizeDelta        = new Vector2(GemSize + 40f, GemSize + 40f);
        crt.anchoredPosition = new Vector2(GemX, GemY);

        // Gem image
        var gemGO = new GameObject("GemImage");
        gemGO.transform.SetParent(container.transform, false);
        var gemImg = gemGO.AddComponent<Image>();
        gemImg.color = new Color(0.6f, 0.3f, 0.9f); // placeholder tint until sprite assigned
        var gemRT = gemGO.GetComponent<RectTransform>();
        gemRT.sizeDelta        = new Vector2(GemSize, GemSize);
        gemRT.anchoredPosition = Vector2.zero;

        // Cloth overlay — a rect that slides across; assign your cloth Material in the Inspector
        var clothGO = new GameObject("ClothOverlay");
        clothGO.transform.SetParent(container.transform, false);
        var clothImg = clothGO.AddComponent<Image>();
        clothImg.color = new Color(0.55f, 0.35f, 0.15f, 0.85f);
        var clothRT = clothGO.GetComponent<RectTransform>();
        clothRT.sizeDelta        = new Vector2(GemSize + 80f, GemSize + 40f);
        clothRT.anchoredPosition = new Vector2(-(GemSize * 0.5f + 100f), 0f);
        clothGO.SetActive(false);

        // 8 shine rays: thin rectangles rotated around gem center
        var rays = new RectTransform[8];
        for (int i = 0; i < 8; i++)
        {
            var rayGO = new GameObject($"ShineRay_{i}");
            rayGO.transform.SetParent(container.transform, false);
            var rayImg  = rayGO.AddComponent<Image>();
            rayImg.color = new Color(1f, 0.9f, 0.2f, 1f);
            var rayRT   = rayGO.GetComponent<RectTransform>();
            rayRT.sizeDelta        = new Vector2(8f, GemSize * 0.8f);
            rayRT.anchoredPosition = Vector2.zero;
            // Pivot at bottom of ray so it radiates outward from gem center
            rayRT.pivot = new Vector2(0.5f, 0f);
            rayGO.transform.localRotation = Quaternion.Euler(0f, 0f, i * 45f);
            rayGO.transform.localScale    = new Vector3(1f, 0f, 1f);
            rayGO.SetActive(false);
            rays[i] = rayRT;
        }

        // Wire GemDisplayController
        var ctrl   = container.AddComponent<GemDisplayController>();
        var ctrlSO = new SerializedObject(ctrl);
        ctrlSO.FindProperty("gemImage").objectReferenceValue    = gemImg;
        ctrlSO.FindProperty("clothOverlay").objectReferenceValue = clothImg;
        AssignArray(ctrlSO, "shineRays", rays);
        ctrlSO.ApplyModifiedProperties();

        return ctrl;
    }

    static GameObject BuildSellPanel(GameObject canvas)
    {
        var panel = MakeFullPanel(canvas, "SellPanel", new Color(0.08f, 0.12f, 0.08f));

        var headerText   = MakeText(panel, "HeaderText",   "SELL PHASE",    26, -300f, 460f, 900,  55);
        var scoreText    = MakeText(panel, "ScoreText",    "Correct: 0 / 0",22,  600f, 460f, 400,  55);
        var questionText = MakeText(panel, "QuestionText", "",               22,    0f, 300f, 1680, 190);
        questionText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;

        var choiceBtns   = new Button[4];
        var choiceLabels = new TextMeshProUGUI[4];
        for (int i = 0; i < 4; i++)
        {
            choiceBtns[i]   = MakeButton(panel, $"Choice_{(char)('A'+i)}", "", 0, 110f - i * 85f, 1400, 75);
            choiceLabels[i] = choiceBtns[i].GetComponentInChildren<TextMeshProUGUI>();
            choiceLabels[i].alignment          = TextAlignmentOptions.Left;
            choiceLabels[i].enableWordWrapping = true;
        }

        var feedbackText    = MakeText(panel, "FeedbackText",    "", 20, 0, -210f, 1680, 50);
        var explanationText = MakeText(panel, "ExplanationText", "", 18, 0, -310f, 1680, 100);
        var nextBtn         = MakeButton(panel, "NextButton", "Next >", 0, -420f, 240, 65);

        var sellUI = panel.AddComponent<SellPhaseUI>();
        var so     = new SerializedObject(sellUI);
        so.FindProperty("headerText").objectReferenceValue      = headerText.GetComponent<TextMeshProUGUI>();
        so.FindProperty("questionText").objectReferenceValue    = questionText.GetComponent<TextMeshProUGUI>();
        so.FindProperty("feedbackText").objectReferenceValue    = feedbackText.GetComponent<TextMeshProUGUI>();
        so.FindProperty("explanationText").objectReferenceValue = explanationText.GetComponent<TextMeshProUGUI>();
        so.FindProperty("scoreText").objectReferenceValue       = scoreText.GetComponent<TextMeshProUGUI>();
        so.FindProperty("nextButton").objectReferenceValue      = nextBtn;
        AssignArray(so, "choiceButtons", choiceBtns);
        AssignArray(so, "choiceLabels",  choiceLabels);
        so.ApplyModifiedProperties();

        panel.SetActive(false);
        return panel;
    }

    static GameObject BuildResultsPanel(GameObject canvas)
    {
        var panel      = MakeFullPanel(canvas, "ResultsPanel", new Color(0.12f, 0.1f, 0.05f));
        MakeText(panel, "Title", "ROUND COMPLETE!", 50, 0, 220, 800, 90);
        var gemText    = MakeText(panel, "GemValueText",    "", 28, 0,  100, 700, 55);
        var multText   = MakeText(panel, "MultiplierText",  "", 28, 0,   20, 700, 55);
        var earnedText = MakeText(panel, "CoinsEarnedText", "", 34, 0,  -80, 700, 65);
        var totalText  = MakeText(panel, "TotalCoinsText",  "", 28, 0, -165, 700, 55);
        var playBtn    = MakeButton(panel, "PlayAgainButton", "Play Again", -150, -290, 260, 70);
        var menuBtn    = MakeButton(panel, "MainMenuButton",  "Main Menu",   150, -290, 260, 70);

        var resultsUI = panel.AddComponent<ResultsUI>();
        var so        = new SerializedObject(resultsUI);
        so.FindProperty("gemValueText").objectReferenceValue    = gemText.GetComponent<TextMeshProUGUI>();
        so.FindProperty("multiplierText").objectReferenceValue  = multText.GetComponent<TextMeshProUGUI>();
        so.FindProperty("coinsEarnedText").objectReferenceValue = earnedText.GetComponent<TextMeshProUGUI>();
        so.FindProperty("totalCoinsText").objectReferenceValue  = totalText.GetComponent<TextMeshProUGUI>();
        so.FindProperty("playAgainButton").objectReferenceValue = playBtn;
        so.FindProperty("mainMenuButton").objectReferenceValue  = menuBtn;
        so.ApplyModifiedProperties();

        panel.SetActive(false);
        return panel;
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    static GameObject MakeFullPanel(GameObject parent, string name, Color bg)
    {
        var go  = new GameObject(name);
        go.transform.SetParent(parent.transform, false);
        go.AddComponent<Image>().color = bg;
        var rt  = go.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
        return go;
    }

    static GameObject MakeText(GameObject parent, string name, string text, int size,
                                float x, float y, float w, float h)
    {
        var go  = new GameObject(name);
        go.transform.SetParent(parent.transform, false);
        var tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = text; tmp.fontSize = size;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.enableWordWrapping = true;
        var rt  = go.GetComponent<RectTransform>();
        rt.sizeDelta       = new Vector2(w, h);
        rt.anchoredPosition = new Vector2(x, y);
        return go;
    }

    static Button MakeButton(GameObject parent, string name, string label,
                              float x, float y, float w, float h)
    {
        var go  = new GameObject(name);
        go.transform.SetParent(parent.transform, false);
        go.AddComponent<Image>().color = new Color(0.25f, 0.25f, 0.35f);
        var btn = go.AddComponent<Button>();
        var rt  = go.GetComponent<RectTransform>();
        rt.sizeDelta       = new Vector2(w, h);
        rt.anchoredPosition = new Vector2(x, y);

        var lGO = new GameObject("Label");
        lGO.transform.SetParent(go.transform, false);
        var tmp = lGO.AddComponent<TextMeshProUGUI>();
        tmp.text = label; tmp.fontSize = 20;
        tmp.color = Color.white; tmp.alignment = TextAlignmentOptions.Center;
        tmp.enableWordWrapping = false;
        var lrt = lGO.GetComponent<RectTransform>();
        lrt.anchorMin = Vector2.zero; lrt.anchorMax = Vector2.one;
        lrt.offsetMin = new Vector2(4, 4); lrt.offsetMax = new Vector2(-4, -4);

        return btn;
    }

    static void AssignArray<T>(SerializedObject so, string propName, T[] arr) where T : Object
    {
        var prop = so.FindProperty(propName);
        prop.arraySize = arr.Length;
        for (int i = 0; i < arr.Length; i++)
            prop.GetArrayElementAtIndex(i).objectReferenceValue = arr[i];
    }
}
#endif
