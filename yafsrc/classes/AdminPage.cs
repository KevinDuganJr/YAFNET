/* Yet Another Forum.net
 * Copyright (C) 2003 Bj�rnar Henden
 * http://www.yetanotherforum.net/
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 */

using System;

namespace yaf
{
	/// <summary>
	/// Summary description for AdminPage.
	/// </summary>
	public class AdminPage : BaseAdminPage
	{
		public AdminPage()
		{
			this.Load += new System.EventHandler(this.AdminPage_Load);
		}
	
		private void AdminPage_Load(object sender, System.EventArgs e)
		{
			if(!IsAdmin)
				Data.AccessDenied();
		}
	}
}
