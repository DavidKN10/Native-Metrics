<#
    Run a test:
        ./run.ps1 -{build type} -{build system} -{test choice}
#>

param(
    [switch]$Debug,
    [switch]$Release,
    [switch]$Msvc,
    [switch]$Ninja,
    [switch]$Cpu,
    [switch]$Disk,
    [switch]$Gpu,
    [switch]$Memory,
    [switch]$Network,
    [switch]$Process
)

# root directory
$RootDir = Resolve-Path (Get-Item "$PSScriptRoot\..\NativeMetrics_core").FullName

# exe names
$CpuExe = "CpuTest.exe"
$DiskExe = "DiskTest.exe"
$GpuExe = "GpuTest.exe"
$MemoryExe = "MemoryTest.exe"
$NetworkExe = "NetworkTest.exe"
$ProcessExe = "ProcessTest.exe"

# build paths
$DebugPathMsvc = Join-Path $RootDir "build\x64-windows-msvc-debug\Debug"
$DebugPathNinja = Join-Path $RootDir "build\x64-windows-ninja-debug"
$ReleasePathMsvc = Join-Path $RootDir "build\x64-windows-msvc-release\Release"
$ReleasePathNinja = Join-Path $RootDir "build\x64-windows-ninja-release"

# exe paths
$CpuDebugMsvc = Join-Path $DebugPathMsvc $CpuExe
$CpuDebugNinja = Join-Path $DebugPathNinja $CpuExe
$CpuReleaseMsvc = Join-Path $ReleasePathMsvc $CpuExe
$CpuReleaseNinja = Join-Path $ReleasePathNinja $CpuExe

$DiskDebugMsvc = Join-Path $DebugPathMsvc $DiskExe
$DiskDebugNinja = Join-Path $DebugPathNinja $DiskExe
$DiskReleaseMsvc = Join-Path $ReleasePathMsvc $DiskExe
$DiskReleaseNinja = Join-Path $ReleasePathNinja $DiskExe

$GpuDebugMsvc = Join-Path $DebugPathMsvc $GpuExe
$GpuDebugNinja = Join-Path $DebugPathNinja $GpuExe
$GpuReleaseMsvc = Join-Path $ReleasePathMsvc $GpuExe
$GpuReleaseNinja = Join-Path $ReleasePathNinja $GpuExe

$MemoryDebugMsvc = Join-Path $DebugPathMsvc $MemoryExe
$MemoryDebugNinja = Join-Path $DebugPathNinja $MemoryExe
$MemoryReleaseMsvc = Join-Path $ReleasePathMsvc $MemoryExe
$MemoryReleaseNinja = Join-Path $ReleasePathNinja $MemoryExe

$NetworkDebugMsvc = Join-Path $DebugPathMsvc $NetworkExe
$NetworkDebugNinja = Join-Path $DebugPathNinja $NetworkExe
$NetworkReleaseMsvc = Join-Path $ReleasePathMsvc $NetworkExe
$NetworkReleaseNinja = Join-Path $ReleasePathNinja $NetworkExe

$ProcessDebugMsvc = Join-Path $DebugPathMsvc $ProcessExe
$ProcessDebugNinja = Join-Path $DebugPathNinja $ProcessExe
$ProcessReleaseMsvc = Join-Path $ReleasePathMsvc $ProcessExe
$ProcessReleaseNinja = Join-Path $ReleasePathNinja $ProcessExe

if ($Debug) {
    if ($Msvc) {
        if ($Cpu) {
            if (!(Test-Path $CpuDebugMsvc)) {
                Write-Error "Could not find CpuTest.exe"
            }
            & $CpuDebugMsvc
        }
        elseif ($Disk) {
            if (!(Test-Path $DiskDebugMsvc)) {
                Write-Error "Could not find DiskTest.exe"
            }
            & $DiskDebugMsvc
        }
        elseif ($Gpu) {
            if (!(Test-Path $GpuDebugMsvc)) {
                Write-Error "Could not find GpuTest.exe"
            }
            & $GpuDebugMsvc
        }
        elseif ($Memory) {
            if (!(Test-Path $MemoryDebugMsvc)) {
                Write-Error "Could not find MemoryTest.exe"
            }
            & $MemoryDebugMsvc
        }
        elseif ($Network) {
            if (!(Test-Path $NetworkDebugMsvc)) {
                Write-Error "Could not find NetworkTest.exe"
            }
            & $NetworkDebugMsvc 
        }
        elseif ($Process) {
            if (!(Test-Path $ProcessDebugMsvc)) {
                Write-Error "Could not find ProcessTest.exe"
            }
            & $ProcessDebugMsvc 
        }
        else {
            Write-Host "Did not specify a test."
        }
    }
    elseif ($Ninja) {
        if ($Cpu) {
            if (!(Test-Path $CpuDebugNinja)) {
                Write-Error "Could not find CpuTest.exe"
            }
            & $CpuDebugNinja 
        }
        elseif ($Disk) {
            if (!(Test-Path $DiskDebugNinja)) {
                Write-Error "Could not find DiskTest.exe"
            }
            & $DiskDebugNinja 
        }
        elseif ($Gpu) {
            if (!(Test-Path $GpuDebugNinja)) {
                Write-Error "Could not find GpuTest.exe"
            }
            & $GpuDebugNinja
        }
        elseif ($Memory) {
            if (!(Test-Path $MemoryDebugNinja)) {
                Write-Error "Could not find MemoryTest.exe"
            }
            & $MemoryDebugNinja 
        }
        elseif ($Network) {
            if (!(Test-Path $NetworkDebugNinja)) {
                Write-Error "Could not find NetworkTest.exe"
            }
            & $NetworkDebugNinja 
        }
        elseif ($Process) {
            if (!(Test-Path $ProcessDebugNinja)) {
                Write-Error "Could not find ProcessTest.exe"
            }
            & $ProcessDebugNinja 
        }
        else {
            Write-Host "Did not specify a test."
        }
    }
    else {
        Write-Host "Did not specify a build system."
    }
}
elseif ($Release) {
    if ($Msvc) {
        if ($Cpu) {
            if (!(Test-Path $CpuReleaseMsvc)) {
                Write-Error "Could not find CpuTest.exe"
            }
            & $CpuReleaseMsvc 
        }
        elseif ($Disk) {
            if (!(Test-Path $DiskReleaseMsvc)) {
                Write-Error "Could not find DiskTest.exe"
            }
            & $DiskReleaseMsvc 
        }
        elseif ($Gpu) {
            if (!(Test-Path $GpuReleaseMsvc)) {
                Write-Error "Could not find GpuTest.exe"
            }
            & $GpuReleaseMsvc
        }
        elseif ($Memory) {
            if (!(Test-Path $MemoryReleaseMsvc)) {
                Write-Error "Could not find MemoryTest.exe"
            }
            & $MemoryReleaseMsvc 
        }
        elseif ($Network) {
            if (!(Test-Path $NetworkReleaseMsvc)) {
                Write-Error "Could not find NetworkTest.exe"
            }
            & $NetworkReleaseMsvc 
        }
        elseif ($Process) {
            if (!(Test-Path $ProcessReleaseMsvc)) {
                Write-Error "Could not find ProcessTest.exe"
            }
            & $ProcessReleaseMsvc 
        }
        else {
            Write-Host "Did not specify a test."
        }
    }
    elseif ($Ninja) {
        if ($Cpu) {
            if (!(Test-Path $CpuReleaseNinja)) {
                Write-Error "Could not find CpuTest.exe"
            }
            & $CpuReleaseNinja 
        }
        elseif ($Disk) {
            if (!(Test-Path $DiskReleaseNinja)) {
                Write-Error "Could not find DiskTest.exe"
            }
            & $DiskReleaseNinja
        }
        elseif ($Gpu) {
            if (!(Test-Path $GpuReleaseNinja)) {
                Write-Error "Could not find GpuTest.exe"
            }
            & $GpuReleaseNinja 
        }
        elseif ($Memory) {
            if (!(Test-Path $MemoryReleaseNinja)) {
                Write-Error "Could not find MemoryTest.exe"
            }
            & $MemoryReleaseNinja 
        }
        elseif ($Network) {
            if (!(Test-Path $NetworkReleaseNinja)) {
                Write-Error "Could not find NetworkTest.exe"
            }
            & $NetworkReleaseNinja 
        }
        elseif ($Process) {
            if (!(Test-Path $ProcessReleaseNinja)) {
                Write-Error "Could not find ProcessTest.exe"
            }
            & $ProcessReleaseNinja 
        }
        else {
            Write-Host "Did not specify a test."
        }
    }
    else {
        Write-Host "Did not specify a build system."
    }
}
else {
    Write-Error "Did not specify build type."
}
