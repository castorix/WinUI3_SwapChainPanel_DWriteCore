using System;
using System.Runtime.InteropServices;
using System.Threading;

using GlobalStructures;
using static GlobalStructures.GlobalTools;
using Direct2D;
using static Direct2D.D2DTools;
using DWrite;
using WIC;
using DXGI;
using System.Diagnostics;

// Adapted from : https://github.com/microsoft/Windows-universal-samples/blob/main/Samples/DWriteColorGlyph/cpp/CustomTextRenderer.cpp

public class CustomTextRenderer : IDWriteTextRenderer, IDisposable
{
    private readonly ID2D1DeviceContext4 m_pD2DDeviceContext;
    private readonly IDWriteFactory4 m_pDwriteFactory;

    private ID2D1SolidColorBrush m_pOutlineBrush;
    private ID2D1SolidColorBrush m_pTempBrush;
    private D2D1_COLOR_F m_DefaultColor;
    private IntPtr m_hWndParent = IntPtr.Zero;

    // DWriteCore only
    private IDWriteBitmapRenderTarget3 m_pBitmapRenderTarget3 = null;
    public bool IsDWriteCore = false;

    private IDWriteRenderingParams m_pRenderingParams = null;

    public CustomTextRenderer(IDWriteFactory4 pDWriteFactory, ID2D1DeviceContext4 pD2DDeviceContext, D2D1_COLOR_F DefaultColor, IntPtr hWnd)
    {
        HRESULT hr = HRESULT.S_OK;
        m_pDwriteFactory = pDWriteFactory;
        m_pD2DDeviceContext = pD2DDeviceContext;
        m_DefaultColor = DefaultColor;
        m_hWndParent = hWnd;

        hr = m_pDwriteFactory.CreateRenderingParams(out m_pRenderingParams);
        IDWriteGdiInterop pGdiInterop = null;
        hr = m_pDwriteFactory.GetGdiInterop(out pGdiInterop);
        if (hr == HRESULT.S_OK)
        {
            int nDisplayWidth = Microsoft.UI.Windowing.DisplayArea.Primary.WorkArea.Width;
            int nDisplayHeight = Microsoft.UI.Windowing.DisplayArea.Primary.WorkArea.Height;

            //int nDisplayWidth = 800;
            //int nDisplayHeight = 200;

            IDWriteBitmapRenderTarget pBitmapRenderTarget;
            hr = pGdiInterop.CreateBitmapRenderTarget(IntPtr.Zero, nDisplayWidth, nDisplayHeight, out pBitmapRenderTarget);
            if (hr == HRESULT.S_OK)
            {
                try
                {
                    m_pBitmapRenderTarget3 = (IDWriteBitmapRenderTarget3)pBitmapRenderTarget;
                    IsDWriteCore = true;
                }
                catch (System.Exception)
                {
                    IsDWriteCore = false;
                    SafeRelease(ref pBitmapRenderTarget);
                }
            }
            SafeRelease(ref pGdiInterop);
        }

        hr = m_pD2DDeviceContext.CreateSolidColorBrush(m_DefaultColor, BrushProperties(), out m_pOutlineBrush);
        hr = m_pD2DDeviceContext.CreateSolidColorBrush(new ColorF(ColorF.Enum.Black, 1.0f), BrushProperties(), out m_pTempBrush);
    }
 
    public HRESULT DrawGlyphRun(IntPtr clientDrawingContext, float baselineOriginX, float baselineOriginY, DWrite.DWRITE_MEASURING_MODE measuringMode,
        ref DWRITE_GLYPH_RUN glyphRun, IntPtr glyphRunDescription, IntPtr clientDrawingEffect)
    {
        HRESULT hr = (HRESULT)DWRITE_HRESULT.DWRITE_E_NOCOLOR;
        DWrite.D2D1_POINT_2F baselineOrigin = new DWrite.D2D1_POINT_2F { x = baselineOriginX, y = baselineOriginY };

        DWrite.DWRITE_GLYPH_IMAGE_FORMATS supportedFormats = (DWrite.DWRITE_GLYPH_IMAGE_FORMATS.DWRITE_GLYPH_IMAGE_FORMATS_TRUETYPE |
                                       DWrite.DWRITE_GLYPH_IMAGE_FORMATS.DWRITE_GLYPH_IMAGE_FORMATS_CFF |
                                       DWrite.DWRITE_GLYPH_IMAGE_FORMATS.DWRITE_GLYPH_IMAGE_FORMATS_COLR |
                                       DWrite.DWRITE_GLYPH_IMAGE_FORMATS.DWRITE_GLYPH_IMAGE_FORMATS_SVG |
                                       DWrite.DWRITE_GLYPH_IMAGE_FORMATS.DWRITE_GLYPH_IMAGE_FORMATS_PNG |
                                       DWrite.DWRITE_GLYPH_IMAGE_FORMATS.DWRITE_GLYPH_IMAGE_FORMATS_JPEG |
                                       DWrite.DWRITE_GLYPH_IMAGE_FORMATS.DWRITE_GLYPH_IMAGE_FORMATS_TIFF |
                                       DWrite.DWRITE_GLYPH_IMAGE_FORMATS.DWRITE_GLYPH_IMAGE_FORMATS_PREMULTIPLIED_B8G8R8A8);

        hr = m_pDwriteFactory.TranslateColorGlyphRun4(baselineOrigin, ref glyphRun, glyphRunDescription, supportedFormats,
           measuringMode, IntPtr.Zero, 0, out IDWriteColorGlyphRunEnumerator1 glyphRunEnumerator);

        if (hr == (HRESULT)DWRITE_HRESULT.DWRITE_E_NOCOLOR)
        {
            if (!IsDWriteCore)
                m_pD2DDeviceContext.DrawGlyphRun(Point2F(baselineOrigin.x, baselineOrigin.y), glyphRun, glyphRunDescription, m_pOutlineBrush, (Direct2D.DWRITE_MEASURING_MODE)measuringMode);
            else
            {               
                RECT blackBoxRect;
                var foregroundColor = m_DefaultColor;
                var nOldAntialiasMode = m_pBitmapRenderTarget3.GetTextAntialiasMode();
                hr = m_pBitmapRenderTarget3.SetTextAntialiasMode(DWRITE_TEXT_ANTIALIAS_MODE.DWRITE_TEXT_ANTIALIAS_MODE_GRAYSCALE);
                hr = m_pBitmapRenderTarget3.DrawGlyphRun(baselineOrigin.x, baselineOrigin.y, measuringMode, glyphRun,
                    m_pRenderingParams, D2D1ColorFToCOLORREF(foregroundColor), out blackBoxRect);
                hr = m_pBitmapRenderTarget3.SetTextAntialiasMode(nOldAntialiasMode);

                //hr = m_pBitmapRenderTarget3.DrawGlyphRunWithColorSupport(baselineOrigin.x, baselineOrigin.y, measuringMode,
                //    glyphRun, m_RenderingParams, D2D1ColorFToCOLORREF(foregroundColor), 0, out blackBoxRect);

            }
        }
        else
        {             
            for (; ;)
            {
                bool bHaveRun;
                hr = glyphRunEnumerator.MoveNext(out bHaveRun);
                if (!bHaveRun)
                    break;

                IntPtr pColorRun1 = IntPtr.Zero;
                hr = glyphRunEnumerator.GetCurrentRun1(out pColorRun1);
                DWRITE_COLOR_GLYPH_RUN1 colorRun1 = new DWRITE_COLOR_GLYPH_RUN1();
                colorRun1 = (DWRITE_COLOR_GLYPH_RUN1)Marshal.PtrToStructure(pColorRun1, typeof(DWRITE_COLOR_GLYPH_RUN1));

                DWrite.D2D1_POINT_2F currentBaselineOrigin = new DWrite.D2D1_POINT_2F { x = colorRun1.baselineOriginX, y = colorRun1.baselineOriginY };

                if (!IsDWriteCore)
                {
                    switch (colorRun1.glyphImageFormat)
                    {
                        case DWrite.DWRITE_GLYPH_IMAGE_FORMATS.DWRITE_GLYPH_IMAGE_FORMATS_PNG:
                        case DWrite.DWRITE_GLYPH_IMAGE_FORMATS.DWRITE_GLYPH_IMAGE_FORMATS_JPEG:
                        case DWrite.DWRITE_GLYPH_IMAGE_FORMATS.DWRITE_GLYPH_IMAGE_FORMATS_TIFF:
                        case DWrite.DWRITE_GLYPH_IMAGE_FORMATS.DWRITE_GLYPH_IMAGE_FORMATS_PREMULTIPLIED_B8G8R8A8:
                            {
                                m_pD2DDeviceContext.DrawColorBitmapGlyphRun((Direct2D.DWRITE_GLYPH_IMAGE_FORMATS)colorRun1.glyphImageFormat, Point2F(currentBaselineOrigin.x, currentBaselineOrigin.y), colorRun1.glyphRun, (Direct2D.DWRITE_MEASURING_MODE)measuringMode);
                            }
                            break;
                        case DWrite.DWRITE_GLYPH_IMAGE_FORMATS.DWRITE_GLYPH_IMAGE_FORMATS_SVG:
                            {
                                m_pD2DDeviceContext.DrawSvgGlyphRun(Point2F(currentBaselineOrigin.x, currentBaselineOrigin.y), colorRun1.glyphRun, m_pOutlineBrush, null, 0, (Direct2D.DWRITE_MEASURING_MODE)measuringMode);
                            }
                            break;
                        case DWrite.DWRITE_GLYPH_IMAGE_FORMATS.DWRITE_GLYPH_IMAGE_FORMATS_TRUETYPE:
                        case DWrite.DWRITE_GLYPH_IMAGE_FORMATS.DWRITE_GLYPH_IMAGE_FORMATS_CFF:
                        case DWrite.DWRITE_GLYPH_IMAGE_FORMATS.DWRITE_GLYPH_IMAGE_FORMATS_COLR:
                        default:
                            {
                                ID2D1Brush layerBrush;
                                if (colorRun1.paletteIndex == 0xFFFF)
                                {
                                    // This run uses the current text color.
                                    layerBrush = m_pOutlineBrush;
                                }
                                else
                                {
                                    // This run specifies its own color.
                                    D2D1_COLOR_F col = new D2D1_COLOR_F();
                                    col.a = colorRun1.runColor.a;
                                    col.r = colorRun1.runColor.r;
                                    col.g = colorRun1.runColor.g;
                                    col.b = colorRun1.runColor.b;
                                    m_pTempBrush.SetColor(col);
                                    layerBrush = m_pTempBrush;
                                }
                                m_pD2DDeviceContext.DrawGlyphRun(Point2F(currentBaselineOrigin.x, currentBaselineOrigin.y), colorRun1.glyphRun, glyphRunDescription, layerBrush, (Direct2D.DWRITE_MEASURING_MODE)measuringMode);
                            }
                            break;
                    }
                }
                else
                {
                    var foregroundColor = new ColorF(colorRun1.runColor.r, colorRun1.runColor.g, colorRun1.runColor.b, colorRun1.runColor.a);
                    RECT blackBoxRect;
                    var nOldAntialiasMode = m_pBitmapRenderTarget3.GetTextAntialiasMode();
                    hr = m_pBitmapRenderTarget3.SetTextAntialiasMode(DWRITE_TEXT_ANTIALIAS_MODE.DWRITE_TEXT_ANTIALIAS_MODE_GRAYSCALE);
                    //if (colorRun1.paletteIndex != 0)                  
                    hr = m_pBitmapRenderTarget3.DrawGlyphRunWithColorSupport(currentBaselineOrigin.x, currentBaselineOrigin.y, measuringMode,
                        colorRun1.glyphRun, m_pRenderingParams, D2D1ColorFToCOLORREF(foregroundColor), 0, out blackBoxRect);
                    hr = m_pBitmapRenderTarget3.SetTextAntialiasMode(nOldAntialiasMode);
                }
            }
        }
        return hr;
    }

    public HRESULT DrawBitmapRenderTarget(Direct2D.D2D1_POINT_2F pt)
    {
        HRESULT hr = HRESULT.S_OK;
        IntPtr pBitmapData = IntPtr.Zero;
        DWRITE_BITMAP_DATA_BGRA32 BitmapData = new DWRITE_BITMAP_DATA_BGRA32();
        hr = m_pBitmapRenderTarget3.GetBitmapData(out BitmapData);
        if (hr == HRESULT.S_OK)
        {
            Direct2D.D2D1_SIZE_U sizeBitmapU = new Direct2D.D2D1_SIZE_U();
            sizeBitmapU.width = BitmapData.width;
            sizeBitmapU.height = BitmapData.height;
            uint nPitch = (sizeBitmapU.width * 4);

            D2D1_BITMAP_PROPERTIES1 bitmapProperties1 = new D2D1_BITMAP_PROPERTIES1();
            bitmapProperties1.pixelFormat = D2DTools.PixelFormat(DXGI.DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM, D2D1_ALPHA_MODE.D2D1_ALPHA_MODE_PREMULTIPLIED);
            bitmapProperties1.dpiX = 96;
            bitmapProperties1.dpiY = 96;
            bitmapProperties1.bitmapOptions = D2D1_BITMAP_OPTIONS.D2D1_BITMAP_OPTIONS_NONE;

            ID2D1Bitmap1 pBitmap1;
            hr = m_pD2DDeviceContext.CreateBitmap(sizeBitmapU, BitmapData.pixels, nPitch, ref bitmapProperties1, out pBitmap1);

            if (hr == HRESULT.S_OK)
            {
                D2D1_SIZE_F size = m_pD2DDeviceContext.GetSize();
                D2D1_RECT_F imageRectangle = new D2D1_RECT_F();
                imageRectangle.left = 0.0f;
                imageRectangle.top = 0.0f;
                imageRectangle.right = size.width;
                imageRectangle.bottom = size.height;
                //imageRectangle.right = Microsoft.UI.Windowing.DisplayArea.Primary.WorkArea.Width;
                //imageRectangle.bottom = Microsoft.UI.Windowing.DisplayArea.Primary.WorkArea.Height;

                // Quality loss with DWriteCore (bigger outline)
                // while pBitmap1 seems OK if it is saved to file (SaveD2D1BitmapToFile from other sources)
                // or displayed to screen DC from GetMemoryDC with BitBlt

                //m_pD2DDeviceContext.Clear(null);
                m_pD2DDeviceContext.DrawImage(pBitmap1, pt, ref imageRectangle, D2D1_INTERPOLATION_MODE.D2D1_INTERPOLATION_MODE_LINEAR, D2D1_COMPOSITE_MODE.D2D1_COMPOSITE_MODE_SOURCE_OVER);
    
                SafeRelease(ref pBitmap1);
            }
        }
        return hr;
    }

    public HRESULT DrawUnderline(IntPtr clientDrawingContext, float baselineOriginX, float baselineOriginY, DWRITE_UNDERLINE underline, IntPtr clientDrawingEffect)
    {
        return HRESULT.S_OK;
    }

    public HRESULT DrawStrikethrough(IntPtr clientDrawingContext, float baselineOriginX, float baselineOriginY, DWRITE_STRIKETHROUGH strikethrough, IntPtr clientDrawingEffect)
    {
        return HRESULT.S_OK;
    }

    public HRESULT DrawInlineObject(IntPtr clientDrawingContext, float originX, float originY, IDWriteInlineObject inlineObject, bool isSideways, bool isRightToLeft, IntPtr clientDrawingEffect)
    {       
        return HRESULT.S_OK;
    }

    public HRESULT IsPixelSnappingDisabled(IntPtr clientDrawingContext, out bool isDisabled)
    {
        isDisabled = false;
        return HRESULT.S_OK;
    }

    public HRESULT GetCurrentTransform(IntPtr clientDrawingContext, out DWRITE_MATRIX transform)
    {
        D2D1_MATRIX_3X2_F_STRUCT d2dTransform = new D2D1_MATRIX_3X2_F_STRUCT();
        m_pD2DDeviceContext.GetTransform(out d2dTransform);
        transform.m11 = d2dTransform._11;
        transform.m12 = d2dTransform._12;
        transform.m21 = d2dTransform._21;
        transform.m22 = d2dTransform._22;
        transform.dx = d2dTransform._31;
        transform.dy = d2dTransform._32;   
        return HRESULT.S_OK;
    }

    public HRESULT GetPixelsPerDip(IntPtr clientDrawingContext, out float pixelsPerDip)
    {
        //if (m_pBitmapRenderTarget3 != null)
        //    pixelsPerDip = m_pBitmapRenderTarget3.GetPixelsPerDip();
        //else
        //{
        //    m_pD2DDeviceContext.GetDpi(out float nDpiX, out _);
        //    pixelsPerDip = nDpiX / 96.0f;
        //}

        uint nDPI = GetDpiForWindow(m_hWndParent);
        pixelsPerDip = nDPI / 96.0f;

        return HRESULT.S_OK;
    }

    private static uint D2D1ColorFToCOLORREF(D2D1_COLOR_F color)
    {
        byte red = (byte)(color.r * 255);
        byte green = (byte)(color.g * 255);
        byte blue = (byte)(color.b * 255);
        return (uint)(red | (green << 8) | (blue << 16));
    }

    public void Dispose()
    {
        if (m_pOutlineBrush != null)
            SafeRelease(ref m_pOutlineBrush);

        if (m_pTempBrush != null)
            SafeRelease(ref m_pTempBrush);

        if (m_pBitmapRenderTarget3 != null)
            SafeRelease(ref m_pBitmapRenderTarget3);        
    }    
}