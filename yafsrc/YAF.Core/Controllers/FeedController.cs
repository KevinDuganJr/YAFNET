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

namespace YAF.Core.Controllers;

using System;
using System.IO;
using System.ServiceModel.Syndication;
using System.Xml;

using YAF.Core.BasePages;

/// <summary>
/// The Feed controller.
/// </summary>
[Route("api/[controller]")]
public class Feed : ForumBaseController
{
    /// <summary>
    /// Gets the latest posts feed.
    /// </summary>
    /// <returns>ActionResult.</returns>
    [Produces("application/rss+xml")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("GetLatestPosts")]
    public ActionResult GetLatestPosts()
    {
        if (!this.Get<BoardSettings>().ShowAtomLink)
        {
            return this.NotFound();
        }

        if (!(this.Get<BoardSettings>().ShowActiveDiscussions && this.Get<IPermissions>()
                  .Check(this.Get<BoardSettings>().PostLatestFeedAccess)))
        {
            return this.NotFound();
        }

        var lastPostName = this.GetText("DEFAULT", "GO_LAST_POST");

        try
        {
            var feed = this.Get<SyndicationFeeds>().GetPostLatestFeed(lastPostName);

            var settings = new XmlWriterSettings {Encoding = Encoding.UTF8};

            using var stream = new MemoryStream();
            using (var xmlWriter = XmlWriter.Create(stream, settings))
            {
                var formatter = new Atom10FeedFormatter(feed);
                formatter.WriteTo(xmlWriter);
                xmlWriter.Flush();
            }

            return this.File(stream.ToArray(), "application/rss+xml; charset=utf-8");
        }
        catch (Exception)
        {
            return this.NotFound();
        }
    }

    /// <summary>
    /// Gets the topics feed.
    /// </summary>
    /// <param name="f">The forum identifier.</param>
    /// <returns>ActionResult.</returns>
    [Produces("application/rss+xml")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("GetTopicsFeed")]
    public ActionResult GetTopicsFeed(int f)
    {
        if (!this.Get<BoardSettings>().ShowAtomLink)
        {
            return this.NotFound();
        }

        if (!(this.PageBoardContext.ForumReadAccess && this.Get<IPermissions>()
                  .Check(this.Get<BoardSettings>().TopicsFeedAccess)))
        {
            return this.NotFound();
        }

        var lastPostName = this.GetText("DEFAULT", "GO_LAST_POST");

        try
        {
            var feed = this.Get<SyndicationFeeds>().GetTopicsFeed(lastPostName, f);

            var settings = new XmlWriterSettings { Encoding = Encoding.UTF8 };

            using var stream = new MemoryStream();
            using (var xmlWriter = XmlWriter.Create(stream, settings))
            {
                var formatter = new Atom10FeedFormatter(feed);
                formatter.WriteTo(xmlWriter);
                xmlWriter.Flush();
            }

            return this.File(stream.ToArray(), "application/rss+xml; charset=utf-8");
        }
        catch (Exception)
        {
            return this.NotFound();
        }
    }

    /// <summary>
    /// Gets the posts feed.
    /// </summary>
    /// <param name="t">The topic identifier.</param>
    /// <returns>ActionResult.</returns>
    [Produces("application/rss+xml")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("GetPostsFeed")]
    public ActionResult GetPostsFeed(int t)
    {
        if (!this.Get<BoardSettings>().ShowAtomLink)
        {
            return this.NotFound();
        }

        if (!(this.PageBoardContext.ForumReadAccess && this.Get<IPermissions>()
                  .Check(this.Get<BoardSettings>().PostsFeedAccess)))
        {
            return this.NotFound();
        }

        try
        {
            var feed = this.Get<SyndicationFeeds>().GetPostsFeed(t);

            var settings = new XmlWriterSettings { Encoding = Encoding.UTF8 };

            using var stream = new MemoryStream();
            using (var xmlWriter = XmlWriter.Create(stream, settings))
            {
                var formatter = new Atom10FeedFormatter(feed);
                formatter.WriteTo(xmlWriter);
                xmlWriter.Flush();
            }

            return this.File(stream.ToArray(), "application/rss+xml; charset=utf-8");
        }
        catch (Exception)
        {
            return this.NotFound();
        }
    }
}