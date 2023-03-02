Param (
        $BaseFolder,
        $FileVersion = $null,
        $CSSA_STORAGE_ACCOUNT_NAME,
        $CSSA_STORAGE_ACCOUNT_KEY,
        $CSSA_MP_DEPLOYMENT_CONTAINER,
        $CSSA_CERT_KEY_VAULT_NAME,
        $CSSA_SAS_EXPIRY_YEARS,
        $CSSA_SAS_EXPIRY_MONTHS,
        $CSSA_SAS_EXPIRY_DAYS
)

Write-Host "Starting upload & SaS key processing"
Write-Host "Using base directory:" $BaseFolder

Get-ChildItem
Import-Module .\Upload-CCWMPArtifact.psm1 -Force

Set-Location $BaseFolder

Write-Host "Processing DNS/SSL/CDN configurations"
Upload-CCWMPArtifact -FileName new-CCW-cssa-sub-domain.sh -WorkingFolder "$BaseFolder/_CCW/Deployment/Scripts/SSL" -FileVersion $FileVersion -StorageAccountName $CSSA_STORAGE_ACCOUNT_NAME -StorageAccountKey $CSSA_STORAGE_ACCOUNT_KEY -StorageAccountContainer $CSSA_MP_DEPLOYMENT_CONTAINER -KeyVaultName $CSSA_CERT_KEY_VAULT_NAME -ExpiryYears $CSSA_SAS_EXPIRY_YEARS -ExpiryMonths $CSSA_SAS_EXPIRY_MONTHS -ExpiryDays $CSSA_SAS_EXPIRY_DAYS

Write-Host "Processing application publishing configurations"
Upload-CCWMPArtifact -FileName Import-AllCCWApplications.ps1 -WorkingFolder "$BaseFolder/_CCW/Deployment/Scripts" -FileVersion $FileVersion -StorageAccountName $CSSA_STORAGE_ACCOUNT_NAME -StorageAccountKey $CSSA_STORAGE_ACCOUNT_KEY -StorageAccountContainer $CSSA_MP_DEPLOYMENT_CONTAINER -KeyVaultName $CSSA_CERT_KEY_VAULT_NAME -ExpiryYears $CSSA_SAS_EXPIRY_YEARS -ExpiryMonths $CSSA_SAS_EXPIRY_MONTHS -ExpiryDays $CSSA_SAS_EXPIRY_DAYS
Upload-CCWMPArtifact -FileName Import-DashboardData.ps1 -WorkingFolder "$BaseFolder/_CCW/Deployment/Scripts" -FileVersion $FileVersion -StorageAccountName $CSSA_STORAGE_ACCOUNT_NAME -StorageAccountKey $CSSA_STORAGE_ACCOUNT_KEY -StorageAccountContainer $CSSA_MP_DEPLOYMENT_CONTAINER -KeyVaultName $CSSA_CERT_KEY_VAULT_NAME -ExpiryYears $CSSA_SAS_EXPIRY_YEARS -ExpiryMonths $CSSA_SAS_EXPIRY_MONTHS -ExpiryDays $CSSA_SAS_EXPIRY_DAYS
Upload-CCWMPArtifact -FileName admin.zip -WorkingFolder "$BaseFolder/_CCW-admin/drop" -FileVersion $FileVersion -StorageAccountName $CSSA_STORAGE_ACCOUNT_NAME -StorageAccountKey $CSSA_STORAGE_ACCOUNT_KEY -StorageAccountContainer $CSSA_MP_DEPLOYMENT_CONTAINER -KeyVaultName $CSSA_CERT_KEY_VAULT_NAME -ExpiryYears $CSSA_SAS_EXPIRY_YEARS -ExpiryMonths $CSSA_SAS_EXPIRY_MONTHS -ExpiryDays $CSSA_SAS_EXPIRY_DAYS
Upload-CCWMPArtifact -FileName application.zip -WorkingFolder "$BaseFolder/_CCW-application/drop" -FileVersion $FileVersion -StorageAccountName $CSSA_STORAGE_ACCOUNT_NAME -StorageAccountKey $CSSA_STORAGE_ACCOUNT_KEY -StorageAccountContainer $CSSA_MP_DEPLOYMENT_CONTAINER -KeyVaultName $CSSA_CERT_KEY_VAULT_NAME -ExpiryYears $CSSA_SAS_EXPIRY_YEARS -ExpiryMonths $CSSA_SAS_EXPIRY_MONTHS -ExpiryDays $CSSA_SAS_EXPIRY_DAYS
Upload-CCWMPArtifact -FileName document.zip -WorkingFolder "$BaseFolder/_CCW-document/drop" -FileVersion $FileVersion -StorageAccountName $CSSA_STORAGE_ACCOUNT_NAME -StorageAccountKey $CSSA_STORAGE_ACCOUNT_KEY -StorageAccountContainer $CSSA_MP_DEPLOYMENT_CONTAINER -KeyVaultName $CSSA_CERT_KEY_VAULT_NAME -ExpiryYears $CSSA_SAS_EXPIRY_YEARS -ExpiryMonths $CSSA_SAS_EXPIRY_MONTHS -ExpiryDays $CSSA_SAS_EXPIRY_DAYS
Upload-CCWMPArtifact -FileName payment.zip -WorkingFolder "$BaseFolder/_CCW-payment/drop" -FileVersion $FileVersion -StorageAccountName $CSSA_STORAGE_ACCOUNT_NAME -StorageAccountKey $CSSA_STORAGE_ACCOUNT_KEY -StorageAccountContainer $CSSA_MP_DEPLOYMENT_CONTAINER -KeyVaultName $CSSA_CERT_KEY_VAULT_NAME -ExpiryYears $CSSA_SAS_EXPIRY_YEARS -ExpiryMonths $CSSA_SAS_EXPIRY_MONTHS -ExpiryDays $CSSA_SAS_EXPIRY_DAYS
Upload-CCWMPArtifact -FileName schedule.zip -WorkingFolder "$BaseFolder/_CCW-schedule/drop" -FileVersion $FileVersion -StorageAccountName $CSSA_STORAGE_ACCOUNT_NAME -StorageAccountKey $CSSA_STORAGE_ACCOUNT_KEY -StorageAccountContainer $CSSA_MP_DEPLOYMENT_CONTAINER -KeyVaultName $CSSA_CERT_KEY_VAULT_NAME -ExpiryYears $CSSA_SAS_EXPIRY_YEARS -ExpiryMonths $CSSA_SAS_EXPIRY_MONTHS -ExpiryDays $CSSA_SAS_EXPIRY_DAYS
Upload-CCWMPArtifact -FileName userprofile.zip -WorkingFolder "$BaseFolder/_CCW-UserProfile/drop" -FileVersion $FileVersion -StorageAccountName $CSSA_STORAGE_ACCOUNT_NAME -StorageAccountKey $CSSA_STORAGE_ACCOUNT_KEY -StorageAccountContainer $CSSA_MP_DEPLOYMENT_CONTAINER -KeyVaultName $CSSA_CERT_KEY_VAULT_NAME -ExpiryYears $CSSA_SAS_EXPIRY_YEARS -ExpiryMonths $CSSA_SAS_EXPIRY_MONTHS -ExpiryDays $CSSA_SAS_EXPIRY_DAYS

Rename-Item -Path "$BaseFolder/_CCW/Deployment/Scripts/mp-config.json" -NewName "config.json" -Force
Upload-CCWMPArtifact -FileName config.json -WorkingFolder "$BaseFolder/_CCW/Deployment/Scripts" -FileVersion $FileVersion -StorageAccountName $CSSA_STORAGE_ACCOUNT_NAME -StorageAccountKey $CSSA_STORAGE_ACCOUNT_KEY -StorageAccountContainer $CSSA_MP_DEPLOYMENT_CONTAINER -KeyVaultName $CSSA_CERT_KEY_VAULT_NAME -ExpiryYears $CSSA_SAS_EXPIRY_YEARS -ExpiryMonths $CSSA_SAS_EXPIRY_MONTHS -ExpiryDays $CSSA_SAS_EXPIRY_DAYS

$UiPackagePath = "$BaseFolder/_CCW-UI/drop"
Get-ChildItem -Path $UiPackagePath | Where-Object { $_.Name -match '^[0-9]*\.zip' } | Rename-Item -NewName ui.zip
Upload-CCWMPArtifact -FileName ui.zip -WorkingFolder $UiPackagePath -FileVersion $FileVersion -StorageAccountName $CSSA_STORAGE_ACCOUNT_NAME -StorageAccountKey $CSSA_STORAGE_ACCOUNT_KEY -StorageAccountContainer $CSSA_MP_DEPLOYMENT_CONTAINER -KeyVaultName $CSSA_CERT_KEY_VAULT_NAME -ExpiryYears $CSSA_SAS_EXPIRY_YEARS -ExpiryMonths $CSSA_SAS_EXPIRY_MONTHS -ExpiryDays $CSSA_SAS_EXPIRY_DAYS


Write-Host "Finished upload & SaS key processing"