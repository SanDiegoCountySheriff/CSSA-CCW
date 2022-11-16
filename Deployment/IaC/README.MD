CSSA-CCW IaC

## Temp Notes

### Bicep

Idea is to use bicep modules, called from a `main.bicep` file.  
To generate the ARM template, run `bicep build main.bicep` from the folder containing `main.bicep`
The thought is to create reusable components that are easier to maintain vs. one large ARM template.

[What is Bicep](https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/overview?tabs=bicep)
[Parameters in bicep](https://ochzhen.com/blog/azure-bicep-parameters)
[Bicep CosmosDB example](https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/manage-with-bicep)
[Bicep modules](https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/modules)
