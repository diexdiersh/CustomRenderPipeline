using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
    using UnityEngine.Rendering;

public partial class CameraRenderer
{
    private partial void DrawUnsupportedShaders();
    private partial void DrawGizmos();

#if UNITY_EDITOR
    static ShaderTagId[] legacyShaderTagIDs = new ShaderTagId[]
    {
        new ShaderTagId("Always"),
        new ShaderTagId("ForwardBase"),
        new ShaderTagId("PrepassBase"),
        new ShaderTagId("Vertex"),
        new ShaderTagId("VertexLMRGMB"),
        new ShaderTagId("VertexLM")
    };
    static Material errorMaterial;

    private partial void DrawUnsupportedShaders()
    {
        if (errorMaterial == null)
        {
            errorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));
        }
        var drawingSettings = new DrawingSettings
            (legacyShaderTagIDs[0], new SortingSettings(camera))
        { overrideMaterial = errorMaterial};
        for (int i = 1; i < legacyShaderTagIDs.Length; i++)
        {
            drawingSettings.SetShaderPassName(i, legacyShaderTagIDs[i]);
        }

        var filteringSettings = FilteringSettings.defaultValue;
        context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
    }

    private partial void DrawGizmos()
    {
        if (Handles.ShouldRenderGizmos())
        {
            context.DrawGizmos(camera, GizmoSubset.PreImageEffects);
            context.DrawGizmos(camera, GizmoSubset.PostImageEffects);
        }
    }
#endif
}
