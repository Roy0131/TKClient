using System;
public static class NetByteHelper
{
    //public static byte[] DecompressDataByZip(byte[] value)
    //{
    //    MemoryStream ms = new MemoryStream(value);
    //    ZInputStream zIn = new ZInputStream(ms);

    //    List<byte> lst = new List<byte>();
    //    byte[] bs = new byte[4096];
    //    int z = 0;
    //    List<byte> curLst = new List<byte>();
    //    while ((z = zIn.read(bs, 0, 4096)) > 0)
    //    {
    //        curLst.AddRange(bs);
    //        lst.AddRange(curLst.GetRange(0, z));
    //        curLst.Clear();
    //    }
    //    return lst.ToArray();
    //}

    public static byte[] DecompressDataBySnappy(byte[] value)
    {
        SnappyDecompressor sd = new SnappyDecompressor();
        return sd.Decompress(value, 0, value.Length);
    }

    //public static byte[] CompressDataByZip(byte[] value)
    //{
    //    MemoryStream ms = new MemoryStream();

    //    ZOutputStream zOut = new ZOutputStream(ms, zlibConst.Z_DEFAULT_COMPRESSION);
    //    zOut.Write(value, 0, value.Length);
    //    zOut.finish();
    //    zOut.Close();
    //    return ms.ToArray();
    //}

    public static byte[] CompressDataBySnappy(byte[] data)
    {
        var target = new SnappyCompressor();
        var compressed = new byte[target.MaxCompressedLength(data.Length)];
        int compressedSize = target.Compress(data, 0, data.Length, compressed);
        var result = new byte[compressedSize];
        Array.Copy(compressed, result, compressedSize);
        return result;
    }

}