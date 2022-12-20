﻿using Azure.Core;
using Microsoft.Azure.Cosmos;
using System.Net;
using CCW.UserProfile.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Container = Microsoft.Azure.Cosmos.Container;
using User = CCW.UserProfile.Entities.User;


namespace CCW.UserProfile.Services;

public class CosmosDbService : ICosmosDbService
{
    private Container _container;

    public CosmosDbService(
        CosmosClient cosmosDbClient,
        string databaseName,
        string containerName)
    {
        _container = cosmosDbClient.GetContainer(databaseName, containerName);
    }

    public async Task<User?> AddAsync(User user, CancellationToken cancellationToken)
    {
        var existingUser = await GetAsync(user.Id, default);

        if (existingUser != null)
        {
            if (existingUser.UserEmail == user.UserEmail)
            {
                throw new ArgumentException("Email address already exists.");
            }

            foreach (var email in existingUser.PreviousEmails)
            {
                if (email.EmailAddress == user.UserEmail)
                {
                    throw new ArgumentException("Email address used in past.");
                }
            }

            user.PreviousEmails = existingUser.PreviousEmails;
            user.PreviousEmails.Append(new Email
            {
                EmailAddress = existingUser.UserEmail,
                CreateDateTimeUtc = existingUser.ProfileUpdateDateTimeUtc,
            });
    
            var updatedUser = await _container.PatchItemAsync<User>(
                user.Id,
                new PartitionKey(user.Id),
                new[]
                {
                    PatchOperation.Set("/UserEmail", user.UserEmail),
                    PatchOperation.Set("/PreviousEmails", user.PreviousEmails),
                    PatchOperation.Set("/ProfileUpdateDateTimeUtc", DateTime.Now.ToUniversalTime()),
                },
                null,
                cancellationToken
            );

            return updatedUser;
        }

        user.PreviousEmails = Array.Empty<Email>();
        user.UserCreateDateTimeUtc = DateTime.Now.ToUniversalTime();
        user.ProfileUpdateDateTimeUtc = DateTime.Now.ToUniversalTime();

        User createdItem = await _container.CreateItemAsync(user, new PartitionKey(user.Id), null, cancellationToken);
        return createdItem;
    }

    public async Task<User?> GetAsync(string userId, CancellationToken cancellationToken)
    {
        try
        {
            var queryString = "SELECT * FROM users p WHERE p.id = @userId";

            var parameterizedQuery = new QueryDefinition(query: queryString)
                .WithParameter("@userId", userId);


            using FeedIterator<User> filteredFeed = _container.GetItemQueryIterator<User>(
                queryDefinition: parameterizedQuery,
                requestOptions: new QueryRequestOptions() { PartitionKey = new PartitionKey(userId) }
            );

            if (filteredFeed.HasMoreResults)
            {
                FeedResponse<User> response = await filteredFeed.ReadNextAsync(cancellationToken);

                return response.Resource.FirstOrDefault();
            }

            return null!;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null!;
        }
    }
}
