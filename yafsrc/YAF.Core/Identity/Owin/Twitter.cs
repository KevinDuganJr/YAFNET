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
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANYc
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

namespace YAF.Core.Identity.Owin;

using System;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using YAF.Core.Model;
using YAF.Types.Attributes;
using YAF.Types.Models;

/// <summary>
/// Twitter Single Sign On Class
/// </summary>
public class Twitter : IAuthBase, IHaveServiceLocator
{
    /// <summary>
    /// Gets or sets ServiceLocator.
    /// </summary>
    public IServiceLocator ServiceLocator => BoardContext.Current.ServiceLocator;

    /// <summary>
    /// Logins the or create user.
    /// </summary>
    /// <returns>
    /// The <see cref="AspNetUsers"/>.
    /// </returns>
    public async Task<(string Message, AspNetUsers User)> LoginOrCreateUserAsync()
    {
        if (!this.Get<BoardSettings>().AllowSingleSignOn)
        {
            return (this.Get<ILocalization>().GetText("LOGIN", "SSO_DEACTIVATED"), null);
        }

        var loginInfo = await this.Get<SignInManager<AspNetUsers>>().GetExternalLoginInfoAsync();

        if (loginInfo == null)
        {
            return (this.Get<ILocalization>().GetText("LOGIN", "SSO_TWITTER_FAILED3"), null);
        }

        // Get Values
        var name = loginInfo.Principal.FindFirst(ClaimTypes.Name).Value;
        var email = $"{name}@twitter.com";
        var twitterUserId = loginInfo.Principal.FindFirst(ClaimTypes.NameIdentifier).Value;

        // Check if user exists
        var existingUser = await this.Get<IAspNetUsersHelper>().GetUserByNameAsync(name);

        if (existingUser == null)
        {
            // Create new PageUser
            return await this.CreateTwitterUserAsync(name, email, twitterUserId);
        }

        if (existingUser.Profile_TwitterId == twitterUserId)
        {
            return (string.Empty, existingUser);
        }

        return (this.Get<ILocalization>().GetText("LOGIN", "SSO_TWITTER_FAILED3"), null);
    }

    /// <summary>
    /// Creates the twitter user
    /// </summary>
    /// <param name="name">
    /// The name.
    /// </param>
    /// <param name="email">
    /// The email.
    /// </param>
    /// <param name="twitterUserId">
    /// The twitter PageUser Id.
    /// </param>
    /// <returns>
    /// Returns if the login was successfully or not
    /// </returns>
    private async Task<(string Message, AspNetUsers User)> CreateTwitterUserAsync(string name, string email, string twitterUserId)
    {
        if (this.Get<BoardSettings>().DisableRegistrations)
        {
            return (this.Get<ILocalization>().GetText("LOGIN", "SSO_FAILED"), null);
        }

        // Check if user name is null
        var userName = name;
        var displayName = userName;

        userName = displayName.Replace(" ", ".");

        var pass = PasswordGenerator.GeneratePassword(true, true, true, true, false, 16);

        var user = new AspNetUsers
                       {
                           Id = Guid.NewGuid().ToString(),
                           ApplicationId = this.Get<BoardSettings>().ApplicationId,
                           UserName = userName,
                           LoweredUserName = userName.ToLower(),
                           Email = email,
                           IsApproved = true,
                           EmailConfirmed = true,
                           Profile_RealName = name,
                           Profile_Twitter = userName,
                           Profile_TwitterId = twitterUserId,
                       };

        var result = await this.Get<IAspNetUsersHelper>().CreateUserAsync(user, pass);

        if (!result.Succeeded)
        {
            // error of some kind
            return (result.Errors.FirstOrDefault()?.Description, null);
        }

        // setup initial roles (if any) for this user
        await this.Get<IAspNetRolesHelper>().SetupUserRolesAsync(BoardContext.Current.PageBoardID, user);

        // create the user in the YAF DB as well as sync roles...
        var userId = await this.Get<IAspNetRolesHelper>().CreateForumUserAsync(user, displayName, BoardContext.Current.PageBoardID);

        if (userId == null)
        {
            // something is seriously wrong here -- redirect to failure...
            return (this.Get<ILocalization>().GetText("LOGIN", "SSO_FAILED"), null);
        }

        // send user register notification to the user...
        this.SendRegistrationMessageToTwitterUser(user, pass, userId.Value);

        if (this.Get<BoardSettings>().NotificationOnUserRegisterEmailList.IsSet())
        {
            // send user register notification to the following admin users...
            await this.Get<ISendNotification>().SendRegistrationNotificationEmailAsync(user, userId.Value);
        }

        var autoWatchTopicsEnabled = this.Get<BoardSettings>().DefaultNotificationSetting
                                     == UserNotificationSetting.TopicsIPostToOrSubscribeTo;

        // save the settings...
        this.GetRepository<User>().SaveNotification(
            userId.Value,
            autoWatchTopicsEnabled,
            this.Get<BoardSettings>().DefaultNotificationSetting.ToInt(),
            this.Get<BoardSettings>().DefaultSendDigestEmail);

        this.Get<IRaiseEvent>().Raise(new NewUserRegisteredEvent(user, userId.Value));

        return (string.Empty, user);
    }

    /// <summary>
    /// Send an Private Message to the Newly Created PageUser with
    /// his Account Info (Pass, Security Question and Answer)
    /// </summary>
    /// <param name="user">
    /// The user.
    /// </param>
    /// <param name="pass">
    /// The pass.
    /// </param>
    /// <param name="userId">
    /// The user Id.
    /// </param>
    private void SendRegistrationMessageToTwitterUser(
        [NotNull] AspNetUsers user,
        [NotNull] string pass,
        [NotNull] int userId)
    {
        var subject = string.Format(
            BoardContext.Current.Get<ILocalization>().GetText("COMMON", "NOTIFICATION_ON_NEW_FACEBOOK_USER_SUBJECT"),
            BoardContext.Current.BoardSettings.Name);

        var notifyUser = new TemplateEmail("NOTIFICATION_ON_TWITTER_REGISTER")
                             {
                                 TemplateParams =
                                     {
                                         ["{user}"] = user.UserName,
                                         ["{email}"] = user.Email,
                                         ["{pass}"] = pass,
                                         ["{forumname}"] = BoardContext.Current.BoardSettings.Name
                                     }
                             };

        var emailBody = notifyUser.ProcessTemplate("NOTIFICATION_ON_TWITTER_REGISTER");

        var hostUser = this.GetRepository<User>()
            .Get(u => u.BoardID == BoardContext.Current.PageBoardID && (u.Flags & 1) == 1)
            .FirstOrDefault();

        // Send Message also as DM to Twitter.
        this.GetRepository<PrivateMessage>().Insert(
            new PrivateMessage
                {
                    Created = DateTime.UtcNow,
                    Flags = 0,
                    FromUserId = hostUser.ID,
                    ToUserId = userId,
                    Body = emailBody
                });
    }
}