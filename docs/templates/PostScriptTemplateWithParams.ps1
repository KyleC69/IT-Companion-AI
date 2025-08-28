#Requires -Version 7.2
<#
.SYNOPSIS
Template script description (update this).

.DESCRIPTION
Longer description of what this script does. Include assumptions, side effects, idempotency notes.

.PARAMETER Name
One or more target names (supports pipeline input).

.PARAMETER Environment
Execution environment selector (Dev/Test/Prod).

.PARAMETER TimeoutSeconds
Operation timeout in seconds.

.PARAMETER Credential
Credential used for authenticated operations (mutually exclusive with ApiKey).

.PARAMETER ApiKey
API key alternative to Credential.

.PARAMETER ConfigPath
Optional path to a JSON/YAML config file.

.PARAMETER OutputPath
If provided, results will also be written to this file (JSON).

.PARAMETER Force
Bypasses confirmation or safety checks.

.PARAMETER PassThru
Emits resulting objects to the pipeline.

.EXAMPLE
.\TemplateScript.ps1 -Name Alpha,Beta -Environment Test -Verbose

.EXAMPLE
'Alpha','Beta' | .\TemplateScript.ps1 -ApiKey $env:MY_API_KEY -WhatIf

.NOTES
Author: (you)
#>

[CmdletBinding(SupportsShouldProcess = $true,
               ConfirmImpact = 'Medium',
               DefaultParameterSetName = 'CredentialSet')]
param(
    [Parameter(Mandatory = $true,
               Position = 0,
               ValueFromPipeline = $true,
               ValueFromPipelineByPropertyName = $true)]
               [Alias ('Name')]
    [ValidateNotNullOrEmpty()]
    [string[]] $Name,

    [ValidateSet('Dev','Test','Prod')]
    [string] $Environment = 'Dev',

    [ValidateRange(1, 3600)]
    [int] $TimeoutSeconds = 30,

    [Parameter(ParameterSetName = 'CredentialSet')]
    [System.Management.Automation.PSCredential] $Credential,

    [Parameter(ParameterSetName = 'ApiKeySet')]
    [ValidateNotNullOrEmpty()]
    [string] $ApiKey,

    [ValidateNotNullOrEmpty()]
    [string] $ConfigPath,

    [ValidateNotNullOrEmpty()]
    [string] $OutputPath,

    [switch] $Force,
    [switch] $PassThru
)

begin {
    Set-StrictMode -Version Latest
    $ErrorActionPreference = 'Stop'

    function Write-Log {
        [CmdletBinding()]
        param(
            [Parameter(Mandatory)][string] $Message,
            [ValidateSet('INFO','WARN','ERROR','DEBUG','VERBOSE')]
            [string] $Level = 'INFO'
        )
        $ts = (Get-Date).ToString('o')
        $line = "[{0}] [{1}] {2}" -f $ts, $Level, $Message
        switch ($Level) {
            'ERROR'   { Write-Error $Message }
            'WARN'    { Write-Warning $Message }
            'DEBUG'   { Write-Debug $Message }
            'VERBOSE' { Write-Verbose $Message }
            default   { Write-Host $line }
        }
    }

    function Load-Config {
        param([string] $Path)
        if (-not $Path) { return @{} }
        if (-not (Test-Path -LiteralPath $Path)) {
            throw "Config file not found: $Path"
        }
        $raw = Get-Content -LiteralPath $Path -Raw
        switch -Wildcard ($Path) {
            '*.json' { return $raw | ConvertFrom-Json }
            '*.yml'  { throw "YAML parsing not implemented. Convert to JSON or extend Load-Config." }
            '*.yaml' { throw "YAML parsing not implemented. Convert to JSON or extend Load-Config." }
            default  { throw "Unsupported config format: $Path" }
        }
    }

    $Script:Config = Load-Config -Path $ConfigPath

    $Script:Results = New-Object System.Collections.Generic.List[object]

    Write-Verbose "ParameterSetName: $($PSCmdlet.ParameterSetName)"
    Write-Verbose "Environment: $Environment"
    if ($PSCmdlet.ParameterSetName -eq 'CredentialSet' -and $Credential) {
        Write-Verbose "Using credential for user: $($Credential.UserName)"
    } elseif ($PSCmdlet.ParameterSetName -eq 'ApiKeySet') {
        Write-Verbose "Using API key (masked)."
    }
}

process {
    foreach ($n in $Name) {
        $target = $n.Trim()
        if (-not $target) { continue }

        if ($PSCmdlet.ShouldProcess($target, "Process target in $Environment")) {

            if (-not $Force -and $Environment -eq 'Prod') {
                Write-Verbose "Production safeguard active (use -Force to override)."
                # Insert additional pre-check logic here
            }

            try {
                Write-Log -Message "Starting $target" -Level VERBOSE

                # ---- Core operation placeholder ----
                # Simulate work (replace with real logic):
                $sw = [System.Diagnostics.Stopwatch]::StartNew()
                Start-Sleep -Milliseconds 150
                $sw.Stop()

                $obj = [PSCustomObject]@{
                    Name          = $target
                    Environment   = $Environment
                    DurationMs    = $sw.ElapsedMilliseconds
                    TimestampUtc  = (Get-Date).ToUniversalTime()
                    Status        = 'Success'
                }

                $Script:Results.Add($obj)

                if ($PassThru) {
                    $obj
                }

            } catch {
                $err = $_
                Write-Log -Message "Error processing '$target': $($err.Exception.Message)" -Level ERROR
                $obj = [PSCustomObject]@{
                    Name          = $target
                    Environment   = $Environment
                    DurationMs    = $sw.ElapsedMilliseconds
                    TimestampUtc  = (Get-Date).ToUniversalTime()
                    Status        = 'Failed'
                    Error         = $err.Exception.Message
                }
                $Script:Results.Add($obj)
                if ($PassThru) { $obj }
                continue
            }
        }
    }
}

end {
    if ($OutputPath) {
        try {
            $dir = Split-Path -Parent $OutputPath
            if ($dir -and -not (Test-Path -LiteralPath $dir)) {
                New-Item -ItemType Directory -Path $dir | Out-Null
            }
            $json = $Script:Results | ConvertTo-Json -Depth 6
            $json | Set-Content -LiteralPath $OutputPath -Encoding UTF8
            Write-Verbose "Wrote results to $OutputPath"
        } catch {
            Write-Log -Message "Failed to write output: $($_.Exception.Message)" -Level ERROR
        }
    }

    if (-not $PassThru) {
        Write-Verbose "Completed. Total items: $($Script:Results.Count)"
    }
}