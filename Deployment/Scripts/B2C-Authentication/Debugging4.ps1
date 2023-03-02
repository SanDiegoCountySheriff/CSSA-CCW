function GetAzureUrl
{
    param(
        [string] $urlKey
    )

    if($null -eq $azureUrls)
    {
        $azureUrls = (az cloud show --query endpoints) | ConvertFrom-Json 
    }

    $fullUrl = $azureUrls.$urlKey
    if($fullUrl.EndsWith('/'))
    {
        $ComputerName = $ComputerName
        $fullUrl = $fullUrl.Substring(0, $fullUrl.Length - 1)
    }

    return $fullUrl
}

$test = GetAzureUrl -urlKey "gallery"
$test