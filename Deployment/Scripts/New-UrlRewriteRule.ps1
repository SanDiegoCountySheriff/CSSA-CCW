
function New-UrlRewriteRule
{
    param (
        [Parameter(Mandatory = $true, HelpMessage = "The name of the CDN Endpoint that needs rewrite")] 
        $EndpointName,
        [Parameter(Mandatory = $true, HelpMessage = "The name of the URL path that needs rewrite")] 
        $PathName,
        [Parameter(Mandatory = $true, HelpMessage = "The order number of the rule")] 
		$Order
	)

    $ruleName = "handle" + $PathName + "path"
    Write-Host "Using order #" $Order "for rule" $ruleName

    az cdn endpoint rule add `
        --subscription 5edf03ce-623c-47c3-b15f-dab2be8a319e `
        -g rg-cssa-it-ops-shd `
        --profile-name cssa-global-cdn `
        -n $EndpointName `
        --order $Order `
        --action-name UrlRewrite `
        --operator Contains `
        --match-variable UrlPath `
        --match-values $PathName `
        --transform Lowercase `
        --preserve-unmatched-path true `
        --rule-name $ruleName `
        --source-pattern "/$PathName" `
        --destination "/"
}

# $myOrder = 1
# New-UrlRewriteRule -EndpointName cep-sdsd-it-ccw-dev-public-001 -Order ($myOrder += 1) -PathName home
New-UrlRewriteRule -EndpointName cep-sdsd-it-ccw-dev-public-001 -Order ($myOrder += 1) -PathName application
New-UrlRewriteRule -EndpointName cep-sdsd-it-ccw-dev-public-001 -Order ($myOrder += 1) -PathName status
New-UrlRewriteRule -EndpointName cep-sdsd-it-ccw-dev-public-001 -Order ($myOrder += 1) -PathName finalize
New-UrlRewriteRule -EndpointName cep-sdsd-it-ccw-dev-public-001 -Order ($myOrder += 1) -PathName form
New-UrlRewriteRule -EndpointName cep-sdsd-it-ccw-dev-public-001 -Order ($myOrder += 1) -PathName form2
New-UrlRewriteRule -EndpointName cep-sdsd-it-ccw-dev-public-001 -Order ($myOrder += 1) -PathName moreinformation
New-UrlRewriteRule -EndpointName cep-sdsd-it-ccw-dev-public-001 -Order ($myOrder += 1) -PathName qualifyingquestions
New-UrlRewriteRule -EndpointName cep-sdsd-it-ccw-dev-public-001 -Order ($myOrder += 1) -PathName receipt
New-UrlRewriteRule -EndpointName cep-sdsd-it-ccw-dev-public-001 -Order ($myOrder += 1) -PathName renewform
New-UrlRewriteRule -EndpointName cep-sdsd-it-ccw-dev-public-001 -Order ($myOrder += 1) -PathName renewapplication
New-UrlRewriteRule -EndpointName cep-sdsd-it-ccw-dev-public-001 -Order ($myOrder += 1) -PathName renewform2
New-UrlRewriteRule -EndpointName cep-sdsd-it-ccw-dev-public-001 -Order ($myOrder += 1) -PathName penalcode

$myOrder = 1
New-UrlRewriteRule -EndpointName cep-sdsd-it-ccw-dev-admin-001 -Order ($myOrder += 1) -PathName home
New-UrlRewriteRule -EndpointName cep-sdsd-it-ccw-dev-admin-001 -Order ($myOrder += 1) -PathName dashboard
New-UrlRewriteRule -EndpointName cep-sdsd-it-ccw-dev-admin-001 -Order ($myOrder += 1) -PathName work
New-UrlRewriteRule -EndpointName cep-sdsd-it-ccw-dev-admin-001 -Order ($myOrder += 1) -PathName numbers
New-UrlRewriteRule -EndpointName cep-sdsd-it-ccw-dev-admin-001 -Order ($myOrder += 1) -PathName settings
New-UrlRewriteRule -EndpointName cep-sdsd-it-ccw-dev-admin-001 -Order ($myOrder += 1) -PathName applications
New-UrlRewriteRule -EndpointName cep-sdsd-it-ccw-dev-admin-001 -Order ($myOrder += 1) -PathName appointments

