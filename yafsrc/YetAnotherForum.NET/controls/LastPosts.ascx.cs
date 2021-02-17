/* Yet Another Forum.NET
 * Copyright (C) 2003-2005 Bjørnar Henden
 * Copyright (C) 2006-2013 Jaben Cargman
 * Copyright (C) 2014-2021 Ingo Herbote
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

namespace YAF.Controls
{
    #region Using

    using System;
    using System.Data;

    using YAF.Configuration;
    using YAF.Core.BaseControls;
    using YAF.Core.Model;
    using YAF.Types;
    using YAF.Types.Extensions;
    using YAF.Types.Interfaces;
    using YAF.Types.Models;
    using YAF.Utils.Helpers;

    #endregion

    /// <summary>
    /// The last posts.
    /// </summary>
    public partial class LastPosts : BaseUserControl
    {
        #region Properties

        /// <summary>
        ///   Gets or sets TopicID.
        /// </summary>
        public long? TopicID
        {
            get => this.ViewState["TopicID"]?.ToType<int>();

            set => this.ViewState["TopicID"] = value;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load([NotNull] object sender, [NotNull] EventArgs e)
        {
            this.BindData();
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        {
            if (this.TopicID.HasValue)
            {
                var showDeleted = false;
                var userId = 0;

                if (this.Get<BoardSettings>().ShowDeletedMessagesToAll)
                {
                    showDeleted = true;
                }

                if (!showDeleted && this.Get<BoardSettings>().ShowDeletedMessages && !this.Get<BoardSettings>().ShowDeletedMessagesToAll || this.PageContext.IsAdmin
                    || this.PageContext.IsForumModerator)
                {
                    userId = this.PageContext.PageUserID;
                }

                var dt = this.GetRepository<Message>().PostListAsDataTable(
                    this.TopicID,
                    this.PageContext.PageUserID,
                    userId,
                    0,
                    showDeleted,
                    false,
                    false,
                    DateTimeHelper.SqlDbMinTime(),
                    DateTime.UtcNow,
                    DateTimeHelper.SqlDbMinTime(),
                    DateTime.UtcNow,
                    0,
                    10,
                    2,
                    0,
                    0,
                    false,
                    -1);

                this.repLastPosts.DataSource = dt.AsEnumerable();
            }
            else
            {
                this.repLastPosts.DataSource = null;
            }

            this.repLastPosts.DataBind();
        }

        #endregion
    }
}