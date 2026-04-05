using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages tutorial visual effects: glow (flashing sprites), arrows, and key hints.
/// Listens for events from Dialogue.cs and applies effects to game objects.
/// Attach to a persistent GameObject in the scene.
/// </summary>
public class TutorialVisualManager : MonoBehaviour
{
    [Header("Arrow Settings")]
    public Color arrowColor = Color.yellow;
    public float arrowFloatSpeed = 2f;
    public float arrowFloatAmount = 0.15f;

    [Header("Glow Settings")]
    public float glowSpeed = 2f;
    public float glowMinAlpha = 0.3f;

    // Active effects tracking
    private List<GameObject> activeArrows = new List<GameObject>();
    private List<Coroutine> activeGlowCoroutines = new List<Coroutine>();
    private List<SpriteRenderer> glowingSprites = new List<SpriteRenderer>();

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener("TutorialGlow", OnGlow);
        EventCenter.Instance.AddEventListener("TutorialArrow", OnArrow);
        EventCenter.Instance.AddEventListener("TutorialClear", OnClear);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener("TutorialGlow", OnGlow);
        EventCenter.Instance.RemoveEventListener("TutorialArrow", OnArrow);
        EventCenter.Instance.RemoveEventListener("TutorialClear", OnClear);
    }

    #region Glow Effect

    private void OnGlow(object arg0)
    {
        string target = arg0 as string;
        if (string.IsNullOrEmpty(target)) return;

        // Find target objects by tag or name pattern
        GameObject[] targets = FindTargetObjects(target);

        foreach (GameObject go in targets)
        {
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                glowingSprites.Add(sr);
                Coroutine c = StartCoroutine(GlowCoroutine(sr));
                activeGlowCoroutines.Add(c);
            }
        }
    }

    private IEnumerator GlowCoroutine(SpriteRenderer sr)
    {
        Color originalColor = sr.color;
        float time = 0f;

        while (true)
        {
            if (sr == null) yield break;

            time += Time.deltaTime * glowSpeed;
            float alpha = Mathf.Lerp(glowMinAlpha, 1f, (Mathf.Sin(time) + 1f) / 2f);
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            sr.color = newColor;
            yield return null;
        }
    }

    #endregion

    #region Arrow Effect

    private void OnArrow(object arg0)
    {
        string direction = arg0 as string;
        if (string.IsNullOrEmpty(direction)) return;

        // Find player character
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            // Fallback: find by name
            CharacterViewItem character = FindObjectOfType<CharacterViewItem>();
            if (character != null)
                player = character.gameObject;
        }

        if (player == null) return;

        Vector3 offset = GetDirectionOffset(direction);
        Vector3 arrowPos = player.transform.position + offset;

        GameObject arrow = CreateArrowObject(arrowPos, direction);
        activeArrows.Add(arrow);
        StartCoroutine(ArrowFloatCoroutine(arrow, direction));
    }

    private Vector3 GetDirectionOffset(string direction)
    {
        switch (direction.ToLower())
        {
            case "up": return new Vector3(0, 1.2f, 0);
            case "down": return new Vector3(0, -1.2f, 0);
            case "left": return new Vector3(-1.2f, 0, 0);
            case "right": return new Vector3(1.2f, 0, 0);
            default: return new Vector3(0, 1.2f, 0);
        }
    }

    private float GetArrowRotation(string direction)
    {
        switch (direction.ToLower())
        {
            case "up": return 0f;
            case "down": return 180f;
            case "left": return 90f;
            case "right": return -90f;
            default: return 0f;
        }
    }

    private GameObject CreateArrowObject(Vector3 position, string direction)
    {
        // Create a simple triangle arrow using a sprite
        GameObject arrow = new GameObject("TutorialArrow");
        arrow.transform.position = position;
        arrow.transform.rotation = Quaternion.Euler(0, 0, GetArrowRotation(direction));

        SpriteRenderer sr = arrow.AddComponent<SpriteRenderer>();
        sr.sprite = CreateTriangleSprite();
        sr.color = arrowColor;
        sr.sortingOrder = 100;

        return arrow;
    }

    private Sprite CreateTriangleSprite()
    {
        // Create a simple 32x32 triangle texture
        int size = 32;
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Point;

        Color transparent = new Color(0, 0, 0, 0);

        // Fill transparent
        for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y++)
                tex.SetPixel(x, y, transparent);

        // Draw upward-pointing triangle
        for (int y = 0; y < size; y++)
        {
            float progress = (float)y / size;
            int halfWidth = Mathf.RoundToInt((1f - progress) * size / 2f);
            int center = size / 2;

            for (int x = center - halfWidth; x <= center + halfWidth; x++)
            {
                if (x >= 0 && x < size)
                    tex.SetPixel(x, y, Color.white);
            }
        }

        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 32f);
    }

    private IEnumerator ArrowFloatCoroutine(GameObject arrow, string direction)
    {
        if (arrow == null) yield break;

        Vector3 basePos = arrow.transform.position;
        float time = 0f;

        // Float axis depends on direction
        bool floatVertical = (direction == "left" || direction == "right");

        while (arrow != null)
        {
            time += Time.deltaTime * arrowFloatSpeed;
            float offset = Mathf.Sin(time) * arrowFloatAmount;

            if (floatVertical)
                arrow.transform.position = basePos + new Vector3(0, offset, 0);
            else
                arrow.transform.position = basePos + new Vector3(offset, 0, 0);

            yield return null;
        }
    }

    #endregion

    #region Find Targets

    private GameObject[] FindTargetObjects(string target)
    {
        List<GameObject> results = new List<GameObject>();

        switch (target.ToLower())
        {
            case "crystal":
                // Find all crystal objects in current map
                AddObjectsByType<UnitViewItem>(results, "Crystal");
                break;
            case "exit":
                AddObjectsByType<ChangeScenceViewItem>(results);
                break;
            case "button":
                AddObjectsByType<ButtonViewItem>(results);
                break;
            case "ice":
                AddObjectsByType<IceViewItem>(results);
                break;
            case "spike":
            case "spikes":
                AddObjectsByName(results, "Spike");
                break;
            case "trap":
            case "traps":
                AddObjectsByType<TrapsViemItem>(results);
                break;
            case "box":
                AddObjectsByName(results, "Box");
                break;
            case "door":
                AddObjectsByName(results, "Door");
                break;
            default:
                AddObjectsByName(results, target);
                break;
        }

        return results.ToArray();
    }

    private void AddObjectsByType<T>(List<GameObject> results, string nameFilter = null) where T : MonoBehaviour
    {
        T[] objects = FindObjectsOfType<T>();
        foreach (T obj in objects)
        {
            if (nameFilter == null || obj.gameObject.name.Contains(nameFilter))
                results.Add(obj.gameObject);
        }
    }

    private void AddObjectsByName(List<GameObject> results, string nameContains)
    {
        // Search within the map container
        MapMgr map = FindObjectOfType<MapMgr>();
        if (map == null) return;

        foreach (Transform child in map.transform)
        {
            if (child.name.ToLower().Contains(nameContains.ToLower()))
                results.Add(child.gameObject);
        }
    }

    #endregion

    #region Clear

    private void OnClear(object arg0)
    {
        ClearAll();
    }

    private void ClearAll()
    {
        // Stop glow coroutines and restore colors
        foreach (Coroutine c in activeGlowCoroutines)
        {
            if (c != null) StopCoroutine(c);
        }
        activeGlowCoroutines.Clear();

        foreach (SpriteRenderer sr in glowingSprites)
        {
            if (sr != null)
            {
                Color c = sr.color;
                sr.color = new Color(c.r, c.g, c.b, 1f);
            }
        }
        glowingSprites.Clear();

        // Destroy arrows
        foreach (GameObject arrow in activeArrows)
        {
            if (arrow != null) Destroy(arrow);
        }
        activeArrows.Clear();
    }

    private void OnDestroy()
    {
        ClearAll();
    }

    #endregion
}
