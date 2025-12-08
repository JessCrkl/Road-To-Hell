using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GateController : MonoBehaviour
{
    [Header("Key Requirements")]
    public bool requiresCerberusKey = true;
    public string cerberusKeyName = "Cerberus Key"; 

    [Header("Gate / UI")]
    public GameObject gateVisual;
    public Animator gateAnimator;  
    public CanvasGroup endScreenCanvas;
    public float fadeDuration = 2f;
    public string mainMenuSceneName = "Menu";

    private bool gateOpened = false;

    private void OnTriggerEnter(Collider other)
    {
        if (gateOpened) return;
        if (!other.CompareTag("Player")) return;

        if (CanOpenGate())
        {
            StartCoroutine(OpenGateAndEnd());
        }
        else
        {
            Debug.Log("The gate is sealed. You need Cerberus's key.");
        }
    }

    private bool CanOpenGate()
    {
        if (PlayerStats.Instance == null) return false;

        if (!requiresCerberusKey)
            return true;

        // Simplest: any key
        return PlayerStats.Instance.keyCount > 0;
        // If you track specific keys by name, check that here instead.
    }

    private IEnumerator OpenGateAndEnd()
    {
        gateOpened = true;
        Debug.Log("Opening gate...");

        if (gateAnimator != null)
        {
            gateAnimator.SetTrigger("Open");
        }
        else if (gateVisual != null)
        {
            gateVisual.SetActive(false);
        }

        // Small delay before fade
        yield return new WaitForSeconds(1f);

        if (endScreenCanvas != null)
        {
            endScreenCanvas.gameObject.SetActive(true);
            endScreenCanvas.alpha = 0f;

            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                endScreenCanvas.alpha = Mathf.Clamp01(t / fadeDuration);
                yield return null;
            }

            // Here you can play your "title" + "about/credits" animation/text
            yield return new WaitForSeconds(3f);
        }

        // back to main menu
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
