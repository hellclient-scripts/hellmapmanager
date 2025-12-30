using System;
using System.CommandLine;
using HellMapManager.Cores;
using HellMapManager.Models;

namespace HellMapManager.Helpers;

public class CommandLineHelper
{
    public string? OpenFile;

    public string? SettingsPath;
    public int? Port;
    public string? Username;

    public string? Password;
    public bool? AutoStart;
    public bool? Headless;

    public string? Host;

    public static CommandLineHelper Current { get; } = new CommandLineHelper();

    public void Prase(string[] args)
    {
        RootCommand rootCommand = new("HellMapManager Command Line Options");
        var openFileArgument = new Argument<string?>("openfile") { DefaultValueFactory = (ArgumentResult) => null };
        rootCommand.Arguments.Add(openFileArgument);
        var settingsOption = new Option<string?>("-s", "--settings") { Description = "Path to settings file", };
        var portOption = new Option<int?>("-p", "--port") { Description = "Port number for the server" };
        var usernameOption = new Option<string?>("-u", "--username") { Description = "Username for authentication" };
        var passwordOption = new Option<string?>("-w", "--password") { Description = "Password for authentication" };
        var autoStartOption = new Option<bool?>("-a", "--autostart") { Description = "Auto start the server" };
        var headlessOption = new Option<bool?>("-c", "--cli") { Description = "Run in CLI(headless) mode" };
        var hostOption = new Option<string?>("-o", "--host"){ Description = "Host address for the server" };
        rootCommand.Options.Add(settingsOption);
        rootCommand.Options.Add(portOption);
        rootCommand.Options.Add(usernameOption);
        rootCommand.Options.Add(passwordOption);
        rootCommand.Options.Add(autoStartOption);
        rootCommand.Options.Add(headlessOption);
        rootCommand.Options.Add(hostOption);
        ParseResult parseResult = rootCommand.Parse(args);

        if (parseResult.Errors.Count == 0)
        {
            OpenFile = parseResult.GetValue(openFileArgument);
            SettingsPath = parseResult.GetValue(settingsOption);
            Port = parseResult.GetValue(portOption);
            Username = parseResult.GetValue(usernameOption);
            Password = parseResult.GetValue(passwordOption);
            AutoStart = parseResult.GetValue(autoStartOption);
            Headless = parseResult.GetValue(headlessOption);
            Host = parseResult.GetValue(hostOption);
        }
        else
        {
            foreach (var error in parseResult.Errors)
            {
                Console.WriteLine(error.Message);
            }
        }
    }
    public void ApplyTo(Settings settings)
    {
        if (Port is not null)
        {
            settings.APIPort = Port.Value;
        }
        if (Username is not null)
        {
            settings.APIUserName = Username;
        }
        if (Password is not null)
        {
            settings.APIPassWord = Password;
        }
        if (AutoStart is not null)
        {
            settings.APIEnabled = AutoStart.Value;
        }
        if (!string.IsNullOrEmpty(Host))
        {
            settings.Host = Host;
        }
    }
    public string GetSettingsPath()
    {
        if (!string.IsNullOrEmpty(SettingsPath))
        {
            return SettingsPath;
        }
        string process;
        string path;
        if (System.Environment.GetEnvironmentVariable(AppPreset.ProcessPathEnvName) is string envPath && !string.IsNullOrEmpty(envPath))
        {
            process = envPath;
        }
        else
        {
            process = System.Environment.ProcessPath ?? "";
        }
        path = System.IO.Path.GetDirectoryName(process ?? "") ?? "";

        var settingspath = System.IO.Path.Join([path, AppPreset.SettingsFileName]);
        return settingspath;
    }
}