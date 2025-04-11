
using System.IO;
using HellMapManager.Models;
using System.Text;
using HellMapManager.Services.HMMXml;
namespace HellMapManager.Services;

public class HMMFile
{
    public static void Save(string name, MapFile mf)
    {
        using (var fileStream = new FileStream(name, FileMode.Create))
        {
            var hm = new HMMMap();
            hm.FromModel(mf.Map);
            var data = hm.ToXML();
            fileStream.Write(data);
        }
    }
    public static Map? Open(string name)
    {
        var body = File.ReadAllBytes(name);
        var hm = HMMMap.FromXML(body);
        if (hm == null)
        {
            return null;
        }
        return hm.ToModel();

    }
}