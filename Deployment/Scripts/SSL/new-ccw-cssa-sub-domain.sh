echo "Setting variables"
echo

OUTPUT_LEVEL="DEBUG"
echo "OUTPUT_LEVEL:" $OUTPUT_LEVEL

sleep_time=1s
echo "Setting sleep time to: $sleep_time"

# Creates DNS & CDN Endpoints and configures custom DNS for cssa.cloud based URLs
echo
echo
echo "Starting DNS/Custom Host/Certificate deployment"
echo

# OUTPUT_LEVEL: Set to DEBUG to output all result objects to the output window
# CLOUD_TYPE: This identifies the Azure Cloud that this application is being deployed to
# CSSA_TENANT_ID: This is the core CSSA Tenant ID
# CSSA_SP_APP_ID: This is a Service Principal that has been granted access to the core CSSA global resources
# CSSA_SP_SECRET: This is the password/secret for the Service Principal
# CSSA_SHD_SUBSCRIPTION_ID: This is the CSSA Subscription ID where the CSSA global resources are
# CSSA_RESOURCE_GROUP_NAME: This is the main resource group where the CSSA global are. KV, CDN, DNS Zone (sdsd-gen-p-rg)
# CSSA_CDN_RESOURCE_GROUP_NAME: This is a destination resource group where CDN will be homed
# CSSA_CDN_PROFILE_NAME: This is the CSSA global DNS Zone resource name (cssa.cloud)
# CSSA_CDN_ENDPOINT_NAME: The name that should be used to deploy the CDN Endpoint
# CSSA_CDN_ENDPOINT_TYPE: The type of configuration, i.e., admin UI or public UI. Must be either 'admin' or 'public'
# CSSA_DNS_ROOT_ZONE: This is the root DNS entry for the CSSA DNS ZOne (cssa.cloud)
# CSSA_CERT_KEY_VAULT_RG: This is the resource group where the CSSA wildcard certificate is stored (sdsd-gen-p-rg)
# CSSA_CERT_KEY_VAULT_NAME: This is the name of hte key vault where the CSSA wildcard certificate is stored (sdsd-gen-p-kv)
# CSSA_CERT_SECRET_NAME: This is the name of the secret where the CSSA wildcard certificate is stored (star-cssa-cloud)
# AGENCY_ORI: The California Justice Department unique identifier for this agency
# AGENCY_ABBREVIATION: This is the deploying agencies identified abbreviation
# APPLICATION_SUBSCRIPTION_ID: This is the subscription where the application is being deployed to
# APPLICATION_RESOURCE_GROUP_NAME: This is the agncies Azure resource group where the application is deployed
# APPLICATION_NAME: This is the name of the deployed application (ccw)
# APPLICATION_UI_SA_NAME: This is the name os the UI storage account where the application is deployed
# APP_DOMAIN_TYPE: This identifies the type of domain the user chose during deployment (private_domain or cssa_cloud_domain)
# APPLICATION_FW_PUBLIC_IPA: This is the IP Address for the agencies Firewall Protected IP Address. This is NOT the PIP associated directly to the AG

# These settings are only used when the user select private/custom domain
# CUSTOM_PRIMARY_DOMAIN: This is the primary domain name if the user chose private/custom domain (sample.gov)
# CUSTOM_APP_CNAME_ALIAS: This is the application alias the user chose whe using private/custom domain
# CUSTOM_CERT_KEY_VAULT_RID: This is the resource id (RID) of the key vault that holds the private/custom certificate (/subscriptions/{subscriptionId}/resourceGroups/{rg}/providers/Microsoft.KeyVault/vaults/{keyVaultName})
# CUSTOM_CERT_SECRET_NAME: This is the secret name of the private/custom certififcate

API_APPLICATION_NAME="$APPLICATION_NAME-api-$AGENCY_ABBREVIATION";

if [ $CSSA_CDN_ENDPOINT_TYPE == 'admin' ]
then
    APPLICATION_NAME="$APPLICATION_NAME-admin";
    echo "CSSA_CDN_ENDPOINT_TYPE: " $CSSA_CDN_ENDPOINT_TYPE "using ADMIN configuration" $APPLICATION_NAME;
fi

dns_sub_domain_name=$APPLICATION_NAME-$AGENCY_ABBREVIATION

# If user chose private/custom domain we must remap the correct values provided from MP UI
if [ $APP_DOMAIN_TYPE == 'private_domain' ]
then
    echo "Using private domain configuration"

    CSSA_DNS_ROOT_ZONE=$CUSTOM_PRIMARY_DOMAIN
    APPLICATION_NAME=$CUSTOM_PRIMARY_DOMAIN

    RID=$CUSTOM_CERT_KEY_VAULT_RID
    parts=(${RID//// })
    CSSA_CERT_KEY_VAULT_RG=${parts[3]}
    CSSA_CERT_KEY_VAULT_NAME=${parts[7]}
    CSSA_CERT_SECRET_NAME=$CUSTOM_CERT_SECRET_NAME

    dns_sub_domain_name=$CUSTOM_APP_CNAME_ALIAS
fi

echo "CLOUD_TYPE: " $CLOUD_TYPE
echo "CSSA_TENANT_ID: " $CSSA_TENANT_ID
echo "CSSA_SP_APP_ID: " $CSSA_SP_APP_ID
echo "CSSA_SHD_SUBSCRIPTION_ID: " $CSSA_SHD_SUBSCRIPTION_ID
echo "CSSA_RESOURCE_GROUP_NAME: " $CSSA_RESOURCE_GROUP_NAME

echo "CSSA_CDN_RESOURCE_GROUP_NAME: " $CSSA_CDN_RESOURCE_GROUP_NAME
echo "CSSA_CDN_PROFILE_NAME: " $CSSA_CDN_PROFILE_NAME
echo "CSSA_CDN_ENDPOINT_NAME: " $CSSA_CDN_ENDPOINT_NAME

echo "CSSA_DNS_ROOT_ZONE: " $CSSA_DNS_ROOT_ZONE
echo "AGENCY_ORI: " $AGENCY_ORI
echo "AGENCY_ABBREVIATION: " $AGENCY_ABBREVIATION
echo "APPLICATION_SUBSCRIPTION_ID: " $APPLICATION_SUBSCRIPTION_ID
echo "APPLICATION_RESOURCE_GROUP_NAME: " $APPLICATION_RESOURCE_GROUP_NAME
echo "APPLICATION_NAME: " $APPLICATION_NAME
echo "APPLICATION_UI_SA_NAME: " $APPLICATION_UI_SA_NAME
echo "API_APPLICATION_NAME:" $API_APPLICATION_NAME
echo "CSSA_CERT_KEY_VAULT_RG: " $CSSA_CERT_KEY_VAULT_RG
echo "CSSA_CERT_KEY_VAULT_NAME: " $CSSA_CERT_KEY_VAULT_NAME
echo "CSSA_CERT_SECRET_NAME: " $CSSA_CERT_SECRET_NAME
echo 

echo "APP_DOMAIN_TYPE: " $APP_DOMAIN_TYPE
echo "CUSTOM_PRIMARY_DOMAIN: " $CUSTOM_PRIMARY_DOMAIN
echo "CUSTOM_APP_CNAME_ALIAS: " $CUSTOM_APP_CNAME_ALIAS
echo "CUSTOM_CERT_KEY_VAULT_RID: " $CUSTOM_CERT_KEY_VAULT_RID
echo "CUSTOM_CERT_SECRET_NAME: " $CUSTOM_CERT_SECRET_NAME
echo

echo "Setting Azure cloud:" $CLOUD_TYPE
az cloud set -n $CLOUD_TYPE

echo "Logging in with Azure CLI:" $CSSA_TENANT_ID $CSSA_SP_APP_ID
az login --service-principal --tenant $CSSA_TENANT_ID -u $CSSA_SP_APP_ID -p "$CSSA_SP_SECRET"

echo "Setting default subscription:" $CSSA_SHD_SUBSCRIPTION_ID
az account set -s $CSSA_SHD_SUBSCRIPTION_ID
az account show
echo


echo "Configuring storage account static website:" $APPLICATION_RESOURCE_GROUP_NAME"/"$APPLICATION_UI_SA_NAME
staticWebResult=$(az storage blob service-properties update --subscription $APPLICATION_SUBSCRIPTION_ID --account-name $APPLICATION_UI_SA_NAME --index-document index.html --static-website true 2>&1)
if [ $OUTPUT_LEVEL == 'DEBUG' ]; then echo "$staticWebResult"; fi;

WEB_CONTENT_URL1=$(az storage account show --subscription $APPLICATION_SUBSCRIPTION_ID -g $APPLICATION_RESOURCE_GROUP_NAME -n $APPLICATION_UI_SA_NAME --query "primaryEndpoints.web" --output tsv 2>&1)
echo "WEB_CONTENT_URL1: " $WEB_CONTENT_URL1
WEB_CONTENT_URL2="${WEB_CONTENT_URL1///$''}"
echo "WEB_CONTENT_URL2: " $WEB_CONTENT_URL2
WEB_CONTENT_URL="${WEB_CONTENT_URL2/https:$''}"
echo "WEB_CONTENT_URL: " $WEB_CONTENT_URL
echo

dns_host_name=$dns_sub_domain_name.$CSSA_DNS_ROOT_ZONE

echo "API_APPLICATION_NAME:" $API_APPLICATION_NAME

# Default to Gov cloud, change if not
cname_alias=$CSSA_CDN_ENDPOINT_NAME".azureedge.us"
if [ $CLOUD_TYPE = "AzureCloud" ]
then
    echo "Using azureedge.net"
    cname_alias=$CSSA_CDN_ENDPOINT_NAME".azureedge.net"
fi

dns_hostname_name=${dns_host_name//.}
dns_hostname_name=${dns_hostname_name//-}

echo "dns_sub_domain_name: " $dns_sub_domain_name
echo "dns_host_name: " $dns_host_name
echo "CSSA_CDN_ENDPOINT_NAME: " $CSSA_CDN_ENDPOINT_NAME
echo "cname_alias: " $cname_alias
echo "dns_hostname_name: " $dns_hostname_name
echo

if [ $APP_DOMAIN_TYPE == 'cssa_cloud_domain' ]
then
    # Create DNS CNAMEs for the application URL and for CDN Verify
    echo "Using CSSA DNS configuration"
    echo "Creating/updating DNS CNAMES"
    result=$(az network dns record-set cname set-record --subscription $CSSA_SHD_SUBSCRIPTION_ID -g $CSSA_RESOURCE_GROUP_NAME -z $CSSA_DNS_ROOT_ZONE -n $dns_sub_domain_name -c $cname_alias 2>&1)
    if [ $OUTPUT_LEVEL == 'DEBUG' ]; then echo "$result"; fi;
    echo "Created" $dns_sub_domain_name "record"
    
    result=$(az network dns record-set cname set-record --subscription $CSSA_SHD_SUBSCRIPTION_ID -g $CSSA_RESOURCE_GROUP_NAME -z $CSSA_DNS_ROOT_ZONE -n cdnverify.$dns_sub_domain_name -c cdnverify.$cname_alias 2>&1)
    if [ $OUTPUT_LEVEL == 'DEBUG' ]; then echo "$result"; fi;
    echo "Created cdnverify."$dns_sub_domain_name "record"

    echo "APPLICATION_FW_PUBLIC_IPA:" $APPLICATION_FW_PUBLIC_IPA

    if [ -n "$APPLICATION_FW_PUBLIC_IPA" ] # -n If NOT NULL/EMPTY
    then 
        echo "Creating API Host record"
        echo "Using PIP:" $APPLICATION_FW_PUBLIC_IPA

        result=$(az network dns record-set a add-record --subscription $CSSA_SHD_SUBSCRIPTION_ID -g $CSSA_RESOURCE_GROUP_NAME -z $CSSA_DNS_ROOT_ZONE -n $API_APPLICATION_NAME -a "$APPLICATION_FW_PUBLIC_IPA" 2>&1)
        if [ $OUTPUT_LEVEL == 'DEBUG' ]; then echo "$result"; fi;
        echo "Created "$dns_sub_domain_name"-api record"
    fi
    echo

    echo "Sleeping $sleep_time seconds"
    echo
    sleep $sleep_time
fi

echo "Creating CDN Profile" $CSSA_CDN_PROFILE_NAME

profileResult=$(az cdn profile create --subscription $CSSA_SHD_SUBSCRIPTION_ID -g $CSSA_CDN_RESOURCE_GROUP_NAME -n $CSSA_CDN_PROFILE_NAME -l Global --sku Standard_Microsoft 2>&1)
if [ $OUTPUT_LEVEL == 'DEBUG' ]; then echo "$profileResult"; fi;
echo "Created:" $CSSA_CDN_PROFILE_NAME
echo

echo "Sleeping $sleep_time seconds"
sleep $sleep_time

echo "Creating CDN Endpoint" $CSSA_CDN_ENDPOINT_NAME

 # Force these values to lowercase
ori=${AGENCY_ORI,,}
agency=${AGENCY_ABBREVIATION,,}

# echo "az cdn endpoint create --subscription $CSSA_SHD_SUBSCRIPTION_ID --resource-group $CSSA_CDN_RESOURCE_GROUP_NAME --profile-name $CSSA_CDN_PROFILE_NAME --name $CSSA_CDN_ENDPOINT_NAME --location Global --origin $WEB_CONTENT_URL --origin-host-header $WEB_CONTENT_URL --enable-compression true --no-http false --no-https false"
endpointResult=$(
    az cdn endpoint create \
        --subscription $CSSA_SHD_SUBSCRIPTION_ID \
        --resource-group $CSSA_CDN_RESOURCE_GROUP_NAME \
        --profile-name $CSSA_CDN_PROFILE_NAME \
        --name $CSSA_CDN_ENDPOINT_NAME \
        --location Global \
        --origin $WEB_CONTENT_URL \
        --origin-host-header $WEB_CONTENT_URL \
        --enable-compression true \
        --no-http false \
        --no-https false \
        --tags ori=$ori agency=$agency application=ccw 2>&1)
if [ $OUTPUT_LEVEL == 'DEBUG' ]; then echo "$endpointResult"; fi;
echo "Created:" $CSSA_CDN_ENDPOINT_NAME
echo

echo "Sleeping $sleep_time seconds"
sleep $sleep_time

echo "Setting Geo filters"
geoFilterResult=$(
    az cdn endpoint update \
        --subscription $CSSA_SHD_SUBSCRIPTION_ID \
        --resource-group $CSSA_CDN_RESOURCE_GROUP_NAME \
        --profile-name $CSSA_CDN_PROFILE_NAME \
        --name $CSSA_CDN_ENDPOINT_NAME \
        --set geoFilters="[{\"relativePath\":\"/\",\"action\":\"Allow\",\"countryCodes\":[\"US\"]}]" 2>&1)
if [ $OUTPUT_LEVEL == 'DEBUG' ]; then echo "$geoFilterResult"; fi;
echo "Configured Geo filters:" $CSSA_CDN_ENDPOINT_NAME
echo

echo "Sleeping $sleep_time seconds"
sleep $sleep_time

echo "Creating custom domain" $dns_host_name
customDomainResult=$(
    az cdn custom-domain create \
        --resource-group $CSSA_CDN_RESOURCE_GROUP_NAME \
        --profile-name $CSSA_CDN_PROFILE_NAME \
        --endpoint-name $CSSA_CDN_ENDPOINT_NAME \
        --hostname $dns_host_name \
        --name $dns_hostname_name 2>&1)
if [ $OUTPUT_LEVEL == 'DEBUG' ]; then echo "$customDomainResult"; fi;
echo "Created:" $dns_host_name
echo

echo "Sleeping $sleep_time seconds"
sleep $sleep_time

echo "Enabling HTTPS on custom domain" $dns_host_name
enableHttpsResult=$(
    az cdn custom-domain enable-https \
        --resource-group $CSSA_CDN_RESOURCE_GROUP_NAME \
        --profile-name $CSSA_CDN_PROFILE_NAME \
        --endpoint-name $CSSA_CDN_ENDPOINT_NAME \
        --name $dns_hostname_name \
        --min-tls-version 1.2 \
        --user-cert-subscription-id $CSSA_SHD_SUBSCRIPTION_ID \
        --user-cert-group-name $CSSA_RESOURCE_GROUP_NAME \
        --user-cert-vault-name $CSSA_CERT_KEY_VAULT_NAME \
        --user-cert-secret-name $CSSA_CERT_SECRET_NAME \
        --user-cert-protocol-type 'sni' 2>&1)
if [ $OUTPUT_LEVEL == 'DEBUG' ]; then echo "$enableHttpsResult"; fi;
echo "Enabled Https:" $dns_host_name
echo

echo "Sleeping $sleep_time seconds"
sleep $sleep_time

rulesFile="*-$CSSA_CDN_ENDPOINT_TYPE-rules.json"
echo "Using rules file:" "$rulesFile"

rulesFileContent="$(cat $rulesFile)"
echo "$rulesFileContent"

echo "Setting rules"
rulesResult=$(
    az cdn endpoint update \
        --subscription $CSSA_SHD_SUBSCRIPTION_ID \
        --resource-group $CSSA_CDN_RESOURCE_GROUP_NAME \
        --profile-name $CSSA_CDN_PROFILE_NAME \
        --name $CSSA_CDN_ENDPOINT_NAME \
        --set deliveryPolicy.rules="$rulesFileContent" 2>&1)
if [ $OUTPUT_LEVEL == 'DEBUG' ]; then echo "$enableHttpsResult"; fi;
echo "Created rules:" $CSSA_CDN_ENDPOINT_NAME
echo

echo "Finished DNS/Custom Host/Certificate deployment"