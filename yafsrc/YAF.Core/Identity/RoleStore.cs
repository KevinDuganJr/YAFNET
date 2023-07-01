﻿/* Yet Another Forum.NET
 * Copyright (C) 2003-2005 Bjørnar Henden
 * Copyright (C) 2006-2013 Jaben Cargman
 * Copyright (C) 2014-2023 Ingo Herbote
 * https://www.yetanotherforum.net/
 *
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at

 * https://www.apache.org/licenses/LICENSE-2.0

 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

namespace YAF.Core.Identity;

using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using YAF.Types.Attributes;

/// <summary>
/// The role store.
/// </summary>
public class RoleStore : IQueryableRoleStore<AspNetRoles>,
                         IHaveServiceLocator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RoleStore"/> class.
    /// </summary>
    /// <param name="serviceLocator">
    /// The service locator.
    /// </param>
    public RoleStore([NotNull] IServiceLocator serviceLocator)
    {
        this.ServiceLocator = serviceLocator;
    }

    /// <summary>
    /// Gets the service locator.
    /// </summary>
    public IServiceLocator ServiceLocator { get; }

    /// <summary>
    /// The roles.
    /// </summary>
    public virtual IQueryable<AspNetRoles> Roles => this.GetRepository<AspNetRoles>().GetAll().AsQueryable();

    /// <summary>
    /// The create async.
    /// </summary>
    /// <param name="role">
    /// The role.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    public async Task<IdentityResult> CreateAsync([NotNull]AspNetRoles role, CancellationToken cancellationToken)
    {
        CodeContracts.VerifyNotNull(role);

        cancellationToken.ThrowIfCancellationRequested();

        await this.GetRepository<AspNetRoles>().InsertAsync(role, false, token: cancellationToken);

        return IdentityResult.Success;
    }

    /// <summary>
    /// The update async.
    /// </summary>
    /// <param name="role">
    /// The role.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    public async Task<IdentityResult> UpdateAsync([NotNull] AspNetRoles role, CancellationToken cancellationToken)
    {
        CodeContracts.VerifyNotNull(role);

        cancellationToken.ThrowIfCancellationRequested();

        return await this.GetRepository<AspNetRoles>().UpdateAsync(role, cancellationToken) > 0
                   ? IdentityResult.Success
                   : IdentityResult.Failed();
    }

    /// <summary>
    /// Deletes a role from the store as an asynchronous operation.
    /// </summary>
    /// <param name="role">The role to delete from the store.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> of the asynchronous query.</returns>
    public async Task<IdentityResult> DeleteAsync([NotNull]AspNetRoles role, CancellationToken cancellationToken)
    {
        CodeContracts.VerifyNotNull(role);

        cancellationToken.ThrowIfCancellationRequested();

        return await this.GetRepository<AspNetRoles>().DeleteAsync(r => r.Id == role.Id, cancellationToken) > 0
                   ? IdentityResult.Success
                   : IdentityResult.Failed();
    }

    /// <summary>
    /// The find by id async.
    /// </summary>
    /// <param name="roleId">
    /// The role id.
    /// </param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    public Task<AspNetRoles> FindByIdAsync([NotNull]string roleId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return this.GetRepository<AspNetRoles>().GetSingleAsync(r => r.Id == roleId, cancellationToken);
    }

    /// <summary>
    /// The find by name async.
    /// </summary>
    /// <param name="roleName">
    /// The role name.
    /// </param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    public Task<AspNetRoles> FindByNameAsync([NotNull]string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return this.GetRepository<AspNetRoles>().GetSingleAsync(r => r.Name == roleName, cancellationToken);
    }

    public Task<string> GetRoleIdAsync(AspNetRoles role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.Id);
    }

    public Task<string> GetRoleNameAsync(AspNetRoles role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.Name);
    }

    public Task SetRoleNameAsync(AspNetRoles role, string roleName, CancellationToken cancellationToken)
    {
        role.Name = roleName;
        return Task.FromResult(0);
    }

    public Task<string> GetNormalizedRoleNameAsync(AspNetRoles role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.Name);
    }

    public Task SetNormalizedRoleNameAsync(AspNetRoles role, string normalizedName, CancellationToken cancellationToken)
    {
        role.Name = normalizedName;
        return Task.FromResult(0);
    }

    /// <summary>
    /// The dispose.
    /// </summary>
    public virtual void Dispose()
    {
        // No resource to dispose for now!
    }
}