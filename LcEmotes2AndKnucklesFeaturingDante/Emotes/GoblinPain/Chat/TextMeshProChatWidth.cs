using TMPro;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class TextMeshProChatWidth : AbstractChatWidth
{
    public TextMeshPro? label;
    public RectTransform? rectTransform;
    
    public override float GetWidth()
    {
        return TextWidthApproximation() / 10f;
    }

    public override void SetXPos(float x)
    {
        if (rectTransform is null)
            return;

        var anchoredPos = new Vector2(x, rectTransform.anchoredPosition.y);
        rectTransform.anchoredPosition = anchoredPos;
    }

    public override void UpdateState()
    {
        UpdateWidth();
    }

    private void Awake()
    {
        if (rectTransform is null)
            rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        if (rectTransform is null)
            rectTransform = GetComponent<RectTransform>();
        
        UpdateWidth();
    }

    public void UpdateWidth()
    {
        if (rectTransform is null)
            return;
        
        var sizeDelta = new Vector2(GetWidth(), rectTransform.sizeDelta.y);
        rectTransform.sizeDelta = sizeDelta;
    }

    // Below method is thanks to: https://forum.unity.com/threads/calculate-width-of-a-text-before-without-assigning-it-to-a-tmp-object.758867/#post-5057900
    private float TextWidthApproximation()
    {
        if (label is null)
            return 0;

        var text = label.text;

        var fontSize = label.fontSize;
        TMP_FontAsset fontAsset = label.font;
        FontStyles style = label.fontStyle;

        // Compute scale of the target point size relative to the sampling point size of the font asset.
        float pointSizeScale = fontSize / (fontAsset.faceInfo.pointSize * fontAsset.faceInfo.scale);
        float emScale = fontSize * 0.01f;

        float styleSpacingAdjustment = (style & FontStyles.Bold) == FontStyles.Bold ? fontAsset.boldSpacing : 0;
        float normalSpacingAdjustment = fontAsset.normalSpacingOffset;

        float width = 0;

        foreach (var unicode in text)
        {
            // Make sure the given unicode exists in the font asset.
            if (fontAsset.characterLookupTable.TryGetValue(unicode, out var character))
                width += character.glyph.metrics.horizontalAdvance * pointSizeScale + (styleSpacingAdjustment + normalSpacingAdjustment) * emScale;
        }

        return width;
    }
}