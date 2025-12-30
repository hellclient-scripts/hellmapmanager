using System.Net.WebSockets;
using HellMapManager.Cores;
using HellMapManager.Helpers;
using Metsys.Bson;

namespace TestProject;

public class CommandLineHelperTest
{
    [Fact]
    public void TestCommandLineHelper()
    {
        var helper = CommandLineHelper.Current;
        Assert.Null(helper.Port);
        Assert.Null(helper.OpenFile);
        Assert.Null(helper.SettingsPath);
        Assert.Null(helper.AutoStart);
        Assert.Null(helper.Username);
        Assert.Null(helper.Password);
        Assert.Null(helper.Host);
        Assert.Null(helper.Headless);
        helper.Prase(["file", "-p", "abcd"]);
        Assert.Null(helper.OpenFile);
        Assert.Null(helper.Port);
        Assert.Null(helper.SettingsPath);
        Assert.Null(helper.AutoStart);
        Assert.Null(helper.Username);
        Assert.Null(helper.Password);
        Assert.Null(helper.Host);
        Assert.Null(helper.Headless);
        helper = new CommandLineHelper();
        helper.Prase(["file", "-p", "1234", "-s", "path/to/settings", "-a", "-c", "-u", "user", "-w", "pass", "-o", "0.0.0.0"]);
        Assert.Equal("file", helper.OpenFile);
        Assert.Equal(1234, helper.Port);
        Assert.Equal("path/to/settings", helper.SettingsPath);
        Assert.Equal(true, helper.AutoStart);
        Assert.Equal("user", helper.Username);
        Assert.Equal("pass", helper.Password);
        Assert.Equal(true, helper.Headless);
        Assert.Equal("0.0.0.0", helper.Host);
        var settings = new HellMapManager.Models.Settings();
        helper.ApplyTo(settings);
        Assert.Equal(1234, settings.APIPort);
        Assert.Equal("user", settings.APIUserName);
        Assert.Equal("pass", settings.APIPassWord);
        Assert.True(settings.APIEnabled);
        Assert.Equal("0.0.0.0", settings.Host);
        Assert.True(settings.APIEnabled);

        Assert.Equal("path/to/settings", helper.GetSettingsPath());

        var oldenv = System.Environment.GetEnvironmentVariable(AppPreset.ProcessPathEnvName);
        try
        {
            System.Environment.SetEnvironmentVariable(AppPreset.ProcessPathEnvName, null);
            helper = new CommandLineHelper();
            var process = System.Environment.ProcessPath ?? "";
            Assert.Equal(System.IO.Path.Join([System.IO.Path.GetDirectoryName(process ?? "") ?? "", AppPreset.SettingsFileName]), helper.GetSettingsPath());
            process = "custom/path/";
            System.Environment.SetEnvironmentVariable(AppPreset.ProcessPathEnvName, process);
            Assert.Equal(System.IO.Path.Join([System.IO.Path.GetDirectoryName(process ?? "") ?? "", AppPreset.SettingsFileName]), helper.GetSettingsPath());
        }
        finally
        {
            System.Environment.SetEnvironmentVariable(AppPreset.ProcessPathEnvName, oldenv);
        }
    }
}
