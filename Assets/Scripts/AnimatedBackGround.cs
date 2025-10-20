using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BackgroundType{Blue, Brown, Gray, Green, Pink, Purple, Yellow}

public class AnimatedBackGround
    : MonoBehaviour
{
    [SerializeField] private Vector2 movementDirection;
    private MeshRenderer mesh;

    [Header("Color")]
    [SerializeField] private BackgroundType backgroundType;
    [SerializeField] private Texture2D[] texture;

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
        UpdateBackGroundTextTure();
    }

    private void Update()
    {
        mesh.material.mainTextureOffset += movementDirection * Time.deltaTime;
    }

    [ContextMenu("Update Background")]
    private void UpdateBackGroundTextTure()
    {
        if(mesh == null) mesh = GetComponent<MeshRenderer>();
        mesh.sharedMaterial.mainTexture = texture[(int)backgroundType] ;
    }
}
