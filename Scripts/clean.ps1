<#
    Clear contents of build folders:
        ./clean.ps1             # clear all builds 
        ./clean.ps1 -Debug      # clear all debug builds 
        ./clean.ps1 -Release    # clear all release builds
#>

param(
    [switch]$Debug,
    [switch]$Clean
)

# root directory
$RootDir = Resolve-Path (Get-Item "$PSScriptRoot\..\NativeMetrics_core").FullName

# exe paths
$DebugPathMsvc = Join-Path $RootDir "build\x64-windows-msvc-debug"
$DebugPathNinja = Join-Path $RootDir "build\x64-windows-ninja-debug"
$ReleasePathMsvc = Join-Path $RootDir "build\x64-windows-msvc-release"
$ReleasePathNinja = Join-Path $RootDir "build\x64-windows-ninja-release"

if ($Debug) {
    Write-Host "Cleaning debug builds..."
    if (Test-Path $DebugPathMsvc) {
        Remove-Item -Recurse -Force $DebugPathMsvc
    }

    if (Test-Path $DebugPathNinja) {
        Remove-Item -Recurse -Force $DebugPathNinja
    }

    Write-Host "Cleaned debug builds."
}
elseif ($Release) {
    Write-Host "Cleaning release builds..."
    if (Test-Path $ReleasePathMsvc) {
        Remove-Item -Recurse -Force $ReleasePathMsvc
    }
    
    if (Test-Path $ReleasePathNinja) {
        Remove-Item -Recurse -Force $ReleasePathNinja
    }

    Write-Host "Cleaned release builds."
}
else {
    Write-Host "Cleaning all builds..."
    if (Test-Path $DebugPathMsvc) {
        Remove-Item -Recurse -Force $DebugPathMsvc
    }

    if (Test-Path $DebugPathNinja) {
        Remove-Item -Recurse -Force $DebugPathNinja
    }

    if (Test-Path $ReleasePathMsvc) {
        Remove-Item -Recurse -Force $ReleasePathMsvc
    }
    
    if (Test-Path $ReleasePathNinja) {
        Remove-Item -Recurse -Force $ReleasePathNinja
    }

    Write-Host "Done."
}