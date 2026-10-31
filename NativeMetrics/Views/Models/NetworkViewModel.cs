using NativeMetrics.Services;
using NativeMetrics.Services.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace NativeMetrics.Views.Models;

public class NetworkViewModel : INotifyPropertyChanged
{
    private string? _alias;
    private string? _description;
    private ulong _luid;
    private ulong _type;
    private ulong _receivedLinkSpeedBits;
    private ulong _transmitLinkSpeedBits;
    private ulong _receivedBytes;
    private ulong _sentBytes;
    private double _downloadBytesPerSec;
    private double _uploadBytesPerSec;
    private double _throughput;
   
    public string? Alias
    {
        get { return _alias; }
        set { _alias = value; OnPropertyChanged(); }
    }

    public string? Description
    {
        get { return _description; }
        set { _description = value; OnPropertyChanged(); }
    }

    public ulong Luid
    {
        get { return _luid; }
        set { _luid = value; OnPropertyChanged(); }
    }

    public ulong Type 
    {
        get { return _type; }
        set { _type = value; OnPropertyChanged(); }
    }

    public ulong ReceivedLinkSpeedBits
    {
        get { return _receivedLinkSpeedBits; }
        set { _receivedLinkSpeedBits = value; OnPropertyChanged(); }
    }

    public ulong TransmitLinkSpeedBits
    {
        get { return _transmitLinkSpeedBits; }
        set { _transmitLinkSpeedBits = value; OnPropertyChanged(); }
    }

    public ulong ReceivedBytes
    {
        get { return _receivedBytes; }
        set { _receivedBytes = value; OnPropertyChanged(); }
    }

    public ulong SentBytes
    {
        get { return _sentBytes; }
        set { _sentBytes = value; OnPropertyChanged(); }
    }

    public double DownloadBytesPerSec
    {
        get { return _downloadBytesPerSec; }
        set { _downloadBytesPerSec = value; OnPropertyChanged(); }
    }

    public double UploadBytesPerSec
    {
        get { return _uploadBytesPerSec; }
        set { _uploadBytesPerSec = value; OnPropertyChanged(); }
    }

    public double Throughput
    {
        get { return _throughput; }
        set { _throughput = value; OnPropertyChanged(); }
    }

    private FormattedValue _receivedLSbFormatted;
    public FormattedValue ReceivedLSbFormatted
    {
        get { return _receivedLSbFormatted; }
        set { _receivedLSbFormatted = value; OnPropertyChanged(); }
    }

    private FormattedValue _transmitLSbFormatted;
    public FormattedValue TransmitLSbFormatted
    {
        get { return _transmitLSbFormatted; }
        set { _transmitLSbFormatted = value; OnPropertyChanged(); }
    }

    private FormattedValue _receivedBFormatted;
    public FormattedValue ReceiveBFormatted
    {
        get { return _receivedBFormatted; }
        set { _receivedBFormatted = value; OnPropertyChanged(); }
    }

    private FormattedValue _sentBFormatted;
    public FormattedValue SentBFormatted
    {
        get { return _sentBFormatted; }
        set { _sentBFormatted = value; OnPropertyChanged(); }
    }

    private FormattedValue _downloadBpsFormatted;
    public FormattedValue DownloadBpsFormatted
    {
        get { return _downloadBpsFormatted; }
        set { _downloadBpsFormatted = value; OnPropertyChanged(); }
    }

    private FormattedValue _uploadBpsFormatted;
    public FormattedValue UploadBpsFormatted
    {
        get { return _uploadBpsFormatted; }
        set { _uploadBpsFormatted = value; OnPropertyChanged(); }
    }

    private FormattedValue _throughputFormatted;
    public FormattedValue ThroughputFormatted
    {
        get { return _throughputFormatted; }
        set { _throughputFormatted = value; OnPropertyChanged(); }
    }

    public DynamicAxisScaler ThroughputHistory;
    public ObservableCollection<double> ThroughputObservable { get; } = new();
    public double ThroughputMax
    {
        get { return ThroughputHistory.CurrentMax; }
    }
    
    public NetworkViewModel() 
    {
        _alias = string.Empty;
        _description = string.Empty;
        _luid = 0;
        _type = 0;
        _receivedLinkSpeedBits = 0;
        _transmitLinkSpeedBits = 0;
        _receivedBytes = 0;
        _sentBytes = 0;
        _downloadBytesPerSec = 0.0;
        _uploadBytesPerSec = 0.0;
        _throughput = 0.0;

        ReceivedLSbFormatted = new();
        TransmitLSbFormatted = new();
        ReceiveBFormatted = new();
        SentBFormatted = new();
        DownloadBpsFormatted = new();
        UploadBpsFormatted = new();
        ThroughputFormatted = new();

        ThroughputHistory = new();
    }

    public NetworkViewModel(NetworkAdapterInfo adapter)
    {
        Alias = adapter.alias;
        Description = adapter.description;
        Luid = adapter.luid;
        Type = adapter.type;
        ReceivedLinkSpeedBits = adapter.receivedLinkSpeedBits;
        TransmitLinkSpeedBits = adapter.transmitLinkSpeedBits;
        ReceivedBytes = adapter.receivedBytes;
        SentBytes = adapter.sentBytes;
        DownloadBytesPerSec = adapter.downloadBytesPerSec;
        UploadBytesPerSec = adapter.uploadBytesPerSec;
        Throughput = adapter.throughput;

        ReceivedLSbFormatted = Formatter.LinkSpeedString(ReceivedLinkSpeedBits);
        TransmitLSbFormatted = Formatter.LinkSpeedString(TransmitLinkSpeedBits);
        ReceiveBFormatted = Formatter.ByteMetricString(ReceivedBytes);
        SentBFormatted = Formatter.ByteMetricString(SentBytes);
        // we can use DiskSpeedString() to also get the download and upload speeds and throughput
        DownloadBpsFormatted = Formatter.DiskSpeedString(DownloadBytesPerSec);
        UploadBpsFormatted = Formatter.DiskSpeedString(UploadBytesPerSec);
        ThroughputFormatted = Formatter.DiskSpeedString(Throughput);

        ThroughputHistory = new();
    }

    public void Update(NetworkAdapterInfo adapter)
    {
        Alias = adapter.alias;
        Description = adapter.description;
        Luid = adapter.luid;
        Type = adapter.type;
        ReceivedLinkSpeedBits = adapter.receivedLinkSpeedBits;
        TransmitLinkSpeedBits = adapter.transmitLinkSpeedBits;
        ReceivedBytes = adapter.receivedBytes;
        SentBytes = adapter.sentBytes;
        DownloadBytesPerSec = adapter.downloadBytesPerSec;
        UploadBytesPerSec = adapter.uploadBytesPerSec;
        Throughput = adapter.throughput;

        ReceivedLSbFormatted = Formatter.LinkSpeedString(ReceivedLinkSpeedBits);
        TransmitLSbFormatted = Formatter.LinkSpeedString(TransmitLinkSpeedBits);
        ReceiveBFormatted = Formatter.ByteMetricString(ReceivedBytes);
        SentBFormatted = Formatter.ByteMetricString(SentBytes);
        // we can use DiskSpeedString() to also get the download and upload speeds and throughput
        DownloadBpsFormatted = Formatter.DiskSpeedString(DownloadBytesPerSec);
        UploadBpsFormatted = Formatter.DiskSpeedString(UploadBytesPerSec);
        ThroughputFormatted = Formatter.DiskSpeedString(Throughput);

        ThroughputHistory.History.Add(Throughput);

        if (ThroughputObservable.Count >= 61)
        {
            ThroughputObservable.RemoveAt(0);
        }
        ThroughputHistory.Update();
        ThroughputObservable.Add(Throughput / ThroughputHistory.Divisor);
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
