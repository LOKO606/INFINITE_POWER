using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    [System.Serializable]
    public struct MateriaisCores
    {
        public Material material;
        [ColorUsage(true, true)]
        public Color defaultColor;
        [ColorUsage(true, true)]
        public Color invertedColor;
    }
    public MateriaisCores[] MateriaisParaAlterar;

    public void SetInvert()
    {
        for (int i = 0; i < MateriaisParaAlterar.Length; i++)
        {
            MateriaisParaAlterar[i].material.SetColor("_EmissionColor", MateriaisParaAlterar[i].invertedColor);
        }
    }
    public void SetDefault()
    {
        for (int i = 0; i < MateriaisParaAlterar.Length; i++)
        {
            MateriaisParaAlterar[i].material.SetColor("_EmissionColor", MateriaisParaAlterar[i].defaultColor);
        }
    }

}
