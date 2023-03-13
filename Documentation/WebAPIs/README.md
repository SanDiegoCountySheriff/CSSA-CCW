# CSSA CCW Web API

## Web API Project Structure

Each API project has same structure except for Clients folder found in Application and Document APIs. These projects have dependencies on some of the other projects and the Clients folder holds those client classes.

### Clients Folder

Some APIs will contain a service folder because the API projects exchange information using HttpClients. To use another API endpoint an HttpClient will need to be configured along with passing the JWT token.

How to add a new (httpclient) service client:

**Step 1:** Add NuGet package: Microsoft.AspNetCore.HeaderPropagation

**Step 2:** In Program.cs file add HTTP Header Propogation

<code>

    builder.Services.AddHeaderPropagation(o =>
    {
        o.Headers.Add("Authorization");
    });

    builder.Services.AddHttpClient<IApiNameHereServiceClient, ApiNameHereServiceClient>("ApiNameHereHttpClient", c =>
    {
        c.BaseAddress = new Uri(builder.Configuration.GetSection("ApiNameHereApi:BaseUrl").Value);
        c.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(builder.Configuration.GetSection("ApiNameHereApi:Timeout").Value));
        c.DefaultRequestHeaders.Add("Accept", "application/json");
    }).AddHeaderPropagation();
    
    Note: AddHeaderPropagation adds the JWT token to the HttpClient behind the scenes so we don't need to do it every time.
</code>

**Step 3:** Add the interface and implementation class to the service folder. If one doesn’t exist, create the Service folder. (Highlighted part above has to correspond to the interface and implementation class names)

### Controllers Folder

This folder should hold all Web API Controllers. It is best to create separate controllers as needed to align functionality.

Contains API controller with all the endpoints. Each endpoint is defined in the endpoint action. Because this system has two types of users public/private (B2C and AAD) some endpoints will be duplicated.

However, B2C user methods will extract the user id from the JWT token and use it to perform the request.

While AAD users’ endpoints will request the 'applicationId' or 'userId' in order to perform the request. Moreover, if the AAD user methods is to update or delete, the name of the AAD user will be extracted from the JWT token in order to maintain historic record of who perform the request.

For JWT implementation and authorization please refer to the Program.cs file. Default authentication is set to AAD. That is, first runs to check if user is an admin user if not checks if it is a B2C user.

### Entities Folder

This folder shold hold the database models (entities) the API is responsible for.

Represents all objects/classes found in Cosmos Db.

    **Note: If an existing entity is modified by adding or deleting a property corresponding mappers will also need to be updated.**

### Enums Folder

This folder should hold all Enums required by the API.

### Mappers Folder

This folder should hold all mapper classses needed by the API. These API use custom mappers to ensure the data is serialized or deserialized based on request/response needs.

Contains all the mappings between the request/response models and database entities.

IMapper.cs is the class that represents the interface for the mapping between models and entities and entities and models.

How to modify:

- Add another interface with corresponding number of needed parameters if not implemented, that is: T1, T2, T3, T4, and so on and always out TDestinationType.
- Create your new mapper; naming should reflect the objects mapping from to and add the corresponding interface. And add mapper in Program.cs file.

Example: A mapper that takes 3 source types.

**Step 1:** Create the mapper interface:
<code>

    public interface IMapper<in TSourceType1, in TSourceType2, in TSourceType3, out TDestinationType>
    {
        TDestinationType Map(TSourceType1 source1, TSourceType2 source2, TSourceType3 source3);
    }
</code>

**Step 2:** Create the mapper implementation

<code>

    public class FromObjectAObjectBMapper : IMapper<string, bool, ObjectARequestModel, ObjectB>
    {
        …// TODO: Implement the mapping code
    }
</code>

**Step 3:** Add dependency injection in Program.cs file

<code>

    builder.Services.AddSingleton<IMapper<string, bool, ObjectARequestModel>, FromObjectAObjectBMapper >();
</code>

### Models Folder

This folder should hold any request/response models required/used by the UI. Request/response models should be used to isolate the underlying database models from the UI requirements.

Contains all the objects that represent the request and/or response models for the API endpoint(s).

    **Note:** A controller and the Cosmos Db Service should only use the objects found in the models folder. Do not create entities/model outside the Models folder.

### Services Folder

The Services folder should hold any "services" the API needs like database or storage.

CosmosDbService Class for Example

- Contains all functions that allow CRUD & other required methods to be performed on the database.
- Database name and container is found the appsettings.Development.json (for local) and under App Service Configuration in Azure.
- How to add a new method modify:
    1. Add a new method to the interface: ICosmosDbService.cs
    2. Implement in the CosmosDbServiceClass

## Managing Application Configuraton Settings

When developing new functionality it is often needed to add configuration data. It is important to remember that these settings need to be configured for local, dev, & Azure Marketplace deployments.

How to add a new settings:

1. Update relevant appsettings.Development.json section with the new setting for local development. Run and test your changes locally.
    - **<span style="color:red">DO NOT check-in sensitive or secret settings!**</span>. Use local secrets: [Visual Studio - Managing local secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows)
2. Navigate to Azure App Service corresponding to API service you are looking to modify, go to Configuration -> Application settings and enter new setting.
    - Use double underscore "parent__child" to define nested settings in a Linux environment.
    - With Windows hosting you would use a colon "parent:child".
    - Keep in mind that CCW implments Linux App Services.
3. These settings should be configured in the [IaC](../../Deployment/IaC) as well so that they get deployed with the system.

### App Settings Example

|![Azure App Service Application Settings](../images/CSSA-CCW-WebAPI-App-Settings.png)|
|---|

|![Azure App Service Application Settings](../images/CSSA-CCW-WebAPI-App-Settings-DU.png)|
|---|

## Azure Storage for system and client documents

There are three containers. One contains all files pertaining to the system & agency, one holds client related documents, and the third holds document related to the agencies license processors & system admins.

- Agency container: ccw-agency-documents
- Public container: ccw-public-documents
- Processor container: ccw-admin-user-documents

*Note: Document API has endpoints for each container to upload, download and delete files. Because Document API connects to Azure Storage under the Services folder Cosmos DB service file will not be found instead will be an Azure Storage client.

## Unit Testing

Each API has unit tests for the corresponding feature. All Unit Tests use the Moq framework.

Dependencies Moq framework uses:

- AutoFixture.AutoMoq
- AutoFixture.NUnit3

Here are a few links for learning moq framework:

https://learn.microsoft.com/en-us/shows/visual-studio-toolbox/unit-testing-moq-framework

https://methodpoet.com/unit-testing-with-moq/
