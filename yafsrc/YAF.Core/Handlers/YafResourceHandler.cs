﻿/* Yet Another Forum.NET
 * Copyright (C) 2003-2005 Bjørnar Henden
 * Copyright (C) 2006-2013 Jaben Cargman
 * Copyright (C) 2014-2019 Ingo Herbote
 * http://www.yetanotherforum.net/
 * 
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at

 * http://www.apache.org/licenses/LICENSE-2.0

 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

namespace YAF.Core.Handlers
{
    #region Using

    using System;
    using System.Net;
    using System.Web;
    using System.Web.SessionState;
    using YAF.Configuration;
    using YAF.Core;
    using YAF.Types;
    using YAF.Types.Extensions;
    using YAF.Types.Interfaces;

    #endregion

    /// <summary>
    /// YAF Resource Handler for all kind of Stuff (Avatars, Attachments, Albums, etc.)
    /// </summary>
    public class YafResourceHandler : IHttpHandler, IReadOnlySessionState, IHaveServiceLocator
    {
        #region Properties

        /// <summary>
        ///   Gets a value indicating whether IsReusable.
        /// </summary>
        public bool IsReusable => false;

        /// <summary>
        /// Gets ServiceLocator.
        /// </summary>
        public IServiceLocator ServiceLocator => YafContext.Current.ServiceLocator;

        #endregion

        #region Implemented Interfaces

        #region IHttpHandler

        /// <summary>
        /// The process request.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public void ProcessRequest([NotNull] HttpContext context)
        {
            if (context.Session["lastvisit"] != null
                     ||
                     context.Request.UrlReferrer != null
                     && context.Request.UrlReferrer.AbsoluteUri.Contains(BaseUrlBuilder.BaseUrl))
            {
                // defaults
                var previewCropped = false;
                var localizationFile = "english.xml";

                if (context.Session["imagePreviewCropped"] is bool)
                {
                    previewCropped = context.Session["imagePreviewCropped"].ToType<bool>();
                }

                if (context.Session["localizationFile"] is string)
                {
                    localizationFile = context.Session["localizationFile"].ToString();
                }

                if (context.Session["localizationFile"] is string)
                {
                    localizationFile = context.Session["localizationFile"].ToString();
                }

                /////////////
                if (context.Request.QueryString.GetFirstOrDefault("twitterinfo") != null)
                {
                    this.Get<IResources>().GetTwitterUserInfo(context);
                }
                else if (context.Request.QueryString.GetFirstOrDefault("userinfo") != null)
                {
                    this.Get<IResources>().GetUserInfo(context);
                }
                else if (context.Request.QueryString.GetFirstOrDefault("bbcodelist") != null)
                {
                    this.Get<IResources>().GetCustomBBCodes(context);
                }
                else if (context.Request.QueryString.GetFirstOrDefault("users") != null)
                {
                    this.Get<IResources>().GetMentionUsers(context);
                }
                else if (context.Request.QueryString.GetFirstOrDefault("u") != null)
                {
                    this.Get<IResources>().GetResponseLocalAvatar(context);
                }
                else if (context.Request.QueryString.GetFirstOrDefault("url") != null
                         && context.Request.QueryString.GetFirstOrDefault("width") != null
                         && context.Request.QueryString.GetFirstOrDefault("height") != null)
                {
                    var maxWidth = int.Parse(context.Request.QueryString.GetFirstOrDefault("width"));
                    var maxHeight = int.Parse(context.Request.QueryString.GetFirstOrDefault("height"));

                    var etagCodeCode =
                        $@"""{(context.Request.QueryString.GetFirstOrDefault("url") + maxHeight + maxWidth).GetHashCode()}""";

                    if (!CheckETagCode(context, etagCodeCode))
                    {
                        this.Get<IResources>().GetResponseRemoteAvatar(context);
                    }
                }
                else if (context.Request.QueryString.GetFirstOrDefault("a") != null)
                {
                    this.Get<IAttachment>().GetResponseAttachment(context);
                }
                else if (context.Request.QueryString.GetFirstOrDefault("i") != null)
                {
                    var etagCodeCode = $@"""{context.Request.QueryString.GetFirstOrDefault("i")}""";

                    if (!CheckETagCode(context, etagCodeCode))
                    {
                        this.Get<IAttachment>().GetResponseImage(context);
                    }
                }
                else if (context.Request.QueryString.GetFirstOrDefault("p") != null)
                {
                    var etagCodeCode = $@"""{context.Request.QueryString.GetFirstOrDefault("p")}{localizationFile.GetHashCode()}""";

                    if (!CheckETagCode(context, etagCodeCode))
                    {
                        this.Get<IAlbum>().GetResponseImagePreview(context, localizationFile, previewCropped);
                    }
                }
                else if (context.Request.QueryString.GetFirstOrDefault("c") != null)
                {
                    // captcha
                    this.Get<IResources>().GetResponseCaptcha(context);
                }
                else if (context.Request.QueryString.GetFirstOrDefault("cover") != null
                         && context.Request.QueryString.GetFirstOrDefault("album") != null)
                {
                    var etagCode = $@"""{context.Request.QueryString.GetFirstOrDefault("cover")}{localizationFile.GetHashCode()}""";

                    if (!CheckETagCode(context, etagCode))
                    {
                        // album cover
                        this.Get<IAlbum>().GetAlbumCover(context, localizationFile, previewCropped);
                    }
                }
                else if (context.Request.QueryString.GetFirstOrDefault("imgprv") != null)
                {
                    // album image preview
                    var etagCode =
                        $@"""{context.Request.QueryString.GetFirstOrDefault("imgprv")}{localizationFile.GetHashCode()}""";

                    if (!CheckETagCode(context, etagCode))
                    {
                        this.Get<IAlbum>().GetAlbumImagePreview(context, localizationFile, previewCropped);
                    }
                }
                else if (context.Request.QueryString.GetFirstOrDefault("image") != null)
                {
                    var etagCode = $@"""{context.Request.QueryString.GetFirstOrDefault("image")}""";

                    if (!CheckETagCode(context, etagCode))
                    {
                        // album image
                        this.Get<IAlbum>().GetAlbumImage(context);
                    }
                }
            }
            else
            {
                // they don't have a session...
                context.Response.Write(
                    "Please do not link directly to this resource. You must have a session in the forum.");
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Check if the eTag Code that sent from the client is match to the current eTag Code.
        ///   If so, set the status code to 'Not Modified' and stop the response.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="etagCode">
        /// The eTag Code.
        /// </param>
        /// <returns>
        /// The check eTag code.
        /// </returns>
        private static bool CheckETagCode([NotNull] HttpContext context, [NotNull] string etagCode)
        {
            var ifNoneMatch = context.Request.Headers["If-None-Match"];

            if (!etagCode.Equals(ifNoneMatch, StringComparison.Ordinal))
            {
                return false;
            }

            if (context.Request.QueryString.GetFirstOrDefault("v") != null)
            {
                return false;
            }

            context.Response.AppendHeader("Content-Length", "0");
            context.Response.StatusCode = HttpStatusCode.NotModified.ToType<int>();
            context.Response.StatusDescription = "Not modified";
            context.Response.SuppressContent = true;
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetETag(etagCode);
            context.Response.Flush();

            return true;
        }

        #endregion
    }
}