using Microsoft.Web.WebView2.Core;
using NativeMetrics.Services;
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

    private FormattedValue _totalSpaceFormatted;
    public FormattedValue TotalSpaceFormatted
    {
        get { return _totalSpaceFormatted; }
        set { _totalSpaceFormatted = value; OnPropertyChanged(); }
    }

    private FormattedValue _availableSpaceFormatted;
    public FormattedValue AvailableSpaceFormatted
    {
        get { return _availableSpaceFormatted; }
        set { _availableSpaceFormatted = value; OnPropertyChanged();}
    }

    private FormattedValue _freeSpaceFormatted;
    public FormattedValue FreeSpaceFormatted
    {
        get { return _freeSpaceFormatted; }
        set { _freeSpaceFormatted = value; OnPropertyChanged();}
    }

    private FormattedValue _bytesReadFormatted;
    public FormattedValue BytesReadFormatted
    {
        get { return _bytesReadFormatted; }
        set { _bytesReadFormatted = value; OnPropertyChanged(); }
    }

    private FormattedValue _bytesWrittenFormatted;
    public FormattedValue BytesWrittenFormatted
    {
        get { return _bytesWrittenFormatted; }
        set { _bytesWrittenFormatted = value; OnPropertyChanged(); }
    }

    private FormattedValue _readSpeedFormatted;
    public FormattedValue ReadSpeedFormatted
    {
        get { return _readSpeedFormatted; }
        set { _readSpeedFormatted = value; OnPropertyChanged(); }
    }

    private FormattedValue _writeSpeedFormatted;
    public FormattedValue WriteSpeedFormatted
    {
        get { return _writeSpeedFormatted; }
        set { _writeSpeedFormatted = value; OnPropertyChanged(); }
    }

    public DiskViewModel() 
    {
        _driveLetter = string.Empty; 
        _driveType = string.Empty;
        _volumeName = string.Empty; 
        _fileSystemName = string.Empty;
        _busType = string.Empty;
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

        TotalSpaceFormatted = new();
        AvailableSpaceFormatted = new();
        FreeSpaceFormatted = new();
        BytesReadFormatted = new();
        BytesWrittenFormatted = new();
        ReadSpeedFormatted = new();
        WriteSpeedFormatted = new();
    }

    public DiskViewModel(DiskInfo disk)
    {
        DriveLetter = disk.driveLetter;
        DriveType = disk.driveType;
        VolumeName = disk.volumeName;
        FileSystemName = disk.fileSystemName;
        BusType = disk.busType;
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

        TotalSpaceFormatted = Formatter.ByteMetricString(TotalSpaceBytes);
        AvailableSpaceFormatted = Formatter.ByteMetricString(AvailableSpaceBytes);
        FreeSpaceFormatted = Formatter.ByteMetricString(FreeSpaceBytes);
        BytesReadFormatted = Formatter.ByteMetricString(BytesRead);
        BytesWrittenFormatted = Formatter.ByteMetricString(BytesWritten);
        ReadSpeedFormatted = Formatter.DiskSpeedString(ReadSpeed);
        WriteSpeedFormatted = Formatter.DiskSpeedString(WriteSpeed);
    }

    public void Update(DiskInfo disk)
    {
        DriveLetter = disk.driveLetter;
        DriveType = disk.driveType;
        VolumeName = disk.volumeName;
        FileSystemName = disk.fileSystemName;
        BusType = disk.busType;
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

        TotalSpaceFormatted = Formatter.ByteMetricString(TotalSpaceBytes);
        AvailableSpaceFormatted = Formatter.ByteMetricString(AvailableSpaceBytes);
        FreeSpaceFormatted = Formatter.ByteMetricString(FreeSpaceBytes);
        BytesReadFormatted = Formatter.ByteMetricString(BytesRead);
        BytesWrittenFormatted = Formatter.ByteMetricString(BytesWritten);
        ReadSpeedFormatted = Formatter.DiskSpeedString(ReadSpeed);
        WriteSpeedFormatted = Formatter.DiskSpeedString(WriteSpeed);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
