using System;
using System.Runtime.InteropServices;
using GlobalStructures;

//C: \Users\Christian\.nuget\packages\microsoft.windowsappsdk\1.5.240627000\include
//dwrite_core.h

namespace DWrite
{ 
    internal class DWriteTools
    {
        // C:\Program Files\WindowsApps\Microsoft.WindowsAppRuntime.1.5_5001.178.1908.0_x86__8wekyb3d8bbwe
        // C:\Program Files\WindowsApps\Microsoft.WindowsAppRuntime.1.5_5001.178.1908.0_x64__8wekyb3d8bbwe
        [DllImport("DWriteCore.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern HRESULT DWriteCoreCreateFactory(DWRITE_FACTORY_TYPE factoryType, ref Guid iid, out IntPtr factory);

        [DllImport("DWrite.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern HRESULT DWriteCreateFactory(DWRITE_FACTORY_TYPE factoryType, ref Guid iid, out IntPtr factory);

        public static Guid CLSID_DWriteFactory = new Guid("B859EE5A-D838-4B5B-A2E8-1ADC7D93DB48");
        public static Guid CLSID_DWriteFactory1 = new Guid("30572f99-dac6-41db-a16e-0486307e606a");
        public static Guid CLSID_DWriteFactory2 = new Guid("0439fc60-ca44-4994-8dee-3a9af7b732ec");
        public static Guid CLSID_DWriteFactory3 = new Guid("9A1B41C3-D3BB-466A-87FC-FE67556A3B65");
        public static Guid CLSID_DWriteFactory4 = new Guid("4B0B5BD3-0797-4549-8AC5-FE915CC53856");
        public static Guid CLSID_DWriteFactory5 = new Guid("958DB99A-BE2A-4F09-AF7D-65189803D1D3");
        public static Guid CLSID_DWriteFactory6 = new Guid("F3744D80-21F7-42EB-B35D-995BC72FC223");
        public static Guid CLSID_DWriteFactory7 = new Guid("35D0E0B3-9076-4D2E-A016-A91B568A06B4");

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SIZE
    {
        public int cx;
        public int cy;
        public SIZE(int cx, int cy)
        {
            this.cx = cx;
            this.cy = cy;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class LOGFONT
    {
        public int lfHeight = 0;
        public int lfWidth = 0;
        public int lfEscapement = 0;
        public int lfOrientation = 0;
        public int lfWeight = 0;
        public byte lfItalic = 0;
        public byte lfUnderline = 0;
        public byte lfStrikeOut = 0;
        public byte lfCharSet = 0;
        public byte lfOutPrecision = 0;
        public byte lfClipPrecision = 0;
        public byte lfQuality = 0;
        public byte lfPitchAndFamily = 0;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string lfFaceName = string.Empty;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct D2D1_POINT_2F
    {
        public float x;
        public float y;

        public D2D1_POINT_2F(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public enum DWRITE_FACTORY_TYPE
    {
        /// <summary>
        /// Shared factory allow for re-use of cached font data across multiple in process components.
        /// Such factories also take advantage of cross process font caching components for better performance.
        /// </summary>
        DWRITE_FACTORY_TYPE_SHARED,
        /// <summary>
        /// Objects created from the isolated factory do not interact with internal DirectWrite state from other components.
        /// </summary>
        DWRITE_FACTORY_TYPE_ISOLATED
    }

    [ComImport]
    [Guid("b859ee5a-d838-4b5b-a2e8-1adc7d93db48")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFactory
    {
        HRESULT GetSystemFontCollection(out IDWriteFontCollection fontCollection, bool checkForUpdates = false);
        [PreserveSig]
        HRESULT CreateCustomFontCollection(IDWriteFontCollectionLoader collectionLoader, IntPtr collectionKey, int collectionKeySize, out IDWriteFontCollection fontCollection);
        [PreserveSig]
        HRESULT RegisterFontCollectionLoader(IDWriteFontCollectionLoader fontCollectionLoader);
        [PreserveSig]
        HRESULT UnregisterFontCollectionLoader(IDWriteFontCollectionLoader fontCollectionLoader);
        //HRESULT CreateFontFileReference(string filePath, System.Runtime.InteropServices.ComTypes.FILETIME lastWriteTime, out IDWriteFontFile fontFile);
        [PreserveSig]
        HRESULT CreateFontFileReference([MarshalAs(UnmanagedType.LPWStr)] string filePath, IntPtr lastWriteTime, out IDWriteFontFile fontFile);
        [PreserveSig]
        HRESULT CreateCustomFontFileReference(IntPtr fontFileReferenceKey, uint fontFileReferenceKeySize, IDWriteFontFileLoader fontFileLoader, out IDWriteFontFile fontFile);
        //HRESULT CreateCustomFontFileReference(IntPtr fontFileReferenceKey, uint fontFileReferenceKeySize,
        //[In()][MarshalAs(UnmanagedType.IUnknown)] object fontFileLoader,
        //[Out, MarshalAs(UnmanagedType.IUnknown)] out IDWriteFontFile fontFile);
        [PreserveSig]
        HRESULT CreateFontFace(DWRITE_FONT_FACE_TYPE fontFaceType, int numberOfFiles, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] IDWriteFontFile[] fontFiles, int faceIndex, DWRITE_FONT_SIMULATIONS fontFaceSimulationFlags, out IDWriteFontFace fontFace);
        [PreserveSig]
        HRESULT CreateRenderingParams(out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        HRESULT CreateMonitorRenderingParams(IntPtr monitor, out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        HRESULT CreateCustomRenderingParams(float gamma, float enhancedContrast, float clearTypeLevel, DWRITE_PIXEL_GEOMETRY pixelGeometry, DWRITE_RENDERING_MODE renderingMode, out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        HRESULT RegisterFontFileLoader(IDWriteFontFileLoader fontFileLoader);
        [PreserveSig]
        HRESULT UnregisterFontFileLoader(IDWriteFontFileLoader fontFileLoader);
        [PreserveSig]
        HRESULT CreateTextFormat([MarshalAs(UnmanagedType.LPWStr)] string fontFamilyName, IDWriteFontCollection fontCollection, DWRITE_FONT_WEIGHT fontWeight, DWRITE_FONT_STYLE fontStyle, DWRITE_FONT_STRETCH fontStretch, float fontSize,
            [MarshalAs(UnmanagedType.LPWStr)] string localeName, out IDWriteTextFormat textFormat);
        [PreserveSig]
        HRESULT CreateTypography(out IDWriteTypography typography);
        [PreserveSig]
        HRESULT GetGdiInterop(out IDWriteGdiInterop gdiInterop);
        [PreserveSig]
        HRESULT CreateTextLayout([MarshalAs(UnmanagedType.LPWStr)] string str, int stringLength, IDWriteTextFormat textFormat, float maxWidth, float maxHeight, out IDWriteTextLayout textLayout);
        [PreserveSig]
        HRESULT CreateGdiCompatibleTextLayout([MarshalAs(UnmanagedType.LPWStr)] string str, int stringLength, IDWriteTextFormat textFormat, float layoutWidth,
            float layoutHeight, float pixelsPerDip, DWRITE_MATRIX transform, bool useGdiNatural, out IDWriteTextLayout textLayout);
        [PreserveSig]
        HRESULT CreateEllipsisTrimmingSign(IDWriteTextFormat textFormat, out IDWriteInlineObject trimmingSign);
        [PreserveSig]
        HRESULT CreateTextAnalyzer(out IDWriteTextAnalyzer textAnalyzer);
        [PreserveSig]
        HRESULT CreateNumberSubstitution(DWRITE_NUMBER_SUBSTITUTION_METHOD substitutionMethod, string localeName, bool ignoreUserOverride, out IDWriteNumberSubstitution numberSubstitution);
        [PreserveSig]
        HRESULT CreateGlyphRunAnalysis(ref DWRITE_GLYPH_RUN glyphRun, float pixelsPerDip, DWRITE_MATRIX transform, DWRITE_RENDERING_MODE renderingMode,
            DWRITE_MEASURING_MODE measuringMode, float baselineOriginX, float baselineOriginY, out IDWriteGlyphRunAnalysis glyphRunAnalysis);
    }

    [ComImport]
    [Guid("30572f99-dac6-41db-a16e-0486307e606a")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFactory1 : IDWriteFactory
    {
        #region IDWriteFactory
        new HRESULT GetSystemFontCollection(out IDWriteFontCollection fontCollection, bool checkForUpdates = false);
        [PreserveSig]
        new HRESULT CreateCustomFontCollection(IDWriteFontCollectionLoader collectionLoader, IntPtr collectionKey, int collectionKeySize, out IDWriteFontCollection fontCollection);
        [PreserveSig]
        new HRESULT RegisterFontCollectionLoader(IDWriteFontCollectionLoader fontCollectionLoader);
        [PreserveSig]
        new HRESULT UnregisterFontCollectionLoader(IDWriteFontCollectionLoader fontCollectionLoader);
        //new HRESULT CreateFontFileReference(string filePath, System.Runtime.InteropServices.ComTypes.FILETIME lastWriteTime, out IDWriteFontFile fontFile);
        [PreserveSig]
        new HRESULT CreateFontFileReference([MarshalAs(UnmanagedType.LPWStr)] string filePath, IntPtr lastWriteTime, out IDWriteFontFile fontFile);
        [PreserveSig]
        new HRESULT CreateCustomFontFileReference(IntPtr fontFileReferenceKey, uint fontFileReferenceKeySize, IDWriteFontFileLoader fontFileLoader, out IDWriteFontFile fontFile);
        [PreserveSig]
        new HRESULT CreateFontFace(DWRITE_FONT_FACE_TYPE fontFaceType, int numberOfFiles, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] IDWriteFontFile[] fontFiles, int faceIndex, DWRITE_FONT_SIMULATIONS fontFaceSimulationFlags, out IDWriteFontFace fontFace);
        [PreserveSig]
        new HRESULT CreateRenderingParams(out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        new HRESULT CreateMonitorRenderingParams(IntPtr monitor, out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        new HRESULT CreateCustomRenderingParams(float gamma, float enhancedContrast, float clearTypeLevel, DWRITE_PIXEL_GEOMETRY pixelGeometry, DWRITE_RENDERING_MODE renderingMode, out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        new HRESULT RegisterFontFileLoader(IDWriteFontFileLoader fontFileLoader);
        [PreserveSig]
        new HRESULT UnregisterFontFileLoader(IDWriteFontFileLoader fontFileLoader);
        [PreserveSig]
        new HRESULT CreateTextFormat([MarshalAs(UnmanagedType.LPWStr)] string fontFamilyName, IDWriteFontCollection fontCollection, DWRITE_FONT_WEIGHT fontWeight, DWRITE_FONT_STYLE fontStyle, DWRITE_FONT_STRETCH fontStretch, float fontSize,
            [MarshalAs(UnmanagedType.LPWStr)] string localeName, out IDWriteTextFormat textFormat);
        [PreserveSig]
        new HRESULT CreateTypography(out IDWriteTypography typography);
        [PreserveSig]
        new HRESULT GetGdiInterop(out IDWriteGdiInterop gdiInterop);
        [PreserveSig]
        new HRESULT CreateTextLayout([MarshalAs(UnmanagedType.LPWStr)] string str, int stringLength, IDWriteTextFormat textFormat, float maxWidth, float maxHeight, out IDWriteTextLayout textLayout);
        [PreserveSig]
        new HRESULT CreateGdiCompatibleTextLayout([MarshalAs(UnmanagedType.LPWStr)] string str, int stringLength, IDWriteTextFormat textFormat, float layoutWidth,
            float layoutHeight, float pixelsPerDip, DWRITE_MATRIX transform, bool useGdiNatural, out IDWriteTextLayout textLayout);
        [PreserveSig]
        new HRESULT CreateEllipsisTrimmingSign(IDWriteTextFormat textFormat, out IDWriteInlineObject trimmingSign);
        [PreserveSig]
        new HRESULT CreateTextAnalyzer(out IDWriteTextAnalyzer textAnalyzer);
        [PreserveSig]
        new HRESULT CreateNumberSubstitution(DWRITE_NUMBER_SUBSTITUTION_METHOD substitutionMethod, string localeName, bool ignoreUserOverride, out IDWriteNumberSubstitution numberSubstitution);
        [PreserveSig]
        new HRESULT CreateGlyphRunAnalysis(ref DWRITE_GLYPH_RUN glyphRun, float pixelsPerDip, DWRITE_MATRIX transform, DWRITE_RENDERING_MODE renderingMode,
            DWRITE_MEASURING_MODE measuringMode, float baselineOriginX, float baselineOriginY, out IDWriteGlyphRunAnalysis glyphRunAnalysis);
        #endregion

        [PreserveSig]
        HRESULT GetEudcFontCollection(out IDWriteFontCollection fontCollection,  bool checkForUpdates = false);
        [PreserveSig]
        HRESULT CreateCustomRenderingParams1(float gamma, float enhancedContrast, float enhancedContrastGrayscale, float clearTypeLevel,
            DWRITE_PIXEL_GEOMETRY pixelGeometry, DWRITE_RENDERING_MODE renderingMode, out IDWriteRenderingParams1 renderingParams);
    }

    [ComImport]
    [Guid("0439fc60-ca44-4994-8dee-3a9af7b732ec")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFactory2 : IDWriteFactory1
    {
        #region IDWriteFactory1
        #region IDWriteFactory
        [PreserveSig]
        new HRESULT GetSystemFontCollection(out IDWriteFontCollection fontCollection, bool checkForUpdates = false);
        [PreserveSig]
        new HRESULT CreateCustomFontCollection(IDWriteFontCollectionLoader collectionLoader, IntPtr collectionKey, int collectionKeySize, out IDWriteFontCollection fontCollection);
        [PreserveSig]
        new HRESULT RegisterFontCollectionLoader(IDWriteFontCollectionLoader fontCollectionLoader);
        [PreserveSig]
        new HRESULT UnregisterFontCollectionLoader(IDWriteFontCollectionLoader fontCollectionLoader);
        //new HRESULT CreateFontFileReference(string filePath, System.Runtime.InteropServices.ComTypes.FILETIME lastWriteTime, out IDWriteFontFile fontFile);
        [PreserveSig]
        new HRESULT CreateFontFileReference([MarshalAs(UnmanagedType.LPWStr)] string filePath, IntPtr lastWriteTime, out IDWriteFontFile fontFile);
        [PreserveSig]
        new HRESULT CreateCustomFontFileReference(IntPtr fontFileReferenceKey, uint fontFileReferenceKeySize, IDWriteFontFileLoader fontFileLoader, out IDWriteFontFile fontFile);
        [PreserveSig]
        new HRESULT CreateFontFace(DWRITE_FONT_FACE_TYPE fontFaceType, int numberOfFiles, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] IDWriteFontFile[] fontFiles, int faceIndex, DWRITE_FONT_SIMULATIONS fontFaceSimulationFlags, out IDWriteFontFace fontFace);
        [PreserveSig]
        new HRESULT CreateRenderingParams(out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        new HRESULT CreateMonitorRenderingParams(IntPtr monitor, out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        new HRESULT CreateCustomRenderingParams(float gamma, float enhancedContrast, float clearTypeLevel, DWRITE_PIXEL_GEOMETRY pixelGeometry, DWRITE_RENDERING_MODE renderingMode, out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        new HRESULT RegisterFontFileLoader(IDWriteFontFileLoader fontFileLoader);
        new HRESULT UnregisterFontFileLoader(IDWriteFontFileLoader fontFileLoader);
        [PreserveSig]
        new HRESULT CreateTextFormat([MarshalAs(UnmanagedType.LPWStr)] string fontFamilyName, IDWriteFontCollection fontCollection, DWRITE_FONT_WEIGHT fontWeight, DWRITE_FONT_STYLE fontStyle, DWRITE_FONT_STRETCH fontStretch, float fontSize,
            [MarshalAs(UnmanagedType.LPWStr)] string localeName, out IDWriteTextFormat textFormat);
        [PreserveSig]
        new HRESULT CreateTypography(out IDWriteTypography typography);
        [PreserveSig]
        new HRESULT GetGdiInterop(out IDWriteGdiInterop gdiInterop);
        [PreserveSig]
        new HRESULT CreateTextLayout([MarshalAs(UnmanagedType.LPWStr)] string str, int stringLength, IDWriteTextFormat textFormat, float maxWidth, float maxHeight, out IDWriteTextLayout textLayout);
        [PreserveSig]
        new HRESULT CreateGdiCompatibleTextLayout([MarshalAs(UnmanagedType.LPWStr)] string str, int stringLength, IDWriteTextFormat textFormat, float layoutWidth,
            float layoutHeight, float pixelsPerDip, DWRITE_MATRIX transform, bool useGdiNatural, out IDWriteTextLayout textLayout);
        [PreserveSig]
        new HRESULT CreateEllipsisTrimmingSign(IDWriteTextFormat textFormat, out IDWriteInlineObject trimmingSign);
        [PreserveSig]
        new HRESULT CreateTextAnalyzer(out IDWriteTextAnalyzer textAnalyzer);
        [PreserveSig]
        new HRESULT CreateNumberSubstitution(DWRITE_NUMBER_SUBSTITUTION_METHOD substitutionMethod, string localeName, bool ignoreUserOverride, out IDWriteNumberSubstitution numberSubstitution);
        [PreserveSig]
        new HRESULT CreateGlyphRunAnalysis(ref DWRITE_GLYPH_RUN glyphRun, float pixelsPerDip, DWRITE_MATRIX transform, DWRITE_RENDERING_MODE renderingMode,
            DWRITE_MEASURING_MODE measuringMode, float baselineOriginX, float baselineOriginY, out IDWriteGlyphRunAnalysis glyphRunAnalysis);
        #endregion

        [PreserveSig]
        new HRESULT GetEudcFontCollection(out IDWriteFontCollection fontCollection, bool checkForUpdates = false);
        [PreserveSig]
        new HRESULT CreateCustomRenderingParams1(float gamma, float enhancedContrast, float enhancedContrastGrayscale, float clearTypeLevel,
            DWRITE_PIXEL_GEOMETRY pixelGeometry, DWRITE_RENDERING_MODE renderingMode, out IDWriteRenderingParams1 renderingParams);
        #endregion

        [PreserveSig]
        HRESULT GetSystemFontFallback(out IDWriteFontFallback fontFallback);
        [PreserveSig]
        HRESULT CreateFontFallbackBuilder(out IDWriteFontFallbackBuilder fontFallbackBuilder);
        [PreserveSig]
        HRESULT TranslateColorGlyphRun(D2D1_POINT_2F baselineOriginX, D2D1_POINT_2F baselineOriginY, ref DWRITE_GLYPH_RUN glyphRun, IntPtr glyphRunDescription,
            DWRITE_MEASURING_MODE measuringMode, IntPtr worldToDeviceTransform, uint colorPaletteIndex, out IDWriteColorGlyphRunEnumerator colorLayers);
        [PreserveSig]
        HRESULT CreateCustomRenderingParams2(float gamma, float enhancedContrast, float grayscaleEnhancedContrast, float clearTypeLevel, DWRITE_PIXEL_GEOMETRY pixelGeometry,
            DWRITE_RENDERING_MODE renderingMode, DWRITE_GRID_FIT_MODE gridFitMode, out IDWriteRenderingParams2 renderingParams);
        [PreserveSig]
        HRESULT CreateGlyphRunAnalysis2(ref DWRITE_GLYPH_RUN glyphRun, DWRITE_MATRIX transform, DWRITE_RENDERING_MODE renderingMode, DWRITE_MEASURING_MODE measuringMode,
            DWRITE_GRID_FIT_MODE gridFitMode, DWRITE_TEXT_ANTIALIAS_MODE antialiasMode, float baselineOriginX, float baselineOriginY, out IDWriteGlyphRunAnalysis glyphRunAnalysis);
    }

    [ComImport]
    [Guid("9A1B41C3-D3BB-466A-87FC-FE67556A3B65")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFactory3 : IDWriteFactory2
    {
        #region IDWriteFactory2
        #region IDWriteFactory1
        #region IDWriteFactory
        new HRESULT GetSystemFontCollection(out IDWriteFontCollection fontCollection, bool checkForUpdates = false);
        [PreserveSig]
        new HRESULT CreateCustomFontCollection(IDWriteFontCollectionLoader collectionLoader, IntPtr collectionKey, int collectionKeySize, out IDWriteFontCollection fontCollection);
        [PreserveSig]
        new HRESULT RegisterFontCollectionLoader(IDWriteFontCollectionLoader fontCollectionLoader);
        [PreserveSig]
        new HRESULT UnregisterFontCollectionLoader(IDWriteFontCollectionLoader fontCollectionLoader);
        //new HRESULT CreateFontFileReference(string filePath, System.Runtime.InteropServices.ComTypes.FILETIME lastWriteTime, out IDWriteFontFile fontFile);
        [PreserveSig]
        new HRESULT CreateFontFileReference([MarshalAs(UnmanagedType.LPWStr)] string filePath, IntPtr lastWriteTime, out IDWriteFontFile fontFile);
        [PreserveSig]
        new HRESULT CreateCustomFontFileReference(IntPtr fontFileReferenceKey, uint fontFileReferenceKeySize, IDWriteFontFileLoader fontFileLoader, out IDWriteFontFile fontFile);
        [PreserveSig]
        new HRESULT CreateFontFace(DWRITE_FONT_FACE_TYPE fontFaceType, int numberOfFiles, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] IDWriteFontFile[] fontFiles, int faceIndex, DWRITE_FONT_SIMULATIONS fontFaceSimulationFlags, out IDWriteFontFace fontFace);
        [PreserveSig]
        new HRESULT CreateRenderingParams(out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        new HRESULT CreateMonitorRenderingParams(IntPtr monitor, out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        new HRESULT CreateCustomRenderingParams(float gamma, float enhancedContrast, float clearTypeLevel, DWRITE_PIXEL_GEOMETRY pixelGeometry, DWRITE_RENDERING_MODE renderingMode, out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        new HRESULT RegisterFontFileLoader(IDWriteFontFileLoader fontFileLoader);
        [PreserveSig]
        new HRESULT UnregisterFontFileLoader(IDWriteFontFileLoader fontFileLoader);
        [PreserveSig]
        new HRESULT CreateTextFormat([MarshalAs(UnmanagedType.LPWStr)] string fontFamilyName, IDWriteFontCollection fontCollection, DWRITE_FONT_WEIGHT fontWeight, DWRITE_FONT_STYLE fontStyle, DWRITE_FONT_STRETCH fontStretch, float fontSize,
            [MarshalAs(UnmanagedType.LPWStr)] string localeName, out IDWriteTextFormat textFormat);
        [PreserveSig]
        new HRESULT CreateTypography(out IDWriteTypography typography);
        [PreserveSig]
        new HRESULT GetGdiInterop(out IDWriteGdiInterop gdiInterop);
        [PreserveSig]
        new HRESULT CreateTextLayout([MarshalAs(UnmanagedType.LPWStr)] string str, int stringLength, IDWriteTextFormat textFormat, float maxWidth, float maxHeight, out IDWriteTextLayout textLayout);
        [PreserveSig]
        new HRESULT CreateGdiCompatibleTextLayout([MarshalAs(UnmanagedType.LPWStr)] string str, int stringLength, IDWriteTextFormat textFormat, float layoutWidth,
            float layoutHeight, float pixelsPerDip, DWRITE_MATRIX transform, bool useGdiNatural, out IDWriteTextLayout textLayout);
        [PreserveSig]
        new HRESULT CreateEllipsisTrimmingSign(IDWriteTextFormat textFormat, out IDWriteInlineObject trimmingSign);
        [PreserveSig]
        new HRESULT CreateTextAnalyzer(out IDWriteTextAnalyzer textAnalyzer);
        [PreserveSig]
        new HRESULT CreateNumberSubstitution(DWRITE_NUMBER_SUBSTITUTION_METHOD substitutionMethod, string localeName, bool ignoreUserOverride, out IDWriteNumberSubstitution numberSubstitution);
        [PreserveSig]
        new HRESULT CreateGlyphRunAnalysis(ref DWRITE_GLYPH_RUN glyphRun, float pixelsPerDip, DWRITE_MATRIX transform, DWRITE_RENDERING_MODE renderingMode,
            DWRITE_MEASURING_MODE measuringMode, float baselineOriginX, float baselineOriginY, out IDWriteGlyphRunAnalysis glyphRunAnalysis);
        #endregion

        [PreserveSig]
        new HRESULT GetEudcFontCollection(out IDWriteFontCollection fontCollection, bool checkForUpdates = false);
        [PreserveSig]
        new HRESULT CreateCustomRenderingParams1(float gamma, float enhancedContrast, float enhancedContrastGrayscale, float clearTypeLevel,
            DWRITE_PIXEL_GEOMETRY pixelGeometry, DWRITE_RENDERING_MODE renderingMode, out IDWriteRenderingParams1 renderingParams);
        #endregion

        [PreserveSig]
        new HRESULT GetSystemFontFallback(out IDWriteFontFallback fontFallback);
        [PreserveSig]
        new HRESULT CreateFontFallbackBuilder(out IDWriteFontFallbackBuilder fontFallbackBuilder);
        [PreserveSig]
        new HRESULT TranslateColorGlyphRun(D2D1_POINT_2F baselineOriginX, D2D1_POINT_2F baselineOriginY, ref DWRITE_GLYPH_RUN glyphRun, IntPtr glyphRunDescription,
            DWRITE_MEASURING_MODE measuringMode, IntPtr worldToDeviceTransform, uint colorPaletteIndex, out IDWriteColorGlyphRunEnumerator colorLayers);
        [PreserveSig]
        new HRESULT CreateCustomRenderingParams2(float gamma, float enhancedContrast, float grayscaleEnhancedContrast, float clearTypeLevel, DWRITE_PIXEL_GEOMETRY pixelGeometry,
            DWRITE_RENDERING_MODE renderingMode, DWRITE_GRID_FIT_MODE gridFitMode, out IDWriteRenderingParams2 renderingParams);
        [PreserveSig]
        new HRESULT CreateGlyphRunAnalysis2(ref DWRITE_GLYPH_RUN glyphRun, DWRITE_MATRIX transform, DWRITE_RENDERING_MODE renderingMode, DWRITE_MEASURING_MODE measuringMode,
            DWRITE_GRID_FIT_MODE gridFitMode, DWRITE_TEXT_ANTIALIAS_MODE antialiasMode, float baselineOriginX, float baselineOriginY, out IDWriteGlyphRunAnalysis glyphRunAnalysis);
        #endregion

        [PreserveSig]
        HRESULT CreateGlyphRunAnalysis3(ref DWRITE_GLYPH_RUN glyphRun, DWRITE_MATRIX  transform, DWRITE_RENDERING_MODE1 renderingMode, DWRITE_MEASURING_MODE measuringMode,
            DWRITE_GRID_FIT_MODE gridFitMode, DWRITE_TEXT_ANTIALIAS_MODE antialiasMode, float baselineOriginX, float baselineOriginY, out IDWriteGlyphRunAnalysis glyphRunAnalysis);
        [PreserveSig]
        HRESULT CreateCustomRenderingParams3(float gamma, float enhancedContrast, float grayscaleEnhancedContrast, float clearTypeLevel, DWRITE_PIXEL_GEOMETRY pixelGeometry,
            DWRITE_RENDERING_MODE1 renderingMode, DWRITE_GRID_FIT_MODE gridFitMode, out IDWriteRenderingParams3 renderingParams);
        //HRESULT CreateFontFaceReference3(string filePath, System.Runtime.InteropServices.ComTypes.FILETIME lastWriteTime, uint faceIndex, DWRITE_FONT_SIMULATIONS fontSimulations, out IDWriteFontFaceReference fontFaceReference);
        [PreserveSig]
        HRESULT CreateFontFaceReference3(string filePath, IntPtr lastWriteTime, uint faceIndex, DWRITE_FONT_SIMULATIONS fontSimulations, out IDWriteFontFaceReference fontFaceReference);
        [PreserveSig]
        HRESULT CreateFontFaceReference3(IDWriteFontFile fontFile, uint faceIndex, DWRITE_FONT_SIMULATIONS fontSimulations, out IDWriteFontFaceReference fontFaceReference);
        [PreserveSig]
        HRESULT GetSystemFontSet3(out IDWriteFontSet fontSet);
        [PreserveSig]
        HRESULT CreateFontSetBuilder3(out IDWriteFontSetBuilder fontSetBuilder);
        [PreserveSig]
        HRESULT CreateFontCollectionFromFontSet3(IDWriteFontSet fontSet, out IDWriteFontCollection1 fontCollection);
        [PreserveSig]
        HRESULT GetSystemFontCollection3(bool includeDownloadableFonts, out IDWriteFontCollection1 fontCollection, bool checkForUpdates = false);
        [PreserveSig]
        HRESULT GetFontDownloadQueue(out IDWriteFontDownloadQueue fontDownloadQueue);
    }

    [ComImport]
    [Guid("4B0B5BD3-0797-4549-8AC5-FE915CC53856")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFactory4 : IDWriteFactory3
    {
        #region IDWriteFactory3
        #region IDWriteFactory2
        #region IDWriteFactory1
        #region IDWriteFactory
        new HRESULT GetSystemFontCollection(out IDWriteFontCollection fontCollection, bool checkForUpdates = false);
        [PreserveSig]
        new HRESULT CreateCustomFontCollection(IDWriteFontCollectionLoader collectionLoader, IntPtr collectionKey, int collectionKeySize, out IDWriteFontCollection fontCollection);
        [PreserveSig]
        new HRESULT RegisterFontCollectionLoader(IDWriteFontCollectionLoader fontCollectionLoader);
        [PreserveSig]
        new HRESULT UnregisterFontCollectionLoader(IDWriteFontCollectionLoader fontCollectionLoader);
        //new HRESULT CreateFontFileReference(string filePath, System.Runtime.InteropServices.ComTypes.FILETIME lastWriteTime, out IDWriteFontFile fontFile);
        [PreserveSig]
        new HRESULT CreateFontFileReference([MarshalAs(UnmanagedType.LPWStr)] string filePath, IntPtr lastWriteTime, out IDWriteFontFile fontFile);
        [PreserveSig]
        new HRESULT CreateCustomFontFileReference(IntPtr fontFileReferenceKey, uint fontFileReferenceKeySize, IDWriteFontFileLoader fontFileLoader, out IDWriteFontFile fontFile);
        [PreserveSig]
        new HRESULT CreateFontFace(DWRITE_FONT_FACE_TYPE fontFaceType, int numberOfFiles, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] IDWriteFontFile[] fontFiles, int faceIndex, DWRITE_FONT_SIMULATIONS fontFaceSimulationFlags, out IDWriteFontFace fontFace);
        [PreserveSig]
        new HRESULT CreateRenderingParams(out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        new HRESULT CreateMonitorRenderingParams(IntPtr monitor, out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        new HRESULT CreateCustomRenderingParams(float gamma, float enhancedContrast, float clearTypeLevel, DWRITE_PIXEL_GEOMETRY pixelGeometry, DWRITE_RENDERING_MODE renderingMode, out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        new HRESULT RegisterFontFileLoader(IDWriteFontFileLoader fontFileLoader);
        [PreserveSig]
        new HRESULT UnregisterFontFileLoader(IDWriteFontFileLoader fontFileLoader);
        [PreserveSig]
        new HRESULT CreateTextFormat([MarshalAs(UnmanagedType.LPWStr)] string fontFamilyName, IDWriteFontCollection fontCollection, DWRITE_FONT_WEIGHT fontWeight, DWRITE_FONT_STYLE fontStyle, DWRITE_FONT_STRETCH fontStretch, float fontSize,
            [MarshalAs(UnmanagedType.LPWStr)] string localeName, out IDWriteTextFormat textFormat);
        [PreserveSig]
        new HRESULT CreateTypography(out IDWriteTypography typography);
        [PreserveSig]
        new HRESULT GetGdiInterop(out IDWriteGdiInterop gdiInterop);
        [PreserveSig]
        new HRESULT CreateTextLayout([MarshalAs(UnmanagedType.LPWStr)] string str, int stringLength, IDWriteTextFormat textFormat, float maxWidth, float maxHeight, out IDWriteTextLayout textLayout);
        [PreserveSig]
        new HRESULT CreateGdiCompatibleTextLayout([MarshalAs(UnmanagedType.LPWStr)] string str, int stringLength, IDWriteTextFormat textFormat, float layoutWidth,
            float layoutHeight, float pixelsPerDip, DWRITE_MATRIX transform, bool useGdiNatural, out IDWriteTextLayout textLayout);
        [PreserveSig]
        new HRESULT CreateEllipsisTrimmingSign(IDWriteTextFormat textFormat, out IDWriteInlineObject trimmingSign);
        [PreserveSig]
        new HRESULT CreateTextAnalyzer(out IDWriteTextAnalyzer textAnalyzer);
        [PreserveSig]
        new HRESULT CreateNumberSubstitution(DWRITE_NUMBER_SUBSTITUTION_METHOD substitutionMethod, string localeName, bool ignoreUserOverride, out IDWriteNumberSubstitution numberSubstitution);
        [PreserveSig]
        new HRESULT CreateGlyphRunAnalysis(ref DWRITE_GLYPH_RUN glyphRun, float pixelsPerDip, DWRITE_MATRIX transform, DWRITE_RENDERING_MODE renderingMode,
            DWRITE_MEASURING_MODE measuringMode, float baselineOriginX, float baselineOriginY, out IDWriteGlyphRunAnalysis glyphRunAnalysis);
        #endregion

        [PreserveSig]
        new HRESULT GetEudcFontCollection(out IDWriteFontCollection fontCollection, bool checkForUpdates = false);
        [PreserveSig]
        new HRESULT CreateCustomRenderingParams1(float gamma, float enhancedContrast, float enhancedContrastGrayscale, float clearTypeLevel,
            DWRITE_PIXEL_GEOMETRY pixelGeometry, DWRITE_RENDERING_MODE renderingMode, out IDWriteRenderingParams1 renderingParams);
        #endregion

        [PreserveSig]
        new HRESULT GetSystemFontFallback(out IDWriteFontFallback fontFallback);
        [PreserveSig]
        new HRESULT CreateFontFallbackBuilder(out IDWriteFontFallbackBuilder fontFallbackBuilder);
        [PreserveSig]
        new HRESULT TranslateColorGlyphRun(D2D1_POINT_2F baselineOriginX, D2D1_POINT_2F baselineOriginY, ref DWRITE_GLYPH_RUN glyphRun, IntPtr glyphRunDescription,
            DWRITE_MEASURING_MODE measuringMode, IntPtr worldToDeviceTransform, uint colorPaletteIndex, out IDWriteColorGlyphRunEnumerator colorLayers);
        [PreserveSig]
        new HRESULT CreateCustomRenderingParams2(float gamma, float enhancedContrast, float grayscaleEnhancedContrast, float clearTypeLevel, DWRITE_PIXEL_GEOMETRY pixelGeometry,
            DWRITE_RENDERING_MODE renderingMode, DWRITE_GRID_FIT_MODE gridFitMode, out IDWriteRenderingParams2 renderingParams);
        [PreserveSig]
        new HRESULT CreateGlyphRunAnalysis2(ref DWRITE_GLYPH_RUN glyphRun, DWRITE_MATRIX transform, DWRITE_RENDERING_MODE renderingMode, DWRITE_MEASURING_MODE measuringMode,
            DWRITE_GRID_FIT_MODE gridFitMode, DWRITE_TEXT_ANTIALIAS_MODE antialiasMode, float baselineOriginX, float baselineOriginY, out IDWriteGlyphRunAnalysis glyphRunAnalysis);
        #endregion

        [PreserveSig]
        new HRESULT CreateGlyphRunAnalysis3(ref DWRITE_GLYPH_RUN glyphRun, DWRITE_MATRIX transform, DWRITE_RENDERING_MODE1 renderingMode, DWRITE_MEASURING_MODE measuringMode,
            DWRITE_GRID_FIT_MODE gridFitMode, DWRITE_TEXT_ANTIALIAS_MODE antialiasMode, float baselineOriginX, float baselineOriginY, out IDWriteGlyphRunAnalysis glyphRunAnalysis);
        [PreserveSig]
        new HRESULT CreateCustomRenderingParams3(float gamma, float enhancedContrast, float grayscaleEnhancedContrast, float clearTypeLevel, DWRITE_PIXEL_GEOMETRY pixelGeometry,
            DWRITE_RENDERING_MODE1 renderingMode, DWRITE_GRID_FIT_MODE gridFitMode, out IDWriteRenderingParams3 renderingParams);
        //new HRESULT CreateFontFaceReference3(string filePath, System.Runtime.InteropServices.ComTypes.FILETIME lastWriteTime, uint faceIndex, DWRITE_FONT_SIMULATIONS fontSimulations, out IDWriteFontFaceReference fontFaceReference);
        [PreserveSig]
        new HRESULT CreateFontFaceReference3(string filePath, IntPtr lastWriteTime, uint faceIndex, DWRITE_FONT_SIMULATIONS fontSimulations, out IDWriteFontFaceReference fontFaceReference);
        [PreserveSig]
        new HRESULT CreateFontFaceReference3(IDWriteFontFile fontFile, uint faceIndex, DWRITE_FONT_SIMULATIONS fontSimulations, out IDWriteFontFaceReference fontFaceReference);
        new HRESULT GetSystemFontSet3(out IDWriteFontSet fontSet);
        [PreserveSig]
        new HRESULT CreateFontSetBuilder3(out IDWriteFontSetBuilder fontSetBuilder);
        [PreserveSig]
        new HRESULT CreateFontCollectionFromFontSet3(IDWriteFontSet fontSet, out IDWriteFontCollection1 fontCollection);
        [PreserveSig]
        new HRESULT GetSystemFontCollection3(bool includeDownloadableFonts, out IDWriteFontCollection1 fontCollection, bool checkForUpdates = false);
        [PreserveSig]
        new HRESULT GetFontDownloadQueue(out IDWriteFontDownloadQueue fontDownloadQueue);
        #endregion

        [PreserveSig]   
        HRESULT TranslateColorGlyphRun4(D2D1_POINT_2F baselineOrigin, ref DWRITE_GLYPH_RUN glyphRun, IntPtr glyphRunDescription,
        DWRITE_GLYPH_IMAGE_FORMATS desiredGlyphImageFormats, DWRITE_MEASURING_MODE measuringMode, IntPtr worldAndDpiTransform,
        uint colorPaletteIndex, out IDWriteColorGlyphRunEnumerator1 colorLayers);

        [PreserveSig]
        HRESULT ComputeGlyphOrigins(ref DWRITE_GLYPH_RUN glyphRun, DWRITE_MEASURING_MODE measuringMode, D2D1_POINT_2F baselineOrigin, DWRITE_MATRIX worldAndDpiTransform,
            out D2D1_POINT_2F glyphOrigins);
        [PreserveSig]
        HRESULT ComputeGlyphOrigins(ref DWRITE_GLYPH_RUN glyphRun, D2D1_POINT_2F baselineOrigin, out D2D1_POINT_2F glyphOrigins);
    }

    [ComImport]
    [Guid("958DB99A-BE2A-4F09-AF7D-65189803D1D3")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFactory5 : IDWriteFactory4
    {
        #region IDWriteFactory4
        #region IDWriteFactory3
        #region IDWriteFactory2
        #region IDWriteFactory1
        #region IDWriteFactory
        new HRESULT GetSystemFontCollection(out IDWriteFontCollection fontCollection, bool checkForUpdates = false);
        [PreserveSig]
        new HRESULT CreateCustomFontCollection(IDWriteFontCollectionLoader collectionLoader, IntPtr collectionKey, int collectionKeySize, out IDWriteFontCollection fontCollection);
        [PreserveSig]
        new HRESULT RegisterFontCollectionLoader(IDWriteFontCollectionLoader fontCollectionLoader);
        new HRESULT UnregisterFontCollectionLoader(IDWriteFontCollectionLoader fontCollectionLoader);
        //new HRESULT CreateFontFileReference(string filePath, System.Runtime.InteropServices.ComTypes.FILETIME lastWriteTime, out IDWriteFontFile fontFile);
        [PreserveSig]
        new HRESULT CreateFontFileReference([MarshalAs(UnmanagedType.LPWStr)] string filePath, IntPtr lastWriteTime, out IDWriteFontFile fontFile);
        [PreserveSig]
        new HRESULT CreateCustomFontFileReference(IntPtr fontFileReferenceKey, uint fontFileReferenceKeySize, IDWriteFontFileLoader fontFileLoader, out IDWriteFontFile fontFile);
        [PreserveSig]
        new HRESULT CreateFontFace(DWRITE_FONT_FACE_TYPE fontFaceType, int numberOfFiles, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] IDWriteFontFile[] fontFiles, int faceIndex, DWRITE_FONT_SIMULATIONS fontFaceSimulationFlags, out IDWriteFontFace fontFace);
        [PreserveSig]
        new HRESULT CreateRenderingParams(out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        new HRESULT CreateMonitorRenderingParams(IntPtr monitor, out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        new HRESULT CreateCustomRenderingParams(float gamma, float enhancedContrast, float clearTypeLevel, DWRITE_PIXEL_GEOMETRY pixelGeometry, DWRITE_RENDERING_MODE renderingMode, out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        new HRESULT RegisterFontFileLoader(IDWriteFontFileLoader fontFileLoader);
        new HRESULT UnregisterFontFileLoader(IDWriteFontFileLoader fontFileLoader);
        [PreserveSig]
        new HRESULT CreateTextFormat([MarshalAs(UnmanagedType.LPWStr)] string fontFamilyName, IDWriteFontCollection fontCollection, DWRITE_FONT_WEIGHT fontWeight, DWRITE_FONT_STYLE fontStyle, DWRITE_FONT_STRETCH fontStretch, float fontSize,
            [MarshalAs(UnmanagedType.LPWStr)] string localeName, out IDWriteTextFormat textFormat);
        [PreserveSig]
        new HRESULT CreateTypography(out IDWriteTypography typography);
        [PreserveSig]
        new HRESULT GetGdiInterop(out IDWriteGdiInterop gdiInterop);
        [PreserveSig]
        new HRESULT CreateTextLayout([MarshalAs(UnmanagedType.LPWStr)] string str, int stringLength, IDWriteTextFormat textFormat, float maxWidth, float maxHeight, out IDWriteTextLayout textLayout);
        [PreserveSig]
        new HRESULT CreateGdiCompatibleTextLayout([MarshalAs(UnmanagedType.LPWStr)] string str, int stringLength, IDWriteTextFormat textFormat, float layoutWidth,
            float layoutHeight, float pixelsPerDip, DWRITE_MATRIX transform, bool useGdiNatural, out IDWriteTextLayout textLayout);
        [PreserveSig]
        new HRESULT CreateEllipsisTrimmingSign(IDWriteTextFormat textFormat, out IDWriteInlineObject trimmingSign);
        [PreserveSig]
        new HRESULT CreateTextAnalyzer(out IDWriteTextAnalyzer textAnalyzer);
        [PreserveSig]
        new HRESULT CreateNumberSubstitution(DWRITE_NUMBER_SUBSTITUTION_METHOD substitutionMethod, string localeName, bool ignoreUserOverride, out IDWriteNumberSubstitution numberSubstitution);
        [PreserveSig]
        new HRESULT CreateGlyphRunAnalysis(ref DWRITE_GLYPH_RUN glyphRun, float pixelsPerDip, DWRITE_MATRIX transform, DWRITE_RENDERING_MODE renderingMode,
            DWRITE_MEASURING_MODE measuringMode, float baselineOriginX, float baselineOriginY, out IDWriteGlyphRunAnalysis glyphRunAnalysis);
        #endregion

        new HRESULT GetEudcFontCollection(out IDWriteFontCollection fontCollection, bool checkForUpdates = false);
        [PreserveSig]
        new HRESULT CreateCustomRenderingParams1(float gamma, float enhancedContrast, float enhancedContrastGrayscale, float clearTypeLevel,
            DWRITE_PIXEL_GEOMETRY pixelGeometry, DWRITE_RENDERING_MODE renderingMode, out IDWriteRenderingParams1 renderingParams);
        #endregion

        [PreserveSig]
        new HRESULT GetSystemFontFallback(out IDWriteFontFallback fontFallback);
        [PreserveSig]
        new HRESULT CreateFontFallbackBuilder(out IDWriteFontFallbackBuilder fontFallbackBuilder);
        [PreserveSig]
        new HRESULT TranslateColorGlyphRun(D2D1_POINT_2F baselineOriginX, D2D1_POINT_2F baselineOriginY, ref DWRITE_GLYPH_RUN glyphRun, IntPtr glyphRunDescription,
            DWRITE_MEASURING_MODE measuringMode, IntPtr worldToDeviceTransform, uint colorPaletteIndex, out IDWriteColorGlyphRunEnumerator colorLayers);
        [PreserveSig]
        new HRESULT CreateCustomRenderingParams2(float gamma, float enhancedContrast, float grayscaleEnhancedContrast, float clearTypeLevel, DWRITE_PIXEL_GEOMETRY pixelGeometry,
            DWRITE_RENDERING_MODE renderingMode, DWRITE_GRID_FIT_MODE gridFitMode, out IDWriteRenderingParams2 renderingParams);
        [PreserveSig]
        new HRESULT CreateGlyphRunAnalysis2(ref DWRITE_GLYPH_RUN glyphRun, DWRITE_MATRIX transform, DWRITE_RENDERING_MODE renderingMode, DWRITE_MEASURING_MODE measuringMode,
            DWRITE_GRID_FIT_MODE gridFitMode, DWRITE_TEXT_ANTIALIAS_MODE antialiasMode, float baselineOriginX, float baselineOriginY, out IDWriteGlyphRunAnalysis glyphRunAnalysis);
        #endregion

        [PreserveSig]
        new HRESULT CreateGlyphRunAnalysis3(ref DWRITE_GLYPH_RUN glyphRun, DWRITE_MATRIX transform, DWRITE_RENDERING_MODE1 renderingMode, DWRITE_MEASURING_MODE measuringMode,
            DWRITE_GRID_FIT_MODE gridFitMode, DWRITE_TEXT_ANTIALIAS_MODE antialiasMode, float baselineOriginX, float baselineOriginY, out IDWriteGlyphRunAnalysis glyphRunAnalysis);
        [PreserveSig]
        new HRESULT CreateCustomRenderingParams3(float gamma, float enhancedContrast, float grayscaleEnhancedContrast, float clearTypeLevel, DWRITE_PIXEL_GEOMETRY pixelGeometry,
            DWRITE_RENDERING_MODE1 renderingMode, DWRITE_GRID_FIT_MODE gridFitMode, out IDWriteRenderingParams3 renderingParams);
        //new HRESULT CreateFontFaceReference3(string filePath, System.Runtime.InteropServices.ComTypes.FILETIME lastWriteTime, uint faceIndex, DWRITE_FONT_SIMULATIONS fontSimulations, out IDWriteFontFaceReference fontFaceReference);
        [PreserveSig]
        new HRESULT CreateFontFaceReference3(string filePath, IntPtr lastWriteTime, uint faceIndex, DWRITE_FONT_SIMULATIONS fontSimulations, out IDWriteFontFaceReference fontFaceReference);
        [PreserveSig]
        new HRESULT CreateFontFaceReference3(IDWriteFontFile fontFile, uint faceIndex, DWRITE_FONT_SIMULATIONS fontSimulations, out IDWriteFontFaceReference fontFaceReference);
        [PreserveSig]
        new HRESULT GetSystemFontSet3(out IDWriteFontSet fontSet);
        [PreserveSig]
        new HRESULT CreateFontSetBuilder3(out IDWriteFontSetBuilder fontSetBuilder);
        [PreserveSig]
        new HRESULT CreateFontCollectionFromFontSet3(IDWriteFontSet fontSet, out IDWriteFontCollection1 fontCollection);
        [PreserveSig]
        new HRESULT GetSystemFontCollection3(bool includeDownloadableFonts, out IDWriteFontCollection1 fontCollection, bool checkForUpdates = false);
        [PreserveSig]
        new HRESULT GetFontDownloadQueue(out IDWriteFontDownloadQueue fontDownloadQueue);
        #endregion

        new HRESULT TranslateColorGlyphRun4(D2D1_POINT_2F baselineOrigin, ref DWRITE_GLYPH_RUN glyphRun, IntPtr glyphRunDescription,
            DWRITE_GLYPH_IMAGE_FORMATS desiredGlyphImageFormats, DWRITE_MEASURING_MODE measuringMode, IntPtr worldAndDpiTransform,
            uint colorPaletteIndex, out IDWriteColorGlyphRunEnumerator1 colorLayers);
        new HRESULT ComputeGlyphOrigins(ref DWRITE_GLYPH_RUN glyphRun, DWRITE_MEASURING_MODE measuringMode, D2D1_POINT_2F baselineOrigin, DWRITE_MATRIX worldAndDpiTransform,
            out D2D1_POINT_2F glyphOrigins);
        new HRESULT ComputeGlyphOrigins(ref DWRITE_GLYPH_RUN glyphRun, D2D1_POINT_2F baselineOrigin, out D2D1_POINT_2F glyphOrigins);
        #endregion

        [PreserveSig]
        HRESULT CreateFontSetBuilder5(out IDWriteFontSetBuilder1 fontSetBuilder);
        [PreserveSig]
        HRESULT CreateInMemoryFontFileLoader(out IDWriteInMemoryFontFileLoader newLoader);
        [PreserveSig]
        HRESULT CreateHttpFontFileLoader(string referrerUrl, string extraHeaders, out IDWriteRemoteFontFileLoader newLoader);
        DWRITE_CONTAINER_TYPE AnalyzeContainerType(IntPtr fileData, uint fileDataSize);
        [PreserveSig]
        HRESULT UnpackFontFile(DWRITE_CONTAINER_TYPE containerType, IntPtr fileData, uint fileDataSize, out IDWriteFontFileStream unpackedFontStream);
    }

    [ComImport]
    [Guid("F3744D80-21F7-42EB-B35D-995BC72FC223")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFactory6 : IDWriteFactory5
    {
        #region IDWriteFactory5
        #region IDWriteFactory4
        #region IDWriteFactory3
        #region IDWriteFactory2
        #region IDWriteFactory1
        #region IDWriteFactory
        new HRESULT GetSystemFontCollection(out IDWriteFontCollection fontCollection, bool checkForUpdates = false);
        [PreserveSig]
        new HRESULT CreateCustomFontCollection(IDWriteFontCollectionLoader collectionLoader, IntPtr collectionKey, int collectionKeySize, out IDWriteFontCollection fontCollection);
        [PreserveSig]
        new HRESULT RegisterFontCollectionLoader(IDWriteFontCollectionLoader fontCollectionLoader);
        new HRESULT UnregisterFontCollectionLoader(IDWriteFontCollectionLoader fontCollectionLoader);
        //new HRESULT CreateFontFileReference(string filePath, System.Runtime.InteropServices.ComTypes.FILETIME lastWriteTime, out IDWriteFontFile fontFile);
        [PreserveSig]
        new HRESULT CreateFontFileReference([MarshalAs(UnmanagedType.LPWStr)] string filePath, IntPtr lastWriteTime, out IDWriteFontFile fontFile);
        [PreserveSig]
        new HRESULT CreateCustomFontFileReference(IntPtr fontFileReferenceKey, uint fontFileReferenceKeySize, IDWriteFontFileLoader fontFileLoader, out IDWriteFontFile fontFile);
        [PreserveSig]
        new HRESULT CreateFontFace(DWRITE_FONT_FACE_TYPE fontFaceType, int numberOfFiles, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] IDWriteFontFile[] fontFiles, int faceIndex, DWRITE_FONT_SIMULATIONS fontFaceSimulationFlags, out IDWriteFontFace fontFace);
        [PreserveSig]
        new HRESULT CreateRenderingParams(out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        new HRESULT CreateMonitorRenderingParams(IntPtr monitor, out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        new HRESULT CreateCustomRenderingParams(float gamma, float enhancedContrast, float clearTypeLevel, DWRITE_PIXEL_GEOMETRY pixelGeometry, DWRITE_RENDERING_MODE renderingMode, out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        new HRESULT RegisterFontFileLoader(IDWriteFontFileLoader fontFileLoader);
        new HRESULT UnregisterFontFileLoader(IDWriteFontFileLoader fontFileLoader);
        [PreserveSig]
        new HRESULT CreateTextFormat([MarshalAs(UnmanagedType.LPWStr)] string fontFamilyName, IDWriteFontCollection fontCollection, DWRITE_FONT_WEIGHT fontWeight, DWRITE_FONT_STYLE fontStyle, DWRITE_FONT_STRETCH fontStretch, float fontSize,
            [MarshalAs(UnmanagedType.LPWStr)] string localeName, out IDWriteTextFormat textFormat);
        [PreserveSig]
        new HRESULT CreateTypography(out IDWriteTypography typography);
        [PreserveSig]
        new HRESULT GetGdiInterop(out IDWriteGdiInterop gdiInterop);
        [PreserveSig]
        new HRESULT CreateTextLayout([MarshalAs(UnmanagedType.LPWStr)] string str, int stringLength, IDWriteTextFormat textFormat, float maxWidth, float maxHeight, out IDWriteTextLayout textLayout);
        [PreserveSig]
        new HRESULT CreateGdiCompatibleTextLayout([MarshalAs(UnmanagedType.LPWStr)] string str, int stringLength, IDWriteTextFormat textFormat, float layoutWidth,
            float layoutHeight, float pixelsPerDip, DWRITE_MATRIX transform, bool useGdiNatural, out IDWriteTextLayout textLayout);
        [PreserveSig]
        new HRESULT CreateEllipsisTrimmingSign(IDWriteTextFormat textFormat, out IDWriteInlineObject trimmingSign);
        [PreserveSig]
        new HRESULT CreateTextAnalyzer(out IDWriteTextAnalyzer textAnalyzer);
        [PreserveSig]
        new HRESULT CreateNumberSubstitution(DWRITE_NUMBER_SUBSTITUTION_METHOD substitutionMethod, string localeName, bool ignoreUserOverride, out IDWriteNumberSubstitution numberSubstitution);
        [PreserveSig]
        new HRESULT CreateGlyphRunAnalysis(ref DWRITE_GLYPH_RUN glyphRun, float pixelsPerDip, DWRITE_MATRIX transform, DWRITE_RENDERING_MODE renderingMode,
            DWRITE_MEASURING_MODE measuringMode, float baselineOriginX, float baselineOriginY, out IDWriteGlyphRunAnalysis glyphRunAnalysis);
        #endregion

        new HRESULT GetEudcFontCollection(out IDWriteFontCollection fontCollection, bool checkForUpdates = false);
        [PreserveSig]
        new HRESULT CreateCustomRenderingParams1(float gamma, float enhancedContrast, float enhancedContrastGrayscale, float clearTypeLevel,
            DWRITE_PIXEL_GEOMETRY pixelGeometry, DWRITE_RENDERING_MODE renderingMode, out IDWriteRenderingParams1 renderingParams);
        #endregion

        [PreserveSig]
        new HRESULT GetSystemFontFallback(out IDWriteFontFallback fontFallback);
        [PreserveSig]
        new HRESULT CreateFontFallbackBuilder(out IDWriteFontFallbackBuilder fontFallbackBuilder);
        [PreserveSig]
        new HRESULT TranslateColorGlyphRun(D2D1_POINT_2F baselineOriginX, D2D1_POINT_2F baselineOriginY, ref DWRITE_GLYPH_RUN glyphRun, IntPtr glyphRunDescription,
            DWRITE_MEASURING_MODE measuringMode, IntPtr worldToDeviceTransform, uint colorPaletteIndex, out IDWriteColorGlyphRunEnumerator colorLayers);
        [PreserveSig]
        new HRESULT CreateCustomRenderingParams2(float gamma, float enhancedContrast, float grayscaleEnhancedContrast, float clearTypeLevel, DWRITE_PIXEL_GEOMETRY pixelGeometry,
            DWRITE_RENDERING_MODE renderingMode, DWRITE_GRID_FIT_MODE gridFitMode, out IDWriteRenderingParams2 renderingParams);
        [PreserveSig]
        new HRESULT CreateGlyphRunAnalysis2(ref DWRITE_GLYPH_RUN glyphRun, DWRITE_MATRIX transform, DWRITE_RENDERING_MODE renderingMode, DWRITE_MEASURING_MODE measuringMode,
            DWRITE_GRID_FIT_MODE gridFitMode, DWRITE_TEXT_ANTIALIAS_MODE antialiasMode, float baselineOriginX, float baselineOriginY, out IDWriteGlyphRunAnalysis glyphRunAnalysis);
        #endregion

        [PreserveSig]
        new HRESULT CreateGlyphRunAnalysis3(ref DWRITE_GLYPH_RUN glyphRun, DWRITE_MATRIX transform, DWRITE_RENDERING_MODE1 renderingMode, DWRITE_MEASURING_MODE measuringMode,
            DWRITE_GRID_FIT_MODE gridFitMode, DWRITE_TEXT_ANTIALIAS_MODE antialiasMode, float baselineOriginX, float baselineOriginY, out IDWriteGlyphRunAnalysis glyphRunAnalysis);
        [PreserveSig]
        new HRESULT CreateCustomRenderingParams3(float gamma, float enhancedContrast, float grayscaleEnhancedContrast, float clearTypeLevel, DWRITE_PIXEL_GEOMETRY pixelGeometry,
            DWRITE_RENDERING_MODE1 renderingMode, DWRITE_GRID_FIT_MODE gridFitMode, out IDWriteRenderingParams3 renderingParams);
        //new HRESULT CreateFontFaceReference3(string filePath, System.Runtime.InteropServices.ComTypes.FILETIME lastWriteTime, uint faceIndex, DWRITE_FONT_SIMULATIONS fontSimulations, out IDWriteFontFaceReference fontFaceReference);
        [PreserveSig]
        new HRESULT CreateFontFaceReference3(string filePath, IntPtr lastWriteTime, uint faceIndex, DWRITE_FONT_SIMULATIONS fontSimulations, out IDWriteFontFaceReference fontFaceReference);
        [PreserveSig]
        new HRESULT CreateFontFaceReference3(IDWriteFontFile fontFile, uint faceIndex, DWRITE_FONT_SIMULATIONS fontSimulations, out IDWriteFontFaceReference fontFaceReference);
        [PreserveSig]
        new HRESULT GetSystemFontSet3(out IDWriteFontSet fontSet);
        [PreserveSig]
        new HRESULT CreateFontSetBuilder3(out IDWriteFontSetBuilder fontSetBuilder);
        [PreserveSig]
        new HRESULT CreateFontCollectionFromFontSet3(IDWriteFontSet fontSet, out IDWriteFontCollection1 fontCollection);
        [PreserveSig]
        new HRESULT GetSystemFontCollection3(bool includeDownloadableFonts, out IDWriteFontCollection1 fontCollection, bool checkForUpdates = false);
        [PreserveSig]
        new HRESULT GetFontDownloadQueue(out IDWriteFontDownloadQueue fontDownloadQueue);
        #endregion

        [PreserveSig]
        new HRESULT TranslateColorGlyphRun4(D2D1_POINT_2F baselineOrigin, ref DWRITE_GLYPH_RUN glyphRun, IntPtr glyphRunDescription,
            DWRITE_GLYPH_IMAGE_FORMATS desiredGlyphImageFormats, DWRITE_MEASURING_MODE measuringMode, IntPtr worldAndDpiTransform,
            uint colorPaletteIndex, out IDWriteColorGlyphRunEnumerator1 colorLayers);
        [PreserveSig]
        new HRESULT ComputeGlyphOrigins(ref DWRITE_GLYPH_RUN glyphRun, DWRITE_MEASURING_MODE measuringMode, D2D1_POINT_2F baselineOrigin, DWRITE_MATRIX worldAndDpiTransform,
            out D2D1_POINT_2F glyphOrigins);
        [PreserveSig]
        new HRESULT ComputeGlyphOrigins(ref DWRITE_GLYPH_RUN glyphRun, D2D1_POINT_2F baselineOrigin, out D2D1_POINT_2F glyphOrigins);
        #endregion

        [PreserveSig]
        new HRESULT CreateFontSetBuilder5(out IDWriteFontSetBuilder1 fontSetBuilder);
        [PreserveSig]
        new HRESULT CreateInMemoryFontFileLoader(out IDWriteInMemoryFontFileLoader newLoader);
        [PreserveSig]
        new HRESULT CreateHttpFontFileLoader(string referrerUrl, string extraHeaders, out IDWriteRemoteFontFileLoader newLoader);
        [PreserveSig]
        new DWRITE_CONTAINER_TYPE AnalyzeContainerType(IntPtr fileData, uint fileDataSize);
        [PreserveSig]
        new HRESULT UnpackFontFile(DWRITE_CONTAINER_TYPE containerType, IntPtr fileData, uint fileDataSize, out IDWriteFontFileStream unpackedFontStream);
        #endregion

        [PreserveSig]
        HRESULT CreateFontFaceReference6(IDWriteFontFile fontFile, uint faceIndex, DWRITE_FONT_SIMULATIONS fontSimulations, DWRITE_FONT_AXIS_VALUE fontAxisValues,
            uint fontAxisValueCount, out IDWriteFontFaceReference1 fontFaceReference);
        [PreserveSig]
        HRESULT CreateFontResource(IDWriteFontFile fontFile, uint faceIndex, out IDWriteFontResource fontResource);
        [PreserveSig]
        HRESULT GetSystemFontSet6(bool includeDownloadableFonts, out IDWriteFontSet1 fontSet);
        [PreserveSig]
        HRESULT GetSystemFontCollection6(bool includeDownloadableFonts, DWRITE_FONT_FAMILY_MODEL fontFamilyModel, out IDWriteFontCollection2 fontCollection);
        [PreserveSig]
        HRESULT CreateFontCollectionFromFontSet6(IDWriteFontSet fontSet, DWRITE_FONT_FAMILY_MODEL fontFamilyModel, out IDWriteFontCollection2 fontCollection);
        [PreserveSig]
        HRESULT CreateFontSetBuilder6(out IDWriteFontSetBuilder2 fontSetBuilder);
        [PreserveSig]
        HRESULT CreateTextFormat6(string fontFamilyName, IDWriteFontCollection fontCollection, DWRITE_FONT_AXIS_VALUE fontAxisValues, uint fontAxisValueCount,
            float fontSize, string localeName, out IDWriteTextFormat3 textFormat);
    }

    [ComImport]
    [Guid("35D0E0B3-9076-4D2E-A016-A91B568A06B4")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFactory7 : IDWriteFactory6
    {
        #region IDWriteFactory6
        #region IDWriteFactory5
        #region IDWriteFactory4
        #region IDWriteFactory3
        #region IDWriteFactory2
        #region IDWriteFactory1
        #region IDWriteFactory
        new HRESULT GetSystemFontCollection(out IDWriteFontCollection fontCollection, bool checkForUpdates = false);
        [PreserveSig]
        new HRESULT CreateCustomFontCollection(IDWriteFontCollectionLoader collectionLoader, IntPtr collectionKey, int collectionKeySize, out IDWriteFontCollection fontCollection);
        [PreserveSig]
        new HRESULT RegisterFontCollectionLoader(IDWriteFontCollectionLoader fontCollectionLoader);
        new HRESULT UnregisterFontCollectionLoader(IDWriteFontCollectionLoader fontCollectionLoader);
        //new HRESULT CreateFontFileReference(string filePath, System.Runtime.InteropServices.ComTypes.FILETIME lastWriteTime, out IDWriteFontFile fontFile);
        [PreserveSig]
        new HRESULT CreateFontFileReference([MarshalAs(UnmanagedType.LPWStr)] string filePath, IntPtr lastWriteTime, out IDWriteFontFile fontFile);
        [PreserveSig]
        new HRESULT CreateCustomFontFileReference(IntPtr fontFileReferenceKey, uint fontFileReferenceKeySize, IDWriteFontFileLoader fontFileLoader, out IDWriteFontFile fontFile);
        [PreserveSig]
        new HRESULT CreateFontFace(DWRITE_FONT_FACE_TYPE fontFaceType, int numberOfFiles, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] IDWriteFontFile[] fontFiles, int faceIndex, DWRITE_FONT_SIMULATIONS fontFaceSimulationFlags, out IDWriteFontFace fontFace);
        [PreserveSig]
        new HRESULT CreateRenderingParams(out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        new HRESULT CreateMonitorRenderingParams(IntPtr monitor, out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        new HRESULT CreateCustomRenderingParams(float gamma, float enhancedContrast, float clearTypeLevel, DWRITE_PIXEL_GEOMETRY pixelGeometry, DWRITE_RENDERING_MODE renderingMode, out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        new HRESULT RegisterFontFileLoader(IDWriteFontFileLoader fontFileLoader);
        new HRESULT UnregisterFontFileLoader(IDWriteFontFileLoader fontFileLoader);
        [PreserveSig]
        new HRESULT CreateTextFormat([MarshalAs(UnmanagedType.LPWStr)] string fontFamilyName, IDWriteFontCollection fontCollection, DWRITE_FONT_WEIGHT fontWeight, DWRITE_FONT_STYLE fontStyle, DWRITE_FONT_STRETCH fontStretch, float fontSize,
            [MarshalAs(UnmanagedType.LPWStr)] string localeName, out IDWriteTextFormat textFormat);
        [PreserveSig]
        new HRESULT CreateTypography(out IDWriteTypography typography);
        [PreserveSig]
        new HRESULT GetGdiInterop(out IDWriteGdiInterop gdiInterop);
        [PreserveSig]
        new HRESULT CreateTextLayout([MarshalAs(UnmanagedType.LPWStr)] string str, int stringLength, IDWriteTextFormat textFormat, float maxWidth, float maxHeight, out IDWriteTextLayout textLayout);
        [PreserveSig]
        new HRESULT CreateGdiCompatibleTextLayout([MarshalAs(UnmanagedType.LPWStr)] string str, int stringLength, IDWriteTextFormat textFormat, float layoutWidth,
            float layoutHeight, float pixelsPerDip, DWRITE_MATRIX transform, bool useGdiNatural, out IDWriteTextLayout textLayout);
        [PreserveSig]
        new HRESULT CreateEllipsisTrimmingSign(IDWriteTextFormat textFormat, out IDWriteInlineObject trimmingSign);
        [PreserveSig]
        new HRESULT CreateTextAnalyzer(out IDWriteTextAnalyzer textAnalyzer);
        [PreserveSig]
        new HRESULT CreateNumberSubstitution(DWRITE_NUMBER_SUBSTITUTION_METHOD substitutionMethod, string localeName, bool ignoreUserOverride, out IDWriteNumberSubstitution numberSubstitution);
        [PreserveSig]
        new HRESULT CreateGlyphRunAnalysis(ref DWRITE_GLYPH_RUN glyphRun, float pixelsPerDip, DWRITE_MATRIX transform, DWRITE_RENDERING_MODE renderingMode,
            DWRITE_MEASURING_MODE measuringMode, float baselineOriginX, float baselineOriginY, out IDWriteGlyphRunAnalysis glyphRunAnalysis);
        #endregion

        new HRESULT GetEudcFontCollection(out IDWriteFontCollection fontCollection, bool checkForUpdates = false);
        [PreserveSig]
        new HRESULT CreateCustomRenderingParams1(float gamma, float enhancedContrast, float enhancedContrastGrayscale, float clearTypeLevel,
            DWRITE_PIXEL_GEOMETRY pixelGeometry, DWRITE_RENDERING_MODE renderingMode, out IDWriteRenderingParams1 renderingParams);
        #endregion

        [PreserveSig]
        new HRESULT GetSystemFontFallback(out IDWriteFontFallback fontFallback);
        [PreserveSig]
        new HRESULT CreateFontFallbackBuilder(out IDWriteFontFallbackBuilder fontFallbackBuilder);
        [PreserveSig]
        new HRESULT TranslateColorGlyphRun(D2D1_POINT_2F baselineOriginX, D2D1_POINT_2F baselineOriginY, ref DWRITE_GLYPH_RUN glyphRun, IntPtr glyphRunDescription,
            DWRITE_MEASURING_MODE measuringMode, IntPtr worldToDeviceTransform, uint colorPaletteIndex, out IDWriteColorGlyphRunEnumerator colorLayers);
        [PreserveSig]
        new HRESULT CreateCustomRenderingParams2(float gamma, float enhancedContrast, float grayscaleEnhancedContrast, float clearTypeLevel, DWRITE_PIXEL_GEOMETRY pixelGeometry,
            DWRITE_RENDERING_MODE renderingMode, DWRITE_GRID_FIT_MODE gridFitMode, out IDWriteRenderingParams2 renderingParams);
        [PreserveSig]
        new HRESULT CreateGlyphRunAnalysis2(ref DWRITE_GLYPH_RUN glyphRun, DWRITE_MATRIX transform, DWRITE_RENDERING_MODE renderingMode, DWRITE_MEASURING_MODE measuringMode,
            DWRITE_GRID_FIT_MODE gridFitMode, DWRITE_TEXT_ANTIALIAS_MODE antialiasMode, float baselineOriginX, float baselineOriginY, out IDWriteGlyphRunAnalysis glyphRunAnalysis);
        #endregion

        [PreserveSig]
        new HRESULT CreateGlyphRunAnalysis3(ref DWRITE_GLYPH_RUN glyphRun, DWRITE_MATRIX transform, DWRITE_RENDERING_MODE1 renderingMode, DWRITE_MEASURING_MODE measuringMode,
            DWRITE_GRID_FIT_MODE gridFitMode, DWRITE_TEXT_ANTIALIAS_MODE antialiasMode, float baselineOriginX, float baselineOriginY, out IDWriteGlyphRunAnalysis glyphRunAnalysis);
        [PreserveSig]
        new HRESULT CreateCustomRenderingParams3(float gamma, float enhancedContrast, float grayscaleEnhancedContrast, float clearTypeLevel, DWRITE_PIXEL_GEOMETRY pixelGeometry,
            DWRITE_RENDERING_MODE1 renderingMode, DWRITE_GRID_FIT_MODE gridFitMode, out IDWriteRenderingParams3 renderingParams);
        //new HRESULT CreateFontFaceReference3(string filePath, System.Runtime.InteropServices.ComTypes.FILETIME lastWriteTime, uint faceIndex, DWRITE_FONT_SIMULATIONS fontSimulations, out IDWriteFontFaceReference fontFaceReference);
        [PreserveSig]
        new HRESULT CreateFontFaceReference3(string filePath, IntPtr lastWriteTime, uint faceIndex, DWRITE_FONT_SIMULATIONS fontSimulations, out IDWriteFontFaceReference fontFaceReference);
        [PreserveSig]
        new HRESULT CreateFontFaceReference3(IDWriteFontFile fontFile, uint faceIndex, DWRITE_FONT_SIMULATIONS fontSimulations, out IDWriteFontFaceReference fontFaceReference);
        [PreserveSig]
        new HRESULT GetSystemFontSet3(out IDWriteFontSet fontSet);
        [PreserveSig]
        new HRESULT CreateFontSetBuilder3(out IDWriteFontSetBuilder fontSetBuilder);
        [PreserveSig]
        new HRESULT CreateFontCollectionFromFontSet3(IDWriteFontSet fontSet, out IDWriteFontCollection1 fontCollection);
        [PreserveSig]
        new HRESULT GetSystemFontCollection3(bool includeDownloadableFonts, out IDWriteFontCollection1 fontCollection, bool checkForUpdates = false);
        [PreserveSig]
        new HRESULT GetFontDownloadQueue(out IDWriteFontDownloadQueue fontDownloadQueue);
        #endregion

        [PreserveSig]
        new HRESULT TranslateColorGlyphRun4(D2D1_POINT_2F baselineOrigin, ref DWRITE_GLYPH_RUN glyphRun, IntPtr glyphRunDescription,
            DWRITE_GLYPH_IMAGE_FORMATS desiredGlyphImageFormats, DWRITE_MEASURING_MODE measuringMode, IntPtr worldAndDpiTransform,
            uint colorPaletteIndex, out IDWriteColorGlyphRunEnumerator1 colorLayers);
        [PreserveSig]
        new HRESULT ComputeGlyphOrigins(ref DWRITE_GLYPH_RUN glyphRun, DWRITE_MEASURING_MODE measuringMode, D2D1_POINT_2F baselineOrigin, DWRITE_MATRIX worldAndDpiTransform,
            out D2D1_POINT_2F glyphOrigins);
        [PreserveSig]
        new HRESULT ComputeGlyphOrigins(ref DWRITE_GLYPH_RUN glyphRun, D2D1_POINT_2F baselineOrigin, out D2D1_POINT_2F glyphOrigins);
        #endregion

        [PreserveSig]
        new HRESULT CreateFontSetBuilder5(out IDWriteFontSetBuilder1 fontSetBuilder);
        [PreserveSig]
        new HRESULT CreateInMemoryFontFileLoader(out IDWriteInMemoryFontFileLoader newLoader);
        [PreserveSig]
        new HRESULT CreateHttpFontFileLoader(string referrerUrl, string extraHeaders, out IDWriteRemoteFontFileLoader newLoader);
        [PreserveSig]
        new DWRITE_CONTAINER_TYPE AnalyzeContainerType(IntPtr fileData, uint fileDataSize);
        [PreserveSig]
        new HRESULT UnpackFontFile(DWRITE_CONTAINER_TYPE containerType, IntPtr fileData, uint fileDataSize, out IDWriteFontFileStream unpackedFontStream);
        #endregion

        [PreserveSig]
        new HRESULT CreateFontFaceReference6(IDWriteFontFile fontFile, uint faceIndex, DWRITE_FONT_SIMULATIONS fontSimulations, DWRITE_FONT_AXIS_VALUE fontAxisValues,
            uint fontAxisValueCount, out IDWriteFontFaceReference1 fontFaceReference);
        [PreserveSig]
        new HRESULT CreateFontResource(IDWriteFontFile fontFile, uint faceIndex, out IDWriteFontResource fontResource);
        [PreserveSig]
        new HRESULT GetSystemFontSet6(bool includeDownloadableFonts, out IDWriteFontSet1 fontSet);
        [PreserveSig]
        new HRESULT GetSystemFontCollection6(bool includeDownloadableFonts, DWRITE_FONT_FAMILY_MODEL fontFamilyModel, out IDWriteFontCollection2 fontCollection);
        [PreserveSig]
        new HRESULT CreateFontCollectionFromFontSet6(IDWriteFontSet fontSet, DWRITE_FONT_FAMILY_MODEL fontFamilyModel, out IDWriteFontCollection2 fontCollection);
        [PreserveSig]
        new HRESULT CreateFontSetBuilder6(out IDWriteFontSetBuilder2 fontSetBuilder);
        [PreserveSig]
        new HRESULT CreateTextFormat6(string fontFamilyName, IDWriteFontCollection fontCollection, DWRITE_FONT_AXIS_VALUE fontAxisValues, uint fontAxisValueCount,
            float fontSize, string localeName, out IDWriteTextFormat3 textFormat);
        #endregion

        [PreserveSig]
        HRESULT GetSystemFontSet7(bool includeDownloadableFonts,out IDWriteFontSet2 fontSet);
        [PreserveSig]
        HRESULT GetSystemFontCollection7(bool includeDownloadableFonts, DWRITE_FONT_FAMILY_MODEL fontFamilyModel, out IDWriteFontCollection3 fontCollection);
    }

    [ComImport]
    [Guid("EE0A7FB5-DEF4-4C23-A454-C9C7DC878398")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFactory8 : IDWriteFactory7
    {
        #region IDWriteFactory6
        #region IDWriteFactory5
        #region IDWriteFactory4
        #region IDWriteFactory3
        #region IDWriteFactory2
        #region IDWriteFactory1
        #region IDWriteFactory
        new HRESULT GetSystemFontCollection(out IDWriteFontCollection fontCollection, bool checkForUpdates = false);
        [PreserveSig]
        new HRESULT CreateCustomFontCollection(IDWriteFontCollectionLoader collectionLoader, IntPtr collectionKey, int collectionKeySize, out IDWriteFontCollection fontCollection);
        [PreserveSig]
        new HRESULT RegisterFontCollectionLoader(IDWriteFontCollectionLoader fontCollectionLoader);
        new HRESULT UnregisterFontCollectionLoader(IDWriteFontCollectionLoader fontCollectionLoader);
        //new HRESULT CreateFontFileReference(string filePath, System.Runtime.InteropServices.ComTypes.FILETIME lastWriteTime, out IDWriteFontFile fontFile);
        [PreserveSig]
        new HRESULT CreateFontFileReference([MarshalAs(UnmanagedType.LPWStr)] string filePath, IntPtr lastWriteTime, out IDWriteFontFile fontFile);
        [PreserveSig]
        new HRESULT CreateCustomFontFileReference(IntPtr fontFileReferenceKey, uint fontFileReferenceKeySize, IDWriteFontFileLoader fontFileLoader, out IDWriteFontFile fontFile);
        [PreserveSig]
        new HRESULT CreateFontFace(DWRITE_FONT_FACE_TYPE fontFaceType, int numberOfFiles, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] IDWriteFontFile[] fontFiles, int faceIndex, DWRITE_FONT_SIMULATIONS fontFaceSimulationFlags, out IDWriteFontFace fontFace);
        [PreserveSig]
        new HRESULT CreateRenderingParams(out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        new HRESULT CreateMonitorRenderingParams(IntPtr monitor, out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        new HRESULT CreateCustomRenderingParams(float gamma, float enhancedContrast, float clearTypeLevel, DWRITE_PIXEL_GEOMETRY pixelGeometry, DWRITE_RENDERING_MODE renderingMode, out IDWriteRenderingParams renderingParams);
        [PreserveSig]
        new HRESULT RegisterFontFileLoader(IDWriteFontFileLoader fontFileLoader);
        new HRESULT UnregisterFontFileLoader(IDWriteFontFileLoader fontFileLoader);
        [PreserveSig]
        new HRESULT CreateTextFormat([MarshalAs(UnmanagedType.LPWStr)] string fontFamilyName, IDWriteFontCollection fontCollection, DWRITE_FONT_WEIGHT fontWeight, DWRITE_FONT_STYLE fontStyle, DWRITE_FONT_STRETCH fontStretch, float fontSize,
            [MarshalAs(UnmanagedType.LPWStr)] string localeName, out IDWriteTextFormat textFormat);
        [PreserveSig]
        new HRESULT CreateTypography(out IDWriteTypography typography);
        [PreserveSig]
        new HRESULT GetGdiInterop(out IDWriteGdiInterop gdiInterop);
        [PreserveSig]
        new HRESULT CreateTextLayout([MarshalAs(UnmanagedType.LPWStr)] string str, int stringLength, IDWriteTextFormat textFormat, float maxWidth, float maxHeight, out IDWriteTextLayout textLayout);
        [PreserveSig]
        new HRESULT CreateGdiCompatibleTextLayout([MarshalAs(UnmanagedType.LPWStr)] string str, int stringLength, IDWriteTextFormat textFormat, float layoutWidth,
            float layoutHeight, float pixelsPerDip, DWRITE_MATRIX transform, bool useGdiNatural, out IDWriteTextLayout textLayout);
        [PreserveSig]
        new HRESULT CreateEllipsisTrimmingSign(IDWriteTextFormat textFormat, out IDWriteInlineObject trimmingSign);
        [PreserveSig]
        new HRESULT CreateTextAnalyzer(out IDWriteTextAnalyzer textAnalyzer);
        [PreserveSig]
        new HRESULT CreateNumberSubstitution(DWRITE_NUMBER_SUBSTITUTION_METHOD substitutionMethod, string localeName, bool ignoreUserOverride, out IDWriteNumberSubstitution numberSubstitution);
        [PreserveSig]
        new HRESULT CreateGlyphRunAnalysis(ref DWRITE_GLYPH_RUN glyphRun, float pixelsPerDip, DWRITE_MATRIX transform, DWRITE_RENDERING_MODE renderingMode,
            DWRITE_MEASURING_MODE measuringMode, float baselineOriginX, float baselineOriginY, out IDWriteGlyphRunAnalysis glyphRunAnalysis);
        #endregion

        new HRESULT GetEudcFontCollection(out IDWriteFontCollection fontCollection, bool checkForUpdates = false);
        [PreserveSig]
        new HRESULT CreateCustomRenderingParams1(float gamma, float enhancedContrast, float enhancedContrastGrayscale, float clearTypeLevel,
            DWRITE_PIXEL_GEOMETRY pixelGeometry, DWRITE_RENDERING_MODE renderingMode, out IDWriteRenderingParams1 renderingParams);
        #endregion

        [PreserveSig]
        new HRESULT GetSystemFontFallback(out IDWriteFontFallback fontFallback);
        [PreserveSig]
        new HRESULT CreateFontFallbackBuilder(out IDWriteFontFallbackBuilder fontFallbackBuilder);
        [PreserveSig]
        new HRESULT TranslateColorGlyphRun(D2D1_POINT_2F baselineOriginX, D2D1_POINT_2F baselineOriginY, ref DWRITE_GLYPH_RUN glyphRun, IntPtr glyphRunDescription,
            DWRITE_MEASURING_MODE measuringMode, IntPtr worldToDeviceTransform, uint colorPaletteIndex, out IDWriteColorGlyphRunEnumerator colorLayers);
        [PreserveSig]
        new HRESULT CreateCustomRenderingParams2(float gamma, float enhancedContrast, float grayscaleEnhancedContrast, float clearTypeLevel, DWRITE_PIXEL_GEOMETRY pixelGeometry,
            DWRITE_RENDERING_MODE renderingMode, DWRITE_GRID_FIT_MODE gridFitMode, out IDWriteRenderingParams2 renderingParams);
        [PreserveSig]
        new HRESULT CreateGlyphRunAnalysis2(ref DWRITE_GLYPH_RUN glyphRun, DWRITE_MATRIX transform, DWRITE_RENDERING_MODE renderingMode, DWRITE_MEASURING_MODE measuringMode,
            DWRITE_GRID_FIT_MODE gridFitMode, DWRITE_TEXT_ANTIALIAS_MODE antialiasMode, float baselineOriginX, float baselineOriginY, out IDWriteGlyphRunAnalysis glyphRunAnalysis);
        #endregion

        [PreserveSig]
        new HRESULT CreateGlyphRunAnalysis3(ref DWRITE_GLYPH_RUN glyphRun, DWRITE_MATRIX transform, DWRITE_RENDERING_MODE1 renderingMode, DWRITE_MEASURING_MODE measuringMode,
            DWRITE_GRID_FIT_MODE gridFitMode, DWRITE_TEXT_ANTIALIAS_MODE antialiasMode, float baselineOriginX, float baselineOriginY, out IDWriteGlyphRunAnalysis glyphRunAnalysis);
        [PreserveSig]
        new HRESULT CreateCustomRenderingParams3(float gamma, float enhancedContrast, float grayscaleEnhancedContrast, float clearTypeLevel, DWRITE_PIXEL_GEOMETRY pixelGeometry,
            DWRITE_RENDERING_MODE1 renderingMode, DWRITE_GRID_FIT_MODE gridFitMode, out IDWriteRenderingParams3 renderingParams);
        //new HRESULT CreateFontFaceReference3(string filePath, System.Runtime.InteropServices.ComTypes.FILETIME lastWriteTime, uint faceIndex, DWRITE_FONT_SIMULATIONS fontSimulations, out IDWriteFontFaceReference fontFaceReference);
        [PreserveSig]
        new HRESULT CreateFontFaceReference3(string filePath, IntPtr lastWriteTime, uint faceIndex, DWRITE_FONT_SIMULATIONS fontSimulations, out IDWriteFontFaceReference fontFaceReference);
        [PreserveSig]
        new HRESULT CreateFontFaceReference3(IDWriteFontFile fontFile, uint faceIndex, DWRITE_FONT_SIMULATIONS fontSimulations, out IDWriteFontFaceReference fontFaceReference);
        [PreserveSig]
        new HRESULT GetSystemFontSet3(out IDWriteFontSet fontSet);
        [PreserveSig]
        new HRESULT CreateFontSetBuilder3(out IDWriteFontSetBuilder fontSetBuilder);
        [PreserveSig]
        new HRESULT CreateFontCollectionFromFontSet3(IDWriteFontSet fontSet, out IDWriteFontCollection1 fontCollection);
        [PreserveSig]
        new HRESULT GetSystemFontCollection3(bool includeDownloadableFonts, out IDWriteFontCollection1 fontCollection, bool checkForUpdates = false);
        [PreserveSig]
        new HRESULT GetFontDownloadQueue(out IDWriteFontDownloadQueue fontDownloadQueue);
        #endregion

        [PreserveSig]
        new HRESULT TranslateColorGlyphRun4(D2D1_POINT_2F baselineOrigin, ref DWRITE_GLYPH_RUN glyphRun, IntPtr glyphRunDescription,
            DWRITE_GLYPH_IMAGE_FORMATS desiredGlyphImageFormats, DWRITE_MEASURING_MODE measuringMode, IntPtr worldAndDpiTransform,
            uint colorPaletteIndex, out IDWriteColorGlyphRunEnumerator1 colorLayers);
        [PreserveSig]
        new HRESULT ComputeGlyphOrigins(ref DWRITE_GLYPH_RUN glyphRun, DWRITE_MEASURING_MODE measuringMode, D2D1_POINT_2F baselineOrigin, DWRITE_MATRIX worldAndDpiTransform,
            out D2D1_POINT_2F glyphOrigins);
        [PreserveSig]
        new HRESULT ComputeGlyphOrigins(ref DWRITE_GLYPH_RUN glyphRun, D2D1_POINT_2F baselineOrigin, out D2D1_POINT_2F glyphOrigins);
        #endregion

        [PreserveSig]
        new HRESULT CreateFontSetBuilder5(out IDWriteFontSetBuilder1 fontSetBuilder);
        [PreserveSig]
        new HRESULT CreateInMemoryFontFileLoader(out IDWriteInMemoryFontFileLoader newLoader);
        [PreserveSig]
        new HRESULT CreateHttpFontFileLoader(string referrerUrl, string extraHeaders, out IDWriteRemoteFontFileLoader newLoader);
        [PreserveSig]
        new DWRITE_CONTAINER_TYPE AnalyzeContainerType(IntPtr fileData, uint fileDataSize);
        [PreserveSig]
        new HRESULT UnpackFontFile(DWRITE_CONTAINER_TYPE containerType, IntPtr fileData, uint fileDataSize, out IDWriteFontFileStream unpackedFontStream);
        #endregion

        [PreserveSig]
        new HRESULT CreateFontFaceReference6(IDWriteFontFile fontFile, uint faceIndex, DWRITE_FONT_SIMULATIONS fontSimulations, DWRITE_FONT_AXIS_VALUE fontAxisValues,
            uint fontAxisValueCount, out IDWriteFontFaceReference1 fontFaceReference);
        [PreserveSig]
        new HRESULT CreateFontResource(IDWriteFontFile fontFile, uint faceIndex, out IDWriteFontResource fontResource);
        [PreserveSig]
        new HRESULT GetSystemFontSet6(bool includeDownloadableFonts, out IDWriteFontSet1 fontSet);
        [PreserveSig]
        new HRESULT GetSystemFontCollection6(bool includeDownloadableFonts, DWRITE_FONT_FAMILY_MODEL fontFamilyModel, out IDWriteFontCollection2 fontCollection);
        [PreserveSig]
        new HRESULT CreateFontCollectionFromFontSet6(IDWriteFontSet fontSet, DWRITE_FONT_FAMILY_MODEL fontFamilyModel, out IDWriteFontCollection2 fontCollection);
        [PreserveSig]
        new HRESULT CreateFontSetBuilder6(out IDWriteFontSetBuilder2 fontSetBuilder);
        [PreserveSig]
        new HRESULT CreateTextFormat6(string fontFamilyName, IDWriteFontCollection fontCollection, DWRITE_FONT_AXIS_VALUE fontAxisValues, uint fontAxisValueCount,
            float fontSize, string localeName, out IDWriteTextFormat3 textFormat);
        #endregion

        #region <IDWriteFactory7>
        [PreserveSig]
        new HRESULT GetSystemFontSet7(bool includeDownloadableFonts, out IDWriteFontSet2 fontSet);
        [PreserveSig]
        new HRESULT GetSystemFontCollection7(bool includeDownloadableFonts, DWRITE_FONT_FAMILY_MODEL fontFamilyModel, out IDWriteFontCollection3 fontCollection);
        #endregion

        [PreserveSig]
        HRESULT TranslateColorGlyphRun(D2D1_POINT_2F baselineOrigin, ref DWRITE_GLYPH_RUN glyphRun, IntPtr glyphRunDescription,
            DWRITE_GLYPH_IMAGE_FORMATS desiredGlyphImageFormats, DWRITE_PAINT_FEATURE_LEVEL paintFeatureLevel,
            DWRITE_MEASURING_MODE measuringMode, IntPtr worldAndDpiTransform, uint colorPaletteIndex, out IDWriteColorGlyphRunEnumerator1 colorEnumerator);
    }

    [ComImport]
    [Guid("53585141-D9F8-4095-8321-D73CF6BD116B")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontSet
    {
        [PreserveSig]
        uint GetFontCount();
        HRESULT GetFontFaceReference(uint listIndex, out IDWriteFontFaceReference fontFaceReference);
        HRESULT FindFontFaceReference(IDWriteFontFaceReference fontFaceReference, out uint listIndex, out bool exists);
        HRESULT FindFontFace(IDWriteFontFace fontFace, out uint listIndex, out bool exists);
       
        HRESULT GetPropertyValues(DWRITE_FONT_PROPERTY_ID propertyID, out IDWriteStringList values);
        HRESULT GetPropertyValues(DWRITE_FONT_PROPERTY_ID propertyID, [MarshalAs(UnmanagedType.LPWStr)] string preferredLocaleNames, out IDWriteStringList values);
        HRESULT GetPropertyValues(uint listIndex, DWRITE_FONT_PROPERTY_ID propertyId, out bool exists, out IDWriteLocalizedStrings values);

        HRESULT GetPropertyOccurrenceCount(DWRITE_FONT_PROPERTY  property, out uint propertyOccurrenceCount);
        HRESULT GetMatchingFonts(DWRITE_FONT_PROPERTY properties, uint propertyCount, out IDWriteFontSet filteredSet);
        HRESULT GetMatchingFonts(string familyName, DWRITE_FONT_WEIGHT fontWeight, DWRITE_FONT_STRETCH fontStretch, DWRITE_FONT_STYLE fontStyle, out IDWriteFontSet filteredSet);
    }

    [ComImport]
    [Guid("7E9FDA85-6C92-4053-BC47-7AE3530DB4D3")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontSet1 : IDWriteFontSet
    {
        #region IDWriteFontSet
        [PreserveSig]
        new uint GetFontCount();
        new HRESULT GetFontFaceReference(uint listIndex, out IDWriteFontFaceReference fontFaceReference);
        new HRESULT FindFontFaceReference(IDWriteFontFaceReference fontFaceReference, out uint listIndex, out bool exists);
        new HRESULT FindFontFace(IDWriteFontFace fontFace, out uint listIndex, out bool exists);

        new HRESULT GetPropertyValues(DWRITE_FONT_PROPERTY_ID propertyID, out IDWriteStringList values);
        new HRESULT GetPropertyValues(DWRITE_FONT_PROPERTY_ID propertyID, [MarshalAs(UnmanagedType.LPWStr)] string preferredLocaleNames, out IDWriteStringList values);
        new HRESULT GetPropertyValues(uint listIndex, DWRITE_FONT_PROPERTY_ID propertyId, out bool exists, out IDWriteLocalizedStrings values);

        new HRESULT GetPropertyOccurrenceCount(DWRITE_FONT_PROPERTY property, out uint propertyOccurrenceCount);
        new HRESULT GetMatchingFonts(DWRITE_FONT_PROPERTY properties, uint propertyCount, out IDWriteFontSet filteredSet);
        new HRESULT GetMatchingFonts(string familyName, DWRITE_FONT_WEIGHT fontWeight, DWRITE_FONT_STRETCH fontStretch, DWRITE_FONT_STYLE fontStyle, out IDWriteFontSet filteredSet);
        #endregion

        HRESULT GetMatchingFonts(DWRITE_FONT_PROPERTY fontProperty, DWRITE_FONT_AXIS_VALUE fontAxisValues, uint fontAxisValueCount, out IDWriteFontSet1 matchingFonts);
        HRESULT GetFirstFontResources(out IDWriteFontSet1 filteredFontSet);
        HRESULT GetFilteredFonts(DWRITE_FONT_PROPERTY properties, uint propertyCount, bool selectAnyProperty, out IDWriteFontSet1 filteredFontSet);
        HRESULT GetFilteredFonts(DWRITE_FONT_AXIS_RANGE fontAxisRanges, uint fontAxisRangeCount, bool selectAnyRange, out IDWriteFontSet1 filteredFontSet);
        HRESULT GetFilteredFonts(uint indices, uint indexCount, out IDWriteFontSet1 filteredFontSet);
        HRESULT GetFilteredFontIndices(DWRITE_FONT_PROPERTY properties, uint propertyCount, bool selectAnyProperty, out uint indices,
            uint maxIndexCount, out uint actualIndexCount);
        HRESULT GetFilteredFontIndices(DWRITE_FONT_AXIS_RANGE fontAxisRanges, uint fontAxisRangeCount, bool selectAnyRange, out uint indices,
            uint maxIndexCount, out uint actualIndexCount);
        HRESULT GetFontAxisRanges(out DWRITE_FONT_AXIS_RANGE fontAxisRanges,
            uint maxFontAxisRangeCount, out uint actualFontAxisRangeCount);
        HRESULT GetFontAxisRanges(uint listIndex, out DWRITE_FONT_AXIS_RANGE fontAxisRanges, uint maxFontAxisRangeCount, out uint actualFontAxisRangeCount);
        HRESULT GetFontFaceReference(uint listIndex, out IDWriteFontFaceReference1 fontFaceReference);
        [PreserveSig]
        HRESULT CreateFontResource(uint listIndex, out IDWriteFontResource fontResource);
        [PreserveSig]
        HRESULT CreateFontFace(uint listIndex, out IDWriteFontFace5 fontFace);
        DWRITE_LOCALITY GetFontLocality(uint listIndex);
    }

    [ComImport]
    [Guid("DC7EAD19-E54C-43AF-B2DA-4E2B79BA3F7F")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontSet2 : IDWriteFontSet1
    {
        #region IDWriteFontSet1
        #region IDWriteFontSet
        [PreserveSig]
        new uint GetFontCount();
        new HRESULT GetFontFaceReference(uint listIndex, out IDWriteFontFaceReference fontFaceReference);
        new HRESULT FindFontFaceReference(IDWriteFontFaceReference fontFaceReference, out uint listIndex, out bool exists);
        new HRESULT FindFontFace(IDWriteFontFace fontFace, out uint listIndex, out bool exists);

        new HRESULT GetPropertyValues(DWRITE_FONT_PROPERTY_ID propertyID, out IDWriteStringList values);
        new HRESULT GetPropertyValues(DWRITE_FONT_PROPERTY_ID propertyID, [MarshalAs(UnmanagedType.LPWStr)] string preferredLocaleNames, out IDWriteStringList values);
        new HRESULT GetPropertyValues(uint listIndex, DWRITE_FONT_PROPERTY_ID propertyId, out bool exists, out IDWriteLocalizedStrings values);

        new HRESULT GetPropertyOccurrenceCount(DWRITE_FONT_PROPERTY property, out uint propertyOccurrenceCount);
        new HRESULT GetMatchingFonts(DWRITE_FONT_PROPERTY properties, uint propertyCount, out IDWriteFontSet filteredSet);
        new HRESULT GetMatchingFonts(string familyName, DWRITE_FONT_WEIGHT fontWeight, DWRITE_FONT_STRETCH fontStretch, DWRITE_FONT_STYLE fontStyle, out IDWriteFontSet filteredSet);
        #endregion

        new HRESULT GetMatchingFonts(DWRITE_FONT_PROPERTY fontProperty, DWRITE_FONT_AXIS_VALUE fontAxisValues, uint fontAxisValueCount, out IDWriteFontSet1 matchingFonts);
        new HRESULT GetFirstFontResources(out IDWriteFontSet1 filteredFontSet);
        new HRESULT GetFilteredFonts(DWRITE_FONT_PROPERTY properties, uint propertyCount, bool selectAnyProperty, out IDWriteFontSet1 filteredFontSet);
        new HRESULT GetFilteredFonts(DWRITE_FONT_AXIS_RANGE fontAxisRanges, uint fontAxisRangeCount, bool selectAnyRange, out IDWriteFontSet1 filteredFontSet);
        new HRESULT GetFilteredFonts(uint indices, uint indexCount, out IDWriteFontSet1 filteredFontSet);
        new HRESULT GetFilteredFontIndices(DWRITE_FONT_PROPERTY properties, uint propertyCount, bool selectAnyProperty, out uint indices,
            uint maxIndexCount, out uint actualIndexCount);
        new HRESULT GetFilteredFontIndices(DWRITE_FONT_AXIS_RANGE fontAxisRanges, uint fontAxisRangeCount, bool selectAnyRange, out uint indices,
            uint maxIndexCount, out uint actualIndexCount);
        new HRESULT GetFontAxisRanges(out DWRITE_FONT_AXIS_RANGE fontAxisRanges,
            uint maxFontAxisRangeCount, out uint actualFontAxisRangeCount);
        new HRESULT GetFontAxisRanges(uint listIndex, out DWRITE_FONT_AXIS_RANGE fontAxisRanges, uint maxFontAxisRangeCount, out uint actualFontAxisRangeCount);
        new HRESULT GetFontFaceReference(uint listIndex, out IDWriteFontFaceReference1 fontFaceReference);
        [PreserveSig]
        new HRESULT CreateFontResource(uint listIndex, out IDWriteFontResource fontResource);
        [PreserveSig]
        new HRESULT CreateFontFace(uint listIndex, out IDWriteFontFace5 fontFace);
        new DWRITE_LOCALITY GetFontLocality(uint listIndex);
        #endregion

        IntPtr GetExpirationEvent();
    }

    [ComImport]
    [Guid("7C073EF2-A7F4-4045-8C32-8AB8AE640F90")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontSet3 : IDWriteFontSet2
    {
        #region IDWriteFontSet2
        #region IDWriteFontSet1
        #region IDWriteFontSet
        [PreserveSig]
        new uint GetFontCount();
        new HRESULT GetFontFaceReference(uint listIndex, out IDWriteFontFaceReference fontFaceReference);
        new HRESULT FindFontFaceReference(IDWriteFontFaceReference fontFaceReference, out uint listIndex, out bool exists);
        new HRESULT FindFontFace(IDWriteFontFace fontFace, out uint listIndex, out bool exists);
      
        new HRESULT GetPropertyValues(DWRITE_FONT_PROPERTY_ID propertyID, out IDWriteStringList values);
        new HRESULT GetPropertyValues(DWRITE_FONT_PROPERTY_ID propertyID, [MarshalAs(UnmanagedType.LPWStr)] string preferredLocaleNames, out IDWriteStringList values);
        new HRESULT GetPropertyValues(uint listIndex, DWRITE_FONT_PROPERTY_ID propertyId, out bool exists, out IDWriteLocalizedStrings values);

        new HRESULT GetPropertyOccurrenceCount(DWRITE_FONT_PROPERTY property, out uint propertyOccurrenceCount);
        new HRESULT GetMatchingFonts(DWRITE_FONT_PROPERTY properties, uint propertyCount, out IDWriteFontSet filteredSet);
        new HRESULT GetMatchingFonts(string familyName, DWRITE_FONT_WEIGHT fontWeight, DWRITE_FONT_STRETCH fontStretch, DWRITE_FONT_STYLE fontStyle, out IDWriteFontSet filteredSet);
        #endregion

        new HRESULT GetMatchingFonts(DWRITE_FONT_PROPERTY fontProperty, DWRITE_FONT_AXIS_VALUE fontAxisValues, uint fontAxisValueCount, out IDWriteFontSet1 matchingFonts);
        new HRESULT GetFirstFontResources(out IDWriteFontSet1 filteredFontSet);
        new HRESULT GetFilteredFonts(DWRITE_FONT_PROPERTY properties, uint propertyCount, bool selectAnyProperty, out IDWriteFontSet1 filteredFontSet);
        new HRESULT GetFilteredFonts(DWRITE_FONT_AXIS_RANGE fontAxisRanges, uint fontAxisRangeCount, bool selectAnyRange, out IDWriteFontSet1 filteredFontSet);
        new HRESULT GetFilteredFonts(uint indices, uint indexCount, out IDWriteFontSet1 filteredFontSet);
        new HRESULT GetFilteredFontIndices(DWRITE_FONT_PROPERTY properties, uint propertyCount, bool selectAnyProperty, out uint indices,
            uint maxIndexCount, out uint actualIndexCount);
        new HRESULT GetFilteredFontIndices(DWRITE_FONT_AXIS_RANGE fontAxisRanges, uint fontAxisRangeCount, bool selectAnyRange, out uint indices,
            uint maxIndexCount, out uint actualIndexCount);
        new HRESULT GetFontAxisRanges(out DWRITE_FONT_AXIS_RANGE fontAxisRanges,
            uint maxFontAxisRangeCount, out uint actualFontAxisRangeCount);
        new HRESULT GetFontAxisRanges(uint listIndex, out DWRITE_FONT_AXIS_RANGE fontAxisRanges, uint maxFontAxisRangeCount, out uint actualFontAxisRangeCount);
        new HRESULT GetFontFaceReference(uint listIndex, out IDWriteFontFaceReference1 fontFaceReference);
        [PreserveSig]
        new HRESULT CreateFontResource(uint listIndex, out IDWriteFontResource fontResource);
        [PreserveSig]
        new HRESULT CreateFontFace(uint listIndex, out IDWriteFontFace5 fontFace);
        new DWRITE_LOCALITY GetFontLocality(uint listIndex);
        #endregion

        new IntPtr GetExpirationEvent();
        #endregion

        [PreserveSig]
        DWRITE_FONT_SOURCE_TYPE GetFontSourceType(uint fontIndex);
        [PreserveSig]
        uint GetFontSourceNameLength(uint listIndex);
        HRESULT GetFontSourceName(uint listIndex, System.Text.StringBuilder stringBuffer, uint stringBufferSize);
    }

    [ComImport]
    [Guid("EEC175FC-BEA9-4C86-8B53-CCBDD7DF0C82")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontSet4 : IDWriteFontSet3
    {
        #region IDWriteFontSet2
        #region IDWriteFontSet1
        #region IDWriteFontSet
        [PreserveSig]
        new uint GetFontCount();
        new HRESULT GetFontFaceReference(uint listIndex, out IDWriteFontFaceReference fontFaceReference);
        new HRESULT FindFontFaceReference(IDWriteFontFaceReference fontFaceReference, out uint listIndex, out bool exists);
        new HRESULT FindFontFace(IDWriteFontFace fontFace, out uint listIndex, out bool exists);

        new HRESULT GetPropertyValues(DWRITE_FONT_PROPERTY_ID propertyID, out IDWriteStringList values);
        new HRESULT GetPropertyValues(DWRITE_FONT_PROPERTY_ID propertyID, [MarshalAs(UnmanagedType.LPWStr)] string preferredLocaleNames, out IDWriteStringList values);
        new HRESULT GetPropertyValues(uint listIndex, DWRITE_FONT_PROPERTY_ID propertyId, out bool exists, out IDWriteLocalizedStrings values);

        new HRESULT GetPropertyOccurrenceCount(DWRITE_FONT_PROPERTY property, out uint propertyOccurrenceCount);
        new HRESULT GetMatchingFonts(DWRITE_FONT_PROPERTY properties, uint propertyCount, out IDWriteFontSet filteredSet);
        new HRESULT GetMatchingFonts(string familyName, DWRITE_FONT_WEIGHT fontWeight, DWRITE_FONT_STRETCH fontStretch, DWRITE_FONT_STYLE fontStyle, out IDWriteFontSet filteredSet);
        #endregion

        new HRESULT GetMatchingFonts(DWRITE_FONT_PROPERTY fontProperty, DWRITE_FONT_AXIS_VALUE fontAxisValues, uint fontAxisValueCount, out IDWriteFontSet1 matchingFonts);
        new HRESULT GetFirstFontResources(out IDWriteFontSet1 filteredFontSet);
        new HRESULT GetFilteredFonts(DWRITE_FONT_PROPERTY properties, uint propertyCount, bool selectAnyProperty, out IDWriteFontSet1 filteredFontSet);
        new HRESULT GetFilteredFonts(DWRITE_FONT_AXIS_RANGE fontAxisRanges, uint fontAxisRangeCount, bool selectAnyRange, out IDWriteFontSet1 filteredFontSet);
        new HRESULT GetFilteredFonts(uint indices, uint indexCount, out IDWriteFontSet1 filteredFontSet);
        new HRESULT GetFilteredFontIndices(DWRITE_FONT_PROPERTY properties, uint propertyCount, bool selectAnyProperty, out uint indices,
            uint maxIndexCount, out uint actualIndexCount);
        new HRESULT GetFilteredFontIndices(DWRITE_FONT_AXIS_RANGE fontAxisRanges, uint fontAxisRangeCount, bool selectAnyRange, out uint indices,
            uint maxIndexCount, out uint actualIndexCount);
        new HRESULT GetFontAxisRanges(out DWRITE_FONT_AXIS_RANGE fontAxisRanges,
            uint maxFontAxisRangeCount, out uint actualFontAxisRangeCount);
        new HRESULT GetFontAxisRanges(uint listIndex, out DWRITE_FONT_AXIS_RANGE fontAxisRanges, uint maxFontAxisRangeCount, out uint actualFontAxisRangeCount);
        new HRESULT GetFontFaceReference(uint listIndex, out IDWriteFontFaceReference1 fontFaceReference);
        [PreserveSig]
        new HRESULT CreateFontResource(uint listIndex, out IDWriteFontResource fontResource);
        [PreserveSig]
        new HRESULT CreateFontFace(uint listIndex, out IDWriteFontFace5 fontFace);
        new DWRITE_LOCALITY GetFontLocality(uint listIndex);
        #endregion

        new IntPtr GetExpirationEvent();
        #endregion

        #region <IDWriteFontSet3>
        [PreserveSig]
        new DWRITE_FONT_SOURCE_TYPE GetFontSourceType(uint fontIndex);
        [PreserveSig]
        new uint GetFontSourceNameLength(uint listIndex);
        [PreserveSig]
        new HRESULT GetFontSourceName(uint listIndex, System.Text.StringBuilder stringBuffer, uint stringBufferSize);
        #endregion

        [PreserveSig]
        uint ConvertWeightStretchStyleToFontAxisValues([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] DWRITE_FONT_AXIS_VALUE[] inputAxisValues,
            uint inputAxisCount, DWRITE_FONT_WEIGHT fontWeight, DWRITE_FONT_STRETCH fontStretch, DWRITE_FONT_STYLE fontStyle,
            float fontSize, [Out, MarshalAs(UnmanagedType.LPArray)] DWRITE_FONT_AXIS_VALUE[] outputAxisValues);

        [PreserveSig]
        HRESULT GetMatchingFonts([MarshalAs(UnmanagedType.LPWStr)] string familyName, 
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] DWRITE_FONT_AXIS_VALUE[] fontAxisValues, 
            int fontAxisValueCount, DWRITE_FONT_SIMULATIONS allowedSimulations, out IDWriteFontSet4 matchingFonts);
    }

    [ComImport]
    [Guid("1F803A76-6871-48E8-987F-B975551C50F2")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontResource
    {
        HRESULT GetFontFile(out IDWriteFontFile fontFile);
        [PreserveSig]
        uint GetFontFaceIndex();
        [PreserveSig]
        uint GetFontAxisCount();
        [PreserveSig]
        HRESULT GetDefaultFontAxisValues(out DWRITE_FONT_AXIS_VALUE fontAxisValues, uint fontAxisValueCount);
        [PreserveSig]
        HRESULT GetFontAxisRanges(out DWRITE_FONT_AXIS_RANGE fontAxisRanges, uint fontAxisRangeCount);
        [PreserveSig]
        DWRITE_FONT_AXIS_ATTRIBUTES GetFontAxisAttributes(uint axisIndex);
        [PreserveSig]
        HRESULT GetAxisNames(uint axisIndex, out IDWriteLocalizedStrings names);
        [PreserveSig]
        uint GetAxisValueNameCount(uint axisIndex);
        [PreserveSig]
        HRESULT GetAxisValueNames(uint axisIndex, uint axisValueIndex, out DWRITE_FONT_AXIS_RANGE fontAxisRange, out IDWriteLocalizedStrings names);
        [PreserveSig]
        HRESULT CreateFontFace(DWRITE_FONT_SIMULATIONS fontSimulations, DWRITE_FONT_AXIS_VALUE fontAxisValues, uint fontAxisValueCount, out IDWriteFontFace5 fontFace);
        [PreserveSig]
        HRESULT CreateFontFaceReference(DWRITE_FONT_SIMULATIONS fontSimulations, DWRITE_FONT_AXIS_VALUE  fontAxisValues, uint fontAxisValueCount, out IDWriteFontFaceReference1 fontFaceReference);
    }  

    [ComImport]
    [Guid("B71E6052-5AEA-4FA3-832E-F60D431F7E91")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontDownloadQueue
    {
        HRESULT AddListener(IDWriteFontDownloadListener listener, out uint token);
        HRESULT RemoveListener(uint token);
        bool IsEmpty();
        HRESULT BeginDownload(IntPtr context);
        HRESULT CancelDownload();
        [PreserveSig]
        UInt64 GetGenerationCount();
    }

    [ComImport]
    [Guid("B06FE5B9-43EC-4393-881B-DBE4DC72FDA7")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontDownloadListener
    {
        void DownloadCompleted(IDWriteFontDownloadQueue downloadQueue, IntPtr context, HRESULT downloadResult);
    }

    [ComImport]
    [Guid("CFEE3140-1157-47CA-8B85-31BFCF3F2D0E")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteStringList
    {
        [PreserveSig]
        uint GetCount();
        HRESULT GetLocaleNameLength(uint listIndex, out uint length);
        HRESULT GetLocaleName(uint listIndex, System.Text.StringBuilder localeName, uint size);
        HRESULT GetStringLength(uint listIndex, out uint length);
        HRESULT GetString(uint listIndex, System.Text.StringBuilder stringBuffer, uint stringBufferSize);
    }

    [ComImport]
    [Guid("5E7FA7CA-DDE3-424C-89F0-9FCD6FED58CD")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontFaceReference
    {
        [PreserveSig]
        HRESULT CreateFontFace(out IDWriteFontFace3 fontFace);
        [PreserveSig]
        HRESULT CreateFontFaceWithSimulations(DWRITE_FONT_SIMULATIONS fontFaceSimulationFlags, out IDWriteFontFace3 fontFace);
        [PreserveSig]
        bool Equals(IDWriteFontFaceReference fontFaceReference);
        [PreserveSig]
        uint GetFontFaceIndex();
        [PreserveSig]
        DWRITE_FONT_SIMULATIONS GetSimulations();
        [PreserveSig]
        HRESULT GetFontFile(out IDWriteFontFile fontFile);
        [PreserveSig]
        UInt64 GetLocalFileSize();
        [PreserveSig]
        UInt64 GetFileSize();
        [PreserveSig]
        HRESULT GetFileTime(out System.Runtime.InteropServices.ComTypes.FILETIME lastWriteTime);
        [PreserveSig]
        DWRITE_LOCALITY GetLocality();
        [PreserveSig]
        HRESULT EnqueueFontDownloadRequest();
        [PreserveSig]
        HRESULT EnqueueCharacterDownloadRequest(string characters, uint characterCount);
        [PreserveSig]
        HRESULT EnqueueGlyphDownloadRequest(UInt16 glyphIndices, uint glyphCount);
        [PreserveSig]
        HRESULT EnqueueFileFragmentDownloadRequest(UInt64 fileOffset, UInt64 fragmentSize);
    }

    [ComImport]
    [Guid("C081FE77-2FD1-41AC-A5A3-34983C4BA61A")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontFaceReference1 : IDWriteFontFaceReference
    {
        #region IDWriteFontFaceReference
        [PreserveSig]
        new HRESULT CreateFontFace(out IDWriteFontFace3 fontFace);
        [PreserveSig]
        new HRESULT CreateFontFaceWithSimulations(DWRITE_FONT_SIMULATIONS fontFaceSimulationFlags, out IDWriteFontFace3 fontFace);
        [PreserveSig]
        new bool Equals(IDWriteFontFaceReference fontFaceReference);
        [PreserveSig]
        new uint GetFontFaceIndex();
        [PreserveSig]
        new DWRITE_FONT_SIMULATIONS GetSimulations();
        [PreserveSig]
        new HRESULT GetFontFile(out IDWriteFontFile fontFile);
        [PreserveSig]
        new UInt64 GetLocalFileSize();
        [PreserveSig]
        new UInt64 GetFileSize();
        [PreserveSig]
        new HRESULT GetFileTime(out System.Runtime.InteropServices.ComTypes.FILETIME lastWriteTime);
        [PreserveSig]
        new DWRITE_LOCALITY GetLocality();
        [PreserveSig]
        new HRESULT EnqueueFontDownloadRequest();
        [PreserveSig]
        new HRESULT EnqueueCharacterDownloadRequest(string characters, uint characterCount);
        [PreserveSig]
        new HRESULT EnqueueGlyphDownloadRequest(UInt16 glyphIndices, uint glyphCount);
        [PreserveSig]
        new HRESULT EnqueueFileFragmentDownloadRequest(UInt64 fileOffset, UInt64 fragmentSize);
        #endregion

        [PreserveSig]
        HRESULT CreateFontFace(out IDWriteFontFace5 fontFace);
        [PreserveSig]
        uint GetFontAxisValueCount();
        [PreserveSig]
        HRESULT GetFontAxisValues(out DWRITE_FONT_AXIS_VALUE fontAxisValues, uint fontAxisValueCount);
    }

    [ComImport]
    [Guid("2F642AFE-9C68-4F40-B8BE-457401AFCB3D")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontSetBuilder
    {
        HRESULT AddFontFaceReference(IDWriteFontFaceReference fontFaceReference);
        HRESULT AddFontFaceReference(IDWriteFontFaceReference fontFaceReference, DWRITE_FONT_PROPERTY properties, uint propertyCount);
        HRESULT AddFontSet(IDWriteFontSet fontSet);
        [PreserveSig]
        HRESULT CreateFontSet(out IDWriteFontSet fontSet);
    }

    [ComImport]
    [Guid("3FF7715F-3CDC-4DC6-9B72-EC5621DCCAFD")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontSetBuilder1 : IDWriteFontSetBuilder
    {
        #region IDWriteFontSetBuilder
        new HRESULT AddFontFaceReference(IDWriteFontFaceReference fontFaceReference);
        new HRESULT AddFontFaceReference(IDWriteFontFaceReference fontFaceReference, DWRITE_FONT_PROPERTY properties, uint propertyCount);
        new HRESULT AddFontSet(IDWriteFontSet fontSet);
        [PreserveSig]
        new HRESULT CreateFontSet(out IDWriteFontSet fontSet);
        #endregion

        HRESULT AddFontFile(IDWriteFontFile fontFile);
    }

    [ComImport]
    [Guid("EE5BA612-B131-463C-8F4F-3189B9401E45")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontSetBuilder2 : IDWriteFontSetBuilder1
    {
        #region IDWriteFontSetBuilder1
        #region IDWriteFontSetBuilder
        new HRESULT AddFontFaceReference(IDWriteFontFaceReference fontFaceReference);
        new HRESULT AddFontFaceReference(IDWriteFontFaceReference fontFaceReference, DWRITE_FONT_PROPERTY properties, uint propertyCount);
        new HRESULT AddFontSet(IDWriteFontSet fontSet);
        [PreserveSig]
        new HRESULT CreateFontSet(out IDWriteFontSet fontSet);
        #endregion

        new HRESULT AddFontFile(IDWriteFontFile fontFile);
        #endregion

        HRESULT AddFont(IDWriteFontFile fontFile, uint fontFaceIndex, DWRITE_FONT_SIMULATIONS fontSimulations, DWRITE_FONT_AXIS_VALUE fontAxisValues,
            uint fontAxisValueCount, DWRITE_FONT_AXIS_RANGE fontAxisRanges, uint fontAxisRangeCount, DWRITE_FONT_PROPERTY properties, uint propertyCount);
        HRESULT AddFontFile(string filePath);
    }

    [ComImport]
    [Guid("CE25F8FD-863B-4D13-9651-C1F88DC73FE2")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteAsyncResult
    {
        [PreserveSig]
        IntPtr GetWaitHandle();
        [PreserveSig]
        HRESULT GetResult();
    }

    [ComImport]
    [Guid("d31fbe17-f157-41a2-8d24-cb779e0560e8")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteColorGlyphRunEnumerator
    {
        [PreserveSig]
        HRESULT MoveNext(out bool hasRun);
        [PreserveSig]
        HRESULT GetCurrentRun(out IntPtr colorGlyphRun);
    }

    [ComImport]
    [Guid("7C5F86DA-C7A1-4F05-B8E1-55A179FE5A35")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteColorGlyphRunEnumerator1 : IDWriteColorGlyphRunEnumerator
    {
        #region IDWriteColorGlyphRunEnumerator
        [PreserveSig]
        new HRESULT MoveNext(out bool hasRun);
        [PreserveSig]
        //new HRESULT GetCurrentRun(out DWRITE_COLOR_GLYPH_RUN colorGlyphRun);
        new HRESULT GetCurrentRun(out IntPtr colorGlyphRun);
        #endregion

        [PreserveSig]
        //HRESULT GetCurrentRun(out DWRITE_COLOR_GLYPH_RUN1 colorGlyphRun);
        HRESULT GetCurrentRun1(out IntPtr colorGlyphRun);
    }

    [ComImport]
    [Guid("a84cee02-3eea-4eee-a827-87c1a02a0fcc")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontCollection
    {
        //[return: MarshalAs(UnmanagedType.U4)]
        [PreserveSig]
        uint GetFontFamilyCount();
        HRESULT GetFontFamily(uint index, out IDWriteFontFamily fontFamily);
        HRESULT FindFamilyName(string familyName, out uint index, out bool exists);
        HRESULT GetFontFromFontFace(IDWriteFontFace fontFace, out IDWriteFont font);
    }

    [ComImport]
    [Guid("53585141-D9F8-4095-8321-D73CF6BD116C")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontCollection1 : IDWriteFontCollection
    {
        #region IDWriteFontCollection
        [PreserveSig]
        new uint GetFontFamilyCount();
        new HRESULT GetFontFamily(uint index, out IDWriteFontFamily fontFamily);
        new HRESULT FindFamilyName(string familyName, out uint index, out bool exists);
        new HRESULT GetFontFromFontFace(IDWriteFontFace fontFace, out IDWriteFont font);
        #endregion

        HRESULT GetFontSet(out IDWriteFontSet fontSet);
        HRESULT GetFontFamily(uint index, out IDWriteFontFamily1 fontFamily);
    }

    [ComImport]
    [Guid("514039C6-4617-4064-BF8B-92EA83E506E0")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontCollection2 : IDWriteFontCollection1
    {
        #region IDWriteFontCollection1
        #region IDWriteFontCollection
        [PreserveSig]
        new uint GetFontFamilyCount();
        [PreserveSig]
        new HRESULT GetFontFamily(uint index, out IDWriteFontFamily fontFamily);
        [PreserveSig]
        new HRESULT FindFamilyName(string familyName, out uint index, out bool exists);
        [PreserveSig]
        new HRESULT GetFontFromFontFace(IDWriteFontFace fontFace, out IDWriteFont font);
        #endregion

        [PreserveSig]
        new HRESULT GetFontSet(out IDWriteFontSet fontSet);
        [PreserveSig]
        new HRESULT GetFontFamily(uint index, out IDWriteFontFamily1 fontFamily);
        #endregion

        [PreserveSig]
        HRESULT GetFontFamily(uint index, out IDWriteFontFamily2 fontFamily);
        [PreserveSig]
        HRESULT GetMatchingFonts(string familyName, DWRITE_FONT_AXIS_VALUE fontAxisValues, uint fontAxisValueCount, out IDWriteFontList2 fontList);
        [PreserveSig]
        DWRITE_FONT_FAMILY_MODEL GetFontFamilyModel();
        [PreserveSig]
        HRESULT GetFontSet(out IDWriteFontSet1 fontSet);
    }

    [ComImport]
    [Guid("A4D055A6-F9E3-4E25-93B7-9E309F3AF8E9")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontCollection3 : IDWriteFontCollection2
    {
        #region IDWriteFontCollection2
        #region IDWriteFontCollection1
        #region IDWriteFontCollection
        [PreserveSig]
        new uint GetFontFamilyCount();
        [PreserveSig]
        new HRESULT GetFontFamily(uint index, out IDWriteFontFamily fontFamily);
        [PreserveSig]
        new HRESULT FindFamilyName(string familyName, out uint index, out bool exists);
        [PreserveSig]
        new HRESULT GetFontFromFontFace(IDWriteFontFace fontFace, out IDWriteFont font);
        #endregion

        [PreserveSig]
        new HRESULT GetFontSet(out IDWriteFontSet fontSet);
        [PreserveSig]
        new HRESULT GetFontFamily(uint index, out IDWriteFontFamily1 fontFamily);
        #endregion

        [PreserveSig]
        new HRESULT GetFontFamily(uint index, out IDWriteFontFamily2 fontFamily);
        [PreserveSig]
        new HRESULT GetMatchingFonts(string familyName, DWRITE_FONT_AXIS_VALUE fontAxisValues, uint fontAxisValueCount, out IDWriteFontList2 fontList);
        [PreserveSig]
        new DWRITE_FONT_FAMILY_MODEL GetFontFamilyModel();
        [PreserveSig]
        new HRESULT GetFontSet(out IDWriteFontSet1 fontSet);
        #endregion

        [PreserveSig]
        IntPtr GetExpirationEvent();
    }

    [ComImport]
    [Guid("cca920e4-52f0-492b-bfa8-29c72ee0a468")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontCollectionLoader
    {
        [PreserveSig]
        HRESULT CreateEnumeratorFromKey(IDWriteFactory factory, IntPtr collectionKey, uint collectionKeySize, out IDWriteFontFileEnumerator fontFileEnumerator);
        //HRESULT CreateEnumeratorFromKey(IntPtr factory, IntPtr collectionKey, uint collectionKeySize, out IntPtr fontFileEnumerator);
    }

    [ComImport]
    [Guid("72755049-5ff7-435d-8348-4be97cfa6c7c")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontFileEnumerator
    {
        [PreserveSig]
        HRESULT MoveNext([MarshalAs(UnmanagedType.Bool)] out bool hasCurrentFile);
        [PreserveSig]
        HRESULT GetCurrentFontFile(out IDWriteFontFile fontFile);
     }

    [ComImport]
    [Guid("739d886a-cef5-47dc-8769-1a8b41bebbb0")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontFile
    {
        HRESULT GetReferenceKey(out IntPtr fontFileReferenceKey, out int fontFileReferenceKeySize);
        HRESULT GetLoader(out IDWriteFontFileLoader fontFileLoader);
        HRESULT Analyze(out bool isSupportedFontType, out DWRITE_FONT_FILE_TYPE fontFileType, out DWRITE_FONT_FACE_TYPE fontFaceType, out int numberOfFaces);
    }

    [ComImport]
    [Guid("727cad4e-d6af-4c9e-8a08-d695b11caa49")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontFileLoader
    {
        [PreserveSig]
        HRESULT CreateStreamFromKey(IntPtr fontFileReferenceKey, uint fontFileReferenceKeySize, out IDWriteFontFileStream fontFileStream);
    }

    [ComImport]
    [Guid("b2d9f3ec-c9fe-4a11-a2ec-d86208f7c0a2")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteLocalFontFileLoader : IDWriteFontFileLoader
    {
        #region IDWriteFontFileLoader
        [PreserveSig]
        new HRESULT CreateStreamFromKey(IntPtr fontFileReferenceKey, uint fontFileReferenceKeySize, out IDWriteFontFileStream fontFileStream);
        #endregion

        HRESULT GetFilePathLengthFromKey(IntPtr fontFileReferenceKey, uint fontFileReferenceKeySize, out uint filePathLength);
        HRESULT GetFilePathFromKey(IntPtr fontFileReferenceKey, uint fontFileReferenceKeySize, System.Text.StringBuilder filePath, uint filePathSize);
        HRESULT GetLastWriteTimeFromKey(IntPtr fontFileReferenceKey, uint fontFileReferenceKeySize, out /*FILETIME**/ IntPtr lastWriteTime) ;
    }

    [ComImport]
    [Guid("68648C83-6EDE-46C0-AB46-20083A887FDE")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteRemoteFontFileLoader : IDWriteFontFileLoader
    {
        #region IDWriteFontFileLoader
        [PreserveSig]
        new HRESULT CreateStreamFromKey(IntPtr fontFileReferenceKey, uint fontFileReferenceKeySize, out IDWriteFontFileStream fontFileStream);
        #endregion

        [PreserveSig]
        HRESULT CreateRemoteStreamFromKey(IntPtr fontFileReferenceKey, uint fontFileReferenceKeySize, out IDWriteRemoteFontFileStream fontFileStream);
        HRESULT GetLocalityFromKey(IntPtr fontFileReferenceKey, uint fontFileReferenceKeySize, out DWRITE_LOCALITY locality);
        [PreserveSig]
        HRESULT CreateFontFileReferenceFromUrl(IDWriteFactory factory, [MarshalAs(UnmanagedType.LPWStr)] string baseUrl, [MarshalAs(UnmanagedType.LPWStr)] string fontFileUrl, out IDWriteFontFile fontFile);
    }

    [ComImport]
    [Guid("DC102F47-A12D-4B1C-822D-9E117E33043F")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteInMemoryFontFileLoader : IDWriteFontFileLoader
    {
        #region IDWriteFontFileLoader
        [PreserveSig]
        new HRESULT CreateStreamFromKey(IntPtr fontFileReferenceKey, uint fontFileReferenceKeySize, out IDWriteFontFileStream fontFileStream);
        #endregion

        [PreserveSig]
        HRESULT CreateInMemoryFontFileReference(IDWriteFactory factory, IntPtr fontData, uint fontDataSize, IntPtr ownerObject, out IDWriteFontFile fontFile);
        [PreserveSig]
        uint GetFileCount();
    }

    [ComImport]
    [Guid("6d4865fe-0ab8-4d91-8f62-5dd6be34a3e0")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontFileStream
    {
        HRESULT ReadFileFragment(out IntPtr fragmentStart, UInt64 fileOffset, UInt64 fragmentSize, out IntPtr fragmentContext);
        void ReleaseFileFragment(IntPtr fragmentContext);
        HRESULT GetFileSize(out UInt64 fileSize);
        HRESULT GetLastWriteTime(out UInt64 lastWriteTime);
    }

    [ComImport]
    [Guid("4DB3757A-2C72-4ED9-B2B6-1ABABE1AFF9C")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteRemoteFontFileStream : IDWriteFontFileStream
    {
        #region IDWriteFontFileStream
        new HRESULT ReadFileFragment(out IntPtr fragmentStart, UInt64 fileOffset, UInt64 fragmentSize, out IntPtr fragmentContext);
        new void ReleaseFileFragment(IntPtr fragmentContext);
        new HRESULT GetFileSize(out UInt64 fileSize);
        new HRESULT GetLastWriteTime(out UInt64 lastWriteTime);
        #endregion

        HRESULT GetLocalFileSize(out UInt64 localFileSize);
        HRESULT GetFileFragmentLocality(UInt64 fileOffset, UInt64 fragmentSize, out bool isLocal, out UInt64 partialSize);
        DWRITE_LOCALITY GetLocality();
        HRESULT BeginDownload(ref Guid downloadOperationID, DWRITE_FILE_FRAGMENT fileFragments, uint fragmentCount, out IDWriteAsyncResult asyncResult);
    }

    [ComImport]
    [Guid("2f0da53a-2add-47cd-82ee-d9ec34688e75")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteRenderingParams
    {
        [PreserveSig]
        float GetGamma();
        [PreserveSig]
        float GetEnhancedContrast();
        [PreserveSig]
        float GetClearTypeLevel();
        [PreserveSig]
        DWRITE_PIXEL_GEOMETRY GetPixelGeometry();
        [PreserveSig]
        DWRITE_RENDERING_MODE GetRenderingMode();
    }

    [ComImport]
    [Guid("94413cf4-a6fc-4248-8b50-6674348fcad3")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteRenderingParams1 : IDWriteRenderingParams
    {
        #region IDWriteRenderingParams

        [PreserveSig]
        new float GetGamma();
        [PreserveSig]
        new float GetEnhancedContrast();
        [PreserveSig]
        new float GetClearTypeLevel();
        [PreserveSig]
        new DWRITE_PIXEL_GEOMETRY GetPixelGeometry();
        [PreserveSig]
        new DWRITE_RENDERING_MODE GetRenderingMode();
        #endregion

        [PreserveSig]
        float GetGrayscaleEnhancedContrast();
    }

    [ComImport]
    [Guid("F9D711C3-9777-40AE-87E8-3E5AF9BF0948")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteRenderingParams2 : IDWriteRenderingParams1
    {
        #region IDWriteRenderingParams1
        #region IDWriteRenderingParams

        [PreserveSig]
        new float GetGamma();
        [PreserveSig]
        new float GetEnhancedContrast();
        [PreserveSig]
        new float GetClearTypeLevel();
        [PreserveSig]
        new DWRITE_PIXEL_GEOMETRY GetPixelGeometry();
        [PreserveSig]
        new DWRITE_RENDERING_MODE GetRenderingMode();
        #endregion

        [PreserveSig]
        new float GetGrayscaleEnhancedContrast();
        #endregion

        [PreserveSig]
        DWRITE_GRID_FIT_MODE GetGridFitMode();
    }

    [ComImport]
    [Guid("B7924BAA-391B-412A-8C5C-E44CC2D867DC")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteRenderingParams3 : IDWriteRenderingParams2
    {
        #region IDWriteRenderingParams2
        #region IDWriteRenderingParams1
        #region IDWriteRenderingParams

        [PreserveSig]
        new float GetGamma();
        [PreserveSig]
        new float GetEnhancedContrast();
        [PreserveSig]
        new float GetClearTypeLevel();
        [PreserveSig]
        new DWRITE_PIXEL_GEOMETRY GetPixelGeometry();
        [PreserveSig]
        new DWRITE_RENDERING_MODE GetRenderingMode();
        #endregion

        [PreserveSig]
        new float GetGrayscaleEnhancedContrast();
        #endregion

        [PreserveSig]
        new DWRITE_GRID_FIT_MODE GetGridFitMode();
        #endregion

        [PreserveSig]
        DWRITE_RENDERING_MODE1 GetRenderingMode1();
    }

    [ComImport]
    [Guid("9c906818-31d7-4fd3-a151-7c5e225db55a")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteTextFormat
    {
        HRESULT SetTextAlignment(DWRITE_TEXT_ALIGNMENT textAlignment);
        HRESULT SetParagraphAlignment(DWRITE_PARAGRAPH_ALIGNMENT paragraphAlignment);
        HRESULT SetWordWrapping(DWRITE_WORD_WRAPPING wordWrapping);
        HRESULT SetReadingDirection(DWRITE_READING_DIRECTION readingDirection);
        HRESULT SetFlowDirection(DWRITE_FLOW_DIRECTION flowDirection);
        HRESULT SetIncrementalTabStop(float incrementalTabStop);
        HRESULT SetTrimming(DWRITE_TRIMMING trimmingOptions, IDWriteInlineObject trimmingSign);
        HRESULT SetLineSpacing(DWRITE_LINE_SPACING_METHOD lineSpacingMethod, float lineSpacing, float baseline);
        [PreserveSig]
        DWRITE_TEXT_ALIGNMENT GetTextAlignment();
        [PreserveSig]
        DWRITE_PARAGRAPH_ALIGNMENT GetParagraphAlignment();
        [PreserveSig]
        DWRITE_WORD_WRAPPING GetWordWrapping();
        [PreserveSig]
        DWRITE_READING_DIRECTION GetReadingDirection();
        [PreserveSig]
        DWRITE_FLOW_DIRECTION GetFlowDirection();
        [PreserveSig]
        float GetIncrementalTabStop();
        HRESULT GetTrimming(out DWRITE_TRIMMING trimmingOptions, out IDWriteInlineObject trimmingSign);
        HRESULT GetLineSpacing(out DWRITE_LINE_SPACING_METHOD lineSpacingMethod, out float lineSpacing, out float baseline);
        HRESULT GetFontCollection(out IDWriteFontCollection fontCollection);
        [PreserveSig]
        uint GetFontFamilyNameLength();
        HRESULT GetFontFamilyName(out string fontFamilyName, uint nameSize);
        [PreserveSig]
        DWRITE_FONT_WEIGHT GetFontWeight();
        [PreserveSig]
        DWRITE_FONT_STYLE GetFontStyle();
        [PreserveSig]
        DWRITE_FONT_STRETCH GetFontStretch();
        [PreserveSig]
        float GetFontSize();
        [PreserveSig]
        uint GetLocaleNameLength();
        HRESULT GetLocaleName(out string localeName, uint nameSize);
    }

    [ComImport]
    [Guid("5F174B49-0D8B-4CFB-8BCA-F1CCE9D06C67")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteTextFormat1 : IDWriteTextFormat
    {
        #region IDWriteTextFormat
        new HRESULT SetTextAlignment(DWRITE_TEXT_ALIGNMENT textAlignment);
        new HRESULT SetParagraphAlignment(DWRITE_PARAGRAPH_ALIGNMENT paragraphAlignment);
        new HRESULT SetWordWrapping(DWRITE_WORD_WRAPPING wordWrapping);
        new HRESULT SetReadingDirection(DWRITE_READING_DIRECTION readingDirection);
        new HRESULT SetFlowDirection(DWRITE_FLOW_DIRECTION flowDirection);
        new HRESULT SetIncrementalTabStop(float incrementalTabStop);
        new HRESULT SetTrimming(DWRITE_TRIMMING trimmingOptions, IDWriteInlineObject trimmingSign);
        new HRESULT SetLineSpacing(DWRITE_LINE_SPACING_METHOD lineSpacingMethod, float lineSpacing, float baseline);
        [PreserveSig]
        new DWRITE_TEXT_ALIGNMENT GetTextAlignment();
        [PreserveSig]
        new DWRITE_PARAGRAPH_ALIGNMENT GetParagraphAlignment();
        [PreserveSig]
        new DWRITE_WORD_WRAPPING GetWordWrapping();
        [PreserveSig]
        new DWRITE_READING_DIRECTION GetReadingDirection();
        [PreserveSig]
        new DWRITE_FLOW_DIRECTION GetFlowDirection();
        [PreserveSig]
        new float GetIncrementalTabStop();
        new HRESULT GetTrimming(out DWRITE_TRIMMING trimmingOptions, out IDWriteInlineObject trimmingSign);
        new HRESULT GetLineSpacing(out DWRITE_LINE_SPACING_METHOD lineSpacingMethod, out float lineSpacing, out float baseline);
        new HRESULT GetFontCollection(out IDWriteFontCollection fontCollection);
        [PreserveSig]
        new uint GetFontFamilyNameLength();
        new HRESULT GetFontFamilyName(out string fontFamilyName, uint nameSize);
        [PreserveSig]
        new DWRITE_FONT_WEIGHT GetFontWeight();
        [PreserveSig]
        new DWRITE_FONT_STYLE GetFontStyle();
        [PreserveSig]
        new DWRITE_FONT_STRETCH GetFontStretch();
        [PreserveSig]
        new float GetFontSize();
        [PreserveSig]
        new uint GetLocaleNameLength();
        new HRESULT GetLocaleName(out string localeName, uint nameSize);
        #endregion

        HRESULT SetVerticalGlyphOrientation(DWRITE_VERTICAL_GLYPH_ORIENTATION glyphOrientation);
        [PreserveSig]
        DWRITE_VERTICAL_GLYPH_ORIENTATION GetVerticalGlyphOrientation();
        HRESULT SetLastLineWrapping(bool isLastLineWrappingEnabled);
        [PreserveSig]
        bool GetLastLineWrapping();
        HRESULT SetOpticalAlignment(DWRITE_OPTICAL_ALIGNMENT opticalAlignment);
        [PreserveSig]
        DWRITE_OPTICAL_ALIGNMENT GetOpticalAlignment();
        HRESULT SetFontFallback(IDWriteFontFallback fontFallback);
        HRESULT GetFontFallback(out IDWriteFontFallback fontFallback);
    }

    [ComImport]
    [Guid("F67E0EDD-9E3D-4ECC-8C32-4183253DFE70")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteTextFormat2 : IDWriteTextFormat1
    {
        #region IDWriteTextForma1
        #region IDWriteTextFormat
        new HRESULT SetTextAlignment(DWRITE_TEXT_ALIGNMENT textAlignment);
        new HRESULT SetParagraphAlignment(DWRITE_PARAGRAPH_ALIGNMENT paragraphAlignment);
        new HRESULT SetWordWrapping(DWRITE_WORD_WRAPPING wordWrapping);
        new HRESULT SetReadingDirection(DWRITE_READING_DIRECTION readingDirection);
        new HRESULT SetFlowDirection(DWRITE_FLOW_DIRECTION flowDirection);
        new HRESULT SetIncrementalTabStop(float incrementalTabStop);
        new HRESULT SetTrimming(DWRITE_TRIMMING trimmingOptions, IDWriteInlineObject trimmingSign);
        new HRESULT SetLineSpacing(DWRITE_LINE_SPACING_METHOD lineSpacingMethod, float lineSpacing, float baseline);
        [PreserveSig]
        new DWRITE_TEXT_ALIGNMENT GetTextAlignment();
        [PreserveSig]
        new DWRITE_PARAGRAPH_ALIGNMENT GetParagraphAlignment();
        [PreserveSig]
        new DWRITE_WORD_WRAPPING GetWordWrapping();
        [PreserveSig]
        new DWRITE_READING_DIRECTION GetReadingDirection();
        [PreserveSig]
        new DWRITE_FLOW_DIRECTION GetFlowDirection();
        [PreserveSig]
        new float GetIncrementalTabStop();
        new HRESULT GetTrimming(out DWRITE_TRIMMING trimmingOptions, out IDWriteInlineObject trimmingSign);
        new HRESULT GetLineSpacing(out DWRITE_LINE_SPACING_METHOD lineSpacingMethod, out float lineSpacing, out float baseline);
        new HRESULT GetFontCollection(out IDWriteFontCollection fontCollection);
        [PreserveSig]
        new uint GetFontFamilyNameLength();
        new HRESULT GetFontFamilyName(out string fontFamilyName, uint nameSize);
        [PreserveSig]
        new DWRITE_FONT_WEIGHT GetFontWeight();
        [PreserveSig]
        new DWRITE_FONT_STYLE GetFontStyle();
        [PreserveSig]
        new DWRITE_FONT_STRETCH GetFontStretch();
        [PreserveSig]
        new float GetFontSize();
        [PreserveSig]
        new uint GetLocaleNameLength();
        new HRESULT GetLocaleName(out string localeName, uint nameSize);
        #endregion

        new HRESULT SetVerticalGlyphOrientation(DWRITE_VERTICAL_GLYPH_ORIENTATION glyphOrientation);
        [PreserveSig]
        new DWRITE_VERTICAL_GLYPH_ORIENTATION GetVerticalGlyphOrientation();
        new HRESULT SetLastLineWrapping(bool isLastLineWrappingEnabled);
        [PreserveSig]
        new bool GetLastLineWrapping();
        new HRESULT SetOpticalAlignment(DWRITE_OPTICAL_ALIGNMENT opticalAlignment);
        [PreserveSig]
        new DWRITE_OPTICAL_ALIGNMENT GetOpticalAlignment();
        new HRESULT SetFontFallback(IDWriteFontFallback fontFallback);
        new HRESULT GetFontFallback(out IDWriteFontFallback fontFallback);
        #endregion

        HRESULT SetLineSpacing(DWRITE_LINE_SPACING lineSpacingOptions);
        HRESULT GetLineSpacing(out DWRITE_LINE_SPACING lineSpacingOptions);
    }

    [ComImport]
    [Guid("6D3B5641-E550-430D-A85B-B7BF48A93427")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteTextFormat3 : IDWriteTextFormat2
    {
        #region IDWriteTextFormat2
        #region IDWriteTextForma1
        #region IDWriteTextFormat
        new HRESULT SetTextAlignment(DWRITE_TEXT_ALIGNMENT textAlignment);
        new HRESULT SetParagraphAlignment(DWRITE_PARAGRAPH_ALIGNMENT paragraphAlignment);
        new HRESULT SetWordWrapping(DWRITE_WORD_WRAPPING wordWrapping);
        new HRESULT SetReadingDirection(DWRITE_READING_DIRECTION readingDirection);
        new HRESULT SetFlowDirection(DWRITE_FLOW_DIRECTION flowDirection);
        new HRESULT SetIncrementalTabStop(float incrementalTabStop);
        new HRESULT SetTrimming(DWRITE_TRIMMING trimmingOptions, IDWriteInlineObject trimmingSign);
        new HRESULT SetLineSpacing(DWRITE_LINE_SPACING_METHOD lineSpacingMethod, float lineSpacing, float baseline);
        [PreserveSig]
        new DWRITE_TEXT_ALIGNMENT GetTextAlignment();
        [PreserveSig]
        new DWRITE_PARAGRAPH_ALIGNMENT GetParagraphAlignment();
        [PreserveSig]
        new DWRITE_WORD_WRAPPING GetWordWrapping();
        [PreserveSig]
        new DWRITE_READING_DIRECTION GetReadingDirection();
        [PreserveSig]
        new DWRITE_FLOW_DIRECTION GetFlowDirection();
        [PreserveSig]
        new float GetIncrementalTabStop();
        new HRESULT GetTrimming(out DWRITE_TRIMMING trimmingOptions, out IDWriteInlineObject trimmingSign);
        new HRESULT GetLineSpacing(out DWRITE_LINE_SPACING_METHOD lineSpacingMethod, out float lineSpacing, out float baseline);
        new HRESULT GetFontCollection(out IDWriteFontCollection fontCollection);
        [PreserveSig]
        new uint GetFontFamilyNameLength();
        new HRESULT GetFontFamilyName(out string fontFamilyName, uint nameSize);
        [PreserveSig]
        new DWRITE_FONT_WEIGHT GetFontWeight();
        [PreserveSig]
        new DWRITE_FONT_STYLE GetFontStyle();
        [PreserveSig]
        new DWRITE_FONT_STRETCH GetFontStretch();
        [PreserveSig]
        new float GetFontSize();
        [PreserveSig]
        new uint GetLocaleNameLength();
        new HRESULT GetLocaleName(out string localeName, uint nameSize);
        #endregion

        new HRESULT SetVerticalGlyphOrientation(DWRITE_VERTICAL_GLYPH_ORIENTATION glyphOrientation);
        [PreserveSig]
        new DWRITE_VERTICAL_GLYPH_ORIENTATION GetVerticalGlyphOrientation();
        new HRESULT SetLastLineWrapping(bool isLastLineWrappingEnabled);
        [PreserveSig]
        new bool GetLastLineWrapping();
        new HRESULT SetOpticalAlignment(DWRITE_OPTICAL_ALIGNMENT opticalAlignment);
        [PreserveSig]
        new DWRITE_OPTICAL_ALIGNMENT GetOpticalAlignment();
        new HRESULT SetFontFallback(IDWriteFontFallback fontFallback);
        new HRESULT GetFontFallback(out IDWriteFontFallback fontFallback);
        #endregion

        new HRESULT SetLineSpacing(DWRITE_LINE_SPACING lineSpacingOptions);
        new HRESULT GetLineSpacing(out DWRITE_LINE_SPACING lineSpacingOptions);
        #endregion

        HRESULT SetFontAxisValues(DWRITE_FONT_AXIS_VALUE fontAxisValues, uint fontAxisValueCount);
        [PreserveSig]
        uint GetFontAxisValueCount();
        HRESULT GetFontAxisValues(out DWRITE_FONT_AXIS_VALUE fontAxisValues, uint fontAxisValueCount);
        DWRITE_AUTOMATIC_FONT_AXES GetAutomaticFontAxes();
        HRESULT SetAutomaticFontAxes(DWRITE_AUTOMATIC_FONT_AXES automaticFontAxes);
    }

    [ComImport]
    [Guid("EFA008F9-F7A1-48BF-B05C-F224713CC0FF")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontFallback
    {
        HRESULT MapCharacters(IDWriteTextAnalysisSource analysisSource, uint textPosition, uint textLength, IDWriteFontCollection baseFontCollection,
            string baseFamilyName, DWRITE_FONT_WEIGHT baseWeight, DWRITE_FONT_STYLE baseStyle, DWRITE_FONT_STRETCH baseStretch, out uint mappedLength,
            out IDWriteFont mappedFont, out float scale);
    }

    [ComImport]
    [Guid("FD882D06-8ABA-4FB8-B849-8BE8B73E14DE")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontFallbackBuilder
    {
        HRESULT AddMapping(DWRITE_UNICODE_RANGE ranges, uint rangesCount, string targetFamilyNames, uint targetFamilyNamesCount, IDWriteFontCollection fontCollection = null,
            string localeName = null, string baseFamilyName = null, float scale = 1.0f);
        HRESULT AddMappings(IDWriteFontFallback fontFallback);
        [PreserveSig]
        HRESULT CreateFontFallback(out IDWriteFontFallback fontFallback);
    }

    [ComImport]
    [Guid("55f1112b-1dc2-4b3c-9541-f46894ed85b6")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteTypography
    {
        HRESULT AddFontFeature(DWRITE_FONT_FEATURE fontFeature);
        [PreserveSig]
        uint GetFontFeatureCount();
        HRESULT GetFontFeature(uint fontFeatureIndex, out DWRITE_FONT_FEATURE fontFeature);
    }

    [ComImport]
    [Guid("1edd9491-9853-4299-898f-6432983b6f3a")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteGdiInterop
    {
        [PreserveSig]
        HRESULT CreateFontFromLOGFONT(LOGFONT logFont, out IDWriteFont font);
        HRESULT ConvertFontToLOGFONT(IDWriteFont font, out LOGFONT logFont, out bool isSystemFont);
        HRESULT ConvertFontFaceToLOGFONT(IDWriteFontFace font, out LOGFONT logFont);
        [PreserveSig]
        HRESULT CreateFontFaceFromHdc(IntPtr hdc, out IDWriteFontFace fontFace);
        [PreserveSig]
        HRESULT CreateBitmapRenderTarget(IntPtr hdc, int width, int height, out IDWriteBitmapRenderTarget renderTarget);
    }

    [ComImport]
    [Guid("53737037-6d14-410b-9bfe-0b182bb70961")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteTextLayout : IDWriteTextFormat
    {
        #region IDWriteTextFormat
        new HRESULT SetTextAlignment(DWRITE_TEXT_ALIGNMENT textAlignment);
        new HRESULT SetParagraphAlignment(DWRITE_PARAGRAPH_ALIGNMENT paragraphAlignment);
        new HRESULT SetWordWrapping(DWRITE_WORD_WRAPPING wordWrapping);
        new HRESULT SetReadingDirection(DWRITE_READING_DIRECTION readingDirection);
        new HRESULT SetFlowDirection(DWRITE_FLOW_DIRECTION flowDirection);
        new HRESULT SetIncrementalTabStop(float incrementalTabStop);
        new HRESULT SetTrimming(DWRITE_TRIMMING trimmingOptions, IDWriteInlineObject trimmingSign);
        new HRESULT SetLineSpacing(DWRITE_LINE_SPACING_METHOD lineSpacingMethod, float lineSpacing, float baseline);
        [PreserveSig]
        new DWRITE_TEXT_ALIGNMENT GetTextAlignment();
        [PreserveSig]
        new DWRITE_PARAGRAPH_ALIGNMENT GetParagraphAlignment();
        [PreserveSig]
        new DWRITE_WORD_WRAPPING GetWordWrapping();
        [PreserveSig]
        new DWRITE_READING_DIRECTION GetReadingDirection();
        [PreserveSig]
        new DWRITE_FLOW_DIRECTION GetFlowDirection();
        [PreserveSig]
        new float GetIncrementalTabStop();
        new HRESULT GetTrimming(out DWRITE_TRIMMING trimmingOptions, out IDWriteInlineObject trimmingSign);
        new HRESULT GetLineSpacing(out DWRITE_LINE_SPACING_METHOD lineSpacingMethod, out float lineSpacing, out float baseline);
        new HRESULT GetFontCollection(out IDWriteFontCollection fontCollection);
        [PreserveSig]
        new uint GetFontFamilyNameLength();
        new HRESULT GetFontFamilyName(out string fontFamilyName, uint nameSize);
        [PreserveSig]
        new DWRITE_FONT_WEIGHT GetFontWeight();
        [PreserveSig]
        new DWRITE_FONT_STYLE GetFontStyle();
        [PreserveSig]
        new DWRITE_FONT_STRETCH GetFontStretch();
        [PreserveSig]
        new float GetFontSize();
        [PreserveSig]
        new uint GetLocaleNameLength();
        new HRESULT GetLocaleName(out string localeName, uint nameSize);
        #endregion

        HRESULT SetMaxWidth(float maxWidth);
        HRESULT SetMaxHeight(float maxHeight);
        HRESULT SetFontCollection(IDWriteFontCollection fontCollection, DWRITE_TEXT_RANGE textRange);
        HRESULT SetFontFamilyName(string fontFamilyName, DWRITE_TEXT_RANGE textRange);
        HRESULT SetFontWeight(DWRITE_FONT_WEIGHT fontWeight, DWRITE_TEXT_RANGE textRange);
        HRESULT SetFontStyle(DWRITE_FONT_STYLE fontStyle, DWRITE_TEXT_RANGE textRange);
        HRESULT SetFontStretch(DWRITE_FONT_STRETCH fontStretch, DWRITE_TEXT_RANGE textRange);
        HRESULT SetFontSize(float fontSize, DWRITE_TEXT_RANGE textRange);
        HRESULT SetUnderline(bool hasUnderline, DWRITE_TEXT_RANGE textRange);
        HRESULT SetStrikethrough(bool hasStrikethrough, DWRITE_TEXT_RANGE textRange);
        //HRESULT SetDrawingEffect(IUnknown drawingEffect, DWRITE_TEXT_RANGE textRange);
        HRESULT SetDrawingEffect(IntPtr drawingEffect, DWRITE_TEXT_RANGE textRange);
        HRESULT SetInlineObject(IDWriteInlineObject inlineObject, DWRITE_TEXT_RANGE textRange);
        HRESULT SetTypography(IDWriteTypography typography, DWRITE_TEXT_RANGE textRange);
        HRESULT SetLocaleName(string localeName, DWRITE_TEXT_RANGE textRange);
        [PreserveSig]
        float GetMaxWidth();
        [PreserveSig]
        float GetMaxHeight();
        HRESULT GetFontCollection(uint currentPosition, out IDWriteFontCollection fontCollection, out DWRITE_TEXT_RANGE textRange);
        HRESULT GetFontFamilyNameLength(uint currentPosition, out uint nameLength, out DWRITE_TEXT_RANGE textRange);
        HRESULT GetFontFamilyName(uint currentPosition, out string fontFamilyName, uint nameSize, out DWRITE_TEXT_RANGE textRange);
        HRESULT GetFontWeight(uint currentPosition, out DWRITE_FONT_WEIGHT fontWeight, out DWRITE_TEXT_RANGE textRange);
        HRESULT GetFontStyle(uint currentPosition, out DWRITE_FONT_STYLE fontStyle, out DWRITE_TEXT_RANGE textRange);
        HRESULT GetFontStretch(uint currentPosition, out DWRITE_FONT_STRETCH fontStretch, out DWRITE_TEXT_RANGE textRange);
        HRESULT GetFontSize(uint currentPosition, out float fontSize, out DWRITE_TEXT_RANGE textRange);
        HRESULT GetUnderline(uint currentPosition, out bool hasUnderline, out DWRITE_TEXT_RANGE textRange);
        HRESULT GetStrikethrough(uint currentPosition, out bool hasStrikethrough, out DWRITE_TEXT_RANGE textRange);
        //HRESULT GetDrawingEffect(uint currentPosition, out IUnknown drawingEffect, out DWRITE_TEXT_RANGE textRange);
        HRESULT GetDrawingEffect(uint currentPosition, out IntPtr drawingEffect, out DWRITE_TEXT_RANGE textRange);
        HRESULT GetInlineObject(uint currentPosition, out IDWriteInlineObject inlineObject, out DWRITE_TEXT_RANGE textRange);
        HRESULT GetTypography(uint currentPosition, out IDWriteTypography typography, out DWRITE_TEXT_RANGE textRange);
        HRESULT GetLocaleNameLength(uint currentPosition, out uint nameLength, out DWRITE_TEXT_RANGE textRange);
        HRESULT GetLocaleName(uint currentPosition, out string localeName, uint nameSize, out DWRITE_TEXT_RANGE textRange);
        [PreserveSig]
        HRESULT Draw(IntPtr clientDrawingContext, IDWriteTextRenderer renderer, float originX, float originY);
        HRESULT GetLineMetrics(out DWRITE_LINE_METRICS lineMetrics, uint maxLineCount, out uint actualLineCount);
        HRESULT GetMetrics(out DWRITE_TEXT_METRICS textMetrics);
        HRESULT GetOverhangMetrics(out DWRITE_OVERHANG_METRICS overhangs);
        HRESULT GetClusterMetrics(out DWRITE_CLUSTER_METRICS clusterMetrics, uint maxClusterCount, out uint actualClusterCount);
        HRESULT DetermineMinWidth(out float minWidth);
        HRESULT HitTestPoint(float pointX, float pointY, out bool isTrailingHit, out bool isInside, out DWRITE_HIT_TEST_METRICS hitTestMetrics);
        HRESULT HitTestTextPosition(uint textPosition, bool isTrailingHit, out float pointX, out float pointY, out DWRITE_HIT_TEST_METRICS hitTestMetrics);
        HRESULT HitTestTextRange(uint textPosition, uint textLength, float originX, float originY, out DWRITE_HIT_TEST_METRICS hitTestMetrics, uint maxHitTestMetricsCount, out uint actualHitTestMetricsCount);
    }

    [ComImport()]
    [Guid("9064D822-80A7-465C-A986-DF65F78B8FEB")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteTextLayout1 : IDWriteTextLayout
    {
        #region IDWriteTextLayout
        new HRESULT SetTextAlignment(DWRITE_TEXT_ALIGNMENT textAlignment);
        new HRESULT SetParagraphAlignment(DWRITE_PARAGRAPH_ALIGNMENT paragraphAlignment);
        new HRESULT SetWordWrapping(DWRITE_WORD_WRAPPING wordWrapping);
        new HRESULT SetReadingDirection(DWRITE_READING_DIRECTION readingDirection);
        new HRESULT SetFlowDirection(DWRITE_FLOW_DIRECTION flowDirection);
        new HRESULT SetIncrementalTabStop(float incrementalTabStop);
        new HRESULT SetTrimming(DWRITE_TRIMMING trimmingOptions, IDWriteInlineObject trimmingSign);
        new HRESULT SetLineSpacing(DWRITE_LINE_SPACING_METHOD lineSpacingMethod, float lineSpacing, float baseline);
        [PreserveSig]
        new DWRITE_TEXT_ALIGNMENT GetTextAlignment();
        [PreserveSig]
        new DWRITE_PARAGRAPH_ALIGNMENT GetParagraphAlignment();
        [PreserveSig]
        new DWRITE_WORD_WRAPPING GetWordWrapping();
        [PreserveSig]
        new DWRITE_READING_DIRECTION GetReadingDirection();
        [PreserveSig]
        new DWRITE_FLOW_DIRECTION GetFlowDirection();
        [PreserveSig]
        new float GetIncrementalTabStop();
        new HRESULT GetTrimming(out DWRITE_TRIMMING trimmingOptions, out IDWriteInlineObject trimmingSign);
        new HRESULT GetLineSpacing(out DWRITE_LINE_SPACING_METHOD lineSpacingMethod, out float lineSpacing, out float baseline);
        new HRESULT GetFontCollection(out IDWriteFontCollection fontCollection);
        [PreserveSig]
        new uint GetFontFamilyNameLength();
        new HRESULT GetFontFamilyName(out string fontFamilyName, uint nameSize);
        [PreserveSig]
        new DWRITE_FONT_WEIGHT GetFontWeight();
        [PreserveSig]
        new DWRITE_FONT_STYLE GetFontStyle();
        [PreserveSig]
        new DWRITE_FONT_STRETCH GetFontStretch();
        [PreserveSig]
        new float GetFontSize();
        [PreserveSig]
        new uint GetLocaleNameLength();
        new HRESULT GetLocaleName(out string localeName, uint nameSize);
        new HRESULT SetMaxWidth(float maxWidth);
        new HRESULT SetMaxHeight(float maxHeight);
        new HRESULT SetFontCollection(IDWriteFontCollection fontCollection, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetFontFamilyName(string fontFamilyName, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetFontWeight(DWRITE_FONT_WEIGHT fontWeight, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetFontStyle(DWRITE_FONT_STYLE fontStyle, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetFontStretch(DWRITE_FONT_STRETCH fontStretch, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetFontSize(float fontSize, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetUnderline(bool hasUnderline, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetStrikethrough(bool hasStrikethrough, DWRITE_TEXT_RANGE textRange);

        new HRESULT SetDrawingEffect(IntPtr drawingEffect, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetInlineObject(IDWriteInlineObject inlineObject, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetTypography(IDWriteTypography typography, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetLocaleName(string localeName, DWRITE_TEXT_RANGE textRange);
        [PreserveSig]
        new float GetMaxWidth();
        [PreserveSig]
        new float GetMaxHeight();
        new HRESULT GetFontCollection(uint currentPosition, out IDWriteFontCollection fontCollection, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetFontFamilyNameLength(uint currentPosition, out uint nameLength, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetFontFamilyName(uint currentPosition, out string fontFamilyName, uint nameSize, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetFontWeight(uint currentPosition, out DWRITE_FONT_WEIGHT fontWeight, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetFontStyle(uint currentPosition, out DWRITE_FONT_STYLE fontStyle, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetFontStretch(uint currentPosition, out DWRITE_FONT_STRETCH fontStretch, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetFontSize(uint currentPosition, out float fontSize, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetUnderline(uint currentPosition, out bool hasUnderline, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetStrikethrough(uint currentPosition, out bool hasStrikethrough, out DWRITE_TEXT_RANGE textRange);

        new HRESULT GetDrawingEffect(uint currentPosition, out IntPtr drawingEffect, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetInlineObject(uint currentPosition, out IDWriteInlineObject inlineObject, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetTypography(uint currentPosition, out IDWriteTypography typography, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetLocaleNameLength(uint currentPosition, out uint nameLength, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetLocaleName(uint currentPosition, out string localeName, uint nameSize, out DWRITE_TEXT_RANGE textRange);
        new HRESULT Draw(IntPtr clientDrawingContext, IDWriteTextRenderer renderer, float originX, float originY);
        new HRESULT GetLineMetrics(out DWRITE_LINE_METRICS lineMetrics, uint maxLineCount, out uint actualLineCount);
        new HRESULT GetMetrics(out DWRITE_TEXT_METRICS textMetrics);
        new HRESULT GetOverhangMetrics(out DWRITE_OVERHANG_METRICS overhangs);
        new HRESULT GetClusterMetrics(out DWRITE_CLUSTER_METRICS clusterMetrics, uint maxClusterCount, out uint actualClusterCount);
        new HRESULT DetermineMinWidth(out float minWidth);
        new HRESULT HitTestPoint(float pointX, float pointY, out bool isTrailingHit, out bool isInside, out DWRITE_HIT_TEST_METRICS hitTestMetrics);
        new HRESULT HitTestTextPosition(uint textPosition, bool isTrailingHit, out float pointX, out float pointY, out DWRITE_HIT_TEST_METRICS hitTestMetrics);
        new HRESULT HitTestTextRange(uint textPosition, uint textLength, float originX, float originY, out DWRITE_HIT_TEST_METRICS hitTestMetrics, uint maxHitTestMetricsCount, out uint actualHitTestMetricsCount);
        #endregion

        HRESULT SetPairKerning(bool isPairKerningEnabled, DWRITE_TEXT_RANGE textRange);
        HRESULT GetPairKerning(uint currentPosition, ref bool isPairKerningEnabled, ref DWRITE_TEXT_RANGE textRange);
        HRESULT SetCharacterSpacing(float leadingSpacing, float trailingSpacing, float minimumAdvanceWidth, DWRITE_TEXT_RANGE textRange);
        HRESULT GetCharacterSpacing(uint currentPosition, ref float leadingSpacing, out float trailingSpacing, out float minimumAdvanceWidth, out DWRITE_TEXT_RANGE textRange);
    }

    [ComImport()]
    [Guid("1093C18F-8D5E-43F0-B064-0917311B525E")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteTextLayout2 : IDWriteTextLayout1
    {
        #region IDWriteTextLayou1
        #region IDWriteTextLayout
        new HRESULT SetTextAlignment(DWRITE_TEXT_ALIGNMENT textAlignment);
        new HRESULT SetParagraphAlignment(DWRITE_PARAGRAPH_ALIGNMENT paragraphAlignment);
        new HRESULT SetWordWrapping(DWRITE_WORD_WRAPPING wordWrapping);
        new HRESULT SetReadingDirection(DWRITE_READING_DIRECTION readingDirection);
        new HRESULT SetFlowDirection(DWRITE_FLOW_DIRECTION flowDirection);
        new HRESULT SetIncrementalTabStop(float incrementalTabStop);
        new HRESULT SetTrimming(DWRITE_TRIMMING trimmingOptions, IDWriteInlineObject trimmingSign);
        new HRESULT SetLineSpacing(DWRITE_LINE_SPACING_METHOD lineSpacingMethod, float lineSpacing, float baseline);
        [PreserveSig]
        new DWRITE_TEXT_ALIGNMENT GetTextAlignment();
        [PreserveSig]
        new DWRITE_PARAGRAPH_ALIGNMENT GetParagraphAlignment();
        [PreserveSig]
        new DWRITE_WORD_WRAPPING GetWordWrapping();
        [PreserveSig]
        new DWRITE_READING_DIRECTION GetReadingDirection();
        [PreserveSig]
        new DWRITE_FLOW_DIRECTION GetFlowDirection();
        [PreserveSig]
        new float GetIncrementalTabStop();
        new HRESULT GetTrimming(out DWRITE_TRIMMING trimmingOptions, out IDWriteInlineObject trimmingSign);
        new HRESULT GetLineSpacing(out DWRITE_LINE_SPACING_METHOD lineSpacingMethod, out float lineSpacing, out float baseline);
        new HRESULT GetFontCollection(out IDWriteFontCollection fontCollection);
        [PreserveSig]
        new uint GetFontFamilyNameLength();
        new HRESULT GetFontFamilyName(out string fontFamilyName, uint nameSize);
        [PreserveSig]
        new DWRITE_FONT_WEIGHT GetFontWeight();
        [PreserveSig]
        new DWRITE_FONT_STYLE GetFontStyle();
        [PreserveSig]
        new DWRITE_FONT_STRETCH GetFontStretch();
        [PreserveSig]
        new float GetFontSize();
        [PreserveSig]
        new uint GetLocaleNameLength();
        new HRESULT GetLocaleName(out string localeName, uint nameSize);
        new HRESULT SetMaxWidth(float maxWidth);
        new HRESULT SetMaxHeight(float maxHeight);
        new HRESULT SetFontCollection(IDWriteFontCollection fontCollection, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetFontFamilyName(string fontFamilyName, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetFontWeight(DWRITE_FONT_WEIGHT fontWeight, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetFontStyle(DWRITE_FONT_STYLE fontStyle, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetFontStretch(DWRITE_FONT_STRETCH fontStretch, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetFontSize(float fontSize, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetUnderline(bool hasUnderline, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetStrikethrough(bool hasStrikethrough, DWRITE_TEXT_RANGE textRange);

        new HRESULT SetDrawingEffect(IntPtr drawingEffect, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetInlineObject(IDWriteInlineObject inlineObject, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetTypography(IDWriteTypography typography, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetLocaleName(string localeName, DWRITE_TEXT_RANGE textRange);
        [PreserveSig]
        new float GetMaxWidth();
        [PreserveSig]
        new float GetMaxHeight();
        new HRESULT GetFontCollection(uint currentPosition, out IDWriteFontCollection fontCollection, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetFontFamilyNameLength(uint currentPosition, out uint nameLength, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetFontFamilyName(uint currentPosition, out string fontFamilyName, uint nameSize, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetFontWeight(uint currentPosition, out DWRITE_FONT_WEIGHT fontWeight, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetFontStyle(uint currentPosition, out DWRITE_FONT_STYLE fontStyle, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetFontStretch(uint currentPosition, out DWRITE_FONT_STRETCH fontStretch, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetFontSize(uint currentPosition, out float fontSize, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetUnderline(uint currentPosition, out bool hasUnderline, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetStrikethrough(uint currentPosition, out bool hasStrikethrough, out DWRITE_TEXT_RANGE textRange);

        new HRESULT GetDrawingEffect(uint currentPosition, out IntPtr drawingEffect, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetInlineObject(uint currentPosition, out IDWriteInlineObject inlineObject, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetTypography(uint currentPosition, out IDWriteTypography typography, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetLocaleNameLength(uint currentPosition, out uint nameLength, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetLocaleName(uint currentPosition, out string localeName, uint nameSize, out DWRITE_TEXT_RANGE textRange);
        new HRESULT Draw(IntPtr clientDrawingContext, IDWriteTextRenderer renderer, float originX, float originY);
        new HRESULT GetLineMetrics(out DWRITE_LINE_METRICS lineMetrics, uint maxLineCount, out uint actualLineCount);
        new HRESULT GetMetrics(out DWRITE_TEXT_METRICS textMetrics);
        new HRESULT GetOverhangMetrics(out DWRITE_OVERHANG_METRICS overhangs);
        new HRESULT GetClusterMetrics(out DWRITE_CLUSTER_METRICS clusterMetrics, uint maxClusterCount, out uint actualClusterCount);
        new HRESULT DetermineMinWidth(out float minWidth);
        new HRESULT HitTestPoint(float pointX, float pointY, out bool isTrailingHit, out bool isInside, out DWRITE_HIT_TEST_METRICS hitTestMetrics);
        new HRESULT HitTestTextPosition(uint textPosition, bool isTrailingHit, out float pointX, out float pointY, out DWRITE_HIT_TEST_METRICS hitTestMetrics);
        new HRESULT HitTestTextRange(uint textPosition, uint textLength, float originX, float originY, out DWRITE_HIT_TEST_METRICS hitTestMetrics, uint maxHitTestMetricsCount, out uint actualHitTestMetricsCount);
        #endregion

        new HRESULT SetPairKerning(bool isPairKerningEnabled, DWRITE_TEXT_RANGE textRange);
        new HRESULT GetPairKerning(uint currentPosition, ref bool isPairKerningEnabled, ref DWRITE_TEXT_RANGE textRange);
        new HRESULT SetCharacterSpacing(float leadingSpacing, float trailingSpacing, float minimumAdvanceWidth, DWRITE_TEXT_RANGE textRange);
        new HRESULT GetCharacterSpacing(uint currentPosition, ref float leadingSpacing, out float trailingSpacing, out float minimumAdvanceWidth, out DWRITE_TEXT_RANGE textRange);
        #endregion

        HRESULT GetMetrics(out DWRITE_TEXT_METRICS1 textMetrics);
        HRESULT SetVerticalGlyphOrientation(DWRITE_VERTICAL_GLYPH_ORIENTATION glyphOrientation);
        [PreserveSig]
        DWRITE_VERTICAL_GLYPH_ORIENTATION GetVerticalGlyphOrientation();
        HRESULT SetLastLineWrapping(bool isLastLineWrappingEnabled);
        [PreserveSig]
        bool GetLastLineWrapping();
        HRESULT SetOpticalAlignment(DWRITE_OPTICAL_ALIGNMENT opticalAlignment);
        [PreserveSig]
        DWRITE_OPTICAL_ALIGNMENT GetOpticalAlignment();
        HRESULT SetFontFallback(IDWriteFontFallback fontFallback);
        HRESULT GetFontFallback(out IDWriteFontFallback fontFallback);
    }

    [ComImport()]
    [Guid("07DDCD52-020E-4DE8-AC33-6C953D83F92D")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteTextLayout3 : IDWriteTextLayout2
    {
        #region IDWriteTextLayout1
        #region IDWriteTextLayout        
        new HRESULT SetTextAlignment(DWRITE_TEXT_ALIGNMENT textAlignment);
        new HRESULT SetParagraphAlignment(DWRITE_PARAGRAPH_ALIGNMENT paragraphAlignment);
        new HRESULT SetWordWrapping(DWRITE_WORD_WRAPPING wordWrapping);
        new HRESULT SetReadingDirection(DWRITE_READING_DIRECTION readingDirection);
        new HRESULT SetFlowDirection(DWRITE_FLOW_DIRECTION flowDirection);
        new HRESULT SetIncrementalTabStop(float incrementalTabStop);
        new HRESULT SetTrimming(DWRITE_TRIMMING trimmingOptions, IDWriteInlineObject trimmingSign);
        new HRESULT SetLineSpacing(DWRITE_LINE_SPACING_METHOD lineSpacingMethod, float lineSpacing, float baseline);
        [PreserveSig]
        new DWRITE_TEXT_ALIGNMENT GetTextAlignment();
        [PreserveSig]
        new DWRITE_PARAGRAPH_ALIGNMENT GetParagraphAlignment();
        [PreserveSig]
        new DWRITE_WORD_WRAPPING GetWordWrapping();
        [PreserveSig]
        new DWRITE_READING_DIRECTION GetReadingDirection();
        [PreserveSig]
        new DWRITE_FLOW_DIRECTION GetFlowDirection();
        [PreserveSig]
        new float GetIncrementalTabStop();
        new HRESULT GetTrimming(out DWRITE_TRIMMING trimmingOptions, out IDWriteInlineObject trimmingSign);
        new HRESULT GetLineSpacing(out DWRITE_LINE_SPACING_METHOD lineSpacingMethod, out float lineSpacing, out float baseline);
        new HRESULT GetFontCollection(out IDWriteFontCollection fontCollection);
        [PreserveSig]
        new uint GetFontFamilyNameLength();
        new HRESULT GetFontFamilyName(out string fontFamilyName, uint nameSize);
        [PreserveSig]
        new DWRITE_FONT_WEIGHT GetFontWeight();
        [PreserveSig]
        new DWRITE_FONT_STYLE GetFontStyle();
        [PreserveSig]
        new DWRITE_FONT_STRETCH GetFontStretch();
        [PreserveSig]
        new float GetFontSize();
        [PreserveSig]
        new uint GetLocaleNameLength();
        new HRESULT GetLocaleName(out string localeName, uint nameSize);
        new HRESULT SetMaxWidth(float maxWidth);
        new HRESULT SetMaxHeight(float maxHeight);
        new HRESULT SetFontCollection(IDWriteFontCollection fontCollection, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetFontFamilyName(string fontFamilyName, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetFontWeight(DWRITE_FONT_WEIGHT fontWeight, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetFontStyle(DWRITE_FONT_STYLE fontStyle, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetFontStretch(DWRITE_FONT_STRETCH fontStretch, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetFontSize(float fontSize, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetUnderline(bool hasUnderline, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetStrikethrough(bool hasStrikethrough, DWRITE_TEXT_RANGE textRange);

        new HRESULT SetDrawingEffect(IntPtr drawingEffect, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetInlineObject(IDWriteInlineObject inlineObject, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetTypography(IDWriteTypography typography, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetLocaleName(string localeName, DWRITE_TEXT_RANGE textRange);
        [PreserveSig]
        new float GetMaxWidth();
        [PreserveSig]
        new float GetMaxHeight();
        new HRESULT GetFontCollection(uint currentPosition, out IDWriteFontCollection fontCollection, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetFontFamilyNameLength(uint currentPosition, out uint nameLength, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetFontFamilyName(uint currentPosition, out string fontFamilyName, uint nameSize, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetFontWeight(uint currentPosition, out DWRITE_FONT_WEIGHT fontWeight, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetFontStyle(uint currentPosition, out DWRITE_FONT_STYLE fontStyle, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetFontStretch(uint currentPosition, out DWRITE_FONT_STRETCH fontStretch, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetFontSize(uint currentPosition, out float fontSize, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetUnderline(uint currentPosition, out bool hasUnderline, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetStrikethrough(uint currentPosition, out bool hasStrikethrough, out DWRITE_TEXT_RANGE textRange);

        new HRESULT GetDrawingEffect(uint currentPosition, out IntPtr drawingEffect, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetInlineObject(uint currentPosition, out IDWriteInlineObject inlineObject, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetTypography(uint currentPosition, out IDWriteTypography typography, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetLocaleNameLength(uint currentPosition, out uint nameLength, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetLocaleName(uint currentPosition, out string localeName, uint nameSize, out DWRITE_TEXT_RANGE textRange);
        new HRESULT Draw(IntPtr clientDrawingContext, IDWriteTextRenderer renderer, float originX, float originY);
        new HRESULT GetLineMetrics(out DWRITE_LINE_METRICS lineMetrics, uint maxLineCount, out uint actualLineCount);
        new HRESULT GetMetrics(out DWRITE_TEXT_METRICS textMetrics);
        new HRESULT GetOverhangMetrics(out DWRITE_OVERHANG_METRICS overhangs);
        new HRESULT GetClusterMetrics(out DWRITE_CLUSTER_METRICS clusterMetrics, uint maxClusterCount, out uint actualClusterCount);
        new HRESULT DetermineMinWidth(out float minWidth);
        new HRESULT HitTestPoint(float pointX, float pointY, out bool isTrailingHit, out bool isInside, out DWRITE_HIT_TEST_METRICS hitTestMetrics);
        new HRESULT HitTestTextPosition(uint textPosition, bool isTrailingHit, out float pointX, out float pointY, out DWRITE_HIT_TEST_METRICS hitTestMetrics);
        new HRESULT HitTestTextRange(uint textPosition, uint textLength, float originX, float originY, out DWRITE_HIT_TEST_METRICS hitTestMetrics, uint maxHitTestMetricsCount, out uint actualHitTestMetricsCount);
        #endregion

        new HRESULT SetPairKerning(bool isPairKerningEnabled, DWRITE_TEXT_RANGE textRange);
        new HRESULT GetPairKerning(uint currentPosition, ref bool isPairKerningEnabled, ref DWRITE_TEXT_RANGE textRange);
        new HRESULT SetCharacterSpacing(float leadingSpacing, float trailingSpacing, float minimumAdvanceWidth, DWRITE_TEXT_RANGE textRange);
        new HRESULT GetCharacterSpacing(uint currentPosition, ref float leadingSpacing, out float trailingSpacing, out float minimumAdvanceWidth, out DWRITE_TEXT_RANGE textRange);
        #endregion

        #region <IDWriteTextLayout2>
        new  HRESULT GetMetrics(out DWRITE_TEXT_METRICS1 textMetrics);
        new HRESULT SetVerticalGlyphOrientation(DWRITE_VERTICAL_GLYPH_ORIENTATION glyphOrientation);
        [PreserveSig]
        new DWRITE_VERTICAL_GLYPH_ORIENTATION GetVerticalGlyphOrientation();
        new HRESULT SetLastLineWrapping(bool isLastLineWrappingEnabled);
        [PreserveSig]
        new bool GetLastLineWrapping();
        new HRESULT SetOpticalAlignment(DWRITE_OPTICAL_ALIGNMENT opticalAlignment);
        [PreserveSig]
        new DWRITE_OPTICAL_ALIGNMENT GetOpticalAlignment();
        new HRESULT SetFontFallback(IDWriteFontFallback fontFallback);
        new HRESULT GetFontFallback(out IDWriteFontFallback fontFallback);
        #endregion

        [PreserveSig]
        HRESULT InvalidateLayout();
        [PreserveSig]
        HRESULT SetLineSpacing(ref DWRITE_LINE_SPACING lineSpacingOptions);
        [PreserveSig]
        HRESULT GetLineSpacing(out DWRITE_LINE_SPACING lineSpacingOptions);
        [PreserveSig]
        HRESULT GetLineMetrics([Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] DWRITE_LINE_METRICS[] lineMetrics,
            uint maxLineCount, out uint actualLineCount);
    }

    [ComImport()]
    [Guid("05A9BF42-223F-4441-B5FB-8263685F55E9")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteTextLayout4 : IDWriteTextLayout3
    {
        #region IDWriteTextLayout1
        #region IDWriteTextLayout        
        new HRESULT SetTextAlignment(DWRITE_TEXT_ALIGNMENT textAlignment);
        new HRESULT SetParagraphAlignment(DWRITE_PARAGRAPH_ALIGNMENT paragraphAlignment);
        new HRESULT SetWordWrapping(DWRITE_WORD_WRAPPING wordWrapping);
        new HRESULT SetReadingDirection(DWRITE_READING_DIRECTION readingDirection);
        new HRESULT SetFlowDirection(DWRITE_FLOW_DIRECTION flowDirection);
        new HRESULT SetIncrementalTabStop(float incrementalTabStop);
        new HRESULT SetTrimming(DWRITE_TRIMMING trimmingOptions, IDWriteInlineObject trimmingSign);
        new HRESULT SetLineSpacing(DWRITE_LINE_SPACING_METHOD lineSpacingMethod, float lineSpacing, float baseline);
        [PreserveSig]
        new DWRITE_TEXT_ALIGNMENT GetTextAlignment();
        [PreserveSig]
        new DWRITE_PARAGRAPH_ALIGNMENT GetParagraphAlignment();
        [PreserveSig]
        new DWRITE_WORD_WRAPPING GetWordWrapping();
        [PreserveSig]
        new DWRITE_READING_DIRECTION GetReadingDirection();
        [PreserveSig]
        new DWRITE_FLOW_DIRECTION GetFlowDirection();
        [PreserveSig]
        new float GetIncrementalTabStop();
        new HRESULT GetTrimming(out DWRITE_TRIMMING trimmingOptions, out IDWriteInlineObject trimmingSign);
        new HRESULT GetLineSpacing(out DWRITE_LINE_SPACING_METHOD lineSpacingMethod, out float lineSpacing, out float baseline);
        new HRESULT GetFontCollection(out IDWriteFontCollection fontCollection);
        [PreserveSig]
        new uint GetFontFamilyNameLength();
        new HRESULT GetFontFamilyName(out string fontFamilyName, uint nameSize);
        [PreserveSig]
        new DWRITE_FONT_WEIGHT GetFontWeight();
        [PreserveSig]
        new DWRITE_FONT_STYLE GetFontStyle();
        [PreserveSig]
        new DWRITE_FONT_STRETCH GetFontStretch();
        [PreserveSig]
        new float GetFontSize();
        [PreserveSig]
        new uint GetLocaleNameLength();
        new HRESULT GetLocaleName(out string localeName, uint nameSize);
        new HRESULT SetMaxWidth(float maxWidth);
        new HRESULT SetMaxHeight(float maxHeight);
        new HRESULT SetFontCollection(IDWriteFontCollection fontCollection, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetFontFamilyName(string fontFamilyName, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetFontWeight(DWRITE_FONT_WEIGHT fontWeight, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetFontStyle(DWRITE_FONT_STYLE fontStyle, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetFontStretch(DWRITE_FONT_STRETCH fontStretch, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetFontSize(float fontSize, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetUnderline(bool hasUnderline, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetStrikethrough(bool hasStrikethrough, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetDrawingEffect(IntPtr drawingEffect, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetInlineObject(IDWriteInlineObject inlineObject, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetTypography(IDWriteTypography typography, DWRITE_TEXT_RANGE textRange);
        new HRESULT SetLocaleName(string localeName, DWRITE_TEXT_RANGE textRange);
        [PreserveSig]
        new float GetMaxWidth();
        [PreserveSig]
        new float GetMaxHeight();
        new HRESULT GetFontCollection(uint currentPosition, out IDWriteFontCollection fontCollection, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetFontFamilyNameLength(uint currentPosition, out uint nameLength, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetFontFamilyName(uint currentPosition, out string fontFamilyName, uint nameSize, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetFontWeight(uint currentPosition, out DWRITE_FONT_WEIGHT fontWeight, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetFontStyle(uint currentPosition, out DWRITE_FONT_STYLE fontStyle, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetFontStretch(uint currentPosition, out DWRITE_FONT_STRETCH fontStretch, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetFontSize(uint currentPosition, out float fontSize, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetUnderline(uint currentPosition, out bool hasUnderline, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetStrikethrough(uint currentPosition, out bool hasStrikethrough, out DWRITE_TEXT_RANGE textRange);

        new HRESULT GetDrawingEffect(uint currentPosition, out IntPtr drawingEffect, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetInlineObject(uint currentPosition, out IDWriteInlineObject inlineObject, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetTypography(uint currentPosition, out IDWriteTypography typography, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetLocaleNameLength(uint currentPosition, out uint nameLength, out DWRITE_TEXT_RANGE textRange);
        new HRESULT GetLocaleName(uint currentPosition, out string localeName, uint nameSize, out DWRITE_TEXT_RANGE textRange);
        new HRESULT Draw(IntPtr clientDrawingContext, IDWriteTextRenderer renderer, float originX, float originY);
        new HRESULT GetLineMetrics(out DWRITE_LINE_METRICS lineMetrics, uint maxLineCount, out uint actualLineCount);
        new HRESULT GetMetrics(out DWRITE_TEXT_METRICS textMetrics);
        new HRESULT GetOverhangMetrics(out DWRITE_OVERHANG_METRICS overhangs);
        new HRESULT GetClusterMetrics(out DWRITE_CLUSTER_METRICS clusterMetrics, uint maxClusterCount, out uint actualClusterCount);
        new HRESULT DetermineMinWidth(out float minWidth);
        new HRESULT HitTestPoint(float pointX, float pointY, out bool isTrailingHit, out bool isInside, out DWRITE_HIT_TEST_METRICS hitTestMetrics);
        new HRESULT HitTestTextPosition(uint textPosition, bool isTrailingHit, out float pointX, out float pointY, out DWRITE_HIT_TEST_METRICS hitTestMetrics);
        new HRESULT HitTestTextRange(uint textPosition, uint textLength, float originX, float originY, out DWRITE_HIT_TEST_METRICS hitTestMetrics, uint maxHitTestMetricsCount, out uint actualHitTestMetricsCount);
        #endregion

        new HRESULT SetPairKerning(bool isPairKerningEnabled, DWRITE_TEXT_RANGE textRange);
        new HRESULT GetPairKerning(uint currentPosition, ref bool isPairKerningEnabled, ref DWRITE_TEXT_RANGE textRange);
        new HRESULT SetCharacterSpacing(float leadingSpacing, float trailingSpacing, float minimumAdvanceWidth, DWRITE_TEXT_RANGE textRange);
        new HRESULT GetCharacterSpacing(uint currentPosition, ref float leadingSpacing, out float trailingSpacing, out float minimumAdvanceWidth, out DWRITE_TEXT_RANGE textRange);
        #endregion

        #region <IDWriteTextLayout2>
        new HRESULT GetMetrics(out DWRITE_TEXT_METRICS1 textMetrics);
        new HRESULT SetVerticalGlyphOrientation(DWRITE_VERTICAL_GLYPH_ORIENTATION glyphOrientation);
        [PreserveSig]
        new DWRITE_VERTICAL_GLYPH_ORIENTATION GetVerticalGlyphOrientation();
        new HRESULT SetLastLineWrapping(bool isLastLineWrappingEnabled);
        [PreserveSig]
        new bool GetLastLineWrapping();
        new HRESULT SetOpticalAlignment(DWRITE_OPTICAL_ALIGNMENT opticalAlignment);
        [PreserveSig]
        new DWRITE_OPTICAL_ALIGNMENT GetOpticalAlignment();
        new HRESULT SetFontFallback(IDWriteFontFallback fontFallback);
        new HRESULT GetFontFallback(out IDWriteFontFallback fontFallback);
        #endregion

        #region <IDWriteTextLayout3>
        [PreserveSig]
        new HRESULT InvalidateLayout();
        [PreserveSig]
        new HRESULT SetLineSpacing(ref DWRITE_LINE_SPACING lineSpacingOptions);
        [PreserveSig]
        new HRESULT GetLineSpacing(out DWRITE_LINE_SPACING lineSpacingOptions);
        [PreserveSig]
        new HRESULT GetLineMetrics([Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] DWRITE_LINE_METRICS[] lineMetrics,
            uint maxLineCount, out uint actualLineCount);
        #endregion

        [PreserveSig]
        HRESULT SetFontAxisValues([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] DWRITE_FONT_AXIS_VALUE[] fontAxisValues, uint fontAxisValueCount, DWRITE_TEXT_RANGE textRange);
        [PreserveSig]
        uint GetFontAxisValueCount(uint currentPosition);
        [PreserveSig]
        HRESULT GetFontAxisValues(uint currentPosition, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] DWRITE_FONT_AXIS_VALUE[] fontAxisValues, uint fontAxisValueCount, /* _Out_opt_ DWRITE_TEXT_RANGE* */ IntPtr textRange);
        [PreserveSig]
        DWRITE_AUTOMATIC_FONT_AXES GetAutomaticFontAxes();
        [PreserveSig]
        HRESULT SetAutomaticFontAxes(DWRITE_AUTOMATIC_FONT_AXES automaticFontAxes);
    }

    [ComImport]
    [Guid("8339FDE3-106F-47ab-8373-1C6295EB10B3")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteInlineObject
    {
        //HRESULT Draw(IntPtr clientDrawingContext, IDWriteTextRenderer renderer, float originX, float originY, bool isSideways, bool isRightToLeft, IUnknown* clientDrawingEffect);
        HRESULT Draw(IntPtr clientDrawingContext, IDWriteTextRenderer renderer, float originX, float originY, bool isSideways, bool isRightToLeft, IntPtr clientDrawingEffect);
        HRESULT GetMetrics(out DWRITE_INLINE_OBJECT_METRICS metrics);
        HRESULT GetOverhangMetrics(out DWRITE_OVERHANG_METRICS overhangs);
        HRESULT GetBreakConditions(out DWRITE_BREAK_CONDITION breakConditionBefore, out DWRITE_BREAK_CONDITION breakConditionAfter);
    }

    [ComImport]
    [Guid("b7e6163e-7f46-43b4-84b3-e4e6249c365d")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteTextAnalyzer
    {
        HRESULT AnalyzeScript(IDWriteTextAnalysisSource analysisSource, int textPosition, int textLength, IDWriteTextAnalysisSink analysisSink);
        HRESULT AnalyzeBidi(IDWriteTextAnalysisSource analysisSource, int textPosition, int textLength, IDWriteTextAnalysisSink analysisSink);
        HRESULT AnalyzeNumberSubstitution(IDWriteTextAnalysisSource analysisSource, int textPosition, int textLength, IDWriteTextAnalysisSink analysisSink);
        HRESULT AnalyzeLineBreakpoints(IDWriteTextAnalysisSource analysisSource, int textPosition, int textLength, IDWriteTextAnalysisSink analysisSink);
        HRESULT GetGlyphs(string textString, int textLength, IDWriteFontFace fontFace, bool isSideways, bool isRightToLeft, DWRITE_SCRIPT_ANALYSIS scriptAnalysis,
            string localeName, IDWriteNumberSubstitution numberSubstitution, DWRITE_TYPOGRAPHIC_FEATURES features, int featureRangeLengths, int featureRanges,
            int maxGlyphCount, out UInt16 clusterMap, out DWRITE_SHAPING_TEXT_PROPERTIES textProps, out UInt16 glyphIndices, out DWRITE_SHAPING_GLYPH_PROPERTIES glyphProps, out int actualGlyphCount);
        HRESULT GetGlyphPlacements(string textString, UInt16 clusterMap, DWRITE_SHAPING_TEXT_PROPERTIES textProps, int textLength, UInt16 glyphIndices,
            DWRITE_SHAPING_GLYPH_PROPERTIES glyphProps, int glyphCount, IDWriteFontFace fontFace, float fontEmSize, bool isSideways, bool isRightToLeft,
            DWRITE_SCRIPT_ANALYSIS scriptAnalysis, string localeName, DWRITE_TYPOGRAPHIC_FEATURES features, int featureRangeLengths, int featureRanges,
            out float glyphAdvances, out DWRITE_GLYPH_OFFSET glyphOffsets);
        HRESULT GetGdiCompatibleGlyphPlacements(string textString, UInt16 clusterMap, DWRITE_SHAPING_TEXT_PROPERTIES textProps, int textLength, UInt16 glyphIndices,
            DWRITE_SHAPING_GLYPH_PROPERTIES glyphProps, int glyphCount, IDWriteFontFace fontFace, float fontEmSize, float pixelsPerDip, DWRITE_MATRIX transform,
            bool useGdiNatural, bool isSideways, bool isRightToLeft, DWRITE_SCRIPT_ANALYSIS scriptAnalysis, string localeName, DWRITE_TYPOGRAPHIC_FEATURES features,
            int featureRangeLengths, int featureRanges, out float glyphAdvances, out DWRITE_GLYPH_OFFSET glyphOffsets);
    }

    [ComImport]
    [Guid("80DAD800-E21F-4E83-96CE-BFCCE500DB7C")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteTextAnalyzer1 : IDWriteTextAnalyzer
    {
        #region IDWriteTextAnalyzer
        new HRESULT AnalyzeScript(IDWriteTextAnalysisSource analysisSource, int textPosition, int textLength, IDWriteTextAnalysisSink analysisSink);
        new HRESULT AnalyzeBidi(IDWriteTextAnalysisSource analysisSource, int textPosition, int textLength, IDWriteTextAnalysisSink analysisSink);
        new HRESULT AnalyzeNumberSubstitution(IDWriteTextAnalysisSource analysisSource, int textPosition, int textLength, IDWriteTextAnalysisSink analysisSink);
        new HRESULT AnalyzeLineBreakpoints(IDWriteTextAnalysisSource analysisSource, int textPosition, int textLength, IDWriteTextAnalysisSink analysisSink);
        new HRESULT GetGlyphs(string textString, int textLength, IDWriteFontFace fontFace, bool isSideways, bool isRightToLeft, DWRITE_SCRIPT_ANALYSIS scriptAnalysis,
            string localeName, IDWriteNumberSubstitution numberSubstitution, DWRITE_TYPOGRAPHIC_FEATURES features, int featureRangeLengths, int featureRanges,
            int maxGlyphCount, out UInt16 clusterMap, out DWRITE_SHAPING_TEXT_PROPERTIES textProps, out UInt16 glyphIndices, out DWRITE_SHAPING_GLYPH_PROPERTIES glyphProps, out int actualGlyphCount);
        new HRESULT GetGlyphPlacements(string textString, UInt16 clusterMap, DWRITE_SHAPING_TEXT_PROPERTIES textProps, int textLength, UInt16 glyphIndices,
            DWRITE_SHAPING_GLYPH_PROPERTIES glyphProps, int glyphCount, IDWriteFontFace fontFace, float fontEmSize, bool isSideways, bool isRightToLeft,
            DWRITE_SCRIPT_ANALYSIS scriptAnalysis, string localeName, DWRITE_TYPOGRAPHIC_FEATURES features, int featureRangeLengths, int featureRanges,
            out float glyphAdvances, out DWRITE_GLYPH_OFFSET glyphOffsets);
        new HRESULT GetGdiCompatibleGlyphPlacements(string textString, UInt16 clusterMap, DWRITE_SHAPING_TEXT_PROPERTIES textProps, int textLength, UInt16 glyphIndices,
            DWRITE_SHAPING_GLYPH_PROPERTIES glyphProps, int glyphCount, IDWriteFontFace fontFace, float fontEmSize, float pixelsPerDip, DWRITE_MATRIX transform,
            bool useGdiNatural, bool isSideways, bool isRightToLeft, DWRITE_SCRIPT_ANALYSIS scriptAnalysis, string localeName, DWRITE_TYPOGRAPHIC_FEATURES features,
            int featureRangeLengths, int featureRanges, out float glyphAdvances, out DWRITE_GLYPH_OFFSET glyphOffsets);
        #endregion

        HRESULT ApplyCharacterSpacing(float leadingSpacing, float trailingSpacing, float minimumAdvanceWidth, uint textLength, uint glyphCount, UInt16 clusterMap,
            float glyphAdvances, DWRITE_GLYPH_OFFSET glyphOffsets, DWRITE_SHAPING_GLYPH_PROPERTIES glyphProperties, out float modifiedGlyphAdvances, out DWRITE_GLYPH_OFFSET modifiedGlyphOffsets);
        HRESULT GetBaseline(IDWriteFontFace fontFace, DWRITE_BASELINE baseline, bool isVertical, bool isSimulationAllowed,  DWRITE_SCRIPT_ANALYSIS scriptAnalysis,
            string localeName, out int baselineCoordinate, out bool exists);
        HRESULT AnalyzeVerticalGlyphOrientation(IDWriteTextAnalysisSource1 analysisSource, uint textPosition, uint textLength, IDWriteTextAnalysisSink1 analysisSink);
        HRESULT GetGlyphOrientationTransform(DWRITE_GLYPH_ORIENTATION_ANGLE glyphOrientationAngle, bool isSideways, out DWRITE_MATRIX transform);
        HRESULT GetScriptProperties(DWRITE_SCRIPT_ANALYSIS scriptAnalysis, out DWRITE_SCRIPT_PROPERTIES scriptProperties);
        HRESULT GetTextComplexity(string textString, uint textLength, IDWriteFontFace fontFace, out bool isTextSimple, out uint textLengthRead, out UInt16 glyphIndices);
        HRESULT GetJustificationOpportunities(IDWriteFontFace fontFace, float fontEmSize, DWRITE_SCRIPT_ANALYSIS scriptAnalysis, uint textLength, uint glyphCount,
            string textString, UInt16 clusterMap, DWRITE_SHAPING_GLYPH_PROPERTIES glyphProperties, out DWRITE_JUSTIFICATION_OPPORTUNITY justificationOpportunities);
        HRESULT JustifyGlyphAdvances(float lineWidth, uint glyphCount, DWRITE_JUSTIFICATION_OPPORTUNITY justificationOpportunities, float glyphAdvances, DWRITE_GLYPH_OFFSET glyphOffsets,
            out float justifiedGlyphAdvances, out DWRITE_GLYPH_OFFSET justifiedGlyphOffsets);
        HRESULT GetJustifiedGlyphs(IDWriteFontFace fontFace, float fontEmSize, DWRITE_SCRIPT_ANALYSIS scriptAnalysis, uint textLength, uint glyphCount, uint maxGlyphCount,
            UInt16 clusterMap, UInt16 glyphIndices, float glyphAdvances, float justifiedGlyphAdvances, DWRITE_GLYPH_OFFSET justifiedGlyphOffsets, DWRITE_SHAPING_GLYPH_PROPERTIES glyphProperties,
            out uint actualGlyphCount, out UInt16 modifiedClusterMap, out UInt16 modifiedGlyphIndices, out float modifiedGlyphAdvances, out DWRITE_GLYPH_OFFSET modifiedGlyphOffsets);
    }

    [ComImport]
    [Guid("553A9FF3-5693-4DF7-B52B-74806F7F2EB9")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteTextAnalyzer2 : IDWriteTextAnalyzer1
    {
        #region IDWriteTextAnalyzer1
        #region IDWriteTextAnalyzer
        new HRESULT AnalyzeScript(IDWriteTextAnalysisSource analysisSource, int textPosition, int textLength, IDWriteTextAnalysisSink analysisSink);
        new HRESULT AnalyzeBidi(IDWriteTextAnalysisSource analysisSource, int textPosition, int textLength, IDWriteTextAnalysisSink analysisSink);
        new HRESULT AnalyzeNumberSubstitution(IDWriteTextAnalysisSource analysisSource, int textPosition, int textLength, IDWriteTextAnalysisSink analysisSink);
        new HRESULT AnalyzeLineBreakpoints(IDWriteTextAnalysisSource analysisSource, int textPosition, int textLength, IDWriteTextAnalysisSink analysisSink);
        new HRESULT GetGlyphs(string textString, int textLength, IDWriteFontFace fontFace, bool isSideways, bool isRightToLeft, DWRITE_SCRIPT_ANALYSIS scriptAnalysis,
            string localeName, IDWriteNumberSubstitution numberSubstitution, DWRITE_TYPOGRAPHIC_FEATURES features, int featureRangeLengths, int featureRanges,
            int maxGlyphCount, out UInt16 clusterMap, out DWRITE_SHAPING_TEXT_PROPERTIES textProps, out UInt16 glyphIndices, out DWRITE_SHAPING_GLYPH_PROPERTIES glyphProps, out int actualGlyphCount);
        new HRESULT GetGlyphPlacements(string textString, UInt16 clusterMap, DWRITE_SHAPING_TEXT_PROPERTIES textProps, int textLength, UInt16 glyphIndices,
            DWRITE_SHAPING_GLYPH_PROPERTIES glyphProps, int glyphCount, IDWriteFontFace fontFace, float fontEmSize, bool isSideways, bool isRightToLeft,
            DWRITE_SCRIPT_ANALYSIS scriptAnalysis, string localeName, DWRITE_TYPOGRAPHIC_FEATURES features, int featureRangeLengths, int featureRanges,
            out float glyphAdvances, out DWRITE_GLYPH_OFFSET glyphOffsets);
        new HRESULT GetGdiCompatibleGlyphPlacements(string textString, UInt16 clusterMap, DWRITE_SHAPING_TEXT_PROPERTIES textProps, int textLength, UInt16 glyphIndices,
            DWRITE_SHAPING_GLYPH_PROPERTIES glyphProps, int glyphCount, IDWriteFontFace fontFace, float fontEmSize, float pixelsPerDip, DWRITE_MATRIX transform,
            bool useGdiNatural, bool isSideways, bool isRightToLeft, DWRITE_SCRIPT_ANALYSIS scriptAnalysis, string localeName, DWRITE_TYPOGRAPHIC_FEATURES features,
            int featureRangeLengths, int featureRanges, out float glyphAdvances, out DWRITE_GLYPH_OFFSET glyphOffsets);
        #endregion

        new HRESULT ApplyCharacterSpacing(float leadingSpacing, float trailingSpacing, float minimumAdvanceWidth, uint textLength, uint glyphCount, UInt16 clusterMap,
            float glyphAdvances, DWRITE_GLYPH_OFFSET glyphOffsets, DWRITE_SHAPING_GLYPH_PROPERTIES glyphProperties, out float modifiedGlyphAdvances, out DWRITE_GLYPH_OFFSET modifiedGlyphOffsets);
        new HRESULT GetBaseline(IDWriteFontFace fontFace, DWRITE_BASELINE baseline, bool isVertical, bool isSimulationAllowed, DWRITE_SCRIPT_ANALYSIS scriptAnalysis,
            string localeName, out int baselineCoordinate, out bool exists);
        new HRESULT AnalyzeVerticalGlyphOrientation(IDWriteTextAnalysisSource1 analysisSource, uint textPosition, uint textLength, IDWriteTextAnalysisSink1 analysisSink);
        new HRESULT GetGlyphOrientationTransform(DWRITE_GLYPH_ORIENTATION_ANGLE glyphOrientationAngle, bool isSideways, out DWRITE_MATRIX transform);
        new HRESULT GetScriptProperties(DWRITE_SCRIPT_ANALYSIS scriptAnalysis, out DWRITE_SCRIPT_PROPERTIES scriptProperties);
        new HRESULT GetTextComplexity(string textString, uint textLength, IDWriteFontFace fontFace, out bool isTextSimple, out uint textLengthRead, out UInt16 glyphIndices);
        new HRESULT GetJustificationOpportunities(IDWriteFontFace fontFace, float fontEmSize, DWRITE_SCRIPT_ANALYSIS scriptAnalysis, uint textLength, uint glyphCount,
            string textString, UInt16 clusterMap, DWRITE_SHAPING_GLYPH_PROPERTIES glyphProperties, out DWRITE_JUSTIFICATION_OPPORTUNITY justificationOpportunities);
        new HRESULT JustifyGlyphAdvances(float lineWidth, uint glyphCount, DWRITE_JUSTIFICATION_OPPORTUNITY justificationOpportunities, float glyphAdvances, DWRITE_GLYPH_OFFSET glyphOffsets,
            out float justifiedGlyphAdvances, out DWRITE_GLYPH_OFFSET justifiedGlyphOffsets);
        new HRESULT GetJustifiedGlyphs(IDWriteFontFace fontFace, float fontEmSize, DWRITE_SCRIPT_ANALYSIS scriptAnalysis, uint textLength, uint glyphCount, uint maxGlyphCount,
            UInt16 clusterMap, UInt16 glyphIndices, float glyphAdvances, float justifiedGlyphAdvances, DWRITE_GLYPH_OFFSET justifiedGlyphOffsets, DWRITE_SHAPING_GLYPH_PROPERTIES glyphProperties,
            out uint actualGlyphCount, out UInt16 modifiedClusterMap, out UInt16 modifiedGlyphIndices, out float modifiedGlyphAdvances, out DWRITE_GLYPH_OFFSET modifiedGlyphOffsets);
        #endregion

        HRESULT GetGlyphOrientationTransform(DWRITE_GLYPH_ORIENTATION_ANGLE glyphOrientationAngle, bool isSideways, float originX, float originY, out DWRITE_MATRIX transform);
        HRESULT GetTypographicFeatures(IDWriteFontFace fontFace, DWRITE_SCRIPT_ANALYSIS scriptAnalysis, string localeName, uint maxTagCount, out uint actualTagCount, out DWRITE_FONT_FEATURE_TAG tags);
        HRESULT CheckTypographicFeature(IDWriteFontFace fontFace, DWRITE_SCRIPT_ANALYSIS scriptAnalysis, string localeName, DWRITE_FONT_FEATURE_TAG featureTag, uint glyphCount, UInt16 glyphIndices, out byte featureApplies);
    }

    [ComImport]
    [Guid("acd16696-8c14-4f5d-877e-fe3fc1d32737")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFont
    {
        HRESULT GetFontFamily(out IDWriteFontFamily fontFamily);
        [PreserveSig]
        DWRITE_FONT_WEIGHT GetWeight();
        [PreserveSig]
        DWRITE_FONT_STRETCH GetStretch();
        [PreserveSig]
        DWRITE_FONT_STYLE GetStyle();
        [PreserveSig]
        bool IsSymbolFont();
        HRESULT GetFaceNames(out IDWriteLocalizedStrings names);
        HRESULT GetInformationalStrings(DWRITE_INFORMATIONAL_STRING_ID informationalStringID, out IDWriteLocalizedStrings informationalStrings, out bool exists);
        [PreserveSig]
        DWRITE_FONT_SIMULATIONS GetSimulations();
        void GetMetrics(out DWRITE_FONT_METRICS fontMetrics);
        HRESULT HasCharacter(int unicodeValue, out bool exists);
        [PreserveSig]
        HRESULT CreateFontFace(out IDWriteFontFace fontFace);
    }

    [ComImport]
    [Guid("acd16696-8c14-4f5d-877e-fe3fc1d32738")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFont1 : IDWriteFont
    {
        #region IDWriteFont
        new HRESULT GetFontFamily(out IDWriteFontFamily fontFamily);
        [PreserveSig]
        new DWRITE_FONT_WEIGHT GetWeight();
        [PreserveSig]
        new DWRITE_FONT_STRETCH GetStretch();
        [PreserveSig]
        new DWRITE_FONT_STYLE GetStyle();
        [PreserveSig]
        new bool IsSymbolFont();
        new HRESULT GetFaceNames(out IDWriteLocalizedStrings names);
        new HRESULT GetInformationalStrings(DWRITE_INFORMATIONAL_STRING_ID informationalStringID, out IDWriteLocalizedStrings informationalStrings, out bool exists);
        [PreserveSig]
        new DWRITE_FONT_SIMULATIONS GetSimulations();
        new void GetMetrics(out DWRITE_FONT_METRICS fontMetrics);
        new HRESULT HasCharacter(int unicodeValue, out bool exists);
        [PreserveSig]
        new HRESULT CreateFontFace(out IDWriteFontFace fontFace);
        #endregion

        void GetMetrics(out DWRITE_FONT_METRICS1 fontMetrics);
        //void GetPanose(out DWRITE_PANOSE panose);
        void GetPanose(out IntPtr panose);
        HRESULT GetUnicodeRanges(uint maxRangeCount, out DWRITE_UNICODE_RANGE unicodeRanges, out uint actualRangeCount);
        [PreserveSig]
        bool IsMonospacedFont();
    }

    public enum DWRITE_PANOSE_FAMILY
    {
        DWRITE_PANOSE_FAMILY_ANY = 0,
        DWRITE_PANOSE_FAMILY_NO_FIT = 1,
        DWRITE_PANOSE_FAMILY_TEXT_DISPLAY = 2,
        DWRITE_PANOSE_FAMILY_SCRIPT = 3, // or hand written
        DWRITE_PANOSE_FAMILY_DECORATIVE = 4,
        DWRITE_PANOSE_FAMILY_SYMBOL = 5, // or symbol
        DWRITE_PANOSE_FAMILY_PICTORIAL = DWRITE_PANOSE_FAMILY_SYMBOL
    }

    public enum DWRITE_PANOSE_SERIF_STYLE
    {
        DWRITE_PANOSE_SERIF_STYLE_ANY = 0,
        DWRITE_PANOSE_SERIF_STYLE_NO_FIT = 1,
        DWRITE_PANOSE_SERIF_STYLE_COVE = 2,
        DWRITE_PANOSE_SERIF_STYLE_OBTUSE_COVE = 3,
        DWRITE_PANOSE_SERIF_STYLE_SQUARE_COVE = 4,
        DWRITE_PANOSE_SERIF_STYLE_OBTUSE_SQUARE_COVE = 5,
        DWRITE_PANOSE_SERIF_STYLE_SQUARE = 6,
        DWRITE_PANOSE_SERIF_STYLE_THIN = 7,
        DWRITE_PANOSE_SERIF_STYLE_OVAL = 8,
        DWRITE_PANOSE_SERIF_STYLE_EXAGGERATED = 9,
        DWRITE_PANOSE_SERIF_STYLE_TRIANGLE = 10,
        DWRITE_PANOSE_SERIF_STYLE_NORMAL_SANS = 11,
        DWRITE_PANOSE_SERIF_STYLE_OBTUSE_SANS = 12,
        DWRITE_PANOSE_SERIF_STYLE_PERPENDICULAR_SANS = 13,
        DWRITE_PANOSE_SERIF_STYLE_FLARED = 14,
        DWRITE_PANOSE_SERIF_STYLE_ROUNDED = 15,
        DWRITE_PANOSE_SERIF_STYLE_SCRIPT = 16,
        DWRITE_PANOSE_SERIF_STYLE_PERP_SANS = DWRITE_PANOSE_SERIF_STYLE_PERPENDICULAR_SANS,
        DWRITE_PANOSE_SERIF_STYLE_BONE = DWRITE_PANOSE_SERIF_STYLE_OVAL
    }

    public enum DWRITE_PANOSE_WEIGHT
    {
        DWRITE_PANOSE_WEIGHT_ANY = 0,
        DWRITE_PANOSE_WEIGHT_NO_FIT = 1,
        DWRITE_PANOSE_WEIGHT_VERY_LIGHT = 2,
        DWRITE_PANOSE_WEIGHT_LIGHT = 3,
        DWRITE_PANOSE_WEIGHT_THIN = 4,
        DWRITE_PANOSE_WEIGHT_BOOK = 5,
        DWRITE_PANOSE_WEIGHT_MEDIUM = 6,
        DWRITE_PANOSE_WEIGHT_DEMI = 7,
        DWRITE_PANOSE_WEIGHT_BOLD = 8,
        DWRITE_PANOSE_WEIGHT_HEAVY = 9,
        DWRITE_PANOSE_WEIGHT_BLACK = 10,
        DWRITE_PANOSE_WEIGHT_EXTRA_BLACK = 11,
        DWRITE_PANOSE_WEIGHT_NORD = DWRITE_PANOSE_WEIGHT_EXTRA_BLACK
    }

    public enum DWRITE_PANOSE_PROPORTION
    {
        DWRITE_PANOSE_PROPORTION_ANY = 0,
        DWRITE_PANOSE_PROPORTION_NO_FIT = 1,
        DWRITE_PANOSE_PROPORTION_OLD_STYLE = 2,
        DWRITE_PANOSE_PROPORTION_MODERN = 3,
        DWRITE_PANOSE_PROPORTION_EVEN_WIDTH = 4,
        DWRITE_PANOSE_PROPORTION_EXPANDED = 5,
        DWRITE_PANOSE_PROPORTION_CONDENSED = 6,
        DWRITE_PANOSE_PROPORTION_VERY_EXPANDED = 7,
        DWRITE_PANOSE_PROPORTION_VERY_CONDENSED = 8,
        DWRITE_PANOSE_PROPORTION_MONOSPACED = 9
    }

    public enum DWRITE_PANOSE_CONTRAST
    {
        DWRITE_PANOSE_CONTRAST_ANY = 0,
        DWRITE_PANOSE_CONTRAST_NO_FIT = 1,
        DWRITE_PANOSE_CONTRAST_NONE = 2,
        DWRITE_PANOSE_CONTRAST_VERY_LOW = 3,
        DWRITE_PANOSE_CONTRAST_LOW = 4,
        DWRITE_PANOSE_CONTRAST_MEDIUM_LOW = 5,
        DWRITE_PANOSE_CONTRAST_MEDIUM = 6,
        DWRITE_PANOSE_CONTRAST_MEDIUM_HIGH = 7,
        DWRITE_PANOSE_CONTRAST_HIGH = 8,
        DWRITE_PANOSE_CONTRAST_VERY_HIGH = 9,
        DWRITE_PANOSE_CONTRAST_HORIZONTAL_LOW = 10,
        DWRITE_PANOSE_CONTRAST_HORIZONTAL_MEDIUM = 11,
        DWRITE_PANOSE_CONTRAST_HORIZONTAL_HIGH = 12,
        DWRITE_PANOSE_CONTRAST_BROKEN = 13
    }

    public enum DWRITE_PANOSE_STROKE_VARIATION
    {
        DWRITE_PANOSE_STROKE_VARIATION_ANY = 0,
        DWRITE_PANOSE_STROKE_VARIATION_NO_FIT = 1,
        DWRITE_PANOSE_STROKE_VARIATION_NO_VARIATION = 2,
        DWRITE_PANOSE_STROKE_VARIATION_GRADUAL_DIAGONAL = 3,
        DWRITE_PANOSE_STROKE_VARIATION_GRADUAL_TRANSITIONAL = 4,
        DWRITE_PANOSE_STROKE_VARIATION_GRADUAL_VERTICAL = 5,
        DWRITE_PANOSE_STROKE_VARIATION_GRADUAL_HORIZONTAL = 6,
        DWRITE_PANOSE_STROKE_VARIATION_RAPID_VERTICAL = 7,
        DWRITE_PANOSE_STROKE_VARIATION_RAPID_HORIZONTAL = 8,
        DWRITE_PANOSE_STROKE_VARIATION_INSTANT_VERTICAL = 9,
        DWRITE_PANOSE_STROKE_VARIATION_INSTANT_HORIZONTAL = 10
    }

    public enum DWRITE_PANOSE_ARM_STYLE
    {
        DWRITE_PANOSE_ARM_STYLE_ANY = 0,
        DWRITE_PANOSE_ARM_STYLE_NO_FIT = 1,
        DWRITE_PANOSE_ARM_STYLE_STRAIGHT_ARMS_HORIZONTAL = 2,
        DWRITE_PANOSE_ARM_STYLE_STRAIGHT_ARMS_WEDGE = 3,
        DWRITE_PANOSE_ARM_STYLE_STRAIGHT_ARMS_VERTICAL = 4,
        DWRITE_PANOSE_ARM_STYLE_STRAIGHT_ARMS_SINGLE_SERIF = 5,
        DWRITE_PANOSE_ARM_STYLE_STRAIGHT_ARMS_DOUBLE_SERIF = 6,
        DWRITE_PANOSE_ARM_STYLE_NONSTRAIGHT_ARMS_HORIZONTAL = 7,
        DWRITE_PANOSE_ARM_STYLE_NONSTRAIGHT_ARMS_WEDGE = 8,
        DWRITE_PANOSE_ARM_STYLE_NONSTRAIGHT_ARMS_VERTICAL = 9,
        DWRITE_PANOSE_ARM_STYLE_NONSTRAIGHT_ARMS_SINGLE_SERIF = 10,
        DWRITE_PANOSE_ARM_STYLE_NONSTRAIGHT_ARMS_DOUBLE_SERIF = 11,
        DWRITE_PANOSE_ARM_STYLE_STRAIGHT_ARMS_HORZ = DWRITE_PANOSE_ARM_STYLE_STRAIGHT_ARMS_HORIZONTAL,
        DWRITE_PANOSE_ARM_STYLE_STRAIGHT_ARMS_VERT = DWRITE_PANOSE_ARM_STYLE_STRAIGHT_ARMS_VERTICAL,
        DWRITE_PANOSE_ARM_STYLE_BENT_ARMS_HORZ = DWRITE_PANOSE_ARM_STYLE_NONSTRAIGHT_ARMS_HORIZONTAL,
        DWRITE_PANOSE_ARM_STYLE_BENT_ARMS_WEDGE = DWRITE_PANOSE_ARM_STYLE_NONSTRAIGHT_ARMS_WEDGE,
        DWRITE_PANOSE_ARM_STYLE_BENT_ARMS_VERT = DWRITE_PANOSE_ARM_STYLE_NONSTRAIGHT_ARMS_VERTICAL,
        DWRITE_PANOSE_ARM_STYLE_BENT_ARMS_SINGLE_SERIF = DWRITE_PANOSE_ARM_STYLE_NONSTRAIGHT_ARMS_SINGLE_SERIF,
        DWRITE_PANOSE_ARM_STYLE_BENT_ARMS_DOUBLE_SERIF = DWRITE_PANOSE_ARM_STYLE_NONSTRAIGHT_ARMS_DOUBLE_SERIF
    }

    public enum DWRITE_PANOSE_LETTERFORM
    {
        DWRITE_PANOSE_LETTERFORM_ANY = 0,
        DWRITE_PANOSE_LETTERFORM_NO_FIT = 1,
        DWRITE_PANOSE_LETTERFORM_NORMAL_CONTACT = 2,
        DWRITE_PANOSE_LETTERFORM_NORMAL_WEIGHTED = 3,
        DWRITE_PANOSE_LETTERFORM_NORMAL_BOXED = 4,
        DWRITE_PANOSE_LETTERFORM_NORMAL_FLATTENED = 5,
        DWRITE_PANOSE_LETTERFORM_NORMAL_ROUNDED = 6,
        DWRITE_PANOSE_LETTERFORM_NORMAL_OFF_CENTER = 7,
        DWRITE_PANOSE_LETTERFORM_NORMAL_SQUARE = 8,
        DWRITE_PANOSE_LETTERFORM_OBLIQUE_CONTACT = 9,
        DWRITE_PANOSE_LETTERFORM_OBLIQUE_WEIGHTED = 10,
        DWRITE_PANOSE_LETTERFORM_OBLIQUE_BOXED = 11,
        DWRITE_PANOSE_LETTERFORM_OBLIQUE_FLATTENED = 12,
        DWRITE_PANOSE_LETTERFORM_OBLIQUE_ROUNDED = 13,
        DWRITE_PANOSE_LETTERFORM_OBLIQUE_OFF_CENTER = 14,
        DWRITE_PANOSE_LETTERFORM_OBLIQUE_SQUARE = 15
    }

    public enum DWRITE_PANOSE_MIDLINE
    {
        DWRITE_PANOSE_MIDLINE_ANY = 0,
        DWRITE_PANOSE_MIDLINE_NO_FIT = 1,
        DWRITE_PANOSE_MIDLINE_STANDARD_TRIMMED = 2,
        DWRITE_PANOSE_MIDLINE_STANDARD_POINTED = 3,
        DWRITE_PANOSE_MIDLINE_STANDARD_SERIFED = 4,
        DWRITE_PANOSE_MIDLINE_HIGH_TRIMMED = 5,
        DWRITE_PANOSE_MIDLINE_HIGH_POINTED = 6,
        DWRITE_PANOSE_MIDLINE_HIGH_SERIFED = 7,
        DWRITE_PANOSE_MIDLINE_CONSTANT_TRIMMED = 8,
        DWRITE_PANOSE_MIDLINE_CONSTANT_POINTED = 9,
        DWRITE_PANOSE_MIDLINE_CONSTANT_SERIFED = 10,
        DWRITE_PANOSE_MIDLINE_LOW_TRIMMED = 11,
        DWRITE_PANOSE_MIDLINE_LOW_POINTED = 12,
        DWRITE_PANOSE_MIDLINE_LOW_SERIFED = 13
    }

    public enum DWRITE_PANOSE_XHEIGHT
    {
        DWRITE_PANOSE_XHEIGHT_ANY = 0,
        DWRITE_PANOSE_XHEIGHT_NO_FIT = 1,
        DWRITE_PANOSE_XHEIGHT_CONSTANT_SMALL = 2,
        DWRITE_PANOSE_XHEIGHT_CONSTANT_STANDARD = 3,
        DWRITE_PANOSE_XHEIGHT_CONSTANT_LARGE = 4,
        DWRITE_PANOSE_XHEIGHT_DUCKING_SMALL = 5,
        DWRITE_PANOSE_XHEIGHT_DUCKING_STANDARD = 6,
        DWRITE_PANOSE_XHEIGHT_DUCKING_LARGE = 7,
        DWRITE_PANOSE_XHEIGHT_CONSTANT_STD = DWRITE_PANOSE_XHEIGHT_CONSTANT_STANDARD,
        DWRITE_PANOSE_XHEIGHT_DUCKING_STD = DWRITE_PANOSE_XHEIGHT_DUCKING_STANDARD
    }

    public enum DWRITE_PANOSE_TOOL_KIND
    {
        DWRITE_PANOSE_TOOL_KIND_ANY = 0,
        DWRITE_PANOSE_TOOL_KIND_NO_FIT = 1,
        DWRITE_PANOSE_TOOL_KIND_FLAT_NIB = 2,
        DWRITE_PANOSE_TOOL_KIND_PRESSURE_POINT = 3,
        DWRITE_PANOSE_TOOL_KIND_ENGRAVED = 4,
        DWRITE_PANOSE_TOOL_KIND_BALL = 5,
        DWRITE_PANOSE_TOOL_KIND_BRUSH = 6,
        DWRITE_PANOSE_TOOL_KIND_ROUGH = 7,
        DWRITE_PANOSE_TOOL_KIND_FELT_PEN_BRUSH_TIP = 8,
        DWRITE_PANOSE_TOOL_KIND_WILD_BRUSH = 9
    }

    public enum DWRITE_PANOSE_SPACING
    {
        DWRITE_PANOSE_SPACING_ANY = 0,
        DWRITE_PANOSE_SPACING_NO_FIT = 1,
        DWRITE_PANOSE_SPACING_PROPORTIONAL_SPACED = 2,
        DWRITE_PANOSE_SPACING_MONOSPACED = 3,
    }

    public enum DWRITE_PANOSE_ASPECT_RATIO
    {
        DWRITE_PANOSE_ASPECT_RATIO_ANY = 0,
        DWRITE_PANOSE_ASPECT_RATIO_NO_FIT = 1,
        DWRITE_PANOSE_ASPECT_RATIO_VERY_CONDENSED = 2,
        DWRITE_PANOSE_ASPECT_RATIO_CONDENSED = 3,
        DWRITE_PANOSE_ASPECT_RATIO_NORMAL = 4,
        DWRITE_PANOSE_ASPECT_RATIO_EXPANDED = 5,
        DWRITE_PANOSE_ASPECT_RATIO_VERY_EXPANDED = 6
    }

    public enum DWRITE_PANOSE_SCRIPT_TOPOLOGY
    {
        DWRITE_PANOSE_SCRIPT_TOPOLOGY_ANY = 0,
        DWRITE_PANOSE_SCRIPT_TOPOLOGY_NO_FIT = 1,
        DWRITE_PANOSE_SCRIPT_TOPOLOGY_ROMAN_DISCONNECTED = 2,
        DWRITE_PANOSE_SCRIPT_TOPOLOGY_ROMAN_TRAILING = 3,
        DWRITE_PANOSE_SCRIPT_TOPOLOGY_ROMAN_CONNECTED = 4,
        DWRITE_PANOSE_SCRIPT_TOPOLOGY_CURSIVE_DISCONNECTED = 5,
        DWRITE_PANOSE_SCRIPT_TOPOLOGY_CURSIVE_TRAILING = 6,
        DWRITE_PANOSE_SCRIPT_TOPOLOGY_CURSIVE_CONNECTED = 7,
        DWRITE_PANOSE_SCRIPT_TOPOLOGY_BLACKLETTER_DISCONNECTED = 8,
        DWRITE_PANOSE_SCRIPT_TOPOLOGY_BLACKLETTER_TRAILING = 9,
        DWRITE_PANOSE_SCRIPT_TOPOLOGY_BLACKLETTER_CONNECTED = 10
    }

    public enum DWRITE_PANOSE_SCRIPT_FORM
    {
        DWRITE_PANOSE_SCRIPT_FORM_ANY = 0,
        DWRITE_PANOSE_SCRIPT_FORM_NO_FIT = 1,
        DWRITE_PANOSE_SCRIPT_FORM_UPRIGHT_NO_WRAPPING = 2,
        DWRITE_PANOSE_SCRIPT_FORM_UPRIGHT_SOME_WRAPPING = 3,
        DWRITE_PANOSE_SCRIPT_FORM_UPRIGHT_MORE_WRAPPING = 4,
        DWRITE_PANOSE_SCRIPT_FORM_UPRIGHT_EXTREME_WRAPPING = 5,
        DWRITE_PANOSE_SCRIPT_FORM_OBLIQUE_NO_WRAPPING = 6,
        DWRITE_PANOSE_SCRIPT_FORM_OBLIQUE_SOME_WRAPPING = 7,
        DWRITE_PANOSE_SCRIPT_FORM_OBLIQUE_MORE_WRAPPING = 8,
        DWRITE_PANOSE_SCRIPT_FORM_OBLIQUE_EXTREME_WRAPPING = 9,
        DWRITE_PANOSE_SCRIPT_FORM_EXAGGERATED_NO_WRAPPING = 10,
        DWRITE_PANOSE_SCRIPT_FORM_EXAGGERATED_SOME_WRAPPING = 11,
        DWRITE_PANOSE_SCRIPT_FORM_EXAGGERATED_MORE_WRAPPING = 12,
        DWRITE_PANOSE_SCRIPT_FORM_EXAGGERATED_EXTREME_WRAPPING = 13
    }

    public enum DWRITE_PANOSE_FINIALS
    {
        DWRITE_PANOSE_FINIALS_ANY = 0,
        DWRITE_PANOSE_FINIALS_NO_FIT = 1,
        DWRITE_PANOSE_FINIALS_NONE_NO_LOOPS = 2,
        DWRITE_PANOSE_FINIALS_NONE_CLOSED_LOOPS = 3,
        DWRITE_PANOSE_FINIALS_NONE_OPEN_LOOPS = 4,
        DWRITE_PANOSE_FINIALS_SHARP_NO_LOOPS = 5,
        DWRITE_PANOSE_FINIALS_SHARP_CLOSED_LOOPS = 6,
        DWRITE_PANOSE_FINIALS_SHARP_OPEN_LOOPS = 7,
        DWRITE_PANOSE_FINIALS_TAPERED_NO_LOOPS = 8,
        DWRITE_PANOSE_FINIALS_TAPERED_CLOSED_LOOPS = 9,
        DWRITE_PANOSE_FINIALS_TAPERED_OPEN_LOOPS = 10,
        DWRITE_PANOSE_FINIALS_ROUND_NO_LOOPS = 11,
        DWRITE_PANOSE_FINIALS_ROUND_CLOSED_LOOPS = 12,
        DWRITE_PANOSE_FINIALS_ROUND_OPEN_LOOPS = 13
    }

    public enum DWRITE_PANOSE_XASCENT
    {
        DWRITE_PANOSE_XASCENT_ANY = 0,
        DWRITE_PANOSE_XASCENT_NO_FIT = 1,
        DWRITE_PANOSE_XASCENT_VERY_LOW = 2,
        DWRITE_PANOSE_XASCENT_LOW = 3,
        DWRITE_PANOSE_XASCENT_MEDIUM = 4,
        DWRITE_PANOSE_XASCENT_HIGH = 5,
        DWRITE_PANOSE_XASCENT_VERY_HIGH = 6
    }

    public enum DWRITE_PANOSE_DECORATIVE_CLASS
    {
        DWRITE_PANOSE_DECORATIVE_CLASS_ANY = 0,
        DWRITE_PANOSE_DECORATIVE_CLASS_NO_FIT = 1,
        DWRITE_PANOSE_DECORATIVE_CLASS_DERIVATIVE = 2,
        DWRITE_PANOSE_DECORATIVE_CLASS_NONSTANDARD_TOPOLOGY = 3,
        DWRITE_PANOSE_DECORATIVE_CLASS_NONSTANDARD_ELEMENTS = 4,
        DWRITE_PANOSE_DECORATIVE_CLASS_NONSTANDARD_ASPECT = 5,
        DWRITE_PANOSE_DECORATIVE_CLASS_INITIALS = 6,
        DWRITE_PANOSE_DECORATIVE_CLASS_CARTOON = 7,
        DWRITE_PANOSE_DECORATIVE_CLASS_PICTURE_STEMS = 8,
        DWRITE_PANOSE_DECORATIVE_CLASS_ORNAMENTED = 9,
        DWRITE_PANOSE_DECORATIVE_CLASS_TEXT_AND_BACKGROUND = 10,
        DWRITE_PANOSE_DECORATIVE_CLASS_COLLAGE = 11,
        DWRITE_PANOSE_DECORATIVE_CLASS_MONTAGE = 12
    }

    public enum DWRITE_PANOSE_ASPECT
    {
        DWRITE_PANOSE_ASPECT_ANY = 0,
        DWRITE_PANOSE_ASPECT_NO_FIT = 1,
        DWRITE_PANOSE_ASPECT_SUPER_CONDENSED = 2,
        DWRITE_PANOSE_ASPECT_VERY_CONDENSED = 3,
        DWRITE_PANOSE_ASPECT_CONDENSED = 4,
        DWRITE_PANOSE_ASPECT_NORMAL = 5,
        DWRITE_PANOSE_ASPECT_EXTENDED = 6,
        DWRITE_PANOSE_ASPECT_VERY_EXTENDED = 7,
        DWRITE_PANOSE_ASPECT_SUPER_EXTENDED = 8,
        DWRITE_PANOSE_ASPECT_MONOSPACED = 9
    }

    public enum DWRITE_PANOSE_FILL
    {
        DWRITE_PANOSE_FILL_ANY = 0,
        DWRITE_PANOSE_FILL_NO_FIT = 1,
        DWRITE_PANOSE_FILL_STANDARD_SOLID_FILL = 2,
        DWRITE_PANOSE_FILL_NO_FILL = 3,
        DWRITE_PANOSE_FILL_PATTERNED_FILL = 4,
        DWRITE_PANOSE_FILL_COMPLEX_FILL = 5,
        DWRITE_PANOSE_FILL_SHAPED_FILL = 6,
        DWRITE_PANOSE_FILL_DRAWN_DISTRESSED = 7,
    }

    public enum DWRITE_PANOSE_LINING
    {
        DWRITE_PANOSE_LINING_ANY = 0,
        DWRITE_PANOSE_LINING_NO_FIT = 1,
        DWRITE_PANOSE_LINING_NONE = 2,
        DWRITE_PANOSE_LINING_INLINE = 3,
        DWRITE_PANOSE_LINING_OUTLINE = 4,
        DWRITE_PANOSE_LINING_ENGRAVED = 5,
        DWRITE_PANOSE_LINING_SHADOW = 6,
        DWRITE_PANOSE_LINING_RELIEF = 7,
        DWRITE_PANOSE_LINING_BACKDROP = 8
    }

    public enum DWRITE_PANOSE_DECORATIVE_TOPOLOGY
    {
        DWRITE_PANOSE_DECORATIVE_TOPOLOGY_ANY = 0,
        DWRITE_PANOSE_DECORATIVE_TOPOLOGY_NO_FIT = 1,
        DWRITE_PANOSE_DECORATIVE_TOPOLOGY_STANDARD = 2,
        DWRITE_PANOSE_DECORATIVE_TOPOLOGY_SQUARE = 3,
        DWRITE_PANOSE_DECORATIVE_TOPOLOGY_MULTIPLE_SEGMENT = 4,
        DWRITE_PANOSE_DECORATIVE_TOPOLOGY_ART_DECO = 5,
        DWRITE_PANOSE_DECORATIVE_TOPOLOGY_UNEVEN_WEIGHTING = 6,
        DWRITE_PANOSE_DECORATIVE_TOPOLOGY_DIVERSE_ARMS = 7,
        DWRITE_PANOSE_DECORATIVE_TOPOLOGY_DIVERSE_FORMS = 8,
        DWRITE_PANOSE_DECORATIVE_TOPOLOGY_LOMBARDIC_FORMS = 9,
        DWRITE_PANOSE_DECORATIVE_TOPOLOGY_UPPER_CASE_IN_LOWER_CASE = 10,
        DWRITE_PANOSE_DECORATIVE_TOPOLOGY_IMPLIED_TOPOLOGY = 11,
        DWRITE_PANOSE_DECORATIVE_TOPOLOGY_HORSESHOE_E_AND_A = 12,
        DWRITE_PANOSE_DECORATIVE_TOPOLOGY_CURSIVE = 13,
        DWRITE_PANOSE_DECORATIVE_TOPOLOGY_BLACKLETTER = 14,
        DWRITE_PANOSE_DECORATIVE_TOPOLOGY_SWASH_VARIANCE = 15
    }

    public enum DWRITE_PANOSE_CHARACTER_RANGES
    {
        DWRITE_PANOSE_CHARACTER_RANGES_ANY = 0,
        DWRITE_PANOSE_CHARACTER_RANGES_NO_FIT = 1,
        DWRITE_PANOSE_CHARACTER_RANGES_EXTENDED_COLLECTION = 2,
        DWRITE_PANOSE_CHARACTER_RANGES_LITERALS = 3,
        DWRITE_PANOSE_CHARACTER_RANGES_NO_LOWER_CASE = 4,
        DWRITE_PANOSE_CHARACTER_RANGES_SMALL_CAPS = 5
    }

    public enum DWRITE_PANOSE_SYMBOL_KIND
    {
        DWRITE_PANOSE_SYMBOL_KIND_ANY = 0,
        DWRITE_PANOSE_SYMBOL_KIND_NO_FIT = 1,
        DWRITE_PANOSE_SYMBOL_KIND_MONTAGES = 2,
        DWRITE_PANOSE_SYMBOL_KIND_PICTURES = 3,
        DWRITE_PANOSE_SYMBOL_KIND_SHAPES = 4,
        DWRITE_PANOSE_SYMBOL_KIND_SCIENTIFIC = 5,
        DWRITE_PANOSE_SYMBOL_KIND_MUSIC = 6,
        DWRITE_PANOSE_SYMBOL_KIND_EXPERT = 7,
        DWRITE_PANOSE_SYMBOL_KIND_PATTERNS = 8,
        DWRITE_PANOSE_SYMBOL_KIND_BOARDERS = 9,
        DWRITE_PANOSE_SYMBOL_KIND_ICONS = 10,
        DWRITE_PANOSE_SYMBOL_KIND_LOGOS = 11,
        DWRITE_PANOSE_SYMBOL_KIND_INDUSTRY_SPECIFIC = 12
    }

    public enum DWRITE_PANOSE_SYMBOL_ASPECT_RATIO
    {
        DWRITE_PANOSE_SYMBOL_ASPECT_RATIO_ANY = 0,
        DWRITE_PANOSE_SYMBOL_ASPECT_RATIO_NO_FIT = 1,
        DWRITE_PANOSE_SYMBOL_ASPECT_RATIO_NO_WIDTH = 2,
        DWRITE_PANOSE_SYMBOL_ASPECT_RATIO_EXCEPTIONALLY_WIDE = 3,
        DWRITE_PANOSE_SYMBOL_ASPECT_RATIO_SUPER_WIDE = 4,
        DWRITE_PANOSE_SYMBOL_ASPECT_RATIO_VERY_WIDE = 5,
        DWRITE_PANOSE_SYMBOL_ASPECT_RATIO_WIDE = 6,
        DWRITE_PANOSE_SYMBOL_ASPECT_RATIO_NORMAL = 7,
        DWRITE_PANOSE_SYMBOL_ASPECT_RATIO_NARROW = 8,
        DWRITE_PANOSE_SYMBOL_ASPECT_RATIO_VERY_NARROW = 9
    }

    [ComImport]
    [Guid("29748ed6-8c9c-4a6a-be0b-d912e8538944")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFont2 : IDWriteFont1
    {
        #region IDWriteFont1
        #region IDWriteFont
        new HRESULT GetFontFamily(out IDWriteFontFamily fontFamily);
        [PreserveSig]
        new DWRITE_FONT_WEIGHT GetWeight();
        [PreserveSig]
        new DWRITE_FONT_STRETCH GetStretch();
        [PreserveSig]
        new DWRITE_FONT_STYLE GetStyle();
        [PreserveSig]
        new bool IsSymbolFont();
        new HRESULT GetFaceNames(out IDWriteLocalizedStrings names);
        new HRESULT GetInformationalStrings(DWRITE_INFORMATIONAL_STRING_ID informationalStringID, out IDWriteLocalizedStrings informationalStrings, out bool exists);
        new DWRITE_FONT_SIMULATIONS GetSimulations();
        new void GetMetrics(out DWRITE_FONT_METRICS fontMetrics);
        new HRESULT HasCharacter(int unicodeValue, out bool exists);
        [PreserveSig]
        new HRESULT CreateFontFace(out IDWriteFontFace fontFace);
        #endregion

        new void GetMetrics(out DWRITE_FONT_METRICS1 fontMetrics);
        //new void GetPanose(out DWRITE_PANOSE panose);
        new void GetPanose(out IntPtr panose);
        new HRESULT GetUnicodeRanges(uint maxRangeCount, out DWRITE_UNICODE_RANGE unicodeRanges, out uint actualRangeCount);
        [PreserveSig]
        new bool IsMonospacedFont();
        #endregion

        [PreserveSig]
        bool IsColorFont();
    }

    [ComImport]
    [Guid("29748ED6-8C9C-4A6A-BE0B-D912E8538944")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFont3 : IDWriteFont2
    {
        #region IDWriteFont2
        #region IDWriteFont1
        #region IDWriteFont
        new HRESULT GetFontFamily(out IDWriteFontFamily fontFamily);
        [PreserveSig]
        new DWRITE_FONT_WEIGHT GetWeight();
        [PreserveSig]
        new DWRITE_FONT_STRETCH GetStretch();
        [PreserveSig]
        new DWRITE_FONT_STYLE GetStyle();
        [PreserveSig]
        new bool IsSymbolFont();
        new HRESULT GetFaceNames(out IDWriteLocalizedStrings names);
        new HRESULT GetInformationalStrings(DWRITE_INFORMATIONAL_STRING_ID informationalStringID, out IDWriteLocalizedStrings informationalStrings, out bool exists);
        new DWRITE_FONT_SIMULATIONS GetSimulations();
        new void GetMetrics(out DWRITE_FONT_METRICS fontMetrics);
        new HRESULT HasCharacter(int unicodeValue, out bool exists);
        [PreserveSig]
        new HRESULT CreateFontFace(out IDWriteFontFace fontFace);
        #endregion

        new void GetMetrics(out DWRITE_FONT_METRICS1 fontMetrics);
        //new void GetPanose(out DWRITE_PANOSE panose);
        new void GetPanose(out IntPtr panose);
        new HRESULT GetUnicodeRanges(uint maxRangeCount, out DWRITE_UNICODE_RANGE unicodeRanges, out uint actualRangeCount);
        [PreserveSig]
        new bool IsMonospacedFont();
        #endregion

        [PreserveSig]
        new bool IsColorFont();
        #endregion

        [PreserveSig]
        HRESULT CreateFontFace(out IDWriteFontFace3 fontFace);
        [PreserveSig]
        bool Equals(IDWriteFont font);
        HRESULT GetFontFaceReference( out IDWriteFontFaceReference fontFaceReference);
        [PreserveSig]
        bool HasCharacter(uint unicodeValue);
        [PreserveSig]
        DWRITE_LOCALITY GetLocality();
    }

    [ComImport]
    [Guid("08256209-099a-4b34-b86d-c22b110e7771")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteLocalizedStrings
    {
        [PreserveSig]
        int GetCount();
        HRESULT FindLocaleName(string localeName, out uint index, out bool exists);
        HRESULT GetLocaleNameLength(uint index, out uint length);
        HRESULT GetLocaleName(uint index, out string localeName, uint size);
        HRESULT GetStringLength(uint index, out uint length);
        HRESULT GetString(uint index, System.Text.StringBuilder stringBuffer, uint size);
    }

    [ComImport]
    [Guid("da20d8ef-812a-4c43-9802-62ec4abd7add")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontFamily : IDWriteFontList
    {
        #region IDWriteFontList
        new HRESULT GetFontCollection(out IDWriteFontCollection fontCollection);
        [PreserveSig]
        new int GetFontCount();
        new HRESULT GetFont(int index, out IDWriteFont font);
        #endregion

        HRESULT GetFamilyNames(out IDWriteLocalizedStrings names);
        HRESULT GetFirstMatchingFont(DWRITE_FONT_WEIGHT weight, DWRITE_FONT_STRETCH stretch, DWRITE_FONT_STYLE style, out IDWriteFont matchingFont);
        HRESULT GetMatchingFonts(DWRITE_FONT_WEIGHT weight, DWRITE_FONT_STRETCH stretch, DWRITE_FONT_STYLE style, out IDWriteFontList matchingFonts);
    }

    [ComImport]
    [Guid("DA20D8EF-812A-4C43-9802-62EC4ABD7ADF")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontFamily1 : IDWriteFontFamily
    {
        #region IDWriteFontFamily
        #region IDWriteFontList
        new HRESULT GetFontCollection(out IDWriteFontCollection fontCollection);
        [PreserveSig]
        new int GetFontCount();
        new HRESULT GetFont(int index, out IDWriteFont font);
        #endregion

        new HRESULT GetFamilyNames(out IDWriteLocalizedStrings names);
        new HRESULT GetFirstMatchingFont(DWRITE_FONT_WEIGHT weight, DWRITE_FONT_STRETCH stretch, DWRITE_FONT_STYLE style, out IDWriteFont matchingFont);
        new HRESULT GetMatchingFonts(DWRITE_FONT_WEIGHT weight, DWRITE_FONT_STRETCH stretch, DWRITE_FONT_STYLE style, out IDWriteFontList matchingFonts);
        #endregion

        DWRITE_LOCALITY GetFontLocality(uint listIndex);
        HRESULT GetFont(uint listIndex, out IDWriteFont3 font);
        HRESULT GetFontFaceReference(uint listIndex, out IDWriteFontFaceReference fontFaceReference);
    }

    [ComImport]
    [Guid("3ED49E77-A398-4261-B9CF-C126C2131EF3")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontFamily2 : IDWriteFontFamily1
    {
        #region IDWriteFontFamily1
        #region IDWriteFontFamily
        #region IDWriteFontList
        new HRESULT GetFontCollection(out IDWriteFontCollection fontCollection);
        [PreserveSig]
        new int GetFontCount();
        new HRESULT GetFont(int index, out IDWriteFont font);
        #endregion

        new HRESULT GetFamilyNames(out IDWriteLocalizedStrings names);
        new HRESULT GetFirstMatchingFont(DWRITE_FONT_WEIGHT weight, DWRITE_FONT_STRETCH stretch, DWRITE_FONT_STYLE style, out IDWriteFont matchingFont);
        new HRESULT GetMatchingFonts(DWRITE_FONT_WEIGHT weight, DWRITE_FONT_STRETCH stretch, DWRITE_FONT_STYLE style, out IDWriteFontList matchingFonts);
        #endregion

        new DWRITE_LOCALITY GetFontLocality(uint listIndex);
        new HRESULT GetFont(uint listIndex, out IDWriteFont3 font);
        new HRESULT GetFontFaceReference(uint listIndex, out IDWriteFontFaceReference fontFaceReference);
        #endregion

        HRESULT GetMatchingFonts(DWRITE_FONT_AXIS_VALUE fontAxisValues, uint fontAxisValueCount, out IDWriteFontList2 matchingFonts); 
        HRESULT GetFontSet(out IDWriteFontSet1 fontSet);
    }

    [ComImport]
    [Guid("1a0d8438-1d97-4ec1-aef9-a2fb86ed6acb")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontList
    {
        HRESULT GetFontCollection(out IDWriteFontCollection fontCollection);
        [PreserveSig]
        int GetFontCount();
        HRESULT GetFont(int index, out IDWriteFont font);
    }

    [ComImport]
    [Guid("DA20D8EF-812A-4C43-9802-62EC4ABD7ADE")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontList1 : IDWriteFontList
    {
        #region IDWriteFontList
        new HRESULT GetFontCollection(out IDWriteFontCollection fontCollection);
        [PreserveSig]
        new int GetFontCount();
        new HRESULT GetFont(int index, out IDWriteFont font);
        #endregion

        [PreserveSig]
        DWRITE_LOCALITY GetFontLocality(uint listIndex);
        HRESULT GetFont(uint listIndex, out IDWriteFont3 font);
        HRESULT GetFontFaceReference(uint listIndex, out IDWriteFontFaceReference fontFaceReference);
    }

    [ComImport]
    [Guid("C0763A34-77AF-445A-B735-08C37B0A5BF5")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontList2 : IDWriteFontList1
    {
        #region IDWriteFontList1
        #region IDWriteFontList
        new HRESULT GetFontCollection(out IDWriteFontCollection fontCollection);
        [PreserveSig]
        new int GetFontCount();
        new HRESULT GetFont(int index, out IDWriteFont font);
        #endregion

        [PreserveSig]
        new DWRITE_LOCALITY GetFontLocality(uint listIndex);
        new HRESULT GetFont(uint listIndex, out IDWriteFont3 font);
        new HRESULT GetFontFaceReference(uint listIndex, out IDWriteFontFaceReference fontFaceReference);
        #endregion

        HRESULT GetFontSet(out IDWriteFontSet1 fontSet);
    }

    [ComImport]
    [Guid("688e1a58-5094-47c8-adc8-fbcea60ae92b")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteTextAnalysisSource
    {
        HRESULT GetTextAtPosition(int textPosition, out string textString, out int textLength);
        HRESULT GetTextBeforePosition(int textPosition, out string textString, out int textLength);
        [PreserveSig]
        DWRITE_READING_DIRECTION GetParagraphReadingDirection();
        HRESULT GetLocaleName(int textPosition, out int textLength, out string localeName);
        HRESULT GetNumberSubstitution(int textPosition, out int textLength, out IDWriteNumberSubstitution numberSubstitution);
    }

    [ComImport]
    [Guid("639CFAD8-0FB4-4B21-A58A-067920120009")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteTextAnalysisSource1 : IDWriteTextAnalysisSource
    {
        #region IDWriteTextAnalysisSource
        new HRESULT GetTextAtPosition(int textPosition, out string textString, out int textLength);
        new HRESULT GetTextBeforePosition(int textPosition, out string textString, out int textLength);
        [PreserveSig]
        new DWRITE_READING_DIRECTION GetParagraphReadingDirection();
        new HRESULT GetLocaleName(int textPosition, out int textLength, out string localeName);
        new HRESULT GetNumberSubstitution(int textPosition, out int textLength, out IDWriteNumberSubstitution numberSubstitution);
        #endregion

        HRESULT GetVerticalGlyphOrientation(uint textPosition, out uint textLength, out DWRITE_VERTICAL_GLYPH_ORIENTATION glyphOrientation, out byte bidiLevel);
    }

    [ComImport]
    [Guid("14885CC9-BAB0-4f90-B6ED-5C366A2CD03D")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteNumberSubstitution
    {

    }

    [ComImport]
    [Guid("5f49804d-7024-4d43-bfa9-d25984f53849")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontFace
    {
        [PreserveSig]
        DWRITE_FONT_FACE_TYPE GetType();       
        HRESULT GetFiles([In, Out] ref uint numberOfFiles, [Out, MarshalAs(UnmanagedType.LPArray)] IDWriteFontFile[] fontFiles);
        [PreserveSig]
        int GetIndex();
        [PreserveSig]
        DWRITE_FONT_SIMULATIONS GetSimulations();
        [PreserveSig]
        bool IsSymbolFont();
        void GetMetrics(out DWRITE_FONT_METRICS fontFaceMetrics);
        [PreserveSig]
        UInt16 GetGlyphCount();
        HRESULT GetDesignGlyphMetrics([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] ushort[] glyphIndices, int glyphCount, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] DWRITE_GLYPH_METRICS[] glyphMetrics, bool isSideways = false);
        [PreserveSig]
        HRESULT GetGlyphIndices([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] uint[] codePoints, int codePointCount, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] ushort[] glyphIndices);
        HRESULT TryGetFontTable(int openTypeTableTag, out IntPtr tableData, out int tableSize, out IntPtr tableContext, out bool exists);
        void ReleaseFontTable(IntPtr tableContext);
        HRESULT GetGlyphRunOutline(float emSize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] ushort[] glyphIndices, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] float[] glyphAdvances, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] DWRITE_GLYPH_OFFSET[] glyphOffsets, int glyphCount, bool isSideways, bool isRightToLeft, ID2D1SimplifiedGeometrySink geometrySink);
        HRESULT GetRecommendedRenderingMode(float emSize, float pixelsPerDip, DWRITE_MEASURING_MODE measuringMode, IDWriteRenderingParams renderingParams, out DWRITE_RENDERING_MODE renderingMode);
        HRESULT GetGdiCompatibleMetrics(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, out DWRITE_FONT_METRICS fontFaceMetrics);
        HRESULT GetGdiCompatibleGlyphMetrics(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, bool useGdiNatural, UInt16 glyphIndices, int glyphCount, out DWRITE_GLYPH_METRICS glyphMetrics, bool isSideways = false);
    }

    [ComImport]
    [Guid("a71efdb4-9fdb-4838-ad90-cfc3be8c3daf")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontFace1 : IDWriteFontFace
    {
        #region IDWriteFontFace
        [PreserveSig]
        new DWRITE_FONT_FACE_TYPE GetType();
        new HRESULT GetFiles([In, Out] ref uint numberOfFiles, [Out, MarshalAs(UnmanagedType.LPArray)] IDWriteFontFile[] fontFiles);
        [PreserveSig]
        new int GetIndex();
        [PreserveSig]
        new DWRITE_FONT_SIMULATIONS GetSimulations();
        [PreserveSig]
        new bool IsSymbolFont();
        new void GetMetrics(out DWRITE_FONT_METRICS fontFaceMetrics);
        [PreserveSig]
        new UInt16 GetGlyphCount();
        new HRESULT GetDesignGlyphMetrics([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] ushort[] glyphIndices, int glyphCount, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] DWRITE_GLYPH_METRICS[] glyphMetrics, bool isSideways = false);
        [PreserveSig]
        new HRESULT GetGlyphIndices([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] uint[] codePoints, int codePointCount, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] ushort[] glyphIndices);
        new HRESULT TryGetFontTable(int openTypeTableTag, out IntPtr tableData, out int tableSize, out IntPtr tableContext, out bool exists);
        new void ReleaseFontTable(IntPtr tableContext);
        new HRESULT GetGlyphRunOutline(float emSize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] ushort[] glyphIndices, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] float[] glyphAdvances, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] DWRITE_GLYPH_OFFSET[] glyphOffsets, int glyphCount, bool isSideways, bool isRightToLeft, ID2D1SimplifiedGeometrySink geometrySink);
        new HRESULT GetRecommendedRenderingMode(float emSize, float pixelsPerDip, DWRITE_MEASURING_MODE measuringMode, IDWriteRenderingParams renderingParams, out DWRITE_RENDERING_MODE renderingMode);
        new HRESULT GetGdiCompatibleMetrics(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, out DWRITE_FONT_METRICS fontFaceMetrics);
        new HRESULT GetGdiCompatibleGlyphMetrics(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, bool useGdiNatural, UInt16 glyphIndices, int glyphCount, out DWRITE_GLYPH_METRICS glyphMetrics, bool isSideways = false);
        #endregion

        void GetMetrics(out DWRITE_FONT_METRICS1 fontMetrics);
        HRESULT GetGdiCompatibleMetrics(float emSize, float pixelsPerDip, DWRITE_MATRIX  transform, out DWRITE_FONT_METRICS1 fontMetrics);
        void GetCaretMetrics(out DWRITE_CARET_METRICS caretMetrics);
        HRESULT GetUnicodeRanges(uint maxRangeCount, out DWRITE_UNICODE_RANGE unicodeRanges, out uint actualRangeCount);
        [PreserveSig]
        bool IsMonospacedFont();
        HRESULT GetDesignGlyphAdvances(uint glyphCount, UInt16 glyphIndices, out int glyphAdvances, bool isSideways = false);
        HRESULT GetGdiCompatibleGlyphAdvances(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, bool useGdiNatural, bool isSideways, uint glyphCount,
            UInt16 glyphIndices, out int glyphAdvances);
        HRESULT GetKerningPairAdjustments(uint glyphCount, UInt16 glyphIndices, out int glyphAdvanceAdjustments);
        [PreserveSig]
        bool HasKerningPairs();
        HRESULT GetRecommendedRenderingMode(float fontEmSize, float dpiX, float dpiY, DWRITE_MATRIX transform, bool isSideways, 
            DWRITE_OUTLINE_THRESHOLD outlineThreshold, DWRITE_MEASURING_MODE measuringMode, out DWRITE_RENDERING_MODE renderingMode);
        HRESULT GetVerticalGlyphVariants(uint glyphCount, UInt16 nominalGlyphIndices, out UInt16 verticalGlyphIndices);
        [PreserveSig]
        bool HasVerticalGlyphVariants();
    }

    [ComImport]
    [Guid("d8b768ff-64bc-4e66-982b-ec8e87f693f7")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontFace2 : IDWriteFontFace1
    {
        #region IDWriteFontFace1
        #region IDWriteFontFace
        [PreserveSig]
        new DWRITE_FONT_FACE_TYPE GetType();
        new HRESULT GetFiles([In, Out] ref uint numberOfFiles, [Out, MarshalAs(UnmanagedType.LPArray)] IDWriteFontFile[] fontFiles);
        [PreserveSig]
        new int GetIndex();
        [PreserveSig]
        new DWRITE_FONT_SIMULATIONS GetSimulations();
        [PreserveSig]
        new bool IsSymbolFont();
        new void GetMetrics(out DWRITE_FONT_METRICS fontFaceMetrics);
        [PreserveSig]
        new UInt16 GetGlyphCount();
        new HRESULT GetDesignGlyphMetrics([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] ushort[] glyphIndices, int glyphCount, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] DWRITE_GLYPH_METRICS[] glyphMetrics, bool isSideways = false);
        [PreserveSig]
        new HRESULT GetGlyphIndices([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] uint[] codePoints, int codePointCount, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] ushort[] glyphIndices);
        new HRESULT TryGetFontTable(int openTypeTableTag, out IntPtr tableData, out int tableSize, out IntPtr tableContext, out bool exists);
        new void ReleaseFontTable(IntPtr tableContext);
        new HRESULT GetGlyphRunOutline(float emSize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] ushort[] glyphIndices, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] float[] glyphAdvances, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] DWRITE_GLYPH_OFFSET[] glyphOffsets, int glyphCount, bool isSideways, bool isRightToLeft, ID2D1SimplifiedGeometrySink geometrySink);
        new HRESULT GetRecommendedRenderingMode(float emSize, float pixelsPerDip, DWRITE_MEASURING_MODE measuringMode, IDWriteRenderingParams renderingParams, out DWRITE_RENDERING_MODE renderingMode);
        new HRESULT GetGdiCompatibleMetrics(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, out DWRITE_FONT_METRICS fontFaceMetrics);
        new HRESULT GetGdiCompatibleGlyphMetrics(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, bool useGdiNatural, UInt16 glyphIndices, int glyphCount, out DWRITE_GLYPH_METRICS glyphMetrics, bool isSideways = false);
        #endregion

        new void GetMetrics(out DWRITE_FONT_METRICS1 fontMetrics);
        new HRESULT GetGdiCompatibleMetrics(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, out DWRITE_FONT_METRICS1 fontMetrics);
        new void GetCaretMetrics(out DWRITE_CARET_METRICS caretMetrics);
        new HRESULT GetUnicodeRanges(uint maxRangeCount, out DWRITE_UNICODE_RANGE unicodeRanges, out uint actualRangeCount);
        [PreserveSig]
        new bool IsMonospacedFont();
        new HRESULT GetDesignGlyphAdvances(uint glyphCount, UInt16 glyphIndices, out int glyphAdvances, bool isSideways = false);
        new HRESULT GetGdiCompatibleGlyphAdvances(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, bool useGdiNatural, bool isSideways, uint glyphCount,
            UInt16 glyphIndices, out int glyphAdvances);
        new HRESULT GetKerningPairAdjustments(uint glyphCount, UInt16 glyphIndices, out int glyphAdvanceAdjustments);
        [PreserveSig]
        new bool HasKerningPairs();
        new HRESULT GetRecommendedRenderingMode(float fontEmSize, float dpiX, float dpiY, DWRITE_MATRIX transform, bool isSideways,
            DWRITE_OUTLINE_THRESHOLD outlineThreshold, DWRITE_MEASURING_MODE measuringMode, out DWRITE_RENDERING_MODE renderingMode);
        new HRESULT GetVerticalGlyphVariants(uint glyphCount, UInt16 nominalGlyphIndices, out UInt16 verticalGlyphIndices);
        [PreserveSig]
        new bool HasVerticalGlyphVariants();
        #endregion

        [PreserveSig]
        bool IsColorFont();
        [PreserveSig]
        uint GetColorPaletteCount();
        [PreserveSig]
        uint GetPaletteEntryCount();
        HRESULT GetPaletteEntries(uint colorPaletteIndex, uint firstEntryIndex, uint entryCount, out DWRITE_COLOR_F paletteEntries);
        HRESULT GetRecommendedRenderingMode(float fontEmSize, float dpiX, float dpiY, DWRITE_MATRIX transform, bool isSideways,
            DWRITE_OUTLINE_THRESHOLD outlineThreshold, DWRITE_MEASURING_MODE measuringMode, IDWriteRenderingParams renderingParams, out DWRITE_RENDERING_MODE renderingMode,
            out DWRITE_GRID_FIT_MODE gridFitMode);
    }

    [ComImport]
    [Guid("D37D7598-09BE-4222-A236-2081341CC1F2")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontFace3 : IDWriteFontFace2
    {
        #region IDWriteFontFace2
        #region IDWriteFontFace1
        #region IDWriteFontFace
        [PreserveSig]
        new DWRITE_FONT_FACE_TYPE GetType();
        new HRESULT GetFiles([In, Out] ref uint numberOfFiles, [Out, MarshalAs(UnmanagedType.LPArray)] IDWriteFontFile[] fontFiles);
        [PreserveSig]
        new int GetIndex();
        [PreserveSig]
        new DWRITE_FONT_SIMULATIONS GetSimulations();
        [PreserveSig]
        new bool IsSymbolFont();
        new void GetMetrics(out DWRITE_FONT_METRICS fontFaceMetrics);
        [PreserveSig]
        new UInt16 GetGlyphCount();
        new HRESULT GetDesignGlyphMetrics([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] ushort[] glyphIndices, int glyphCount, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] DWRITE_GLYPH_METRICS[] glyphMetrics, bool isSideways = false);
        [PreserveSig]
        new HRESULT GetGlyphIndices([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] uint[] codePoints, int codePointCount, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] ushort[] glyphIndices);
        new HRESULT TryGetFontTable(int openTypeTableTag, out IntPtr tableData, out int tableSize, out IntPtr tableContext, out bool exists);
        new void ReleaseFontTable(IntPtr tableContext);
        new HRESULT GetGlyphRunOutline(float emSize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] ushort[] glyphIndices, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] float[] glyphAdvances, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] DWRITE_GLYPH_OFFSET[] glyphOffsets, int glyphCount, bool isSideways, bool isRightToLeft, ID2D1SimplifiedGeometrySink geometrySink);
        new HRESULT GetRecommendedRenderingMode(float emSize, float pixelsPerDip, DWRITE_MEASURING_MODE measuringMode, IDWriteRenderingParams renderingParams, out DWRITE_RENDERING_MODE renderingMode);
        new HRESULT GetGdiCompatibleMetrics(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, out DWRITE_FONT_METRICS fontFaceMetrics);
        new HRESULT GetGdiCompatibleGlyphMetrics(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, bool useGdiNatural, UInt16 glyphIndices, int glyphCount, out DWRITE_GLYPH_METRICS glyphMetrics, bool isSideways = false);
        #endregion

        new void GetMetrics(out DWRITE_FONT_METRICS1 fontMetrics);
        new HRESULT GetGdiCompatibleMetrics(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, out DWRITE_FONT_METRICS1 fontMetrics);
        new void GetCaretMetrics(out DWRITE_CARET_METRICS caretMetrics);
        new HRESULT GetUnicodeRanges(uint maxRangeCount, out DWRITE_UNICODE_RANGE unicodeRanges, out uint actualRangeCount);
        [PreserveSig]
        new bool IsMonospacedFont();
        new HRESULT GetDesignGlyphAdvances(uint glyphCount, UInt16 glyphIndices, out int glyphAdvances, bool isSideways = false);
        new HRESULT GetGdiCompatibleGlyphAdvances(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, bool useGdiNatural, bool isSideways, uint glyphCount,
            UInt16 glyphIndices, out int glyphAdvances);
        new HRESULT GetKerningPairAdjustments(uint glyphCount, UInt16 glyphIndices, out int glyphAdvanceAdjustments);
        [PreserveSig]
        new bool HasKerningPairs();
        new HRESULT GetRecommendedRenderingMode(float fontEmSize, float dpiX, float dpiY, DWRITE_MATRIX transform, bool isSideways,
            DWRITE_OUTLINE_THRESHOLD outlineThreshold, DWRITE_MEASURING_MODE measuringMode, out DWRITE_RENDERING_MODE renderingMode);
        new HRESULT GetVerticalGlyphVariants(uint glyphCount, UInt16 nominalGlyphIndices, out UInt16 verticalGlyphIndices);
        [PreserveSig]
        new bool HasVerticalGlyphVariants();
        #endregion

        [PreserveSig]
        new bool IsColorFont();
        [PreserveSig]
        new uint GetColorPaletteCount();
        [PreserveSig]
        new uint GetPaletteEntryCount();
        new HRESULT GetPaletteEntries(uint colorPaletteIndex, uint firstEntryIndex, uint entryCount, out DWRITE_COLOR_F paletteEntries);
        new HRESULT GetRecommendedRenderingMode(float fontEmSize, float dpiX, float dpiY, DWRITE_MATRIX transform, bool isSideways,
            DWRITE_OUTLINE_THRESHOLD outlineThreshold, DWRITE_MEASURING_MODE measuringMode, IDWriteRenderingParams renderingParams, out DWRITE_RENDERING_MODE renderingMode,
            out DWRITE_GRID_FIT_MODE gridFitMode);
        #endregion

        HRESULT GetFontFaceReference(out IDWriteFontFaceReference fontFaceReference);
        //void GetPanose(out DWRITE_PANOSE panose);
        void GetPanose(out IntPtr panose);
        [PreserveSig]
        DWRITE_FONT_WEIGHT GetWeight();
        [PreserveSig]
        DWRITE_FONT_STRETCH GetStretch();
        [PreserveSig]
        DWRITE_FONT_STYLE GetStyle();
        HRESULT GetFamilyNames(out IDWriteLocalizedStrings names);
        HRESULT GetFaceNames(out IDWriteLocalizedStrings names);
        HRESULT GetInformationalStrings(DWRITE_INFORMATIONAL_STRING_ID informationalStringID, out IDWriteLocalizedStrings informationalStrings, out bool exists);
        [PreserveSig]
        bool HasCharacter(uint unicodeValue); 
        HRESULT GetRecommendedRenderingMode(float fontEmSize, float dpiX, float dpiY, DWRITE_MATRIX transform, bool isSideways, DWRITE_OUTLINE_THRESHOLD outlineThreshold,
            DWRITE_MEASURING_MODE measuringMode, IDWriteRenderingParams renderingParams, out DWRITE_RENDERING_MODE1 renderingMode, out DWRITE_GRID_FIT_MODE gridFitMode);
        [PreserveSig]
        bool IsCharacterLocal(uint unicodeValue);
        [PreserveSig]
        bool IsGlyphLocal(UInt16 glyphId); 
        HRESULT AreCharactersLocal(string characters, uint characterCount, bool enqueueIfNotLocal, out bool isLocal);
        HRESULT AreGlyphsLocal(UInt16 glyphIndices, uint glyphCount, bool enqueueIfNotLocal, out bool isLocal);
    }

    [ComImport]
    [Guid("27F2A904-4EB8-441D-9678-0563F53E3E2F")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontFace4 : IDWriteFontFace3
    {
        #region IDWriteFontFace3
        #region IDWriteFontFace2
        #region IDWriteFontFace1
        #region IDWriteFontFace
        [PreserveSig]
        new DWRITE_FONT_FACE_TYPE GetType();
        new HRESULT GetFiles([In, Out] ref uint numberOfFiles, [Out, MarshalAs(UnmanagedType.LPArray)] IDWriteFontFile[] fontFiles);
        [PreserveSig]
        new int GetIndex();
        [PreserveSig]
        new DWRITE_FONT_SIMULATIONS GetSimulations();
        [PreserveSig]
        new bool IsSymbolFont();
        new void GetMetrics(out DWRITE_FONT_METRICS fontFaceMetrics);
        [PreserveSig]
        new UInt16 GetGlyphCount();
        new HRESULT GetDesignGlyphMetrics([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] ushort[] glyphIndices, int glyphCount, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] DWRITE_GLYPH_METRICS[] glyphMetrics, bool isSideways = false);
        [PreserveSig]
        new HRESULT GetGlyphIndices([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] uint[] codePoints, int codePointCount, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] ushort[] glyphIndices);
        new HRESULT TryGetFontTable(int openTypeTableTag, out IntPtr tableData, out int tableSize, out IntPtr tableContext, out bool exists);
        new void ReleaseFontTable(IntPtr tableContext);
        new HRESULT GetGlyphRunOutline(float emSize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] ushort[] glyphIndices, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] float[] glyphAdvances, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] DWRITE_GLYPH_OFFSET[] glyphOffsets, int glyphCount, bool isSideways, bool isRightToLeft, ID2D1SimplifiedGeometrySink geometrySink);
        new HRESULT GetRecommendedRenderingMode(float emSize, float pixelsPerDip, DWRITE_MEASURING_MODE measuringMode, IDWriteRenderingParams renderingParams, out DWRITE_RENDERING_MODE renderingMode);
        new HRESULT GetGdiCompatibleMetrics(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, out DWRITE_FONT_METRICS fontFaceMetrics);
        new HRESULT GetGdiCompatibleGlyphMetrics(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, bool useGdiNatural, UInt16 glyphIndices, int glyphCount, out DWRITE_GLYPH_METRICS glyphMetrics, bool isSideways = false);
        #endregion

        new void GetMetrics(out DWRITE_FONT_METRICS1 fontMetrics);
        new HRESULT GetGdiCompatibleMetrics(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, out DWRITE_FONT_METRICS1 fontMetrics);
        new void GetCaretMetrics(out DWRITE_CARET_METRICS caretMetrics);
        new HRESULT GetUnicodeRanges(uint maxRangeCount, out DWRITE_UNICODE_RANGE unicodeRanges, out uint actualRangeCount);
        [PreserveSig]
        new bool IsMonospacedFont();
        new HRESULT GetDesignGlyphAdvances(uint glyphCount, UInt16 glyphIndices, out int glyphAdvances, bool isSideways = false);
        new HRESULT GetGdiCompatibleGlyphAdvances(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, bool useGdiNatural, bool isSideways, uint glyphCount,
            UInt16 glyphIndices, out int glyphAdvances);
        new HRESULT GetKerningPairAdjustments(uint glyphCount, UInt16 glyphIndices, out int glyphAdvanceAdjustments);
        [PreserveSig]
        new bool HasKerningPairs();
        new HRESULT GetRecommendedRenderingMode(float fontEmSize, float dpiX, float dpiY, DWRITE_MATRIX transform, bool isSideways,
            DWRITE_OUTLINE_THRESHOLD outlineThreshold, DWRITE_MEASURING_MODE measuringMode, out DWRITE_RENDERING_MODE renderingMode);
        new HRESULT GetVerticalGlyphVariants(uint glyphCount, UInt16 nominalGlyphIndices, out UInt16 verticalGlyphIndices);
        [PreserveSig]
        new bool HasVerticalGlyphVariants();
        #endregion

        [PreserveSig]
        new bool IsColorFont();
        [PreserveSig]
        new uint GetColorPaletteCount();
        [PreserveSig]
        new uint GetPaletteEntryCount();
        new HRESULT GetPaletteEntries(uint colorPaletteIndex, uint firstEntryIndex, uint entryCount, out DWRITE_COLOR_F paletteEntries);
        new HRESULT GetRecommendedRenderingMode(float fontEmSize, float dpiX, float dpiY, DWRITE_MATRIX transform, bool isSideways,
            DWRITE_OUTLINE_THRESHOLD outlineThreshold, DWRITE_MEASURING_MODE measuringMode, IDWriteRenderingParams renderingParams, out DWRITE_RENDERING_MODE renderingMode,
            out DWRITE_GRID_FIT_MODE gridFitMode);
        #endregion

        new HRESULT GetFontFaceReference(out IDWriteFontFaceReference fontFaceReference);
        //new void GetPanose(out DWRITE_PANOSE panose);
        new void GetPanose(out IntPtr panose);
        [PreserveSig]
        new DWRITE_FONT_WEIGHT GetWeight();
        [PreserveSig]
        new DWRITE_FONT_STRETCH GetStretch();
        [PreserveSig]
        new DWRITE_FONT_STYLE GetStyle();
        new HRESULT GetFamilyNames(out IDWriteLocalizedStrings names);
        new HRESULT GetFaceNames(out IDWriteLocalizedStrings names);
        new HRESULT GetInformationalStrings(DWRITE_INFORMATIONAL_STRING_ID informationalStringID, out IDWriteLocalizedStrings informationalStrings, out bool exists);
        [PreserveSig]
        new bool HasCharacter(uint unicodeValue);
        new HRESULT GetRecommendedRenderingMode(float fontEmSize, float dpiX, float dpiY, DWRITE_MATRIX transform, bool isSideways, DWRITE_OUTLINE_THRESHOLD outlineThreshold,
            DWRITE_MEASURING_MODE measuringMode, IDWriteRenderingParams renderingParams, out DWRITE_RENDERING_MODE1 renderingMode, out DWRITE_GRID_FIT_MODE gridFitMode);
        [PreserveSig]
        new bool IsCharacterLocal(uint unicodeValue);
        [PreserveSig]
        new bool IsGlyphLocal(UInt16 glyphId);
        new HRESULT AreCharactersLocal(string characters, uint characterCount, bool enqueueIfNotLocal, out bool isLocal);
        new HRESULT AreGlyphsLocal(UInt16 glyphIndices, uint glyphCount, bool enqueueIfNotLocal, out bool isLocal);
        #endregion

        [PreserveSig]
        DWRITE_GLYPH_IMAGE_FORMATS GetGlyphImageFormats();
        HRESULT GetGlyphImageFormats(UInt16 glyphId, uint pixelsPerEmFirst, uint pixelsPerEmLast, out DWRITE_GLYPH_IMAGE_FORMATS glyphImageFormats);
        HRESULT GetGlyphImageData(UInt16 glyphId, uint pixelsPerEm, DWRITE_GLYPH_IMAGE_FORMATS glyphImageFormat, out DWRITE_GLYPH_IMAGE_DATA glyphData, out IntPtr glyphDataContext);
        void ReleaseGlyphImageData(IntPtr glyphDataContext);
    }

    [ComImport]
    [Guid("98EFF3A5-B667-479A-B145-E2FA5B9FDC29")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontFace5 : IDWriteFontFace4
    {
        #region IDWriteFontFace4
        #region IDWriteFontFace3
        #region IDWriteFontFace2
        #region IDWriteFontFace1
        #region IDWriteFontFace
        [PreserveSig]
        new DWRITE_FONT_FACE_TYPE GetType();        
        new HRESULT GetFiles([In, Out] ref uint numberOfFiles, [Out, MarshalAs(UnmanagedType.LPArray)] IDWriteFontFile[] fontFiles);
        [PreserveSig]
        new int GetIndex();
        [PreserveSig]
        new DWRITE_FONT_SIMULATIONS GetSimulations();
        [PreserveSig]
        new bool IsSymbolFont();
        new void GetMetrics(out DWRITE_FONT_METRICS fontFaceMetrics);
        [PreserveSig]
        new UInt16 GetGlyphCount();
        new HRESULT GetDesignGlyphMetrics([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] ushort[] glyphIndices, int glyphCount, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] DWRITE_GLYPH_METRICS[] glyphMetrics, bool isSideways = false);
        [PreserveSig]
        new HRESULT GetGlyphIndices([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] uint[] codePoints, int codePointCount, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] ushort[] glyphIndices);
        new HRESULT TryGetFontTable(int openTypeTableTag, out IntPtr tableData, out int tableSize, out IntPtr tableContext, out bool exists);
        new void ReleaseFontTable(IntPtr tableContext);
        new HRESULT GetGlyphRunOutline(float emSize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] ushort[] glyphIndices, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] float[] glyphAdvances, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] DWRITE_GLYPH_OFFSET[] glyphOffsets, int glyphCount, bool isSideways, bool isRightToLeft, ID2D1SimplifiedGeometrySink geometrySink);
        new HRESULT GetRecommendedRenderingMode(float emSize, float pixelsPerDip, DWRITE_MEASURING_MODE measuringMode, IDWriteRenderingParams renderingParams, out DWRITE_RENDERING_MODE renderingMode);
        new HRESULT GetGdiCompatibleMetrics(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, out DWRITE_FONT_METRICS fontFaceMetrics);
        new HRESULT GetGdiCompatibleGlyphMetrics(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, bool useGdiNatural, UInt16 glyphIndices, int glyphCount, out DWRITE_GLYPH_METRICS glyphMetrics, bool isSideways = false);
        #endregion

        new void GetMetrics(out DWRITE_FONT_METRICS1 fontMetrics);
        new HRESULT GetGdiCompatibleMetrics(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, out DWRITE_FONT_METRICS1 fontMetrics);
        new void GetCaretMetrics(out DWRITE_CARET_METRICS caretMetrics);
        new HRESULT GetUnicodeRanges(uint maxRangeCount, out DWRITE_UNICODE_RANGE unicodeRanges, out uint actualRangeCount);
        [PreserveSig]
        new bool IsMonospacedFont();
        new HRESULT GetDesignGlyphAdvances(uint glyphCount, UInt16 glyphIndices, out int glyphAdvances, bool isSideways = false);
        new HRESULT GetGdiCompatibleGlyphAdvances(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, bool useGdiNatural, bool isSideways, uint glyphCount,
            UInt16 glyphIndices, out int glyphAdvances);
        new HRESULT GetKerningPairAdjustments(uint glyphCount, UInt16 glyphIndices, out int glyphAdvanceAdjustments);
        [PreserveSig]
        new bool HasKerningPairs();
        new HRESULT GetRecommendedRenderingMode(float fontEmSize, float dpiX, float dpiY, DWRITE_MATRIX transform, bool isSideways,
            DWRITE_OUTLINE_THRESHOLD outlineThreshold, DWRITE_MEASURING_MODE measuringMode, out DWRITE_RENDERING_MODE renderingMode);
        new HRESULT GetVerticalGlyphVariants(uint glyphCount, UInt16 nominalGlyphIndices, out UInt16 verticalGlyphIndices);
        [PreserveSig]
        new bool HasVerticalGlyphVariants();
        #endregion

        [PreserveSig]
        new bool IsColorFont();
        [PreserveSig]
        new uint GetColorPaletteCount();
        [PreserveSig]
        new uint GetPaletteEntryCount();
        new HRESULT GetPaletteEntries(uint colorPaletteIndex, uint firstEntryIndex, uint entryCount, out DWRITE_COLOR_F paletteEntries);
        new HRESULT GetRecommendedRenderingMode(float fontEmSize, float dpiX, float dpiY, DWRITE_MATRIX transform, bool isSideways,
            DWRITE_OUTLINE_THRESHOLD outlineThreshold, DWRITE_MEASURING_MODE measuringMode, IDWriteRenderingParams renderingParams, out DWRITE_RENDERING_MODE renderingMode,
            out DWRITE_GRID_FIT_MODE gridFitMode);
        #endregion

        new HRESULT GetFontFaceReference(out IDWriteFontFaceReference fontFaceReference);
        //new void GetPanose(out DWRITE_PANOSE panose);
        new void GetPanose(out IntPtr panose);
        [PreserveSig]
        new DWRITE_FONT_WEIGHT GetWeight();
        [PreserveSig]
        new DWRITE_FONT_STRETCH GetStretch();
        [PreserveSig]
        new DWRITE_FONT_STYLE GetStyle();
        new HRESULT GetFamilyNames(out IDWriteLocalizedStrings names);
        new HRESULT GetFaceNames(out IDWriteLocalizedStrings names);
        new HRESULT GetInformationalStrings(DWRITE_INFORMATIONAL_STRING_ID informationalStringID, out IDWriteLocalizedStrings informationalStrings, out bool exists);
        [PreserveSig]
        new bool HasCharacter(uint unicodeValue);
        new HRESULT GetRecommendedRenderingMode(float fontEmSize, float dpiX, float dpiY, DWRITE_MATRIX transform, bool isSideways, DWRITE_OUTLINE_THRESHOLD outlineThreshold,
            DWRITE_MEASURING_MODE measuringMode, IDWriteRenderingParams renderingParams, out DWRITE_RENDERING_MODE1 renderingMode, out DWRITE_GRID_FIT_MODE gridFitMode);
        [PreserveSig]
        new bool IsCharacterLocal(uint unicodeValue);
        [PreserveSig]
        new bool IsGlyphLocal(UInt16 glyphId);
        new HRESULT AreCharactersLocal(string characters, uint characterCount, bool enqueueIfNotLocal, out bool isLocal);
        new HRESULT AreGlyphsLocal(UInt16 glyphIndices, uint glyphCount, bool enqueueIfNotLocal, out bool isLocal);
        #endregion

        [PreserveSig]
        new DWRITE_GLYPH_IMAGE_FORMATS GetGlyphImageFormats();
        new HRESULT GetGlyphImageFormats(UInt16 glyphId, uint pixelsPerEmFirst, uint pixelsPerEmLast, out DWRITE_GLYPH_IMAGE_FORMATS glyphImageFormats);
        new HRESULT GetGlyphImageData(UInt16 glyphId, uint pixelsPerEm, DWRITE_GLYPH_IMAGE_FORMATS glyphImageFormat, out DWRITE_GLYPH_IMAGE_DATA glyphData, out IntPtr glyphDataContext);
        new void ReleaseGlyphImageData(IntPtr glyphDataContext);
        #endregion

        [PreserveSig]
        uint GetFontAxisValueCount();
        [PreserveSig]
        HRESULT GetFontAxisValues(out DWRITE_FONT_AXIS_VALUE fontAxisValues, uint fontAxisValueCount);
        [PreserveSig]
        bool HasVariations();
        [PreserveSig]
        HRESULT GetFontResource(out IDWriteFontResource fontResource);
        [PreserveSig]
        bool Equals(IDWriteFontFace fontFace);
    }

    [ComImport]
    [Guid("C4B1FE1B-6E84-47D5-B54C-A597981B06AD")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontFace6 : IDWriteFontFace5
    {
        #region IDWriteFontFace4
        #region IDWriteFontFace3
        #region IDWriteFontFace2
        #region IDWriteFontFace1
        #region IDWriteFontFace
        [PreserveSig]
        new DWRITE_FONT_FACE_TYPE GetType();
        new HRESULT GetFiles([In, Out] ref uint numberOfFiles, [Out, MarshalAs(UnmanagedType.LPArray)] IDWriteFontFile[] fontFiles);
        [PreserveSig]
        new int GetIndex();
        [PreserveSig]
        new DWRITE_FONT_SIMULATIONS GetSimulations();
        [PreserveSig]
        new bool IsSymbolFont();
        new void GetMetrics(out DWRITE_FONT_METRICS fontFaceMetrics);
        [PreserveSig]
        new UInt16 GetGlyphCount();
        new HRESULT GetDesignGlyphMetrics([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] ushort[] glyphIndices, int glyphCount, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] DWRITE_GLYPH_METRICS[] glyphMetrics, bool isSideways = false);
        [PreserveSig]
        new HRESULT GetGlyphIndices([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] uint[] codePoints, int codePointCount, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] ushort[] glyphIndices);
        new HRESULT TryGetFontTable(int openTypeTableTag, out IntPtr tableData, out int tableSize, out IntPtr tableContext, out bool exists);
        new void ReleaseFontTable(IntPtr tableContext);
        new HRESULT GetGlyphRunOutline(float emSize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] ushort[] glyphIndices, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] float[] glyphAdvances, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] DWRITE_GLYPH_OFFSET[] glyphOffsets, int glyphCount, bool isSideways, bool isRightToLeft, ID2D1SimplifiedGeometrySink geometrySink);
        new HRESULT GetRecommendedRenderingMode(float emSize, float pixelsPerDip, DWRITE_MEASURING_MODE measuringMode, IDWriteRenderingParams renderingParams, out DWRITE_RENDERING_MODE renderingMode);
        new HRESULT GetGdiCompatibleMetrics(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, out DWRITE_FONT_METRICS fontFaceMetrics);
        new HRESULT GetGdiCompatibleGlyphMetrics(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, bool useGdiNatural, UInt16 glyphIndices, int glyphCount, out DWRITE_GLYPH_METRICS glyphMetrics, bool isSideways = false);
        #endregion

        new void GetMetrics(out DWRITE_FONT_METRICS1 fontMetrics);
        new HRESULT GetGdiCompatibleMetrics(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, out DWRITE_FONT_METRICS1 fontMetrics);
        new void GetCaretMetrics(out DWRITE_CARET_METRICS caretMetrics);
        new HRESULT GetUnicodeRanges(uint maxRangeCount, out DWRITE_UNICODE_RANGE unicodeRanges, out uint actualRangeCount);
        [PreserveSig]
        new bool IsMonospacedFont();
        new HRESULT GetDesignGlyphAdvances(uint glyphCount, UInt16 glyphIndices, out int glyphAdvances, bool isSideways = false);
        new HRESULT GetGdiCompatibleGlyphAdvances(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, bool useGdiNatural, bool isSideways, uint glyphCount,
            UInt16 glyphIndices, out int glyphAdvances);
        new HRESULT GetKerningPairAdjustments(uint glyphCount, UInt16 glyphIndices, out int glyphAdvanceAdjustments);
        [PreserveSig]
        new bool HasKerningPairs();
        new HRESULT GetRecommendedRenderingMode(float fontEmSize, float dpiX, float dpiY, DWRITE_MATRIX transform, bool isSideways,
            DWRITE_OUTLINE_THRESHOLD outlineThreshold, DWRITE_MEASURING_MODE measuringMode, out DWRITE_RENDERING_MODE renderingMode);
        new HRESULT GetVerticalGlyphVariants(uint glyphCount, UInt16 nominalGlyphIndices, out UInt16 verticalGlyphIndices);
        [PreserveSig]
        new bool HasVerticalGlyphVariants();
        #endregion

        [PreserveSig]
        new bool IsColorFont();
        [PreserveSig]
        new uint GetColorPaletteCount();
        [PreserveSig]
        new uint GetPaletteEntryCount();
        new HRESULT GetPaletteEntries(uint colorPaletteIndex, uint firstEntryIndex, uint entryCount, out DWRITE_COLOR_F paletteEntries);
        new HRESULT GetRecommendedRenderingMode(float fontEmSize, float dpiX, float dpiY, DWRITE_MATRIX transform, bool isSideways,
            DWRITE_OUTLINE_THRESHOLD outlineThreshold, DWRITE_MEASURING_MODE measuringMode, IDWriteRenderingParams renderingParams, out DWRITE_RENDERING_MODE renderingMode,
            out DWRITE_GRID_FIT_MODE gridFitMode);
        #endregion

        new HRESULT GetFontFaceReference(out IDWriteFontFaceReference fontFaceReference);
        //new void GetPanose(out DWRITE_PANOSE panose);
        new void GetPanose(out IntPtr panose);
        [PreserveSig]
        new DWRITE_FONT_WEIGHT GetWeight();
        [PreserveSig]
        new DWRITE_FONT_STRETCH GetStretch();
        [PreserveSig]
        new DWRITE_FONT_STYLE GetStyle();
        new HRESULT GetFamilyNames(out IDWriteLocalizedStrings names);
        new HRESULT GetFaceNames(out IDWriteLocalizedStrings names);
        new HRESULT GetInformationalStrings(DWRITE_INFORMATIONAL_STRING_ID informationalStringID, out IDWriteLocalizedStrings informationalStrings, out bool exists);
        [PreserveSig]
        new bool HasCharacter(uint unicodeValue);
        new HRESULT GetRecommendedRenderingMode(float fontEmSize, float dpiX, float dpiY, DWRITE_MATRIX transform, bool isSideways, DWRITE_OUTLINE_THRESHOLD outlineThreshold,
            DWRITE_MEASURING_MODE measuringMode, IDWriteRenderingParams renderingParams, out DWRITE_RENDERING_MODE1 renderingMode, out DWRITE_GRID_FIT_MODE gridFitMode);
        [PreserveSig]
        new bool IsCharacterLocal(uint unicodeValue);
        [PreserveSig]
        new bool IsGlyphLocal(UInt16 glyphId);
        new HRESULT AreCharactersLocal(string characters, uint characterCount, bool enqueueIfNotLocal, out bool isLocal);
        new HRESULT AreGlyphsLocal(UInt16 glyphIndices, uint glyphCount, bool enqueueIfNotLocal, out bool isLocal);
        #endregion

        [PreserveSig]
        new DWRITE_GLYPH_IMAGE_FORMATS GetGlyphImageFormats();
        new HRESULT GetGlyphImageFormats(UInt16 glyphId, uint pixelsPerEmFirst, uint pixelsPerEmLast, out DWRITE_GLYPH_IMAGE_FORMATS glyphImageFormats);
        new HRESULT GetGlyphImageData(UInt16 glyphId, uint pixelsPerEm, DWRITE_GLYPH_IMAGE_FORMATS glyphImageFormat, out DWRITE_GLYPH_IMAGE_DATA glyphData, out IntPtr glyphDataContext);
        new void ReleaseGlyphImageData(IntPtr glyphDataContext);
        #endregion

        #region <IDWriteFontFace5>
        [PreserveSig]
        new uint GetFontAxisValueCount();
        [PreserveSig]
        new HRESULT GetFontAxisValues(out DWRITE_FONT_AXIS_VALUE fontAxisValues, uint fontAxisValueCount);
        [PreserveSig]
        new bool HasVariations();
        [PreserveSig]
        new HRESULT GetFontResource(out IDWriteFontResource fontResource);
        [PreserveSig]
        new bool Equals(IDWriteFontFace fontFace);
        #endregion

        [PreserveSig]
        HRESULT GetFamilyNames(DWRITE_FONT_FAMILY_MODEL fontFamilyModel, out IDWriteLocalizedStrings names);
        [PreserveSig]
        HRESULT GetFaceNames(DWRITE_FONT_FAMILY_MODEL fontFamilyModel, out IDWriteLocalizedStrings names);
    }

    [ComImport]
    [Guid("3945B85B-BC95-40F7-B72C-8B73BFC7E13B")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteFontFace7 : IDWriteFontFace6
    {
        #region IDWriteFontFace4
        #region IDWriteFontFace3
        #region IDWriteFontFace2
        #region IDWriteFontFace1
        #region IDWriteFontFace
        [PreserveSig]
        new DWRITE_FONT_FACE_TYPE GetType();
        new HRESULT GetFiles([In, Out] ref uint numberOfFiles, [Out, MarshalAs(UnmanagedType.LPArray)] IDWriteFontFile[] fontFiles);
        [PreserveSig]
        new int GetIndex();
        [PreserveSig]
        new DWRITE_FONT_SIMULATIONS GetSimulations();
        [PreserveSig]
        new bool IsSymbolFont();
        new void GetMetrics(out DWRITE_FONT_METRICS fontFaceMetrics);
        [PreserveSig]
        new UInt16 GetGlyphCount();
        new HRESULT GetDesignGlyphMetrics([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] ushort[] glyphIndices, int glyphCount, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] DWRITE_GLYPH_METRICS[] glyphMetrics, bool isSideways = false);
        [PreserveSig]
        new HRESULT GetGlyphIndices([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] uint[] codePoints, int codePointCount, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] ushort[] glyphIndices);
        new HRESULT TryGetFontTable(int openTypeTableTag, out IntPtr tableData, out int tableSize, out IntPtr tableContext, out bool exists);
        new void ReleaseFontTable(IntPtr tableContext);
        new HRESULT GetGlyphRunOutline(float emSize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] ushort[] glyphIndices, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] float[] glyphAdvances, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] DWRITE_GLYPH_OFFSET[] glyphOffsets, int glyphCount, bool isSideways, bool isRightToLeft, ID2D1SimplifiedGeometrySink geometrySink);
        new HRESULT GetRecommendedRenderingMode(float emSize, float pixelsPerDip, DWRITE_MEASURING_MODE measuringMode, IDWriteRenderingParams renderingParams, out DWRITE_RENDERING_MODE renderingMode);
        new HRESULT GetGdiCompatibleMetrics(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, out DWRITE_FONT_METRICS fontFaceMetrics);
        new HRESULT GetGdiCompatibleGlyphMetrics(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, bool useGdiNatural, UInt16 glyphIndices, int glyphCount, out DWRITE_GLYPH_METRICS glyphMetrics, bool isSideways = false);
        #endregion

        new void GetMetrics(out DWRITE_FONT_METRICS1 fontMetrics);
        new HRESULT GetGdiCompatibleMetrics(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, out DWRITE_FONT_METRICS1 fontMetrics);
        new void GetCaretMetrics(out DWRITE_CARET_METRICS caretMetrics);
        new HRESULT GetUnicodeRanges(uint maxRangeCount, out DWRITE_UNICODE_RANGE unicodeRanges, out uint actualRangeCount);
        [PreserveSig]
        new bool IsMonospacedFont();
        new HRESULT GetDesignGlyphAdvances(uint glyphCount, UInt16 glyphIndices, out int glyphAdvances, bool isSideways = false);
        new HRESULT GetGdiCompatibleGlyphAdvances(float emSize, float pixelsPerDip, DWRITE_MATRIX transform, bool useGdiNatural, bool isSideways, uint glyphCount,
            UInt16 glyphIndices, out int glyphAdvances);
        new HRESULT GetKerningPairAdjustments(uint glyphCount, UInt16 glyphIndices, out int glyphAdvanceAdjustments);
        [PreserveSig]
        new bool HasKerningPairs();
        new HRESULT GetRecommendedRenderingMode(float fontEmSize, float dpiX, float dpiY, DWRITE_MATRIX transform, bool isSideways,
            DWRITE_OUTLINE_THRESHOLD outlineThreshold, DWRITE_MEASURING_MODE measuringMode, out DWRITE_RENDERING_MODE renderingMode);
        new HRESULT GetVerticalGlyphVariants(uint glyphCount, UInt16 nominalGlyphIndices, out UInt16 verticalGlyphIndices);
        [PreserveSig]
        new bool HasVerticalGlyphVariants();
        #endregion

        [PreserveSig]
        new bool IsColorFont();
        [PreserveSig]
        new uint GetColorPaletteCount();
        [PreserveSig]
        new uint GetPaletteEntryCount();
        new HRESULT GetPaletteEntries(uint colorPaletteIndex, uint firstEntryIndex, uint entryCount, out DWRITE_COLOR_F paletteEntries);
        new HRESULT GetRecommendedRenderingMode(float fontEmSize, float dpiX, float dpiY, DWRITE_MATRIX transform, bool isSideways,
            DWRITE_OUTLINE_THRESHOLD outlineThreshold, DWRITE_MEASURING_MODE measuringMode, IDWriteRenderingParams renderingParams, out DWRITE_RENDERING_MODE renderingMode,
            out DWRITE_GRID_FIT_MODE gridFitMode);
        #endregion

        new HRESULT GetFontFaceReference(out IDWriteFontFaceReference fontFaceReference);
        //new void GetPanose(out DWRITE_PANOSE panose);
        new void GetPanose(out IntPtr panose);
        [PreserveSig]
        new DWRITE_FONT_WEIGHT GetWeight();
        [PreserveSig]
        new DWRITE_FONT_STRETCH GetStretch();
        [PreserveSig]
        new DWRITE_FONT_STYLE GetStyle();
        new HRESULT GetFamilyNames(out IDWriteLocalizedStrings names);
        new HRESULT GetFaceNames(out IDWriteLocalizedStrings names);
        new HRESULT GetInformationalStrings(DWRITE_INFORMATIONAL_STRING_ID informationalStringID, out IDWriteLocalizedStrings informationalStrings, out bool exists);
        [PreserveSig]
        new bool HasCharacter(uint unicodeValue);
        new HRESULT GetRecommendedRenderingMode(float fontEmSize, float dpiX, float dpiY, DWRITE_MATRIX transform, bool isSideways, DWRITE_OUTLINE_THRESHOLD outlineThreshold,
            DWRITE_MEASURING_MODE measuringMode, IDWriteRenderingParams renderingParams, out DWRITE_RENDERING_MODE1 renderingMode, out DWRITE_GRID_FIT_MODE gridFitMode);
        [PreserveSig]
        new bool IsCharacterLocal(uint unicodeValue);
        [PreserveSig]
        new bool IsGlyphLocal(UInt16 glyphId);
        new HRESULT AreCharactersLocal(string characters, uint characterCount, bool enqueueIfNotLocal, out bool isLocal);
        new HRESULT AreGlyphsLocal(UInt16 glyphIndices, uint glyphCount, bool enqueueIfNotLocal, out bool isLocal);
        #endregion

        [PreserveSig]
        new DWRITE_GLYPH_IMAGE_FORMATS GetGlyphImageFormats();
        new HRESULT GetGlyphImageFormats(UInt16 glyphId, uint pixelsPerEmFirst, uint pixelsPerEmLast, out DWRITE_GLYPH_IMAGE_FORMATS glyphImageFormats);
        new HRESULT GetGlyphImageData(UInt16 glyphId, uint pixelsPerEm, DWRITE_GLYPH_IMAGE_FORMATS glyphImageFormat, out DWRITE_GLYPH_IMAGE_DATA glyphData, out IntPtr glyphDataContext);
        new void ReleaseGlyphImageData(IntPtr glyphDataContext);
        #endregion

        #region <IDWriteFontFace5>
        [PreserveSig]
        new uint GetFontAxisValueCount();
        [PreserveSig]
        new HRESULT GetFontAxisValues(out DWRITE_FONT_AXIS_VALUE fontAxisValues, uint fontAxisValueCount);
        [PreserveSig]
        new bool HasVariations();
        [PreserveSig]
        new HRESULT GetFontResource(out IDWriteFontResource fontResource);
        [PreserveSig]
        new bool Equals(IDWriteFontFace fontFace);
        #endregion

        #region <IDWriteFontFace6>
        [PreserveSig]
        new HRESULT GetFamilyNames(DWRITE_FONT_FAMILY_MODEL fontFamilyModel, out IDWriteLocalizedStrings names);
        [PreserveSig]
        new HRESULT GetFaceNames(DWRITE_FONT_FAMILY_MODEL fontFamilyModel, out IDWriteLocalizedStrings names);
        #endregion

        [PreserveSig]
        DWRITE_PAINT_FEATURE_LEVEL GetPaintFeatureLevel(DWRITE_GLYPH_IMAGE_FORMATS glyphImageFormat);
        [PreserveSig]
        HRESULT CreatePaintReader(DWRITE_GLYPH_IMAGE_FORMATS glyphImageFormat, DWRITE_PAINT_FEATURE_LEVEL paintFeatureLevel,
            out IDWritePaintReader paintReader);
    }

    [ComImport]
    [Guid("8128E912-3B97-42A5-AB6C-24AAD3A86E54")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWritePaintReader
    {
        HRESULT SetCurrentGlyph(uint glyphIndex,/*DWRITE_PAINT_ELEMENT**/ IntPtr paintElement, uint structSize,
            out Direct2D.D2D1_RECT_F clipBox, IntPtr /*out DWRITE_PAINT_ATTRIBUTES*/ glyphAttributes);
        HRESULT SetTextColor(ref DWRITE_COLOR_F textColor);
        HRESULT SetColorPaletteIndex(uint colorPaletteIndex);
        HRESULT SetCustomColorPalette([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] DWRITE_COLOR_F[] paletteEntries, uint paletteEntryCount);
        HRESULT MoveToFirstChild(IntPtr /*DWRITE_PAINT_ELEMENT**/ paintElement, uint structSize);
        HRESULT MoveToNextSibling(IntPtr /*DWRITE_PAINT_ELEMENT**/ paintElement, uint structSize);
        HRESULT MoveToParent();
        HRESULT GetGradientStops(uint firstGradientStopIndex, uint gradientStopCount,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] Direct2D.D2D1_GRADIENT_STOP[] gradientStops);
        HRESULT GetGradientStopColors(uint firstGradientStopIndex, uint gradientStopCount,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] DWRITE_PAINT_COLOR[] gradientStopColors);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_PAINT_COLOR
    {
        public DWRITE_COLOR_F value;
        public UInt16 paletteEntryIndex;
        public float alphaMultiplier;
        public DWRITE_PAINT_ATTRIBUTES colorAttributes;
    }

    public enum DWRITE_PAINT_ATTRIBUTES
    {
        DWRITE_PAINT_ATTRIBUTES_NONE = 0,

        /// <summary>
        /// Specifies that the color value (or any color value in the glyph) comes from the font's
        /// color palette. This means the appearance may depend on the current palette index, which
        /// may be important to clients that cache color glyphs.
        /// </summary>
        DWRITE_PAINT_ATTRIBUTES_USES_PALETTE = 0x01,

        /// <summary>
        /// Specifies that the color value (or any color value in the glyph) comes from the client-specified
        /// text color. This means the appearance may depend on the text color, which may be important to
        /// clients that cache color glyphs.
        /// </summary>
        DWRITE_PAINT_ATTRIBUTES_USES_TEXT_COLOR = 0x02
    }
    
    public enum DWRITE_COLOR_COMPOSITE_MODE
    {
        // Porter-Duff modes.
        DWRITE_COLOR_COMPOSITE_CLEAR,
        DWRITE_COLOR_COMPOSITE_SRC,
        DWRITE_COLOR_COMPOSITE_DEST,
        DWRITE_COLOR_COMPOSITE_SRC_OVER,
        DWRITE_COLOR_COMPOSITE_DEST_OVER,
        DWRITE_COLOR_COMPOSITE_SRC_IN,
        DWRITE_COLOR_COMPOSITE_DEST_IN,
        DWRITE_COLOR_COMPOSITE_SRC_OUT,
        DWRITE_COLOR_COMPOSITE_DEST_OUT,
        DWRITE_COLOR_COMPOSITE_SRC_ATOP,
        DWRITE_COLOR_COMPOSITE_DEST_ATOP,
        DWRITE_COLOR_COMPOSITE_XOR,
        DWRITE_COLOR_COMPOSITE_PLUS,

        // Separable color blend modes.
        DWRITE_COLOR_COMPOSITE_SCREEN,
        DWRITE_COLOR_COMPOSITE_OVERLAY,
        DWRITE_COLOR_COMPOSITE_DARKEN,
        DWRITE_COLOR_COMPOSITE_LIGHTEN,
        DWRITE_COLOR_COMPOSITE_COLOR_DODGE,
        DWRITE_COLOR_COMPOSITE_COLOR_BURN,
        DWRITE_COLOR_COMPOSITE_HARD_LIGHT,
        DWRITE_COLOR_COMPOSITE_SOFT_LIGHT,
        DWRITE_COLOR_COMPOSITE_DIFFERENCE,
        DWRITE_COLOR_COMPOSITE_EXCLUSION,
        DWRITE_COLOR_COMPOSITE_MULTIPLY,

        // Non-separable color blend modes.
        DWRITE_COLOR_COMPOSITE_HSL_HUE,
        DWRITE_COLOR_COMPOSITE_HSL_SATURATION,
        DWRITE_COLOR_COMPOSITE_HSL_COLOR,
        DWRITE_COLOR_COMPOSITE_HSL_LUMINOSITY
    }

    public partial struct DWRITE_PAINT_ELEMENT
    {
        public DWRITE_PAINT_TYPE paintType;
        public PAINT_UNION paint;
        [StructLayout(LayoutKind.Explicit)]
        public partial struct PAINT_UNION
        {
            [FieldOffset(0)]
            public PAINT_LAYERS layers;
            [FieldOffset(0)]
            public PAINT_SOLID_GLYPH solidGlyph;
            [FieldOffset(0)]
            public DWRITE_PAINT_COLOR solid;
            [FieldOffset(0)]
            public PAINT_LINEAR_GRADIENT linearGradient;
            [FieldOffset(0)]
            public PAINT_RADIAL_GRADIENT radialGradient;
            [FieldOffset(0)]
            public PAINT_SWEEP_GRADIENT sweepGradient;
            [FieldOffset(0)]
            public PAINT_GLYPH glyph;
            [FieldOffset(0)]
            public PAINT_COLOR_GLYPH colorGlyph;
            [FieldOffset(0)]
            public DWRITE_MATRIX transform;
            [FieldOffset(0)]
            public PAINT_COMPOSITE composite;
            public partial struct PAINT_LAYERS
            {
                public uint childCount;
            }
            public partial struct PAINT_SOLID_GLYPH
            {
                public uint glyphIndex;
                public DWRITE_PAINT_COLOR color;
            }
            public partial struct PAINT_LINEAR_GRADIENT
            {
                public uint extendMode;
                public uint gradientStopCount;
                public float x0;
                public float y0;
                public float x1;
                public float y1;
                public float x2;
                public float y2;
            }
            public partial struct PAINT_RADIAL_GRADIENT
            {
                public uint extendMode;
                public uint gradientStopCount;
                public float x0;
                public float y0;
                public float radius0;
                public float x1;
                public float y1;
                public float radius1;
            }
            public partial struct PAINT_SWEEP_GRADIENT
            {
                public uint extendMode;
                public uint gradientStopCount;
                public float centerX;
                public float centerY;
                public float startAngle;
                public float endAngle;
            }
            public partial struct PAINT_GLYPH
            {
                public uint glyphIndex;
            }
            public partial struct PAINT_COLOR_GLYPH
            {
                public uint glyphIndex;
                public Direct2D.D2D1_RECT_F clipBox;
            }
            public partial struct PAINT_COMPOSITE
            {
                public DWRITE_COLOR_COMPOSITE_MODE mode;
            }
        }
    }

    public enum DWRITE_PAINT_TYPE
    {
        // The following paint types may be returned for color feature levels greater than
        // or equal to DWRITE_PAINT_FEATURE_LEVEL_COLR_V0.
        DWRITE_PAINT_TYPE_NONE,
        DWRITE_PAINT_TYPE_LAYERS,
        DWRITE_PAINT_TYPE_SOLID_GLYPH,

        // The following paint types may be returned for color feature levels greater than
        // or equal to DWRITE_PAINT_FEATURE_LEVEL_COLR_V1.
        DWRITE_PAINT_TYPE_SOLID,
        DWRITE_PAINT_TYPE_LINEAR_GRADIENT,
        DWRITE_PAINT_TYPE_RADIAL_GRADIENT,
        DWRITE_PAINT_TYPE_SWEEP_GRADIENT,
        DWRITE_PAINT_TYPE_GLYPH,
        DWRITE_PAINT_TYPE_COLOR_GLYPH,
        DWRITE_PAINT_TYPE_TRANSFORM,
        DWRITE_PAINT_TYPE_COMPOSITE
    }

    [ComImport]
    [Guid("7d97dbf7-e085-42d4-81e3-6a883bded118")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteGlyphRunAnalysis
    {
        [PreserveSig]
        HRESULT GetAlphaTextureBounds(DWRITE_TEXTURE_TYPE textureType, out RECT textureBounds);
        [PreserveSig]
        HRESULT CreateAlphaTexture(DWRITE_TEXTURE_TYPE textureType, RECT textureBounds, out IntPtr alphaValues, int bufferSize);
        [PreserveSig]
        HRESULT GetAlphaBlendParams(IDWriteRenderingParams renderingParams, out float blendGamma, out float blendEnhancedContrast, out float blendClearTypeLevel);
    }

    [ComImport]
    [Guid("5e5a32a3-8dff-4773-9ff6-0696eab77267")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteBitmapRenderTarget
    {
        [PreserveSig]
        HRESULT DrawGlyphRun(float baselineOriginX, float baselineOriginY, DWRITE_MEASURING_MODE measuringMode, ref DWRITE_GLYPH_RUN glyphRun, IDWriteRenderingParams renderingParams, uint textColor, out RECT blackBoxRect);
        [PreserveSig]
        IntPtr GetMemoryDC();
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.R4)]
        float GetPixelsPerDip();
        [PreserveSig]
        HRESULT SetPixelsPerDip(float pixelsPerDip);
        [PreserveSig]
        HRESULT GetCurrentTransform(out DWRITE_MATRIX transform);
        [PreserveSig]
        HRESULT SetCurrentTransform(DWRITE_MATRIX transform);
        [PreserveSig]
        HRESULT GetSize(out SIZE size);
        [PreserveSig]
        HRESULT Resize(int width, int height);
    }

    [ComImport]
    [Guid("791e8298-3ef3-4230-9880-c9bdecc42064")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteBitmapRenderTarget1 : IDWriteBitmapRenderTarget
    {
        #region <IDWriteBitmapRenderTarget>
        [PreserveSig]
        new HRESULT DrawGlyphRun(float baselineOriginX, float baselineOriginY, DWRITE_MEASURING_MODE measuringMode, ref DWRITE_GLYPH_RUN glyphRun, IDWriteRenderingParams renderingParams, uint textColor, out RECT blackBoxRect);
        [PreserveSig]
        new IntPtr GetMemoryDC();
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.R4)]
        new float GetPixelsPerDip();
        [PreserveSig]
        new HRESULT SetPixelsPerDip(float pixelsPerDip);
        [PreserveSig]
        new HRESULT GetCurrentTransform(out DWRITE_MATRIX transform);
        [PreserveSig]
        new HRESULT SetCurrentTransform(DWRITE_MATRIX transform);
        [PreserveSig]
        new HRESULT GetSize(out SIZE size);
        [PreserveSig]
        new HRESULT Resize(int width, int height);
        #endregion

        [PreserveSig]
        DWRITE_TEXT_ANTIALIAS_MODE GetTextAntialiasMode();
        [PreserveSig]
        HRESULT SetTextAntialiasMode(DWRITE_TEXT_ANTIALIAS_MODE antialiasMode);
    }

    [ComImport]
    [Guid("C553A742-FC01-44DA-A66E-B8B9ED6C3995")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteBitmapRenderTarget2 : IDWriteBitmapRenderTarget1
    {
        #region <IDWriteBitmapRenderTarget>
        [PreserveSig]
        new HRESULT DrawGlyphRun(float baselineOriginX, float baselineOriginY, DWRITE_MEASURING_MODE measuringMode, ref DWRITE_GLYPH_RUN glyphRun, IDWriteRenderingParams renderingParams, uint textColor, out RECT blackBoxRect);
        [PreserveSig]
        new IntPtr GetMemoryDC();
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.R4)]
        new float GetPixelsPerDip();
        [PreserveSig]
        new HRESULT SetPixelsPerDip(float pixelsPerDip);
        [PreserveSig]
        new HRESULT GetCurrentTransform(out DWRITE_MATRIX transform);
        [PreserveSig]
        new HRESULT SetCurrentTransform(DWRITE_MATRIX transform);
        [PreserveSig]
        new HRESULT GetSize(out SIZE size);
        [PreserveSig]
        new HRESULT Resize(int width, int height);
        #endregion

        #region <IDWriteBitmapRenderTarget1>
        [PreserveSig]
        new DWRITE_TEXT_ANTIALIAS_MODE GetTextAntialiasMode();
        [PreserveSig]
        new HRESULT SetTextAntialiasMode(DWRITE_TEXT_ANTIALIAS_MODE antialiasMode);
        #endregion

        [PreserveSig]
        HRESULT GetBitmapData(out DWRITE_BITMAP_DATA_BGRA32 bitmapData);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_BITMAP_DATA_BGRA32
    {
        public uint width;
        public uint height;
        public IntPtr pixels;
    }

    [ComImport]
    [Guid("AEEC37DB-C337-40F1-8E2A-9A41B167B238")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteBitmapRenderTarget3 : IDWriteBitmapRenderTarget2
    {
        #region <IDWriteBitmapRenderTarget>
        [PreserveSig]
        new HRESULT DrawGlyphRun(float baselineOriginX, float baselineOriginY, DWRITE_MEASURING_MODE measuringMode, ref DWRITE_GLYPH_RUN glyphRun, IDWriteRenderingParams renderingParams, uint textColor, out RECT blackBoxRect);
        [PreserveSig]
        new IntPtr GetMemoryDC();
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.R4)]
        new float GetPixelsPerDip();
        [PreserveSig]
        new HRESULT SetPixelsPerDip(float pixelsPerDip);
        [PreserveSig]
        new HRESULT GetCurrentTransform(out DWRITE_MATRIX transform);
        [PreserveSig]
        new HRESULT SetCurrentTransform(DWRITE_MATRIX transform);
        [PreserveSig]
        new HRESULT GetSize(out SIZE size);
        [PreserveSig]
        new HRESULT Resize(int width, int height);
        #endregion

        #region <IDWriteBitmapRenderTarget1>
        [PreserveSig]
        new DWRITE_TEXT_ANTIALIAS_MODE GetTextAntialiasMode();
        [PreserveSig]
        new HRESULT SetTextAntialiasMode(DWRITE_TEXT_ANTIALIAS_MODE antialiasMode);
        #endregion

        #region <IDWriteBitmapRenderTarget2>
        [PreserveSig]        
        new HRESULT GetBitmapData(out DWRITE_BITMAP_DATA_BGRA32 bitmapData);
        #endregion

        [PreserveSig]
        DWRITE_PAINT_FEATURE_LEVEL GetPaintFeatureLevel();
        [PreserveSig]
        HRESULT DrawPaintGlyphRun(float baselineOriginX,float baselineOriginY, DWRITE_MEASURING_MODE measuringMode,
            ref DWRITE_GLYPH_RUN glyphRun, DWRITE_GLYPH_IMAGE_FORMATS glyphImageFormat, uint textColor,
            uint colorPaletteIndex, out RECT blackBoxRect);
        [PreserveSig]
        HRESULT DrawGlyphRunWithColorSupport(float baselineOriginX, float baselineOriginY, DWRITE_MEASURING_MODE measuringMode,
            ref DWRITE_GLYPH_RUN glyphRun, IDWriteRenderingParams renderingParams, uint textColor,
            uint colorPaletteIndex, out RECT blackBoxRect);
    }

    public enum DWRITE_PAINT_FEATURE_LEVEL : int
    {
        /// <summary>
        /// No paint API support.
        /// </summary>
        DWRITE_PAINT_FEATURE_LEVEL_NONE = 0,

        /// <summary>
        /// Specifies a level of functionality corresponding to OpenType COLR version 0.
        /// </summary>
        DWRITE_PAINT_FEATURE_LEVEL_COLR_V0 = 1,

        /// <summary>
        /// Specifies a level of functionality corresponding to OpenType COLR version 1.
        /// </summary>
        DWRITE_PAINT_FEATURE_LEVEL_COLR_V1 = 2
    }

    [ComImport]
    [Guid("5810cd44-0ca0-4701-b3fa-bec5182ae4f6")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteTextAnalysisSink
    {
        HRESULT SetScriptAnalysis(int textPosition, int textLength, DWRITE_SCRIPT_ANALYSIS scriptAnalysis);
        HRESULT SetLineBreakpoints(int textPosition, int textLength, DWRITE_LINE_BREAKPOINT lineBreakpoints);
        HRESULT SetBidiLevel(int textPosition, int textLength, byte explicitLevel, byte resolvedLevel);
        HRESULT SetNumberSubstitution(int textPosition, int textLength, IDWriteNumberSubstitution numberSubstitution);
    }

    [ComImport]
    [Guid("B0D941A0-85E7-4D8B-9FD3-5CED9934482A")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteTextAnalysisSink1 : IDWriteTextAnalysisSink
    {
        #region IDWriteTextAnalysisSink
        new HRESULT SetScriptAnalysis(int textPosition, int textLength, DWRITE_SCRIPT_ANALYSIS scriptAnalysis);
        new HRESULT SetLineBreakpoints(int textPosition, int textLength, DWRITE_LINE_BREAKPOINT lineBreakpoints);
        new HRESULT SetBidiLevel(int textPosition, int textLength, byte explicitLevel, byte resolvedLevel);
        new HRESULT SetNumberSubstitution(int textPosition, int textLength, IDWriteNumberSubstitution numberSubstitution);
        #endregion

        HRESULT SetGlyphOrientation(uint textPosition,uint textLength, DWRITE_GLYPH_ORIENTATION_ANGLE glyphOrientationAngle, byte adjustedBidiLevel,
            bool isSideways, bool isRightToLeft);
    }

    [ComImport]
    [Guid("ef8a8135-5cc6-45fe-8825-c5a0724eb819")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteTextRenderer : IDWritePixelSnapping
    {
        #region IDWritePixelSnapping
        [PreserveSig]
        new HRESULT IsPixelSnappingDisabled(IntPtr clientDrawingContext, out bool isDisabled);
        [PreserveSig]
        new HRESULT GetCurrentTransform(IntPtr clientDrawingContext, out DWRITE_MATRIX transform);
        [PreserveSig]
        new HRESULT GetPixelsPerDip(IntPtr clientDrawingContext, [MarshalAs(UnmanagedType.R4)] out float pixelsPerDip);
        #endregion

        [PreserveSig]     
        HRESULT DrawGlyphRun(IntPtr clientDrawingContext, float baselineOriginX, float baselineOriginY, DWRITE_MEASURING_MODE measuringMode,
        ref DWRITE_GLYPH_RUN glyphRun, IntPtr glyphRunDescription, IntPtr clientDrawingEffect);
        [PreserveSig]
        HRESULT DrawUnderline(IntPtr clientDrawingContext, float baselineOriginX, float baselineOriginY, DWRITE_UNDERLINE underline, IntPtr clientDrawingEffect);
        [PreserveSig]
        HRESULT DrawStrikethrough(IntPtr clientDrawingContext, float baselineOriginX, float baselineOriginY, DWRITE_STRIKETHROUGH strikethrough, IntPtr clientDrawingEffect);
        [PreserveSig]
        HRESULT DrawInlineObject(IntPtr clientDrawingContext, float originX, float originY, IDWriteInlineObject inlineObject, bool isSideways, bool isRightToLeft, IntPtr clientDrawingEffect);
    }

    [ComImport]
    [Guid("D3E0E934-22A0-427E-AAE4-7D9574B59DB1")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWriteTextRenderer1 : IDWriteTextRenderer
    {
        #region IDWriteTextRenderer
        #region IDWritePixelSnapping
        [PreserveSig]
        new HRESULT IsPixelSnappingDisabled(IntPtr clientDrawingContext, out bool isDisabled);
        [PreserveSig]
        new HRESULT GetCurrentTransform(IntPtr clientDrawingContext, out DWRITE_MATRIX transform);
        [PreserveSig]
        new HRESULT GetPixelsPerDip(IntPtr clientDrawingContext, [MarshalAs(UnmanagedType.R4)] out float pixelsPerDip);
        #endregion

        [PreserveSig]
        new HRESULT DrawGlyphRun(IntPtr clientDrawingContext, float baselineOriginX, float baselineOriginY, DWRITE_MEASURING_MODE measuringMode,
            ref DWRITE_GLYPH_RUN glyphRun, IntPtr glyphRunDescription, IntPtr clientDrawingEffect);
        [PreserveSig]
        new HRESULT DrawUnderline(IntPtr clientDrawingContext, float baselineOriginX, float baselineOriginY, DWRITE_UNDERLINE underline, IntPtr clientDrawingEffect);
        [PreserveSig]
        new HRESULT DrawStrikethrough(IntPtr clientDrawingContext, float baselineOriginX, float baselineOriginY, DWRITE_STRIKETHROUGH strikethrough, IntPtr clientDrawingEffect);
        [PreserveSig]
        new HRESULT DrawInlineObject(IntPtr clientDrawingContext, float originX, float originY, IDWriteInlineObject inlineObject, bool isSideways, bool isRightToLeft, IntPtr clientDrawingEffect);
        #endregion

        [PreserveSig]
        HRESULT DrawGlyphRun(IntPtr clientDrawingContext, float baselineOriginX, float baselineOriginY, DWRITE_GLYPH_ORIENTATION_ANGLE orientationAngle, DWRITE_MEASURING_MODE measuringMode,
            ref DWRITE_GLYPH_RUN glyphRun, IntPtr glyphRunDescription, IntPtr clientDrawingEffect);
        [PreserveSig]
        HRESULT DrawUnderline(IntPtr clientDrawingContext, float baselineOriginX, float baselineOriginY, DWRITE_GLYPH_ORIENTATION_ANGLE orientationAngle, DWRITE_UNDERLINE underline, IntPtr clientDrawingEffect);
        [PreserveSig]
        HRESULT DrawStrikethrough(IntPtr clientDrawingContext, float baselineOriginX, float baselineOriginY,DWRITE_GLYPH_ORIENTATION_ANGLE orientationAngle,DWRITE_STRIKETHROUGH strikethrough, IntPtr clientDrawingEffect);
        [PreserveSig]
        HRESULT DrawInlineObject(IntPtr clientDrawingContext, float originX, float originY, DWRITE_GLYPH_ORIENTATION_ANGLE orientationAngle, IDWriteInlineObject inlineObject, bool isSideways, bool isRightToLeft, IntPtr clientDrawingEffect);
    }

    [ComImport]
    [Guid("eaf3a2da-ecf4-4d24-b644-b34f6842024b")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDWritePixelSnapping
    {
        [PreserveSig]
        HRESULT IsPixelSnappingDisabled(IntPtr clientDrawingContext, out bool isDisabled);
        [PreserveSig]
        HRESULT GetCurrentTransform(IntPtr clientDrawingContext, out DWRITE_MATRIX transform);
        [PreserveSig]
        HRESULT GetPixelsPerDip(IntPtr clientDrawingContext, [MarshalAs(UnmanagedType.R4)] out float pixelsPerDip);
    }

    [ComImport]
    [Guid("2cd9069e-12e2-11dc-9fed-001143a055f9")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ID2D1SimplifiedGeometrySink
    {
        HRESULT SetFillMode(D2D1_FILL_MODE fillMode);
        HRESULT SetSegmentFlags(D2D1_PATH_SEGMENT vertexFlags);
        HRESULT BeginFigure(D2D1_POINT_2F startPoint, D2D1_FIGURE_BEGIN figureBegin);
        HRESULT AddLines([MarshalAs(UnmanagedType.LPArray)] D2D1_POINT_2F[] points, uint pointsCount);
        HRESULT AddBeziers(D2D1_BEZIER_SEGMENT beziers, uint beziersCount);
        HRESULT EndFigure(D2D1_FIGURE_END figureEnd);
        HRESULT Close();
    }

    public enum DWRITE_TEXTURE_TYPE
    {
        /// <summary>
        /// Specifies an alpha texture for aliased text rendering (i.e., bi-level, where each pixel is either fully opaque or fully transparent),
        /// with one byte per pixel.
        /// </summary>
        DWRITE_TEXTURE_ALIASED_1x1,
        /// <summary>
        /// Specifies an alpha texture for ClearType text rendering, with three bytes per pixel in the horizontal dimension and 
        /// one byte per pixel in the vertical dimension.
        /// </summary>
        DWRITE_TEXTURE_CLEARTYPE_3x1
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_TYPOGRAPHIC_FEATURES
    {
        /// <summary>
        /// Array of font features.
        /// </summary>
        public DWRITE_FONT_FEATURE features;

        /// <summary>
        /// The number of features.
        /// </summary>
        public int featureCount;
    }

    public enum DWRITE_RENDERING_MODE
    {
        /// <summary>
        /// Specifies that the rendering mode is determined automatically based on the font and size.
        /// </summary>
        DWRITE_RENDERING_MODE_DEFAULT,
        /// <summary>
        /// Specifies that no antialiasing is performed. Each pixel is either set to the foreground 
        /// color of the text or retains the color of the background.
        /// </summary>
        DWRITE_RENDERING_MODE_ALIASED,
        /// <summary>
        /// Specifies that antialiasing is performed in the horizontal direction and the appearance
        /// of glyphs is layout-compatible with GDI using CLEARTYPE_QUALITY. Use DWRITE_MEASURING_MODE_GDI_CLASSIC 
        /// to get glyph advances. The antialiasing may be either ClearType or grayscale depending on
        /// the text antialiasing mode.
        /// </summary>
        DWRITE_RENDERING_MODE_GDI_CLASSIC,
        /// <summary>
        /// Specifies that antialiasing is performed in the horizontal direction and the appearance
        /// of glyphs is layout-compatible with GDI using CLEARTYPE_NATURAL_QUALITY. Glyph advances
        /// are close to the font design advances, but are still rounded to whole pixels. Use
        /// DWRITE_MEASURING_MODE_GDI_NATURAL to get glyph advances. The antialiasing may be either
        /// ClearType or grayscale depending on the text antialiasing mode.
        /// </summary>
        DWRITE_RENDERING_MODE_GDI_NATURAL,
        /// <summary>
        /// Specifies that antialiasing is performed in the horizontal direction. This rendering
        /// mode allows glyphs to be positioned with subpixel precision and is therefore suitable
        /// for natural (i.e., resolution-independent) layout. The antialiasing may be either
        /// ClearType or grayscale depending on the text antialiasing mode.
        /// </summary>
        DWRITE_RENDERING_MODE_NATURAL,
        /// <summary>
        /// Similar to natural mode except that antialiasing is performed in both the horizontal
        /// and vertical directions. This is typically used at larger sizes to make curves and
        /// diagonal lines look smoother. The antialiasing may be either ClearType or grayscale
        /// depending on the text antialiasing mode.
        /// </summary>
        DWRITE_RENDERING_MODE_NATURAL_SYMMETRIC,
        /// <summary>
        /// Specifies that rendering should bypass the rasterizer and use the outlines directly. 
        /// This is typically used at very large sizes.
        /// </summary>
        DWRITE_RENDERING_MODE_OUTLINE,
        // The following names are obsolete, but are kept as aliases to avoid breaking existing code.
        // Each of these rendering modes may result in either ClearType or grayscale antialiasing 
        // depending on the DWRITE_TEXT_ANTIALIASING_MODE.
        DWRITE_RENDERING_MODE_CLEARTYPE_GDI_CLASSIC = DWRITE_RENDERING_MODE_GDI_CLASSIC,
        DWRITE_RENDERING_MODE_CLEARTYPE_GDI_NATURAL = DWRITE_RENDERING_MODE_GDI_NATURAL,
        DWRITE_RENDERING_MODE_CLEARTYPE_NATURAL = DWRITE_RENDERING_MODE_NATURAL,
        DWRITE_RENDERING_MODE_CLEARTYPE_NATURAL_SYMMETRIC = DWRITE_RENDERING_MODE_NATURAL_SYMMETRIC
    }

    public enum DWRITE_PIXEL_GEOMETRY
    {
        /// <summary>
        /// The red, green, and blue color components of each pixel are assumed to occupy the same point.
        /// </summary>
        DWRITE_PIXEL_GEOMETRY_FLAT,
        /// <summary>
        /// Each pixel comprises three vertical stripes, with red on the left, green in the center, and 
        /// blue on the right. This is the most common pixel geometry for LCD monitors.
        /// </summary>
        DWRITE_PIXEL_GEOMETRY_RGB,
        /// <summary>
        /// Each pixel comprises three vertical stripes, with blue on the left, green in the center, and 
        /// red on the right.
        /// </summary>
        DWRITE_PIXEL_GEOMETRY_BGR
    }

    public enum DWRITE_FONT_FILE_TYPE
    {
        /// <summary>
        /// Font type is not recognized by the DirectWrite font system.
        /// </summary>
        DWRITE_FONT_FILE_TYPE_UNKNOWN,
        /// <summary>
        /// OpenType font with CFF outlines.
        /// </summary>
        DWRITE_FONT_FILE_TYPE_CFF,
        /// <summary>
        /// OpenType font with TrueType outlines.
        /// </summary>
        DWRITE_FONT_FILE_TYPE_TRUETYPE,
        /// <summary>
        /// OpenType font that contains a TrueType collection.
        /// </summary>
        DWRITE_FONT_FILE_TYPE_OPENTYPE_COLLECTION,
        /// <summary>
        /// Type 1 PFM font.
        /// </summary>
        DWRITE_FONT_FILE_TYPE_TYPE1_PFM,
        /// <summary>
        /// Type 1 PFB font.
        /// </summary>
        DWRITE_FONT_FILE_TYPE_TYPE1_PFB,
        /// <summary>
        /// Vector .FON font.
        /// </summary>
        DWRITE_FONT_FILE_TYPE_VECTOR,
        /// <summary>
        /// Bitmap .FON font.
        /// </summary>
        DWRITE_FONT_FILE_TYPE_BITMAP,
        // The following name is obsolete, but kept as an alias to avoid breaking existing code.
        DWRITE_FONT_FILE_TYPE_TRUETYPE_COLLECTION = DWRITE_FONT_FILE_TYPE_OPENTYPE_COLLECTION,
    }

    public enum DWRITE_FONT_FACE_TYPE
    {
        /// <summary>
        /// OpenType font face with CFF outlines.
        /// </summary>
        DWRITE_FONT_FACE_TYPE_CFF,
        /// <summary>
        /// OpenType font face with TrueType outlines.
        /// </summary>
        DWRITE_FONT_FACE_TYPE_TRUETYPE,
        /// <summary>
        /// OpenType font face that is a part of a TrueType or CFF collection.
        /// </summary>
        DWRITE_FONT_FACE_TYPE_OPENTYPE_COLLECTION,
        /// <summary>
        /// A Type 1 font face.
        /// </summary>
        DWRITE_FONT_FACE_TYPE_TYPE1,
        /// <summary>
        /// A vector .FON format font face.
        /// </summary>
        DWRITE_FONT_FACE_TYPE_VECTOR,
        /// <summary>
        /// A bitmap .FON format font face.
        /// </summary>
        DWRITE_FONT_FACE_TYPE_BITMAP,
        /// <summary>
        /// Font face type is not recognized by the DirectWrite font system.
        /// </summary>
        DWRITE_FONT_FACE_TYPE_UNKNOWN,
        /// <summary>
        /// The font data includes only the CFF table from an OpenType CFF font.
        /// This font face type can be used only for embedded fonts (i.e., custom
        /// font file loaders) and the resulting font face object supports only the
        /// minimum functionality necessary to render glyphs.
        /// </summary>
        DWRITE_FONT_FACE_TYPE_RAW_CFF,
        // The following name is obsolete, but kept as an alias to avoid breaking existing code.
        DWRITE_FONT_FACE_TYPE_TRUETYPE_COLLECTION = DWRITE_FONT_FACE_TYPE_OPENTYPE_COLLECTION,
    }

    public enum DWRITE_FONT_SIMULATIONS
    {
        /// <summary>
        /// No simulations are performed.
        /// </summary>
        DWRITE_FONT_SIMULATIONS_NONE = 0x0000,
        /// <summary>
        /// Algorithmic emboldening is performed.
        /// </summary>
        DWRITE_FONT_SIMULATIONS_BOLD = 0x0001,
        /// <summary>
        /// Algorithmic italicization is performed.
        /// </summary>
        DWRITE_FONT_SIMULATIONS_OBLIQUE = 0x0002
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_FONT_METRICS
    {
        /// <summary>
        /// The number of font design units per em unit.
        /// Font files use their own coordinate system of font design units.
        /// A font design unit is the smallest measurable unit in the em square,
        /// an imaginary square that is used to size and align glyphs.
        /// The concept of em square is used as a reference scale factor when defining font size and device transformation semantics.
        /// The size of one em square is also commonly used to compute the paragraph indentation value.
        /// </summary>
        public UInt16 designUnitsPerEm;
        /// <summary>
        /// Ascent value of the font face in font design units.
        /// Ascent is the distance from the top of font character alignment box to English baseline.
        /// </summary>
        public UInt16 ascent;
        /// <summary>
        /// Descent value of the font face in font design units.
        /// Descent is the distance from the bottom of font character alignment box to English baseline.
        /// </summary>
        public UInt16 descent;
        /// <summary>
        /// Line gap in font design units.
        /// Recommended additional white space to add between lines to improve legibility. The recommended line spacing 
        /// (baseline-to-baseline distance) is thus the sum of ascent, descent, and lineGap. The line gap is usually 
        /// positive or zero but can be negative, in which case the recommended line spacing is less than the height
        /// of the character alignment box.
        /// </summary>
        public UInt16 lineGap;
        /// <summary>
        /// Cap height value of the font face in font design units.
        /// Cap height is the distance from English baseline to the top of a typical English capital.
        /// Capital "H" is often used as a reference character for the purpose of calculating the cap height value.
        /// </summary>
        public UInt16 capHeight;
        /// <summary>
        /// x-height value of the font face in font design units.
        /// x-height is the distance from English baseline to the top of lowercase letter "x", or a similar lowercase character.
        /// </summary>
        public UInt16 xHeight;
        /// <summary>
        /// The underline position value of the font face in font design units.
        /// Underline position is the position of underline relative to the English baseline.
        /// The value is usually made negative in order to place the underline below the baseline.
        /// </summary>
        public UInt16 underlinePosition;
        /// <summary>
        /// The suggested underline thickness value of the font face in font design units.
        /// </summary>
        public UInt16 underlineThickness;
        /// <summary>
        /// The strikethrough position value of the font face in font design units.
        /// Strikethrough position is the position of strikethrough relative to the English baseline.
        /// The value is usually made positive in order to place the strikethrough above the baseline.
        /// </summary>
        public UInt16 strikethroughPosition;
        /// <summary>
        /// The suggested strikethrough thickness value of the font face in font design units.
        /// </summary>
        public UInt16 strikethroughThickness;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_GLYPH_METRICS
    {
        /// <summary>
        /// Specifies the X offset from the glyph origin to the left edge of the black box.
        /// The glyph origin is the current horizontal writing position.
        /// A negative value means the black box extends to the left of the origin (often true for lowercase italic 'f').
        /// </summary>
        public int leftSideBearing;
        /// <summary>
        /// Specifies the X offset from the origin of the current glyph to the origin of the next glyph when writing horizontally.
        /// </summary>
        public int advanceWidth;
        /// <summary>
        /// Specifies the X offset from the right edge of the black box to the origin of the next glyph when writing horizontally.
        /// The value is negative when the right edge of the black box overhangs the layout box.
        /// </summary>
        public int rightSideBearing;
        /// <summary>
        /// Specifies the vertical offset from the vertical origin to the top of the black box.
        /// Thus, a positive value adds whitespace whereas a negative value means the glyph overhangs the top of the layout box.
        /// </summary>
        public int topSideBearing;
        /// <summary>
        /// Specifies the Y offset from the vertical origin of the current glyph to the vertical origin of the next glyph when writing vertically.
        /// (Note that the term "origin" by itself denotes the horizontal origin. The vertical origin is different.
        /// Its Y coordinate is specified by verticalOriginY value,
        /// and its X coordinate is half the advanceWidth to the right of the horizontal origin).
        /// </summary>
        public int advanceHeight;
        /// <summary>
        /// Specifies the vertical distance from the black box's bottom edge to the advance height.
        /// Positive when the bottom edge of the black box is within the layout box.
        /// Negative when the bottom edge of black box overhangs the layout box.
        /// </summary>
        public int bottomSideBearing;
        /// <summary>
        /// Specifies the Y coordinate of a glyph's vertical origin, in the font's design coordinate system.
        /// The y coordinate of a glyph's vertical origin is the sum of the glyph's top side bearing
        /// and the top (i.e. yMax) of the glyph's bounding box.
        /// </summary>
        public int verticalOriginY;
    }

    public enum DWRITE_TEXT_ALIGNMENT
    {
        /// <summary>
        /// The leading edge of the paragraph text is aligned to the layout box's leading edge.
        /// </summary>
        DWRITE_TEXT_ALIGNMENT_LEADING,
        /// <summary>
        /// The trailing edge of the paragraph text is aligned to the layout box's trailing edge.
        /// </summary>
        DWRITE_TEXT_ALIGNMENT_TRAILING,
        /// <summary>
        /// The center of the paragraph text is aligned to the center of the layout box.
        /// </summary>
        DWRITE_TEXT_ALIGNMENT_CENTER,
        /// <summary>
        /// Align text to the leading side, and also justify text to fill the lines.
        /// </summary>
        DWRITE_TEXT_ALIGNMENT_JUSTIFIED
    }

    public enum DWRITE_PARAGRAPH_ALIGNMENT
    {
        /// <summary>
        /// The first line of paragraph is aligned to the flow's beginning edge of the layout box.
        /// </summary>
        DWRITE_PARAGRAPH_ALIGNMENT_NEAR,
        /// <summary>
        /// The last line of paragraph is aligned to the flow's ending edge of the layout box.
        /// </summary>
        DWRITE_PARAGRAPH_ALIGNMENT_FAR,
        /// <summary>
        /// The center of the paragraph is aligned to the center of the flow of the layout box.
        /// </summary>
        DWRITE_PARAGRAPH_ALIGNMENT_CENTER
    }

    public enum DWRITE_WORD_WRAPPING
    {
        /// <summary>
        /// Words are broken across lines to avoid text overflowing the layout box.
        /// </summary>
        DWRITE_WORD_WRAPPING_WRAP = 0,
        /// <summary>
        /// Words are kept within the same line even when it overflows the layout box.
        /// This option is often used with scrolling to reveal overflow text. 
        /// </summary>
        DWRITE_WORD_WRAPPING_NO_WRAP = 1,
        /// <summary>
        /// Words are broken across lines to avoid text overflowing the layout box.
        /// Emergency wrapping occurs if the word is larger than the maximum width.
        /// </summary>
        DWRITE_WORD_WRAPPING_EMERGENCY_BREAK = 2,
        /// <summary>
        /// Only wrap whole words, never breaking words (emergency wrapping) when the
        /// layout width is too small for even a single word.
        /// </summary>
        DWRITE_WORD_WRAPPING_WHOLE_WORD = 3,
        /// <summary>
        /// Wrap between any valid characters clusters.
        /// </summary>
        DWRITE_WORD_WRAPPING_CHARACTER = 4,
    }

    public enum DWRITE_LINE_SPACING_METHOD
    {
        /// <summary>
        /// Line spacing depends solely on the content, growing to accommodate the size of fonts and inline objects.
        /// </summary>
        DWRITE_LINE_SPACING_METHOD_DEFAULT,
        /// <summary>
        /// Lines are explicitly set to uniform spacing, regardless of contained font sizes.
        /// This can be useful to avoid the uneven appearance that can occur from font fallback.
        /// </summary>
        DWRITE_LINE_SPACING_METHOD_UNIFORM,
        /// <summary>
        /// Line spacing and baseline distances are proportional to the computed values based on the content, the size of the fonts and inline objects.
        /// </summary>
        DWRITE_LINE_SPACING_METHOD_PROPORTIONAL
    }

    public enum DWRITE_READING_DIRECTION
    {
        /// <summary>
        /// Reading progresses from left to right.
        /// </summary>
        DWRITE_READING_DIRECTION_LEFT_TO_RIGHT = 0,
        /// <summary>
        /// Reading progresses from right to left.
        /// </summary>
        DWRITE_READING_DIRECTION_RIGHT_TO_LEFT = 1,
        /// <summary>
        /// Reading progresses from top to bottom.
        /// </summary>
        DWRITE_READING_DIRECTION_TOP_TO_BOTTOM = 2,
        /// <summary>
        /// Reading progresses from bottom to top.
        /// </summary>
        DWRITE_READING_DIRECTION_BOTTOM_TO_TOP = 3,
    }

    public enum DWRITE_FLOW_DIRECTION
    {
        /// <summary>
        /// Text lines are placed from top to bottom.
        /// </summary>
        DWRITE_FLOW_DIRECTION_TOP_TO_BOTTOM = 0,
        /// <summary>
        /// Text lines are placed from bottom to top.
        /// </summary>
        DWRITE_FLOW_DIRECTION_BOTTOM_TO_TOP = 1,
        /// <summary>
        /// Text lines are placed from left to right.
        /// </summary>
        DWRITE_FLOW_DIRECTION_LEFT_TO_RIGHT = 2,
        /// <summary>
        /// Text lines are placed from right to left.
        /// </summary>
        DWRITE_FLOW_DIRECTION_RIGHT_TO_LEFT = 3,
    }

    public enum DWRITE_TRIMMING_GRANULARITY
    {
        /// <summary>
        /// No trimming occurs. Text flows beyond the layout width.
        /// </summary>
        DWRITE_TRIMMING_GRANULARITY_NONE,
        /// <summary>
        /// Trimming occurs at character cluster boundary.
        /// </summary>
        DWRITE_TRIMMING_GRANULARITY_CHARACTER,
        /// <summary>
        /// Trimming occurs at word boundary.
        /// </summary>
        DWRITE_TRIMMING_GRANULARITY_WORD
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_TRIMMING
    {
        /// <summary>
        /// Text granularity of which trimming applies.
        /// </summary>
        public DWRITE_TRIMMING_GRANULARITY granularity;
        /// <summary>
        /// Character code used as the delimiter signaling the beginning of the portion of text to be preserved,
        /// most useful for path ellipsis, where the delimiter would be a slash. Leave this zero if there is no
        /// delimiter.
        /// </summary>
        public uint delimiter;
        /// <summary>
        /// How many occurrences of the delimiter to step back. Leave this zero if there is no delimiter.
        /// </summary>
        public uint delimiterCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_INLINE_OBJECT_METRICS
    {
        /// <summary>
        /// Width of the inline object.
        /// </summary>
        public float width;
        /// <summary>
        /// Height of the inline object as measured from top to bottom.
        /// </summary>
        public float height;
        /// <summary>
        /// Distance from the top of the object to the baseline where it is lined up with the adjacent text.
        /// If the baseline is at the bottom, baseline simply equals height.
        /// </summary>
        public float baseline;
        /// <summary>
        /// Flag indicating whether the object is to be placed upright or alongside the text baseline
        /// for vertical text.
        /// </summary>
        public bool supportsSideways;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_OVERHANG_METRICS
    {
        /// <summary>
        /// The distance from the left-most visible DIP to its left alignment edge.
        /// </summary>
        public float left;
        /// <summary>
        /// The distance from the top-most visible DIP to its top alignment edge.
        /// </summary>
        public float top;
        /// <summary>
        /// The distance from the right-most visible DIP to its right alignment edge.
        /// </summary>
        public float right;
        /// <summary>
        /// The distance from the bottom-most visible DIP to its bottom alignment edge.
        /// </summary>
        public float bottom;
    }

    public enum DWRITE_BREAK_CONDITION
    {
        /// <summary>
        /// Whether a break is allowed is determined by the condition of the
        /// neighboring text span or inline object.
        /// </summary>
        DWRITE_BREAK_CONDITION_NEUTRAL,
        /// <summary>
        /// A break is allowed, unless overruled by the condition of the
        /// neighboring text span or inline object, either prohibited by a
        /// May Not or forced by a Must.
        /// </summary>
        DWRITE_BREAK_CONDITION_CAN_BREAK,
        /// <summary>
        /// There should be no break, unless overruled by a Must condition from
        /// the neighboring text span or inline object.
        /// </summary>
        DWRITE_BREAK_CONDITION_MAY_NOT_BREAK,
        /// <summary>
        /// The break must happen, regardless of the condition of the adjacent
        /// text span or inline object.
        /// </summary>
        DWRITE_BREAK_CONDITION_MUST_BREAK
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_GLYPH_OFFSET
    {
        /// <summary>
        /// Offset in the advance direction of the run. A positive advance offset moves the glyph to the right
        /// (in pre-transform coordinates) if the run is left-to-right or to the left if the run is right-to-left.
        /// </summary>
        public float advanceOffset;
        /// <summary>
        /// Offset in the ascent direction, i.e., the direction ascenders point. A positive ascender offset moves
        /// the glyph up (in pre-transform coordinates).
        /// </summary>
        public float ascenderOffset;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_GLYPH_RUN
    {
        /// <summary>
        /// The physical font face to draw with.
        /// </summary>
        public IntPtr fontFace;
        /// <summary>
        /// Logical size of the font in DIPs, not points (equals 1/96 inch).
        /// </summary>
        public float fontEmSize;
        /// <summary>
        /// The number of glyphs.
        /// </summary>
        public uint glyphCount;
        /// <summary>
        /// The indices to render.
        /// </summary>
        public IntPtr glyphIndices;
        /// <summary>
        /// Glyph advance widths.
        /// </summary>
        public IntPtr glyphAdvances;
        /// <summary>
        /// Glyph offsets.
        /// </summary>
        public IntPtr glyphOffsets;
        /// <summary>
        /// If true, specifies that glyphs are rotated 90 degrees to the left and vertical metrics are used. Vertical writing is achieved by specifying isSideways = true and rotating the entire run 90 degrees to the right via a rotate transform.
        /// </summary>
        public bool isSideways;
        /// <summary>
        /// The implicit resolved bidi level of the run. Odd levels indicate right-to-left languages like Hebrew and Arabic, while even levels indicate left-to-right languages like English and Japanese (when written horizontally). For right-to-left languages, the text origin is on the right, and text should be drawn to the left.
        /// </summary>
        public uint bidiLevel;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_GLYPH_RUN_DESCRIPTION
    {
        /// <summary>
        /// The locale name associated with this run.
        /// </summary>
        public string localeName;
        /// <summary>
        /// The text associated with the glyphs.
        /// </summary>
        public string str;
        /// <summary>
        /// The number of characters (UTF16 code-units).
        /// Note that this may be different than the number of glyphs.
        /// </summary>
        public uint stringLength;

        public UInt16 clusterMap;

        /// <summary>
        /// Corresponding text position in the original string
        /// this glyph run came from.
        /// </summary>
        public uint textPosition;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_UNDERLINE
    {
        /// <summary>
        /// Width of the underline, measured parallel to the baseline.
        /// </summary>
        public float width;
        /// <summary>
        /// Thickness of the underline, measured perpendicular to the
        /// baseline.
        /// </summary>
        public float thickness;
        /// <summary>
        /// Offset of the underline from the baseline.
        /// A positive offset represents a position below the baseline and
        /// a negative offset is above.
        /// </summary>
        public float offset;
        /// <summary>
        /// Height of the tallest run where the underline applies.
        /// </summary>
        public float runHeight;
        /// <summary>
        /// Reading direction of the text associated with the underline.  This 
        /// value is used to interpret whether the width value runs horizontally 
        /// or vertically.
        /// </summary>
        public DWRITE_READING_DIRECTION readingDirection;
        /// <summary>
        /// Flow direction of the text associated with the underline.  This value
        /// is used to interpret whether the thickness value advances top to 
        /// bottom, left to right, or right to left.
        /// </summary>
        public DWRITE_FLOW_DIRECTION flowDirection;
        /// <summary>
        /// Locale of the text the underline is being drawn under. Can be
        /// pertinent where the locale affects how the underline is drawn.
        /// For example, in vertical text, the underline belongs on the
        /// left for Chinese but on the right for Japanese.
        /// This choice is completely left up to higher levels.
        /// </summary>
        public string localeName;
        /// <summary>
        /// The measuring mode can be useful to the renderer to determine how
        /// underlines are rendered, e.g. rounding the thickness to a whole pixel
        /// in GDI-compatible modes.
        /// </summary>
        public DWRITE_MEASURING_MODE measuringMode;
    }

    public enum DWRITE_MEASURING_MODE
    {
        //
        // Text is measured using glyph ideal metrics whose values are independent to the current display resolution.
        //
        DWRITE_MEASURING_MODE_NATURAL,
        //
        // Text is measured using glyph display compatible metrics whose values tuned for the current display resolution.
        //
        DWRITE_MEASURING_MODE_GDI_CLASSIC,
        //
        // Text is measured using the same glyph display metrics as text measured by GDI using a font
        // created with CLEARTYPE_NATURAL_QUALITY.
        //
        DWRITE_MEASURING_MODE_GDI_NATURAL
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_STRIKETHROUGH
    {
        /// <summary>
        /// Width of the strikethrough, measured parallel to the baseline.
        /// </summary>
        public float width;
        /// <summary>
        /// Thickness of the strikethrough, measured perpendicular to the
        /// baseline.
        /// </summary>
        public float thickness;
        /// <summary>
        /// Offset of the strikethrough from the baseline.
        /// A positive offset represents a position below the baseline and
        /// a negative offset is above.
        /// </summary>
        public float offset;
        /// <summary>
        /// Reading direction of the text associated with the strikethrough.  This
        /// value is used to interpret whether the width value runs horizontally 
        /// or vertically.
        /// </summary>
        public DWRITE_READING_DIRECTION readingDirection;
        /// <summary>
        /// Flow direction of the text associated with the strikethrough.  This 
        /// value is used to interpret whether the thickness value advances top to
        /// bottom, left to right, or right to left.
        /// </summary>
        public DWRITE_FLOW_DIRECTION flowDirection;
        /// <summary>
        /// Locale of the range. Can be pertinent where the locale affects the style.
        /// </summary>
        public string localeName;
        /// <summary>
        /// The measuring mode can be useful to the renderer to determine how
        /// underlines are rendered, e.g. rounding the thickness to a whole pixel
        /// in GDI-compatible modes.
        /// </summary>
        public DWRITE_MEASURING_MODE measuringMode;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_MATRIX
    {
        /// <summary>
        /// Horizontal scaling / cosine of rotation
        /// </summary>
        public float m11;
        /// <summary>
        /// Vertical shear / sine of rotation
        /// </summary>
        public float m12;
        /// <summary>
        /// Horizontal shear / negative sine of rotation
        /// </summary>
        public float m21;
        /// <summary>
        /// Vertical scaling / cosine of rotation
        /// </summary>
        public float m22;
        /// <summary>
        /// Horizontal shift (always orthogonal regardless of rotation)
        /// </summary>
        public float dx;
        /// <summary>
        /// Vertical shift (always orthogonal regardless of rotation)
        /// </summary>
        public float dy;
    }

    public enum DWRITE_FONT_WEIGHT
    {
        /// <summary>
        /// Predefined font weight : Thin (100).
        /// </summary>
        DWRITE_FONT_WEIGHT_THIN = 100,
        /// <summary>
        /// Predefined font weight : Extra-light (200).
        /// </summary>
        DWRITE_FONT_WEIGHT_EXTRA_LIGHT = 200,
        /// <summary>
        /// Predefined font weight : Ultra-light (200).
        /// </summary>
        DWRITE_FONT_WEIGHT_ULTRA_LIGHT = 200,
        /// <summary>
        /// Predefined font weight : Light (300).
        /// </summary>
        DWRITE_FONT_WEIGHT_LIGHT = 300,
        /// <summary>
        /// Predefined font weight : Semi-light (350).
        /// </summary>
        DWRITE_FONT_WEIGHT_SEMI_LIGHT = 350,
        /// <summary>
        /// Predefined font weight : Normal (400).
        /// </summary>
        DWRITE_FONT_WEIGHT_NORMAL = 400,
        /// <summary>
        /// Predefined font weight : Regular (400).
        /// </summary>
        DWRITE_FONT_WEIGHT_REGULAR = 400,
        /// <summary>
        /// Predefined font weight : Medium (500).
        /// </summary>
        DWRITE_FONT_WEIGHT_MEDIUM = 500,
        /// <summary>
        /// Predefined font weight : Demi-bold (600).
        /// </summary>
        DWRITE_FONT_WEIGHT_DEMI_BOLD = 600,
        /// <summary>
        /// Predefined font weight : Semi-bold (600).
        /// </summary>
        DWRITE_FONT_WEIGHT_SEMI_BOLD = 600,
        /// <summary>
        /// Predefined font weight : Bold (700).
        /// </summary>
        DWRITE_FONT_WEIGHT_BOLD = 700,
        /// <summary>
        /// Predefined font weight : Extra-bold (800).
        /// </summary>
        DWRITE_FONT_WEIGHT_EXTRA_BOLD = 800,
        /// <summary>
        /// Predefined font weight : Ultra-bold (800).
        /// </summary>
        DWRITE_FONT_WEIGHT_ULTRA_BOLD = 800,
        /// <summary>
        /// Predefined font weight : Black (900).
        /// </summary>
        DWRITE_FONT_WEIGHT_BLACK = 900,
        /// <summary>
        /// Predefined font weight : Heavy (900).
        /// </summary>
        DWRITE_FONT_WEIGHT_HEAVY = 900,
        /// <summary>
        /// Predefined font weight : Extra-black (950).
        /// </summary>
        DWRITE_FONT_WEIGHT_EXTRA_BLACK = 950,
        /// <summary>
        /// Predefined font weight : Ultra-black (950).
        /// </summary>
        DWRITE_FONT_WEIGHT_ULTRA_BLACK = 950
    }

    /// <summary>
    /// The font stretch enumeration describes relative change from the normal aspect ratio
    /// as specified by a font designer for the glyphs in a font.
    /// Values less than 1 or greater than 9 are considered to be invalid, and they are rejected by font API functions.
    /// </summary>
    public enum DWRITE_FONT_STRETCH
    {
        /// <summary>
        /// Predefined font stretch : Not known (0).
        /// </summary>
        DWRITE_FONT_STRETCH_UNDEFINED = 0,
        /// <summary>
        /// Predefined font stretch : Ultra-condensed (1).
        /// </summary>
        DWRITE_FONT_STRETCH_ULTRA_CONDENSED = 1,
        /// <summary>
        /// Predefined font stretch : Extra-condensed (2).
        /// </summary>
        DWRITE_FONT_STRETCH_EXTRA_CONDENSED = 2,
        /// <summary>
        /// Predefined font stretch : Condensed (3).
        /// </summary>
        DWRITE_FONT_STRETCH_CONDENSED = 3,
        /// <summary>
        /// Predefined font stretch : Semi-condensed (4).
        /// </summary>
        DWRITE_FONT_STRETCH_SEMI_CONDENSED = 4,
        /// <summary>
        /// Predefined font stretch : Normal (5).
        /// </summary>
        DWRITE_FONT_STRETCH_NORMAL = 5,
        /// <summary>
        /// Predefined font stretch : Medium (5).
        /// </summary>
        DWRITE_FONT_STRETCH_MEDIUM = 5,
        /// <summary>
        /// Predefined font stretch : Semi-expanded (6).
        /// </summary>
        DWRITE_FONT_STRETCH_SEMI_EXPANDED = 6,
        /// <summary>
        /// Predefined font stretch : Expanded (7).
        /// </summary>
        DWRITE_FONT_STRETCH_EXPANDED = 7,
        /// <summary>
        /// Predefined font stretch : Extra-expanded (8).
        /// </summary>
        DWRITE_FONT_STRETCH_EXTRA_EXPANDED = 8,
        /// <summary>
        /// Predefined font stretch : Ultra-expanded (9).
        /// </summary>
        DWRITE_FONT_STRETCH_ULTRA_EXPANDED = 9
    }

    public enum DWRITE_FONT_STYLE
    {
        /// <summary>
        /// Font slope style : Normal.
        /// </summary>
        DWRITE_FONT_STYLE_NORMAL,
        /// <summary>
        /// Font slope style : Oblique.
        /// </summary>
        DWRITE_FONT_STYLE_OBLIQUE,
        /// <summary>
        /// Font slope style : Italic.
        /// </summary>
        DWRITE_FONT_STYLE_ITALIC
    }

    public enum DWRITE_FONT_FEATURE_TAG
    {
        DWRITE_FONT_FEATURE_TAG_ALTERNATIVE_FRACTIONS = 0x63726661, // 'afrc'
        DWRITE_FONT_FEATURE_TAG_PETITE_CAPITALS_FROM_CAPITALS = 0x63703263, // 'c2pc'
        DWRITE_FONT_FEATURE_TAG_SMALL_CAPITALS_FROM_CAPITALS = 0x63733263, // 'c2sc'
        DWRITE_FONT_FEATURE_TAG_CONTEXTUAL_ALTERNATES = 0x746c6163, // 'calt'
        DWRITE_FONT_FEATURE_TAG_CASE_SENSITIVE_FORMS = 0x65736163, // 'case'
        DWRITE_FONT_FEATURE_TAG_GLYPH_COMPOSITION_DECOMPOSITION = 0x706d6363, // 'ccmp'
        DWRITE_FONT_FEATURE_TAG_CONTEXTUAL_LIGATURES = 0x67696c63, // 'clig'
        DWRITE_FONT_FEATURE_TAG_CAPITAL_SPACING = 0x70737063, // 'cpsp'
        DWRITE_FONT_FEATURE_TAG_CONTEXTUAL_SWASH = 0x68777363, // 'cswh'
        DWRITE_FONT_FEATURE_TAG_CURSIVE_POSITIONING = 0x73727563, // 'curs'
        DWRITE_FONT_FEATURE_TAG_DEFAULT = 0x746c6664, // 'dflt'
        DWRITE_FONT_FEATURE_TAG_DISCRETIONARY_LIGATURES = 0x67696c64, // 'dlig'
        DWRITE_FONT_FEATURE_TAG_EXPERT_FORMS = 0x74707865, // 'expt'
        DWRITE_FONT_FEATURE_TAG_FRACTIONS = 0x63617266, // 'frac'
        DWRITE_FONT_FEATURE_TAG_FULL_WIDTH = 0x64697766, // 'fwid'
        DWRITE_FONT_FEATURE_TAG_HALF_FORMS = 0x666c6168, // 'half'
        DWRITE_FONT_FEATURE_TAG_HALANT_FORMS = 0x6e6c6168, // 'haln'
        DWRITE_FONT_FEATURE_TAG_ALTERNATE_HALF_WIDTH = 0x746c6168, // 'halt'
        DWRITE_FONT_FEATURE_TAG_HISTORICAL_FORMS = 0x74736968, // 'hist'
        DWRITE_FONT_FEATURE_TAG_HORIZONTAL_KANA_ALTERNATES = 0x616e6b68, // 'hkna'
        DWRITE_FONT_FEATURE_TAG_HISTORICAL_LIGATURES = 0x67696c68, // 'hlig'
        DWRITE_FONT_FEATURE_TAG_HALF_WIDTH = 0x64697768, // 'hwid'
        DWRITE_FONT_FEATURE_TAG_HOJO_KANJI_FORMS = 0x6f6a6f68, // 'hojo'
        DWRITE_FONT_FEATURE_TAG_JIS04_FORMS = 0x3430706a, // 'jp04'
        DWRITE_FONT_FEATURE_TAG_JIS78_FORMS = 0x3837706a, // 'jp78'
        DWRITE_FONT_FEATURE_TAG_JIS83_FORMS = 0x3338706a, // 'jp83'
        DWRITE_FONT_FEATURE_TAG_JIS90_FORMS = 0x3039706a, // 'jp90'
        DWRITE_FONT_FEATURE_TAG_KERNING = 0x6e72656b, // 'kern'
        DWRITE_FONT_FEATURE_TAG_STANDARD_LIGATURES = 0x6167696c, // 'liga'
        DWRITE_FONT_FEATURE_TAG_LINING_FIGURES = 0x6d756e6c, // 'lnum'
        DWRITE_FONT_FEATURE_TAG_LOCALIZED_FORMS = 0x6c636f6c, // 'locl'
        DWRITE_FONT_FEATURE_TAG_MARK_POSITIONING = 0x6b72616d, // 'mark'
        DWRITE_FONT_FEATURE_TAG_MATHEMATICAL_GREEK = 0x6b72676d, // 'mgrk'
        DWRITE_FONT_FEATURE_TAG_MARK_TO_MARK_POSITIONING = 0x6b6d6b6d, // 'mkmk'
        DWRITE_FONT_FEATURE_TAG_ALTERNATE_ANNOTATION_FORMS = 0x746c616e, // 'nalt'
        DWRITE_FONT_FEATURE_TAG_NLC_KANJI_FORMS = 0x6b636c6e, // 'nlck'
        DWRITE_FONT_FEATURE_TAG_OLD_STYLE_FIGURES = 0x6d756e6f, // 'onum'
        DWRITE_FONT_FEATURE_TAG_ORDINALS = 0x6e64726f, // 'ordn'
        DWRITE_FONT_FEATURE_TAG_PROPORTIONAL_ALTERNATE_WIDTH = 0x746c6170, // 'palt'
        DWRITE_FONT_FEATURE_TAG_PETITE_CAPITALS = 0x70616370, // 'pcap'
        DWRITE_FONT_FEATURE_TAG_PROPORTIONAL_FIGURES = 0x6d756e70, // 'pnum'
        DWRITE_FONT_FEATURE_TAG_PROPORTIONAL_WIDTHS = 0x64697770, // 'pwid'
        DWRITE_FONT_FEATURE_TAG_QUARTER_WIDTHS = 0x64697771, // 'qwid'
        DWRITE_FONT_FEATURE_TAG_REQUIRED_LIGATURES = 0x67696c72, // 'rlig'
        DWRITE_FONT_FEATURE_TAG_RUBY_NOTATION_FORMS = 0x79627572, // 'ruby'
        DWRITE_FONT_FEATURE_TAG_STYLISTIC_ALTERNATES = 0x746c6173, // 'salt'
        DWRITE_FONT_FEATURE_TAG_SCIENTIFIC_INFERIORS = 0x666e6973, // 'sinf'
        DWRITE_FONT_FEATURE_TAG_SMALL_CAPITALS = 0x70636d73, // 'smcp'
        DWRITE_FONT_FEATURE_TAG_SIMPLIFIED_FORMS = 0x6c706d73, // 'smpl'
        DWRITE_FONT_FEATURE_TAG_STYLISTIC_SET_1 = 0x31307373, // 'ss01'
        DWRITE_FONT_FEATURE_TAG_STYLISTIC_SET_2 = 0x32307373, // 'ss02'
        DWRITE_FONT_FEATURE_TAG_STYLISTIC_SET_3 = 0x33307373, // 'ss03'
        DWRITE_FONT_FEATURE_TAG_STYLISTIC_SET_4 = 0x34307373, // 'ss04'
        DWRITE_FONT_FEATURE_TAG_STYLISTIC_SET_5 = 0x35307373, // 'ss05'
        DWRITE_FONT_FEATURE_TAG_STYLISTIC_SET_6 = 0x36307373, // 'ss06'
        DWRITE_FONT_FEATURE_TAG_STYLISTIC_SET_7 = 0x37307373, // 'ss07'
        DWRITE_FONT_FEATURE_TAG_STYLISTIC_SET_8 = 0x38307373, // 'ss08'
        DWRITE_FONT_FEATURE_TAG_STYLISTIC_SET_9 = 0x39307373, // 'ss09'
        DWRITE_FONT_FEATURE_TAG_STYLISTIC_SET_10 = 0x30317373, // 'ss10'
        DWRITE_FONT_FEATURE_TAG_STYLISTIC_SET_11 = 0x31317373, // 'ss11'
        DWRITE_FONT_FEATURE_TAG_STYLISTIC_SET_12 = 0x32317373, // 'ss12'
        DWRITE_FONT_FEATURE_TAG_STYLISTIC_SET_13 = 0x33317373, // 'ss13'
        DWRITE_FONT_FEATURE_TAG_STYLISTIC_SET_14 = 0x34317373, // 'ss14'
        DWRITE_FONT_FEATURE_TAG_STYLISTIC_SET_15 = 0x35317373, // 'ss15'
        DWRITE_FONT_FEATURE_TAG_STYLISTIC_SET_16 = 0x36317373, // 'ss16'
        DWRITE_FONT_FEATURE_TAG_STYLISTIC_SET_17 = 0x37317373, // 'ss17'
        DWRITE_FONT_FEATURE_TAG_STYLISTIC_SET_18 = 0x38317373, // 'ss18'
        DWRITE_FONT_FEATURE_TAG_STYLISTIC_SET_19 = 0x39317373, // 'ss19'
        DWRITE_FONT_FEATURE_TAG_STYLISTIC_SET_20 = 0x30327373, // 'ss20'
        DWRITE_FONT_FEATURE_TAG_SUBSCRIPT = 0x73627573, // 'subs'
        DWRITE_FONT_FEATURE_TAG_SUPERSCRIPT = 0x73707573, // 'sups'
        DWRITE_FONT_FEATURE_TAG_SWASH = 0x68737773, // 'swsh'
        DWRITE_FONT_FEATURE_TAG_TITLING = 0x6c746974, // 'titl'
        DWRITE_FONT_FEATURE_TAG_TRADITIONAL_NAME_FORMS = 0x6d616e74, // 'tnam'
        DWRITE_FONT_FEATURE_TAG_TABULAR_FIGURES = 0x6d756e74, // 'tnum'
        DWRITE_FONT_FEATURE_TAG_TRADITIONAL_FORMS = 0x64617274, // 'trad'
        DWRITE_FONT_FEATURE_TAG_THIRD_WIDTHS = 0x64697774, // 'twid'
        DWRITE_FONT_FEATURE_TAG_UNICASE = 0x63696e75, // 'unic'
        DWRITE_FONT_FEATURE_TAG_VERTICAL_WRITING = 0x74726576, // 'vert'
        DWRITE_FONT_FEATURE_TAG_VERTICAL_ALTERNATES_AND_ROTATION = 0x32747276, // 'vrt2'
        DWRITE_FONT_FEATURE_TAG_SLASHED_ZERO = 0x6f72657a, // 'zero'
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_FONT_FEATURE
    {
        /// <summary>
        /// The feature OpenType name identifier.
        /// </summary>
        public DWRITE_FONT_FEATURE_TAG nameTag;
        /// <summary>
        /// Execution parameter of the feature.
        /// </summary>
        /// <remarks>
        /// The parameter should be non-zero to enable the feature.  Once enabled, a feature can't be disabled again within
        /// the same range.  Features requiring a selector use this value to indicate the selector index. 
        /// </remarks>
        public uint parameter;

        public DWRITE_FONT_FEATURE(DWRITE_FONT_FEATURE_TAG nameTag, uint parameter)
        {
            this.nameTag = nameTag;
            this.parameter = parameter;
        }
    }

    public enum DWRITE_NUMBER_SUBSTITUTION_METHOD
    {
        /// <summary>
        /// Specifies that the substitution method should be determined based
        /// on LOCALE_IDIGITSUBSTITUTION value of the specified text culture.
        /// </summary>
        DWRITE_NUMBER_SUBSTITUTION_METHOD_FROM_CULTURE,
        /// <summary>
        /// If the culture is Arabic or Farsi, specifies that the number shape
        /// depend on the context. Either traditional or nominal number shape
        /// are used depending on the nearest preceding strong character or (if
        /// there is none) the reading direction of the paragraph.
        /// </summary>
        DWRITE_NUMBER_SUBSTITUTION_METHOD_CONTEXTUAL,
        /// <summary>
        /// Specifies that code points 0x30-0x39 are always rendered as nominal numeral 
        /// shapes (ones of the European number), i.e., no substitution is performed.
        /// </summary>
        DWRITE_NUMBER_SUBSTITUTION_METHOD_NONE,
        /// <summary>
        /// Specifies that number are rendered using the national number shape 
        /// as specified by the LOCALE_SNATIVEDIGITS value of the specified text culture.
        /// </summary>
        DWRITE_NUMBER_SUBSTITUTION_METHOD_NATIONAL,
        /// <summary>
        /// Specifies that number are rendered using the traditional shape
        /// for the specified culture. For most cultures, this is the same as
        /// NativeNational. However, NativeNational results in Latin number
        /// for some Arabic cultures, whereas this value results in Arabic
        /// number for all Arabic cultures.
        /// </summary>
        DWRITE_NUMBER_SUBSTITUTION_METHOD_TRADITIONAL
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_TEXT_RANGE
    {
        /// <summary>
        ///         ''' The start text position of the range.
        ///         ''' </summary>
        public uint startPosition;
        /// <summary>
        ///         ''' The number of text positions in the range.
        ///         ''' </summary>
        public uint length;
        public DWRITE_TEXT_RANGE(uint startPosition, uint length)
        {
            this.startPosition = startPosition;
            this.length = length;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_SCRIPT_ANALYSIS
    {
        /// <summary>
        /// Zero-based index representation of writing system script.
        /// </summary>
        public UInt16 script;
        /// <summary>
        /// Additional shaping requirement of text.
        /// </summary>
        public DWRITE_SCRIPT_SHAPES shapes;
    }

    public enum DWRITE_SCRIPT_SHAPES
    {
        /// <summary>
        /// No additional shaping requirement. Text is shaped with the writing system default behavior.
        /// </summary>
        DWRITE_SCRIPT_SHAPES_DEFAULT = 0,
        /// <summary>
        /// Text should leave no visual on display i.e. control or format control characters.
        /// </summary>
        DWRITE_SCRIPT_SHAPES_NO_VISUAL = 1
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_SHAPING_TEXT_PROPERTIES
    {
        /// <summary>
        /// This character can be shaped independently from the others
        /// (usually set for the space character).
        /// </summary>
        [MarshalAs(UnmanagedType.U2, SizeConst = 1)]
        public UInt16 isShapedAlone;
        /// <summary>
        /// Reserved for use by shaping engine.
        /// </summary>
        [MarshalAs(UnmanagedType.U2, SizeConst = 15)]
        public UInt16 reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_SHAPING_GLYPH_PROPERTIES
    {
        /// <summary>
        /// Justification class, whether to use spacing, kashidas, or
        /// another method. This exists for backwards compatibility
        /// with Uniscribe's SCRIPT_JUSTIFY enum.
        /// </summary>
        [MarshalAs(UnmanagedType.U2, SizeConst = 4)]
        public UInt16 justification;
        /// <summary>
        /// Indicates glyph is the first of a cluster.
        /// </summary>
        [MarshalAs(UnmanagedType.U2, SizeConst = 1)]
        public UInt16 isClusterStart1;
        /// <summary>
        /// Glyph is a diacritic.
        /// </summary>
        [MarshalAs(UnmanagedType.U2, SizeConst = 1)]
        public UInt16 isDiacritic;
        /// <summary>
        /// Glyph has no width, blank, ZWJ, ZWNJ etc.
        /// </summary>
        [MarshalAs(UnmanagedType.U2, SizeConst = 1)]
        public UInt16 isZeroWidthSpace;
        /// <summary>
        /// Reserved for use by shaping engine.
        /// </summary>
        [MarshalAs(UnmanagedType.U2, SizeConst = 9)]
        public UInt16 reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_LINE_METRICS
    {
        /// <summary>
        /// The number of total text positions in the line.
        /// This includes any trailing whitespace and newline characters.
        /// </summary>
        public int length;
        /// <summary>
        /// The number of whitespace positions at the end of the line.  Newline
        /// sequences are considered whitespace.
        /// </summary>
        public int trailingWhitespaceLength;
        /// <summary>
        /// The number of characters in the newline sequence at the end of the line.
        /// If the count is zero, then the line was either wrapped or it is the
        /// end of the text.
        /// </summary>
        public int newlineLength;
        /// <summary>
        /// Height of the line as measured from top to bottom.
        /// </summary>
        public float height;
        /// <summary>
        /// Distance from the top of the line to its baseline.
        /// </summary>
        public float baseline;
        /// <summary>
        /// The line is trimmed.
        /// </summary>
        public bool isTrimmed;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_TEXT_METRICS
    {
        /// <summary>
        /// Left-most point of formatted text relative to layout box
        /// (excluding any glyph overhang).
        /// </summary>
        public float left;
        /// <summary>
        /// Top-most point of formatted text relative to layout box
        /// (excluding any glyph overhang).
        /// </summary>
        public float top;
        /// <summary>
        /// The width of the formatted text ignoring trailing whitespace
        /// at the end of each line.
        /// </summary>
        public float width;
        /// <summary>
        /// The width of the formatted text taking into account the
        /// trailing whitespace at the end of each line.
        /// </summary>
        public float widthIncludingTrailingWhitespace;
        /// <summary>
        /// The height of the formatted text. The height of an empty string
        /// is determined by the size of the default font's line height.
        /// </summary>
        public float height;
        /// <summary>
        /// Initial width given to the layout. Depending on whether the text
        /// was wrapped or not, it can be either larger or smaller than the
        /// text content width.
        /// </summary>
        public float layoutWidth;
        /// <summary>
        /// Initial height given to the layout. Depending on the length of the
        /// text, it may be larger or smaller than the text content height.
        /// </summary>
        public float layoutHeight;
        /// <summary>
        /// The maximum reordering count of any line of text, used
        /// to calculate the most number of hit-testing boxes needed.
        /// If the layout has no bidirectional text or no text at all,
        /// the minimum level is 1.
        /// </summary>
        public int maxBidiReorderingDepth;
        /// <summary>
        /// Total number of lines.
        /// </summary>
        public int lineCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_CLUSTER_METRICS
    {
        /// <summary>
        /// The total advance width of all glyphs in the cluster.
        /// </summary>
        public float width;
        /// <summary>
        /// The number of text positions in the cluster.
        /// </summary>
        public UInt16 length;
        /// <summary>
        /// Indicate whether line can be broken right after the cluster.
        /// </summary>
        [MarshalAs(UnmanagedType.U2, SizeConst = 1)]
        public UInt16 canWrapLineAfter;
        /// <summary>
        /// Indicate whether the cluster corresponds to whitespace character.
        /// </summary>
        [MarshalAs(UnmanagedType.U2, SizeConst = 1)]
        public UInt16 isWhitespace;
        /// <summary>
        /// Indicate whether the cluster corresponds to a newline character.
        /// </summary>
        [MarshalAs(UnmanagedType.U2, SizeConst = 1)]
        public UInt16 isNewline;
        /// <summary>
        /// Indicate whether the cluster corresponds to soft hyphen character.
        /// </summary>
        [MarshalAs(UnmanagedType.U2, SizeConst = 1)]
        public UInt16 isSoftHyphen;
        /// <summary>
        /// Indicate whether the cluster is read from right to left.
        /// </summary>
        public UInt16 isRightToLeft;
        [MarshalAs(UnmanagedType.U2, SizeConst = 11)]
        public UInt16 padding;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_HIT_TEST_METRICS
    {
        /// <summary>
        /// First text position within the geometry.
        /// </summary>
        public int textPosition;
        /// <summary>
        /// Number of text positions within the geometry.
        /// </summary>
        public int length;
        /// <summary>
        /// Left position of the top-left coordinate of the geometry.
        /// </summary>
        public float left;
        /// <summary>
        /// Top position of the top-left coordinate of the geometry.
        /// </summary>
        public float top;
        /// <summary>
        /// Geometry's width.
        /// </summary>
        public float width;
        /// <summary>
        /// Geometry's height.
        /// </summary>
        public float height;
        /// <summary>
        /// Bidi level of text positions enclosed within the geometry.
        /// </summary>
        public int bidiLevel;
        /// <summary>
        /// Geometry encloses text?
        /// </summary>
        public bool isText;
        /// <summary>
        /// Range is trimmed.
        /// </summary>
        public bool isTrimmed;
    }

    public enum DWRITE_INFORMATIONAL_STRING_ID
    {
        /// <summary>
        /// Unspecified name ID.
        /// </summary>
        DWRITE_INFORMATIONAL_STRING_NONE,
        /// <summary>
        /// Copyright notice provided by the font.
        /// </summary>
        DWRITE_INFORMATIONAL_STRING_COPYRIGHT_NOTICE,
        /// <summary>
        /// String containing a version number.
        /// </summary>
        DWRITE_INFORMATIONAL_STRING_VERSION_STRINGS,
        /// <summary>
        /// Trademark information provided by the font.
        /// </summary>
        DWRITE_INFORMATIONAL_STRING_TRADEMARK,
        /// <summary>
        /// Name of the font manufacturer.
        /// </summary>
        DWRITE_INFORMATIONAL_STRING_MANUFACTURER,
        /// <summary>
        /// Name of the font designer.
        /// </summary>
        DWRITE_INFORMATIONAL_STRING_DESIGNER,
        /// <summary>
        /// URL of font designer (with protocol, e.g., http://, ftp://).
        /// </summary>
        DWRITE_INFORMATIONAL_STRING_DESIGNER_URL,
        /// <summary>
        /// Description of the font. Can contain revision information, usage recommendations, history, features, etc.
        /// </summary>
        DWRITE_INFORMATIONAL_STRING_DESCRIPTION,
        /// <summary>
        /// URL of font vendor (with protocol, e.g., http://, ftp://). If a unique serial number is embedded in the URL, it can be used to register the font.
        /// </summary>
        DWRITE_INFORMATIONAL_STRING_FONT_VENDOR_URL,
        /// <summary>
        /// Description of how the font may be legally used, or different example scenarios for licensed use. This field should be written in plain language, not legalese.
        /// </summary>
        DWRITE_INFORMATIONAL_STRING_LICENSE_DESCRIPTION,
        /// <summary>
        /// URL where additional licensing information can be found.
        /// </summary>
        DWRITE_INFORMATIONAL_STRING_LICENSE_INFO_URL,
        /// <summary>
        /// GDI-compatible family name. Because GDI allows a maximum of four fonts per family, fonts in the same family may have different GDI-compatible family names
        /// (e.g., "Arial", "Arial Narrow", "Arial Black").
        /// </summary>
        DWRITE_INFORMATIONAL_STRING_WIN32_FAMILY_NAMES,
        /// <summary>
        /// GDI-compatible subfamily name.
        /// </summary>
        DWRITE_INFORMATIONAL_STRING_WIN32_SUBFAMILY_NAMES,
        /// <summary>
        /// Family name preferred by the designer. This enables font designers to group more than four fonts in a single family without losing compatibility with
        /// GDI. This name is typically only present if it differs from the GDI-compatible family name.
        /// </summary>
        DWRITE_INFORMATIONAL_STRING_PREFERRED_FAMILY_NAMES,
        /// <summary>
        /// Subfamily name preferred by the designer. This name is typically only present if it differs from the GDI-compatible subfamily name. 
        /// </summary>
        DWRITE_INFORMATIONAL_STRING_PREFERRED_SUBFAMILY_NAMES,
        /// <summary>
        /// Sample text. This can be the font name or any other text that the designer thinks is the best example to display the font in.
        /// </summary>
        DWRITE_INFORMATIONAL_STRING_SAMPLE_TEXT,
        /// <summary>
        /// The full name of the font, e.g. "Arial Bold", from name id 4 in the name table.
        /// </summary>
        DWRITE_INFORMATIONAL_STRING_FULL_NAME,
        /// <summary>
        /// The postscript name of the font, e.g. "GillSans-Bold" from name id 6 in the name table.
        /// </summary>
        DWRITE_INFORMATIONAL_STRING_POSTSCRIPT_NAME,
        /// <summary>
        /// The postscript CID findfont name, from name id 20 in the name table.
        /// </summary>
        DWRITE_INFORMATIONAL_STRING_POSTSCRIPT_CID_NAME,
        /// <summary>
        /// Family name for the weight-width-slope model.
        /// </summary>
        DWRITE_INFORMATIONAL_STRING_WWS_FAMILY_NAME,
        /// <summary>
        /// Script/language tag to identify the scripts or languages that the font was
        /// primarily designed to support. See DWRITE_FONT_PROPERTY_ID_DESIGN_SCRIPT_LANGUAGE_TAG
        /// for a longer description.
        /// </summary>
        DWRITE_INFORMATIONAL_STRING_DESIGN_SCRIPT_LANGUAGE_TAG,
        /// <summary>
        /// Script/language tag to identify the scripts or languages that the font declares
        /// it is able to support.
        /// </summary>
        DWRITE_INFORMATIONAL_STRING_SUPPORTED_SCRIPT_LANGUAGE_TAG,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_LINE_BREAKPOINT
    {
        /// <summary>
        /// Breaking condition before the character.
        /// </summary>
        [MarshalAs(UnmanagedType.U1, SizeConst = 2)]
        public byte breakConditionBefore;
        /// <summary>
        /// Breaking condition after the character.
        /// </summary>
        [MarshalAs(UnmanagedType.U1, SizeConst = 2)]
        public byte breakConditionAfter;
        /// <summary>
        /// The character is some form of whitespace, which may be meaningful
        /// for justification.
        /// </summary>
        [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
        public byte isWhitespace;
        /// <summary>
        /// The character is a soft hyphen, often used to indicate hyphenation
        /// points inside words.
        /// </summary>
        [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
        public byte isSoftHyphen;

        [MarshalAs(UnmanagedType.U1, SizeConst = 2)]
        public byte padding;
    }

    public enum D2D1_FILL_MODE
    {
        D2D1_FILL_MODE_ALTERNATE = 0,
        D2D1_FILL_MODE_WINDING = 1,
        D2D1_FILL_MODE_FORCE_DWORD = unchecked((int)0xffffffff)
    }

    public enum D2D1_PATH_SEGMENT
    {
        D2D1_PATH_SEGMENT_NONE = 0x00000000,
        D2D1_PATH_SEGMENT_FORCE_UNSTROKED = 0x00000001,
        D2D1_PATH_SEGMENT_FORCE_ROUND_LINE_JOIN = 0x00000002,
        D2D1_PATH_SEGMENT_FORCE_DWORD = unchecked((int)0xffffffff)
    }

    public enum D2D1_FIGURE_BEGIN
    {
        D2D1_FIGURE_BEGIN_FILLED = 0,
        D2D1_FIGURE_BEGIN_HOLLOW = 1,
        D2D1_FIGURE_BEGIN_FORCE_DWORD = unchecked((int)0xffffffff)
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct D2D1_BEZIER_SEGMENT
    {
        public D2D1_POINT_2F point1;
        public D2D1_POINT_2F point2;
        public D2D1_POINT_2F point3;
    }

    public enum D2D1_FIGURE_END
    {
        D2D1_FIGURE_END_OPEN = 0,
        D2D1_FIGURE_END_CLOSED = 1,
        D2D1_FIGURE_END_FORCE_DWORD = unchecked((int)0xffffffff)
    }
    public enum DWRITE_GLYPH_ORIENTATION_ANGLE
    {
        /// <summary>
        /// Glyph orientation is upright.
        /// </summary>
        DWRITE_GLYPH_ORIENTATION_ANGLE_0_DEGREES,

        /// <summary>
        /// Glyph orientation is rotated 90 clockwise.
        /// </summary>
        DWRITE_GLYPH_ORIENTATION_ANGLE_90_DEGREES,

        /// <summary>
        /// Glyph orientation is upside-down.
        /// </summary>
        DWRITE_GLYPH_ORIENTATION_ANGLE_180_DEGREES,

        /// <summary>
        /// Glyph orientation is rotated 270 clockwise.
        /// </summary>
        DWRITE_GLYPH_ORIENTATION_ANGLE_270_DEGREES,
    }

    public enum DWRITE_VERTICAL_GLYPH_ORIENTATION
    {
        /// <summary>
        /// In vertical layout, naturally horizontal scripts (Latin, Thai, Arabic,
        /// Devanagari) rotate 90 degrees clockwise, while ideographic scripts
        /// (Chinese, Japanese, Korean) remain upright, 0 degrees.
        /// </summary>
        DWRITE_VERTICAL_GLYPH_ORIENTATION_DEFAULT,

        /// <summary>
        /// Ideographic scripts and scripts that permit stacking
        /// (Latin, Hebrew) are stacked in vertical reading layout.
        /// Connected scripts (Arabic, Syriac, 'Phags-pa, Ogham),
        /// which would otherwise look broken if glyphs were kept
        /// at 0 degrees, remain connected and rotate.
        /// </summary>
        DWRITE_VERTICAL_GLYPH_ORIENTATION_STACKED,
    }

    public enum DWRITE_OPTICAL_ALIGNMENT
    {
        /// <summary>
        /// Align to the default metrics of the glyph.
        /// </summary>
        DWRITE_OPTICAL_ALIGNMENT_NONE,

        /// <summary>
        /// Align glyphs to the margins. Without this, some small whitespace
        /// may be present between the text and the margin from the glyph's side
        /// bearing values. Note that glyphs may still overhang outside the
        /// margin, such as flourishes or italic slants.
        /// </summary>
        DWRITE_OPTICAL_ALIGNMENT_NO_SIDE_BEARINGS,
    }

    public enum DWRITE_GRID_FIT_MODE
    {
        /// <summary>
        /// Choose grid fitting base on the font's gasp table information.
        /// </summary>
        DWRITE_GRID_FIT_MODE_DEFAULT,

        /// <summary>
        /// Always disable grid fitting, using the ideal glyph outlines.
        /// </summary>
        DWRITE_GRID_FIT_MODE_DISABLED,

        /// <summary>
        /// Enable grid fitting, adjusting glyph outlines for device pixel display.
        /// </summary>
        DWRITE_GRID_FIT_MODE_ENABLED
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_TEXT_METRICS1 // : DWRITE_TEXT_METRICS
    {
        /// <summary>
        /// Left-most point of formatted text relative to layout box
        /// (excluding any glyph overhang).
        /// </summary>
        public float left;
        /// <summary>
        /// Top-most point of formatted text relative to layout box
        /// (excluding any glyph overhang).
        /// </summary>
        public float top;
        /// <summary>
        /// The width of the formatted text ignoring trailing whitespace
        /// at the end of each line.
        /// </summary>
        public float width;
        /// <summary>
        /// The width of the formatted text taking into account the
        /// trailing whitespace at the end of each line.
        /// </summary>
        public float widthIncludingTrailingWhitespace;
        /// <summary>
        /// The height of the formatted text. The height of an empty string
        /// is determined by the size of the default font's line height.
        /// </summary>
        public float height;
        /// <summary>
        /// Initial width given to the layout. Depending on whether the text
        /// was wrapped or not, it can be either larger or smaller than the
        /// text content width.
        /// </summary>
        public float layoutWidth;
        /// <summary>
        /// Initial height given to the layout. Depending on the length of the
        /// text, it may be larger or smaller than the text content height.
        /// </summary>
        public float layoutHeight;
        /// <summary>
        /// The maximum reordering count of any line of text, used
        /// to calculate the most number of hit-testing boxes needed.
        /// If the layout has no bidirectional text or no text at all,
        /// the minimum level is 1.
        /// </summary>
        public int maxBidiReorderingDepth;
        /// <summary>
        /// Total number of lines.
        /// </summary>
        public int lineCount;
        /// <summary>
        /// The height of the formatted text taking into account the
        /// trailing whitespace at the end of each line, which will
        /// matter for vertical reading directions.
        /// </summary>
        public float heightIncludingTrailingWhitespace;
    }

    public enum DWRITE_BASELINE
    {
        /// <summary>
        /// The Roman baseline for horizontal, Central baseline for vertical.
        /// </summary>
        DWRITE_BASELINE_DEFAULT,

        /// <summary>
        /// The baseline used by alphabetic scripts such as Latin, Greek, Cyrillic.
        /// </summary>
        DWRITE_BASELINE_ROMAN,

        /// <summary>
        /// Central baseline, generally used for vertical text.
        /// </summary>
        DWRITE_BASELINE_CENTRAL,

        /// <summary>
        /// Mathematical baseline which math characters are centered on.
        /// </summary>
        DWRITE_BASELINE_MATH,

        /// <summary>
        /// Hanging baseline, used in scripts like Devanagari.
        /// </summary>
        DWRITE_BASELINE_HANGING,

        /// <summary>
        /// Ideographic bottom baseline for CJK, left in vertical.
        /// </summary>
        DWRITE_BASELINE_IDEOGRAPHIC_BOTTOM,

        /// <summary>
        /// Ideographic top baseline for CJK, right in vertical.
        /// </summary>
        DWRITE_BASELINE_IDEOGRAPHIC_TOP,

        /// <summary>
        /// The bottom-most extent in horizontal, left-most in vertical.
        /// </summary>
        DWRITE_BASELINE_MINIMUM,

        /// <summary>
        /// The top-most extent in horizontal, right-most in vertical.
        /// </summary>
        DWRITE_BASELINE_MAXIMUM,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_SCRIPT_PROPERTIES
    {
        /// <summary>
        /// The standardized four character code for the given script.
        /// Note these only include the general Unicode scripts, not any
        /// additional ISO 15924 scripts for bibliographic distinction
        /// (for example, Fraktur Latin vs Gaelic Latin).
        /// http://unicode.org/iso15924/iso15924-codes.html
        /// </summary>
        public uint isoScriptCode;

        /// <summary>
        /// The standardized numeric code, ranging 0-999.
        /// http://unicode.org/iso15924/iso15924-codes.html
        /// </summary>
        public uint isoScriptNumber;

        /// <summary>
        /// Number of characters to estimate look-ahead for complex scripts.
        /// Latin and all Kana are generally 1. Indic scripts are up to 15,
        /// and most others are 8. Note that combining marks and variation
        /// selectors can produce clusters longer than these look-aheads,
        /// so this estimate is considered typical language use. Diacritics
        /// must be tested explicitly separately.
        /// </summary>
        public uint clusterLookahead;

        /// <summary>
        /// Appropriate character to elongate the given script for justification.
        ///
        /// Examples:
        ///   Arabic    - U+0640 Tatweel
        ///   Ogham     - U+1680 Ogham Space Mark
        /// </summary>
        public uint justificationCharacter;

        /// <summary>
        /// Restrict the caret to whole clusters, like Thai and Devanagari. Scripts
        /// such as Arabic by default allow navigation between clusters. Others
        /// like Thai always navigate across whole clusters.
        /// </summary>
        /// 
        [MarshalAs(UnmanagedType.SysUInt, SizeConst = 1)]
        public uint restrictCaretToClusters;

        /// <summary>
        /// The language uses dividers between words, such as spaces between Latin
        /// or the Ethiopic wordspace.
        ///
        /// Examples: Latin, Greek, Devanagari, Ethiopic
        /// Excludes: Chinese, Korean, Thai.
        /// </summary>
        ///
        [MarshalAs(UnmanagedType.SysUInt, SizeConst = 1)]
        public uint usesWordDividers;

        /// <summary>
        /// The characters are discrete units from each other. This includes both
        /// block scripts and clustered scripts.
        ///
        /// Examples: Latin, Greek, Cyrillic, Hebrew, Chinese, Thai
        /// </summary>
        /// 
        [MarshalAs(UnmanagedType.SysUInt, SizeConst = 1)]
        public uint isDiscreteWriting;

        /// <summary>
        /// The language is a block script, expanding between characters.
        ///
        /// Examples: Chinese, Japanese, Korean, Bopomofo.
        /// </summary>
        /// 
        [MarshalAs(UnmanagedType.SysUInt, SizeConst = 1)]
        public uint isBlockWriting;

        /// <summary>
        /// The language is justified within glyph clusters, not just between glyph
        /// clusters. One such as the character sequence is Thai Lu and Sara Am
        /// (U+E026, U+E033) which form a single cluster but still expand between
        /// them.
        ///
        /// Examples: Thai, Lao, Khmer
        /// </summary>
        /// 
        [MarshalAs(UnmanagedType.SysUInt, SizeConst = 1)]
        public uint isDistributedWithinCluster;

        /// <summary>
        /// The script's clusters are connected to each other (such as the
        /// baseline-linked Devanagari), and no separation should be added
        /// between characters. Note that cursively linked scripts like Arabic
        /// are also connected (but not all connected scripts are
        /// cursive).
        /// 
        /// Examples: Devanagari, Arabic, Syriac, Bengali, Gurmukhi, Ogham
        /// Excludes: Latin, Chinese, Thaana
        /// </summary>
        /// 
        [MarshalAs(UnmanagedType.SysUInt, SizeConst = 1)]
        public uint isConnectedWriting1;

        /// <summary>
        /// The script is naturally cursive (Arabic/Syriac), meaning it uses other
        /// justification methods like kashida extension rather than intercharacter
        /// spacing. Note that although other scripts like Latin and Japanese may
        /// actually support handwritten cursive forms, they are not considered
        /// cursive scripts.
        /// 
        /// Examples: Arabic, Syriac, Mongolian
        /// Excludes: Thaana, Devanagari, Latin, Chinese
        /// </summary>
        /// 
        [MarshalAs(UnmanagedType.SysUInt, SizeConst = 1)]
        public uint isCursiveWriting1;

        [MarshalAs(UnmanagedType.SysUInt, SizeConst = 25)]
        public uint reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_JUSTIFICATION_OPPORTUNITY
    {
        /// <summary>
        /// Minimum amount of expansion to apply to the side of the glyph.
        /// This may vary from 0 to infinity, typically being zero except
        /// for kashida.
        /// </summary>
        public float expansionMinimum;

        /// <summary>
        /// Maximum amount of expansion to apply to the side of the glyph.
        /// This may vary from 0 to infinity, being zero for fixed-size characters
        /// and connected scripts, and non-zero for discrete scripts, and non-zero
        /// for cursive scripts at expansion points.
        /// </summary>
        public float expansionMaximum;

        /// <summary>
        /// Maximum amount of compression to apply to the side of the glyph.
        /// This may vary from 0 up to the glyph cluster size.
        /// </summary>
        public float compressionMaximum;

        /// <summary>
        /// Priority of this expansion point. Larger priorities are applied later,
        /// while priority zero does nothing.
        /// </summary>
        /// 
        [MarshalAs(UnmanagedType.SysUInt, SizeConst = 8)]
        public uint expansionPriority;

        /// <summary>
        /// Priority of this compression point. Larger priorities are applied later,
        /// while priority zero does nothing.
        /// </summary>
        /// 
        [MarshalAs(UnmanagedType.SysUInt, SizeConst = 8)]
        public uint compressionPriority;

        /// <summary>
        /// Allow this expansion point to use up any remaining slack space even
        /// after all expansion priorities have been used up.
        /// </summary>
        /// 
        [MarshalAs(UnmanagedType.SysUInt, SizeConst = 1)]
        public uint allowResidualExpansion;

        /// <summary>
        /// Allow this compression point to use up any remaining space even after
        /// all compression priorities have been used up.
        /// </summary>
        /// 
        [MarshalAs(UnmanagedType.SysUInt, SizeConst = 1)]
        public uint allowResidualCompression;

        /// <summary>
        /// Apply expansion/compression to the leading edge of the glyph. This will
        /// be false for connected scripts, fixed-size characters, and diacritics.
        /// It is generally false within a multi-glyph cluster, unless the script
        /// allows expansion of glyphs within a cluster, like Thai.
        /// </summary>
        /// 
        [MarshalAs(UnmanagedType.SysUInt, SizeConst = 1)]
        public uint applyToLeadingEdge;

        /// <summary>
        /// Apply expansion/compression to the trailing edge of the glyph. This will
        /// be false for connected scripts, fixed-size characters, and diacritics.
        /// It is generally false within a multi-glyph cluster, unless the script
        /// allows expansion of glyphs within a cluster, like Thai.
        /// </summary>
        /// 
        [MarshalAs(UnmanagedType.SysUInt, SizeConst = 1)]
        public uint applyToTrailingEdge;

        [MarshalAs(UnmanagedType.SysUInt, SizeConst = 12)]
        public uint reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_UNICODE_RANGE
    {
        /// <summary>
        /// The first codepoint in the Unicode range.
        /// </summary>
        public uint first;

        /// <summary>
        /// The last codepoint in the Unicode range.
        /// </summary>
        public uint last;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_FONT_METRICS1// : public DWRITE_FONT_METRICS
    {
        /// <summary>
        /// The number of font design units per em unit.
        /// Font files use their own coordinate system of font design units.
        /// A font design unit is the smallest measurable unit in the em square,
        /// an imaginary square that is used to size and align glyphs.
        /// The concept of em square is used as a reference scale factor when defining font size and device transformation semantics.
        /// The size of one em square is also commonly used to compute the paragraph indentation value.
        /// </summary>
        public UInt16 designUnitsPerEm;
        /// <summary>
        /// Ascent value of the font face in font design units.
        /// Ascent is the distance from the top of font character alignment box to English baseline.
        /// </summary>
        public UInt16 ascent;
        /// <summary>
        /// Descent value of the font face in font design units.
        /// Descent is the distance from the bottom of font character alignment box to English baseline.
        /// </summary>
        public UInt16 descent;
        /// <summary>
        /// Line gap in font design units.
        /// Recommended additional white space to add between lines to improve legibility. The recommended line spacing 
        /// (baseline-to-baseline distance) is thus the sum of ascent, descent, and lineGap. The line gap is usually 
        /// positive or zero but can be negative, in which case the recommended line spacing is less than the height
        /// of the character alignment box.
        /// </summary>
        public UInt16 lineGap;
        /// <summary>
        /// Cap height value of the font face in font design units.
        /// Cap height is the distance from English baseline to the top of a typical English capital.
        /// Capital "H" is often used as a reference character for the purpose of calculating the cap height value.
        /// </summary>
        public UInt16 capHeight;
        /// <summary>
        /// x-height value of the font face in font design units.
        /// x-height is the distance from English baseline to the top of lowercase letter "x", or a similar lowercase character.
        /// </summary>
        public UInt16 xHeight;
        /// <summary>
        /// The underline position value of the font face in font design units.
        /// Underline position is the position of underline relative to the English baseline.
        /// The value is usually made negative in order to place the underline below the baseline.
        /// </summary>
        public UInt16 underlinePosition;
        /// <summary>
        /// The suggested underline thickness value of the font face in font design units.
        /// </summary>
        public UInt16 underlineThickness;
        /// <summary>
        /// The strikethrough position value of the font face in font design units.
        /// Strikethrough position is the position of strikethrough relative to the English baseline.
        /// The value is usually made positive in order to place the strikethrough above the baseline.
        /// </summary>
        public UInt16 strikethroughPosition;
        /// <summary>
        /// The suggested strikethrough thickness value of the font face in font design units.
        /// </summary>
        public UInt16 strikethroughThickness;
        /// <summary>
        /// Left edge of accumulated bounding blackbox of all glyphs in the font.
        /// </summary>
        public Int16 glyphBoxLeft;

        /// <summary>
        /// Top edge of accumulated bounding blackbox of all glyphs in the font.
        /// </summary>
        public Int16 glyphBoxTop;

        /// <summary>
        /// Right edge of accumulated bounding blackbox of all glyphs in the font.
        /// </summary>
        public Int16 glyphBoxRight;

        /// <summary>
        /// Bottom edge of accumulated bounding blackbox of all glyphs in the font.
        /// </summary>
        public Int16 glyphBoxBottom;

        /// <summary>
        /// Horizontal position of the subscript relative to the baseline origin.
        /// This is typically negative (to the left) in italic/oblique fonts, and
        /// zero in regular fonts.
        /// </summary>
        public Int16 subscriptPositionX;

        /// <summary>
        /// Vertical position of the subscript relative to the baseline.
        /// This is typically negative.
        /// </summary>
        public Int16 subscriptPositionY;

        /// <summary>
        /// Horizontal size of the subscript em box in design units, used to
        /// scale the simulated subscript relative to the full em box size.
        /// This the numerator of the scaling ratio where denominator is the
        /// design units per em. If this member is zero, the font does not specify
        /// a scale factor, and the client should use its own policy.
        /// </summary>
        public Int16 subscriptSizeX;

        /// <summary>
        /// Vertical size of the subscript em box in design units, used to
        /// scale the simulated subscript relative to the full em box size.
        /// This the numerator of the scaling ratio where denominator is the
        /// design units per em. If this member is zero, the font does not specify
        /// a scale factor, and the client should use its own policy.
        /// </summary>
        public Int16 subscriptSizeY;

        /// <summary>
        /// Horizontal position of the superscript relative to the baseline origin.
        /// This is typically positive (to the right) in italic/oblique fonts, and
        /// zero in regular fonts.
        /// </summary>
        public Int16 superscriptPositionX;

        /// <summary>
        /// Vertical position of the superscript relative to the baseline.
        /// This is typically positive.
        /// </summary>
        public Int16 superscriptPositionY;

        /// <summary>
        /// Horizontal size of the superscript em box in design units, used to
        /// scale the simulated superscript relative to the full em box size.
        /// This the numerator of the scaling ratio where denominator is the
        /// design units per em. If this member is zero, the font does not specify
        /// a scale factor, and the client should use its own policy.
        /// </summary>
        public Int16 superscriptSizeX;

        /// <summary>
        /// Vertical size of the superscript em box in design units, used to
        /// scale the simulated superscript relative to the full em box size.
        /// This the numerator of the scaling ratio where denominator is the
        /// design units per em. If this member is zero, the font does not specify
        /// a scale factor, and the client should use its own policy.
        /// </summary>
        public Int16 superscriptSizeY;

        /// <summary>
        /// Indicates that the ascent, descent, and lineGap are based on newer 
        /// 'typographic' values in the font, rather than legacy values.
        /// </summary>
        public bool hasTypographicMetrics;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_CARET_METRICS
    {
        /// <summary>
        /// Vertical rise of the caret. Rise / Run yields the caret angle.
        /// Rise = 1 for perfectly upright fonts (non-italic).
        /// </summary>
        public Int16 slopeRise;

        /// <summary>
        /// Horizontal run of th caret. Rise / Run yields the caret angle.
        /// Run = 0 for perfectly upright fonts (non-italic).
        /// </summary>
        public Int16 slopeRun;

        /// <summary>
        /// Horizontal offset of the caret along the baseline for good appearance.
        /// Offset = 0 for perfectly upright fonts (non-italic).
        /// </summary>
        public Int16 offset;
    }

    [StructLayout(LayoutKind.Explicit)]
    public unsafe partial struct DWRITE_PANOSE
    { 
        [FieldOffset(0)]       
        public fixed byte Values[10];

        [FieldOffset(0)]      
        public byte FamilyKind;

        [FieldOffset(0)]
        public PanoseText Text;

        [FieldOffset(0)]    
        public PanoseScript Script;

        [FieldOffset(0)]   
        public PanoseDecorative Decorative;

        [FieldOffset(0)]    
        public PanoseSymbol Symbol;
    }

    public unsafe partial struct PanoseText
    {
        public byte familyKind; // = 2 for text
        public byte serifStyle;
        public byte weight;
        public byte proportion;
        public byte contrast;
        public byte strokeVariation;
        public byte armStyle;
        public byte letterform;
        public byte midline;
        public byte xHeight;
    }

    public unsafe partial struct PanoseScript
    {
        public byte familyKind; // = 3 for script
        public byte toolKind;
        public byte weight;
        public byte spacing;
        public byte aspectRatio;
        public byte contrast;
        public byte scriptTopology;
        public byte scriptForm;
        public byte finials;
        public byte xAscent;
    }

    public unsafe partial struct PanoseDecorative
    {
        public byte familyKind; // = 4 for decorative
        public byte decorativeClass;
        public byte weight;
        public byte aspect;
        public byte contrast;
        public byte serifVariant;
        public byte fill;
        public byte lining;
        public byte decorativeTopology;
        public byte characterRange;
    }

    public unsafe partial struct PanoseSymbol
    {
        public byte familyKind; // = 5 for symbol
        public byte symbolKind;
        public byte weight;
        public byte spacing;
        public byte aspectRatioAndContrast;
        public byte aspectRatio94;
        public byte aspectRatio119;
        public byte aspectRatio157;
        public byte aspectRatio163;
        public byte aspectRatio211;
    }

    public enum DWRITE_OUTLINE_THRESHOLD
    {
        DWRITE_OUTLINE_THRESHOLD_ANTIALIASED,
        DWRITE_OUTLINE_THRESHOLD_ALIASED
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct D3DCOLORVALUE
    {
        public float r;
        public float g;
        public float b;
        public float a;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct DWRITE_COLOR_F
    {
        public float r;
        public float g;
        public float b;
        public float a;
    }

    public enum DWRITE_TEXT_ANTIALIAS_MODE
    {
        /// <summary>
        /// ClearType antialiasing computes coverage independently for the red, green, and blue
        /// color elements of each pixel. This allows for more detail than conventional antialiasing.
        /// However, because there is no one alpha value for each pixel, ClearType is not suitable
        /// rendering text onto a transparent intermediate bitmap.
        /// </summary>
        DWRITE_TEXT_ANTIALIAS_MODE_CLEARTYPE,

        /// <summary>
        /// Grayscale antialiasing computes one coverage value for each pixel. Because the alpha
        /// value of each pixel is well-defined, text can be rendered onto a transparent bitmap, 
        /// which can then be composited with other content. Note that grayscale rendering with
        /// IDWriteBitmapRenderTarget1 uses premultiplied alpha.
        /// </summary>
        DWRITE_TEXT_ANTIALIAS_MODE_GRAYSCALE
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_COLOR_GLYPH_RUN
    {
        /// <summary>
        /// Glyph run to render.
        /// </summary>
        public DWRITE_GLYPH_RUN glyphRun;

        /// <summary>
        /// Optional glyph run description.
        /// </summary>
        public DWRITE_GLYPH_RUN_DESCRIPTION glyphRunDescription;

        /// <summary>
        /// Location at which to draw this glyph run.
        /// </summary>
        public float baselineOriginX;
        public float baselineOriginY;

        /// <summary>
        /// Color to use for this layer, if any. This is the same color that
        /// IDWriteFontFace2::GetPaletteEntries would return for the current
        /// palette index if the paletteIndex member is less than 0xFFFF. If
        /// the paletteIndex member is 0xFFFF then there is no associated
        /// palette entry, this member is set to { 0, 0, 0, 0 }, and the client
        /// should use the current foreground brush.
        /// </summary>
        /// 
        public DWRITE_COLOR_F runColor;        

        /// <summary>
        /// Zero-based index of this layer's color entry in the current color
        /// palette, or 0xFFFF if this layer is to be rendered using 
        /// the current foreground brush.
        /// </summary>
        public UInt16 paletteIndex;
    }

    public enum DWRITE_FONT_PROPERTY_ID
    {
        /// <summary>
        /// Unspecified font property identifier.
        /// </summary>
        DWRITE_FONT_PROPERTY_ID_NONE,

        /// <summary>
        /// Family name for the weight-stretch-style model.
        /// </summary>
        DWRITE_FONT_PROPERTY_ID_WEIGHT_STRETCH_STYLE_FAMILY_NAME,

        /// <summary>
        /// Family name preferred by the designer. This enables font designers to group more than four fonts in a single family without losing compatibility with
        /// GDI. This name is typically only present if it differs from the GDI-compatible family name.
        /// </summary>
        DWRITE_FONT_PROPERTY_ID_TYPOGRAPHIC_FAMILY_NAME,

        /// <summary>
        /// Face name of the for the weight-stretch-style (e.g., Regular or Bold).
        /// </summary>
        DWRITE_FONT_PROPERTY_ID_WEIGHT_STRETCH_STYLE_FACE_NAME,

        /// <summary>
        /// The full name of the font, e.g. "Arial Bold", from name id 4 in the name table.
        /// </summary>
        DWRITE_FONT_PROPERTY_ID_FULL_NAME,

        /// <summary>
        /// GDI-compatible family name. Because GDI allows a maximum of four fonts per family, fonts in the same family may have different GDI-compatible family names
        /// (e.g., "Arial", "Arial Narrow", "Arial Black").
        /// </summary>
        DWRITE_FONT_PROPERTY_ID_WIN32_FAMILY_NAME,

        /// <summary>
        /// The postscript name of the font, e.g. "GillSans-Bold" from name id 6 in the name table.
        /// </summary>
        DWRITE_FONT_PROPERTY_ID_POSTSCRIPT_NAME,

        /// <summary>
        /// Script/language tag to identify the scripts or languages that the font was
        /// primarily designed to support.
        /// </summary>
        /// <remarks>
        /// The design script/language tag is meant to be understood from the perspective of
        /// users. For example, a font is considered designed for English if it is considered
        /// useful for English users. Note that this is different from what a font might be
        /// capable of supporting. For example, the Meiryo font was primarily designed for
        /// Japanese users. While it is capable of displaying English well, it was not
        /// meant to be offered for the benefit of non-Japanese-speaking English users.
        ///
        /// As another example, a font designed for Chinese may be capable of displaying
        /// Japanese text, but would likely look incorrect to Japanese users.
        /// 
        /// The valid values for this property are "ScriptLangTag" values. These are adapted
        /// from the IETF BCP 47 specification, "Tags for Identifying Languages" (see
        /// http://tools.ietf.org/html/bcp47). In a BCP 47 language tag, a language subtag
        /// element is mandatory and other subtags are optional. In a ScriptLangTag, a
        /// script subtag is mandatory and other subtags are option. The following
        /// augmented BNF syntax, adapted from BCP 47, is used:
        /// 
        ///     ScriptLangTag = [language "-"]
        ///                     script
        ///                     ["-" region]
        ///                     *("-" variant)
        ///                     *("-" extension)
        ///                     ["-" privateuse]
        /// 
        /// The expansion of the elements and the intended semantics associated with each
        /// are as defined in BCP 47. Script subtags are taken from ISO 15924. At present,
        /// no extensions are defined, and any extension should be ignored. Private use
        /// subtags are defined by private agreement between the source and recipient and
        /// may be ignored.
        /// 
        /// Subtags must be valid for use in BCP 47 and contained in the Language Subtag
        /// Registry maintained by IANA. (See
        /// http://www.iana.org/assignments/language-subtag-registry/language-subtag-registry
        /// and section 3 of BCP 47 for details.
        /// 
        /// Any ScriptLangTag value not conforming to these specifications is ignored.
        /// 
        /// Examples:
        ///   "Latn" denotes Latin script (and any language or writing system using Latin)
        ///   "Cyrl" denotes Cyrillic script
        ///   "sr-Cyrl" denotes Cyrillic script as used for writing the Serbian language;
        ///       a font that has this property value may not be suitable for displaying
        ///       text in Russian or other languages written using Cyrillic script
        ///   "Jpan" denotes Japanese writing (Han + Hiragana + Katakana)
        ///
        /// When passing this property to GetPropertyValues, use the overload which does
        /// not take a language parameter, since this property has no specific language.
        /// </remarks>
        DWRITE_FONT_PROPERTY_ID_DESIGN_SCRIPT_LANGUAGE_TAG,

        /// <summary>
        /// Script/language tag to identify the scripts or languages that the font declares
        /// it is able to support.
        /// </summary>
        DWRITE_FONT_PROPERTY_ID_SUPPORTED_SCRIPT_LANGUAGE_TAG,

        /// <summary>
        /// Semantic tag to describe the font (e.g. Fancy, Decorative, Handmade, Sans-serif, Swiss, Pixel, Futuristic).
        /// </summary>
        DWRITE_FONT_PROPERTY_ID_SEMANTIC_TAG,

        /// <summary>
        /// Weight of the font represented as a decimal string in the range 1-999.
        /// </summary>
        /// <remark>
        /// This enum is discouraged for use with IDWriteFontSetBuilder2 in favor of the more generic font axis
        /// DWRITE_FONT_AXIS_TAG_WEIGHT which supports higher precision and range.
        /// </remark>
        DWRITE_FONT_PROPERTY_ID_WEIGHT,

        /// <summary>
        /// Stretch of the font represented as a decimal string in the range 1-9.
        /// </summary>
        /// <remark>
        /// This enum is discouraged for use with IDWriteFontSetBuilder2 in favor of the more generic font axis
        /// DWRITE_FONT_AXIS_TAG_WIDTH which supports higher precision and range.
        /// </remark>
        DWRITE_FONT_PROPERTY_ID_STRETCH,

        /// <summary>
        /// Style of the font represented as a decimal string in the range 0-2.
        /// </summary>
        /// <remark>
        /// This enum is discouraged for use with IDWriteFontSetBuilder2 in favor of the more generic font axes
        /// DWRITE_FONT_AXIS_TAG_SLANT and DWRITE_FONT_AXIS_TAG_ITAL.
        /// </remark>
        DWRITE_FONT_PROPERTY_ID_STYLE,

        /// <summary>
        /// Face name preferred by the designer. This enables font designers to group more than four fonts in a single
        /// family without losing compatibility with GDI.
        /// </summary>
        DWRITE_FONT_PROPERTY_ID_TYPOGRAPHIC_FACE_NAME,

        /// <summary>
        /// Total number of properties for NTDDI_WIN10 (IDWriteFontSet).
        /// </summary>
        /// <remarks>
        /// DWRITE_FONT_PROPERTY_ID_TOTAL cannot be used as a property ID.
        /// </remarks>
        DWRITE_FONT_PROPERTY_ID_TOTAL = DWRITE_FONT_PROPERTY_ID_STYLE + 1,

        /// <summary>
        /// Total number of properties for NTDDI_WIN10_RS3 (IDWriteFontSet1).
        /// </summary>
        DWRITE_FONT_PROPERTY_ID_TOTAL_RS3 = DWRITE_FONT_PROPERTY_ID_TYPOGRAPHIC_FACE_NAME + 1,

        // Obsolete aliases kept to avoid breaking existing code.
        DWRITE_FONT_PROPERTY_ID_PREFERRED_FAMILY_NAME = DWRITE_FONT_PROPERTY_ID_TYPOGRAPHIC_FAMILY_NAME,
        DWRITE_FONT_PROPERTY_ID_FAMILY_NAME = DWRITE_FONT_PROPERTY_ID_WEIGHT_STRETCH_STYLE_FAMILY_NAME,
        DWRITE_FONT_PROPERTY_ID_FACE_NAME = DWRITE_FONT_PROPERTY_ID_WEIGHT_STRETCH_STYLE_FACE_NAME,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_FONT_PROPERTY
    {
        /// <summary>
        /// Specifies the requested font property, such as DWRITE_FONT_PROPERTY_ID_FAMILY_NAME.
        /// </summary>
        public DWRITE_FONT_PROPERTY_ID propertyId;

        /// <summary>
        /// Specifies the property value, such as "Segoe UI".
        /// </summary>
        public string propertyValue;

        /// <summary>
        /// Specifies the language / locale to use, such as "en-US". 
        /// </summary>
        /// <remarks>
        /// When passing property information to AddFontFaceReference, localeName indicates
        /// the language of the property value. BCP 47 language tags should be used. If a
        /// property value is inherently non-linguistic, this can be left empty.
        ///
        /// When used for font set filtering, leave this empty: a match will be found
        /// regardless of language associated with property values.
        /// </remarks>
        public string localeName;
    }

    public enum DWRITE_LOCALITY
    {
        /// <summary>
        /// The resource is remote, and information is unknown yet, including the file size and date.
        /// Attempting to create a font or file stream will fail until locality becomes at least partial.
        /// </summary>
        DWRITE_LOCALITY_REMOTE,

        /// <summary>
        /// The resource is partially local, meaning you can query the size and date of the file
        /// stream, and you may be able to create a font face and retrieve the particular glyphs
        /// for metrics and drawing, but not all the glyphs will be present.
        /// </summary>
        DWRITE_LOCALITY_PARTIAL,

        /// <summary>
        /// The resource is completely local, and all font functions can be called
        /// without concern of missing data or errors related to network connectivity.
        /// </summary>
        DWRITE_LOCALITY_LOCAL,
    }

    public enum DWRITE_RENDERING_MODE1
    {
        /// <summary>
        /// Specifies that the rendering mode is determined automatically based on the font and size.
        /// </summary>
        DWRITE_RENDERING_MODE1_DEFAULT = DWRITE_RENDERING_MODE.DWRITE_RENDERING_MODE_DEFAULT,

        /// <summary>
        /// Specifies that no antialiasing is performed. Each pixel is either set to the foreground 
        /// color of the text or retains the color of the background.
        /// </summary>
        DWRITE_RENDERING_MODE1_ALIASED = DWRITE_RENDERING_MODE.DWRITE_RENDERING_MODE_ALIASED,

        /// <summary>
        /// Specifies that antialiasing is performed in the horizontal direction and the appearance
        /// of glyphs is layout-compatible with GDI using CLEARTYPE_QUALITY. Use DWRITE_MEASURING_MODE_GDI_CLASSIC 
        /// to get glyph advances. The antialiasing may be either ClearType or grayscale depending on
        /// the text antialiasing mode.
        /// </summary>
        DWRITE_RENDERING_MODE1_GDI_CLASSIC = DWRITE_RENDERING_MODE.DWRITE_RENDERING_MODE_GDI_CLASSIC,

        /// <summary>
        /// Specifies that antialiasing is performed in the horizontal direction and the appearance
        /// of glyphs is layout-compatible with GDI using CLEARTYPE_NATURAL_QUALITY. Glyph advances
        /// are close to the font design advances, but are still rounded to whole pixels. Use
        /// DWRITE_MEASURING_MODE_GDI_NATURAL to get glyph advances. The antialiasing may be either
        /// ClearType or grayscale depending on the text antialiasing mode.
        /// </summary>
        DWRITE_RENDERING_MODE1_GDI_NATURAL = DWRITE_RENDERING_MODE.DWRITE_RENDERING_MODE_GDI_NATURAL,

        /// <summary>
        /// Specifies that antialiasing is performed in the horizontal direction. This rendering
        /// mode allows glyphs to be positioned with subpixel precision and is therefore suitable
        /// for natural (i.e., resolution-independent) layout. The antialiasing may be either
        /// ClearType or grayscale depending on the text antialiasing mode.
        /// </summary>
        DWRITE_RENDERING_MODE1_NATURAL = DWRITE_RENDERING_MODE.DWRITE_RENDERING_MODE_NATURAL,

        /// <summary>
        /// Similar to natural mode except that antialiasing is performed in both the horizontal
        /// and vertical directions. This is typically used at larger sizes to make curves and
        /// diagonal lines look smoother. The antialiasing may be either ClearType or grayscale
        /// depending on the text antialiasing mode.
        /// </summary>
        DWRITE_RENDERING_MODE1_NATURAL_SYMMETRIC = DWRITE_RENDERING_MODE.DWRITE_RENDERING_MODE_NATURAL_SYMMETRIC,

        /// <summary>
        /// Specifies that rendering should bypass the rasterizer and use the outlines directly. 
        /// This is typically used at very large sizes.
        /// </summary>
        DWRITE_RENDERING_MODE1_OUTLINE = DWRITE_RENDERING_MODE.DWRITE_RENDERING_MODE_OUTLINE,

        /// <summary>
        /// Similar to natural symmetric mode except that when possible, text should be rasterized
        /// in a downsampled form.
        /// </summary>
        DWRITE_RENDERING_MODE1_NATURAL_SYMMETRIC_DOWNSAMPLED,
    }

    public enum DWRITE_GLYPH_IMAGE_FORMATS
    {
        /// <summary>
        /// Indicates no data is available for this glyph.
        /// </summary>
        DWRITE_GLYPH_IMAGE_FORMATS_NONE = 0x00000000,

        /// <summary>
        /// The glyph has TrueType outlines.
        /// </summary>
        DWRITE_GLYPH_IMAGE_FORMATS_TRUETYPE = 0x00000001,

        /// <summary>
        /// The glyph has CFF outlines.
        /// </summary>
        DWRITE_GLYPH_IMAGE_FORMATS_CFF = 0x00000002,

        /// <summary>
        /// The glyph has multilayered COLR data.
        /// </summary>
        DWRITE_GLYPH_IMAGE_FORMATS_COLR = 0x00000004,

        /// <summary>
        /// The glyph has SVG outlines as standard XML.
        /// </summary>
        /// <remarks>
        /// Fonts may store the content gzip'd rather than plain text,
        /// indicated by the first two bytes as gzip header {0x1F 0x8B}.
        /// </remarks>
        DWRITE_GLYPH_IMAGE_FORMATS_SVG = 0x00000008,

        /// <summary>
        /// The glyph has PNG image data, with standard PNG IHDR.
        /// </summary>
        DWRITE_GLYPH_IMAGE_FORMATS_PNG = 0x00000010,

        /// <summary>
        /// The glyph has JPEG image data, with standard JIFF SOI header.
        /// </summary>
        DWRITE_GLYPH_IMAGE_FORMATS_JPEG = 0x00000020,

        /// <summary>
        /// The glyph has TIFF image data.
        /// </summary>
        DWRITE_GLYPH_IMAGE_FORMATS_TIFF = 0x00000040,

        /// <summary>
        /// The glyph has raw 32-bit premultiplied BGRA data.
        /// </summary>
        DWRITE_GLYPH_IMAGE_FORMATS_PREMULTIPLIED_B8G8R8A8 = 0x00000080,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_GLYPH_IMAGE_DATA
    {
        /// <summary>
        /// Pointer to the glyph data, be it SVG, PNG, JPEG, TIFF.
        /// </summary>
        public IntPtr imageData;

        /// <summary>
        /// Size of glyph data in bytes.
        /// </summary>
        public uint imageDataSize;

        /// <summary>
        /// Unique identifier for the glyph data. Clients may use this to cache a parsed/decompressed
        /// version and tell whether a repeated call to the same font returns the same data.
        /// </summary>
        public uint uniqueDataId;

        /// <summary>
        /// Pixels per em of the returned data. For non-scalable raster data (PNG/TIFF/JPG), this can be larger
        /// or smaller than requested from GetGlyphImageData when there isn't an exact match.
        /// For scaling intermediate sizes, use: desired pixels per em * font em size / actual pixels per em.
        /// </summary>
        public uint pixelsPerEm;

        /// <summary>
        /// Size of image when the format is pixel data.
        /// </summary>
        public D2D1_SIZE_U pixelSize;

        /// <summary>
        /// Left origin along the horizontal Roman baseline.
        /// </summary>
        public D2D1_POINT_2L horizontalLeftOrigin;

        /// <summary>
        /// Right origin along the horizontal Roman baseline.
        /// </summary>
        public D2D1_POINT_2L horizontalRightOrigin;

        /// <summary>
        /// Top origin along the vertical central baseline.
        /// </summary>
        public D2D1_POINT_2L verticalTopOrigin;

        /// <summary>
        /// Bottom origin along vertical central baseline.
        /// </summary>
        public D2D1_POINT_2L verticalBottomOrigin;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct D2D1_SIZE_U
    {
        public uint width;
        public uint height;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct D2D1_POINT_2L
    {
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_COLOR_GLYPH_RUN1 //: DWRITE_COLOR_GLYPH_RUN
    {
        /// <summary>
        /// Glyph run to render.
        /// </summary>
        public DWRITE_GLYPH_RUN glyphRun;
        //public IntPtr glyphRun;

        /// <summary>
        /// Optional glyph run description.
        /// </summary>
        //public DWRITE_GLYPH_RUN_DESCRIPTION glyphRunDescription;
        public IntPtr glyphRunDescription;

        /// <summary>
        /// Location at which to draw this glyph run.
        /// </summary>
        public float baselineOriginX;
        public float baselineOriginY;

        /// <summary>
        /// Color to use for this layer, if any. This is the same color that
        /// IDWriteFontFace2::GetPaletteEntries would return for the current
        /// palette index if the paletteIndex member is less than 0xFFFF. If
        /// the paletteIndex member is 0xFFFF then there is no associated
        /// palette entry, this member is set to { 0, 0, 0, 0 }, and the client
        /// should use the current foreground brush.
        /// </summary>
        /// 
        public DWRITE_COLOR_F runColor;

        /// <summary>
        /// Zero-based index of this layer's color entry in the current color
        /// palette, or 0xFFFF if this layer is to be rendered using 
        /// the current foreground brush.
        /// </summary>
        public UInt16 paletteIndex;

        /// <summary>
        /// Type of glyph image format for this color run. Exactly one type will be set since
        /// TranslateColorGlyphRun has already broken down the run into separate parts.
        /// </summary>
        public DWRITE_GLYPH_IMAGE_FORMATS glyphImageFormat;

        /// <summary>
        /// Measuring mode to use for this glyph run.
        /// </summary>
        public DWRITE_MEASURING_MODE measuringMode;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_FILE_FRAGMENT
    {
        /// <summary>
        /// Starting offset of the fragment from the beginning of the file.
        /// </summary>
        public UInt64 fileOffset;

        /// <summary>
        /// Size of the file fragment, in bytes.
        /// </summary>
        public UInt64 fragmentSize;
    }

    public enum DWRITE_CONTAINER_TYPE
    {
        DWRITE_CONTAINER_TYPE_UNKNOWN,
        DWRITE_CONTAINER_TYPE_WOFF,
        DWRITE_CONTAINER_TYPE_WOFF2
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_FONT_AXIS_VALUE
    {
        /// <summary>
        /// Four character identifier of the font axis (weight, width, slant, italic...).
        /// </summary>
        public DWRITE_FONT_AXIS_TAG axisTag;

        /// <summary>
        /// Value for the given axis, with the meaning and range depending on the axis semantics.
        /// Certain well known axes have standard ranges and defaults, such as weight (1..1000, default=400),
        /// width (>0, default=100), slant (-90..90, default=-20), and italic (0 or 1).
        /// </summary>
        public float value;
    }

    public enum DWRITE_FONT_AXIS_TAG : uint
    {
        DWRITE_FONT_AXIS_TAG_WEIGHT = (((uint)((byte)('t')) << 24) | ((uint)((byte)('h')) << 16) | ((uint)((byte)('g')) << 8) | (uint)((byte)('w'))),
        DWRITE_FONT_AXIS_TAG_WIDTH = (((uint)((byte)('h')) << 24) | ((uint)((byte)('t')) << 16) | ((uint)((byte)('d')) << 8) | (uint)((byte)('w'))),
        DWRITE_FONT_AXIS_TAG_SLANT = (((uint)((byte)('t')) << 24) | ((uint)((byte)('n')) << 16) | ((uint)((byte)('l')) << 8) | (uint)((byte)('s'))),
        DWRITE_FONT_AXIS_TAG_OPTICAL_SIZE = (((uint)((byte)('z')) << 24) | ((uint)((byte)('s')) << 16) | ((uint)((byte)('p')) << 8) | (uint)((byte)('o'))),
        DWRITE_FONT_AXIS_TAG_ITALIC = (((uint)((byte)('l')) << 24) | ((uint)((byte)('a')) << 16) | ((uint)((byte)('t')) << 8) | (uint)((byte)('i'))),
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_FONT_AXIS_RANGE
    {
        /// <summary>
        /// Four character identifier of the font axis (weight, width, slant, italic...).
        /// </summary>
        public DWRITE_FONT_AXIS_TAG axisTag;

        /// <summary>
        /// Minimum value supported by this axis.
        /// </summary>
        public float minValue;

        /// <summary>
        /// Maximum value supported by this axis. The maximum can equal the minimum.
        /// </summary>
        public float maxValue;
    }

    public enum DWRITE_FONT_AXIS_ATTRIBUTES
    {
        /// <summary>
        /// No attributes.
        /// </summary>
        DWRITE_FONT_AXIS_ATTRIBUTES_NONE = 0x0000,

        /// <summary>
        /// This axis is implemented as a variation axis in a variable font, with a continuous range of
        /// values, such as a range of weights from 100..900. Otherwise it is either a static axis that
        /// holds a single point, or it has a range but doesn't vary, such as optical size in the Skia
        /// Heading font which covers a range of points but doesn't interpolate any new glyph outlines.
        /// </summary>
        DWRITE_FONT_AXIS_ATTRIBUTES_VARIABLE = 0x0001,

        /// <summary>
        /// This axis is recommended to be remain hidden in user interfaces. The font developer may
        /// recommend this if an axis is intended to be accessed only programmatically, or is meant for
        /// font-internal or font-developer use only. The axis may be exposed in lower-level font
        /// inspection utilities, but should not be exposed in common or even advanced-mode user
        /// interfaces in content-authoring apps.
        /// </summary>
        DWRITE_FONT_AXIS_ATTRIBUTES_HIDDEN = 0x0002,
    }

    public enum DWRITE_FONT_FAMILY_MODEL
    {
        /// <summary>
        /// Families are grouped by the typographic family name preferred by the font author. The family can contain as
        /// many face as the font author wants.
        /// This corresponds to the DWRITE_FONT_PROPERTY_ID_TYPOGRAPHIC_FAMILY_NAME.
        /// </summary>
        DWRITE_FONT_FAMILY_MODEL_TYPOGRAPHIC,

        /// <summary>
        /// Families are grouped by the weight-stretch-style family name, where all faces that differ only by those three
        /// axes are grouped into the same family, but any other axes go into a distinct family. For example, the Sitka
        /// family with six different optical sizes yields six separate families (Sitka Caption, Display, Text, Subheading,
        /// Heading, Banner...). This corresponds to the DWRITE_FONT_PROPERTY_ID_WEIGHT_STRETCH_STYLE_FAMILY_NAME.
        /// </summary>
        DWRITE_FONT_FAMILY_MODEL_WEIGHT_STRETCH_STYLE,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWRITE_LINE_SPACING
    {
        /// <summary>
        /// Method used to determine line spacing.
        /// </summary>
        public DWRITE_LINE_SPACING_METHOD method;

        /// <summary>
        /// Spacing between lines.
        /// The interpretation of this parameter depends upon the line spacing method, as follows:
        /// - default line spacing: ignored
        /// - uniform line spacing: explicit distance in DIPs between lines
        /// - proportional line spacing: a scaling factor to be applied to the computed line height; 
        ///   for each line, the height of the line is computed as for default line spacing, and the scaling factor is applied to that value.
        /// </summary>
        public float height;

        /// <summary>
        /// Distance from top of line to baseline. 
        /// The interpretation of this parameter depends upon the line spacing method, as follows:
        /// - default line spacing: ignored
        /// - uniform line spacing: explicit distance in DIPs from the top of the line to the baseline
        /// - proportional line spacing: a scaling factor applied to the computed baseline; for each line, 
        ///   the baseline distance is computed as for default line spacing, and the scaling factor is applied to that value.
        /// </summary>
        public float baseline;

        /// <summary>
        /// Proportion of the entire leading distributed before the line. The allowed value is between 0 and 1.0. The remaining
        /// leading is distributed after the line. It is ignored for the default and uniform line spacing methods.
        /// The leading that is available to distribute before or after the line depends on the values of the height and
        /// baseline parameters.
        /// </summary>
        public float leadingBefore;

        /// <summary>
        /// Specify whether DWRITE_FONT_METRICS::lineGap value should be part of the line metrics.
        /// </summary>
        public DWRITE_FONT_LINE_GAP_USAGE fontLineGapUsage;
    }

    public enum DWRITE_FONT_LINE_GAP_USAGE
    {
        /// <summary>
        /// The usage of the font line gap depends on the method used for text layout.
        /// </summary>
        DWRITE_FONT_LINE_GAP_USAGE_DEFAULT,

        /// <summary>
        /// The font line gap is excluded from line spacing
        /// </summary>
        DWRITE_FONT_LINE_GAP_USAGE_DISABLED,

        /// <summary>
        /// The font line gap is included in line spacing
        /// </summary>
        DWRITE_FONT_LINE_GAP_USAGE_ENABLED
    }

    public enum DWRITE_AUTOMATIC_FONT_AXES
    {
        /// <summary>
        /// No axes are automatically applied.
        /// </summary>
        DWRITE_AUTOMATIC_FONT_AXES_NONE = 0x0000,

        /// <summary>
        /// Automatically pick an appropriate optical value based on the font size (via SetFontSize) when no value is
        /// specified via DWRITE_FONT_AXIS_TAG_OPTICAL_SIZE. Callers can still explicitly apply the 'opsz' value over
        /// text ranges via SetFontAxisValues, which take priority.
        /// </summary>
        DWRITE_AUTOMATIC_FONT_AXES_OPTICAL_SIZE = 0x0001,
    }

    public enum DWRITE_FONT_SOURCE_TYPE
    {
        /// <summary>
        /// The font source is unknown or is not any of the other defined font source types.
        /// </summary>
        DWRITE_FONT_SOURCE_TYPE_UNKNOWN,

        /// <summary>
        /// The font source is a font file, which is installed for all users on the device.
        /// </summary>
        DWRITE_FONT_SOURCE_TYPE_PER_MACHINE,

        /// <summary>
        /// The font source is a font file, which is installed for the current user.
        /// </summary>
        DWRITE_FONT_SOURCE_TYPE_PER_USER,

        /// <summary>
        /// The font source is an APPX package, which includes one or more font files.
        /// The font source name is the full name of the package.
        /// </summary>
        DWRITE_FONT_SOURCE_TYPE_APPX_PACKAGE,

        /// <summary>
        /// The font source is a font provider for downloadable fonts.
        /// </summary>
        DWRITE_FONT_SOURCE_TYPE_REMOTE_FONT_PROVIDER
    }

    public enum DWRITE_HRESULT : int
    {
        //
        // MessageId: DWRITE_E_FILEFORMAT
        //
        // MessageText:
        //
        // Indicates an error in an input file such as a font file.
        //
        DWRITE_E_FILEFORMAT = unchecked((int)0x88985000L),

        //
        // MessageId: DWRITE_E_UNEXPECTED
        //
        // MessageText:
        //
        // Indicates an error originating in DirectWrite code, which is not expected to occur but is safe to recover from.
        //
        DWRITE_E_UNEXPECTED = unchecked((int)0x88985001L),

        //
        // MessageId: DWRITE_E_NOFONT
        //
        // MessageText:
        //
        // Indicates the specified font does not exist.
        //
        DWRITE_E_NOFONT = unchecked((int)0x88985002L),

        //
        // MessageId: DWRITE_E_FILENOTFOUND
        //
        // MessageText:
        //
        // A font file could not be opened because the file, directory, network location, drive, or other storage location does not exist or is unavailable.
        //
        DWRITE_E_FILENOTFOUND = unchecked((int)0x88985003L),

        //
        // MessageId: DWRITE_E_FILEACCESS
        //
        // MessageText:
        //
        // A font file exists but could not be opened due to access denied, sharing violation, or similar error.
        //
        DWRITE_E_FILEACCESS = unchecked((int)0x88985004L),

        //
        // MessageId: DWRITE_E_FONTCOLLECTIONOBSOLETE
        //
        // MessageText:
        //
        // A font collection is obsolete due to changes in the system.
        //
        DWRITE_E_FONTCOLLECTIONOBSOLETE = unchecked((int)0x88985005L),

        //
        // MessageId: DWRITE_E_ALREADYREGISTERED
        //
        // MessageText:
        //
        // The given interface is already registered.
        //
        DWRITE_E_ALREADYREGISTERED = unchecked((int)0x88985006L),

        //
        // MessageId: DWRITE_E_CACHEFORMAT
        //
        // MessageText:
        //
        // The font cache contains invalid data.
        //
        DWRITE_E_CACHEFORMAT = unchecked((int)0x88985007L),

        //
        // MessageId: DWRITE_E_CACHEVERSION
        //
        // MessageText:
        //
        // A font cache file corresponds to a different version of DirectWrite.
        //
        DWRITE_E_CACHEVERSION = unchecked((int)0x88985008L),

        //
        // MessageId: DWRITE_E_UNSUPPORTEDOPERATION
        //
        // MessageText:
        //
        // The operation is not supported for this type of font.
        //
        DWRITE_E_UNSUPPORTEDOPERATION = unchecked((int)0x88985009L),

        //
        // MessageId: DWRITE_E_TEXTRENDERERINCOMPATIBLE
        //
        // MessageText:
        //
        // The version of the text renderer interface is not compatible.
        //
        DWRITE_E_TEXTRENDERERINCOMPATIBLE = unchecked((int)0x8898500AL),

        //
        // MessageId: DWRITE_E_FLOWDIRECTIONCONFLICTS
        //
        // MessageText:
        //
        // The flow direction conflicts with the reading direction. They must be perpendicular to each other.
        //
        DWRITE_E_FLOWDIRECTIONCONFLICTS = unchecked((int)0x8898500BL),

        //
        // MessageId: DWRITE_E_NOCOLOR
        //
        // MessageText:
        //
        // The font or glyph run does not contain any colored glyphs.
        //
        DWRITE_E_NOCOLOR = unchecked((int)0x8898500CL),

        //
        // MessageId: DWRITE_E_REMOTEFONT
        //
        // MessageText:
        //
        // A font resource could not be accessed because it is remote.
        //
        DWRITE_E_REMOTEFONT = unchecked((int)0x8898500DL),

        //
        // MessageId: DWRITE_E_DOWNLOADCANCELLED
        //
        // MessageText:
        //
        // A font download was canceled.
        //
        DWRITE_E_DOWNLOADCANCELLED = unchecked((int)0x8898500EL),

        //
        // MessageId: DWRITE_E_DOWNLOADFAILED
        //
        // MessageText:
        //
        // A font download failed.
        //
        DWRITE_E_DOWNLOADFAILED = unchecked((int)0x8898500FL),

        //
        // MessageId: DWRITE_E_TOOMANYDOWNLOADS
        //
        // MessageText:
        //
        // A font download request was not added or a download failed because there are too many active downloads.
        //
        DWRITE_E_TOOMANYDOWNLOADS = unchecked((int)0x88985010L)
    }
}
