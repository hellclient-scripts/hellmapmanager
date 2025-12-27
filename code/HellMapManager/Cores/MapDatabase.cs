using HellMapManager.Models;
using HellMapManager.Helpers;
using System.Threading;

namespace HellMapManager.Cores;


public partial class MapDatabase()
{
    private ReaderWriterLockSlim _lock = new();
    public const int Version = 1003;
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
    public void CloseCurrent()
    {
        _lock.EnterWriteLock();
        try
        {
            Current = null;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
        RaiseMapFileUpdatedEvent(this);
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