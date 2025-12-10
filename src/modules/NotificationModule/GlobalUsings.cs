global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Security.Cryptography;
global using System.Text;
global using System.Data;
global using System.Net;
global using System.Reflection;

global using Microsoft.EntityFrameworkCore.Storage;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Options;
global using Microsoft.Extensions.Logging;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Routing;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Configuration;
global using Microsoft.CodeAnalysis;


global using Asp.Versioning;
global using Carter;
global using Carter.OpenApi;
global using MediatR;
global using Swashbuckle.AspNetCore.SwaggerGen;
global using Swashbuckle.AspNetCore.SwaggerUI;

global using Infrastructure.DI;
global using Infrastructure.Extentions;
global using Infrastructure.Hangfier;
global using Infrastructure.MediatR;
global using Infrastructure.Messaging.Extentions;
global using Infrastructure.Contracts.CQRS;
global using Infrastructure.Exceptions;
global using Infrastructure.Data;
global using Infrastructure.Pagination;
global using Infrastructure.Data.Repository;
global using Infrastructure.Web.ApiResult;

global using NotificationModule.Data.Context;
global using NotificationModule.Repositories;
global using NotificationModule.Entities;
global using NotificationModule.Contract.Configs;
global using NotificationModule.Contract.Enumerations;
global using NotificationModule.Contract.Requests;
global using NotificationModule.Contract.Responses;
global using NotificationModule.Contract.Commands;
global using NotificationModule.Contract.Models;
