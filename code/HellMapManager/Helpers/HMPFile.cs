using System.IO;
using System.IO.Compression;

using HellMapManager.Models;
using HellMapManager.Cores;
namespace HellMapManager.Helpers;

using HellMapManager.Adapters;

public class HMPFile
{
    public static void Save(string name, Diffs df)
    {
        var result = HMPEncoder.HMPEncoder.Encode(df);
        using var zipToOpen = SystemAdapter.File.WriteStream(name);
        using ZipArchive archive = new(zipToOpen, ZipArchiveMode.Update);
        ZipArchiveEntry hmmEntry = archive.CreateEntry(AppPreset.ZipEnityName);
        using var memoryStream = new MemoryStream(result);
        memoryStream.CopyTo(hmmEntry.Open());
    }
    public static Diffs? Open(string name)
    {
        byte[] body;
        using var zipToOpen = SystemAdapter.File.ReadStream(name);
        using ZipArchive archive = new(zipToOpen, ZipArchiveMode.Read);
        ZipArchiveEntry? hmmEntry = archive.GetEntry(AppPreset.ZipEnityName);
        if (hmmEntry is null)
        {
            return null;
        }
        using var memoryStream = new MemoryStream();
        hmmEntry.Open().CopyTo(memoryStream);
        body = memoryStream.ToArray();
        return HMPEncoder.HMPEncoder.Decode(body);
    }
}