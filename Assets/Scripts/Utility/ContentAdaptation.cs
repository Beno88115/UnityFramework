using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ContentAdaptation : MonoBehaviour
{
    public int LineCharacterCount = 57;

    private Text Content { get; set; }

    private RectTransform RectContent { get; set; }

    private RectTransform RectBg { get; set; }

    private float DifferY { get; set; }

    private float DifferX { get; set; }

    private ASCIIEncoding ASCIIEncoding { get; set; }

    private Vector2 Offset { get; set; }

    private void Awake()
    {
        this.RectBg = this.GetComponent<RectTransform>();
        this.Content = this.GetComponentInChildren<Text>();
        this.RectContent = this.Content.GetComponent<RectTransform>();
        this.Offset = this.RectContent.rect.size;
        this.DifferY = this.RectBg.rect.size.y - this.Content.fontSize;// this.RectContent.rect.size.y;
        this.DifferX = this.RectBg.rect.size.x - this.RectContent.rect.size.x;
        this.ASCIIEncoding = new ASCIIEncoding();
    }

    public void SetText(string str)
    {
        //int length = System.Text.Encoding.Default.GetBytes(str).Length;
        int length = this.GetStringLength(str);
        TextGenerator generator = new TextGenerator();
        TextGenerationSettings settings = this.Content.GetGenerationSettings(this.RectContent.rect.size);
        float width = generator.GetPreferredWidth(str, settings);
        float perWidth = Mathf.Floor(width / length);
        if (length > this.LineCharacterCount)
        {
            width = perWidth * this.LineCharacterCount;
        }

        this.RectBg.sizeDelta = new Vector2(width + this.DifferX, this.RectBg.sizeDelta.y);
        settings = this.Content.GetGenerationSettings(this.RectContent.rect.size);
        float height = generator.GetPreferredHeight(str, settings);
        if (height < this.Content.fontSize)
        {
            height = this.Content.fontSize;
        }

        this.RectBg.sizeDelta = new Vector2(this.RectBg.sizeDelta.x, height + this.DifferY);
        this.Content.text = str;
    }

    private int GetStringLength(string str)
    {
        int length = 0;
        byte[] s = this.ASCIIEncoding.GetBytes(str);
        for (int i = 0; i < s.Length; i++)
        {
            if ((int)s[i] == 63)
            {
                length += 2;
            }
            else
            {
                length += 1;
            }
        }

        return length;
    }
}
