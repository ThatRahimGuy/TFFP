using UnityEngine;
using UnityEngine.UI;
public class creditsScript : MonoBehaviour
{
    public float scrollSpeed = 60f;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
    }
}
