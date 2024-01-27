using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShader : MonoBehaviour
{
    public Shader screenShader = null;
    private Material m_renderMaterial;
    // Start is called before the first frame update
    void Start()
    {
        if (screenShader == null)
        {
            Debug.LogError("no awesome shader.");
            m_renderMaterial = null;
            return;
        }
        m_renderMaterial = new Material(screenShader);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, m_renderMaterial);
        }
}
