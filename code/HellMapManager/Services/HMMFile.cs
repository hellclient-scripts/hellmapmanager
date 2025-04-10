
using System.IO;
using HellMapManager.Models;
using System.Text;
using HellMapManager.Services.HMMFormat;
namespace HellMapManager.Services;

public class HMMFile
{
    public static void Save(string name, MapFile mf)
    {
        using (var fileStream = new FileStream(name, FileMode.Create))
        {
            var hm = new HMMMap();
            hm.FromMap(mf.Map);
            var data = Encoding.UTF8.GetBytes(hm.ToXML());
            fileStream.Write(data);
        }
    }
    public static Map? Open(string name)
    {
        using (var fileStream = new FileStream(name, FileMode.Open))
        {
            var body = new byte[fileStream.Length];
            fileStream.ReadAsync(body, 0, (int)fileStream.Length);
            var hm = new HMMMap();
            hm.FromXML(Encoding.UTF8.GetString(body));
            return hm.ToMap();
        }

    }
}