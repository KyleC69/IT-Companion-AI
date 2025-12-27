[System.Reflection.Assembly]::LoadFrom('C:\Users\kcrow\.nuget\packages\microsoft.semantickernel.abstractions\1.68.0\lib\net8.0\Microsoft.SemanticKernel.Abstractions.dll') | Out-Null
[System.Reflection.Assembly]::LoadFrom('C:\Users\kcrow\.nuget\packages\microsoft.extensions.ai.abstractions\10.0.1\lib\net8.0\Microsoft.Extensions.AI.Abstractions.dll') | Out-Null
[System.Reflection.Assembly]::LoadFrom('C:\Users\kcrow\.nuget\packages\microsoft.extensions.vectordata.abstractions\9.7.0\lib\net8.0\Microsoft.Extensions.VectorData.Abstractions.dll') | Out-Null
$asm = [System.Reflection.Assembly]::LoadFrom('C:\Users\kcrow\.nuget\packages\microsoft.semantickernel.core\1.68.0\lib\net8.0\Microsoft.SemanticKernel.Core.dll')
$type = $asm.GetType('Microsoft.SemanticKernel.Data.VectorStoreTextSearch`1').MakeGenericType([System.Object])
foreach($member in $type.GetMembers([System.Reflection.BindingFlags]::Public -bor [System.Reflection.BindingFlags]::Instance -bor [System.Reflection.BindingFlags]::DeclaredOnly)) {
    Write-Output ($member.MemberType.ToString() + ': ' + $member.Name)
}
$asm = [System.Reflection.Assembly]::LoadFrom('C:\Users\kcrow\.nuget\packages\microsoft.semantickernel.connectors.sqlitevec\1.68.0-preview\lib\net8.0\Microsoft.SemanticKernel.Connectors.SqliteVec.dll')
$asm.GetExportedTypes() | Where-Object { $_.FullName -like '*Sqlite*' } | Select-Object FullName
$asm = [System.Reflection.Assembly]::LoadFrom('C:\Users\kcrow\.nuget\packages\microsoft.extensions.vectordata.abstractions\9.7.0\lib\net8.0\Microsoft.Extensions.VectorData.Abstractions.dll')
$asm.GetExportedTypes() | Where-Object { $_.Name -like '*RecordCollection*' } | Select-Object FullName
