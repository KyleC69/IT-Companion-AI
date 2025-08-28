param(
    [string]$DriverPath = "C:\Drivers",
    [switch]$ListOnly
)

if (-not (Test-Path $DriverPath)) {
    Write-Error "Path not found: $DriverPath"
    exit 1
}

Get-ChildItem -Path $DriverPath -Filter *.sys | ForEach-Object {
    if ($ListOnly) {
        $_.FullName
    }
    else {
        # Placeholder for load logic
        Write-Host "Would load driver:" $_.Name
    }
}