using System.Collections;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

public class ZipUtility {

    public static Dictionary<string, MemoryStream> ExtractZipFile(byte[] data)
    {

        ZipFile zf = null;
        Dictionary<string, MemoryStream> ret = new Dictionary<string, MemoryStream>();
        try
        {
            //use MemoryStream!!!!
            using (var mstrm = new MemoryStream(data))
            {
                zf = new ZipFile(mstrm);

                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile)
                    {
                        continue;
                    }

                    string entryFileName = zipEntry.Name;
                    byte[] buffer = new byte[4096];     // 4K is optimum
                    Stream zipStream = zf.GetInputStream(zipEntry);
                    MemoryStream newMs = new MemoryStream();
                    StreamUtils.Copy(zipStream, newMs, buffer);
                    ret.Add(entryFileName, newMs);

                    //string fullZipToPath = Path.Combine(outFolder, entryFileName);
                    //using (FileStream streamWriter = File.Create(fullZipToPath))
                    //{
                    //    StreamUtils.Copy(zipStream, streamWriter, buffer);
                    //}
                }
            }
            return ret;
        }
        finally
        {
            if (zf != null)
            {
                zf.IsStreamOwner = true;
                zf.Close();
            }
        }
        //statusLabel.text = "DONE!";
    }

    public static void ExtractZipFile(byte[] data, string outFolder)
    {

        ZipFile zf = null;
        try
        {
            //use MemoryStream!!!!
            using (var mstrm = new MemoryStream(data))
            {
                zf = new ZipFile(mstrm);

                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile)
                    {
                        continue;
                    }

                    string entryFileName = zipEntry.Name;
                    byte[] buffer = new byte[4096];     // 4K is optimum
                    Stream zipStream = zf.GetInputStream(zipEntry);
                    
                    string fullZipToPath = Path.Combine(outFolder, entryFileName);
                    using (FileStream streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            //statusLabel.text = "Error: " + ex.Message;
            // handle error
        }
        finally
        {
            if (zf != null)
            {
                zf.IsStreamOwner = true;
                zf.Close();
            }
        }
        //statusLabel.text = "DONE!";
    }

}
