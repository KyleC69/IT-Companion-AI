param(
    [string]$LogName = "Security",
    [string]$ProviderName,
    [int]$MaxEvents = 10
)

$filter = @{ LogName = $LogName }
if ($ProviderName) { $filter.ProviderName = $ProviderName }

Get-WinEvent -FilterHashtable $filter -MaxEvents $MaxEvents | ForEach-Object {
    [PSCustomObject]@{
        HostId    = $env:COMPUTERNAME
        SourceId  = $_.ProviderName
        EventId   = $_.Id
        Timestamp = $_.TimeCreated.ToUniversalTime()
        Payload   = @{ Message = $_.Message }
    }
} | ConvertTo-Json -Depth 5