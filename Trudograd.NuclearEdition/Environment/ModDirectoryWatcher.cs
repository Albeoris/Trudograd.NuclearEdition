using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using Object = System.Object;

namespace Trudograd.NuclearEdition
{
    public sealed class ModDirectoryWatcher : MonoBehaviour
    {
        public static ModDirectoryWatcher Instance => The<ModDirectoryWatcher>.Instance;

        private FileSystemWatcher _watcher;
        private readonly HashSet<String> _changed = new HashSet<String>();
        private readonly HashSet<String> _deleted = new HashSet<String>();
        private readonly Object _lock = new Object();

        public event FileChangedDelegate FileChanged;
        public event FileDeletedDelegate FileDeleted;

        public delegate void FileChangedDelegate(String fileName, Stream stream);

        public delegate void FileDeletedDelegate(String fileName);

        private void Awake()
        {
            _watcher = new FileSystemWatcher(ModEnvironment.DirectoryAbsolutePath, "*") {IncludeSubdirectories = true};
            _watcher.Created += WatcherOnCreated;
            _watcher.Renamed += WatcherOnRenamed;
            _watcher.Deleted += WatcherOnDeleted;
            _watcher.Changed += WatcherOnChanged;
            _watcher.Error += WatcherOnError;
            _watcher.Disposed += WatcherOnDisposed;
            _watcher.EnableRaisingEvents = true;
        }

        private void Update()
        {
            if (_changed.Count == 0 && _deleted.Count == 0)
                return;

            if (!Monitor.TryEnter(_lock))
                return;

            try
            {
                ProcessDeletedRecords();
                ProcessChangedRecords();
            }
            finally
            {
                Monitor.Exit(_lock);
            }
        }

        private void ProcessDeletedRecords()
        {
            if (_deleted.Count < 1)
                return;

            var h = FileDeleted;
            if (h == null)
            {
                _deleted.Clear();
            }
            else
            {
                var deleted = _deleted.ToArray();
                foreach (String fullPath in deleted)
                    RaiseFileDeleted(fullPath, h);
            }
        }

        private void ProcessChangedRecords()
        {
            if (_changed.Count < 1)
                return;

            var h = FileChanged;
            if (h == null)
            {
                _changed.Clear();
            }
            else
            {
                var changed = _changed.ToArray();
                foreach (String fullPath in changed)
                    RaiseFileChanged(fullPath, h);
            }
        }

        private void RaiseFileDeleted(String fullPath, FileDeletedDelegate action)
        {
            _deleted.Remove(fullPath);

            fullPath = PrepareFullPath(fullPath);
            foreach (FileDeletedDelegate callback in action.Enumerate())
            {
                try
                {
                    callback(fullPath);
                }
                catch (Exception ex)
                {
                    HandleError(ex);
                }
            }
        }

        private void RaiseFileChanged(String fullPath, FileChangedDelegate action)
        {
            FileStream input;
            try
            {
                input = File.OpenRead(fullPath);
            }
            catch (Exception ex)
            {
                HandleError(ex);

                if (!File.Exists(fullPath))
                    _changed.Remove(fullPath);
                return;
            }

            _changed.Remove(fullPath);

            fullPath = PrepareFullPath(fullPath);
            using (input)
            {
                NotDisposableStream stream = new NotDisposableStream(input);
                foreach (FileChangedDelegate callback in action.Enumerate())
                {
                    try
                    {
                        input.Position = 0;
                        callback(fullPath, stream);
                    }
                    catch (Exception ex)
                    {
                        HandleError(ex);
                    }
                }
            }
        }
        
        private static String PrepareFullPath(String fullPath)
        {
            return fullPath.Replace('\\', '/');
        }

        [BackgroundThread]
        private void WatcherOnCreated(Object sender, FileSystemEventArgs e)
        {
            lock (_lock)
            {
                _changed.Add(e.FullPath);
                _deleted.Remove(e.FullPath);
            }
        }

        [BackgroundThread]
        private void WatcherOnRenamed(Object sender, RenamedEventArgs e)
        {
            lock (_lock)
            {
                _changed.Add(e.FullPath);
                _deleted.Remove(e.FullPath);

                _changed.Remove(e.OldFullPath);
                _deleted.Add(e.OldFullPath);
            }
        }

        [BackgroundThread]
        private void WatcherOnDeleted(Object sender, FileSystemEventArgs e)
        {
            lock (_lock)
            {
                _changed.Remove(e.FullPath);
                _deleted.Add(e.FullPath);
            }
        }

        [BackgroundThread]
        private void WatcherOnChanged(Object sender, FileSystemEventArgs e)
        {
            lock (_lock)
            {
                _changed.Add(e.FullPath);
                _deleted.Remove(e.FullPath);
            }
        }

        [BackgroundThread]
        private void WatcherOnError(Object sender, ErrorEventArgs e)
        {
            HandleError(e.GetException());
        }

        [BackgroundThread]
        private void WatcherOnDisposed(Object sender, EventArgs e)
        {
            Debug.Log("[StreamingAssetsWatcher] Disposed");
        }

        private void OnDestroy()
        {
            _watcher.Dispose();
        }

        private static void HandleError(Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}