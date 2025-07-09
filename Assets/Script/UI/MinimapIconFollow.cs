using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapIconFollow : MonoBehaviour
{
    [Header("Minimap Settings")]
    [SerializeField] private RectTransform minimapIcon; // Icon inside minimap RawImage
    [SerializeField] private Camera minimapCamera;
    private Transform targetToFollow; // e.g., Player

    [Header("Pointer Blink Settings")]
    [SerializeField] private List<Image> pointers = new List<Image>();
    [SerializeField] private float blinkInterval = 0.5f;
    [SerializeField] private float totalBlinkDuration = 3f;
    private Image targetImage;
    private Coroutine blinkCoroutine;

    void Update()
    {
        if (targetToFollow == null) return;
        Vector3 viewportPos = minimapCamera.WorldToViewportPoint(targetToFollow.position);

        // Convert viewport to minimap rect size
        Vector2 minimapSize = ((RectTransform)minimapIcon.parent).sizeDelta;
        minimapIcon.anchoredPosition = new Vector2(
            (viewportPos.x - 0.5f) * minimapSize.x,
            (viewportPos.y - 0.5f) * minimapSize.y
        );

        // Optional: rotate icon to match player's rotation
        minimapIcon.localEulerAngles = new Vector3(0, 0, -targetToFollow.eulerAngles.y);
    }


    public void SetTarget(Transform target)
    {
        targetToFollow = target;
    }

    public void StartBlink(int pointerIndex)
    {
        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);

        targetImage = pointers[pointerIndex];
        blinkCoroutine = StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < totalBlinkDuration)
        {
            if (targetImage != null)
                targetImage.enabled = !targetImage.enabled;
            GameService.Instance.audioManager.PlayOneShotAt(GameAudioType.Emergency, targetImage.transform.position);
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval;
        }

        targetImage.enabled = false; // Ensure the image is hidden after blinking
        blinkCoroutine = null; // Reset coroutine reference
        targetImage = null; // Reset target image reference

        // Ensure it stays visible after blinking
        //if (targetImage != null)
        // targetImage.enabled = true;
    }
}

