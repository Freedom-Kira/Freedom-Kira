using api_snake_case.Extensions;
using api_snake_case.Helpers;
using api_snake_case.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// POST recebendo low snake_case via BindAsync
app.MapPost("/products", (ProductRequest bodyrequest) =>
{

    if (bodyrequest == null)
        return Results.BadRequest("Invalid or non-snake_case JSON");

    return Results.Ok(JsonSerializationExtensionsText.ToJson(bodyrequest));
})
.AddEndpointFilter<ValidarBodyFilter<ProductRequest>>();

app.Run();