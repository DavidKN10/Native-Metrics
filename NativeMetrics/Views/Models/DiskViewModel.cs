using Microsoft.Web.WebView2.Core;
using NativeMetrics.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NativeMetrics.Views.Models;

public class DiskViewModel : INotifyPropertyChanged
{
    private string? _driveLetter;
    private string? _driveType;
    private string? _volumeName;
    private string? _fileSystemName;
    private string? _busType;
    private string? _vendor;
    private string? _model;
    private string? _guidPath;
    private string? _guid;
    public ulong _totalSpaceBytes;
    public ulong _availableSpaceBytes;
    public ulong _freeSpaceBytes;
    public ulong _bytesRead;
    public ulong _bytesWritten;
    public double _readSpeed;
    public double _writeSpeed;

    public string? DriveLetter
    {
        get { return _driveLetter; }
        set { _driveLetter = value; OnPropertyChanged(); }
    }

    public string? DriveType
    {
        get { return  _driveType; }
        set { _driveType = value; OnPropertyChanged(); }
    }

    public string? VolumeName
    {
        get { return _volumeName; }
        set { _volumeName = value; OnPropertyChanged(); }
    }

    public string? FileSystemName
    {
        get { return _fileSystemName; }
        set { _fileSystemName = value; OnPropertyChanged(); }
    }

    public string? BusType
    {
        get { return _busType; }
        set { _busType = value; OnPropertyChanged(); }
    }

    public string? Vendor
    {
        get { return _vendor; }
        set { _vendor = value; OnPropertyChanged(); }
    }

    public string? Model
    {
        get { return _model; }
        set { _model = value; OnPropertyChanged(); }
    }

    public string? GuidPath
    {
        get { return _guidPath; }
        set { _guidPath = value; OnPropertyChanged(); }
    }

    public string? Guid
    {
        get { return _guid; }
        set { _guid = value; OnPropertyChanged(); }
    }

    public ulong TotalSpaceBytes
    {
        get { return _totalSpaceBytes; }
        set { _totalSpaceBytes = value; OnPropertyChanged(); }
    }

    public ulong AvailableSpaceBytes
    {
        get { return _availableSpaceBytes; }
        set { _availableSpaceBytes = value; OnPropertyChanged(); }
    }

    public ulong FreeSpaceBytes
    {
        get { return _freeSpaceBytes; }
        set { _freeSpaceBytes = value; OnPropertyChanged(); }
    }

    public ulong BytesRead
    {
        get { return _bytesRead; }
        set { _bytesRead = value; OnPropertyChanged(); }
    }

    public ulong BytesWritten
    {
        get { return _bytesWritten; }
        set { _bytesWritten = value; OnPropertyChanged(); }
    }

    public double ReadSpeed
    {
        get { return _readSpeed; }
        set { _readSpeed = value; OnPropertyChanged(); }
    }

    public double WriteSpeed
    {
        get { return _writeSpeed; }
        set { _writeSpeed = value; OnPropertyChanged(); }
    }

    public DiskViewModel() 
    {
        _driveLetter = string.Empty; 
        _driveType = string.Empty;
        _volumeName = string.Empty; 
        _fileSystemName = string.Empty;
        _busType = string.Empty;
        _vendor = string.Empty;
        _model = string.Empty;
        _guidPath = string.Empty;
        _guid = string.Empty;
        _totalSpaceBytes = 0;
        _availableSpaceBytes = 0;
        _freeSpaceBytes = 0;
        _bytesRead = 0;
        _bytesWritten = 0;
        _readSpeed = 0.0;
        _writeSpeed = 0.0;
    }

    public DiskViewModel(DiskInfo disk)
    {
        DriveLetter = disk.driveLetter;
        DriveType = disk.driveType;
        VolumeName = disk.volumeName;
        FileSystemName = disk.fileSystemName;
        BusType = disk.busType;
        Vendor = disk.vendor;
        Model = disk.model;
        GuidPath = disk.guidPath;
        TotalSpaceBytes = disk.totalSpaceBytes;
        AvailableSpaceBytes = disk.availableSpaceBytes;
        FreeSpaceBytes = disk.freeSpaceBytes;
        BytesRead = disk.bytesRead;
        BytesWritten = disk.bytesWritten;
        ReadSpeed = disk.readSpeed;
        WriteSpeed = disk.writeSpeed;

        // Extract GUID from the GUID path.
        // \\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\
        Guid = GuidPath?.Substring(11, 36);
    }

    public void Update(DiskInfo disk)
    {
        DriveLetter = disk.driveLetter;
        DriveType = disk.driveType;
        VolumeName = disk.volumeName;
        FileSystemName = disk.fileSystemName;
        BusType = disk.busType;
        Vendor = disk.vendor;
        Model = disk.model;
        GuidPath = disk.guidPath;
        TotalSpaceBytes = disk.totalSpaceBytes;
        AvailableSpaceBytes = disk.availableSpaceBytes;
        FreeSpaceBytes = disk.freeSpaceBytes;
        BytesRead = disk.bytesRead;
        BytesWritten = disk.bytesWritten;
        ReadSpeed = disk.readSpeed;
        WriteSpeed = disk.writeSpeed;
        
        // Extract GUID from the GUID path.
        // \\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\
        Guid = GuidPath?.Substring(11, 36);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
