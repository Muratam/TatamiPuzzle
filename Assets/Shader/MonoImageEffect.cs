using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MonoImageEffect : MonoBehaviour {
    private Material m_Material;
    [SerializeField] Material material;

    protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Graphics.Blit(source, destination, material);
    }
}
