using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

public partial class CameraRenderer
{
    private partial void DrawUnsupportedShaders();
    private partial void DrawGizmos();
    private partial void PrepareForSceneWindow();
    private partial void PrepareBuffer();

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

    private partial void PrepareForSceneWindow()
    {
        if (camera.cameraType == CameraType.SceneView)
        {
            ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
        }
    }

    private string SampleName { get; set; }
    private partial void PrepareBuffer()
    {
        Profiler.BeginSample("Editor Only");
        buffer.name = SampleName = camera.name;
        Profiler.EndSample();
    }
#else

    private const string SampleName = bufferName;

#endif
}
