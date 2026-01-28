using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Direct2D;
using DWrite;
using GlobalStructures;
using static GlobalStructures.GlobalTools;

// From ChatGPT, only works with DWrite

sealed class GeometryTextRenderer : IDWriteTextRenderer, IDWritePixelSnapping
{
    private ID2D1Factory _d2dFactory;
    public List<ID2D1Geometry> GlyphGeometries { get; } = new();

    public GeometryTextRenderer(ID2D1Factory factory) => _d2dFactory = factory;

    HRESULT IDWriteTextRenderer.DrawGlyphRun(
        IntPtr clientDrawingContext,
        float baselineOriginX,
        float baselineOriginY,
        DWrite.DWRITE_MEASURING_MODE measuringMode,
        ref DWRITE_GLYPH_RUN glyphRun,
        IntPtr glyphRunDescription,
        IntPtr clientDrawingEffect)
    {
        HRESULT hr = HRESULT.S_OK;
        var fontFace = Marshal.GetObjectForIUnknown(glyphRun.fontFace) as IDWriteFontFace;
        if (fontFace == null) return HRESULT.E_FAIL;

        int glyphCount = (int)glyphRun.glyphCount;

        ushort[] glyphIndices = new ushort[glyphCount];
        Marshal.Copy(glyphRun.glyphIndices, (short[])(object)glyphIndices, 0, glyphCount);

        float[] glyphAdvances = new float[glyphCount];
        for (int i = 0; i < glyphCount; i++)
            glyphAdvances[i] = Marshal.PtrToStructure<float>(IntPtr.Add(glyphRun.glyphAdvances, i * sizeof(float)));

        DWRITE_GLYPH_OFFSET[] glyphOffsets = new DWRITE_GLYPH_OFFSET[glyphCount];
        for (int i = 0; i < glyphCount; i++)
            glyphOffsets[i] = Marshal.PtrToStructure<DWRITE_GLYPH_OFFSET>(
                IntPtr.Add(glyphRun.glyphOffsets, i * Marshal.SizeOf<DWRITE_GLYPH_OFFSET>()));

        // Create a path geometry for this glyph run
        hr = _d2dFactory.CreatePathGeometry(out ID2D1PathGeometry path);
        if (hr != HRESULT.S_OK) return hr;

        hr = path.Open(out ID2D1GeometrySink sink);
        if (hr != HRESULT.S_OK) { SafeRelease(ref path); return hr; }

        // Fill sink with proper shaped glyph outlines ---
        fontFace.GetGlyphRunOutline(
            glyphRun.fontEmSize,
            glyphIndices,
            glyphAdvances,
            glyphOffsets,
            glyphCount,
            glyphRun.isSideways,
            (glyphRun.bidiLevel & 1) != 0,
            (DWrite.ID2D1SimplifiedGeometrySink)sink
        );

        sink.Close();

        D2D1_MATRIX_3X2_F mat = new D2D1_MATRIX_3X2_F
        {
            _11 = 1,
            _22 = 1,
            _12 = 0,
            _21 = 0,
            _31 = baselineOriginX,
            _32 = baselineOriginY
        };

        hr = _d2dFactory.CreateTransformedGeometry(path, mat, out ID2D1TransformedGeometry transformed);
        SafeRelease(ref path);

        if (hr == HRESULT.S_OK)
            GlyphGeometries.Add(transformed);

        return hr;
    }
    
    HRESULT IDWriteTextRenderer.DrawUnderline(IntPtr c, float x, float y, DWRITE_UNDERLINE u, IntPtr e) => HRESULT.S_OK;
    HRESULT IDWriteTextRenderer.DrawStrikethrough(IntPtr c, float x, float y, DWRITE_STRIKETHROUGH s, IntPtr e) => HRESULT.S_OK;
    HRESULT IDWriteTextRenderer.DrawInlineObject(IntPtr c, float x, float y, IDWriteInlineObject o, bool sw, bool rtl, IntPtr e) => HRESULT.S_OK;
    HRESULT IDWriteTextRenderer.IsPixelSnappingDisabled(IntPtr c, out bool d) { d = false; return HRESULT.S_OK; }
    HRESULT IDWriteTextRenderer.GetCurrentTransform(IntPtr c, out DWRITE_MATRIX t) { t = new DWRITE_MATRIX { m11 = 1, m22 = 1 }; return HRESULT.S_OK; }
    HRESULT IDWriteTextRenderer.GetPixelsPerDip(IntPtr c, out float p) { p = 1.0f; return HRESULT.S_OK; }

    HRESULT IDWritePixelSnapping.IsPixelSnappingDisabled(IntPtr c, out bool d) { d = false; return HRESULT.S_OK; }
    HRESULT IDWritePixelSnapping.GetCurrentTransform(IntPtr c, out DWRITE_MATRIX t) { t = new DWRITE_MATRIX { m11 = 1, m22 = 1 }; return HRESULT.S_OK; }
    HRESULT IDWritePixelSnapping.GetPixelsPerDip(IntPtr c, out float p) { p = 1.0f; return HRESULT.S_OK; }
}

sealed class CollectGlyphRunsRenderer : IDWriteTextRenderer, IDWritePixelSnapping
{
    public struct Run
    {
        public DWRITE_GLYPH_RUN GlyphRun;
        public float X;
        public float Y;
    }

    public readonly List<Run> Runs = new();

    HRESULT IDWriteTextRenderer.DrawGlyphRun(
        IntPtr ctx,
        float x,
        float y,
        DWrite.DWRITE_MEASURING_MODE mode,
        ref DWRITE_GLYPH_RUN run,
        IntPtr desc,
        IntPtr effect)
    {
        Runs.Add(new Run
        {
            GlyphRun = run,
            X = x,
            Y = y
        });
        return HRESULT.S_OK;
    }

    // boilerplate
    HRESULT IDWriteTextRenderer.DrawUnderline(IntPtr clientDrawingContext, float baselineOriginX, float baselineOriginY, DWRITE_UNDERLINE underline, IntPtr clientDrawingEffect) => HRESULT.S_OK;
    HRESULT IDWriteTextRenderer.DrawStrikethrough(IntPtr clientDrawingContext, float baselineOriginX, float baselineOriginY, DWRITE_STRIKETHROUGH strikethrough, IntPtr clientDrawingEffect) => HRESULT.S_OK;
    HRESULT IDWriteTextRenderer.DrawInlineObject(IntPtr clientDrawingContext, float originX, float originY, IDWriteInlineObject inlineObject, bool isSideways, bool isRightToLeft, IntPtr clientDrawingEffect) => HRESULT.S_OK;
    HRESULT IDWriteTextRenderer.IsPixelSnappingDisabled(IntPtr c, out bool d) { d = false; return HRESULT.S_OK; }
    HRESULT IDWriteTextRenderer.GetCurrentTransform(IntPtr c, out DWRITE_MATRIX t) { t = new DWRITE_MATRIX { m11 = 1, m22 = 1 }; return HRESULT.S_OK; }
    HRESULT IDWriteTextRenderer.GetPixelsPerDip(IntPtr c, out float p) { p = 1; return HRESULT.S_OK; }
    HRESULT IDWritePixelSnapping.IsPixelSnappingDisabled(IntPtr c, out bool d) { d = false; return HRESULT.S_OK; }
    HRESULT IDWritePixelSnapping.GetCurrentTransform(IntPtr c, out DWRITE_MATRIX t) { t = new DWRITE_MATRIX { m11 = 1, m22 = 1 }; return HRESULT.S_OK; }
    HRESULT IDWritePixelSnapping.GetPixelsPerDip(IntPtr c, out float p) { p = 1; return HRESULT.S_OK; }
}

sealed class OutlineTextRenderer : IDWriteTextRenderer, IDWritePixelSnapping
{
    private readonly ID2D1Factory _factory;
    private readonly ID2D1GeometrySink _sink;

    public OutlineTextRenderer(ID2D1Factory factory, ID2D1GeometrySink sink)
    {
        _factory = factory;
        _sink = sink;
    }

    HRESULT IDWriteTextRenderer.DrawGlyphRun(
        IntPtr clientDrawingContext,
        float baselineOriginX,
        float baselineOriginY,
        DWrite.DWRITE_MEASURING_MODE measuringMode,
        ref DWRITE_GLYPH_RUN glyphRun,
        IntPtr glyphRunDescription,
        IntPtr clientDrawingEffect)
    {
        // Get shaped font face
        var fontFace = (IDWriteFontFace)Marshal.GetObjectForIUnknown(glyphRun.fontFace);

        int glyphCount = (int)glyphRun.glyphCount;

        // Copy glyph indices
        ushort[] glyphIndices = new ushort[glyphCount];
        for (int i = 0; i < glyphCount; i++)
        {
            glyphIndices[i] = (ushort)Marshal.ReadInt16(
                glyphRun.glyphIndices, i * sizeof(short));
        }

        // Copy advances
        float[] glyphAdvances = new float[glyphCount];
        for (int i = 0; i < glyphCount; i++)
        {
            glyphAdvances[i] = Marshal.PtrToStructure<float>(
                IntPtr.Add(glyphRun.glyphAdvances, i * sizeof(float)));
        }

        // Copy offsets
        DWRITE_GLYPH_OFFSET[] glyphOffsets = new DWRITE_GLYPH_OFFSET[glyphCount];
        for (int i = 0; i < glyphCount; i++)
        {
            glyphOffsets[i] = Marshal.PtrToStructure<DWRITE_GLYPH_OFFSET>(
                IntPtr.Add(glyphRun.glyphOffsets, i * Marshal.SizeOf<DWRITE_GLYPH_OFFSET>()));
        }
        
        var transform = new D2D1_MATRIX_3X2_F
        {
            _11 = 1,
            _12 = 0,
            _21 = 0,
            _22 = 1,
            _31 = baselineOriginX,
            _32 = baselineOriginY
        };

        // Save existing sink transform
        _sink.SetFillMode(Direct2D.D2D1_FILL_MODE.D2D1_FILL_MODE_WINDING);

        // Append the run outline
        fontFace.GetGlyphRunOutline(
            glyphRun.fontEmSize,
            glyphIndices,
            glyphAdvances,
            glyphOffsets,
            glyphCount,
            glyphRun.isSideways,
            (glyphRun.bidiLevel & 1) != 0,
            (DWrite.ID2D1SimplifiedGeometrySink)_sink);

        return HRESULT.S_OK;
    }

    HRESULT IDWriteTextRenderer.DrawUnderline(IntPtr clientDrawingContext, float baselineOriginX, float baselineOriginY, DWRITE_UNDERLINE underline, IntPtr clientDrawingEffect) => HRESULT.S_OK;
    HRESULT IDWriteTextRenderer.DrawStrikethrough(IntPtr clientDrawingContext, float baselineOriginX, float baselineOriginY, DWRITE_STRIKETHROUGH strikethrough, IntPtr clientDrawingEffect) => HRESULT.S_OK;
    HRESULT IDWriteTextRenderer.DrawInlineObject(IntPtr clientDrawingContext, float originX, float originY, IDWriteInlineObject inlineObject, bool isSideways, bool isRightToLeft, IntPtr clientDrawingEffect) => HRESULT.S_OK;

    HRESULT IDWriteTextRenderer.IsPixelSnappingDisabled(IntPtr clientDrawingContext, out bool isDisabled)
    {
        isDisabled = false; return HRESULT.S_OK;
    }

    HRESULT IDWriteTextRenderer.GetCurrentTransform(IntPtr clientDrawingContext, out DWRITE_MATRIX transform)
    {
        transform = new DWRITE_MATRIX
        {
            m11 = 1,
            m12 = 0,
            m21 = 0,
            m22 = 1,
            dx = 0,
            dy = 0
        };
        return HRESULT.S_OK;
    }

    HRESULT IDWriteTextRenderer.GetPixelsPerDip(IntPtr clientDrawingContext, out float pixelsPerDip)
    {
        pixelsPerDip = 1.0f; return HRESULT.S_OK;
    }

    HRESULT IDWritePixelSnapping.IsPixelSnappingDisabled(IntPtr clientDrawingContext, out bool isDisabled)
    {
        isDisabled = false; return HRESULT.S_OK;
    }

    HRESULT IDWritePixelSnapping.GetCurrentTransform(IntPtr clientDrawingContext, out DWRITE_MATRIX transform)
    {
        transform = new DWRITE_MATRIX
        {
            m11 = 1,
            m12 = 0,
            m21 = 0,
            m22 = 1,
            dx = 0,
            dy = 0
        };
        return HRESULT.S_OK;
    }

    HRESULT IDWritePixelSnapping.GetPixelsPerDip(IntPtr clientDrawingContext, out float pixelsPerDip)
    {
        pixelsPerDip = 1.0f; return HRESULT.S_OK;
    }
}



