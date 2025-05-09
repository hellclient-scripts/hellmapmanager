
using System.IO;
using System.IO.Compression;

using HellMapManager.Models;
using HellMapManager.Cores;
namespace HellMapManager.Helpers;

public class HMMFile
{
    public static void Save(string name, MapFile mf)
    {
        var result = HMMEncoder.HMMEncoder.Encode(mf);
        if (name.EndsWith(".hmz"))
        {
            using FileStream zipToOpen = new(name, FileMode.OpenOrCreate);
            using ZipArchive archive = new(zipToOpen, ZipArchiveMode.Update);
            ZipArchiveEntry hmmEntry = archive.CreateEntry(AppPreset.ZipEnityName);
            using var memoryStream = new MemoryStream(result);
            memoryStream.CopyTo(hmmEntry.Open());
        }
        else
        {
            using var fileStream = new FileStream(name, FileMode.Create);
            fileStream.Write(result);
        }
    }
    public static MapFile? Open(string name)
    {
        byte[] body;
        if (name.EndsWith(".hmz"))
        {
            using FileStream zipToOpen = new(name, FileMode.Open);
            using ZipArchive archive = new(zipToOpen, ZipArchiveMode.Read);
            ZipArchiveEntry? hmmEntry = archive.GetEntry(AppPreset.ZipEnityName);
            if (hmmEntry is null)
            {
                return null;
            }
            using var memoryStream = new MemoryStream();
            hmmEntry.Open().CopyTo(memoryStream);
            body = memoryStream.ToArray();
        }
        else
        {
            body = File.ReadAllBytes(name);
        }

        return HMMEncoder.HMMEncoder.Decode(body);
    }
}