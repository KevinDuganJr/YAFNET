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

namespace YAF.Pages.Admin
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.UI.WebControls;

    using YAF.Configuration;
   using YAF.Web;
    using YAF.Core;
    using YAF.Core.Extensions;
    using YAF.Core.Utilities;
    using YAF.Types;
    using YAF.Types.Constants;
    using YAF.Types.Extensions;
    using YAF.Types.Interfaces;
    using YAF.Types.Models;
    using YAF.Utils;
    using YAF.Web.Controls;

    #endregion

    /// <summary>
    /// The Admin Banned Names Page.
    /// </summary>
    public partial class bannedname : AdminPage
    {
        #region Methods

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load([NotNull] object sender, [NotNull] EventArgs e)
        {
            if (this.IsPostBack)
            {
                return;
            }

            this.BindData();
        }

        /// <summary>
        /// Creates page links for this page.
        /// </summary>
        protected override void CreatePageLinks()
        {
            this.PageLinks.AddRoot();
            this.PageLinks.AddLink(
                this.GetText("ADMIN_ADMIN", "Administration"),
                YafBuildLink.GetLink(ForumPages.admin_admin));

            this.PageLinks.AddLink(this.GetText("ADMIN_BANNEDNAME", "TITLE"), string.Empty);

            this.Page.Header.Title =
                $"{this.GetText("ADMIN_ADMIN", "Administration")} - {this.GetText("ADMIN_BANNEDNAME", "TITLE")}";
        }

        /// <summary>
        /// The list_ item command.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterCommandEventArgs"/> instance containing the event data.</param>
        protected void List_ItemCommand([NotNull] object sender, [NotNull] RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "add":
                    this.EditDialog.BindData(null);

                    YafContext.Current.PageElements.RegisterJsBlockStartup(
                        "openModalJs",
                        JavaScriptBlocks.OpenModalJs("EditDialog"));
                    break;
                case "edit":
                    this.EditDialog.BindData(e.CommandArgument.ToType<int>());

                    YafContext.Current.PageElements.RegisterJsBlockStartup(
                        "openModalJs",
                        JavaScriptBlocks.OpenModalJs("EditDialog"));
                    break;
                case "export":
                    {
                        var bannedNames = this.GetRepository<BannedName>().Get(x => x.BoardID == this.PageContext.PageBoardID);

                        this.Get<HttpResponseBase>().Clear();
                        this.Get<HttpResponseBase>().ClearContent();
                        this.Get<HttpResponseBase>().ClearHeaders();

                        this.Get<HttpResponseBase>().ContentType = "application/vnd.text";
                        this.Get<HttpResponseBase>()
                            .AppendHeader("content-disposition", "attachment; filename=BannedEmailsExport.txt");

                        var streamWriter = new StreamWriter(this.Get<HttpResponseBase>().OutputStream);

                        foreach (var name in bannedNames)
                        {
                            streamWriter.Write(name.Mask);
                            streamWriter.Write(streamWriter.NewLine);
                        }

                        streamWriter.Close();

                        this.Response.End();
                    }

                    break;
                case "delete":
                    {
                        this.GetRepository<BannedName>().DeleteById(e.CommandArgument.ToType<int>());

                        this.PageContext.AddLoadMessage(
                            this.GetText("ADMIN_BANNEDNAME", "MSG_REMOVEBAN_NAME"),
                            MessageTypes.success);

                        this.BindData();
                    }

                    break;
            }
        }

        /// <summary>
        /// The pager top_ page change.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void PagerTop_PageChange([NotNull] object sender, [NotNull] EventArgs e)
        {
            // rebind
            this.BindData();
        }

        /// <summary>
        /// Handles the Click event of the Search control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Search_Click(object sender, EventArgs e)
        {
            this.BindData();
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        {
            this.PagerTop.PageSize = this.Get<YafBoardSettings>().MemberListPageSize;

            var searchText = this.SearchInput.Text.Trim();

            List<BannedName> bannedList;

            if (searchText.IsSet())
            {
                bannedList = this.GetRepository<BannedName>().GetPaged(
                    x => x.BoardID == this.PageContext.PageBoardID && x.Mask == searchText,
                    this.PagerTop.CurrentPageIndex,
                    this.PagerTop.PageSize);
            }
            else
            {
                bannedList = this.GetRepository<BannedName>().GetPaged(
                    x => x.BoardID == this.PageContext.PageBoardID,
                    this.PagerTop.CurrentPageIndex,
                    this.PagerTop.PageSize);
            }

            this.list.DataSource = bannedList;

            this.PagerTop.Count = bannedList != null && bannedList.Any()
                                      ? this.GetRepository<BannedName>()
                                          .Count(x => x.BoardID == this.PageContext.PageBoardID).ToType<int>()
                                      : 0;

            this.DataBind();
        }

        #endregion
    }
}