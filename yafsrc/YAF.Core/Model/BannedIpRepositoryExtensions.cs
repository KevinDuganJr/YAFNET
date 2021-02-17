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
namespace YAF.Core.Model
{
    using System;

    using YAF.Core.Extensions;
    using YAF.Types;
    using YAF.Types.Interfaces;
    using YAF.Types.Interfaces.Data;
    using YAF.Types.Models;

    /// <summary>
    ///     The banned IP repository extensions.
    /// </summary>
    public static class BannedIpRepositoryExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// The save.
        /// </summary>
        /// <param name="repository">
        /// The repository. 
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="mask">
        /// The mask. 
        /// </param>
        /// <param name="reason">
        /// The reason. 
        /// </param>
        /// <param name="userId">
        /// The user id. 
        /// </param>
        /// <param name="boardId">
        /// The board Id.
        /// </param>
        public static void Save(
            this IRepository<BannedIP> repository,
            int? id,
            string mask,
            string reason,
            int userId,
            int? boardId = null)
        {
            CodeContracts.VerifyNotNull(repository, "repository");

            if (id.HasValue)
            {
                repository.Upsert(
                    new BannedIP
                        {
                            BoardID = boardId ?? repository.BoardID,
                            ID = id.Value,
                            Mask = mask,
                            Reason = reason,
                            UserID = userId,
                            Since = DateTime.Now
                        });

                repository.FireUpdated(id.Value);
            }
            else
            {
                var banned = repository.GetSingle(b => b.BoardID == repository.BoardID && b.Mask == mask);

                if (banned == null)
                {
                    repository.Upsert(
                        new BannedIP
                            {
                                BoardID = boardId ?? repository.BoardID,
                                Mask = mask,
                                Reason = reason,
                                UserID = userId,
                                Since = DateTime.Now
                            });
                }
            }
        }

        #endregion
    }
}