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
using Microsoft.UI.Xaml.Shapes;
using System.Text;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Xml.Linq;
using Windows.Graphics.Imaging;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Collections;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI3_SwapChainPanel_DWriteCore
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int GetUserDefaultLocaleName(StringBuilder lpLocaleName, int cchLocaleName);

        public const int LOCALE_NAME_MAX_LENGTH = 85;

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr GetCapture();


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

        // To test UnpackFontFile

        //public struct PackedFontInfo
        //{
        //    public byte[] FontData;      // Equivalent to `g_packedFontData1`
        //    public int FontDataSize;     // Size of the font data
        //    public DWRITE_CONTAINER_TYPE FileContainer; // Container type, equivalent to `DWRITE_CONTAINER_TYPE_WOFF2`
        //}

        //static readonly byte[] g_packedFontData1 = new byte[]
        //{
        //    0x77, 0x4F, 0x46, 0x32,   0x00, 0x01, 0x00, 0x00,   0x00, 0x00, 0x03, 0xA4,   0x00, 0x0A, 0x00, 0x00,
        //    0x00, 0x00, 0x07, 0x4C,   0x00, 0x00, 0x03, 0x59,   0x00, 0x00, 0x00, 0x00,   0x00, 0x00, 0x00, 0x00,
        //    0x00, 0x00, 0x00, 0x00,   0x00, 0x00, 0x00, 0x00,   0x00, 0x00, 0x00, 0x00,   0x00, 0x00, 0x00, 0x00,
        //    0x06, 0x60, 0x00, 0x4C,   0x0A, 0x82, 0x78, 0x82,   0x47, 0x01, 0x36, 0x02,   0x24, 0x03, 0x16, 0x0B,
        //    0x0E, 0x00, 0x04, 0x20,   0x05, 0x87, 0x38, 0x07,   0x20, 0x1B, 0x5A, 0x06,   0x00, 0x06, 0x1E, 0xE8,
        //    0xBD, 0xEC, 0xCD, 0x24,   0xE9, 0xA6, 0xD9, 0x1A,   0x69, 0x7D, 0x02, 0x4B,   0x1E, 0x44, 0xEC, 0x9E,
        //    0xF1, 0x0F, 0xE7, 0xEA,   0x52, 0xBE, 0x09, 0x48,   0x5E, 0xE0, 0x40, 0x64,   0xFF, 0xFB, 0x7E, 0xD5,
        //    0x6C, 0x0A, 0xD5, 0x42,   0xB5, 0x03, 0xBE, 0x68,   0x0A, 0x77, 0x8A, 0x76,   0x8E, 0x48, 0x38, 0x18,
        //    0x8A, 0x87, 0x34, 0xBA,   0x96, 0x78, 0xC2, 0xA3,   0x5F, 0xAB, 0x0A, 0x21,   0x40, 0xB7, 0x33, 0x50,
        //    0xD4, 0x42, 0x31, 0x17,   0x0D, 0x5D, 0x5B, 0xAE,   0x55, 0x02, 0x11, 0xBA,   0xFE, 0xB6, 0x72, 0x8F,
        //    0xCE, 0xD2, 0x47, 0xD7,   0xA4, 0x5D, 0x93, 0xAA,   0x54, 0xA5, 0x54, 0x00,   0xC1, 0x74, 0x4D, 0x9B,
        //    0x24, 0xD3, 0xA4, 0x3E,   0x26, 0xE7, 0xB4, 0x07,   0xFF, 0x3B, 0x08, 0x77,   0x7D, 0xBD, 0xFC, 0x2B,
        //    0x49, 0xA3, 0xA2, 0x2D,   0x1F, 0x68, 0x1A, 0x93,   0x40, 0xB9, 0xC9, 0xBA,   0xA1, 0x52, 0x5F, 0x4E,
        //    0xEB, 0x48, 0x54, 0x4C,   0x62, 0xA9, 0x40, 0x03,   0xCE, 0x59, 0x96, 0xBD,   0x7A, 0xF2, 0x06, 0xEB,
        //    0x8D, 0xF5, 0xC6, 0x7A,   0x60, 0x9D, 0xB3, 0x1A,   0x46, 0x15, 0x35, 0xE0,   0xA2, 0xBB, 0x09, 0x73,
        //    0x65, 0x45, 0xD6, 0x96,   0x5B, 0x47, 0xB5, 0x43,   0x19, 0xF5, 0x77, 0x14,   0xDD, 0x50, 0x75, 0xC3,
        //    0x16, 0x6F, 0xB3, 0x8D,   0xF0, 0x48, 0x37, 0x46,   0xD8, 0x87, 0x19, 0x99,   0xDD, 0x17, 0xAF, 0x0F,
        //    0x31, 0x16, 0x3E, 0xF4,   0xCB, 0x1E, 0x61, 0xA4,   0xD9, 0x87, 0xDE, 0xFC,   0xC5, 0xF2, 0x53, 0x8B,
        //    0xEC, 0xC3, 0x8C, 0x3F,   0x0E, 0x07, 0x63, 0x1F,   0x26, 0x63, 0x08, 0xE3,   0xA1, 0x33, 0x35, 0xCD,
        //    0xE1, 0xB0, 0xDB, 0x35,   0xA7, 0xD3, 0x29, 0xC6,   0x3D, 0x4A, 0x13, 0x76,   0x1C, 0x29, 0xC9, 0xF1,
        //    0xD7, 0x9D, 0xA9, 0xF2,   0xAD, 0x23, 0x45, 0x79,   0xF8, 0x90, 0x5F, 0x8F,   0xC4, 0x38, 0x67, 0xAA,
        //    0xAC, 0x5B, 0x50, 0x7D,   0x7D, 0xDF, 0x6A, 0x27,   0xA2, 0x3B, 0x2D, 0xDF,   0x1D, 0x3D, 0x69, 0xF9,
        //    0xF1, 0x1D, 0xB7, 0x95,   0x93, 0xF5, 0x1E, 0x37,   0x55, 0x23, 0xBC, 0x9B,   0x55, 0xFF, 0x7B, 0x92,
        //    0x72, 0x2E, 0xD9, 0xFB,   0x5E, 0x5E, 0x7A, 0x2C,   0xB4, 0xAF, 0xEB, 0xFB,   0x1A, 0xBE, 0x76, 0xF4,
        //    0xC0, 0x7F, 0x65, 0x8A,   0x62, 0x66, 0xCF, 0x98,   0x37, 0x00, 0xFF, 0x13,   0xBB, 0x29, 0xBB, 0x40,
        //    0x2E, 0x29, 0xEC, 0xBC,   0x61, 0xD8, 0xDA, 0x33,   0x37, 0xF6, 0x06, 0xDD,   0xEE, 0x7E, 0x59, 0xED,
        //    0xCE, 0xE9, 0x8A, 0xC3,   0x6B, 0xD6, 0x5F, 0xD8,   0xB7, 0xC6, 0xFA, 0xD2,   0xED, 0x55, 0xDF, 0xA1,
        //    0xE5, 0xE7, 0x75, 0xEC,   0xB4, 0x64, 0xC8, 0xC6,   0x4B, 0x07, 0xB7, 0xFB,   0xEC, 0xEF, 0xBE, 0x6B,
        //    0x93, 0x5C, 0x5B, 0xAF,   0xFE, 0xCA, 0xA9, 0xCE,   0xCE, 0xC1, 0xCD, 0xB8,   0x5D, 0xFE, 0x4C, 0xA7,
        //    0xC2, 0xFC, 0x32, 0x4B,   0x33, 0x3D, 0x7A, 0x64,   0x76, 0x08, 0xAE, 0x34,   0x37, 0x6B, 0x78, 0x5A,
        //    0x46, 0x41, 0xAF, 0xAC,   0x52, 0x4F, 0x01, 0x40,   0x00, 0x7C, 0xEC, 0xDC,   0xD6, 0xBE, 0x32, 0x25,
        //    0xCF, 0xB3, 0xF6, 0x37,   0x14, 0x1D, 0x46, 0x86,   0x7B, 0x8D, 0x4F, 0x26,   0x6D, 0x36, 0x3E, 0x7C,
        //    0xD0, 0xB7, 0x0E, 0x41,   0x56, 0x39, 0xCD, 0xCF,   0x16, 0x3D, 0xE2, 0xD8,   0x90, 0xAD, 0xFC, 0x34,
        //    0xFA, 0xFA, 0x7F, 0xFF,   0x41, 0x2B, 0x49, 0x10,   0xFE, 0x9A, 0x5F, 0x83,   0x82, 0x9A, 0x56, 0xD2,
        //    0x93, 0x15, 0x34, 0x54,   0x82, 0xA0, 0x77, 0x95,   0xDA, 0x1E, 0x07, 0xC5,   0xB6, 0x05, 0x17, 0x2A,
        //    0xA3, 0x60, 0xF5, 0x9F,   0x42, 0x6E, 0x2C, 0x6E,   0x5B, 0x8B, 0xEA, 0x0A,   0x22, 0x17, 0xF6, 0x5C,
        //    0xE0, 0x25, 0x72, 0xB9,   0xC4, 0x43, 0xF4, 0xE6,   0x0A, 0xA6, 0x18, 0x39,   0xAD, 0xDA, 0x71, 0x8D,
        //    0x20, 0xB1, 0x72, 0xDA,   0xE5, 0x8E, 0xDB, 0x18,   0x2C, 0x0E, 0x72, 0x9D,   0x92, 0x78, 0x70, 0x57,
        //    0xFC, 0x64, 0x49, 0xEE,   0xCE, 0x52, 0x59, 0xF7,   0xD9, 0x1E, 0xB4, 0x96,   0x4E, 0xB6, 0x61, 0x52,
        //    0x91, 0xF2, 0x54, 0xA0,   0x1A, 0x26, 0x6D, 0xE8,   0x46, 0x11, 0xFD, 0xE9,   0xC3, 0x00, 0xFA, 0xD0,
        //    0x99, 0x81, 0x98, 0x34,   0xA2, 0x0F, 0xFD, 0xE9,   0xBB, 0xAA, 0x2D, 0x60,   0x20, 0xDD, 0xE8, 0x43,
        //    0xEF, 0x41, 0x89, 0x0C,   0x99, 0xFD, 0xCB, 0xD2,   0xCB, 0x53, 0x28, 0x4B,   0xD1, 0x63, 0xBD, 0x28,
        //    0x41, 0x69, 0x4C, 0x86,   0xD0, 0x8D, 0x81, 0x74,   0xC5, 0xA4, 0x23, 0x9D,   0x18, 0x40, 0x27, 0xFA,
        //    0x33, 0x58, 0xB3, 0xC5,   0x98, 0x34, 0x45, 0x84,   0x58, 0xDB, 0xDA, 0x52,   0x40, 0x2F, 0x3A, 0x61,
        //    0x92, 0x24, 0xD4, 0x93,   0x02, 0x5F, 0xFF, 0x1E,   0x94, 0x3D, 0xD5, 0x4D,   0x61, 0x03, 0xB2, 0x05,
        //    0x98, 0x0C, 0x64, 0xA6,   0x17, 0x6B, 0xB6, 0x17,   0x05, 0xF4, 0xB7, 0x00,   0x1E, 0x63, 0xCC, 0x07,
        //    0xBB, 0xAD, 0x80, 0x27,   0x70, 0xA0, 0x14, 0x7A,   0xB3, 0x96, 0x58, 0x52,   0x12, 0x03, 0xED, 0xAD,
        //    0x4E, 0x71, 0xB3, 0xBD,   0x29, 0xA6, 0x1C, 0x0A,   0x44, 0x93, 0xFC, 0x60,   0x0F, 0x59, 0xE3, 0xA0,
        //    0x7C, 0xA8, 0x3F, 0xDD,   0x3C, 0xAC, 0xB2, 0xDC,   0x1B, 0x87, 0xD0, 0x83,   0x24, 0x06, 0x51, 0x58,
        //    0x03, 0xEA, 0xC2, 0x20,   0x9D, 0xDF, 0xDF, 0x62,   0x9E, 0xF9, 0x60, 0x2A,   0xBA, 0x33, 0x80, 0xE9,
        //    0xFA, 0x97, 0xA7, 0x2C,   0x35, 0x28, 0x5F, 0x11,   0x25, 0xBF, 0x81, 0x95,   0xC1, 0x2E, 0x67, 0xB1,
        //    0x41, 0xF4, 0xF5, 0x90,   0x3A, 0x88, 0x7D, 0x33,   0x56, 0xF2, 0xC1, 0x06,   0xA8, 0x12, 0x1B, 0x6C,
        //    0xC8, 0x22, 0xDF, 0x93,   0x64, 0xBA, 0x32, 0xE8,   0xEA, 0xA5, 0x95, 0x84,   0x8D, 0x43, 0xD4, 0xE9,
        //    0xC4, 0x11, 0x7B, 0x46,   0x17, 0x3A, 0xD1, 0x5B,   0xAA, 0x2E, 0x54, 0xCC,   0x2A, 0xAE, 0xA0, 0xC2,
        //    0x31, 0x9D, 0x44, 0x0B,   0x5A, 0x2B, 0xB5, 0xA3,   0xAF, 0x7D, 0x0F, 0xFA,   0x6C, 0x6B, 0x14, 0xF8,
        //    0xBA, 0xFA, 0x56, 0x89,   0x55, 0x80, 0x77, 0xBA,   0x2B, 0x03, 0x19, 0xA8,   0x54, 0x93, 0x72, 0xFD,
        //    0x92, 0xF0, 0x20, 0x6B,   0x7D, 0x90, 0x00, 0x0E,   0x96, 0xED, 0xA2, 0x5F,   0x17, 0xCA, 0x71, 0x3B,
        //    0x36, 0x80, 0x72, 0xB4,   0xA3, 0x29, 0xAD, 0x29,   0x83, 0x28, 0xFD, 0xA6,   0x17, 0xC2, 0x3A, 0x44,
        //    0x45, 0x26, 0x00, 0x00
        //};

        //static readonly PackedFontInfo[] g_packedFonts = new PackedFontInfo[]
        //{
        //    new PackedFontInfo
        //    {
        //        FontData = g_packedFontData1,
        //        FontDataSize = g_packedFontData1.Length,
        //        FileContainer = DWRITE_CONTAINER_TYPE.DWRITE_CONTAINER_TYPE_WOFF2
        //    }
        //};

        //static readonly int g_packedFontsCount = g_packedFonts.Length;

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
            m_nHeight = 960;
            m_nXPos = (nDisplayWidth - m_nWidth) / 2;
            m_nYPos = (nDisplayHeight - m_nHeight) / 2;
            UpdateWindowSize(hWndMain);
            m_pWICImagingFactory = (IWICImagingFactory)Activator.CreateInstance(Type.GetTypeFromCLSID(WICTools.CLSID_WICImagingFactory));
            m_pWICImagingFactory2 = (IWICImagingFactory2)m_pWICImagingFactory;
            HRESULT hr = CreateD2D1Factory();
            if (hr == HRESULT.S_OK)
            {                
                IntPtr pDWriteFactoryPtr = IntPtr.Zero;
                //hr = DWriteCreateFactory(DWrite.DWRITE_FACTORY_TYPE.DWRITE_FACTORY_TYPE_SHARED, ref CLSID_DWriteFactory7, out pDWriteFactoryPtr);
                hr = DWriteCoreCreateFactory(DWrite.DWRITE_FACTORY_TYPE.DWRITE_FACTORY_TYPE_SHARED, ref CLSID_DWriteFactory7, out pDWriteFactoryPtr);

                if (hr == HRESULT.S_OK)
                {                   
                    m_pDWriteFactory7 = Marshal.GetObjectForIUnknown(pDWriteFactoryPtr) as IDWriteFactory7;

                ////https://learn.microsoft.com/en-us/windows/win32/directwrite/custom-font-sets-win10#using-local-woff-or-woff2-font-data
                //    IDWriteFontFileStream pFontStream = null;
                //    //IntPtr pFonts = Marshal.UnsafeAddrOfPinnedArrayElement(g_packedFonts[0].FontData, 0);

                //    IntPtr pFonts = Marshal.AllocHGlobal(g_packedFonts[0].FontData.Length); // Allocate unmanaged memory                  
                //    Marshal.Copy(g_packedFonts[0].FontData, 0, pFonts, g_packedFonts[0].FontData.Length);

                //    // E_NOTIMPL with DWriteCoreCreateFactory
                //    //hr = m_pDWriteFactory7.UnpackFontFile(g_packedFonts[0].FileContainer, pFonts, 
                //    //    (uint)g_packedFonts[0].FontDataSize, out pFontStream); 

                //    hr = m_pDWriteFactory7.UnpackFontFile(0, pFonts, 
                //        (uint)g_packedFonts[0].FontDataSize, out pFontStream);

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
                    if (hr == HRESULT.S_OK)
                    {
                        Marshal.FreeHGlobal(pFontCollectionKey);
                        IDWriteFontCollection2 pFontCollection2 = (IDWriteFontCollection2)pFontCollection;
                        if (pFontCollection2 != null)
                        {
                            IDWriteFontSet1 pFontSet1;
                            hr = pFontCollection2.GetFontSet(out pFontSet1);
                            if (hr == HRESULT.S_OK)
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
                        if (hr == HRESULT.S_OK)
                        {
                            var unorderedSystemFonts = new System.Collections.ObjectModel.ObservableCollection<Font>();
                            hr = LoadFonts(pFontSet2, unorderedSystemFonts, false);
                            SystemFonts = new ObservableCollection<Font>(unorderedSystemFonts.OrderBy(x => x.Name));
                            unorderedSystemFonts.Clear();
                            SafeRelease(ref pFontSet2);
                        }
                        
                        string sPathFont = System.IO.Path.Combine(sExePath, "Assets\\Lovely Home.ttf");
                        hr = CreateDWriteTextGeometry("This is a text with slow animated gradient", sPathFont, 60.0f, false, false, out m_pD2DGeometry1, out m_nComputedHeight1);

                        sPathFont = System.IO.Path.Combine(sExePath, "Assets\\Lemon Shake.ttf");
                        CreateDWriteTextGeometry("This is a text with shadow", sPathFont, 80.0f, false, false, out m_pD2DGeometry2, out m_nComputedHeight2);

                        sPathFont = System.IO.Path.Combine(sExePath, "Assets\\Play Story.otf");
                        CreateDWriteTextGeometry("This is a text with turbulence", sPathFont, 60.0f, false, false, out m_pD2DGeometry3, out m_nComputedHeight3);

                        sPathFont = System.IO.Path.Combine(sExePath, "Assets\\Simplisicky Fill.ttf");
                        CreateDWriteTextGeometry("This is a text with glowing", sPathFont, 80.0f, false, false, out m_pD2DGeometry4, out m_nComputedHeight4);

                        sPathFont = System.IO.Path.Combine(sExePath, "Assets\\Bloody Scene.otf");
                        CreateDWriteTextGeometry("This is a text with Bitmap brush", sPathFont, 70.0f, false, false, out m_pD2DGeometry5, out m_nComputedHeight5);

                        sPathFont = System.IO.Path.Combine(sExePath, "Assets\\Love Craft.ttf");
                        CreateDWriteTextGeometry("This is a scrolling text", sPathFont, 60.0f, false, false, out m_pD2DGeometry6, out m_nComputedHeight6);

                        sPathFont = System.IO.Path.Combine(sExePath, "Assets\\MagicSchoolOne.ttf");
                        CreateDWriteTextGeometry("This is a Magic School text", sPathFont, 90.0f, false, false, out m_pD2DGeometry7, out m_nComputedHeight7);

                        //string sFontName = "Segoe UI Emoji";
                        string sFontName = "Tolkien";

                        //string sString = "This is a text from Layout";
                        string sString = "";
                        for (int nCode = 0x1F980; nCode <= 0x1F980 + 15; nCode++)
                            sString += char.ConvertFromUtf32(nCode);

                        int nStringLength = sString.Length;
                        IDWriteTextFormat pTextFormat = null;
                        //hr = m_pDWriteFactory7.CreateTextFormat(sFontName, pFontCollection2,
                        hr = m_pDWriteFactory7.CreateTextFormat("Segoe UI Emoji", null,
                           DWRITE_FONT_WEIGHT.DWRITE_FONT_WEIGHT_NORMAL, DWRITE_FONT_STYLE.DWRITE_FONT_STYLE_NORMAL, DWRITE_FONT_STRETCH.DWRITE_FONT_STRETCH_NORMAL,
                           30, "", out pTextFormat);
                        if (hr == HRESULT.S_OK)
                        {
                            hr = pTextFormat.SetTextAlignment(DWRITE_TEXT_ALIGNMENT.DWRITE_TEXT_ALIGNMENT_CENTER);
                            hr = m_pDWriteFactory7.CreateTextLayout(sString, nStringLength, pTextFormat, 800, 60, out m_pTextLayout);
                            if (hr == HRESULT.S_OK)
                            {
                                //IDWriteTypography pTypography = null;
                                //hr = m_pDWriteFactory7.CreateTypography(out pTypography);
                                //if (hr == HRESULT.S_OK)
                                //{
                                //    DWRITE_FONT_FEATURE ff = new DWRITE_FONT_FEATURE (
                                //        DWRITE_FONT_FEATURE_TAG.DWRITE_FONT_FEATURE_TAG_STYLISTIC_SET_7, 1);
                                //    hr = pTypography.AddFontFeature(ff);
                                //    if (hr == HRESULT.S_OK)
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

                hr = CreateDeviceContext();
                hr = CreateDeviceResources();
                hr = CreateSwapChain(IntPtr.Zero);
                if (hr == HRESULT.S_OK)
                {
                    hr = ConfigureSwapChain(hWndMain);
                    ISwapChainPanelNative panelNative = WinRT.CastExtensions.As<ISwapChainPanelNative>(scp1);
                    hr = panelNative.SetSwapChain(m_pDXGISwapChain1);
                    scp1.SizeChanged += scp1_SizeChanged;
                }

                m_pCTR = new CustomTextRenderer((IDWriteFactory4)m_pDWriteFactory7, (ID2D1DeviceContext4)m_pD2DDeviceContext3, new ColorF(ColorF.Enum.Orange, 1.0f), hWndMain);

                CompositionTarget.Rendering += CompositionTarget_Rendering;
            }
            this.Closed += MainWindow_Closed;
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
                if (hr == HRESULT.S_OK)
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
                if (hr == HRESULT.S_OK)
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
                if (hr == HRESULT.S_OK)
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
                if (hr == HRESULT.S_OK)
                {
                    StringBuilder sbLocaleName = new StringBuilder(LOCALE_NAME_MAX_LENGTH);
                    hr = pLocalizedStrings.GetString(0, sbLocaleName, LOCALE_NAME_MAX_LENGTH);
                    string sStyleString = sbLocaleName.ToString();
                    sStyle = ConvertStringToFontStyleString(sStyleString);
                    SafeRelease(ref pLocalizedStrings);
                }
                hr = pFontSet.GetPropertyValues(i, DWRITE_FONT_PROPERTY_ID.DWRITE_FONT_PROPERTY_ID_STRETCH,
                out bPropertyExists, out pLocalizedStrings);
                if (hr == HRESULT.S_OK)
                {
                    StringBuilder sbLocaleName = new StringBuilder(LOCALE_NAME_MAX_LENGTH);
                    hr = pLocalizedStrings.GetString(0, sbLocaleName, LOCALE_NAME_MAX_LENGTH);
                    string sStretchString = sbLocaleName.ToString();
                    sStretch = ConvertStringToFontStretchString(sStretchString);
                    SafeRelease(ref pLocalizedStrings);
                }

                IDWriteFontFaceReference pFontFaceReference = null;
                hr = pFontSet.GetFontFaceReference(i, out pFontFaceReference);
                if (hr == HRESULT.S_OK)
                {
                    IDWriteFontFace3 pFontFace3;
                    hr = pFontFaceReference.CreateFontFace(out pFontFace3);
                    if (hr == HRESULT.S_OK)
                    {
                        uint nNbFiles = 0;
                        hr = pFontFace3.GetFiles(ref nNbFiles, null);
                        IDWriteFontFile[] fontFiles = new IDWriteFontFile[nNbFiles];
                        hr = pFontFace3.GetFiles(ref nNbFiles, fontFiles);
                        if (hr == HRESULT.S_OK)
                        {
                            IDWriteFontFileLoader pFontFileLoader = null;
                            hr = fontFiles[0].GetLoader(out pFontFileLoader);
                            if (hr == HRESULT.S_OK)
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
                                    if (hr == HRESULT.S_OK)
                                    {
                                        uint nFilePathLength = 0;
                                        hr = pLocalFontFileLoader.GetFilePathLengthFromKey(pFontFileReferenceKey, (uint)nFontFileReferenceKeySize, out nFilePathLength);
                                        if (hr == HRESULT.S_OK)
                                        {
                                            StringBuilder sbFilePath = new StringBuilder((int)nFilePathLength + 1);
                                            hr = pLocalFontFileLoader.GetFilePathFromKey(pFontFileReferenceKey, (uint)nFontFileReferenceKeySize, sbFilePath, nFilePathLength + 1);
                                            if (hr == HRESULT.S_OK)
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
            if (GetForegroundWindow() == hWndMain)
            {
                // To stop rendering when dragging
                //if (GetCapture() != hWndMain)
                    hr = Render();
            }
        }

        // For simplified test : Rename to Render and other Render to Render2...

        HRESULT Render2()
        {
            HRESULT hr = HRESULT.S_OK;
            if (m_pD2DDeviceContext != null)
            {
                m_pD2DDeviceContext.BeginDraw();

                m_pD2DDeviceContext.Clear(new ColorF(ColorF.Enum.Blue, 1.0f));                
                m_pD2DDeviceContext.SetTransform(Matrix3x2F.Identity());
                m_pD2DDeviceContext.GetSize(out D2D1_SIZE_F size);
                if (m_pD2DBitmap1 != null)
                {   
                    m_pD2DBitmap1.GetSize(out D2D1_SIZE_F sizeBmpBackground);
                    D2D1_RECT_F destRectBackground = new D2D1_RECT_F(0.0f, 0.0f, size.width, size.height);
                    //D2D1_RECT_F destRectBackground = new D2D1_RECT_F(0.0f, 0.0f, sizeBmpBackground.width, sizeBmpBackground.height);
                    D2D1_RECT_F sourceRectBackground = new D2D1_RECT_F(0.0f, 0.0f, sizeBmpBackground.width, sizeBmpBackground.height);
                    //m_pD2DDeviceContext.DrawBitmap(m_pD2DBitmap1, ref destRectBackground, 1.0f, D2D1_BITMAP_INTERPOLATION_MODE.D2D1_BITMAP_INTERPOLATION_MODE_LINEAR, ref sourceRectBackground);
                }
                m_pD2DDeviceContext.FillRectangle(RectF(10.0f, 10.0f, 200.0f, 200.0f), m_pD2DSolidColorBrushPink);
                m_pD2DDeviceContext.FillEllipse(Ellipse(new Direct2D.D2D1_POINT_2F(300, 300), 100.0f, 100.0f), m_pD2DSolidColorBrushRed);

                if (m_pTextLayout != null)
                {
                    if (m_pCTR != null)
                    {
                        hr = m_pTextLayout.Draw(IntPtr.Zero, m_pCTR, 0, 0);
                        if (hr == HRESULT.S_OK)
                        {
                            if (m_pCTR.IsDWriteCore)
                            {
                                hr = m_pCTR.DrawBitmapRenderTarget(Point2F(0, 0));
                            }
                        }
                    }

                    // DWrite only                    
                   
                //    m_pD2DDeviceContext.DrawTextLayout(Point2F(100, 100), m_pTextLayout, m_pD2DSolidColorBrushPink, 
                //        D2D1_DRAW_TEXT_OPTIONS.D2D1_DRAW_TEXT_OPTIONS_NO_SNAP | D2D1_DRAW_TEXT_OPTIONS.D2D1_DRAW_TEXT_OPTIONS_ENABLE_COLOR_FONT);
                
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

        float m_nY1 = 0.0f;

        float m_nShadowTranslate = 4.0f;
        float m_nShadowTranslateDirection = 1.0f;

        float m_nTurbulenceBaseFrequencyY = 0.03f;
        float m_nTurbulenceDirection = 1.0f;

        float m_nShadowStandardDeviation = 0.0f;
        float m_nShadowDirection = 1.0f;
        float m_nShiftGlowX = 30.0f;
        float m_nShiftGlowY = 20.0f;

        float m_nXBlood = 0.0f;
        float m_nDirectionXBlood = 1.0f;
        float m_nYBlood = 0.0f;
        float m_nDirectionYBlood = 1.0f;

        float m_nXScroll = 300.0f;

        float m_nDisplacementMapScale = 0.1f;
        float m_nDisplacementMapDirection = -1.0f;
        int m_nCptDisplacementPause = 0;

        HRESULT Render()
        {
            HRESULT hr = HRESULT.S_OK;
            if (m_pD2DDeviceContext != null)
            {
                m_pD2DDeviceContext.BeginDraw();

                m_pD2DDeviceContext.Clear(new ColorF(ColorF.Enum.DarkSlateBlue, 1.0f));
                m_pD2DDeviceContext.SetTransform(Matrix3x2F.Identity());
                m_pD2DDeviceContext.GetSize(out D2D1_SIZE_F size);

                if (m_pD2DGeometry1 != null)
                { 
                    m_pD2DDeviceContext.DrawGeometry(m_pD2DGeometry1, m_pD2DMainBrush, 2.0f);
                    var translateMatrix = Matrix3x2F.Translation(new D2D1_SIZE_F(0, m_nY1));
                    m_pD2DLinearGradientBrush1.SetTransform(translateMatrix);
                    m_nY1 += 0.1f;
                    if (m_nY1 >= m_nComputedHeight1 * 1.33f)
                    {
                        m_nY1 = 0.0f;                       
                    }
                    m_pD2DDeviceContext.FillGeometry(m_pD2DGeometry1, m_pD2DLinearGradientBrush1);
                    //D2D1_LAYER_PARAMETERS lp = new D2D1_LAYER_PARAMETERS();
                    //lp.geometricMask = m_pD2DGeometry1;
                    //lp = LayerParameters(InfiniteRect(), m_pD2DGeometry1);
                    //m_pD2DDeviceContext.PushLayer(ref lp);

                    //D2D1_RECT_F rectBounds;
                    //hr = m_pD2DGeometry1.GetBounds(null, out rectBounds);
                    //m_pD2DDeviceContext.FillRectangle(ref rectBounds, m_pD2DLinearGradientBrush1);

                    //m_pD2DDeviceContext.PopLayer();
                    //SafeRelease(ref lp.geometricMask);
                }
                if (m_pD2DGeometry2 != null)
                {
                    m_pD2DDeviceContext.SetTransform(Matrix3x2F.Translation(30.0f, m_nComputedHeight1 + 10.0f));

                    ID2D1BitmapRenderTarget pCompatibleRenderTarget = null;
                    Direct2D.D2D1_SIZE_U sizeU = SizeU((uint)size.width, (uint)size.height);
                    hr = m_pD2DDeviceContext.CreateCompatibleRenderTarget(ref size, ref sizeU, PixelFormat(DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM, D2D1_ALPHA_MODE.D2D1_ALPHA_MODE_PREMULTIPLIED),
                        D2D1_COMPATIBLE_RENDER_TARGET_OPTIONS.D2D1_COMPATIBLE_RENDER_TARGET_OPTIONS_NONE, out pCompatibleRenderTarget);
                    if (hr == HRESULT.S_OK)
                    {
                        pCompatibleRenderTarget.BeginDraw();
                        pCompatibleRenderTarget.Clear(null);

                        pCompatibleRenderTarget.FillGeometry(m_pD2DGeometry2, m_pD2DMainBrush);

                        hr = pCompatibleRenderTarget.EndDraw(out UInt64 tag11, out UInt64 tag21);
                        ID2D1Bitmap pCompatibleBitmap = null;
                        hr = pCompatibleRenderTarget.GetBitmap(out pCompatibleBitmap);

                        ID2D1Effect pShadowEffect = null;
                        hr = m_pD2DDeviceContext.CreateEffect(CLSID_D2D1Shadow, out pShadowEffect);
                        pShadowEffect.SetInput(0, pCompatibleBitmap);

                        //SetEffectFloat(pShadowEffect, (uint)D2D1_SHADOW_PROP.D2D1_SHADOW_PROP_BLUR_STANDARD_DEVIATION, 4.0f);

                        D2D1_RECT_F rectBackground = new D2D1_RECT_F(0.0f, 0.0f, size.width, size.height);
                        pCompatibleBitmap.GetSize(out D2D1_SIZE_F bmpSizeBackground);
                        Direct2D.D2D1_POINT_2F ptShadow = Point2F(m_nShadowTranslate, m_nShadowTranslate);
                        D2D1_RECT_F sourceRectangle = new D2D1_RECT_F(0, 0, size.width, size.height);

                        m_pD2DDeviceContext.DrawImage((ID2D1Image)pShadowEffect, ref ptShadow, ref sourceRectangle, D2D1_INTERPOLATION_MODE.D2D1_INTERPOLATION_MODE_LINEAR, D2D1_COMPOSITE_MODE.D2D1_COMPOSITE_MODE_SOURCE_OVER);
                        m_nShadowTranslate += 0.2f * m_nShadowTranslateDirection;
                        if (m_nShadowTranslate >= 16.0f && m_nShadowTranslateDirection > 0 ||
                            m_nShadowTranslate <= 4.0f && m_nShadowTranslateDirection < 0)
                        {
                            m_nShadowTranslateDirection = -m_nShadowTranslateDirection;
                        }

                        //ID2D1Image pOutputImage = null;
                        //pShadowEffect.GetOutput(out pOutputImage);
                        //m_pD2DDeviceContext.DrawImage(pOutputImage, ref ptShadow, ref sourceRectangle, D2D1_INTERPOLATION_MODE.D2D1_INTERPOLATION_MODE_LINEAR, D2D1_COMPOSITE_MODE.D2D1_COMPOSITE_MODE_SOURCE_OVER);
                        //SafeRelease(ref pOutputImage);

                        SafeRelease(ref pShadowEffect);
                        SafeRelease(ref pCompatibleBitmap);
                        SafeRelease(ref pCompatibleRenderTarget);
                    }
                    m_pD2DDeviceContext.FillGeometry(m_pD2DGeometry2, m_pD2DSolidColorBrushGreen);
                }
                if (m_pD2DGeometry3 != null)
                {
                    m_pD2DDeviceContext.SetTransform(Matrix3x2F.Translation(20.0f, m_nComputedHeight1 + m_nComputedHeight2 + 10.0f));

                    ID2D1BitmapRenderTarget pCompatibleRenderTarget = null;
                    Direct2D.D2D1_SIZE_U sizeU = SizeU((uint)size.width, (uint)size.height);
                    hr = m_pD2DDeviceContext.CreateCompatibleRenderTarget(ref size, ref sizeU, PixelFormat(DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM, D2D1_ALPHA_MODE.D2D1_ALPHA_MODE_PREMULTIPLIED),
                        D2D1_COMPATIBLE_RENDER_TARGET_OPTIONS.D2D1_COMPATIBLE_RENDER_TARGET_OPTIONS_NONE, out pCompatibleRenderTarget);
                    if (hr == HRESULT.S_OK)
                    {
                        pCompatibleRenderTarget.BeginDraw();
                        pCompatibleRenderTarget.Clear(null);

                        pCompatibleRenderTarget.FillGeometry(m_pD2DGeometry3, m_pD2DSolidColorBrushWhite);

                        hr = pCompatibleRenderTarget.EndDraw(out UInt64 tag11, out UInt64 tag21);
                        ID2D1Bitmap pCompatibleBitmap = null;
                        hr = pCompatibleRenderTarget.GetBitmap(out pCompatibleBitmap);

                        ID2D1Effect pDisplacementMapEffect = null;
                        hr = m_pD2DDeviceContext.CreateEffect(D2DTools.CLSID_D2D1DisplacementMap, out pDisplacementMapEffect);
                        pDisplacementMapEffect.SetInput(0, pCompatibleBitmap);
                        //pDisplacementMapEffect.SetInput(1, m_pD2DBitmapWave);

                        SetEffectFloat(pDisplacementMapEffect, (uint)D2D1_DISPLACEMENTMAP_PROP.D2D1_DISPLACEMENTMAP_PROP_SCALE, 10.0f);

                        ID2D1Effect pEffectTurbulence = null;
                        hr = m_pD2DDeviceContext.CreateEffect(D2DTools.CLSID_D2D1Turbulence, out pEffectTurbulence);
                        D2DTools.SetInputEffect(pDisplacementMapEffect, 1, pEffectTurbulence);

                        float[] aFloatArray = { 0.0f, m_nTurbulenceBaseFrequencyY };
                        SetEffectFloatArray(pEffectTurbulence, (uint)D2D1_TURBULENCE_PROP.D2D1_TURBULENCE_PROP_BASE_FREQUENCY, aFloatArray);

                        m_nTurbulenceBaseFrequencyY += 0.0005f * m_nTurbulenceDirection;
                        if (m_nTurbulenceBaseFrequencyY >= 0.08f && m_nTurbulenceDirection > 0 ||
                            m_nTurbulenceBaseFrequencyY <= 0.03f && m_nTurbulenceDirection < 0)
                        {
                            m_nTurbulenceDirection = -m_nTurbulenceDirection;
                        }

                        float[] aFloatArray2 = { size.width, size.height };
                        SetEffectFloatArray(pEffectTurbulence, (uint)D2D1_TURBULENCE_PROP.D2D1_TURBULENCE_PROP_SIZE, aFloatArray2);

                        //SetEffectInt(pDisplacementMapEffect, (uint)D2D1_DISPLACEMENTMAP_PROP.D2D1_DISPLACEMENTMAP_PROP_X_CHANNEL_SELECT, (uint)D2D1_CHANNEL_SELECTOR.D2D1_CHANNEL_SELECTOR_R);
                        //SetEffectInt(pDisplacementMapEffect, (uint)D2D1_DISPLACEMENTMAP_PROP.D2D1_DISPLACEMENTMAP_PROP_Y_CHANNEL_SELECT, (uint)D2D1_CHANNEL_SELECTOR.D2D1_CHANNEL_SELECTOR_R);

                        D2D1_RECT_F rectBackground = new D2D1_RECT_F(0.0f, 0.0f, size.width, size.height);
                        pCompatibleBitmap.GetSize(out D2D1_SIZE_F bmpSizeBackground);
                        Direct2D.D2D1_POINT_2F ptShadow = Point2F(10, 10);
                        D2D1_RECT_F sourceRectangle = new D2D1_RECT_F(0, 0, size.width, size.height);
                        m_pD2DDeviceContext.DrawImage((ID2D1Image)pDisplacementMapEffect, ptShadow, sourceRectangle, D2D1_INTERPOLATION_MODE.D2D1_INTERPOLATION_MODE_LINEAR, D2D1_COMPOSITE_MODE.D2D1_COMPOSITE_MODE_SOURCE_OVER);
                        SafeRelease(ref pDisplacementMapEffect);
                        SafeRelease(ref pCompatibleBitmap);
                        SafeRelease(ref pCompatibleRenderTarget);
                        SafeRelease(ref pEffectTurbulence);
                    }
                }
                if (m_pD2DGeometry4 != null)
                {
                    m_pD2DDeviceContext.SetTransform(Matrix3x2F.Translation(10.0f, m_nComputedHeight1 + m_nComputedHeight2 + m_nComputedHeight3 + 10.0f));

                    ID2D1BitmapRenderTarget pCompatibleRenderTarget = null;
                    Direct2D.D2D1_SIZE_U sizeU = SizeU((uint)size.width, (uint)size.height);
                    hr = m_pD2DDeviceContext.CreateCompatibleRenderTarget(ref size, ref sizeU, PixelFormat(DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM, D2D1_ALPHA_MODE.D2D1_ALPHA_MODE_PREMULTIPLIED),
                        D2D1_COMPATIBLE_RENDER_TARGET_OPTIONS.D2D1_COMPATIBLE_RENDER_TARGET_OPTIONS_NONE, out pCompatibleRenderTarget);
                    if (hr == HRESULT.S_OK)
                    {
                        pCompatibleRenderTarget.SetTransform(Matrix3x2F.Translation(m_nShiftGlowX, m_nShiftGlowY));

                        pCompatibleRenderTarget.BeginDraw();
                        pCompatibleRenderTarget.Clear(null);

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

                        m_nShadowStandardDeviation += 1.0f * m_nShadowDirection;
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

                        D2D1_RECT_F rectBackground = new D2D1_RECT_F(0.0f, 0.0f, size.width, size.height);
                        pCompatibleBitmap.GetSize(out D2D1_SIZE_F bmpSizeBackground);
                        Direct2D.D2D1_POINT_2F ptShadow = Point2F(0, 0);
                        D2D1_RECT_F sourceRectangle = new D2D1_RECT_F(0, 0, size.width, size.height);
                        m_pD2DDeviceContext.DrawImage((ID2D1Image)pBrightnessEffect, ptShadow, sourceRectangle, D2D1_INTERPOLATION_MODE.D2D1_INTERPOLATION_MODE_LINEAR, D2D1_COMPOSITE_MODE.D2D1_COMPOSITE_MODE_SOURCE_OVER);
                        SafeRelease(ref pBrightnessEffect);
                        SafeRelease(ref pShadowEffect);
                        SafeRelease(ref pCompatibleBitmap);
                        SafeRelease(ref pCompatibleRenderTarget);
                    }
                    m_pD2DDeviceContext.SetTransform(Matrix3x2F.Translation(10.0f + m_nShiftGlowX, m_nComputedHeight1 + m_nComputedHeight2 + m_nComputedHeight3 + 10.0f + m_nShiftGlowY));
                    m_pD2DDeviceContext.FillGeometry(m_pD2DGeometry4, m_pD2DSolidColorBrushWhite);
                }
                if (m_pD2DGeometry5 != null)
                {
                    m_pD2DDeviceContext.SetTransform(Matrix3x2F.Translation(30.0f, m_nComputedHeight1 + m_nComputedHeight2 + m_nComputedHeight3 + m_nComputedHeight4 + 10.0f + m_nShiftGlowY));

                    //m_pD2DDeviceContext.DrawGeometry(m_pD2DGeometry5, m_pD2DSolidColorBrushRed, 2.0f);
                    m_pD2DDeviceContext.DrawGeometry(m_pD2DGeometry5, m_pD2DMainBrush, 2.0f);
                    //m_pD2DDeviceContext.FillGeometry(m_pD2DGeometry5, m_pD2DBitmapBrush);

                    //ID2D1Layer pD2DLayer = null;
                    //var sz = new D2D1_SIZE_F(0.0f, 0.0f);
                    //hr = m_pD2DDeviceContext.CreateLayer(ref sz, out pD2DLayer);                         

                    //ID2D1RoundedRectangleGeometry pRRectangleGeometry;
                    //D2D1_ROUNDED_RECT rectangle1 = RoundedRect(RectF(100.0f, 100.0f, 100.0f + 300.0f, 300.0f), 30.0f, 30.0f);
                    //hr = m_pD2DFactory1.CreateRoundedRectangleGeometry(rectangle1, out pRRectangleGeometry);

                    D2D1_LAYER_PARAMETERS lp = new D2D1_LAYER_PARAMETERS();
                    lp.geometricMask = m_pD2DGeometry5;
                    lp = LayerParameters(InfiniteRect(), m_pD2DGeometry5);
                    //m_pD2DDeviceContext.PushLayer(ref lp, pD2DLayer);
                    m_pD2DDeviceContext.PushLayer(ref lp);

                    D2D1_RECT_F rectf2 = new D2D1_RECT_F();
                    rectf2.left = 0;
                    rectf2.top = 0;
                    rectf2.right = size.width;
                    rectf2.bottom = size.height;
                    //m_pD2DDeviceContext.FillRectangle(ref rectf2, m_pD2DSolidColorBrushRed);
                    m_pD2DDeviceContext.FillRectangle(ref rectf2, m_pD2DBitmapBrush1);

                    var translateMatrix = Matrix3x2F.Translation(new D2D1_SIZE_F(m_nXBlood, m_nYBlood));
                    m_pD2DBitmapBrush1.SetTransform(translateMatrix);
                    m_nXBlood += 0.1f * m_nDirectionXBlood;
                    m_nYBlood += 0.1f * m_nDirectionYBlood;

                    if (m_nXBlood >= 20.0f && m_nDirectionXBlood > 0 ||
                        m_nXBlood <= 0.0f && m_nDirectionXBlood < 0)
                    {
                        m_nDirectionXBlood = -m_nDirectionXBlood;
                    }
                    if (m_nYBlood >= 20.0f && m_nDirectionYBlood > 0 ||
                        m_nYBlood <= 0.0f && m_nDirectionYBlood < 0)
                    {
                        m_nDirectionYBlood = -m_nDirectionYBlood;
                    }

                    m_pD2DDeviceContext.PopLayer();
                    SafeRelease(ref lp.geometricMask);
                }
                if (m_pD2DGeometry6 != null)
                {
                    m_pD2DDeviceContext.SetTransform(Matrix3x2F.Translation(m_nXScroll, m_nComputedHeight1 + m_nComputedHeight2 + m_nComputedHeight3 + m_nComputedHeight4 + m_nComputedHeight5 + 10.0f + m_nShiftGlowY));
                    m_pD2DDeviceContext.DrawGeometry(m_pD2DGeometry6, m_pD2DMainBrush, 2.0f);
                    m_pD2DDeviceContext.FillGeometry(m_pD2DGeometry6, m_pD2DSolidColorBrushPink);

                    D2D1_RECT_F rectBounds;
                    hr = m_pD2DGeometry6.GetBounds(null, out rectBounds);
                    m_nXScroll -= 2.0f;
                    if (m_nXScroll <= -(rectBounds.right - rectBounds.left))
                        m_nXScroll = size.width;
                }
                if (m_pD2DGeometry7 != null)
                {
                    float nTextLayoutHeight = 0;
                    if (m_pTextLayout != null)
                    {
                        DWRITE_TEXT_METRICS textMetrics;
                        hr = m_pTextLayout.GetMetrics(out textMetrics);
                        nTextLayoutHeight = textMetrics.layoutHeight;
                    }
                    m_pD2DDeviceContext.SetTransform(Matrix3x2F.Translation(30.0f, m_nComputedHeight1 + m_nComputedHeight2 + m_nComputedHeight3
                        + m_nComputedHeight4 + m_nComputedHeight5 + m_nComputedHeight6 + nTextLayoutHeight /*+ nShiftGlowY*/));

                    ID2D1BitmapRenderTarget pCompatibleRenderTarget = null;
                    Direct2D.D2D1_SIZE_U sizeU = SizeU((uint)size.width, (uint)size.height);
                    hr = m_pD2DDeviceContext.CreateCompatibleRenderTarget(ref size, ref sizeU, PixelFormat(DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM, D2D1_ALPHA_MODE.D2D1_ALPHA_MODE_PREMULTIPLIED),
                        D2D1_COMPATIBLE_RENDER_TARGET_OPTIONS.D2D1_COMPATIBLE_RENDER_TARGET_OPTIONS_NONE, out pCompatibleRenderTarget);
                    if (hr == HRESULT.S_OK)
                    {
                        pCompatibleRenderTarget.BeginDraw();
                        pCompatibleRenderTarget.Clear(null);

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

                        //if (m_nDisplacementMapScale >= 20.0f && m_nDisplacementMapDirection > 0 ||
                        //    m_nDisplacementMapScale <= -20.0f && m_nDisplacementMapDirection < 0)
                        //{                            
                        //    //m_nDisplacementMapDirection = -m_nDisplacementMapDirection;
                        //}

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

                        D2D1_RECT_F rectBackground = new D2D1_RECT_F(0.0f, 0.0f, size.width, size.height);
                        pCompatibleBitmap.GetSize(out D2D1_SIZE_F bmpSizeBackground);
                        Direct2D.D2D1_POINT_2F ptShadow = Point2F(0, 0);
                        D2D1_RECT_F sourceRectangle = new D2D1_RECT_F(0, 1, size.width, size.height - 1);
                        m_pD2DDeviceContext.DrawImage((ID2D1Image)pDisplacementMapEffect, ptShadow, sourceRectangle, D2D1_INTERPOLATION_MODE.D2D1_INTERPOLATION_MODE_LINEAR, D2D1_COMPOSITE_MODE.D2D1_COMPOSITE_MODE_SOURCE_OVER);
                        SafeRelease(ref pDisplacementMapEffect);
                        SafeRelease(ref pCompatibleBitmap);
                        SafeRelease(ref pCompatibleRenderTarget);
                    }
                }

                if (m_pTextLayout != null)
                {
                    m_pD2DDeviceContext.SetTransform(Matrix3x2F.Translation(0.0f, m_nComputedHeight1 + m_nComputedHeight2 + m_nComputedHeight3
                     + m_nComputedHeight4 + m_nComputedHeight5 + m_nComputedHeight6 + m_nShiftGlowY + 10.0f));

                    if (m_pCTR != null)
                    {
                        hr = m_pTextLayout.Draw(IntPtr.Zero, m_pCTR, 0, 0);
                        if (hr == HRESULT.S_OK)
                        {
                            if (m_pCTR.IsDWriteCore)
                            {
                                //DWRITE_TEXT_METRICS textMetrics;
                                //hr = m_pTextLayout.GetMetrics(out textMetrics);
                                //float nX = (size.width - textMetrics.layoutWidth) / 2.0f;
                                //hr = m_pCTR.DrawBitmapRenderTarget(Point2F(nX, 0));

                                hr = m_pCTR.DrawBitmapRenderTarget(Point2F(0.0f, 0.0f));
                            }
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

        private HRESULT CreateDWriteTextGeometry(string sText, string sPathFont, float nHeight, bool bBold, bool bItalic, out ID2D1Geometry pD2DGeometry, out float nComputedHeight)
        {
            HRESULT hr = HRESULT.S_OK;
            pD2DGeometry = null;
            nComputedHeight = 0;
            IDWriteFontFile pDWriteFontFile = null;
            hr = m_pDWriteFactory7.CreateFontFileReference(sPathFont, IntPtr.Zero, out pDWriteFontFile);
            if (hr == HRESULT.S_OK)
            {
                bool bSupportedFontType = false;
                DWRITE_FONT_FILE_TYPE FontFileType;
                DWRITE_FONT_FACE_TYPE FontFaceType;
                int nNumberOfFaces = 0;
                hr = pDWriteFontFile.Analyze(out bSupportedFontType, out FontFileType, out FontFaceType, out nNumberOfFaces);
                if (hr == HRESULT.S_OK)
                {
                    DWRITE_FONT_SIMULATIONS fs = DWRITE_FONT_SIMULATIONS.DWRITE_FONT_SIMULATIONS_NONE;
                    if (bBold && bItalic)
                        fs = DWRITE_FONT_SIMULATIONS.DWRITE_FONT_SIMULATIONS_BOLD | DWRITE_FONT_SIMULATIONS.DWRITE_FONT_SIMULATIONS_OBLIQUE;
                    else if (bBold)
                        fs = DWRITE_FONT_SIMULATIONS.DWRITE_FONT_SIMULATIONS_BOLD;
                    else if (bItalic)
                        fs = DWRITE_FONT_SIMULATIONS.DWRITE_FONT_SIMULATIONS_OBLIQUE;
                    IDWriteFontFace pDWriteFontFace = null;
                    hr = m_pDWriteFactory7.CreateFontFace(FontFaceType, 1,
                        new IDWriteFontFile[] { pDWriteFontFile }, 0, fs, out pDWriteFontFace);
                    if (hr == HRESULT.S_OK)
                    {
                        uint[] codePoints = ToCodePoints(sText).ToArray();
                        ushort[] glyphIndices = new ushort[codePoints.Length];
                        hr = pDWriteFontFace.GetGlyphIndices(codePoints, sText.Length, glyphIndices);
                        if (hr == HRESULT.S_OK)
                        {
                            ID2D1PathGeometry pD2DPathGeometry = null;
                            hr = m_pD2DFactory1.CreatePathGeometry(out pD2DPathGeometry);
                            if (hr == HRESULT.S_OK)
                            {
                                uint nDPI = GetDpiForWindow(hWndMain);
                                ID2D1GeometrySink pD2DGeometrySink = null;
                                hr = pD2DPathGeometry.Open(out pD2DGeometrySink);
                                if (hr == HRESULT.S_OK)
                                {
                                    hr = pDWriteFontFace.GetGlyphRunOutline(nHeight * 96.0f / (float)nDPI, glyphIndices, null, null, sText.Length,
                                        false, false, (DWrite.ID2D1SimplifiedGeometrySink)pD2DGeometrySink);
                                    pD2DGeometrySink.Close();
                                    SafeRelease(ref pD2DGeometrySink);
                                }
                                DWRITE_FONT_METRICS fontFaceMetrics;
                                pDWriteFontFace.GetMetrics(out fontFaceMetrics);
                                float nTranslateY = (fontFaceMetrics.ascent + fontFaceMetrics.descent + fontFaceMetrics.lineGap) * (nHeight * (72.0f / (float)nDPI) / (float)fontFaceMetrics.designUnitsPerEm);
                                nComputedHeight = (fontFaceMetrics.ascent + fontFaceMetrics.descent + fontFaceMetrics.lineGap) * (nHeight * (96.0f / (float)nDPI) / (float)fontFaceMetrics.designUnitsPerEm);

                                float nBaselineOriginX = 0;
                                float nBaselineOriginY = nTranslateY;
                                var translateMatrix = Matrix3x2F.Translation(new D2D1_SIZE_F(nBaselineOriginX, nBaselineOriginY));
                                ID2D1TransformedGeometry pD2DTransformedGeometry = null;
                                hr = m_pD2DFactory1.CreateTransformedGeometry(pD2DPathGeometry, translateMatrix, out pD2DTransformedGeometry);
                                SafeRelease(ref pD2DPathGeometry);
                                pD2DGeometry = pD2DTransformedGeometry;
                            }
                        }
                        SafeRelease(ref pDWriteFontFace);
                    }
                }
                SafeRelease(ref pDWriteFontFile);
            }
            return hr;
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
            if (hr == HRESULT.S_OK)
            {
                m_pDXGIDevice = Marshal.GetObjectForIUnknown(m_pD3D11DevicePtr) as IDXGIDevice1;
                if (m_pD2DFactory1 != null)
                {
                    ID2D1Device pD2DDevice = null;
                    hr = m_pD2DFactory1.CreateDevice(m_pDXGIDevice, out pD2DDevice);
                    if (hr == HRESULT.S_OK)
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
            if (hr == HRESULT.S_OK)
            {
                IntPtr pDXGIFactory2Ptr;
                hr = pDXGIAdapter.GetParent(typeof(IDXGIFactory2).GUID, out pDXGIFactory2Ptr);
                if (hr == HRESULT.S_OK)
                {
                    IDXGIFactory2 pDXGIFactory2 = Marshal.GetObjectForIUnknown(pDXGIFactory2Ptr) as IDXGIFactory2;
                    if (hWnd != IntPtr.Zero)
                        hr = pDXGIFactory2.CreateSwapChainForHwnd(m_pD3D11DevicePtr, hWnd, ref swapChainDesc, IntPtr.Zero, null, out m_pDXGISwapChain1);
                    else
                        hr = pDXGIFactory2.CreateSwapChainForComposition(m_pD3D11DevicePtr, ref swapChainDesc, null, out m_pDXGISwapChain1);

                    if (hr == HRESULT.S_OK)
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
            if (hr == HRESULT.S_OK)
            {
                IDXGISurface pDXGISurface = Marshal.GetObjectForIUnknown(pDXGISurfacePtr) as IDXGISurface;
                hr = m_pD2DDeviceContext.CreateBitmapFromDxgiSurface(pDXGISurface, ref bitmapProperties, out m_pD2DTargetBitmap);
                if (hr == HRESULT.S_OK)
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
            if (hr == HRESULT.S_OK)
            {
                hr = pDecoder.GetFrame(0, out pSource);
                if (hr == HRESULT.S_OK)
                {
                    hr = pIWICFactory.CreateFormatConverter(out pConverter);
                    if (hr == HRESULT.S_OK)
                    {
                        if (destinationWidth != 0 || destinationHeight != 0)
                        {
                            uint originalWidth, originalHeight;
                            hr = pSource.GetSize(out originalWidth, out originalHeight);
                            if (hr == HRESULT.S_OK)
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
                                if (hr == HRESULT.S_OK)
                                {
                                    hr = pScaler.Initialize(pSource, destinationWidth, destinationHeight, WICBitmapInterpolationMode.WICBitmapInterpolationModeCubic);
                                    if (hr == HRESULT.S_OK)
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
                    CreateDWriteTextGeometry("This is a text with shadow", font.FullPath, 80.0f, bBold, bItalic, out m_pD2DGeometry2, out m_nComputedHeight2);
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
                    CreateDWriteTextGeometry("This is a text with shadow", font.FullPath, 80.0f, bBold, bItalic, out m_pD2DGeometry2, out m_nComputedHeight2);
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
                    if (hr == HRESULT.S_OK)
                    {
                        var lgbp = new D2D1_LINEAR_GRADIENT_BRUSH_PROPERTIES(new Direct2D.D2D1_POINT_2F(0.0f, m_nComputedHeight1 / 3.0f), new Direct2D.D2D1_POINT_2F(0.0f, m_nComputedHeight1));
                        hr = m_pD2DDeviceContext.CreateLinearGradientBrush(ref lgbp, IntPtr.Zero, pGSC, out m_pD2DLinearGradientBrush1);
                        SafeRelease(ref pGSC);
                    }
                }

                string sExePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                IWICBitmapSource pWICBitmapSource1 = null;
                string sAbsolutePath = "/Assets/Blood.jpg";
                if (sAbsolutePath.StartsWith("/"))
                    sAbsolutePath = sExePath + sAbsolutePath;
                hr = LoadBitmapFromFile(m_pD2DDeviceContext3, m_pWICImagingFactory, sAbsolutePath,
                    0, 0, out m_pD2DBitmap1, out pWICBitmapSource1);
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
}
