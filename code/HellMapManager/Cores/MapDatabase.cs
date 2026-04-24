using HellMapManager.Models;
using HellMapManager.Helpers;
using System.Threading;
using HellMapManager.Helpers.HMMEncoder;
using System;

namespace HellMapManager.Cores;


public partial class MapDatabase()
{
    public ReaderWriterLockSlim _lock = new();
    public const int Version = 1006;
    public MapFile? Current;
    public Settings Settings = new();

    public void OpenRecent(string file)
    {
        if (file != "")
        {
            LoadFile(file);
        }
    }

    public void Save()
    {
        if (Current != null)
        {
            SaveFile(Current.Path);
        }
    }
    public void Revert()
    {
        if (Current != null && Current.Path != "")
        {
            LoadFile(Current.Path);
        }
    }
    public void AddRecent(RecentFile recent)
    {
        if (Settings.Recents.Count > 0 && Settings.Recents[0].Path == recent.Path && Settings.Recents[0].Name == recent.Name)
        {
            return;
        }
        Settings.Recents.RemoveAll(r => r.Path == recent.Path);
        Settings.Recents.Insert(0, recent);
        if (Settings.Recents.Count > AppPreset.MaxRecents)
        {
            Settings.Recents = Settings.Recents.GetRange(0, AppPreset.MaxRecents);
        }
        RaiseSettingsUpdatedEvent(this);
    }
    public void LoadFile(string file)
    {
        var mf = HMMFile.Open(file);
        if (mf != null)
        {
            _lock.EnterWriteLock();
            try
            {
                Current = mf;
                Current.Modified = false;
                Current.Path = file;
                AddRecent(Current.ToRecentFile());
                RaiseMapFileUpdatedEvent(this);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
    public bool Import(byte[] data)
    {
        var mf = HMMEncoder.Decode(data);
        if (mf != null)
        {
            _lock.EnterWriteLock();
            try
            {
                if (Current != null)
                {
                    return false;
                }
                Current = mf;
                Current.Modified = true;
                Current.Path = "";
                RaiseMapFileUpdatedEvent(this);
                return true;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
        return false;
    }
    public string Export(MapEncoding encoding)
    {
        _lock.EnterWriteLock();
        try
        {
            if (Current == null)
            {
                return "";
            }
            var rawencoding = Current.Map.Encoding;
            Current.Map.Encoding = encoding;
            var data = HMMEncoder.GetEncoding(Current.Map.Encoding).GetString(HMMEncoder.Encode(Current));
            Current.Map.Encoding = rawencoding;
            return data;
        }
        catch (Exception)
        {
            return "";
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }
    public bool Create()
    {
        _lock.EnterWriteLock();
        try
        {
            if (Current != null)
            {
                return false;
            }
            var mapfile = MapFile.Create("", "");
            Current = mapfile;
            return true;
        }
        catch (Exception)
        {
            return false;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public void SaveFile(string file)
    {
        if (Current != null)
        {
            _lock.EnterWriteLock();
            try
            {

                Current.Map.Arrange();
                HMMFile.Save(file, Current);
                Current.Modified = false;
                Current.Path = file;
                AddRecent(Current.ToRecentFile());
                RaiseMapFileUpdatedEvent(this);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
    public Diffs? DiffFile(string file)
    {
        if (Current != null)
        {
            _lock.EnterReadLock();
            try
            {

                var mf = HMMFile.Open(file);
                if (mf != null)
                {
                    var diffs = DiffHelper.Diff(Current.Map, mf.Map);
                    return diffs;
                }
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
        return null;
    }
    public void Exit()
    {
        RaiseExitEvent(this);
    }

    public void NewMap()
    {
        _lock.EnterWriteLock();
        try
        {
            var mapfile = MapFile.Create("", "");
            Current = mapfile;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
        RaiseMapFileUpdatedEvent(this);

    }
    public void SetCurrent(MapFile mapfile)
    {
        _lock.EnterWriteLock();
        try
        {
            Current = mapfile;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
        RaiseMapFileUpdatedEvent(this);
    }
    public bool CloseCurrent()
    {
        _lock.EnterWriteLock();
        try
        {
            if (Current == null)
            {
                return false;
            }
            Current = null;
            return true;
        }
        finally
        {
            _lock.ExitWriteLock();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void UpdateMapSettings(MapSettings s)
    {
        if (Current != null)
        {
            _lock.EnterWriteLock();
            try
            {
                Current.Map.Encoding = s.Encoding;
                Current.Map.Info.Name = s.Name;
                Current.Map.Info.Desc = s.Desc;
                Current.MarkAsModified();
                RaiseMapFileUpdatedEvent(this);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}