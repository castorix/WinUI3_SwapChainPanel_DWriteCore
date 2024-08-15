using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using System.Linq;

using GlobalStructures;
using DWrite;

// Similar Method as in Vanara : https://github.com/dahall/Vanara/blob/ed0ff42b7ac94c6c1ff3051c4f1905ed25033ae5/UnitTests/PInvoke/Graphics/DirectWriteTests.cs#L50

#nullable enable
public class FontCollectionLoader : IDWriteFontCollectionLoader
{
    public HRESULT CreateEnumeratorFromKey([In] IDWriteFactory pDWriteFactory, IntPtr pCollectionKey, uint collectionKeySize, out IDWriteFontFileEnumerator? pDWriteFontFileEnumerator)
    {
        pDWriteFontFileEnumerator = null;
        if (pDWriteFactory is null || pCollectionKey == default)
            return HRESULT.E_INVALIDARG;
        if (pCollectionKey != IntPtr.Zero)
        {
            string? sString = pCollectionKey != IntPtr.Zero?Marshal.PtrToStringUni(pCollectionKey):"";
            pDWriteFontFileEnumerator = new FontEnumerator(pDWriteFactory, sString!=null?sString:"");
        }
        return HRESULT.S_OK;
    }
}

public class FontEnumerator : IDWriteFontFileEnumerator
{
    private IEnumerator<string> m_pEnumerator;
    private IDWriteFactory m_pDWriteFactory;

    public FontEnumerator(IDWriteFactory pDWriteFactory, string sFontPath)
    {
        m_pDWriteFactory = pDWriteFactory;
        m_pEnumerator = Directory.EnumerateFiles(sFontPath, "*.ttf").Union(Directory.EnumerateFiles(sFontPath, "*.otf"))
            .OrderBy(filename => filename)
            .GetEnumerator();
    }

    public HRESULT MoveNext(out bool hasCurrentFile)
    { 
        hasCurrentFile = m_pEnumerator.MoveNext();
        if (!hasCurrentFile) return HRESULT.S_FALSE;
        return HRESULT.S_OK;
    }

    public HRESULT GetCurrentFontFile(out IDWriteFontFile? pDWriteFontFile)
    {
        HRESULT hr = HRESULT.S_OK;

        hr = m_pDWriteFactory.CreateFontFileReference(m_pEnumerator.Current, IntPtr.Zero, out pDWriteFontFile);
        if (hr != HRESULT.S_OK)
        {
            // When font name is too long : ERROR_INVALID_NAME
            //Console.Beep(1000, 10);
            pDWriteFontFile = null;
            return hr;
        }
        return default;
    }
#nullable disable
}
