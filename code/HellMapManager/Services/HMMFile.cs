
using System.IO;
using HellMapManager.Models;
namespace HellMapManager.Services;

public class HMMFile
{
    public static void Save(string name, MapFile mf)
    {
        using (var fileStream = new FileStream(name, FileMode.Create))
        {
            var result = HMMEncoder.HMMEncoder.Encode(mf);

            fileStream.Write(result);
        }
    }
    public static Map? Open(string name)
    {
        var body = File.ReadAllBytes(name);
        return HMMEncoder.HMMEncoder.Decode(body)!.Map;
    }
}