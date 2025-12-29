using System;
using System.Threading.Tasks;
using HellMapManager.Cores;
using Microsoft.AspNetCore.Http;

namespace HellMapManager.Services.API;

public partial class APIServer
{
    public async Task HeadlessQuit(HttpContext ctx)
    {
        await Success(ctx);
        Console.WriteLine("Quiting.");
        await Stop();
    }
    public async Task HeadlessSave(HttpContext ctx)
    {
        if (AppKernel.MapDatabase.Current is not null)
        {
            Console.WriteLine($"Saving {AppKernel.MapDatabase.Current.Path}.");
            try
            {
                AppKernel.MapDatabase.SaveFile(AppKernel.MapDatabase.Current.Path);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error on saving:{e.Message}");
            }
        }
        await Success(ctx);
    }
    public async Task HeadlessLoad(HttpContext ctx)
    {
        if (AppKernel.MapDatabase.Current is not null)
        {
            Console.WriteLine($"Loading {AppKernel.MapDatabase.Current.Path}.");
            try
            {
                AppKernel.MapDatabase.LoadFile(AppKernel.MapDatabase.Current.Path);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error on loading:{e.Message}");
            }
        }
        await Success(ctx);
    }
}