using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;

using GlobalStructures;
using static GlobalStructures.GlobalTools;
using Direct2D;
using static Direct2D.D2DTools;
using DXGI;
using static DXGI.DXGITools;
using WIC;
using static WIC.WICTools;
using DWrite;
using static DWrite.DWriteTools;
using System.Text;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI3_SwapChainPanel_DWriteCore
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int GetUserDefaultLocaleName(StringBuilder lpLocaleName, int cchLocaleName);

        public const int LOCALE_NAME_MAX_LENGTH = 85;

        private IntPtr hWndMain = IntPtr.Zero;

        ID2D1Factory m_pD2DFactory = null;
        ID2D1Factory1 m_pD2DFactory1 = null;
        IWICImagingFactory m_pWICImagingFactory = null;
        IWICImagingFactory2 m_pWICImagingFactory2 = null;

        IntPtr m_pD3D11DevicePtr = IntPtr.Zero; // Used in CreateSwapChain
        ID3D11DeviceContext m_pD3D11DeviceContext = null; // Released in Clean : not used       
        IDXGIDevice1 m_pDXGIDevice = null;
        ID2D1DeviceContext m_pD2DDeviceContext = null;
        ID2D1DeviceContext3 m_pD2DDeviceContext3 = null;

        IDXGISwapChain1 m_pDXGISwapChain1 = null;
        ID2D1Bitmap1 m_pD2DTargetBitmap = null;

        ID2D1SolidColorBrush m_pD2DMainBrush = null;
        ID2D1SolidColorBrush m_pD2DSolidColorBrushRed = null;
        ID2D1SolidColorBrush m_pD2DSolidColorBrushGreen = null;
        ID2D1SolidColorBrush m_pD2DSolidColorBrushBlue = null;
        ID2D1SolidColorBrush m_pD2DSolidColorBrushWhite = null;
        ID2D1SolidColorBrush m_pD2DSolidColorBrushPink = null;
        ID2D1LinearGradientBrush m_pD2DLinearGradientBrush1 = null;
        ID2D1LinearGradientBrush m_pD2DLinearGradientBrush2 = null;
        ID2D1Bitmap m_pD2DBitmap1 = null;
        ID2D1BitmapBrush m_pD2DBitmapBrush1 = null;
        ID2D1Bitmap m_pD2DBitmap2 = null;

        IDWriteFactory7 m_pDWriteFactory7 = null;
        ID2D1Geometry m_pD2DGeometry1 = null;
        ID2D1Geometry m_pD2DGeometry2 = null;
        ID2D1Geometry m_pD2DGeometry3 = null;
        ID2D1Geometry m_pD2DGeometry4 = null;
        ID2D1Geometry m_pD2DGeometry5 = null;
        ID2D1Geometry m_pD2DGeometry6 = null;
        ID2D1Geometry m_pD2DGeometry7 = null;
        float m_nComputedHeight1 = 0, m_nComputedHeight2 = 0, m_nComputedHeight3 = 0, m_nComputedHeight4 = 0,
            m_nComputedHeight5 = 0, m_nComputedHeight6 = 0, m_nComputedHeight7 = 0;
        IDWriteTextLayout m_pTextLayout = null;

        CustomTextRenderer m_pCTR = null;

        public System.Collections.ObjectModel.ObservableCollection<Font> CustomFonts = new System.Collections.ObjectModel.ObservableCollection<Font>();
        public System.Collections.ObjectModel.ObservableCollection<Font> SystemFonts;// = new System.Collections.ObjectModel.ObservableCollection<Font>();

        public double m_nXPos, m_nYPos, m_nWidth, m_nHeight = 0;

        bool m_bUseDirectWrite = false;   

        public MainWindow()
        {  
            this.InitializeComponent();      

            hWndMain = WinRT.Interop.WindowNative.GetWindowHandle(this);
            this.Title = "WinUI 3 - Test DWriteCore";
            Application.Current.Resources["ComboBoxBackgroundPointerOver"] = new SolidColorBrush(Microsoft.UI.Colors.RoyalBlue);
            Application.Current.Resources["ComboBoxItemBackgroundSelected"] = new SolidColorBrush(Microsoft.UI.Colors.RoyalBlue);
            Application.Current.Resources["ComboBoxItemBackgroundPointerOver"] = new SolidColorBrush(Microsoft.UI.Colors.BlueViolet);
            double nDisplayWidth = (Microsoft.UI.Windowing.DisplayArea.Primary.WorkArea.Width);
            double nDisplayHeight = (Microsoft.UI.Windowing.DisplayArea.Primary.WorkArea.Height);
            m_nWidth = 1280;
            m_nHeight = 700;
            m_nXPos = (nDisplayWidth - m_nWidth) / 2;
            m_nYPos = (nDisplayHeight - m_nHeight) / 2;
            UpdateWindowSize(hWndMain);
            m_pWICImagingFactory = (IWICImagingFactory)Activator.CreateInstance(Type.GetTypeFromCLSID(WICTools.CLSID_WICImagingFactory));
            m_pWICImagingFactory2 = (IWICImagingFactory2)m_pWICImagingFactory;
            HRESULT hr = CreateD2D1Factory();
            if (SUCCEEDED(hr))
            {                
                IntPtr pDWriteFactoryPtr = IntPtr.Zero;
                if (m_bUseDirectWrite)
                    hr = DWriteCreateFactory(DWrite.DWRITE_FACTORY_TYPE.DWRITE_FACTORY_TYPE_SHARED, ref CLSID_DWriteFactory7, out pDWriteFactoryPtr);
                else
                    hr = DWriteCoreCreateFactory(DWrite.DWRITE_FACTORY_TYPE.DWRITE_FACTORY_TYPE_SHARED, ref CLSID_DWriteFactory7, out pDWriteFactoryPtr);

                if (SUCCEEDED(hr))
                {                   
                    m_pDWriteFactory7 = Marshal.GetObjectForIUnknown(pDWriteFactoryPtr) as IDWriteFactory7;
                    if (pDWriteFactoryPtr != IntPtr.Zero)
                        Marshal.Release(pDWriteFactoryPtr);
                    
                    string sExePath = AppContext.BaseDirectory;                 

                    var pFontCollectionLoader = new FontCollectionLoader();
                    hr = m_pDWriteFactory7.RegisterFontCollectionLoader(pFontCollectionLoader);
                    //hr = m_pDWriteFactory7.RegisterFontFileLoader(pFontCollectionLoader);      
                    
                    string sFullPath = System.IO.Path.Combine(sExePath, "Assets");
                    var pFontCollectionKey = Marshal.StringToHGlobalUni(sFullPath);                   
                    IDWriteFontCollection pFontCollection = null;
                    hr = m_pDWriteFactory7.CreateCustomFontCollection(pFontCollectionLoader, pFontCollectionKey, (sFullPath.Length+1)*2, out pFontCollection);
                    if (SUCCEEDED(hr))
                    {
                        Marshal.FreeHGlobal(pFontCollectionKey);
                        IDWriteFontCollection2 pFontCollection2 = (IDWriteFontCollection2)pFontCollection;
                        if (pFontCollection2 != null)
                        {
                            IDWriteFontSet1 pFontSet1;
                            hr = pFontCollection2.GetFontSet(out pFontSet1);
                            if (SUCCEEDED(hr))
                            {
                                hr = LoadFonts(pFontSet1, CustomFonts, true);
                                SafeRelease(ref pFontSet1);
                            }                           
                        }

                        //IDWriteFontCollection pFontCollection = null;
                        //hr = m_pDWriteFactory7.GetSystemFontCollection(out pFontCollection);
                        //uint nFonts = pFontCollection.GetFontFamilyCount();

                        IDWriteFontSet2 pFontSet2 = null;
                        hr = m_pDWriteFactory7.GetSystemFontSet7(false, out pFontSet2);
                        if (SUCCEEDED(hr))
                        {
                            var unorderedSystemFonts = new System.Collections.ObjectModel.ObservableCollection<Font>();
                            hr = LoadFonts(pFontSet2, unorderedSystemFonts, false);
                            SystemFonts = new System.Collections.ObjectModel.ObservableCollection<Font>(unorderedSystemFonts.OrderBy(x => x.Name));
                            unorderedSystemFonts.Clear();
                            SafeRelease(ref pFontSet2);
                        }
                        
                        string sPathFont = System.IO.Path.Combine(sExePath, "Assets\\Lovely Home.ttf");
                        hr = CreateDWriteTextGeometry("This is a text with slow animated gradient", sPathFont, 70.0f, true, false, false, out m_pD2DGeometry1, out m_nComputedHeight1);

                        //string sPathFont = "E:\\test\\NotoNastaliqUrdu-Regular.ttf";
                        //hr = CreateTextGeometry("بچے اپنے فیصلے چاہتے پہلے", sPathFont, 60.0f, false, false, out m_pD2DGeometry1, out m_nComputedHeight1);
                        //hr = CreateDWriteTextGeometry("بچے اپنے فیصلے چاہتے پہلے", sPathFont, 60.0f, true, false, false, out m_pD2DGeometry1, out m_nComputedHeight1);

                        sPathFont = System.IO.Path.Combine(sExePath, "Assets\\Lemon Shake.ttf");
                        CreateDWriteTextGeometry("This is a text with shadow", sPathFont, 80.0f, true, false, false, out m_pD2DGeometry2, out m_nComputedHeight2);

                        sPathFont = System.IO.Path.Combine(sExePath, "Assets\\Play Story.otf");
                        CreateDWriteTextGeometry("This is a text with turbulence", sPathFont, 60.0f, true, false, false, out m_pD2DGeometry3, out m_nComputedHeight3);

                        sPathFont = System.IO.Path.Combine(sExePath, "Assets\\Simplisicky Fill.ttf");
                        CreateDWriteTextGeometry("This is a text with glowing", sPathFont, 80.0f, true, false, false, out m_pD2DGeometry4, out m_nComputedHeight4);

                        sPathFont = System.IO.Path.Combine(sExePath, "Assets\\Bloody Scene.otf");
                        CreateDWriteTextGeometry("This is a text with Bitmap brush", sPathFont, 70.0f, true, false, false, out m_pD2DGeometry5, out m_nComputedHeight5);

                        sPathFont = System.IO.Path.Combine(sExePath, "Assets\\Love Craft.ttf");
                        CreateDWriteTextGeometry("This is a sample text", sPathFont, 60.0f, true, false, false, out m_pD2DGeometry6, out m_nComputedHeight6);

                        sPathFont = System.IO.Path.Combine(sExePath, "Assets\\MagicSchoolOne.ttf");
                        CreateDWriteTextGeometry("This is a collapsing text", sPathFont, 90.0f, true, false, false, out m_pD2DGeometry7, out m_nComputedHeight7);

                        //string sFontName = "Segoe UI Emoji";
                        //string sFontName = "Tolkien";

                        //string sString = "This is a text from Layout";
                        string sString = "";
                        for (int nCode = 0x1F980; nCode <= 0x1F980 + 15; nCode++)
                            sString += char.ConvertFromUtf32(nCode);

                        // crashes in DWriteCore with nStringLength = sString.Length
                        //sString = "بچے اپنے فیصلے چاہتے پہلے";                    

                        //sString = "بچے اپنے فیصلے چاہتے پہلےبچے اپنے فیصلے چاہتے پہلے";
                       
                        uint nStringLength = (uint)(sString.Length);
                        IDWriteTextFormat3 pTextFormat3 = null;
                        IDWriteTextFormat pTextFormat = null;
                        //hr = m_pDWriteFactory7.CreateTextFormat(sFontName, pFontCollection2,
                        hr = m_pDWriteFactory7.CreateTextFormat6("Segoe UI Emoji", null, IntPtr.Zero, 0, 30.0f,
                            //hr = m_pDWriteFactory7.CreateTextFormat("E:\\test\\NotoNastaliqUrdu-Regular.ttf", null,
                            //hr = m_pDWriteFactory7.CreateTextFormat("Noto Nastaliq Urdu", null,
                            //   DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_NORMAL, DWRITE_FONT_STYLE.DWRITE_FONT_STYLE_NORMAL, DWRITE_FONT_STRETCH.DWRITE_FONT_STRETCH_NORMAL,
                            //   30, 
                            //   "",
                            //   //"ur-PK",
                            //   out pTextFormat);

                            //DWRITE_FONT_AXIS_VALUE[] axes = null;
                            //hr = m_pDWriteFactory7.CreateTextFormat6("Noto Nastaliq Urdu", null, IntPtr.Zero, 0, 30.0f,
                            //"ur-PK",         // locale
                            "",         // locale
                            out pTextFormat3);

                        if (m_bUseDirectWrite || (!ContainsStrongArabicCharacters(sString) && !m_bUseDirectWrite))
                        {
                            if (SUCCEEDED(hr))
                            {
                                hr = pTextFormat3.SetTextAlignment(DWRITE_TEXT_ALIGNMENT.DWRITE_TEXT_ALIGNMENT_CENTER);
                                hr = pTextFormat3.SetReadingDirection(DWRITE_READING_DIRECTION.DWRITE_READING_DIRECTION_RIGHT_TO_LEFT);
                                //hr = pTextFormat.SetFlowDirection(DWRITE_FLOW_DIRECTION.DWRITE_FLOW_DIRECTION_TOP_TO_BOTTOM);                       

                                hr = pTextFormat3.SetAutomaticFontAxes(DWRITE_AUTOMATIC_FONT_AXES.DWRITE_AUTOMATIC_FONT_AXES_OPTICAL_SIZE);

                                hr = m_pDWriteFactory7.CreateTextLayout(sString, nStringLength, pTextFormat3, 800, 60, out m_pTextLayout);
                                if (SUCCEEDED(hr))
                                {
                                    if (m_pTextLayout != null)
                                    {
                                        IDWriteTypography pTypography = null;
                                        hr = m_pDWriteFactory7.CreateTypography(out pTypography);
                                        if (SUCCEEDED(hr))
                                        {
                                            DWRITE_FONT_FEATURE ff = new DWRITE_FONT_FEATURE(
                                                DWRITE_FONT_FEATURE_TAG.DWRITE_FONT_FEATURE_TAG_MARK_POSITIONING, 1);
                                            hr = pTypography.AddFontFeature(ff);
                                            if (SUCCEEDED(hr))
                                            {
                                                DWRITE_TEXT_RANGE tr = new DWRITE_TEXT_RANGE(0, (uint)nStringLength);
                                                hr = m_pTextLayout.SetTypography(pTypography, tr);
                                            }
                                            SafeRelease(ref pTypography);
                                        }

                                        IDWriteTextLayout4 pTextLayout4 = (IDWriteTextLayout4)m_pTextLayout;

                                        //m_pTextLayout.SetLocaleName("ur-PK", new DWRITE_TEXT_RANGE
                                        //{
                                        //    startPosition = 0,
                                        //    length = (uint)sString.Length
                                        //});

                                        //var fontFamilyName = new StringBuilder(19);
                                        //var f = pTextLayout4.SetCharacterSpacing(10, 5, 1, new DWRITE_TEXT_RANGE());

                                        DWRITE_TEXT_METRICS textMetrics;
                                        // hr = -2003283967 0x88985001 DWRITE_E_UNEXPECTED
                                        //  -2003283957 0x8898500B   DWRITE_E_FLOWDIRECTIONCONFLICTS
                                        // Microsoft C++ exception:  ?? ::st_panic at memory location 0x000000729757B5A0.
                                        hr = pTextLayout4.GetMetrics(out textMetrics);                                        
                                    }

                                    //IDWriteTypography pTypography = null;
                                    //hr = m_pDWriteFactory7.CreateTypography(out pTypography);
                                    //if (SUCCEEDED(hr))
                                    //{
                                    //    DWRITE_FONT_FEATURE ff = new DWRITE_FONT_FEATURE (
                                    //        DWRITE_FONT_FEATURE_TAG.DWRITE_FONT_FEATURE_TAG_STYLISTIC_SET_7, 1);
                                    //    hr = pTypography.AddFontFeature(ff);
                                    //    if (SUCCEEDED(hr))
                                    //    {
                                    //        DWRITE_TEXT_RANGE tr = new DWRITE_TEXT_RANGE (0, (uint)nStringLength);
                                    //        hr = m_pTextLayout.SetTypography(pTypography, tr);
                                    //    }
                                    //    SafeRelease(ref pTypography);
                                    //}
                                }
                                SafeRelease(ref pTextFormat);
                            }
                            SafeRelease(ref pFontCollection);
                        }
                    }
                }
                
                ContentFrame.Navigate(typeof(MainPage));
                NavView.SelectedItem = NavView.MenuItems[0];

                hr = CreateDeviceContext();
                hr = CreateDeviceResources();
                hr = CreateSwapChain(IntPtr.Zero);
                if (SUCCEEDED(hr))
                {
                    hr = ConfigureSwapChain(hWndMain);
                    var pageInstance = ContentFrame.Content as MainPage;
                    ISwapChainPanelNative panelNative = WinRT.CastExtensions.As<ISwapChainPanelNative>(pageInstance.scp1);
                    hr = panelNative.SetSwapChain(m_pDXGISwapChain1);
                    pageInstance.scp1.SizeChanged += scp1_SizeChanged;
                }

                m_pCTR = new CustomTextRenderer((IDWriteFactory4)m_pDWriteFactory7, (ID2D1DeviceContext4)m_pD2DDeviceContext3, new ColorF(ColorF.Enum.Orange, 1.0f), hWndMain);

                CompositionTarget.Rendering += CompositionTarget_Rendering;
            }
            this.Closed += MainWindow_Closed;
        }

        int m_nTextEffect = (int)TEXT_EFFECT.GRADIENT;

        public enum TEXT_EFFECT : int
        {
            GRADIENT = 0,
            SHADOW = 1,
            TURBULENCE = 2,
            WAVES = 3,
            GLOWING = 4,
            BITMAP = 5,
            SCROLLING = 6,
            COLLAPSE = 7,
            POINT_DIFFUSE_LIGHTING = 8,
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItem is NavItem navItem)
            {
                switch (navItem.Tag)
                {
                    case "Gradient":
                        m_nTextEffect = (int)TEXT_EFFECT.GRADIENT;
                        break;
                    case "Shadow":
                        m_nTextEffect = (int)TEXT_EFFECT.SHADOW;
                        break;
                    case "Turbulence":
                        m_nTextEffect = (int)TEXT_EFFECT.TURBULENCE;
                        break;
                    case "Waves":
                        m_nTextEffect = (int)TEXT_EFFECT.WAVES;
                        break;
                    case "Glowing":
                        m_nTextEffect = (int)TEXT_EFFECT.GLOWING;
                        break;
                    case "Bitmap":
                        m_nTextEffect = (int)TEXT_EFFECT.BITMAP;
                        break;
                    case "Scrolling":
                        m_nTextEffect = (int)TEXT_EFFECT.SCROLLING;
                        break;
                    case "Collapse":
                        m_nTextEffect = (int)TEXT_EFFECT.COLLAPSE;
                        break;
                    case "Point-diffuse lighting":
                        m_nTextEffect = (int)TEXT_EFFECT.POINT_DIFFUSE_LIGHTING;
                        break;
                }
            }
        }

        private void UpdateWindowSize(IntPtr hWnd)
        {
            //uint nDPI = GetDpiForWindow(hWnd); 
            //double nScaleX = (double)nDPI / 96.0f;
            //double nScaleY = (double)nDPI / 96.0f;
            this.AppWindow.MoveAndResize(new Windows.Graphics.RectInt32((int)(m_nXPos), (int)(m_nYPos), (int)(m_nWidth), (int)(m_nHeight)));
            //  this.AppWindow.MoveAndResize(new Windows.Graphics.RectInt32((int)(m_nXPos * nScaleX), (int)(m_nYPos * nScaleY), (int)(m_nWidth * nScaleX), (int)(m_nHeight * nScaleY)));
            //Console.Beep(5000, 10);
        }

        private HRESULT LoadFonts(IDWriteFontSet pFontSet, System.Collections.ObjectModel.ObservableCollection<Font> pFonts, bool bCustom)
        {
            HRESULT hr = HRESULT.S_OK;
            string sExePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            uint nFontCount = pFontSet.GetFontCount();
            for (uint i = 0; i < nFontCount; i++)
            {
                string sPath = "";
                string sFamilyName = "";
                string sFullName = "";
                string sFontWeight = "Normal";
                int nWeightValue = 400;
                string sStyle = "Normal";
                string sStretch = "Normal";
                bool bPropertyExists = false;

                IDWriteLocalizedStrings pLocalizedStrings = null;
                hr = pFontSet.GetPropertyValues(i, DWRITE_FONT_PROPERTY_ID.DWRITE_FONT_PROPERTY_ID_FULL_NAME,
                         out bPropertyExists, out pLocalizedStrings);
                if (SUCCEEDED(hr))
                {
                    uint nIndex = 0;
                    bool bExists = false;
                    StringBuilder sbLocaleName = new StringBuilder(LOCALE_NAME_MAX_LENGTH);
                    int nDefaultLocaleSuccess = GetUserDefaultLocaleName(sbLocaleName, LOCALE_NAME_MAX_LENGTH);
                    if (nDefaultLocaleSuccess > 0)
                    {
                        hr = pLocalizedStrings.FindLocaleName(sbLocaleName.ToString(), out nIndex, out bExists);
                    }
                    if (hr == HRESULT.S_OK && !bExists)
                    {
                        hr = pLocalizedStrings.FindLocaleName("en-us", out nIndex, out bExists);
                    }
                    if (!bExists)
                        nIndex = 0;
                    hr = pLocalizedStrings.GetString(nIndex, sbLocaleName, LOCALE_NAME_MAX_LENGTH);
                    sFullName = sbLocaleName.ToString();
                    SafeRelease(ref pLocalizedStrings);
                }

                hr = pFontSet.GetPropertyValues(i, DWRITE_FONT_PROPERTY_ID.DWRITE_FONT_PROPERTY_ID_FAMILY_NAME,
                       out bPropertyExists, out pLocalizedStrings);
                if (SUCCEEDED(hr))
                {
                    uint nIndex = 0;
                    bool bExists = false;
                    StringBuilder sbLocaleName = new StringBuilder(LOCALE_NAME_MAX_LENGTH);
                    int nDefaultLocaleSuccess = GetUserDefaultLocaleName(sbLocaleName, LOCALE_NAME_MAX_LENGTH);
                    if (nDefaultLocaleSuccess > 0)
                    {
                        hr = pLocalizedStrings.FindLocaleName(sbLocaleName.ToString(), out nIndex, out bExists);
                    }
                    if (hr == HRESULT.S_OK && !bExists)
                    {
                        hr = pLocalizedStrings.FindLocaleName("en-us", out nIndex, out bExists);
                    }
                    if (!bExists)
                        nIndex = 0;
                    hr = pLocalizedStrings.GetString(nIndex, sbLocaleName, LOCALE_NAME_MAX_LENGTH);
                    sFamilyName = sbLocaleName.ToString();
                    SafeRelease(ref pLocalizedStrings);
                }

                hr = pFontSet.GetPropertyValues(i, DWRITE_FONT_PROPERTY_ID.DWRITE_FONT_PROPERTY_ID_WEIGHT,
                  out bPropertyExists, out pLocalizedStrings);
                if (SUCCEEDED(hr))
                {
                    StringBuilder sbLocaleName = new StringBuilder(LOCALE_NAME_MAX_LENGTH);
                    hr = pLocalizedStrings.GetString(0, sbLocaleName, LOCALE_NAME_MAX_LENGTH);
                    string sWeightString = sbLocaleName.ToString();
                    Int32.TryParse(sWeightString, out nWeightValue);
                    sFontWeight = ConvertStringToFontWeightString(sWeightString);
                    SafeRelease(ref pLocalizedStrings);
                }
                hr = pFontSet.GetPropertyValues(i, DWRITE_FONT_PROPERTY_ID.DWRITE_FONT_PROPERTY_ID_STYLE,
                 out bPropertyExists, out pLocalizedStrings);
                if (SUCCEEDED(hr))
                {
                    StringBuilder sbLocaleName = new StringBuilder(LOCALE_NAME_MAX_LENGTH);
                    hr = pLocalizedStrings.GetString(0, sbLocaleName, LOCALE_NAME_MAX_LENGTH);
                    string sStyleString = sbLocaleName.ToString();
                    sStyle = ConvertStringToFontStyleString(sStyleString);
                    SafeRelease(ref pLocalizedStrings);
                }
                hr = pFontSet.GetPropertyValues(i, DWRITE_FONT_PROPERTY_ID.DWRITE_FONT_PROPERTY_ID_STRETCH,
                out bPropertyExists, out pLocalizedStrings);
                if (SUCCEEDED(hr))
                {
                    StringBuilder sbLocaleName = new StringBuilder(LOCALE_NAME_MAX_LENGTH);
                    hr = pLocalizedStrings.GetString(0, sbLocaleName, LOCALE_NAME_MAX_LENGTH);
                    string sStretchString = sbLocaleName.ToString();
                    sStretch = ConvertStringToFontStretchString(sStretchString);
                    SafeRelease(ref pLocalizedStrings);
                }

                IDWriteFontFaceReference pFontFaceReference = null;
                hr = pFontSet.GetFontFaceReference(i, out pFontFaceReference);
                if (SUCCEEDED(hr))
                {
                    IDWriteFontFace3 pFontFace3;
                    hr = pFontFaceReference.CreateFontFace(out pFontFace3);
                    if (SUCCEEDED(hr))
                    {
                        uint nNbFiles = 0;
                        hr = pFontFace3.GetFiles(ref nNbFiles, null);
                        IDWriteFontFile[] fontFiles = new IDWriteFontFile[nNbFiles];
                        hr = pFontFace3.GetFiles(ref nNbFiles, fontFiles);
                        if (SUCCEEDED(hr))
                        {
                            IDWriteFontFileLoader pFontFileLoader = null;
                            hr = fontFiles[0].GetLoader(out pFontFileLoader);
                            if (SUCCEEDED(hr))
                            {
                                IntPtr pFontFileReferenceKey = IntPtr.Zero;
                                IDWriteLocalFontFileLoader pLocalFontFileLoader = null;

                                try
                                {
                                    pLocalFontFileLoader = (IDWriteLocalFontFileLoader)pFontFileLoader;
                                }
                                catch (System.Exception ex)
                                {
                                    string sError = ex.Message + "\r\n" + "HRESULT = 0x" + string.Format("{0:X}", ex.HResult);
                                    System.Diagnostics.Debug.WriteLine(sError);
                                }
                                if (pLocalFontFileLoader != null)
                                {
                                    int nFontFileReferenceKeySize = 0;
                                    hr = fontFiles[0].GetReferenceKey(out pFontFileReferenceKey, out nFontFileReferenceKeySize);
                                    if (SUCCEEDED(hr))
                                    {
                                        uint nFilePathLength = 0;
                                        hr = pLocalFontFileLoader.GetFilePathLengthFromKey(pFontFileReferenceKey, (uint)nFontFileReferenceKeySize, out nFilePathLength);
                                        if (SUCCEEDED(hr))
                                        {
                                            StringBuilder sbFilePath = new StringBuilder((int)nFilePathLength + 1);
                                            hr = pLocalFontFileLoader.GetFilePathFromKey(pFontFileReferenceKey, (uint)nFontFileReferenceKeySize, sbFilePath, nFilePathLength + 1);
                                            if (SUCCEEDED(hr))
                                            {
                                                sPath = sbFilePath.ToString();
                                            }
                                        }
                                    }                                   
                                }
                                SafeRelease(ref pFontFileLoader);
                            }
                        }
                        SafeRelease(ref pFontFace3);
                    }
                    SafeRelease(ref pFontFaceReference);
                }

                if (sPath != "")
                {
                    string sRelativePath = "";
                    if (sPath.StartsWith(sExePath, StringComparison.CurrentCultureIgnoreCase))
                        sRelativePath = sPath.Substring(sExePath.Length);//.TrimStart(System.IO.Path.DirectorySeparatorChar);
                    else
                        sRelativePath = sPath;
                    if (bCustom)
                        pFonts.Add(new Font(sFullName, sPath, sRelativePath + "#" + sFamilyName, sFontWeight, nWeightValue, sStyle, sStretch));
                    else
                        pFonts.Add(new Font(sFullName, sPath, sRelativePath + "#" + sFullName, sFontWeight, nWeightValue, sStyle, sStretch));
                }
            }
            return hr;
        }

        private static string ConvertStringToFontWeightString(string sString)
        {
            string sFWString = sString switch
            {
                "100" => "Thin",
                "200" => "ExtraLight",
                "300" => "Light",
                "350" => "SemiLight",
                "400" => "Normal",
                "500" => "Medium",
                "600" => "SemiBold",
                "700" => "Bold",
                "800" => "ExtraBold",
                "900" => "Black",
                "950" => "ExtraBlack",
                _ => "Normal",
            };
            return sFWString;
        }

        private static string ConvertStringToFontStyleString(string sString)
        {
            string sFWString = sString switch
            {
                "0" => "Normal",               
                "1" => "Oblique",
                "2" => "Italic",
                _ => "Normal",
            };
            return sFWString;
        }

        private static string ConvertStringToFontStretchString(string sString)
        {
            string sFWString = sString switch
            {
                "0" => "Undefined",
                "1" => "UltraCondensed",
                "2" => "ExtraCondensed",
                "3" => "Condensed",
                "4" => "SemiCondensed",
                "5" => "Normal",
                "6" => "SemiExpanded",
                "7" => "Expanded",
                "8" => "ExtraExpanded",
                "9" => "UltraExpanded",
                _ => "Normal",
            };
            return sFWString;
        }

        private void CompositionTarget_Rendering(object sender, object e)
        {
            HRESULT hr = HRESULT.S_OK;
            hr = Render();
        }     

        HRESULT Render()
        {
            HRESULT hr = HRESULT.S_OK;
            if (m_pD2DDeviceContext != null)
            {
                m_pD2DDeviceContext.BeginDraw();
                m_pD2DDeviceContext.SetTransform(Matrix3x2F.Identity());
                m_pD2DDeviceContext.GetSize(out D2D1_SIZE_F size);

                m_pD2DDeviceContext.Clear(new ColorF(ColorF.Enum.Black, 1.0f));
                D2D1_RECT_F backgroundRect = new D2D1_RECT_F(0.0f, 0.0f, size.width, size.height);
                m_pD2DLinearGradientBrush2.SetStartPoint(Point2F(0, 0));
                m_pD2DLinearGradientBrush2.SetEndPoint(Point2F(0, size.height));
                m_pD2DDeviceContext.FillRectangle(ref backgroundRect, m_pD2DLinearGradientBrush2);

                //if (m_pD2DBitmap1 != null)
                //{   
                //    m_pD2DBitmap1.GetSize(out D2D1_SIZE_F sizeBmpBackground);
                //    D2D1_RECT_F destRectBackground = new D2D1_RECT_F(0.0f, 0.0f, size.width, size.height);                    
                //    D2D1_RECT_F sourceRectBackground = new D2D1_RECT_F(0.0f, 0.0f, sizeBmpBackground.width, sizeBmpBackground.height);
                //    //m_pD2DDeviceContext.DrawBitmap(m_pD2DBitmap1, ref destRectBackground, 1.0f, D2D1_BITMAP_INTERPOLATION_MODE.D2D1_BITMAP_INTERPOLATION_MODE_LINEAR, ref sourceRectBackground);
                //}
                //m_pD2DDeviceContext.FillRectangle(RectF(10.0f, 10.0f, 200.0f, 200.0f), m_pD2DSolidColorBrushPink);
                //m_pD2DDeviceContext.FillEllipse(Ellipse(new Direct2D.D2D1_POINT_2F(300, 300), 100.0f, 100.0f), m_pD2DSolidColorBrushRed);

                // Test IDWriteTextLayout
                //if (m_pTextLayout != null)
                //{
                //    if (m_pCTR != null)
                //    {
                //        hr = m_pTextLayout.Draw(IntPtr.Zero, m_pCTR, 0, 0);
                //        if (SUCCEEDED(hr))
                //        {
                //            if (m_pCTR.IsDWriteCore)
                //            {
                //                hr = m_pCTR.DrawBitmapRenderTarget(Point2F(0, 0));
                //            }
                //        }
                //    }

                //    // DWrite only
                //    // m_pD2DDeviceContext.DrawTextLayout(Point2F(100, 100), m_pTextLayout, m_pD2DSolidColorBrushPink, 
                //    // D2D1_DRAW_TEXT_OPTIONS.D2D1_DRAW_TEXT_OPTIONS_NO_SNAP | D2D1_DRAW_TEXT_OPTIONS.D2D1_DRAW_TEXT_OPTIONS_ENABLE_COLOR_FONT);
                //}

                if (m_nTextEffect == (int)TEXT_EFFECT.GRADIENT)
                {
                    if (m_pD2DGeometry1 != null)
                    {
                        CenterGeometry(m_pD2DDeviceContext, m_pD2DGeometry1, 1.0f);

                        m_pD2DDeviceContext.DrawGeometry(m_pD2DGeometry1, m_pD2DMainBrush, 2.0f);
                        // Animate gradient
                        var translateMatrix = Matrix3x2F.Translation(new D2D1_SIZE_F(0, m_nY1));
                        m_pD2DLinearGradientBrush1.SetTransform(translateMatrix);
                        m_nY1 += 1.0f;
                        if (m_nY1 >= m_nComputedHeight1 * 1.33f)
                        {
                            m_nY1 = 0.0f;
                        }
                        m_pD2DDeviceContext.FillGeometry(m_pD2DGeometry1, m_pD2DLinearGradientBrush1);
                    }
                }
                else if (m_nTextEffect == (int)TEXT_EFFECT.SHADOW)
                {
                    if (m_pD2DGeometry2 != null)
                    {
                        CenterGeometry(m_pD2DDeviceContext, m_pD2DGeometry2, 0.99f);

                        // Save transform
                        //m_pD2DDeviceContext.GetTransform(out var savedTransform);
                        //D2D1_MATRIX_3X2_F mSavedTransform = ToClass(savedTransform);

                        //m_pD2DDeviceContext.SetTransform(Matrix3x2F.Identity());

                        m_pD2DGeometry2.GetBounds(null, out var geoBounds);

                        // Padding
                        float shadowOffset = Math.Abs(m_nShadowTranslate);
                        float shadowBlur = 3.0f; // default D2D1_SHADOW_PROP_BLUR_STANDARD_DEVIATION
                        float padding = shadowBlur * 3 + shadowOffset;
                        D2D1_RECT_F inflatedBounds = new D2D1_RECT_F(
                            geoBounds.left - padding,
                            geoBounds.top - padding,
                            geoBounds.right + padding,
                            geoBounds.bottom + padding
                        );

                        D2D1_SIZE_F rtSize = new D2D1_SIZE_F(inflatedBounds.right - inflatedBounds.left, inflatedBounds.bottom - inflatedBounds.top);
                        Direct2D.D2D1_SIZE_U rtSizeU = SizeU((uint)Math.Ceiling(rtSize.width), (uint)Math.Ceiling(rtSize.height));

                        hr = m_pD2DDeviceContext.CreateCompatibleRenderTarget(
                            ref rtSize,
                            ref rtSizeU,
                            PixelFormat(DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM, D2D1_ALPHA_MODE.D2D1_ALPHA_MODE_PREMULTIPLIED),
                            D2D1_COMPATIBLE_RENDER_TARGET_OPTIONS.D2D1_COMPATIBLE_RENDER_TARGET_OPTIONS_NONE,
                            out ID2D1BitmapRenderTarget pCompatibleRenderTarget);

                        if (SUCCEEDED(hr))
                        {
                            pCompatibleRenderTarget.BeginDraw();
                            pCompatibleRenderTarget.Clear(null);

                            // Move geometry into bitmap space                            
                            pCompatibleRenderTarget.SetTransform(Matrix3x2F.Translation(-inflatedBounds.left, -inflatedBounds.top));

                            pCompatibleRenderTarget.FillGeometry(m_pD2DGeometry2, m_pD2DMainBrush);

                            pCompatibleRenderTarget.EndDraw(out _, out _);

                            ID2D1Bitmap pCompatibleBitmap = null;
                            pCompatibleRenderTarget.GetBitmap(out pCompatibleBitmap);

                            ID2D1Effect pShadowEffect = null;
                            m_pD2DDeviceContext.CreateEffect(CLSID_D2D1Shadow, out pShadowEffect);
                            pShadowEffect.SetInput(0, pCompatibleBitmap);

                            //SetEffectFloat(pShadowEffect, (uint)D2D1_SHADOW_PROP.D2D1_SHADOW_PROP_BLUR_STANDARD_DEVIATION, 4.0f);

                            // RGBA
                            //float[] aFloatArray = {0.0f, 0x14/255.0f, 0.0f,  1.0f };                           
                            //SetEffectFloatArray(pShadowEffect, (uint)D2D1_SHADOW_PROP.D2D1_SHADOW_PROP_COLOR, aFloatArray);

                            var ptShadowPos = Point2F(inflatedBounds.left + m_nShadowTranslate, inflatedBounds.top + m_nShadowTranslate);

                            //m_pD2DDeviceContext.SetTransform(mSavedTransform);

                            // No sourceRectangle (fixes clipping)
                            m_pD2DDeviceContext.DrawImage((ID2D1Image)pShadowEffect, ref ptShadowPos,
                                IntPtr.Zero,
                                D2D1_INTERPOLATION_MODE.D2D1_INTERPOLATION_MODE_LINEAR, D2D1_COMPOSITE_MODE.D2D1_COMPOSITE_MODE_SOURCE_OVER);

                            // Animate shadow
                            m_nShadowTranslate += 0.2f * m_nShadowTranslateDirection;
                            if (m_nShadowTranslate >= 16.0f && m_nShadowTranslateDirection > 0 || m_nShadowTranslate <= 4.0f && m_nShadowTranslateDirection < 0)
                            {
                                m_nShadowTranslateDirection = -m_nShadowTranslateDirection;
                            }

                            SafeRelease(ref pShadowEffect);
                            SafeRelease(ref pCompatibleBitmap);
                            SafeRelease(ref pCompatibleRenderTarget);
                        }
                        m_pD2DDeviceContext.FillGeometry(m_pD2DGeometry2, m_pD2DSolidColorBrushGreen);
                    }
                }
                else if (m_nTextEffect == (int)TEXT_EFFECT.TURBULENCE)
                {
                    if (m_pD2DGeometry3 != null)
                    {
                        CenterGeometry(m_pD2DDeviceContext, m_pD2DGeometry3, 0.99f);

                        m_pD2DGeometry3.GetBounds(null, out var geoBounds);

                        // Padding
                        float padding = 40.0f;
                        var inflatedBounds = new D2D1_RECT_F(
                            geoBounds.left - padding,
                            geoBounds.top - padding,
                            geoBounds.right + padding,
                            geoBounds.bottom + padding);

                        D2D1_SIZE_F rtSize = new D2D1_SIZE_F(inflatedBounds.right - inflatedBounds.left, inflatedBounds.bottom - inflatedBounds.top);
                        Direct2D.D2D1_SIZE_U rtSizeU = SizeU((uint)Math.Ceiling(rtSize.width), (uint)Math.Ceiling(rtSize.height));

                        hr = m_pD2DDeviceContext.CreateCompatibleRenderTarget(
                            ref rtSize,
                            ref rtSizeU,
                            PixelFormat(DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM, D2D1_ALPHA_MODE.D2D1_ALPHA_MODE_PREMULTIPLIED),
                            D2D1_COMPATIBLE_RENDER_TARGET_OPTIONS.D2D1_COMPATIBLE_RENDER_TARGET_OPTIONS_NONE,
                            out ID2D1BitmapRenderTarget pCompatibleRenderTarget);
                        if (SUCCEEDED(hr))
                        {
                            pCompatibleRenderTarget.BeginDraw();
                            pCompatibleRenderTarget.Clear(null);

                            // Move geometry into bitmap space
                            pCompatibleRenderTarget.SetTransform(Matrix3x2F.Translation(-inflatedBounds.left, -inflatedBounds.top));

                            pCompatibleRenderTarget.FillGeometry(m_pD2DGeometry3, m_pD2DSolidColorBrushWhite);
                            pCompatibleRenderTarget.EndDraw(out _, out _);

                            // Get bitmap of text
                            pCompatibleRenderTarget.GetBitmap(out ID2D1Bitmap pTextBitmap);

                            m_pD2DDeviceContext.CreateEffect(D2DTools.CLSID_D2D1Turbulence, out ID2D1Effect pTurbulenceEffect);

                            SetEffectFloatArray(pTurbulenceEffect, (uint)D2D1_TURBULENCE_PROP.D2D1_TURBULENCE_PROP_BASE_FREQUENCY, new float[] { m_nTurbulenceBaseFrequencyX, 0.0f });
                            SetEffectInt(pTurbulenceEffect, (uint)D2D1_TURBULENCE_PROP.D2D1_TURBULENCE_PROP_NUM_OCTAVES, 1);
                            SetEffectFloatArray(pTurbulenceEffect, (uint)D2D1_TURBULENCE_PROP.D2D1_TURBULENCE_PROP_SIZE, new float[] { rtSize.width, rtSize.height });

                            m_pD2DDeviceContext.CreateEffect(D2DTools.CLSID_D2D1DisplacementMap, out ID2D1Effect pDisplacementMapEffect);
                            pDisplacementMapEffect.SetInput(0, pTextBitmap);       // original text
                            pDisplacementMapEffect.SetInput(1, (ID2D1Image)pTurbulenceEffect);
                            SetEffectFloat(pDisplacementMapEffect, (uint)D2D1_DISPLACEMENTMAP_PROP.D2D1_DISPLACEMENTMAP_PROP_SCALE, 12.0f); // wave amplitude in pixels
                            SetEffectInt(pDisplacementMapEffect, (uint)D2D1_DISPLACEMENTMAP_PROP.D2D1_DISPLACEMENTMAP_PROP_X_CHANNEL_SELECT, (uint)D2D1_CHANNEL_SELECTOR.D2D1_CHANNEL_SELECTOR_R);
                            SetEffectInt(pDisplacementMapEffect, (uint)D2D1_DISPLACEMENTMAP_PROP.D2D1_DISPLACEMENTMAP_PROP_Y_CHANNEL_SELECT, (uint)D2D1_CHANNEL_SELECTOR.D2D1_CHANNEL_SELECTOR_G);

                            m_pD2DDeviceContext.DrawImage(
                                (ID2D1Image)pDisplacementMapEffect,
                                Point2F(inflatedBounds.left, inflatedBounds.top),
                                IntPtr.Zero,
                                D2D1_INTERPOLATION_MODE.D2D1_INTERPOLATION_MODE_LINEAR,
                                D2D1_COMPOSITE_MODE.D2D1_COMPOSITE_MODE_SOURCE_OVER);

                            m_nTurbulenceBaseFrequencyX += 0.0005f * m_nTurbulenceDirection;
                            if (m_nTurbulenceBaseFrequencyX >= 0.02f && m_nTurbulenceDirection > 0 ||
                                m_nTurbulenceBaseFrequencyX <= 0.0f && m_nTurbulenceDirection < 0)
                            {
                                m_nTurbulenceDirection = -m_nTurbulenceDirection;
                            }

                            SafeRelease(ref pDisplacementMapEffect);
                            SafeRelease(ref pTurbulenceEffect);
                            SafeRelease(ref pTextBitmap);
                            SafeRelease(ref pCompatibleRenderTarget);
                        }
                    }
                }
                else if (m_nTextEffect == (int)TEXT_EFFECT.WAVES)
                {
                    // Slow (geometries in loop...)
                    //if (m_pD2DGeometry3 != null)
                    //{                        
                    //    CenterGeometry(m_pD2DDeviceContext, m_pD2DGeometry3);

                    //    m_pD2DGeometry3.GetBounds(Matrix3x2F.Identity(), out var geoBounds);

                    //    float sliceWidth = 2.0f;          // width of each vertical slice
                    //    float amplitude = 12.0f;          // wave height
                    //    float wavelength = 120.0f;        // horizontal wavelength
                    //    float frequency = (2.0f * MathF.PI) / wavelength;
                    //    float time = m_nWaveTime;

                    //    for (float x = geoBounds.left; x < geoBounds.right; x += sliceWidth)
                    //    {
                    //        float xCenter = x + sliceWidth * 0.5f;
                    //        float yOffset = MathF.Sin((xCenter * frequency) + time) * amplitude;

                    //        // Create transformed geometry for this slice
                    //        m_pD2DFactory1.CreateTransformedGeometry(m_pD2DGeometry3, Matrix3x2F.Translation(0, yOffset), out ID2D1TransformedGeometry sliceGeom);

                    //        // Clip slice
                    //        var clipRect = new D2D1_RECT_F(x, geoBounds.top - amplitude, x + sliceWidth, geoBounds.bottom + amplitude);
                    //        m_pD2DDeviceContext.PushAxisAlignedClip(ref clipRect, D2D1_ANTIALIAS_MODE.D2D1_ANTIALIAS_MODE_PER_PRIMITIVE);

                    //        m_pD2DDeviceContext.FillGeometry(sliceGeom, m_pD2DSolidColorBrushWhite);

                    //        m_pD2DDeviceContext.PopAxisAlignedClip();
                    //        SafeRelease(ref sliceGeom);
                    //    }

                    //    // Animate
                    //    m_nWaveTime += 0.2f;
                    //    if (m_nWaveTime > MathF.PI * 2.0f)
                    //        m_nWaveTime -= MathF.PI * 2.0f;
                    //}

                }
                else if (m_nTextEffect == (int)TEXT_EFFECT.GLOWING)
                {
                    if (m_pD2DGeometry4 != null)
                    {
                        CenterGeometry(m_pD2DDeviceContext, m_pD2DGeometry4, 0.99f);

                        // Save transform
                        //m_pD2DDeviceContext.GetTransform(out var savedTransform);
                        //D2D1_MATRIX_3X2_F mSavedTransform = ToClass(savedTransform);

                        //m_pD2DDeviceContext.SetTransform(Matrix3x2F.Identity());

                        m_pD2DGeometry4.GetBounds(null, out var geoBounds);

                        // Padding
                        float shadowOffset = Math.Abs(m_nShadowTranslate);
                        float shadowBlur = 20.0f; // default D2D1_SHADOW_PROP_BLUR_STANDARD_DEVIATION
                        float padding = shadowBlur * 3 + shadowOffset;
                        D2D1_RECT_F inflatedBounds = new D2D1_RECT_F(
                            geoBounds.left - padding,
                            geoBounds.top - padding,
                            geoBounds.right + padding,
                            geoBounds.bottom + padding
                        );

                        D2D1_SIZE_F rtSize = new D2D1_SIZE_F(inflatedBounds.right - inflatedBounds.left, inflatedBounds.bottom - inflatedBounds.top);
                        Direct2D.D2D1_SIZE_U rtSizeU = SizeU((uint)Math.Ceiling(rtSize.width), (uint)Math.Ceiling(rtSize.height));

                        hr = m_pD2DDeviceContext.CreateCompatibleRenderTarget(ref rtSize, ref rtSizeU, PixelFormat(DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM, D2D1_ALPHA_MODE.D2D1_ALPHA_MODE_PREMULTIPLIED),
                            D2D1_COMPATIBLE_RENDER_TARGET_OPTIONS.D2D1_COMPATIBLE_RENDER_TARGET_OPTIONS_NONE, out ID2D1BitmapRenderTarget pCompatibleRenderTarget);
                        if (SUCCEEDED(hr))
                        {
                            pCompatibleRenderTarget.BeginDraw();
                            pCompatibleRenderTarget.Clear(null);

                            // Move geometry into bitmap space                            
                            pCompatibleRenderTarget.SetTransform(Matrix3x2F.Translation(-inflatedBounds.left, -inflatedBounds.top));

                            pCompatibleRenderTarget.FillGeometry(m_pD2DGeometry4, m_pD2DSolidColorBrushPink);

                            hr = pCompatibleRenderTarget.EndDraw(out UInt64 tag11, out UInt64 tag21);
                            ID2D1Bitmap pCompatibleBitmap = null;
                            hr = pCompatibleRenderTarget.GetBitmap(out pCompatibleBitmap);

                            ID2D1Effect pShadowEffect = null;
                            hr = m_pD2DDeviceContext.CreateEffect(CLSID_D2D1Shadow, out pShadowEffect);
                            pShadowEffect.SetInput(0, pCompatibleBitmap);

                            SetEffectFloat(pShadowEffect, (uint)D2D1_SHADOW_PROP.D2D1_SHADOW_PROP_BLUR_STANDARD_DEVIATION, m_nShadowStandardDeviation);

                            Windows.UI.Color ShadowColor = Microsoft.UI.Colors.DeepPink;
                            float[] aFloatArray = { (float)((float)ShadowColor.R / 255.0f), (float)((float)ShadowColor.G / 255.0f), (float)((float)ShadowColor.B / 255.0f), 1.0f };
                            SetEffectFloatArray(pShadowEffect, (uint)D2D1_SHADOW_PROP.D2D1_SHADOW_PROP_COLOR, aFloatArray);

                            // Animate
                            m_nShadowStandardDeviation += 0.5f * m_nShadowDirection;
                            if (m_nShadowStandardDeviation >= 20.0f && m_nShadowDirection > 0 ||
                                m_nShadowStandardDeviation <= 0.0f && m_nShadowDirection < 0)
                            {
                                m_nShadowDirection = -m_nShadowDirection;
                            }

                            ID2D1Effect pBrightnessEffect = null;
                            hr = m_pD2DDeviceContext.CreateEffect(D2DTools.CLSID_D2D1Brightness, out pBrightnessEffect);
                            pBrightnessEffect.SetInput(0, (ID2D1Image)pShadowEffect);

                            float[] aFloatArray1 = { 0.15f, 0.70f };
                            SetEffectFloatArray(pBrightnessEffect, (uint)D2D1_BRIGHTNESS_PROP.D2D1_BRIGHTNESS_PROP_WHITE_POINT, aFloatArray1);

                            var ptShadowPos = Point2F(inflatedBounds.left, inflatedBounds.top);

                            //m_pD2DDeviceContext.SetTransform(mSavedTransform);

                            m_pD2DDeviceContext.DrawImage((ID2D1Image)pBrightnessEffect, ptShadowPos, IntPtr.Zero, D2D1_INTERPOLATION_MODE.D2D1_INTERPOLATION_MODE_LINEAR, D2D1_COMPOSITE_MODE.D2D1_COMPOSITE_MODE_SOURCE_OVER);
                            SafeRelease(ref pBrightnessEffect);
                            SafeRelease(ref pShadowEffect);
                            SafeRelease(ref pCompatibleBitmap);
                            SafeRelease(ref pCompatibleRenderTarget);
                        }
                        m_pD2DDeviceContext.FillGeometry(m_pD2DGeometry4, m_pD2DSolidColorBrushWhite);
                    }
                }
                else if (m_nTextEffect == (int)TEXT_EFFECT.BITMAP)
                {
                    if (m_pD2DGeometry5 != null)
                    {
                        CenterGeometry(m_pD2DDeviceContext, m_pD2DGeometry5, 1.0f);

                        // Save transform
                        //m_pD2DDeviceContext.GetTransform(out var savedTransform);
                        //D2D1_MATRIX_3X2_F mSavedTransform = ToClass(savedTransform);

                        m_pD2DGeometry5.GetBounds(null, out var geoBounds);

                        // Animate the bitmap brush
                        m_imageScrollX += 0.6f;   // horizontal speed
                        m_imageScrollY += 0.25f;  // vertical speed

                        // prevent float overflow 
                        if (m_imageScrollX > 10000) m_imageScrollX = 0;
                        if (m_imageScrollY > 10000) m_imageScrollY = 0;

                        var brushTransform = Matrix3x2F.Translation(m_imageScrollX, m_imageScrollY);
                        m_pD2DBitmapBrush1.SetTransform(brushTransform);

                        // Draw text outline 
                        m_pD2DDeviceContext.DrawGeometry(m_pD2DGeometry5, m_pD2DMainBrush, 2.0f);

                        // Clip to text geometry
                        D2D1_LAYER_PARAMETERS lp = LayerParameters(InfiniteRect(), m_pD2DGeometry5);

                        m_pD2DDeviceContext.PushLayer(ref lp);

                        // Fill text with animated bitmap
                        m_pD2DDeviceContext.FillRectangle(ref geoBounds, m_pD2DBitmapBrush1);

                        m_pD2DDeviceContext.PopLayer();
                        SafeRelease(ref lp.geometricMask);
                    }
                }
                else if (m_nTextEffect == (int)TEXT_EFFECT.SCROLLING)
                {
                    if (m_pD2DGeometry6 != null)
                    {
                        CenterGeometry(m_pD2DDeviceContext, m_pD2DGeometry6, 1.0f);

                        // Save transform
                        m_pD2DDeviceContext.GetTransform(out var savedTransform);
                        D2D1_MATRIX_3X2_F mSavedTransform = ToClass(savedTransform);

                        m_pD2DGeometry6.GetBounds(null, out var geoBounds);

                        D2D1_MATRIX_3X2_F scrollTransform = new D2D1_MATRIX_3X2_F
                        {
                            _11 = mSavedTransform._11,
                            _12 = mSavedTransform._12,
                            _21 = mSavedTransform._21,
                            _22 = mSavedTransform._22,
                            _31 = mSavedTransform._31 + m_nXScroll,
                            _32 = mSavedTransform._32 // Keep centered Y
                        };
                        m_pD2DDeviceContext.SetTransform(scrollTransform);

                        m_pD2DDeviceContext.DrawGeometry(m_pD2DGeometry6, m_pD2DMainBrush, 2.0f);
                        m_pD2DDeviceContext.FillGeometry(m_pD2DGeometry6, m_pD2DSolidColorBrushPink);

                        m_nXScroll -= 2.0f;
                        if (m_nXScroll <= -(geoBounds.right - geoBounds.left))
                            m_nXScroll = size.width;
                    }
                }
                else if (m_nTextEffect == (int)TEXT_EFFECT.COLLAPSE)
                {
                    if (m_pD2DGeometry7 != null)
                    {
                        CenterGeometry(m_pD2DDeviceContext, m_pD2DGeometry7, 1.0f);

                        m_pD2DGeometry7.GetBounds(null, out var geoBounds);

                        var inflatedBounds = new D2D1_RECT_F(
                            geoBounds.left,
                            geoBounds.top,
                            geoBounds.right,
                            geoBounds.bottom);

                        D2D1_SIZE_F rtSize = new D2D1_SIZE_F(inflatedBounds.right - inflatedBounds.left, inflatedBounds.bottom - inflatedBounds.top);
                        Direct2D.D2D1_SIZE_U rtSizeU = SizeU((uint)Math.Ceiling(rtSize.width), (uint)Math.Ceiling(rtSize.height));

                        hr = m_pD2DDeviceContext.CreateCompatibleRenderTarget(ref rtSize, ref rtSizeU, PixelFormat(DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM, D2D1_ALPHA_MODE.D2D1_ALPHA_MODE_PREMULTIPLIED),
                            D2D1_COMPATIBLE_RENDER_TARGET_OPTIONS.D2D1_COMPATIBLE_RENDER_TARGET_OPTIONS_NONE, out ID2D1BitmapRenderTarget pCompatibleRenderTarget);
                        if (SUCCEEDED(hr))
                        {
                            pCompatibleRenderTarget.BeginDraw();
                            pCompatibleRenderTarget.Clear(null);

                            // Move geometry into bitmap space                            
                            pCompatibleRenderTarget.SetTransform(Matrix3x2F.Translation(-inflatedBounds.left, -inflatedBounds.top));

                            pCompatibleRenderTarget.DrawGeometry(m_pD2DGeometry7, m_pD2DMainBrush, 2.0f);
                            pCompatibleRenderTarget.FillGeometry(m_pD2DGeometry7, m_pD2DSolidColorBrushRed);

                            hr = pCompatibleRenderTarget.EndDraw(out UInt64 tag11, out UInt64 tag21);
                            ID2D1Bitmap pCompatibleBitmap = null;
                            hr = pCompatibleRenderTarget.GetBitmap(out pCompatibleBitmap);

                            ID2D1Effect pDisplacementMapEffect = null;
                            hr = m_pD2DDeviceContext.CreateEffect(D2DTools.CLSID_D2D1DisplacementMap, out pDisplacementMapEffect);
                            pDisplacementMapEffect.SetInput(0, pCompatibleBitmap);
                            // m_pD2DBitmap2 size must be > m_pD2DGeometry7 size
                            pDisplacementMapEffect.SetInput(1, m_pD2DBitmap2);

                            SetEffectFloat(pDisplacementMapEffect, (uint)D2D1_DISPLACEMENTMAP_PROP.D2D1_DISPLACEMENTMAP_PROP_SCALE, m_nDisplacementMapScale);
                            if (m_nCptDisplacementPause == 0)
                                m_nDisplacementMapScale += 5.0f * m_nDisplacementMapDirection;

                            if (m_nDisplacementMapScale <= -450.0f && m_nDisplacementMapDirection < 0)
                            {
                                m_nDisplacementMapDirection = -m_nDisplacementMapDirection;
                            }
                            if (m_nDisplacementMapScale >= 0.0f && m_nDisplacementMapDirection > 0)
                            {
                                m_nCptDisplacementPause++;
                                if (m_nCptDisplacementPause >= 60 * 3)
                                {
                                    m_nCptDisplacementPause = 0;
                                    m_nDisplacementMapDirection = -m_nDisplacementMapDirection;
                                }
                            }

                            SetEffectInt(pDisplacementMapEffect, (uint)D2D1_DISPLACEMENTMAP_PROP.D2D1_DISPLACEMENTMAP_PROP_X_CHANNEL_SELECT, (uint)D2D1_CHANNEL_SELECTOR.D2D1_CHANNEL_SELECTOR_G);
                            SetEffectInt(pDisplacementMapEffect, (uint)D2D1_DISPLACEMENTMAP_PROP.D2D1_DISPLACEMENTMAP_PROP_Y_CHANNEL_SELECT, (uint)D2D1_CHANNEL_SELECTOR.D2D1_CHANNEL_SELECTOR_B);

                            var ptShadowPos = Point2F(inflatedBounds.left, inflatedBounds.top);
                            D2D1_RECT_F sourceRectangle = new D2D1_RECT_F(0, 1, rtSize.width, rtSize.height * 3);
                            IntPtr pSourceRectangle = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(D2D1_RECT_F)));
                            Marshal.StructureToPtr(sourceRectangle, pSourceRectangle, false);
                            m_pD2DDeviceContext.DrawImage((ID2D1Image)pDisplacementMapEffect, ptShadowPos, pSourceRectangle, D2D1_INTERPOLATION_MODE.D2D1_INTERPOLATION_MODE_LINEAR, D2D1_COMPOSITE_MODE.D2D1_COMPOSITE_MODE_SOURCE_OVER);
                            Marshal.FreeHGlobal(pSourceRectangle);
                            SafeRelease(ref pDisplacementMapEffect);
                            SafeRelease(ref pCompatibleBitmap);
                            SafeRelease(ref pCompatibleRenderTarget);
                        }
                    }
                }
                else if (m_nTextEffect == (int)TEXT_EFFECT.POINT_DIFFUSE_LIGHTING)
                {
                    if (m_pD2DGeometry6 != null)
                    {
                        D2D1_SIZE_F rtSize = size;
                        Direct2D.D2D1_SIZE_U rtSizeU = SizeU((uint)Math.Ceiling(rtSize.width), (uint)Math.Ceiling(rtSize.height));

                        hr = m_pD2DDeviceContext.CreateCompatibleRenderTarget(
                            ref rtSize,
                            ref rtSizeU,
                            PixelFormat(DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM, D2D1_ALPHA_MODE.D2D1_ALPHA_MODE_PREMULTIPLIED),
                            D2D1_COMPATIBLE_RENDER_TARGET_OPTIONS.D2D1_COMPATIBLE_RENDER_TARGET_OPTIONS_NONE,
                            out ID2D1BitmapRenderTarget pCompatibleRenderTarget);
                        if (SUCCEEDED(hr))
                        {
                            pCompatibleRenderTarget.BeginDraw();
                            pCompatibleRenderTarget.Clear(null);

                            CenterGeometryRT(pCompatibleRenderTarget, m_pD2DGeometry6, 0.99f);

                            pCompatibleRenderTarget.FillGeometry(m_pD2DGeometry6, m_pD2DSolidColorBrushWhite);
                            pCompatibleRenderTarget.EndDraw(out _, out _);

                            // Get bitmap of text
                            pCompatibleRenderTarget.GetBitmap(out ID2D1Bitmap pTextBitmap);

                            m_pD2DDeviceContext.CreateEffect(D2DTools.CLSID_D2D1PointDiffuse, out ID2D1Effect pPointDiffuseEffect);
                            pPointDiffuseEffect.SetInput(0, pTextBitmap);

                            // RGB
                            float[] aFloatArray = { 0xFF / 255.0f, 0xD7 / 255.0f, 0.0f };
                            SetEffectFloatArray(pPointDiffuseEffect, (uint)D2D1_POINTDIFFUSE_PROP.D2D1_POINTDIFFUSE_PROP_COLOR, aFloatArray);

                            m_LightTime += m_LightSpeed;
                            float lightX = (MathF.Sin(m_LightTime) * 0.5f + 0.5f) * rtSize.width;
                            float lightY = rtSize.height * 0.5f + MathF.Sin(m_LightTime * 2.0f) * 40.0f;
                            SetEffectFloatArray(
                                pPointDiffuseEffect,
                                (uint)D2D1_POINTDIFFUSE_PROP.D2D1_POINTDIFFUSE_PROP_LIGHT_POSITION,
                                new float[] { lightX, lightY, m_LightZ }
                            );

                            m_pD2DDeviceContext.DrawImage(
                                (ID2D1Image)pPointDiffuseEffect,
                                Point2F(0, 0),
                                IntPtr.Zero,
                                D2D1_INTERPOLATION_MODE.D2D1_INTERPOLATION_MODE_LINEAR,
                                D2D1_COMPOSITE_MODE.D2D1_COMPOSITE_MODE_SOURCE_OVER);

                            SafeRelease(ref pPointDiffuseEffect);
                            SafeRelease(ref pTextBitmap);
                            SafeRelease(ref pCompatibleRenderTarget);
                        }
                    }
                }

                hr = m_pD2DDeviceContext.EndDraw(out ulong tag1, out ulong tag2);
                if ((uint)hr == D2DTools.D2DERR_RECREATE_TARGET)
                {
                    m_pD2DDeviceContext.SetTarget(null);
                    SafeRelease(ref m_pD2DDeviceContext);
                    hr = CreateDeviceContext();
                    CleanDeviceResources();
                    hr = CreateDeviceResources();
                    hr = CreateSwapChain(IntPtr.Zero);
                    hr = ConfigureSwapChain(hWndMain);
                }
                hr = m_pDXGISwapChain1.Present(1, 0);
            }
            return (hr);
        }

        private void CenterGeometry(ID2D1DeviceContext pD2DDeviceContext, ID2D1Geometry pD2DGeometry, float scale)
        {
            pD2DGeometry.GetBounds(null, out var geoBounds);
            float geoWidth = geoBounds.right - geoBounds.left;
            float geoHeight = geoBounds.bottom - geoBounds.top;

            pD2DDeviceContext.GetSize(out D2D1_SIZE_F panelSize);
            float panelWidth = panelSize.width;
            float panelHeight = panelSize.height;

            // Compute uniform scale to fit (keeping ratio)
            float scaleX = panelWidth / geoWidth;
            float scaleY = panelHeight / geoHeight;
            float uniformScale = Math.Min(scaleX, scaleY); // fit inside SwapChainPanel           
            
            // To reduce width (for shadow for example)
            uniformScale *= scale;

            // Compute translation to center
            float offsetX = (panelWidth - geoWidth * uniformScale) / 2.0f - geoBounds.left * uniformScale;
            float offsetY = (panelHeight - geoHeight * uniformScale) / 2.0f - geoBounds.top * uniformScale;

            // Build transform: scale then translate
            var transform = Matrix3x2F.Scale(uniformScale, uniformScale) * Matrix3x2F.Translation(offsetX, offsetY);
            pD2DDeviceContext.SetTransform(transform);
        }

        private void CenterGeometryRT(ID2D1RenderTarget pRT, ID2D1Geometry pD2DGeometry, float scale)
        {
            pD2DGeometry.GetBounds(null, out var geoBounds);

            float geoWidth = geoBounds.right - geoBounds.left;
            float geoHeight = geoBounds.bottom - geoBounds.top;

            pRT.GetSize(out D2D1_SIZE_F size);

            float scaleX = size.width / geoWidth;
            float scaleY = size.height / geoHeight;
            float uniformScale = Math.Min(scaleX, scaleY) * scale;

            float offsetX = (size.width - geoWidth * uniformScale) * 0.5f - geoBounds.left * uniformScale;
            float offsetY = (size.height - geoHeight * uniformScale) * 0.5f - geoBounds.top * uniformScale;

            var transform = Matrix3x2F.Scale(uniformScale, uniformScale) * Matrix3x2F.Translation(offsetX, offsetY);
            pRT.SetTransform(transform);
        }


        float m_nY1 = 0.0f;

        float m_nShadowTranslate = 4.0f;
        float m_nShadowTranslateDirection = 1.0f;

        float m_nTurbulenceBaseFrequencyX = 0.0f;
        float m_nTurbulenceDirection = 1.0f;

        float m_nWaveTime = 0.0f;

        float m_nShadowStandardDeviation = 0.0f;
        float m_nShadowDirection = 1.0f;

        float m_imageScrollX = 0.0f;
        float m_imageScrollY = 0.0f;

        float m_nXScroll = 300.0f;

        float m_nDisplacementMapScale = 0.1f;
        float m_nDisplacementMapDirection = -1.0f;
        int m_nCptDisplacementPause = 0;

        float m_LightTime = 0.0f;
        float m_LightSpeed = 0.025f;
        float m_LightZ = 50.0f;


        // From Copilot
        private static bool NeedsShaping(string text)
        {
            foreach (var rune in text.EnumerateRunes())
            {
                int u = rune.Value;
                // Arabic, Nastaliq, Indic scripts, etc.
                if ((u >= 0x0600 && u <= 0x08FF) ||   // Arabic + Nastaliq
                    (u >= 0x0900 && u <= 0x0DFF) ||   // Indic
                    (u >= 0xFB50 && u <= 0xFDFF) ||   // Arabic presentation forms
                    (u >= 0xFE70 && u <= 0xFEFF))     // Arabic forms
                {
                    return true;
                }
            }
            return false;
        }

        private HRESULT CreateTextGeometry(string text, string fontPath, float fontSize, bool bold, bool italic, out ID2D1Geometry geometry, out float computedHeight)
        {
            if (NeedsShaping(text))
            {
                return CreateShapedTextGeometry(text, fontPath, fontSize, bold, italic, out geometry, out computedHeight);
            }
            else
            {
                return CreateDWriteTextGeometry(text, fontPath, fontSize, true, bold, italic, out geometry, out computedHeight);
            }
        }

        // From ChatGPT but crashes with DWriteCore
        private HRESULT CreateShapedTextGeometry(string text, string fontPath, float fontSize, bool bold, bool italic,
            out ID2D1Geometry geometry, out float computedHeight)
        {
            geometry = null;
            computedHeight = 0;
            HRESULT hr;

            // --- 1. Create text format ---
            IDWriteTextFormat format;
            hr = m_pDWriteFactory7.CreateTextFormat(
                //Path.GetFileNameWithoutExtension(fontPath),
                  "Noto Nastaliq Urdu",
                null,
                bold ? DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_BOLD : DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_NORMAL,
                italic ? DWRITE_FONT_STYLE.DWRITE_FONT_STYLE_ITALIC : DWRITE_FONT_STYLE.DWRITE_FONT_STYLE_NORMAL,
                DWRITE_FONT_STRETCH.DWRITE_FONT_STRETCH_NORMAL,
                fontSize,
                "", out format);
            if (!SUCCEEDED(hr)) return hr;

            // --- 2. Create text layout ---
            IDWriteTextLayout layout;
            hr = m_pDWriteFactory7.CreateTextLayout(text, (uint)text.Length, format, 4096, 4096, out layout);
            if (!SUCCEEDED(hr)) { SafeRelease(ref format); return hr; }

            // --- 3. Collect shaped glyph runs ---
            var renderer = new GeometryTextRenderer(m_pD2DFactory1);

            // -2003283967 0x88985001 DWRITE_E_UNEXPECTED
            hr = layout.Draw(IntPtr.Zero, renderer, 0, 0);
            if (!SUCCEEDED(hr)) { SafeRelease(ref format); SafeRelease(ref layout); return hr; }

            // --- 4. Create merged PathGeometry ---
            hr = m_pD2DFactory1.CreatePathGeometry(out ID2D1PathGeometry mergedPath);
            if (hr != HRESULT.S_OK) { SafeRelease(ref format); SafeRelease(ref layout); return hr; }

            hr = mergedPath.Open(out ID2D1GeometrySink sink);
            if (hr != HRESULT.S_OK)
            {
                SafeRelease(ref mergedPath);
                SafeRelease(ref format);
                SafeRelease(ref layout);
                return hr;
            }

            // --- 5. Add each glyph geometry ---
            foreach (var g in renderer.GlyphGeometries)
            {
                // Transform glyph geometry into layout space
                D2D1_MATRIX_3X2_F mat = new D2D1_MATRIX_3X2_F { _11 = 1, _22 = 1, _12 = 0, _21 = 0, _31 = 0, _32 = 0 };

                g.Simplify(
                    D2D1_GEOMETRY_SIMPLIFICATION_OPTION.D2D1_GEOMETRY_SIMPLIFICATION_OPTION_CUBICS_AND_LINES,
                    mat,
                    0.01f, // high precision for curves
                    sink
                );

                Marshal.ReleaseComObject(g);
            }

            sink.Close();
            geometry = mergedPath;

            // --- 6. Compute height ---
            DWRITE_TEXT_METRICS metrics;
            layout.GetMetrics(out metrics);
            computedHeight = metrics.height;

            if (computedHeight == 0)
                computedHeight = 60;

            SafeRelease(ref format);
            SafeRelease(ref layout);

            return HRESULT.S_OK;
        }

        // Improved with ChatGPT for arabic fonts
        unsafe HRESULT CreateDWriteTextGeometry(string sText, string sFontPath, float fontSizePoints, bool bFontSizeInPoints, bool bBold, bool bItalic,
        out ID2D1Geometry pGeometry, out float computedHeight)
        {
            pGeometry = null;
            computedHeight = 0;

            if (string.IsNullOrEmpty(sText))
                return HRESULT.S_OK;

            HRESULT hr;

            //-----------------------------------------
            // 1. Load font file & create font face
            //-----------------------------------------
            hr = m_pDWriteFactory7.CreateFontFileReference(sFontPath, IntPtr.Zero, out IDWriteFontFile pFontFile);
            if (!SUCCEEDED(hr)) return hr;

            pFontFile.Analyze(out _, out var fileType, out var faceType, out _);

            DWRITE_FONT_SIMULATIONS sims = 0;
            if (bBold) sims |= DWRITE_FONT_SIMULATIONS.DWRITE_FONT_SIMULATIONS_BOLD;
            if (bItalic) sims |= DWRITE_FONT_SIMULATIONS.DWRITE_FONT_SIMULATIONS_OBLIQUE;

            hr = m_pDWriteFactory7.CreateFontFace(faceType, 1, new[] { pFontFile }, 0, sims, out IDWriteFontFace pFontFace);
            if (!SUCCEEDED(hr)) return hr;

            //-----------------------------------------
            // 2. Create sText pTextAnalyzer
            //-----------------------------------------
            hr = m_pDWriteFactory7.CreateTextAnalyzer(out IDWriteTextAnalyzer pTextAnalyzer);
            if (!SUCCEEDED(hr)) return hr;

            //-----------------------------------------
            // 3. Pin sText for sTextAnalyzerSource (char*)
            //-----------------------------------------
            GCHandle sTextHandle = GCHandle.Alloc(sText, GCHandleType.Pinned);
            try
            {
                char* sTextPtr = (char*)sTextHandle.AddrOfPinnedObject().ToPointer();
                uint nTextLength = (uint)sText.Length;

                bool bRTL = ContainsStrongArabicCharacters(sText);

                var source = new TextAnalyzerSource(
                    sTextPtr,
                    nTextLength,
                    bRTL ? "ur-PK" : "en-US",
                    m_pDWriteFactory7,
                    bRTL,
                    bRTL ? "ur-PK" : "en-US",
                    false,
                    DWRITE_NUMBER_SUBSTITUTION_METHOD.DWRITE_NUMBER_SUBSTITUTION_METHOD_CONTEXTUAL);

                var sink = new SimpleTextAnalysisSink();

                //-----------------------------------------
                // 4. Analyze script & bidi
                //-----------------------------------------
                hr = pTextAnalyzer.AnalyzeScript(source, 0, (int)nTextLength, sink);
                if (!SUCCEEDED(hr)) return hr;

                hr = pTextAnalyzer.AnalyzeBidi(source, 0, (int)nTextLength, sink);
                if (!SUCCEEDED(hr)) return hr;

                //-----------------------------------------
                // 5. Compute font size in DIPs
                //-----------------------------------------
                float dpi = GetDpiForWindow(hWndMain);
                float emSize = bFontSizeInPoints ? fontSizePoints * dpi / 72.0f : fontSizePoints;

                //-----------------------------------------
                // 6. Create pPathpGeometry pGeometry
                //-----------------------------------------
                hr = m_pD2DFactory1.CreatePathGeometry(out ID2D1PathGeometry pPathGeometry);
                if (!SUCCEEDED(hr)) return hr;

                pPathGeometry.Open(out ID2D1GeometrySink pGeometrySink);

                //-----------------------------------------
                // 7. Process each run
                //-----------------------------------------
                foreach (var run in sink.Runs)
                {
                    uint nRunLength = (uint)run.Length;
                    uint nMaxGlyphs = nRunLength * 4;

                    ushort[] clusterMap = new ushort[nRunLength];
                    var sTextProps = new DWRITE_SHAPING_TEXT_PROPERTIES[nRunLength];
                    ushort[] glyphIndices = new ushort[nMaxGlyphs];
                    var glyphProps = new DWRITE_SHAPING_GLYPH_PROPERTIES[nMaxGlyphs];

                    bool bRunRtl = (run.BidiLevel & 1) != 0;
                    var script = run.Script;

                    IntPtr runPtr = (IntPtr)(sTextPtr + run.Start);
                    string sLocale = script.script == 6 /* Arabic */ ? "ur-PK" : "en-US";

                    // ---- Get glyphs ----
                    hr = pTextAnalyzer.GetGlyphs(
                        runPtr,
                        (int)nRunLength,
                        pFontFace,
                        false,
                        bRunRtl,
                        ref script,
                        sLocale,
                        null,
                        IntPtr.Zero,
                        0,
                        0,
                        (int)nMaxGlyphs,
                        clusterMap,
                        sTextProps,
                        glyphIndices,
                        glyphProps,
                        out uint nActualGlyphCount);
                    if (!SUCCEEDED(hr)) return hr;

                    // ---- Get glyph placements ----
                    float[] advances = new float[nActualGlyphCount];
                    var offsets = new DWRITE_GLYPH_OFFSET[nActualGlyphCount];

                    hr = pTextAnalyzer.GetGlyphPlacements(
                        runPtr,
                        clusterMap,
                        sTextProps,
                        nRunLength,
                        glyphIndices,
                        glyphProps,
                        nActualGlyphCount,
                        pFontFace,
                        emSize,
                        false,
                        bRunRtl,
                        ref script,
                        sLocale,
                        IntPtr.Zero,
                        IntPtr.Zero,
                        0,
                        advances,
                        offsets);
                    if (!SUCCEEDED(hr)) return hr;

                    // Add glyph run to Geometry
                    hr = pFontFace.GetGlyphRunOutline(
                        emSize,
                        glyphIndices,
                        advances,
                        offsets,
                        (int)nActualGlyphCount,
                        false,
                        bRunRtl,
                        (DWrite.ID2D1SimplifiedGeometrySink)pGeometrySink);
                    if (!SUCCEEDED(hr)) return hr;
                }

                pGeometrySink.Close();
                pGeometry = pPathGeometry;


                //-----------------------------------------
                // 8. Normalize bounds
                //-----------------------------------------
                pGeometry.GetBounds(null, out var bounds);

                float shiftX = bounds.left < 0 ? -bounds.left : 0;
                float shiftY = bounds.top < 0 ? -bounds.top : 0;

                if (shiftX != 0 || shiftY != 0)
                {
                    m_pD2DFactory1.CreateTransformedGeometry(pGeometry, Matrix3x2F.Translation(shiftX, shiftY), out ID2D1TransformedGeometry pNormalizedGeometry);
                    SafeRelease(ref pGeometry);
                    pGeometry = pNormalizedGeometry;
                }

                //-----------------------------------------
                // 9. Compute height
                //-----------------------------------------
                pFontFace.GetMetrics(out var metrics);
                float scale = emSize / metrics.designUnitsPerEm;
                computedHeight = (metrics.ascent + metrics.descent + metrics.lineGap) * scale;

                SafeRelease(ref pFontFace);
                SafeRelease(ref pGeometrySink);
                SafeRelease(ref pTextAnalyzer);
                SafeRelease(ref pFontFile);

                return HRESULT.S_OK;
            }
            finally
            {
                sTextHandle.Free();
            }
        }

        // From Copilot
        private static bool ContainsStrongArabicCharacters(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            foreach (char c in s)
            {
                int code = c;
                // Arabic block ranges (not exhaustive): 0x0600–0x06FF, 0x0750–0x077F, 0x08A0–0x08FF, Arabic Presentation Forms: 0xFB50–0xFDFF, 0xFE70–0xFEFF
                if ((code >= 0x0600 && code <= 0x06FF) ||
                    (code >= 0x0750 && code <= 0x077F) ||
                    (code >= 0x08A0 && code <= 0x08FF) ||
                    (code >= 0xFB50 && code <= 0xFDFF) ||
                    (code >= 0xFE70 && code <= 0xFEFF))
                    return true;
            }
            return false;
        }   

        private static IEnumerable<uint> ToCodePoints(string sText)
        {
            if (string.IsNullOrEmpty(sText))
                yield break;
            for (int i = 0; i < sText.Length; i++)
            {
                if (char.IsSurrogate(sText, i))
                    if (char.IsSurrogatePair(sText, i))
                        yield return (uint)char.ConvertToUtf32(sText, i++);
                    else
                        yield return sText[i];
                else
                    yield return sText[i];
            }
        }
  

        private void scp1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Resize(e.NewSize);           
        }

        HRESULT Resize(Windows.Foundation.Size sz)
        {
            HRESULT hr = HRESULT.S_OK;
            if (m_pDXGISwapChain1 != null)
            {
                if (m_pD2DDeviceContext != null)
                    m_pD2DDeviceContext.SetTarget(null);

                if (m_pD2DTargetBitmap != null)
                    SafeRelease(ref m_pD2DTargetBitmap);

                // 0, 0 => HRESULT: 0x80070057 (E_INVALIDARG) if not CreateSwapChainForHwnd
                //hr = m_pDXGISwapChain1.ResizeBuffers(
                // 2,
                // 0,
                // 0,
                // DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM,
                // 0
                // );
                if (sz.Width != 0 && sz.Height != 0)
                {
                    hr = m_pDXGISwapChain1.ResizeBuffers(
                      2,
                      (uint)sz.Width,
                      (uint)sz.Height,
                      DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM,
                      0
                      );
                }
                ConfigureSwapChain(hWndMain);
            }
            return (hr);
        }

        public HRESULT CreateD2D1Factory()
        {
            HRESULT hr = HRESULT.S_OK;
            D2D1_FACTORY_OPTIONS options = new D2D1_FACTORY_OPTIONS();

            // Needs "Enable native code Debugging"
#if DEBUG
            options.debugLevel = D2D1_DEBUG_LEVEL.D2D1_DEBUG_LEVEL_INFORMATION;
#endif

            hr = D2DTools.D2D1CreateFactory(D2D1_FACTORY_TYPE.D2D1_FACTORY_TYPE_SINGLE_THREADED, ref D2DTools.CLSID_D2D1Factory, ref options, out m_pD2DFactory);
            m_pD2DFactory1 = (ID2D1Factory1)m_pD2DFactory;
            return hr;
        }

        public HRESULT CreateDeviceContext()
        {
            HRESULT hr = HRESULT.S_OK;
            uint creationFlags = (uint)D3D11_CREATE_DEVICE_FLAG.D3D11_CREATE_DEVICE_BGRA_SUPPORT;

            // Needs "Enable native code Debugging"
#if DEBUG
            creationFlags |= (uint)D3D11_CREATE_DEVICE_FLAG.D3D11_CREATE_DEVICE_DEBUG;
#endif

            int[] aD3D_FEATURE_LEVEL = new int[] { (int)D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_11_1, (int)D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_11_0,
                (int)D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_10_1, (int)D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_10_0, (int)D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_9_3,
                (int)D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_9_2, (int)D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_9_1};

            D3D_FEATURE_LEVEL featureLevel;
            hr = D2DTools.D3D11CreateDevice(null,    // specify null to use the default adapter
                D3D_DRIVER_TYPE.D3D_DRIVER_TYPE_HARDWARE,
                IntPtr.Zero,
                creationFlags,      // optionally set debug and Direct2D compatibility flags
                aD3D_FEATURE_LEVEL, // list of feature levels this app can support
                (uint)aD3D_FEATURE_LEVEL.Length, // number of possible feature levels
                D2DTools.D3D11_SDK_VERSION,
                out m_pD3D11DevicePtr,    // returns the Direct3D device created
                out featureLevel,         // returns feature level of device created            
                out m_pD3D11DeviceContext // returns the device immediate context
            );
            if (SUCCEEDED(hr))
            {
                m_pDXGIDevice = Marshal.GetObjectForIUnknown(m_pD3D11DevicePtr) as IDXGIDevice1;
                if (m_pD2DFactory1 != null)
                {
                    ID2D1Device pD2DDevice = null;
                    hr = m_pD2DFactory1.CreateDevice(m_pDXGIDevice, out pD2DDevice);
                    if (SUCCEEDED(hr))
                    {
                        hr = pD2DDevice.CreateDeviceContext(D2D1_DEVICE_CONTEXT_OPTIONS.D2D1_DEVICE_CONTEXT_OPTIONS_NONE, out m_pD2DDeviceContext);
                        if (m_pD2DDeviceContext != null)
                            m_pD2DDeviceContext3 = (ID2D1DeviceContext3)m_pD2DDeviceContext;
                        SafeRelease(ref pD2DDevice);
                    }
                }
                //Marshal.Release(m_pD3D11DevicePtr);
            }
            return hr;
        }

        HRESULT CreateSwapChain(IntPtr hWnd)
        {
            HRESULT hr = HRESULT.S_OK;
            DXGI_SWAP_CHAIN_DESC1 swapChainDesc = new DXGI_SWAP_CHAIN_DESC1();
            swapChainDesc.Width = 1;
            swapChainDesc.Height = 1;
            swapChainDesc.Format = DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM; // this is the most common swapchain format
            swapChainDesc.Stereo = false;
            swapChainDesc.SampleDesc.Count = 1;                // don't use multi-sampling
            swapChainDesc.SampleDesc.Quality = 0;
            swapChainDesc.BufferUsage = D2DTools.DXGI_USAGE_RENDER_TARGET_OUTPUT;
            swapChainDesc.BufferCount = 2;                     // use double buffering to enable flip
            swapChainDesc.Scaling = (hWnd != IntPtr.Zero) ? DXGI_SCALING.DXGI_SCALING_NONE : DXGI_SCALING.DXGI_SCALING_STRETCH;
            swapChainDesc.SwapEffect = DXGI_SWAP_EFFECT.DXGI_SWAP_EFFECT_FLIP_SEQUENTIAL; // all apps must use this SwapEffect       
            swapChainDesc.Flags = 0;

            swapChainDesc.AlphaMode = DXGI_ALPHA_MODE.DXGI_ALPHA_MODE_PREMULTIPLIED;

            IDXGIAdapter pDXGIAdapter;
            hr = m_pDXGIDevice.GetAdapter(out pDXGIAdapter);
            if (SUCCEEDED(hr))
            {
                IntPtr pDXGIFactory2Ptr;
                hr = pDXGIAdapter.GetParent(typeof(IDXGIFactory2).GUID, out pDXGIFactory2Ptr);
                if (SUCCEEDED(hr))
                {
                    IDXGIFactory2 pDXGIFactory2 = Marshal.GetObjectForIUnknown(pDXGIFactory2Ptr) as IDXGIFactory2;
                    if (hWnd != IntPtr.Zero)
                        hr = pDXGIFactory2.CreateSwapChainForHwnd(m_pD3D11DevicePtr, hWnd, ref swapChainDesc, IntPtr.Zero, null, out m_pDXGISwapChain1);
                    else
                        hr = pDXGIFactory2.CreateSwapChainForComposition(m_pD3D11DevicePtr, ref swapChainDesc, null, out m_pDXGISwapChain1);

                    if (SUCCEEDED(hr))
                    {
                        hr = m_pDXGIDevice.SetMaximumFrameLatency(1);
                    }
                    SafeRelease(ref pDXGIFactory2);
                    Marshal.Release(pDXGIFactory2Ptr);
                }
                SafeRelease(ref pDXGIAdapter);
            }
            return hr;
        }

        HRESULT ConfigureSwapChain(IntPtr hWnd)
        {
            HRESULT hr = HRESULT.S_OK;

            //IntPtr pD3D11Texture2DPtr = IntPtr.Zero;
            //hr = m_pDXGISwapChain1.GetBuffer(0, typeof(ID3D11Texture2D).GUID, ref pD3D11Texture2DPtr);
            //m_pD3D11Texture2D = Marshal.GetObjectForIUnknown(pD3D11Texture2DPtr) as ID3D11Texture2D;

            D2D1_BITMAP_PROPERTIES1 bitmapProperties = new D2D1_BITMAP_PROPERTIES1();
            bitmapProperties.bitmapOptions = D2D1_BITMAP_OPTIONS.D2D1_BITMAP_OPTIONS_TARGET | D2D1_BITMAP_OPTIONS.D2D1_BITMAP_OPTIONS_CANNOT_DRAW;
            //bitmapProperties.pixelFormat = D2DTools.PixelFormat(DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM, D2D1_ALPHA_MODE.D2D1_ALPHA_MODE_IGNORE);
            bitmapProperties.pixelFormat = D2DTools.PixelFormat(DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM, D2D1_ALPHA_MODE.D2D1_ALPHA_MODE_PREMULTIPLIED);
            uint nDPI = GetDpiForWindow(hWnd);
            bitmapProperties.dpiX = nDPI;
            bitmapProperties.dpiY = nDPI;

            //bitmapProperties.dpiX = 96.0f;
            //bitmapProperties.dpiY = 96.0f;
            //double nScaleX = 96.0f / (double)nDPI;
            //double nScaleY = 96.0f / (double)nDPI;

            //double nScaleX = (double)nDPI / 96.0f;
            //double nScaleY = (double)nDPI / 96.0f;
            //scp1.RenderTransform = new ScaleTransform { ScaleX = nScaleX, ScaleY = nScaleY };
            //((Microsoft.UI.Xaml.Media.ScaleTransform)scp1.RenderTransform).CenterX = 0.5f;
            //((Microsoft.UI.Xaml.Media.ScaleTransform)scp1.RenderTransform).CenterY = 0.5f;

            IntPtr pDXGISurfacePtr = IntPtr.Zero;
            hr = m_pDXGISwapChain1.GetBuffer(0, typeof(IDXGISurface).GUID, out pDXGISurfacePtr);
            if (SUCCEEDED(hr))
            {
                IDXGISurface pDXGISurface = Marshal.GetObjectForIUnknown(pDXGISurfacePtr) as IDXGISurface;
                hr = m_pD2DDeviceContext.CreateBitmapFromDxgiSurface(pDXGISurface, ref bitmapProperties, out m_pD2DTargetBitmap);
                if (SUCCEEDED(hr))
                {
                    m_pD2DDeviceContext.SetTarget(m_pD2DTargetBitmap);
                }
                SafeRelease(ref pDXGISurface);
                Marshal.Release(pDXGISurfacePtr);
            }
            return hr;
        }

        //private void myButton_Click(object sender, RoutedEventArgs e)
        //{
        //    myButton.Content = "Clicked";
        //}

        HRESULT LoadBitmapFromFile(ID2D1DeviceContext3 pDeviceContext3, IWICImagingFactory pIWICFactory, string uri, uint destinationWidth,
            uint destinationHeight, out ID2D1Bitmap pD2DBitmap, out IWICBitmapSource pBitmapSource)
        {
            HRESULT hr = HRESULT.S_OK;
            pD2DBitmap = null;
            pBitmapSource = null;

            IWICBitmapDecoder pDecoder = null;
            IWICBitmapFrameDecode pSource = null;
            IWICFormatConverter pConverter = null;
            IWICBitmapScaler pScaler = null;

            hr = pIWICFactory.CreateDecoderFromFilename(uri, Guid.Empty, unchecked((int)GENERIC_READ), WICDecodeOptions.WICDecodeMetadataCacheOnLoad, out pDecoder);
            if (SUCCEEDED(hr))
            {
                hr = pDecoder.GetFrame(0, out pSource);
                if (SUCCEEDED(hr))
                {
                    hr = pIWICFactory.CreateFormatConverter(out pConverter);
                    if (SUCCEEDED(hr))
                    {
                        if (destinationWidth != 0 || destinationHeight != 0)
                        {
                            uint originalWidth, originalHeight;
                            hr = pSource.GetSize(out originalWidth, out originalHeight);
                            if (SUCCEEDED(hr))
                            {
                                if (destinationWidth == 0)
                                {
                                    float scalar = (float)(destinationHeight) / (float)(originalHeight);
                                    destinationWidth = (uint)(scalar * (float)(originalWidth));
                                }
                                else if (destinationHeight == 0)
                                {
                                    float scalar = (float)(destinationWidth) / (float)(originalWidth);
                                    destinationHeight = (uint)(scalar * (float)(originalHeight));
                                }
                                hr = pIWICFactory.CreateBitmapScaler(out pScaler);
                                if (SUCCEEDED(hr))
                                {
                                    hr = pScaler.Initialize(pSource, destinationWidth, destinationHeight, WICBitmapInterpolationMode.WICBitmapInterpolationModeCubic);
                                    if (SUCCEEDED(hr))
                                    {
                                        hr = pConverter.Initialize(pScaler, GUID_WICPixelFormat32bppPBGRA, WICBitmapDitherType.WICBitmapDitherTypeNone, null, 0.0f, WICBitmapPaletteType.WICBitmapPaletteTypeMedianCut);
                                        //hr = pConverter.Initialize(pScaler, GUID_WICPixelFormat32bppBGRA, WICBitmapDitherType.WICBitmapDitherTypeNone, null, 0.0f, WICBitmapPaletteType.WICBitmapPaletteTypeMedianCut);
                                    }
                                    Marshal.ReleaseComObject(pScaler);
                                }
                            }
                        }
                        else // Don't scale the image.
                        {
                            hr = pConverter.Initialize(pSource, GUID_WICPixelFormat32bppPBGRA, WICBitmapDitherType.WICBitmapDitherTypeNone, null, 0.0f, WICBitmapPaletteType.WICBitmapPaletteTypeMedianCut);
                            //hr = pConverter.Initialize(pSource, GUID_WICPixelFormat32bppBGRA, WICBitmapDitherType.WICBitmapDitherTypeNone, null, 0.0f, WICBitmapPaletteType.WICBitmapPaletteTypeMedianCut);
                        }

                        // Create a Direct2D bitmap from the WIC bitmap.
                        D2D1_BITMAP_PROPERTIES bitmapProperties = new D2D1_BITMAP_PROPERTIES();
                        bitmapProperties.pixelFormat = D2DTools.PixelFormat(DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM, D2D1_ALPHA_MODE.D2D1_ALPHA_MODE_PREMULTIPLIED);
                        bitmapProperties.dpiX = 96;
                        bitmapProperties.dpiY = 96;
                        hr = pDeviceContext3.CreateBitmapFromWicBitmap(pConverter, bitmapProperties, out pD2DBitmap);

                        //if (pBitmapSource != null)
                        pBitmapSource = pConverter;
                    }
                    Marshal.ReleaseComObject(pSource);
                }
                Marshal.ReleaseComObject(pDecoder);
            }
            return hr;
        }

        private void SetEffectFloat(ID2D1Effect pEffect, uint nEffect, float fValue)
        {
            float[] aFloatArray = { fValue };
            int nDataSize = aFloatArray.Length * Marshal.SizeOf(typeof(float));
            IntPtr pData = Marshal.AllocHGlobal(nDataSize);
            Marshal.Copy(aFloatArray, 0, pData, aFloatArray.Length);
            HRESULT hr = pEffect.SetValue(nEffect, D2D1_PROPERTY_TYPE.D2D1_PROPERTY_TYPE_UNKNOWN, pData, (uint)nDataSize);
            Marshal.FreeHGlobal(pData);
        }

        private void SetEffectFloatArray(ID2D1Effect pEffect, uint nEffect, float[] aFloatArray)
        {
            int nDataSize = aFloatArray.Length * Marshal.SizeOf(typeof(float));
            IntPtr pData = Marshal.AllocHGlobal(nDataSize);
            Marshal.Copy(aFloatArray, 0, pData, aFloatArray.Length);
            HRESULT hr = pEffect.SetValue(nEffect, D2D1_PROPERTY_TYPE.D2D1_PROPERTY_TYPE_UNKNOWN, pData, (uint)nDataSize);
            Marshal.FreeHGlobal(pData);
        }

        private void SetEffectInt(ID2D1Effect pEffect, uint nEffect, uint nValue)
        {
            IntPtr pData = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Int32)));
            Marshal.WriteInt32(pData, (int)nValue);
            HRESULT hr = pEffect.SetValue(nEffect, D2D1_PROPERTY_TYPE.D2D1_PROPERTY_TYPE_UNKNOWN, pData, (uint)Marshal.SizeOf(typeof(Int32)));
            Marshal.FreeHGlobal(pData);
        }

        private void FontListCustom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                Font font = (Font)e.AddedItems[0];
                if (font != null)
                {
                    SafeRelease(ref m_pD2DGeometry2);
                    bool bBold = false;
                    if (font.WeightValue >= 500)
                        bBold = true;
                    bool bItalic = false;
                    if (font.Style == "Italic" || font.Style == "Oblique")
                        bItalic = true;
                    CreateDWriteTextGeometry("This is a text with shadow", font.FullPath, 80.0f, true, bBold, bItalic, out m_pD2DGeometry2, out m_nComputedHeight2);
                    FontListSystem.SelectedIndex = -1;
                }
            }
        }

        private void FontListSystem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                Font font = (Font)e.AddedItems[0];
                if (font != null)
                {
                    SafeRelease(ref m_pD2DGeometry2);
                    bool bBold = false; 
                    if (font.WeightValue >= 500)
                        bBold = true;
                    bool bItalic = false;
                    if (font.Style == "Italic" || font.Style == "Oblique")
                        bItalic = true;
                    CreateDWriteTextGeometry("This is a text with shadow", font.FullPath, 80.0f, true,bBold, bItalic, out m_pD2DGeometry2, out m_nComputedHeight2);
                    FontListCustom.SelectedIndex = -1;
                }
            }
        }

        HRESULT CreateDeviceResources()
        {
            HRESULT hr = HRESULT.S_OK;
            if (m_pD2DDeviceContext != null)
            {
                if (m_pD2DMainBrush == null)
                {
                    hr = m_pD2DDeviceContext.CreateSolidColorBrush(new ColorF(ColorF.Enum.Black, 1.0f), BrushProperties(), out m_pD2DMainBrush);
                }
                if (m_pD2DSolidColorBrushRed == null)
                    hr = m_pD2DDeviceContext.CreateSolidColorBrush(new ColorF(ColorF.Enum.Red, 1.0f), BrushProperties(), out m_pD2DSolidColorBrushRed);
                if (m_pD2DSolidColorBrushGreen == null)
                    hr = m_pD2DDeviceContext.CreateSolidColorBrush(new ColorF(ColorF.Enum.Lime, 1.0f), BrushProperties(), out m_pD2DSolidColorBrushGreen);
                if (m_pD2DSolidColorBrushBlue == null)
                    hr = m_pD2DDeviceContext.CreateSolidColorBrush(new ColorF(ColorF.Enum.Blue, 1.0f), BrushProperties(), out m_pD2DSolidColorBrushBlue);
                if (m_pD2DSolidColorBrushWhite == null)
                    hr = m_pD2DDeviceContext.CreateSolidColorBrush(new ColorF(ColorF.Enum.White, 1.0f), BrushProperties(), out m_pD2DSolidColorBrushWhite);
                if (m_pD2DSolidColorBrushPink == null)
                    hr = m_pD2DDeviceContext.CreateSolidColorBrush(new ColorF(ColorF.Enum.DeepPink, 1.0f), BrushProperties(), out m_pD2DSolidColorBrushPink);

                if (m_pD2DLinearGradientBrush1 == null)
                {
                    ID2D1GradientStopCollection pGSC = null;
                    D2D1_GRADIENT_STOP[] gs = new D2D1_GRADIENT_STOP[] {
                        GradientStop(0.0f, new ColorF(ColorF.Enum.Red, 1.0f)),
                        GradientStop(0.5f, new ColorF(ColorF.Enum.Orange, 1.0f)),
                        GradientStop(1.0f, new ColorF(ColorF.Enum.Yellow, 1.0f))
                    };
                    hr = m_pD2DDeviceContext.CreateGradientStopCollection(gs, 3, D2D1_GAMMA.D2D1_GAMMA_2_2, D2D1_EXTEND_MODE.D2D1_EXTEND_MODE_MIRROR, out pGSC);
                    if (SUCCEEDED(hr))
                    {
                        var lgbp = new D2D1_LINEAR_GRADIENT_BRUSH_PROPERTIES(new Direct2D.D2D1_POINT_2F(0.0f, m_nComputedHeight1 / 3.0f), new Direct2D.D2D1_POINT_2F(0.0f, m_nComputedHeight1));
                        hr = m_pD2DDeviceContext.CreateLinearGradientBrush(ref lgbp, IntPtr.Zero, pGSC, out m_pD2DLinearGradientBrush1);
                        SafeRelease(ref pGSC);
                    }
                }
                if (m_pD2DLinearGradientBrush2 == null)
                {
                    ID2D1GradientStopCollection pGSC = null;
                    D2D1_GRADIENT_STOP[] gs = new D2D1_GRADIENT_STOP[] {
                        GradientStop(0.0f, new ColorF(ColorF.Enum.Black, 1.0f)),                       
                        //GradientStop(1.0f, new ColorF(ColorF.Enum.DimGray, 1.0f))
                        GradientStop(1.0f, new ColorF(0x3B/255.0f, 0x3B/255.0f,0x3B/255.0f, 1.0f))
                    };
                    hr = m_pD2DDeviceContext.CreateGradientStopCollection(gs, 2, D2D1_GAMMA.D2D1_GAMMA_2_2, D2D1_EXTEND_MODE.D2D1_EXTEND_MODE_CLAMP, out pGSC);
                    if (SUCCEEDED(hr))
                    {
                        var lgbp = new D2D1_LINEAR_GRADIENT_BRUSH_PROPERTIES(new Direct2D.D2D1_POINT_2F(0.0f, 0.0f), new Direct2D.D2D1_POINT_2F(0.0f, 1.0f));
                        hr = m_pD2DDeviceContext.CreateLinearGradientBrush(ref lgbp, IntPtr.Zero, pGSC, out m_pD2DLinearGradientBrush2);
                        SafeRelease(ref pGSC);
                    }
                }

                string sExePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                IWICBitmapSource pWICBitmapSource1 = null;
                string sAbsolutePath = "/Assets/Blood.jpg";
                if (sAbsolutePath.StartsWith("/"))
                    sAbsolutePath = sExePath + sAbsolutePath;
                hr = LoadBitmapFromFile(m_pD2DDeviceContext3, m_pWICImagingFactory, sAbsolutePath, 0, 0, out m_pD2DBitmap1, out pWICBitmapSource1);
                SafeRelease(ref pWICBitmapSource1);

                if (m_pD2DBitmap1 != null)
                {
                    if (m_pD2DBitmapBrush1 == null)
                    {
                        hr = m_pD2DDeviceContext.CreateBitmapBrush(m_pD2DBitmap1, BitmapBrushProperties(D2D1_EXTEND_MODE.D2D1_EXTEND_MODE_WRAP, D2D1_EXTEND_MODE.D2D1_EXTEND_MODE_WRAP), BrushProperties(), out m_pD2DBitmapBrush1);
                    }
                }

                IWICBitmapSource pWICBitmapSource2 = null;
                sAbsolutePath = "/Assets/BlueNoise.jpg";
                if (sAbsolutePath.StartsWith("/"))
                    sAbsolutePath = sExePath + sAbsolutePath;
                hr = LoadBitmapFromFile(m_pD2DDeviceContext3, m_pWICImagingFactory, sAbsolutePath,
                   0, 0, out m_pD2DBitmap2, out pWICBitmapSource1);
                SafeRelease(ref pWICBitmapSource2);
            }
            return hr;
        }

        void CleanDeviceResources()
        {
            SafeRelease(ref m_pD2DBitmap2);
            SafeRelease(ref m_pD2DBitmap1);           
            SafeRelease(ref m_pD2DBitmapBrush1);
            SafeRelease(ref m_pD2DSolidColorBrushRed);
            SafeRelease(ref m_pD2DSolidColorBrushGreen);
            SafeRelease(ref m_pD2DSolidColorBrushBlue);
            SafeRelease(ref m_pD2DSolidColorBrushWhite);
            SafeRelease(ref m_pD2DSolidColorBrushPink);
            SafeRelease(ref m_pD2DLinearGradientBrush1);
            SafeRelease(ref m_pD2DLinearGradientBrush2);
            SafeRelease(ref m_pD2DMainBrush);
        }

        void Clean()
        {
            if (m_pCTR != null)
                m_pCTR.Dispose();
            CleanDeviceResources();

            SafeRelease(ref m_pTextLayout);
            
            SafeRelease(ref m_pD2DGeometry1);
            SafeRelease(ref m_pD2DGeometry2);
            SafeRelease(ref m_pD2DGeometry3);
            SafeRelease(ref m_pD2DGeometry4);
            SafeRelease(ref m_pD2DGeometry5);
            SafeRelease(ref m_pD2DGeometry6);
            SafeRelease(ref m_pD2DGeometry7);

            SafeRelease(ref m_pDWriteFactory7);

            SafeRelease(ref m_pD2DTargetBitmap);
            SafeRelease(ref m_pDXGISwapChain1);

            SafeRelease(ref m_pD2DDeviceContext);

            SafeRelease(ref m_pD3D11DeviceContext);
            if (m_pD3D11DevicePtr != IntPtr.Zero)
                Marshal.Release(m_pD3D11DevicePtr);
            SafeRelease(ref m_pDXGIDevice);

            SafeRelease(ref m_pWICImagingFactory);
            SafeRelease(ref m_pD2DFactory1);
            SafeRelease(ref m_pD2DFactory);
        }

        private void MainWindow_Closed(object sender, WindowEventArgs args)
        {
            Clean();
        }
    }

    public class Font
    {
        #region Properties
        public string Name { get; set; }
        public string FullPath { get; set; }
        public string FontFamilyPath { get; set; }
        public string FontWeight { get; set; }
        public int WeightValue { get; set; }
        public string Style { get; set; }
        public string Stretch { get; set; }
        #endregion

        public Font(string sName, string sFullPath, string sFontFamilyPath, string sFontWeight = "Normal", int nWeightValue = 400, string sStyle = "Normal", string sStretch = "Normal")
        {
            Name = sName;
            FullPath = sFullPath;
            FontFamilyPath = sFontFamilyPath;
            FontWeight = sFontWeight;
            WeightValue = nWeightValue;
            Style = sStyle;
            Stretch = sStretch;
        }
    }

    public class NavItem
    {
        public IconElement Icon { get; set; }
        public string Text { get; set; }
        public string Tag { get; set; }
    }


    // From WPF source code
    public unsafe class TextAnalyzerSource : IDWriteTextAnalysisSource
    {
        struct TextSegment
        {
            internal IntPtr ptr;
            internal uint start;
            internal uint end;
        }

        uint TextLength;

        IDWriteFactory _factory;
        TextSegment[] TextSegments;
        GCHandle PinnedLocaleName;
        DWRITE_READING_DIRECTION ReadingDirection;

        string _numberCulture;
        bool _ignoreUserOverride;
        DWRITE_NUMBER_SUBSTITUTION_METHOD _numberSubstitutionMethod;

        internal TextAnalyzerSource(char* text, uint length, string culture, IDWriteFactory factory, bool isRightToLeft, string numberCulture,
            bool ignoreUserOverride, DWRITE_NUMBER_SUBSTITUTION_METHOD numberSubstitutionMethod)
        {
            var segment = new TextSegment();
            segment.ptr = new IntPtr(text);
            segment.start = 0;
            segment.end = length;
            TextSegments = new TextSegment[] { segment };
            TextLength = length;
            PinnedLocaleName = GCHandle.Alloc(culture, GCHandleType.Pinned);
            ReadingDirection = isRightToLeft ? DWRITE_READING_DIRECTION.DWRITE_READING_DIRECTION_RIGHT_TO_LEFT : DWRITE_READING_DIRECTION.DWRITE_READING_DIRECTION_LEFT_TO_RIGHT;
            _factory = factory;
            _numberCulture = numberCulture;
            _ignoreUserOverride = ignoreUserOverride;
            _numberSubstitutionMethod = numberSubstitutionMethod;
        }

        internal TextAnalyzerSource(IntPtr[] text_ptrs, uint[] lengths, string culture, IDWriteFactory factory, bool isRightToLeft, string numberCulture,
            bool ignoreUserOverride, DWRITE_NUMBER_SUBSTITUTION_METHOD numberSubstitutionMethod)
        {
            TextSegments = new TextSegment[text_ptrs.Length];
            uint pos = 0;
            for (int i = 0; i < text_ptrs.Length; i++)
            {
                var segment = new TextSegment();
                segment.ptr = text_ptrs[i];
                segment.start = pos;
                pos += lengths[i];
                segment.end = pos;
                TextSegments[i] = segment;
            }
            TextLength = pos;
            PinnedLocaleName = GCHandle.Alloc(culture, GCHandleType.Pinned);
            ReadingDirection = isRightToLeft ? DWRITE_READING_DIRECTION.DWRITE_READING_DIRECTION_RIGHT_TO_LEFT : DWRITE_READING_DIRECTION.DWRITE_READING_DIRECTION_LEFT_TO_RIGHT;
            _factory = factory;
            _numberCulture = numberCulture;
            _ignoreUserOverride = ignoreUserOverride;
            _numberSubstitutionMethod = numberSubstitutionMethod;
        }

        private bool FindSegmentAtPosition(uint position, out int segment_index)
        {
            int first = 0;
            int last = TextSegments.Length - 1;

            if (position < 0 || position >= TextSegments[last].end)
            {
                segment_index = -1;
                return false;
            }

            // binary search
            while (last > first)
            {
                int candidate = (first + last) / 2;
                var segment = TextSegments[candidate];
                if (position < segment.start)
                    last = candidate - 1;
                else if (position >= segment.end)
                    first = candidate + 1;
                else
                {
                    segment_index = candidate;
                    return true;
                }
            }

            // The array of segments covers the full range, so we can assume we found it.
            segment_index = first;
            return true;
        }

        public HRESULT GetTextAtPosition(uint position, out IntPtr text, out uint text_len)
        {
            int index;
            if (FindSegmentAtPosition(position, out index))
            {
                var segment = TextSegments[index];
                text = IntPtr.Add(segment.ptr, (int)(position - segment.start) * 2);
                text_len = segment.end - position;
            }
            else
            {
                text = IntPtr.Zero;
                text_len = 0;
            }
            return HRESULT.S_OK;
        }

        public HRESULT GetTextBeforePosition(uint position, out IntPtr text, out uint text_len)
        {
            int index;
            if (position != 0 && FindSegmentAtPosition(position - 1, out index))
            {
                var segment = TextSegments[index];
                text = segment.ptr;
                text_len = position - segment.start;
            }
            else
            {
                text = IntPtr.Zero;
                text_len = 0;
            }
            return HRESULT.S_OK;
        }

        public DWRITE_READING_DIRECTION GetParagraphReadingDirection()
        {
            return ReadingDirection;
        }

        public HRESULT GetLocaleName(uint position, out uint text_len, out IntPtr locale)
        {
            text_len = TextLength - position;
            locale = PinnedLocaleName.AddrOfPinnedObject();
            return HRESULT.S_OK;
        }

        public HRESULT GetNumberSubstitution(uint position, out uint text_len, out IDWriteNumberSubstitution substitution)
        {
            text_len = TextLength - position;
            _factory.CreateNumberSubstitution(_numberSubstitutionMethod, _numberCulture, _ignoreUserOverride, out substitution);
            return HRESULT.S_OK;
        }
    }


    sealed class SimpleTextAnalysisSink : IDWriteTextAnalysisSink
    {
        public struct TextRun
        {
            public int Start;
            public int Length;
            public DWRITE_SCRIPT_ANALYSIS Script;
            public byte BidiLevel;
        }

        public readonly List<TextRun> Runs = new();

        // ----------------------------------------------------------------
        // IDWriteTextAnalysisSink implementation
        // ----------------------------------------------------------------

        public HRESULT SetScriptAnalysis(uint textPosition, uint textLength, ref DWRITE_SCRIPT_ANALYSIS scriptAnalysis)
        {
            Runs.Add(new TextRun
            {
                Start = (int)textPosition,
                Length = (int)textLength,
                Script = scriptAnalysis,
                BidiLevel = 0 // will be set later
            });
            return HRESULT.S_OK;
        }       

        public HRESULT SetBidiLevel(uint textPosition, uint textLength, byte explicitLevel, byte resolvedLevel)
        {
            for (int i = 0; i < Runs.Count; i++)
            {
                var r = Runs[i];
                if (textPosition <= r.Start && textPosition + textLength >= r.Start + r.Length)
                {
                    r.BidiLevel = resolvedLevel;
                    Runs[i] = r;
                }
            }
            return HRESULT.S_OK;
        }

        public HRESULT SetLineBreakpoints(uint textPosition, uint textLength, DWRITE_LINE_BREAKPOINT[] lineBreakpoints)
        {
            // We don’t need line breakpoints for geometry, so just ignore
            return HRESULT.S_OK;
        }

        public HRESULT SetNumberSubstitution(uint textPosition, uint textLength, IDWriteNumberSubstitution numberSubstitution)
        {
            // Optional: ignore number substitution
            return HRESULT.S_OK;
        }
    }


}
